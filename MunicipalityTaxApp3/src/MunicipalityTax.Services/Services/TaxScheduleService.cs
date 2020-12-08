namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Text;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.Repositories;
    using MunicipalityTax.Services.Helpers;

    public class TaxScheduleService : BaseService<TaxScheduleDto, TaxScheduleCreateDto, TaxSchedule>, ITaxScheduleService
    {
        private readonly IRepository<TaxSchedule> repository;
        private readonly IMapper mapper;

        public TaxScheduleService(IRepository<TaxSchedule> repository, IMapper mapper)
            : base(repository, mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public bool IsMunicipalityExist(Guid municipalityId)
        {
            var result = this.repository.ReadAll().FirstOrDefault(x => x.MunicipalityId == municipalityId);
            return result != null;
        }

        public IEnumerable<TaxScheduleDto> ReadAllByMunicipalityId(Guid municipalityId)
        {
            var entities = this.repository.ReadAll().Where(s => s.MunicipalityId == municipalityId);
            return this.mapper.Map<IEnumerable<TaxScheduleDto>>(source: entities);
        }

        public IEnumerable<ExpandoObject> ReadWithParameters(TaxScheduleRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entities = this.repository.ReadAll() as IQueryable<TaxSchedule>;

            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var id = Guid.Parse(request.Id);
                entities = entities.Where(m => m.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(request.ScheduleType))
            {
                Enum.TryParse(request.ScheduleType, out ScheduleType e);
                entities = entities.Where(m => m.ScheduleType == e);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                entities = entities.Where(x => x.Id.ToString().Contains(searchQuery)
                || x.ScheduleType.ToString().Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                var ordering = new StringBuilder();
                var orderByArray = request.OrderBy.Split(',');

                foreach (var req in orderByArray)
                {
                    var trimmedRequest = req.Trim();
                    var isOrderDescending = trimmedRequest.EndsWith(" desc");

                    var indexOfFirstSpace = trimmedRequest.IndexOf(" ");
                    var propertyName = indexOfFirstSpace == -1 ?
                        trimmedRequest : trimmedRequest.Remove(indexOfFirstSpace);

                    ordering
                        .Append(ordering.Length == 0 ? string.Empty : ", ")
                        .Append(propertyName)
                        .Append(isOrderDescending ? " descending" : " ascending");
                }

                entities = entities.OrderBy(ordering.ToString());
            }

            var page = entities.Page(request.PageNumber, request.PageSize);

            return this.mapper.Map<IEnumerable<TaxScheduleDto>>(source: page)
                .ReturnFieldsOnly(request.Fields);
        }
    }
}
