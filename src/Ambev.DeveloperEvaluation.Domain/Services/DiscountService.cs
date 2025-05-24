using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class DiscountService : IDiscountService
    {
        public decimal CalculateDiscount(int quantity, decimal unitPrice, SaleItem saleItem)
        {
            if (quantity >= 10 && quantity <= 20)
                return unitPrice * 0.8m;

            if (quantity >= 4)
                return unitPrice * 0.9m;

            return unitPrice;
        }

        public void ValidateQuantityRules(int quantity)
        {
            if (quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");

            if (quantity < 4 && HasDiscount(quantity))
                throw new InvalidOperationException("Discounts are not allowed for quantities below 4 items.");
        }

        public decimal ValueOfDiscount(SaleItem saleItem)
        {
            if (saleItem.Quantity >= 10 && saleItem.Quantity <= 20)            
                return 0.2m;                          

            if (saleItem.Quantity >= 4 && saleItem.Quantity < 10)
                return 0.1m;

            return 0.0m;
        }

        private bool HasDiscount(int quantity)
        {
            return quantity >= 4;
        }

    }
}
