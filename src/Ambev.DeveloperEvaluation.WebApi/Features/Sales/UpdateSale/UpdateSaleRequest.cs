using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Represents a request to update a sale in the system
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// Gets or sets SaleNumber on creation
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets SaleNumber on creation
        /// </summary>
        public string SaleNumber { get; set; }

        public DateTime CreatedDate { get; set; }

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
        public List<UpdateSaleitemRequest> Items { get; set; } = new List<UpdateSaleitemRequest>();

        /// <summary>
        /// Gets or sets IsCancelled on creation
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
