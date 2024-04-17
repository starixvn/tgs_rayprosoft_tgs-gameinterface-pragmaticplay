using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client
		.CreatePlayerAsync(
			apiUrl: command.ApiUrl,
			secureLogin: command.SecureLogin,
			secretKey: command.SecretKey,
			externalPlayerId: command.PlayerCode,
			currencyCode: command.CurrencyCode,
			cancellationToken: cancellationToken);
}
