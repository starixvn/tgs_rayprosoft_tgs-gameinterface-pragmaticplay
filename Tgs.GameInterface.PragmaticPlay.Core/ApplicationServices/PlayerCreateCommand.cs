namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode,
	string CurrencyCode);
