namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameRecordDataViewModel(
	string ExternalRoundId,
	string ExternalGameId,
	string PlayerCode,
	bool IsClosed,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
