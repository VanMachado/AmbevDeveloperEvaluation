using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesCommand, IEnumerable<GetSaleResult>>
    {
        private readonly ISalesRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetAllSalesHandler(ISalesRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetSaleResult>> Handle(GetAllSalesCommand request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllSalesAsync();
            return _mapper.Map<IEnumerable<GetSaleResult>>(sales);
        }
    }
}
