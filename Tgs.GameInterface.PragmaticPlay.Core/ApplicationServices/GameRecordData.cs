namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordData(
    string ExternalRoundId,
    string PlayerCode,
    bool IsClosed,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal Settle,
    decimal WinLoss,
    string AdditionalData);
