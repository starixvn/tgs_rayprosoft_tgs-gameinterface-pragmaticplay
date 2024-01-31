using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class GameRecordByLiveGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process_Settlement()
    {
        var fakeClient = Substitute.For<IGameRoundClient>();

        var query = new GameRecordByLiveQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            StartTime: DateTimeOffset.Parse("2023-04-21T03:24:00+08:00"));

        var result = new GameRoundsResult<GameRoundByLiveData>(
             NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682047923236),
             GameRoundDatas: new GameRoundByLiveData[]{
                 new()  {
                     PlayerID=1652690393452,
                     ExtPlayerID="4ucmqplxb029",
                     GameID="439",
                     PlaySessionID="97419402008",
                     ParentSessionID=string.Empty,
                     StartDate="2023-04-21 03:24:14",
                     EndDate="2023-04-21 03:24:38",
                     Status="C",
                     Type="R",
                     Bet=1.00m,
                     Win=2.00m,
                     Currency="CNY",
                     Jackpot=0.00m,
                     RoundDetails="{\"result\":\"Player\",\"bets\":[{\"c\":\"Plyr(NoComm)\",\"a\":\"1\"}]}"
                  }
             });
        _ = fakeClient
            .GetGameRoundByLiveAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                startTime: Arg.Is(query.StartTime),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameRecordByLiveGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.NextTime, actual.NextTime);
        Assert.True(actual.GameRecordDatas.Any());
        Assert.True(actual.GameRecordDatas.Single().IsClosed);
        Assert.Equal("439", actual.GameRecordDatas.Single().ExternalGameId);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:24:14+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:24:38+00:00"), actual.GameRecordDatas.Single().ExternalClosedOn);
        Assert.Equal(1, actual.GameRecordDatas.Single().Bet);
        Assert.Equal(2, actual.GameRecordDatas.Single().Settle);
        Assert.Equal(1, actual.GameRecordDatas.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Unsettlement()
    {
        var fakeClient = Substitute.For<IGameRoundClient>();

        var query = new GameRecordByLiveQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            StartTime: DateTimeOffset.Parse("2023-04-21T03:24:00+08:00"));

        var result = new GameRoundsResult<GameRoundByLiveData>(
             NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682047923236),
             GameRoundDatas: new GameRoundByLiveData[]{
                 new()  {
                     PlayerID=1652690393452,
                     ExtPlayerID="4ucmqplxb029",
                     GameID="439",
                     PlaySessionID="97419402408",
                     ParentSessionID=string.Empty,
                     StartDate="2023-04-21 03:24:47",
                     EndDate=string.Empty,
                     Status="I",
                     Type="R",
                     Bet=1.00m,
                     Win=0.00m,
                     Currency="CNY",
                     Jackpot=0.00m,
                     RoundDetails=""
                  }
             });
        _ = fakeClient
            .GetGameRoundByLiveAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                startTime: Arg.Is(query.StartTime),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameRecordByLiveGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.NextTime, actual.NextTime);
        Assert.True(actual.GameRecordDatas.Any());
        Assert.False(actual.GameRecordDatas.Single().IsClosed);
        Assert.Equal("439", actual.GameRecordDatas.Single().ExternalGameId);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:24:47+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.GameRecordDatas.Single().ExternalClosedOn);
        Assert.Equal(1, actual.GameRecordDatas.Single().Bet);
        Assert.Equal(0, actual.GameRecordDatas.Single().Settle);
        Assert.Equal(0, actual.GameRecordDatas.Single().WinLoss);
    }
}
