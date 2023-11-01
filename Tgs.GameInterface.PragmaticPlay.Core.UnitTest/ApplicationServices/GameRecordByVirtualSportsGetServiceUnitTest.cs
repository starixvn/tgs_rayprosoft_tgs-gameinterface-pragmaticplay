using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class GameRecordByVirtualSportsGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process_Settlement()
    {
        var fakeClient = Substitute.For<IGameRoundClient>();

        var query = new GameRecordByVirtualSportsQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            StartTime: DateTimeOffset.Parse("2023-04-21T11:55:00+08:00"));

        var result = new GameRoundsResult<GameRoundByVirtualSportsData>(
             NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682049681569),
             GameRoundDatas: new GameRoundByVirtualSportsData[]{
                 new GameRoundByVirtualSportsData()  {
                     PlayerID=2953411,
                     ExtPlayerID="4ucmqplxb029",
                     GameID="vpdr7",
                     PlaySessionID="63389",
                     ParentSessionID=string.Empty,
                     StartDate="2023-04-21 03:55:07",
                     EndDate="2023-04-21 03:57:32",
                     Status="C",
                     Type="R",
                     Bet=1m,
                     Win=0m,
                     Currency="CNY",
                     Jackpot=0.00m
                  }
             });
        _ = fakeClient
            .GetGameRoundByVirtualSportsAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                startTime: Arg.Is(query.StartTime),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameRecordByVirtualSportsGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.NextTime, actual.NextTime);
        Assert.True(actual.GameRecordDatas.Any());
        Assert.True(actual.GameRecordDatas.Single().IsClosed);
        Assert.Equal("vpdr7", actual.GameRecordDatas.Single().ExternalGameId);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:55:07+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T03:57:32+00:00"), actual.GameRecordDatas.Single().ExternalClosedOn);
        Assert.Equal(1m, actual.GameRecordDatas.Single().Bet);
        Assert.Equal(0m, actual.GameRecordDatas.Single().Settle);
        Assert.Equal(-1m, actual.GameRecordDatas.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Unsettlement()
    {
        var fakeClient = Substitute.For<IGameRoundClient>();

        var query = new GameRecordByVirtualSportsQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            StartTime: DateTimeOffset.Parse("2023-04-21T12:01:00+08:00"));

        var result = new GameRoundsResult<GameRoundByVirtualSportsData>(
             NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1682049681569),
             GameRoundDatas: new GameRoundByVirtualSportsData[]{
                 new GameRoundByVirtualSportsData()  {
                     PlayerID=2953411,
                     ExtPlayerID="4ucmqplxb029",
                     GameID="vpdr7",
                     PlaySessionID="63392",
                     ParentSessionID=string.Empty,
                     StartDate="2023-04-21 04:01:01",
                     EndDate=string.Empty,
                     Status="I",
                     Type="R",
                     Bet=1m,
                     Win=0.00m,
                     Currency="CNY",
                     Jackpot=0.00m
                  }
             });
        _ = fakeClient
            .GetGameRoundByVirtualSportsAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                startTime: Arg.Is(query.StartTime),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameRecordByVirtualSportsGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.NextTime, actual.NextTime);
        Assert.True(actual.GameRecordDatas.Any());
        Assert.False(actual.GameRecordDatas.Single().IsClosed);
        Assert.Equal("vpdr7", actual.GameRecordDatas.Single().ExternalGameId);
        Assert.Equal(DateTimeOffset.Parse("2023-04-21T04:01:01+00:00"), actual.GameRecordDatas.Single().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.GameRecordDatas.Single().ExternalClosedOn);
        Assert.Equal(1m, actual.GameRecordDatas.Single().Bet);
        Assert.Equal(0, actual.GameRecordDatas.Single().Settle);
        Assert.Equal(0, actual.GameRecordDatas.Single().WinLoss);
    }
}
