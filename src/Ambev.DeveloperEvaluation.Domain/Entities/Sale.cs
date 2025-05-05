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
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }

        public Sale(Guid id, 
            string saleNumber, 
            DateTime date, 
            Guid customerId, 
            string customerName, 
            Guid branchId, 
            string branchName, 
            List<SaleItem> items, 
            decimal totalAmount, 
            bool isCancelled)
        {
            Id = id;
            SaleNumber = saleNumber;
            Date = date;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            Items = items;
            TotalAmount = totalAmount;
            IsCancelled = isCancelled;
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
    }
}
