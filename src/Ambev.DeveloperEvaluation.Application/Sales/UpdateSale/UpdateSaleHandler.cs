using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISalesRepository _saleRepository;
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public UpdateSaleHandler(ISalesRepository saleRepository, IMapper mapper, IDiscountService discountService)
        {
            _saleRepository = saleRepository;
            _discountService = discountService;
            _mapper = mapper;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);            

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
                var updatedSale = await _saleRepository.UpdateSaleAsync(sale);
                
                Log.Information($"Sale {sale.Id} sucefully updated!");

                return _mapper.Map<UpdateSaleResult>(updatedSale);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Sale error while updating Sale {command.Id}");
                throw new DomainException($"Sale error while update Sale number {command.Id}");
            }
        }
    }
}
