namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string PlayerCode);
