using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover validation scenarios.
/// </summary>
public class SaleItemTests
{
    [Fact(DisplayName = "Validation should pass for valid SaleItem")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Produto A",
            Quantity = 2,
            UnitPrice = 10,
            Discount = 0.1m,
            TotalAmount = 18
        };

        // Act
        var result = item.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid SaleItem")]
    public void Given_InvalidSaleItem_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.Empty,
            ProductName = "",
            Quantity = 0,
            UnitPrice = 0,
            Discount = 1.5m,
            TotalAmount = -10
        };

        // Act
        var result = item.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
