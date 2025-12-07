using FluentValidation;
using ShopService.Application.Dtos;

namespace ShopService.Application.Validation;

public class CreateItemDtoValidator : AbstractValidator<CreateItemDto>
{
    public CreateItemDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0m)
            .WithMessage("Price must be greater than zero");

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid category");
    }
}