namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string OrderId);
