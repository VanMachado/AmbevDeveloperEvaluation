using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly Mock<ISalesRepository> _mockSalesRepository;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _mockSalesRepository = new Mock<ISalesRepository>();
            _handler = new DeleteSaleHandler(_mockSalesRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesSaleSuccessfully()
        {
            // Arrange
            var command = new DeleteSaleCommand(Guid.NewGuid());

            _mockSalesRepository.Setup(repo => repo.DeleteSaleAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            _mockSalesRepository.Verify(repo => repo.DeleteSaleAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_SaleNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = new DeleteSaleCommand(Guid.NewGuid());

            _mockSalesRepository.Setup(repo => repo.DeleteSaleAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Sale with ID {command.Id} not found", exception.Message);
            _mockSalesRepository.Verify(repo => repo.DeleteSaleAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange:
            var command = new DeleteSaleCommand(Guid.Empty);            

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.NotEmpty(exception.Errors);
            _mockSalesRepository.Verify(repo => repo.DeleteSaleAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
