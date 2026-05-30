using FluentValidation;
using Napoleon.Fos.Contract.Carts;

namespace Napoleon.Fos.Presentation.WebApi.Validation.Carts;

public class CartDtoValidator : AbstractValidator<AddCartDto>
{
    public CartDtoValidator()
    {
        RuleFor(x => x.ProductDetailId)
            .NotEmpty()
            .WithMessage("ProductDetailId is Required");
    }
}
