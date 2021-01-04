namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.Repositories;

    public class FileService : BaseService<TaxScheduleDto, TaxScheduleCreateDto, TaxSchedule>, IFileService
    {
        private readonly IMapper mapper;

        public FileService(IRepository<TaxSchedule> repository, IMapper mapper)
            : base(repository, mapper)
        {
            this.mapper = mapper;
        }

        public IEnumerable<TaxScheduleCreateDto> GetTaxScheduleCreateDtos(byte[] file)
        {
            var str = System.Text.Encoding.Default.GetString(file);

            var arr = str.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var taxScheduleList = new List<TaxScheduleCreateFileDto>();

            foreach (var inst in arr)
            {
                var parameters = inst.Split(';');

                var taxSchedule = (TaxScheduleCreateFileDto)Activator.CreateInstance(typeof(TaxScheduleCreateFileDto), parameters);

                taxScheduleList.Add(taxSchedule);
            }

            return this.mapper.Map<IEnumerable<TaxScheduleCreateDto>>(source: taxScheduleList);
        }
    }
}
