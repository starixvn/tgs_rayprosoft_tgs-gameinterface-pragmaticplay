using CsvHelper.Configuration.Attributes;

namespace Tgs.GameInterface.PragmaticPlay.Core.Models;

public record GetCasinoGamesResult
{
    [Name("遊戲識別碼")]
    public string GameID { get; init; } = default!;
    [Name("遊戲名稱")]
    public string GameName { get; init; } = default!;
    [Name("遊戲類性")]
    public string GameTypeID { get; init; } = default!;
    [Name("遊戲類型說明")]
    public string TypeDescription { get; init; } = default!;
    [Name("遊戲採用技術")]
    public string Technology { get; init; } = default!;
    [Name("遊戲支援平台")]
    public string Platform { get; init; } = default!;
    [Name("遊戲試玩")]
    public bool DemoGameAvailable { get; init; }
    [Name("遊戲識別碼數字")]
    public int GameIdNumeric { get; init; }
    [Name("遊戲產品類型")]
    public string DataType { get; init; } = default!;
}
