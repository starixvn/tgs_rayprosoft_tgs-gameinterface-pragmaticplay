using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordDetailService(
    IGameApiClient client) : IGetService<GameRecordDetailQuery, string>
{
    public ValueTask<string> GetAsync(GameRecordDetailQuery query, CancellationToken cancellationToken = default)
        => client.GetGameDetailUrlAsync(
            apiUrl: query.ApiUrl,
            secureLogin: query.SecureLogin,
            secretKey: query.SecretKey,
            externalPlayerId: query.PlayerCode,
            roundId: query.ExternalRoundId,
            gameId: query.ExternalGameId,
            language: query.Language,
            cancellationToken: cancellationToken);
}
