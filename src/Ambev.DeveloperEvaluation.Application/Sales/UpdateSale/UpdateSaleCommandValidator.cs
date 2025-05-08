using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(sale => sale.Id).NotEmpty();                       
            RuleFor(sale => sale.Items).NotEmpty();            

            //RuleForEach(sale => sale.Items).ChildRules(item =>
            //{
            //    item.RuleFor(i => i.ProductId)
            //        .NotEqual(Guid.Empty).WithMessage("ProductId is required");

            //    item.RuleFor(i => i.ProductName)
            //        .NotEmpty().WithMessage("ProductName is required");                

            //    item.RuleFor(i => i.UnitPrice)
            //        .GreaterThan(0).WithMessage("UnitPrice must be greater than zero");
            //});
        }
    }
}
