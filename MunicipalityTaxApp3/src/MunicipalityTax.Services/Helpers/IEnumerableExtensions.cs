namespace MunicipalityTax.Services.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

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
                var properties = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(properties);
            }
            else
            {
                var fieldsArr = fields.Split(',');

                foreach (var field in fieldsArr)
                {
                    var propertyName = field.Trim();

                    var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

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

        public static IEnumerable<T> OrderByProp<T>(this IEnumerable<T> enumerable, string propertyName)
        {
            if (enumerable.Any() && !string.IsNullOrEmpty(propertyName))
            {
                var propertyInfo = GetPropertyByName(enumerable.First(), propertyName);
                if (propertyInfo != null)
                {
                    return enumerable.OrderBy(x => propertyInfo.GetValue(x, null));
                }
            }

            return enumerable;
        }

        private static PropertyInfo GetPropertyByName<T>(T entity, string propertyName)
        {
            var propertyInfo = entity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return propertyInfo;
        }

        public static TaxSchedule MapToEntity(this TaxScheduleCreateDto dto)
        {
            var entity = new TaxSchedule();
            switch (dto.ScheduleType)
            {
                case ScheduleType.Daily:
                    entity.TaxEndDate = dto.TaxStartDate.Date;
                    break;
                case ScheduleType.Weekly:
                    entity.TaxEndDate = dto.TaxStartDate.AddDays(6).Date;
                    break;
                case ScheduleType.Monthly:
                    entity.TaxEndDate = dto.TaxStartDate.AddMonths(1).AddDays(-1).Date;
                    break;
                case ScheduleType.Yearly:
                    entity.TaxEndDate = dto.TaxStartDate.AddYears(1).AddDays(-1).Date;
                    break;
                default:
                    break;
            }

            entity.ScheduleType = dto.ScheduleType;
            entity.TaxStartDate = dto.TaxStartDate.Date;
            entity.Tax = dto.Tax;
            entity.MunicipalityId = dto.MunicipalityId;

            return entity;
        }

        public static IEnumerable<TaxSchedule> MapToEntities(this IEnumerable<TaxScheduleCreateDto> dtos)
        {
            var entities = new List<TaxSchedule>();

            foreach (var dto in dtos)
            {
                entities.Add(dto.MapToEntity());
            }

            return entities;
        }
    }
}
