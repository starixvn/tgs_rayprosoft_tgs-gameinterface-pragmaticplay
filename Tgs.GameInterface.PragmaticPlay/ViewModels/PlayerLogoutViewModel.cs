namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record PlayerLogoutViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode);
