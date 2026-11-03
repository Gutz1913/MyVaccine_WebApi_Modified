using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineRecordsDtoValidator : AbstractValidator<VaccineRecordRequestDTO>
{
    public VaccineRecordsDtoValidator()
    {
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.DependentId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.VaccineId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.AdministeredDate).NotEmpty();
        RuleFor(dto => dto.AdministeredLocation).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.AdministeredBy).NotEmpty().MaximumLength(100);
    }
}
