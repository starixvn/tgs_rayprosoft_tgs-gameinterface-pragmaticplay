namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    string PlayerCode,
    string OrderId,
    decimal Amount);
