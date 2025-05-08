using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

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

        /// <summary>
        /// Initialize a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The Sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The Logger instance</param>
        public CreateSaleHandler(ISalesRepository saleRepository,
            IMapper mapper,
            IDiscountService discountService)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _discountService = discountService;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (existingSale is not null)
                throw new InvalidOperationException($"Sale with SaleNumber {command.SaleNumber} and {command.Items.Count} items already exists");

            try
            {
                foreach (var item in command.Items)
                {
                    _discountService.ValidateQuantityRules(item.Quantity);
                    var discountUnitPrice = _discountService.CalculateDiscount(item.Quantity, item.UnitPrice, item);

                    item.TotalAmount = item.UnitPrice * (1 - item.Discount) * item.Quantity;
                }

                command.TotalAmount = command.Items.Sum(i => i.TotalAmount);

                var sale = _mapper.Map<Sale>(command);
                var createdSale = await _saleRepository.CreateAsync(sale);

                Log.Information($"Sale {sale.Id} sucefully created!");

                return _mapper.Map<CreateSaleResult>(createdSale);
            }
            catch (Exception ex)
            {
                Log.Error($"Sale error while creating Sale number {command.SaleNumber}");
                throw new DomainException($"Sale error while creating Sale number {command.SaleNumber}");
            }
        }
    }
}
