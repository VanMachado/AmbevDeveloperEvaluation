using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISalesRepository _saleRepository;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISalesRepository salesRepository, IMapper mapper)
        {
            _saleRepository = salesRepository;
            _mapper = mapper;
        }

        public Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
