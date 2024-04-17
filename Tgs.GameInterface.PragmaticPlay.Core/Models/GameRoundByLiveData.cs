using CsvHelper.Configuration.Attributes;

namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public class GameRoundByLiveData
{
	[Name("bet")]
	public decimal Bet { get; set; }

	[Name("currency")]
	public string Currency { get; set; } = default!;

	[Name("endDate")]
	public string EndDate { get; set; } = default!;

	[Name("extPlayerID")]
	public string ExtPlayerID { get; set; } = default!;

	[Name("gameID")]
	public string GameID { get; set; } = default!;

	[Name("jackpot")]
	public decimal Jackpot { get; set; }

	[Name("parentSessionID")]
	public string ParentSessionID { get; set; } = default!;

	[Name("playerID")]
	public long PlayerID { get; set; } = default!;

	[Name("playSessionID")]
	public string PlaySessionID { get; set; } = default!;

	[Name("roundDetails")]
	public string RoundDetails { get; set; } = default!;

	[Name("startDate")]
	public string StartDate { get; set; } = default!;

	[Name("status")]
	public string Status { get; set; } = default!;

	[Name("type")]
	public string Type { get; set; } = default!;

	[Name("win")]
	public decimal Win { get; set; }
}
