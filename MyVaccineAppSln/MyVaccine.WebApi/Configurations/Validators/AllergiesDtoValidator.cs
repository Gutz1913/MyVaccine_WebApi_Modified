using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class AllergiesDtoValidator : AbstractValidator<AllergyRequestDTO>
{
    public AllergiesDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
    }
}
