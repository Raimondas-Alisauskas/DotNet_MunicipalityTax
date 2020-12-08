namespace MunicipalityTax.Services.Mappers
{
    using System;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;
    using MunicipalityTax.Domain.Entities;

    public class AutomapProfile : Profile
    {
        public AutomapProfile()
        {
            this.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));

            this.CreateMap<TaxSchedule, TaxRateDto>();

            this.CreateMap<Municipality, MunicipalityDto>();
            this.CreateMap<Municipality, MunicipalityCreateDto>();

            this.CreateMap<MunicipalityDto, Municipality>();
            this.CreateMap<MunicipalityCreateDto, Municipality>();

            this.CreateMap<TaxSchedule, TaxScheduleDto>();
            this.CreateMap<TaxSchedule, TaxScheduleCreateDto>();

            this.CreateMap<TaxScheduleDto, TaxSchedule>();
            this.CreateMap<TaxScheduleCreateDto, TaxSchedule>();
        }
    }
}
