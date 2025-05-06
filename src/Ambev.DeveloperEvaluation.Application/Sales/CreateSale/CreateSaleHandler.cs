using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand request
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISalesRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IDiscountService _discountService;
        private readonly ILogger<CreateSaleHandler> _logger;

        /// <summary>
        /// Initialize a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The Sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The Logger instance</param>
        public CreateSaleHandler(ISalesRepository saleRepository, 
            IMapper mapper,
            IDiscountService discountService,
            ILogger<CreateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _discountService = discountService;
            _logger = logger;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            //if (existingSale is not null)
            //    throw new InvalidOperationException($"Sale with Id {command.Id} already exists");

            foreach(var item in command.Items)
            {
                _discountService.ValidateQuantityRules(item.Quantity);
                var discountUnitPrice = _discountService.CalculateDiscount(item.Quantity, item.UnitPrice);

                item.UnitPrice = discountUnitPrice;
                item.TotalAmount = item.UnitPrice * item.Quantity;                
            }

            command.TotalAmount = command.Items.Sum(i => i.TotalAmount);

            var sale = _mapper.Map<Sale>(command);
            var createdSale = await _saleRepository.CreateAsync(sale);

            return _mapper.Map<CreateSaleResult>(createdSale);
        }
    }
}
