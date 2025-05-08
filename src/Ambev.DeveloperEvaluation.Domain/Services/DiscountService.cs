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
            {
                saleItem.Discount = 0.2m;
                return unitPrice * 0.8m;
            }

            if (quantity >= 4)
            {
                saleItem.Discount = 0.1m;
                return unitPrice * 0.9m;
            }
            
            return unitPrice;
        }

        public void ValidateQuantityRules(int quantity)
        {
            if (quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");

            if (quantity < 4 && HasDiscount(quantity))
                throw new InvalidOperationException("Discounts are not allowed for quantities below 4 items.");
        }

        private bool HasDiscount(int quantity)
        {
            return quantity >= 4;
        }
    }
}
