namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordByVirtualSportsQuery(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    DateTimeOffset StartTime);
