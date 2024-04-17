using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class PlayerLogoutCommandService(IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> client
		.LogoutAsync(
			apiUrl: command.ApiUrl,
			secureLogin: command.SecureLogin,
			secretKey: command.SecretKey,
			externalPlayerId: command.PlayerCode,
			cancellationToken: cancellationToken);
}
