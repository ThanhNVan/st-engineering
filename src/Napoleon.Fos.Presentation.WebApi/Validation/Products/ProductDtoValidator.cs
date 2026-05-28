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

        RuleFor(x => x.ProductDetails)
            .NotEmpty()
            .WithMessage("Mus have Product Detail");
        
        RuleFor(x => x.ProductDetails.Select(x => x.Price <= 0))
            .NotEmpty()
            .WithMessage("Value must be non-negative");

        RuleFor(x => x.ProductDetails.Select(x => x.Varience.Length <= 2 && x.Varience.Length > 128))
            .NotEmpty()
            .WithMessage("Name is longer than 2 characters and  less than 128 characters");
        
        RuleFor(x => x.ProductDetails.Select(x => x.ImageUrl.Length <= 0))
            .NotEmpty()
            .WithMessage("ImageUrl is Required");
        
        RuleFor(x => x.ProductDetails.Select(x => x.Description.Length <= 0))
            .NotEmpty()
            .WithMessage("Description is Required");
    }
}