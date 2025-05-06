using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Microsoft.AspNetCore.Identity;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale in the system with profile information.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class Sale : BaseEntity
    {        
        public string SaleNumber { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        public bool IsCancelled { get; set; }

        public Sale()
        {
            CreateDate = DateTime.Now;
        }

        /// <summary>
        /// Performs validation of the Sale entity using the SaleValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">SaleNumber format and length</list>
        /// <list type="bullet">Date format</list>
        /// <list type="bullet">CustomerId requirements</list>
        /// <list type="bullet">CustomerName requirements</list>
        /// <list type="bullet">BranchId requirements</list>
        /// <list type="bullet">BranchName requirements</list>
        /// <list type="bullet">If Items contains at last one item</list>
        /// </returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }      
        
        public decimal TotalAmountPurchase()
        {            
            return Items.Sum(item => item.TotalAmount);
        }

        public Dictionary<string, decimal> TotalAmountItem()
        {
            var totalAmount = new Dictionary<string, decimal>();

            foreach (var item in Items)
            {
                if (!totalAmount.TryAdd(item.ProductName, item.TotalAmount))
                    totalAmount[item.ProductName] += item.TotalAmount;
            }

            return totalAmount;
        }
    }
}
