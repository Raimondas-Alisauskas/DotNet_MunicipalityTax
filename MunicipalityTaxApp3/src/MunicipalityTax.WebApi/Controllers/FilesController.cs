namespace MunicipalityTax.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using MunicipalityTax.Services.Helpers;
    using MunicipalityTax.Services.Services;
    using MunicipalityTax.Services.Validators;

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FilesController : Controller
    {
        private readonly IFileService service;
        private readonly long fileSizeLimit;
        private readonly string[] permittedExtensions = { ".csv" };

        public FilesController(IFileService service, IConfiguration config)
        {
            this.service = service;
            this.fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        /// <summary>Imports tax schedule data from file and add it to database.</summary>
        /// <remarks>
        /// File have to be provided of "comma-separated values" format (csv).
        /// File example:
        ///
        ///     7ebced2b-e2f9-45e0-bf75-111111111100;Monthly;2016-04-01;0,6
        ///
        /// where:
        ///
        ///     7ebced2b-e2f9-45e0-bf75-111111111100 - municipality id,
        ///     Monthly - schedule type,
        ///     2016-04-01 - schedule start date,
        ///     0,6 - tax rate.
        ///
        /// </remarks>
        /// <param name="formFile">File with data to import.</param>
        /// <returns>Created or error response.</returns>
        /// <response code="201">Returns if data sucessfuly added to database.</response>
        /// <response code="400">If data provided is wrong.</response>
        // Disable the form value model binding to take control of handling potentially large files.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadToDatabase(IFormFile formFile)
        {
            if (!FileHelper.IsMultipartContentType(this.Request.ContentType))
            {
                this.ModelState.AddModelError("File", "Wrong content type. The request couldn't be processed.");

                // Log error
                return this.BadRequest(this.ModelState);
            }

            var fileContent = await FileHelper.ProcessFormFile(
                formFile,
                this.ModelState,
                this.permittedExtensions,
                this.fileSizeLimit);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var taxScheduleCteateDtos = this.service.GetTaxScheduleCreateDtos(fileContent);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var taxScheduleCreateDtoValidator = new TaxScheduleCreateDtoValidator();
            foreach (var item in taxScheduleCteateDtos)
            {
                var valResult = taxScheduleCreateDtoValidator.Validate(item);

                if (!valResult.IsValid)
                {
                    return this.BadRequest(valResult.Errors);
                }

                this.service.Create(item);
            }

            return this.Created(nameof(FilesController), null);
        }
    }
}
