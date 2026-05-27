using FluentValidation;
using Napoleon.Fos.Contract.Customers;

namespace Napoleon.Fos.Presentation.WebApi.Validation.Customers;

public class CustomerAuthDtoValidator : AbstractValidator<CustomerAuthDto>
{
    public CustomerAuthDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is Required")
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Name is Required")
            .MinimumLength(6)
            .WithMessage("Name is longer than 6 characters");
    }
}
