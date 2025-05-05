using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Microsoft.AspNetCore.Components.Web;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int TotalAmount { get; set; }
        public bool IsCancelled { get; set; }

        public SaleItem(Guid productId, 
            string productName, 
            int quantity, 
            decimal unitPrice, 
            decimal discount, 
            int total, 
            bool isCancelled)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            this.UnitPrice = unitPrice;
            Discount = discount;
            TotalAmount = total;
            IsCancelled = isCancelled;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);

            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
