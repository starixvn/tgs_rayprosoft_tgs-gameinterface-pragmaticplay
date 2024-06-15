namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameRecordCancelRoundViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string ExternalRoundId,
	string ExternalGameId);
