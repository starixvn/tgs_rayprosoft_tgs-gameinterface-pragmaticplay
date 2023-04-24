using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => _client
        .TransferStatusAsync(
            apiUrl: query.ApiUrl,
            secureLogin: query.SecureLogin,
            secretKey: query.SecretKey,
            externalTransactionId: query.OrderId,
            cancellationToken: cancellationToken);
}
