using Monobank.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Monobank.Client.Exceptions;

namespace Monobank.Client.Interfaces
{
    public interface IMonobankCurrenciesService
    {
        /// <summary>
        /// Retrieves a collection of currency information asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// The task result contains a collection of <see cref="CurrencyInfo"/> objects
        /// representing information about different currencies.
        /// </returns>
        /// <exception cref="MonobankClientApiException">The request could not be completed due to api error.</exception>
        Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    }
}