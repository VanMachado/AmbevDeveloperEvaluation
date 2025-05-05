using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Microsoft.AspNetCore.Components.Web;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a SaleItem in the system with profile information.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
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

        /// <summary>
        /// Performs validation of the SaleItem entity using the SaleItemValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">ProductId format and length</list>
        /// <list type="bullet">ProductName requirements</list>
        /// <list type="bullet">Quantity greater than 0 and less than 20</list>
        /// <list type="bullet">UnitPrice greater than 0</list>
        /// <list type="bullet">Discount validation rules</list>
        /// <list type="bullet">TotalAmount greater than 0</list>        
        /// </returns>
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
