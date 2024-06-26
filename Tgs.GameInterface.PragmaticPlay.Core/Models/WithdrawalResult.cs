namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record WithdrawalResult(
	long TransactionId,
	decimal Balance,
	string Resp);
