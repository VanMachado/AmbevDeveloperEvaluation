using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover validation and calculation scenarios.
/// </summary>
public class SaleTests
{
    [Fact(DisplayName = "Validation should pass for valid Sale")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE123",
            CustomerId = Guid.NewGuid(),
            CustomerName = "Cliente Válido",
            BranchId = Guid.NewGuid(),
            BranchName = "Filial Teste",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Produto A",
                    Quantity = 2,
                    UnitPrice = 10,
                    Discount = 0.1m,
                    TotalAmount = 18
                }
            },
            TotalAmount = 18
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid Sale")]
    public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "",
            CustomerId = Guid.Empty,
            CustomerName = "",
            BranchId = Guid.Empty,
            BranchName = "",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow.AddDays(1),
            Items = new List<SaleItem>(),
            TotalAmount = -5
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Should return total amount of all sale items")]
    public void Given_SaleWithItems_When_TotalAmountPurchase_Called_Then_ShouldReturnCorrectTotal()
    {
        // Arrange
        var sale = new Sale
        {
            Items = new List<SaleItem>
            {
                new SaleItem { TotalAmount = 10 },
                new SaleItem { TotalAmount = 20 }
            }
        };

        // Act
        var total = sale.TotalAmountPurchase();

        // Assert
        Assert.Equal(30, total);
    }

    [Fact(DisplayName = "Should return total amount grouped by product name")]
    public void Given_SaleWithDuplicateItems_When_TotalAmountItem_Called_Then_ShouldAggregateByProductName()
    {
        // Arrange
        var sale = new Sale
        {
            Items = new List<SaleItem>
            {
                new SaleItem { ProductName = "Produto A", TotalAmount = 10 },
                new SaleItem { ProductName = "Produto A", TotalAmount = 5 },
                new SaleItem { ProductName = "Produto B", TotalAmount = 7 }
            }
        };

        // Act
        var result = sale.TotalAmountItem();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(15, result["Produto A"]);
        Assert.Equal(7, result["Produto B"]);
    }
}
