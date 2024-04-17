namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record PlayerCreateViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string CurrencyCode);
