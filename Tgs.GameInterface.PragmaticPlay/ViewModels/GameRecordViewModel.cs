namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameRecordViewModel(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    DateTimeOffset StartTime);
