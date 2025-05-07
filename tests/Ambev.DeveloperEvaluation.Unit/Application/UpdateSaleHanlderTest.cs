using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateSaleHandlerTests
    {
        private readonly Mock<ISalesRepository> _mockSalesRepository;
        private readonly Mock<IDiscountService> _mockDiscountService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _mockSalesRepository = new Mock<ISalesRepository>();
            _mockDiscountService = new Mock<IDiscountService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateSaleHandler(_mockSalesRepository.Object, _mockMapper.Object, _mockDiscountService.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSale()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                TotalAmount = 0,
                Items = new List<SaleItem>
        {
            new SaleItem { ProductName = "PROD001", Quantity = 5, UnitPrice = 10.0m, Discount = 0.1m },
            new SaleItem { ProductName = "PROD002", Quantity = 3, UnitPrice = 20.0m, Discount = 0.05m }
        }
            };
                        
            var expectedTotalAmount = command.Items.Sum(item => item.Quantity * item.UnitPrice * (1 - item.Discount));
                        
            var sale = new Sale
            {
                Id = command.Id,
                Items = command.Items.Select(i => new SaleItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalAmount = i.Quantity * i.UnitPrice * (1 - i.Discount)
                }).ToList()
            };

            
            _mockDiscountService.Setup(ds => ds.ValidateQuantityRules(It.IsAny<int>())).Verifiable();
            _mockDiscountService.Setup(ds => ds.CalculateDiscount(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<SaleItem>()))
                .Returns((int quantity, decimal unitPrice, SaleItem item) => unitPrice * (1 - item.Discount));

            _mockSalesRepository.Setup(repo => repo.UpdateSaleAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);
            _mockMapper.Setup(m => m.Map<Sale>(It.IsAny<UpdateSaleCommand>())).Returns(sale);
            _mockMapper.Setup(m => m.Map<UpdateSaleResult>(It.IsAny<Sale>()))
                .Returns(new UpdateSaleResult
                {
                    Id = sale.Id,
                    TotalAmount = sale.Items.Sum(i => i.TotalAmount) 
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(expectedTotalAmount, result.TotalAmount);

            
            _mockSalesRepository.Verify(repo => repo.UpdateSaleAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Sale>(It.IsAny<UpdateSaleCommand>()), Times.Once);
            _mockMapper.Verify(m => m.Map<UpdateSaleResult>(It.IsAny<Sale>()), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new UpdateSaleCommand(); // Invalid command with missing data (e.g., items)

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_SaleNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(), // ID that does not exist in the repository
                Items = new List<SaleItem>
                {
                    new SaleItem { ProductName = "PROD001", Quantity = 5, UnitPrice = 10.0m, Discount = 0.1m }
                }
            };

            _mockSalesRepository.Setup(repo => repo.UpdateSaleAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ThrowsAsync(new KeyNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GenericException_ThrowsDomainException()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                Items = new List<SaleItem>
                {
                    new SaleItem { ProductName = "PROD001", Quantity = 5, UnitPrice = 10.0m, Discount = 0.1m }
                }
            };

            _mockSalesRepository.Setup(repo => repo.UpdateSaleAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Sale error while update Sale number {command.Id}", exception.Message);
        }
    }
}
