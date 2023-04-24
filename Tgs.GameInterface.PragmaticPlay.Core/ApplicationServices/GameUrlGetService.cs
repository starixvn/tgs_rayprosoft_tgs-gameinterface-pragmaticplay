using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
             .GetGameUrlAsync(
                 apiUrl: query.ApiUrl,
                 secureLogin: query.SecureLogin,
                 secretKey: query.SecretKey,
                 externalPlayerId: query.PlayerCode,
                 externalGameId: query.ExternalGameId,
                 language: query.Language,
                 isMobile: query.IsMobile,
                 lobbyUrl: query.LobbyUrl,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        return new GameUrlResult(
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }
}
