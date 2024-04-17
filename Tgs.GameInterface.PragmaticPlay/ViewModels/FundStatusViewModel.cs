namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	string OrderId);
