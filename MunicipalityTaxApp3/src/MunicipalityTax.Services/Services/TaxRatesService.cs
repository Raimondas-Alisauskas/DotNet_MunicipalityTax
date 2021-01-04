namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.Repositories;
    using MunicipalityTax.Services.Helpers;

    public class TaxRatesService : ITaxRatesService
    {
        private readonly IRepository<TaxSchedule> repo;
        private readonly IMapper mapper;

        public TaxRatesService(IRepository<TaxSchedule> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public IEnumerable<TaxRateDto> ReadMunicipalTaxRatesAtGivenDay(Guid municipalityId, TaxRateRequest request)
        {
            var requestDate = request.Date;

            var allPossibleDates = this.repo.ReadAll().Where(x => x.MunicipalityId == municipalityId
            && x.TaxStartDate <= requestDate && x.TaxEndDate >= requestDate)
                .OrderBy(x => x, new ScheduleTypeComparer())
                .ToList();

            if (!allPossibleDates.Any())
            {
                return Enumerable.Empty<TaxRateDto>();
            }

            // In case if different taxes will be inserted for the same date.
            var firstScheduleType = allPossibleDates.First().ScheduleType;
            return this.mapper.Map<IEnumerable<TaxRateDto>>(allPossibleDates.Where(x => x.ScheduleType == firstScheduleType));
        }
    }
}
