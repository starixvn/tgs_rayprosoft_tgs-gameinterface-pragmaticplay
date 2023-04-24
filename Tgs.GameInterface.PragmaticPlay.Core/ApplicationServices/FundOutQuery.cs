namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    string PlayerCode,
    string OrderId,
    decimal Amount);
