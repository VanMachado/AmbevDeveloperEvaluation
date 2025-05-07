using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Provides reusable test data for CreateSaleHandler tests.
/// </summary>
public static class CreateSaleTestData
{
    /// <summary>
    /// Generates a valid CreateSaleCommand with mocked data.
    /// </summary>
    /// <returns>A fully populated and valid CreateSaleCommand.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return new CreateSaleCommand
        {
            SaleNumber = $"SALE-{Guid.NewGuid():N}",
            CustomerId = Guid.NewGuid(),
            CustomerName = "Cliente Teste",
            BranchId = Guid.NewGuid(),
            BranchName = "Filial Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "PROD001",
                    Quantity = 5,
                    UnitPrice = 10.0m,
                    Discount = 0.1m
                },
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "PROD002",
                    Quantity = 3,
                    UnitPrice = 20.0m,
                    Discount = 0.05m
                }
            }
        };
    }
}
