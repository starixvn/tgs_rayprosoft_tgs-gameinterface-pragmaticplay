namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record FundResult(
	long TransactionId,
	decimal Balance,
	string Res);
