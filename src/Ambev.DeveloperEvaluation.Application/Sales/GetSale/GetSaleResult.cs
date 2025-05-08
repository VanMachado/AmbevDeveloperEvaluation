using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents the response returned after successfully geting a sale.
    /// </summary>
    /// <remarks>
    /// This response contains a sale    
    /// </remarks>
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets SaleNumber on creation
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets SaleNumber on creation
        /// </summary>
        public string SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets CreatedDate on creation
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets UpdatedDate on creation
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets CustomerName on creation
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets CustomerName on creation
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets TotalAmount on creation
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets BranchId on creation
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets BranchName on creation
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Gets or sets Items on creation
        /// </summary>
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        /// <summary>
        /// Gets or sets IsCancelled on creation
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
