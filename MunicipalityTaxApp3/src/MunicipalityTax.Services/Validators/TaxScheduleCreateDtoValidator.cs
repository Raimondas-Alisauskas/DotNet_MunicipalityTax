namespace MunicipalityTax.Services.Validators
{
    using System;

    using FluentValidation;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

    public class TaxScheduleCreateDtoValidator : AbstractValidator<TaxScheduleCreateDto>
    {
        public TaxScheduleCreateDtoValidator()
        {
            this.RuleFor(x => x.ScheduleType).Must(p => p.GetType().IsEnum).WithMessage("Please provide correct schedule type");

            this.RuleFor(x => x.TaxStartDate).NotEmpty().WithMessage("Please provide valid Tax start date");
            this.RuleFor(x => x.TaxStartDate.DayOfWeek)
                .Equal(DayOfWeek.Monday)
                .When(x => x.ScheduleType == ScheduleType.Weekly)
                .WithMessage("Tax start date must be on Monday if Weekly schedule type is chosen");

            this.RuleFor(x => x.Tax).NotEmpty().ScalePrecision(1, 2).WithMessage("Please provide valid Tax value");

            this.RuleFor(x => x.MunicipalityId).NotEmpty().WithMessage("Please provide valid Id value");

            // Below are validations not for correct performance, but for data integrity.
            this.RuleFor(x => x.TaxStartDate.Day)
                .Equal(1)
                .When(x => x.ScheduleType == ScheduleType.Monthly || x.ScheduleType == ScheduleType.Yearly)
                .WithMessage("Tax start date must be first day of month if Monthly or Yearly schedule type is chosen");
            this.RuleFor(x => x.TaxStartDate.Month)
                .Equal(1)
                .When(x => x.ScheduleType == ScheduleType.Yearly)
                .WithMessage("Tax start date must be first month of year if Yearly schedule type is chosen");
        }
    }
}