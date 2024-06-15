namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordCancelRoundCommand(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string ExternalRoundId,
	string ExternalGameId);
