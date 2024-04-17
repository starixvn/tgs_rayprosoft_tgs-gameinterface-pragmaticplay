using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client.TransferStatusAsync(
			apiUrl: query.ApiUrl,
			secureLogin: query.SecureLogin,
			secretKey: query.SecretKey,
			externalTransactionId: query.OrderId,
			cancellationToken: cancellationToken);
}
