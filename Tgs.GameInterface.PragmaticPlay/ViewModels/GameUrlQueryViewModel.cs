namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameUrlQueryViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string ExternalGameId,
	string Language,
	bool IsMobile,
	string? LobbyUrl);
