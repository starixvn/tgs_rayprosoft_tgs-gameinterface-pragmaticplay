using CsvHelper.Configuration.Attributes;

namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record GetCasinoGamesResult
{
    [Name("GameID")]
    public string GameID { get; init; } = default!;
    [Name("GameName")]
    public string GameName { get; init; } = default!;
    [Name("GameTypeID")]
    public string GameTypeID { get; init; } = default!;
    [Name("TypeDescription")]
    public string TypeDescription { get; init; } = default!;
    [Name("Technology")]
    public string Technology { get; init; } = default!;
    [Name("Platform")]
    public string Platform { get; init; } = default!;
    [Name("DemoGameAvailable")]
    public bool DemoGameAvailable { get; init; }
    public int GameIdNumeric { get; init; }
    [Name("DataType")]
    public string DataType { get; init; } = default!;
}
