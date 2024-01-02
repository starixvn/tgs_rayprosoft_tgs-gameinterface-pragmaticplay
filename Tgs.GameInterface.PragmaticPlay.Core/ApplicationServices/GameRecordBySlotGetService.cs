using System.Globalization;
using System.Text.Json;
using Tgs.GameInterface.PragmaticPlay.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordBySlotGetService(
    IGameRoundClient client) : IGetService<GameRecordBySlotQuery, GameRecordResult>
{

    public async ValueTask<GameRecordResult> GetAsync(
        GameRecordBySlotQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await client
            .GetGameRoundBySlotAsync(
                apiUrl: query.ApiUrl,
                secureLogin: query.SecureLogin,
                secretKey: query.SecretKey,
                startTime: query.StartTime,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !(result.GameRoundDatas ?? Enumerable.Empty<GameRoundBySlotData>()).Any()
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
