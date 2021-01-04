namespace MunicipalityTax.Services.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class FileHelper
    {
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static async Task<byte[]> ProcessFormFile(
            IFormFile formFile,
            ModelStateDictionary modelState,
            string[] permittedExtensions,
            long sizeLimit)
        {
            if (formFile == null)
            {
                modelState.AddModelError("File", $"Please provide the file.");

                return new byte[0];
            }

            var fileName = WebUtility.HtmlEncode(
                formFile.FileName);

            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, $"File({fileName}) is empty.");

                return new byte[0];
            }

            if (formFile.Length > sizeLimit)
            {
                var megabyteSizeLimit = sizeLimit / 1048576;
                modelState.AddModelError(formFile.Name, $"File({fileName}) exceeds {megabyteSizeLimit:N1} MB.");

                return new byte[0];
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name, $"File({fileName}) is empty.");
                    }

                    if (!IsValidFileExtensionAndSignature(
                        formFile.FileName, memoryStream, permittedExtensions))
                    {
                        modelState.AddModelError(
                            formFile.Name,
                            $"File({fileName}) type isn't permitted or the file's signature " +
                            "doesn't match the file's extension.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError(formFile.Name, $"File({fileName}) upload failed. Error: {ex.HResult}");

                // Log the exception
            }

            return new byte[0];
        }

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    // Limits characters to ASCII encoding.
                    for (var i = 0; i < data.Length; i++)
                    {
                        if (reader.ReadByte() > sbyte.MaxValue)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
    }
}
