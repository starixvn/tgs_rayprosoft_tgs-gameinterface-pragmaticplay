using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _client
        .CreatePlayerAsync(
            apiUrl: command.ApiUrl,
            secureLogin: command.SecureLogin,
            secretKey: command.SecretKey,
            externalPlayerId: command.PlayerCode,
            currencyCode: command.CurrencyCode,
            cancellationToken: cancellationToken);
}
