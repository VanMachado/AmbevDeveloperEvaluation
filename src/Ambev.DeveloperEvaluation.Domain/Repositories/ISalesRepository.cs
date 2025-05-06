using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for sale entity operations
    /// </summary>
    public interface ISalesRepository
    {
        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The created sale</returns>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtain a sale by their unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The sale if found, null otherwise</returns>
        Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtain all sales from system
        /// </summary>
        /// <returns>The sales if theres some, null otherwise</returns>
        Task<IEnumerable<Sale>> GetAllSalesAsync();

        /// <summary>
        /// Update a sale by their unique identifier
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Sale> UpdateSaleAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a sale from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if the user was deleted, false otherwise</returns>
        Task<bool> DeleteSaleAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
