using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Validation
{
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Sale_Is_Valid()
        {
            var sale = CreateValidSale();
            var result = _validator.TestValidate(sale);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_SaleNumber_Is_Empty()
        {
            var sale = CreateValidSale();
            sale.SaleNumber = "";
            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.SaleNumber);
        }

        [Fact]
        public void Should_Have_Error_When_UpdatedDate_Is_In_The_Future()
        {
            var sale = CreateValidSale();
            sale.UpdatedDate = DateTime.UtcNow.AddDays(1);
            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.UpdatedDate);
        }

        [Fact]
        public void Should_Have_Error_When_Items_Are_Empty()
        {
            var sale = CreateValidSale();
            sale.Items.Clear();
            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.Items);
        }

        private static Sale CreateValidSale()
        {
            return new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = "12345",
                CreatedDate = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                UpdatedDate = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Main Branch",
                TotalAmount = 100,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product A",
                        Quantity = 2,
                        UnitPrice = 10,
                        Discount = 0.1m,
                        TotalAmount = 18
                    }
                }
            };
        }
    }    
}