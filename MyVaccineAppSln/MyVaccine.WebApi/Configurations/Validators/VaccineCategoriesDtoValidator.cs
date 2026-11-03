using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineCategoriesDtoValidator : AbstractValidator<VaccineCategoryRequestDTO>
{
    public VaccineCategoriesDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(int.MaxValue);
    }
}
