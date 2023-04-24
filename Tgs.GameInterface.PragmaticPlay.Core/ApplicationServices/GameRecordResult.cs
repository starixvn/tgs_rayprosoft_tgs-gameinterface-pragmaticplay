namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameRecordResult(
    DateTimeOffset NextTime,
    IEnumerable<GameRecordData> GameRecordDatas);
