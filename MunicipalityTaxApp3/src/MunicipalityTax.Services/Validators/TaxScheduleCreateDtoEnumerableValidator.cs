namespace MunicipalityTax.Services.Validators
{
    using System.Collections.Generic;
    using FluentValidation;
    using MunicipalityTax.Contracts.In;

    public class TaxScheduleCreateDtoEnumerableValidator : AbstractValidator<List<TaxScheduleCreateDto>>
    {
        public TaxScheduleCreateDtoEnumerableValidator()
        {
            this.RuleForEach(taxSchedule => taxSchedule).SetValidator(new TaxScheduleCreateDtoValidator());
        }
    }
}
