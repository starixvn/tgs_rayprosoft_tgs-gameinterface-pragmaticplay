using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .GetBalanceAsync(
                apiUrl: query.ApiUrl,
                secureLogin: query.SecureLogin,
                secretKey: query.SecretKey,
                externalPlayerId: query.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult(
            result.IsPlayerExist,
            result.Balance);
    }
}
