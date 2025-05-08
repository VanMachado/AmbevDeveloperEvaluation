using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface IDiscountService
    {
        decimal CalculateDiscount(int quantity, decimal unitPrice, SaleItem saleItem);
        void ValidateQuantityRules(int quantity);
    }
}
