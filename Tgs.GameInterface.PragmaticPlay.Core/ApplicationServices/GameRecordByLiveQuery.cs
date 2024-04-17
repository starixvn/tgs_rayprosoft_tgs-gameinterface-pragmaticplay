namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordByLiveQuery(
	Uri ApiUrl,
	string SecureLogin,
	string SecretKey,
	DateTimeOffset StartTime);
