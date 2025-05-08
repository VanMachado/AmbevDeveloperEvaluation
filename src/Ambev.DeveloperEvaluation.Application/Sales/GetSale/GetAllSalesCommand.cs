using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetAllSalesCommand : IRequest<IEnumerable<GetSaleResult>>
    {
    }
}
