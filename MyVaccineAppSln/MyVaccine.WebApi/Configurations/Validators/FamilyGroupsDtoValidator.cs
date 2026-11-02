using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class FamilyGroupsDtoValidator : AbstractValidator<FamilyGroupRequestDTO>
{
    public FamilyGroupsDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(30);
    }
}
