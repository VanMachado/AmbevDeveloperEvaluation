using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {    
        public CreateSaleRequestValidator()
        {
            RuleFor(sale => sale.SaleNumber).NotEmpty().MaximumLength(50);
            RuleFor(sale => sale.CustomerId).NotEqual(Guid.Empty);
            RuleFor(sale => sale.CustomerName).NotEmpty().MaximumLength(50);
            RuleFor(sale => sale.TotalAmount).GreaterThanOrEqualTo(0);
            RuleFor(sale => sale.BranchId).NotEmpty().Equals(typeof(Guid));
            RuleFor(sale => sale.BranchName).NotEmpty().MaximumLength(250);
            RuleFor(sale => sale.Items).NotEmpty();
            RuleFor(sale => sale.IsCancelled).Equal(false);

            RuleForEach(sale => sale.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEqual(Guid.Empty).WithMessage("ProductId is required");

                item.RuleFor(i => i.ProductName)
                    .NotEmpty().WithMessage("ProductName is required");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .LessThan(20)
                    .WithMessage("Quantity must be greater than zero and less than 20");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("UnitPrice must be greater than zero");
            });
        }
    }
}
