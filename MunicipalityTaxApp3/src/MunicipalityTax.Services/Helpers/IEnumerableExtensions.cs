namespace MunicipalityTax.Services.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ReturnFieldsOnly<T>(this IEnumerable<T> sourceList, string fields)
        {
            if (sourceList == null)
            {
                throw new ArgumentNullException(nameof(sourceList));
            }

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var properties = typeof(T)
                        .GetProperties(BindingFlags.IgnoreCase
                        | BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(properties);
            }
            else
            {
                var fieldsArr = fields.Split(',');

                foreach (var field in fieldsArr)
                {
                    var propertyName = field.Trim();

                    var property = typeof(T).GetProperty(
                        propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null)
                    {
                        throw new ArgumentNullException($"Property {propertyName} wasn't found on {typeof(T)}");
                    }

                    propertyInfoList.Add(property);
                }
            }

            var expandoObjectList = new List<ExpandoObject>();

            foreach (T sourceObject in sourceList)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
