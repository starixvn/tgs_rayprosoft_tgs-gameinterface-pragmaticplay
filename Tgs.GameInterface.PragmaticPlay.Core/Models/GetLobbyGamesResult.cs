using CsvHelper.Configuration.Attributes;

namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record GetLobbyGamesResult
{
	[Name("遊戲識別碼")]
	public string GameID { get; set; } = default!;
	[Name("遊戲識別碼數字")]
	public int GameIdNumeric { get; set; }
	[Name("遊戲名稱")]
	public string GameName { get; set; } = default!;
	[Name("遊戲類型")]
	public string GameTypeID { get; set; } = default!;
	[Name("司法管轄區")]
	public string Jurisdictions { get; set; } = default!;
	[Name("限數")]
	public int Lines { get; set; }
	[Name("免費回合獎勵")]
	public bool FrbAvailable { get; set; }
	[Name("遊戲試玩")]
	public bool VariableFrbAvailable { get; set; }
	[Name("支持功能")]
	public string Features { get; set; } = default!;
	[Name("demoGameAvailable")]
	public bool DemoGameAvailable { get; set; }
	[Name("排序顺序")]
	public int SortingOrder { get; set; }
}
