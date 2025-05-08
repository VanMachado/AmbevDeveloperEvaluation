using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISalesRepository
    {
        private readonly SalesContext _context;

        /// <summary>
        /// Initializes a new instance of SaleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var item in sale.Items)
                {
                    await _context.SaleItems.AddAsync(item, cancellationToken);
                }

                await _context.Sales.AddAsync(sale, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return sale;
            }
            catch (Exception ex)
            {
                Log.Error($"Database error while creating Sale number {sale.SaleNumber}");
                throw new DomainException($"Database error while creating Sale number {sale.SaleNumber}");
            }
        }

        public async Task<bool> DeleteSaleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);

            if (sale is null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync(CancellationToken cancellationToken = default)
        {
            var sales = await _context.Sales
            .Include(sale => sale.Items)
            .ToListAsync(cancellationToken);


            if (sales is null || sales.Count < 1)
                throw new KeyNotFoundException("There's any sale here :/");

            return sales;
        }

        public async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(sale => sale.Items)
                .FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
        }

        /// <summary>
        /// This acts as an idempotency key for the API.
        /// It ensures that repeated requests, whether accidental or due to retries,
        /// do not result in multiple purchases of the same items.
        /// </summary>
        /// <param name="saleNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Sale if exists, otherwise null</returns>
        public async Task<Sale> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.FirstOrDefaultAsync(sale => sale.SaleNumber == saleNumber, cancellationToken);
        }

        public async Task<Sale> UpdateSaleAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingSale = await _context.Sales.AnyAsync(x => x.Id == sale.Id);

                if (!existingSale)
                    throw new KeyNotFoundException($"Sale with ID {sale.Id} not found!");

                sale.CreatedDate = DateTime.Parse(sale.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"));

                _context.Update(sale);
                await _context.SaveChangesAsync(cancellationToken);

                return sale;
            }
            catch (Exception ex)
            {
                Log.Error($"Database error while updating Sale number {sale.SaleNumber}");
                throw new DomainException($"Database error while updating Sale number {sale.SaleNumber}");
            }
        }
    }
}
