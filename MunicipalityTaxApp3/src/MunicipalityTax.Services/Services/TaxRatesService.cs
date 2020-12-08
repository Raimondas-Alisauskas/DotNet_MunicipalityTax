namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.DbContexts;
    using MunicipalityTax.Persistence.Repositories;

    public class TaxRatesService : ITaxRatesService
    {
        private readonly IRepository<TaxSchedule> tRepo;
        private readonly IRepository<Municipality> mRepo;
        private readonly IMapper mapper;

        public TaxRatesService(MtxDbContext mtxDbContext, IMapper mapper)
        {
            this.tRepo = new Repository<TaxSchedule>(mtxDbContext);
            this.mRepo = new Repository<Municipality>(mtxDbContext);
            this.mapper = mapper;
        }

        public IEnumerable<TaxRateDto> ReadMunicipalTaxRatesAtGivenDay(TaxRateRequest request)
        {
            var municipality = this.mRepo.ReadAll().FirstOrDefault(m => m.MunicipalityName == request.MunicipalityName);

            if (municipality is null)
            {
                return Enumerable.Empty<TaxRateDto>();
            }

            var municipalityId = municipality.Id;

            var yearRequested = this.tRepo.ReadAll().Where(t => t.MunicipalityId == municipalityId
            && (t.TaxStartDate.Year == request.Year
                || (t.TaxStartDate.Year == request.Year - 1 && t.TaxStartDate.Month == 12)));

            if (!yearRequested.Any())
            {
                return Enumerable.Empty<TaxRateDto>();
            }

            var requestDate = new DateTime(request.Year, request.Month, request.Day);

            var daily = this.GetDailyEntities(requestDate, yearRequested);

            if (daily.Any())
            {
                return this.mapper.Map<IEnumerable<TaxRateDto>>(daily);
            }

            var weekly = this.GetWeeklyEntities(requestDate, yearRequested);

            if (weekly.Any())
            {
                return this.mapper.Map<IEnumerable<TaxRateDto>>(weekly);
            }

            var monthly = this.GetMonthlyEntities(request, yearRequested);

            if (monthly.Any())
            {
                return this.mapper.Map<IEnumerable<TaxRateDto>>(monthly);
            }

            var yearly = this.GetYearlyEntities(request, yearRequested);

            if (yearly.Any())
            {
                return this.mapper.Map<IEnumerable<TaxRateDto>>(yearly);
            }

            return Enumerable.Empty<TaxRateDto>();
        }

        private IEnumerable<TaxSchedule> GetDailyEntities(DateTime requestDate, IEnumerable<TaxSchedule> entities)
        {
            return entities.Where(t => t.ScheduleType == ScheduleType.Daily
                && requestDate == t.TaxStartDate);
        }

        private IEnumerable<TaxSchedule> GetWeeklyEntities(DateTime requestDate, IEnumerable<TaxSchedule> entities)
        {
            return entities.Where(t => t.ScheduleType == ScheduleType.Weekly
                && t.TaxStartDate <= requestDate
                && t.TaxStartDate >= requestDate.AddDays(-6));
        }

        private IEnumerable<TaxSchedule> GetMonthlyEntities(TaxRateRequest request, IEnumerable<TaxSchedule> entities)
        {
            return entities.Where(t => t.ScheduleType == ScheduleType.Monthly
                && request.Month == t.TaxStartDate.Month
                && request.Year == t.TaxStartDate.Year);
        }

        private IEnumerable<TaxSchedule> GetYearlyEntities(TaxRateRequest request, IEnumerable<TaxSchedule> entities)
        {
            return entities.Where(t => t.ScheduleType == ScheduleType.Yearly
                && request.Year == t.TaxStartDate.Year);
        }
    }
}
