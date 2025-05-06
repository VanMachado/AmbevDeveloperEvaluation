using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;
using FluentValidation.Validators;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(s => s.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number must not exceed 50 characters.");

            RuleFor(s => s.UpdateDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(s => s.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(s => s.CustomerName)
                .NotEmpty()
                .MaximumLength(50).WithMessage("Customer name is required.");

            RuleFor(s => s.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be greater than or equal to zero.");

            RuleFor(s => s.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(s => s.BranchName)
                .NotEmpty()
                .MaximumLength(250).WithMessage("Branch name is required.");

            RuleFor(s => s.Items)
                .NotEmpty().WithMessage("Sale must contain at least one item.");

            RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());
        }
    }
}
