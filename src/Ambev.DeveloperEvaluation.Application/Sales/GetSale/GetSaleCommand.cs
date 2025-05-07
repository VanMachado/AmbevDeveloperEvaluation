using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Command for geting a sale.
    /// </summary>
    /// <remarks>
    /// This command captures the necessary data to get a sale,     
    /// each with its name, quantity, and unit price.
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="GetSaleResult"/>.    
    /// </remarks>
    public class GetSaleCommand : IRequest<GetSaleResult>
    {
        /// <summary>
        /// Gets or sets SaleNumber on creation
        /// </summary>
        public Guid Id { get; set; }

        public GetSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
