using FluentValidation;
using ShopService.Application.Dtos;

namespace ShopService.Application.Validation;

public class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
{
    public UpdateItemDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0m)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid category");
    }
}