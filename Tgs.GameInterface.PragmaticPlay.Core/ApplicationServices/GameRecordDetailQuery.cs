namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordDetailQuery(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string ExternalRoundId,
	string ExternalGameId,
	string Language);
