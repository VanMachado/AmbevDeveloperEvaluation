using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator = new();

    [Fact]
    public void Should_Pass_When_SaleItem_Is_Valid()
    {
        var item = CreateValidItem();
        var result = _validator.TestValidate(item);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Quantity_Is_Invalid(int quantity)
    {
        var item = CreateValidItem();
        item.Quantity = quantity;
        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(i => i.Quantity);
    }

    private static SaleItem CreateValidItem()
    {
        return new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            Quantity = 5,
            UnitPrice = 10,
            Discount = 0.1m,
            TotalAmount = 45
        };
    }
}