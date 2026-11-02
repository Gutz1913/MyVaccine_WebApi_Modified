using FluentValidation;
using MyVaccine.WebApi.DTOs.Request;

namespace MyVaccine.WebApi.Configurations.Validators;

public class UsersDtoValidator : AbstractValidator<UpdateUserRequestDTO>
{
    public UsersDtoValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(30);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(30);
    }
}
