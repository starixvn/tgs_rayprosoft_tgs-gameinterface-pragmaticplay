using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class GameRecordBySlotGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process_Free()
	{
		var fakeClient = Substitute.For<IGameRoundClient>();

		var query = new GameRecordBySlotQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			StartTime: DateTimeOffset.Parse("2023-04-21T13:36:00+08:00"));

		var result = new GameRoundsResult<GameRoundBySlotData>(
			 NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682055513073),
			 GameRoundDatas: [
				 new()  {
					 PlayerID=1652690393452,
					 ExtPlayerID="4ucmqplxb029",
					 GameID="vswaysmonkey",
					 PlaySessionID="2920180886",
					 ParentSessionID="2920180865",
					 StartDate="2023-04-21 05:36:46",
					 EndDate="2023-04-21 05:36:46",
					 Status="C",
					 Type="F",
					 Bet=0.00m,
					 Win=0.00m,
					 Currency="CNY",
					 Jackpot=0.00m
				  }
			 ]);
		_ = fakeClient
			.GetGameRoundBySlotAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				startTime: Arg.Is(query.StartTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var client = new GameRecordBySlotGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(result.NextTime, actual.NextTime);
		Assert.False(actual.GameRecordDatas.Any());
	}

	[Fact]
	public async Task Normal_Process_Settlement()
	{
		var fakeClient = Substitute.For<IGameRoundClient>();

		var query = new GameRecordBySlotQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			StartTime: DateTimeOffset.Parse("2023-04-21T11:15:00+08:00"));

		var result = new GameRoundsResult<GameRoundBySlotData>(
			 NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682047053189),
			 GameRoundDatas: [
				 new()  {
					 PlayerID=2953411,
					 ExtPlayerID="4ucmqplxb029",
					 GameID="vs243lionsgold",
					 PlaySessionID="2920166764",
					 ParentSessionID=string.Empty,
					 StartDate="2023-04-21 03:15:56",
					 EndDate="2023-04-21 03:15:57",
					 Status="C",
					 Type="R",
					 Bet=0.54m,
					 Win=1.62m,
					 Currency="CNY",
					 Jackpot=0.00m
				  }
			 ]);
		_ = fakeClient
			.GetGameRoundBySlotAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				startTime: Arg.Is(query.StartTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var client = new GameRecordBySlotGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(result.NextTime, actual.NextTime);
		Assert.True(actual.GameRecordDatas.Any());
		Assert.True(actual.GameRecordDatas.Single().IsClosed);
		Assert.False(actual.GameRecordDatas.Single().IsCancel);
		Assert.Equal("vs243lionsgold", actual.GameRecordDatas.Single().ExternalGameId);
		Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:15:56+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:15:57+00:00"), actual.GameRecordDatas.Single().ExternalClosedOn);
		Assert.Equal(0.54m, actual.GameRecordDatas.Single().Bet);
		Assert.Equal(1.62m, actual.GameRecordDatas.Single().Settle);
		Assert.Equal(1.08m, actual.GameRecordDatas.Single().WinLoss);
	}

	[Fact]
	public async Task Normal_Process_Unsettlement()
	{
		var fakeClient = Substitute.For<IGameRoundClient>();

		var query = new GameRecordBySlotQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			StartTime: DateTimeOffset.Parse("2023-04-21T13:36:00+08:00"));

		var result = new GameRoundsResult<GameRoundBySlotData>(
			 NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682055513073),
			 GameRoundDatas: [
				 new()  {
					 PlayerID=2953411,
					 ExtPlayerID="4ucmqplxb029",
					 GameID="vswaysmonkey",
					 PlaySessionID="2920180865",
					 ParentSessionID=default,
					 StartDate="2023-04-21 05:36:39",
					 EndDate=default,
					 Status="I",
					 Type="R",
					 Bet=0.60m,
					 Win=0.00m,
					 Currency="CNY",
					 Jackpot=0.00m
				  }
			 ]);
		_ = fakeClient
			.GetGameRoundBySlotAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				startTime: Arg.Is(query.StartTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var client = new GameRecordBySlotGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(result.NextTime, actual.NextTime);
		Assert.True(actual.GameRecordDatas.Any());
		Assert.False(actual.GameRecordDatas.Single().IsClosed);
		Assert.False(actual.GameRecordDatas.Single().IsCancel);
		Assert.Equal("vswaysmonkey", actual.GameRecordDatas.Single().ExternalGameId);
		Assert.Equal(DateTimeOffset.Parse("2023-04-21T05:36:39+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.UnixEpoch, actual.GameRecordDatas.Single().ExternalClosedOn);
		Assert.Equal(0.60m, actual.GameRecordDatas.Single().Bet);
		Assert.Equal(0, actual.GameRecordDatas.Single().Settle);
		Assert.Equal(0, actual.GameRecordDatas.Single().WinLoss);
	}

	[Fact]
	public async Task Normal_Process_Cancel()
	{
		var fakeClient = Substitute.For<IGameRoundClient>();

		var query = new GameRecordBySlotQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			StartTime: DateTimeOffset.Parse("2023-04-21T13:36:00+08:00"));

		var result = new GameRoundsResult<GameRoundBySlotData>(
			 NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1717479120000),
			 GameRoundDatas: [
				 new()  {
					 PlayerID=6270808,
					 ExtPlayerID="4udvzfxyzowl",
					 GameID="vswayscharms",
					 PlaySessionID="620757986000",
					 ParentSessionID=default,
					 StartDate="2024-06-04 05:25:25",
					 EndDate=default,
					 Status="C",
					 Type="R",
					 Bet=6.00m,
					 Win=0.00m,
					 Currency="CNY",
					 Jackpot=0.00m
				  }
			 ]);
		_ = fakeClient
			.GetGameRoundBySlotAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				startTime: Arg.Is(query.StartTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var client = new GameRecordBySlotGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(result.NextTime, actual.NextTime);
		Assert.True(actual.GameRecordDatas.Single().IsClosed);
		Assert.True(actual.GameRecordDatas.Single().IsCancel);
		Assert.Equal("vswayscharms", actual.GameRecordDatas.Single().ExternalGameId);
		Assert.Equal(DateTimeOffset.Parse("2024-06-04T05:25:25+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2024-06-04T05:25:25+00:00"), actual.GameRecordDatas.Single().ExternalClosedOn);
		Assert.Equal(6.00m, actual.GameRecordDatas.Single().Bet);
		Assert.Equal(0.0m, actual.GameRecordDatas.Single().Settle);
		Assert.Equal(-6.00m, actual.GameRecordDatas.Single().WinLoss);
	}
}
