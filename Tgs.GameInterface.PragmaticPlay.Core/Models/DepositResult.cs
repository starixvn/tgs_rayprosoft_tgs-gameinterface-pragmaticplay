namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record DepositResult(
	long TransactionId,
	decimal Balance,
	string Resp);
