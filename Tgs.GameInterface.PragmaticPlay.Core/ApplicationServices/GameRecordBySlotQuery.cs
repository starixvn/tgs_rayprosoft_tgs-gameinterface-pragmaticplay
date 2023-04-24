namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordBySlotQuery(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    DateTimeOffset StartTime);
