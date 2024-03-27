using Monobank.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Monobank.Client.Exceptions;

namespace Monobank.Client.Interfaces
{
    public interface IMonobankSingleClientService
    {
        /// <summary>
        /// Retrieves information about the client asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// The task result contains the <see cref="ClientInfo"/> for the client.
        /// </returns>
        /// <exception cref="MonobankClientApiException">The request could not be completed due to api error.</exception>
        Task<ClientInfo> GetClientInfoAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of statements for a specified card within a specified date range asynchronously.
        /// </summary>
        /// <param name="from">The start date of the date range for statement retrieval.</param>
        /// <param name="to">The end date of the date range for statement retrieval.</param>
        /// <param name="cardId">The ID of the card for which statements are to be retrieved.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// The task result contains a collection of <see cref="Statement"/> objects
        /// representing the statements for the specified card within the specified date range.
        /// </returns>
        /// <exception cref="MonobankClientInvalidCardIdException">The request could not be completed due to invalid card id.</exception>
        /// <exception cref="MonobankClientInvalidDateRange">The request could not be completed due to invalid date range.</exception>
        /// <exception cref="MonobankClientApiException">The request could not be completed due to api error.</exception>
        Task<ICollection<Statement>> GetStatementsAsync(DateTime from, DateTime to, string cardId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the webhook URL for asynchronous notifications.
        /// </summary>
        /// <param name="url">The URL to which webhook notifications will be sent.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// The task result is a boolean value indicating whether
        /// the webhook was successfully set up (true) or not (false).
        /// </returns>
        /// <exception cref="MonobankClientInvalidTokenException">The request could not be completed due to missing token.</exception>
        /// <exception cref="MonobankClientApiException">The request could not be completed due to invalid webhook url.</exception>
        /// <exception cref="MonobankClientApiException">The request could not be completed due to api error.</exception>
        Task<bool> SetWebhookAsync(string url, CancellationToken cancellationToken = default);
    }
}