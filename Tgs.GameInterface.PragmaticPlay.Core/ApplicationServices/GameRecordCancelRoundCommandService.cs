using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordCancelRoundCommandService(IGameApiClient client) : ICommandService<GameRecordCancelRoundCommand>
{
	public ValueTask ExecuteAsync(GameRecordCancelRoundCommand command, CancellationToken cancellationToken = default)
		=> client.CancelRoundAsync(
			apiUrl: command.ApiUrl,
			secureLogin: command.SecureLogin,
			secretKey: command.SecretKey,
			externalPlayerId: command.PlayerCode,
			roundId: command.ExternalRoundId,
			gameId: command.ExternalGameId,
			cancellationToken: cancellationToken);
}
