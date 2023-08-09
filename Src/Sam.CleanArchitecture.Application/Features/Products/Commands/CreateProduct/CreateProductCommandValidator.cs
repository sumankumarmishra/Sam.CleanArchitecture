﻿using FluentValidation;
using Sam.CleanArchitecture.Application.Interfaces;

namespace Sam.CleanArchitecture.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(ITranslator translator)
        {

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .WithName(p => translator["Name"]);

            RuleFor(x => x.BarCode)
                .MaximumLength(50)
                .WithName(translator["BarCode"]);
        }
    }
}
