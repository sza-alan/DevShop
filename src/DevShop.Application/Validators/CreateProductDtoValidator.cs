using DevShop.Application.DTOs;
using FluentValidation;

namespace DevShop.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome do produto deve ter no mínimo 3 caracteres.");

            RuleFor(dto => dto.Price)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.");

            RuleFor(dto => dto.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O estoque não pode ser negativo.");
        }
    }
}
