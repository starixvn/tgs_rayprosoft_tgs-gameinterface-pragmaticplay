namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameRecordDetailViewModel(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    string PlayerCode,
    string ExternalRoundId,
    string ExternalGameId,
    string Language);
