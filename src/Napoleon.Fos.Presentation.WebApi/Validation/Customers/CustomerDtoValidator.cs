using FluentValidation;
using Napoleon.Fos.Contract.Customers;

namespace Napoleon.Fos.Presentation.WebApi.Validation.Customers;

public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(x => x.Age)
            .NotEmpty()
            .WithMessage("Age is Required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is Required")
            .MinimumLength(2)
            .WithMessage("Name is longer than 2 characters");
    }
}
