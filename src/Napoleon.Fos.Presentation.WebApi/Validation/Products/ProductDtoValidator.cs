using FluentValidation;
using Napoleon.Fos.Contract.Products;

namespace Napoleon.Fos.Presentation.WebApi.Validation.Products;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("ImageUrl is Required")
            .MinimumLength(2)
            .WithMessage("ImageUrl is longer than 2 characters")
            .MaximumLength(1024)
            .WithMessage("ImageUrl is less than 1024 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is Required")
            .MinimumLength(2)
            .WithMessage("Description is longer than 2 characters")
            .MaximumLength(1024)
            .WithMessage("ImageUrl is less than 1024 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is Required")
            .MinimumLength(2)
            .WithMessage("Name is longer than 2 characters")
            .MaximumLength(128)
            .WithMessage("Name is less than 128 characters");
    }
}