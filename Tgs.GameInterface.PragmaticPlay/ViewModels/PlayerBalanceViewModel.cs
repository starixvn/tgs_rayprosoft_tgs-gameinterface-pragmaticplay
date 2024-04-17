namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode);
