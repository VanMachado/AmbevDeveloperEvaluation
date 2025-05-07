﻿using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
    {
        private readonly ISalesRepository _saleRepository;

        public DeleteSaleHandler(ISalesRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _saleRepository.DeleteSaleAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"User with ID {command.Id} not found");

            return new DeleteSaleResponse { Success = true };
        }
    }
}
