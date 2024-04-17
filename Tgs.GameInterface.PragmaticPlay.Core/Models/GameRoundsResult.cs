namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record GameRoundsResult<TData>(
	DateTimeOffset NextTime,
	IEnumerable<TData> GameRoundDatas);
