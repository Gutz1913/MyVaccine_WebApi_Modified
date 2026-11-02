using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class DependentsDtoValidator : AbstractValidator<DependentRequestDTO>
{
    public DependentsDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.BirthDate).NotEmpty();
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.FamilyGroupId).NotEmpty().GreaterThan(0);
    }
}
