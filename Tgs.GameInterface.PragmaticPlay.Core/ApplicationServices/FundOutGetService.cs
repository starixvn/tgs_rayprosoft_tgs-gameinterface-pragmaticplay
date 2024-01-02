using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class FundOutGetService(
    IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .DepositAsync(
                apiUrl: query.ApiUrl,
                secureLogin: query.SecureLogin,
                secretKey: query.SecretKey,
                externalPlayerId: query.PlayerCode,
                externalTransactionId: query.OrderId,
                amount: query.Amount,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TransactionId: result.TransactionId,
            Balance: result.Balance,
            Res: result.Res);
    }
}
