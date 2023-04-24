using System.Globalization;
using System.Text.Json;
using Tgs.GameInterface.PragmaticPlay.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordByVirtualSportsGetService : IGetService<GameRecordByVirtualSportsQuery, GameRecordResult>
{
    private readonly IGameRoundClient _client;

    public GameRecordByVirtualSportsGetService(
        IGameRoundClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameRecordResult> GetAsync(
        GameRecordByVirtualSportsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _client
            .GetGameRoundByVirtualSportsAsync(
                apiUrl: query.ApiUrl,
                secureLogin: query.SecureLogin,
                secretKey: query.SecretKey,
                startTime: query.StartTime,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gamerounds = result.GameRoundDatas
            ?? Enumerable.Empty<GameRoundByVirtualSportsData>();

        return !(result.GameRoundDatas ?? Enumerable.Empty<GameRoundByVirtualSportsData>()).Any()
            ? new GameRecordResult(
                NextTime: result.NextTime,
                GameRecordDatas: Enumerable.Empty<GameRecordData>())
            : new GameRecordResult(
                NextTime: result.NextTime,
                GameRecordDatas: result.GameRoundDatas!
                .Where(gameround => !gameround.Type.Equals("F", StringComparison.OrdinalIgnoreCase))
                .Select(gameround => new GameRecordData(
                    ExternalRoundId: gameround.PlaySessionID,
                    ExternalGameId: gameround.GameID,
                    PlayerCode: gameround.ExtPlayerID,
                    IsClosed: gameround.Status.Equals("C", StringComparison.OrdinalIgnoreCase),
                    ExternalCreatedOn: DateTimeOffset.Parse(gameround.StartDate, null, DateTimeStyles.AssumeUniversal),
                    ExternalClosedOn: gameround.Status.Equals("I", StringComparison.OrdinalIgnoreCase)
                    ? DateTimeOffset.UnixEpoch
                    : DateTimeOffset.Parse(gameround.EndDate, null, DateTimeStyles.AssumeUniversal),
                    Bet: gameround.Bet,
                    Settle: gameround.Win,
                    WinLoss: gameround.Status.Equals("I", StringComparison.OrdinalIgnoreCase)
                    ? 0
                    : gameround.Win - gameround.Bet,
                    AdditionalData: JsonSerializer.Serialize(gameround))));
    }
}
