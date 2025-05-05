using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        public SaleItemValidator()
        {
            RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 units of the same product.");

            RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(i => i.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
                .LessThanOrEqualTo(1).WithMessage("Discount must be a fraction (e.g., 0.10 for 10%).");

            RuleFor(i => i.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Item total amount must be greater than or equal to zero.");
        }
    }
}
