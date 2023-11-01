using System.Net;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetGameRoundByVirtualSportsUnitTest
{
    [Fact]
    public async Task Normal_Process_Settlement()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var startTime = DateTimeOffset.Parse("2023-04-21T11:55:00+08:00");

        var fakeLog = NullLogger<GameRoundClient>.Instance;

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.VSB.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var response = "timepoint=1682049681569\r\n" +
            "playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
            "2953411,4ucmqplxb029,vpdr7,63389,null,2023-04-21 03:55:07,2023-04-21 03:57:32,C,R,1.00,0.00,CNY,0.00";
        var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameRoundClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetGameRoundByVirtualSportsAsync(
            apiUrl: apiUrl,
            secureLogin: secureLogin,
            secretKey: secretKey,
            startTime: startTime);

        Assert.NotNull(actual);
        Assert.Equal("1682049681569", actual.NextTime.ToUnixTimeMilliseconds().ToString());
        Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
        Assert.Equal("63389", actual.GameRoundDatas.Single().PlaySessionID);
        Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
        Assert.Equal("2023-04-21 03:55:07", actual.GameRoundDatas.Single().StartDate);
        Assert.Equal("2023-04-21 03:57:32", actual.GameRoundDatas.Single().EndDate);
        Assert.Equal("C", actual.GameRoundDatas.Single().Status);
        Assert.Equal("R", actual.GameRoundDatas.Single().Type);
    }

    [Fact]
    public async Task Normal_Process_Unsettlement()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var startTime = DateTimeOffset.Parse("2023-04-21T12:01:00+08:00");

        var fakeLog = NullLogger<GameRoundClient>.Instance;

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.VSB.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var response = "timepoint=1682049681569\r\n" +
            "playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
            "2953411,4ucmqplxb029,vpdr7,63392,null,2023-04-21 04:01:01,null,I,R,1.00,0.00,CNY,0.00";
        var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameRoundClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetGameRoundByVirtualSportsAsync(
            apiUrl: apiUrl,
            secureLogin: secureLogin,
            secretKey: secretKey,
            startTime: startTime);

        Assert.NotNull(actual);
        Assert.Equal("1682049681569", actual.NextTime.ToUnixTimeMilliseconds().ToString());
        Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
        Assert.Equal("63392", actual.GameRoundDatas.Single().PlaySessionID);
        Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
        Assert.Equal("2023-04-21 04:01:01", actual.GameRoundDatas.Single().StartDate);
        Assert.Null(actual.GameRoundDatas.Single().EndDate);
        Assert.Equal("I", actual.GameRoundDatas.Single().Status);
        Assert.Equal("R", actual.GameRoundDatas.Single().Type);
    }
}
