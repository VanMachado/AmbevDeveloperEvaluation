using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// </summary>
    /// <remarks>
    /// This command captures the necessary data to create a sale, 
    /// including customer information and a list of sale items, 
    /// each with its name, quantity, and unit price.
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="CreateSaleResult"/>.
    /// 
    /// The data provided in this command is validated 
    /// using the <see cref="CreateSaleValidator"/>, which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and adhere to the required business rules.
    /// </remarks>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
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

        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(this);

            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
