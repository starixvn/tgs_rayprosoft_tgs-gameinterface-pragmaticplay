using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordDetailService : IGetService<GameRecordDetailQuery, string>
{
    private readonly IGameApiClient _client;

    public GameRecordDetailService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameRecordDetailQuery query, CancellationToken cancellationToken = default)
        => _client.GetGameDetailUrlAsync(
            apiUrl: query.ApiUrl,
            secureLogin: query.SecureLogin,
            secretKey: query.SecretKey,
            externalPlayerId: query.PlayerCode,
            roundId: query.ExternalRoundId,
            gameId: query.ExternalGameId,
            language: query.Language,
            cancellationToken: cancellationToken);
}
