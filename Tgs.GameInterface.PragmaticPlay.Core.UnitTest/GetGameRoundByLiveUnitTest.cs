using System.Net;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetGameRoundByLiveUnitTest
{
    [Fact]
    public async Task Normal_Process_Settlement()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var startTime = DateTimeOffset.Parse("2023-04-21T03:24:00+08:00");

        var fakeLog = NullLogger<GameRoundClient>.Instance;

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.LC.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var response = "timepoint=1682047923236\r\n" +
            "playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot,roundDetails\r\n" +
            "1652690393452,4ucmqplxb029,439,97419402008,null,2023-04-21 03:24:14,2023-04-21 03:24:38,C,R,1.00,2.00,CNY,0.00,\"{\"\"result\"\":\"\"Player\"\",\"\"bets\"\":[{\"\"c\"\":\"\"Plyr(NoComm)\"\",\"\"a\"\":\"\"1\"\"}]}\"";
        var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameRoundClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetGameRoundByLiveAsync(
            apiUrl: apiUrl,
            secureLogin: secureLogin,
            secretKey: secretKey,
            startTime: startTime);

        Assert.NotNull(actual);
        Assert.Equal("1682047923236", actual.NextTime.ToUnixTimeMilliseconds().ToString());
        Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
        Assert.Equal("97419402008", actual.GameRoundDatas.Single().PlaySessionID);
        Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
        Assert.Equal("2023-04-21 03:24:14", actual.GameRoundDatas.Single().StartDate);
        Assert.Equal("2023-04-21 03:24:38", actual.GameRoundDatas.Single().EndDate);
        Assert.Equal("C", actual.GameRoundDatas.Single().Status);
        Assert.Equal("R", actual.GameRoundDatas.Single().Type);
        Assert.NotEmpty(actual.GameRoundDatas.Single().RoundDetails);
    }

    [Fact]
    public async Task Normal_Process_Unsettlement()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var startTime = DateTimeOffset.Parse("2023-04-21T03:24:00+08:00");

        var fakeLog = NullLogger<GameRoundClient>.Instance;

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.LC.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var response = "timepoint=1682047504013\r\n" +
            "playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot,roundDetails\r\n" +
            "1652690393452,4ucmqplxb029,439,97419402408,null,2023-04-21 03:24:47,null,I,R,1.00,0.00,CNY,0.00,\"\"";
        var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameRoundClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetGameRoundByLiveAsync(
            apiUrl: apiUrl,
            secureLogin: secureLogin,
            secretKey: secretKey,
            startTime: startTime);

        Assert.NotNull(actual);
        Assert.Equal("1682047504013", actual.NextTime.ToUnixTimeMilliseconds().ToString());
        Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
        Assert.Equal("97419402408", actual.GameRoundDatas.Single().PlaySessionID);
        Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
        Assert.Equal("2023-04-21 03:24:47", actual.GameRoundDatas.Single().StartDate);
        Assert.Null(actual.GameRoundDatas.Single().EndDate);
        Assert.Equal("I", actual.GameRoundDatas.Single().Status);
        Assert.Equal("R", actual.GameRoundDatas.Single().Type);
        Assert.Empty(actual.GameRoundDatas.Single().RoundDetails);
    }
}
