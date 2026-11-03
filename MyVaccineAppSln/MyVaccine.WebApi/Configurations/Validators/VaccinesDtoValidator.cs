using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccinesDtoValidator : AbstractValidator<VaccineRequestDTO>
{
    public VaccinesDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
    }
}