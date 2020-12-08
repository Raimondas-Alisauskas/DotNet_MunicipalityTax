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

    public class MunicipalityService : BaseService<MunicipalityDto, MunicipalityCreateDto, Municipality>, IMunicipalityService
    {
        private readonly IRepository<Municipality> repository;
        private readonly IMapper mapper;

        public MunicipalityService(IRepository<Municipality> repository, IMapper mapper)
            : base(repository, mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public IEnumerable<ExpandoObject> ReadWithParameters(MunicipalityRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entities = this.repository.ReadAll() as IQueryable<Municipality>;

            if (!string.IsNullOrWhiteSpace(request.MunicipalityName))
            {
                var municipalityName = request.MunicipalityName.Trim();
                entities = entities.Where(m => m.MunicipalityName == municipalityName);
            }

            if (!request.Id.Equals(Guid.Empty))
            {
                entities = entities.Where(m => m.Id == request.Id);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                entities = entities.Where(x => x.MunicipalityName.Contains(searchQuery)
                || x.Id.ToString().Contains(searchQuery));
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

            return this.mapper.Map<IEnumerable<MunicipalityDto>>(source: page)
                .ReturnFieldsOnly(request.Fields);
        }
    }
}