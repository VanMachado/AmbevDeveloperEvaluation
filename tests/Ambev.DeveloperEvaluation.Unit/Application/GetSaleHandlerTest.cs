using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleHandlerTests
    {
        private readonly Mock<ISalesRepository> _mockSaleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISalesRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSaleResult()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                SaleNumber = "SALE-12345",
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch1"
            };

            var saleResult = new GetSaleResult
            {
                SaleNumber = "SALE-12345",
                CustomerName = "John Doe",
                BranchName = "Branch1"
            };

            var command = new GetSaleCommand(saleId);

            _mockSaleRepository
                .Setup(repo => repo.GetByIdAsync(command.Id, CancellationToken.None))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<GetSaleResult>(sale))
                .Returns(saleResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(saleResult.SaleNumber, result.SaleNumber);
            Assert.Equal(saleResult.CustomerName, result.CustomerName);
            Assert.Equal(saleResult.BranchName, result.BranchName);
        }

        [Fact]
        public async Task Handle_SaleNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new GetSaleCommand(saleId);

            _mockSaleRepository
                .Setup(repo => repo.GetByIdAsync(saleId, CancellationToken.None))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new GetSaleCommand(Guid.Empty);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Id", exception.Message); 
        }
    }
}
