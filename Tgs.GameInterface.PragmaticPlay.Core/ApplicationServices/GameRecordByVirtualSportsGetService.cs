using System.Globalization;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

internal class GameRecordByVirtualSportsGetService(
	IGameRoundClient client) : IGetService<GameRecordByVirtualSportsQuery, GameRecordResult>
{
	public async ValueTask<GameRecordResult> GetAsync(
		GameRecordByVirtualSportsQuery query,
		CancellationToken cancellationToken = default)
	{
		var result = await client
			.GetGameRoundByVirtualSportsAsync(
				apiUrl: query.ApiUrl,
				secureLogin: query.SecureLogin,
				secretKey: query.SecretKey,
				startTime: query.StartTime,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		/*
         * Type 游戏类型：
         *  R -游戏回合
         *  F –免费旋转在游戏回合中触发
         *  PP客服解釋:
         *      免费旋转进行玩时候win部分会算主单
         *      免费旋转的bet和win值是0.00
         */
		return !(result.GameRoundDatas ?? []).Any()
			? new GameRecordResult(
				NextTime: result.NextTime,
				GameRecordDatas: [])
			: new GameRecordResult(
				NextTime: result.NextTime,
				GameRecordDatas: result.GameRoundDatas!
				.Where(gameround => !gameround.Type.Equals("F", StringComparison.OrdinalIgnoreCase))
				.Select(gameround => new GameRecordData(
					ExternalRoundId: gameround.PlaySessionID,
					ExternalGameId: gameround.GameID,
					PlayerCode: gameround.ExtPlayerID,
					IsClosed: gameround.Status.Equals("C", StringComparison.OrdinalIgnoreCase),
					IsCancel: gameround.Status.Equals("C", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(gameround.EndDate),
					ExternalCreatedOn: DateTimeOffset.Parse(gameround.StartDate, null, DateTimeStyles.AssumeUniversal),
					ExternalClosedOn: gameround.Status.Equals("I", StringComparison.OrdinalIgnoreCase)
					? DateTimeOffset.UnixEpoch
					: !string.IsNullOrEmpty(gameround.EndDate)
						? DateTimeOffset.Parse(gameround.EndDate, null, DateTimeStyles.AssumeUniversal)
						: DateTimeOffset.Parse(gameround.StartDate, null, DateTimeStyles.AssumeUniversal),
					Bet: gameround.Bet,
					Settle: gameround.Win,
					WinLoss: gameround.Status.Equals("I", StringComparison.OrdinalIgnoreCase)
					? 0
					: gameround.Win - gameround.Bet,
					AdditionalData: JsonSerializer.Serialize(gameround))));
	}
}
