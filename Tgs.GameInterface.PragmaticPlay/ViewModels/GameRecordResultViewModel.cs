namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GameRecordResultViewModel(
	DateTimeOffset NextTime,
	IEnumerable<GameRecordDataViewModel> GameRecordDatas);
