using System.Net;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetGameRoundBySlotUnitTest
{
	[Fact]
	public async Task Normal_Process_Free()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var startTime = DateTimeOffset.Parse("2023-04-21T13:36:00+08:00");

		var fakeLog = NullLogger<GameRoundClient>.Instance;

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["login"] = secureLogin;
		query["password"] = secretKey;
		query["dataType"] = DataType.RNG.ToString();
		query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
		query["options"] = "addRoundDetails";

		var response = "timepoint=1682055513073\r\n" +
			"playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
			"2953411,4ucmqplxb029,vswaysmonkey,2920180886,2920180865,2023-04-21 05:36:46,2023-04-21 05:36:46,C,F,0.00,0.00,CNY,0.00";
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

		var actual = await client.GetGameRoundBySlotAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			startTime: startTime);

		Assert.NotNull(actual);
		Assert.Equal("1682055513073", actual.NextTime.ToUnixTimeMilliseconds().ToString());
		Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
		Assert.Equal("2920180886", actual.GameRoundDatas.Single().PlaySessionID);
		Assert.Equal("2920180865", actual.GameRoundDatas.Single().ParentSessionID);
		Assert.Equal("2023-04-21 05:36:46", actual.GameRoundDatas.Single().StartDate);
		Assert.Equal("2023-04-21 05:36:46", actual.GameRoundDatas.Single().StartDate);
		Assert.Equal("C", actual.GameRoundDatas.Single().Status);
		Assert.Equal("F", actual.GameRoundDatas.Single().Type);
	}

	[Fact]
	public async Task Normal_Process_Settlement()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var startTime = DateTimeOffset.Parse("2023-04-21T11:15:00+08:00");

		var fakeLog = NullLogger<GameRoundClient>.Instance;

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["login"] = secureLogin;
		query["password"] = secretKey;
		query["dataType"] = DataType.RNG.ToString();
		query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
		query["options"] = "addRoundDetails";

		var response = "timepoint=1682047053189\r\n" +
			"playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
			"2953411,4ucmqplxb029,vs243lionsgold,2920166764,null,2023-04-21 03:15:56,2023-04-21 03:15:57,C,R,0.54,1.62,CNY,0.00\r\n";
		var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameRoundClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetGameRoundBySlotAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			startTime: startTime);

		Assert.NotNull(actual);
		Assert.Equal("1682047053189", actual.NextTime.ToUnixTimeMilliseconds().ToString());
		Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
		Assert.Equal("2920166764", actual.GameRoundDatas.Single().PlaySessionID);
		Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
		Assert.Equal("2023-04-21 03:15:56", actual.GameRoundDatas.Single().StartDate);
		Assert.Equal("2023-04-21 03:15:57", actual.GameRoundDatas.Single().EndDate);
		Assert.Equal("C", actual.GameRoundDatas.Single().Status);
		Assert.Equal("R", actual.GameRoundDatas.Single().Type);
	}

	[Fact]
	public async Task Normal_Process_Unsettlement()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var startTime = DateTimeOffset.Parse("2023-04-21T13:36:00+08:00");

		var fakeLog = NullLogger<GameRoundClient>.Instance;

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["login"] = secureLogin;
		query["password"] = secretKey;
		query["dataType"] = DataType.RNG.ToString();
		query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
		query["options"] = "addRoundDetails";

		var response = "timepoint=1682055513073\r\n" +
			"playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
			"2953411,4ucmqplxb029,vswaysmonkey,2920180865,null,2023-04-21 05:36:39,null,I,R,0.60,0.00,CNY,0.00";
		var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameRoundClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetGameRoundBySlotAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			startTime: startTime);

		Assert.NotNull(actual);
		Assert.Equal("1682055513073", actual.NextTime.ToUnixTimeMilliseconds().ToString());
		Assert.Equal("4ucmqplxb029", actual.GameRoundDatas.Single().ExtPlayerID);
		Assert.Equal("2920180865", actual.GameRoundDatas.Single().PlaySessionID);
		Assert.Null(actual.GameRoundDatas.Single().ParentSessionID);
		Assert.Equal("2023-04-21 05:36:39", actual.GameRoundDatas.Single().StartDate);
		Assert.Null(actual.GameRoundDatas.Single().EndDate);
		Assert.Equal("I", actual.GameRoundDatas.Single().Status);
		Assert.Equal("R", actual.GameRoundDatas.Single().Type);
	}

	[Fact]
	public async Task ParentSessionID_Is_Not_Null()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var startTime = DateTimeOffset.Parse("2024-03-27T14:10:00+08:00");

		var fakeLog = NullLogger<GameRoundClient>.Instance;

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["login"] = secureLogin;
		query["password"] = secretKey;
		query["dataType"] = DataType.RNG.ToString();
		query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
		query["options"] = "addRoundDetails";

		var response = "timepoint=1711520400000\r\n" +
			"playerID,extPlayerID,gameID,playSessionID,parentSessionID,startDate,endDate,status,type,bet,win,currency,jackpot\r\n" +
			"94547158,4umn4rhi9bms,vs243lionsgold,37352553134002,37352530320002,2024-03-27 06:18:29,2024-03-27 06:18:29,C,F,0.00,0.00,VND2,0.00";
		var responseContent = new StringContent(response, Encoding.UTF8, "text/csv");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/IntegrationService/v3/DataFeeds/gamerounds/")
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameRoundClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetGameRoundBySlotAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			startTime: startTime);

		Assert.NotNull(actual);
		Assert.Equal("1711520400000", actual.NextTime.ToUnixTimeMilliseconds().ToString());
		Assert.Equal("4umn4rhi9bms", actual.GameRoundDatas.Single().ExtPlayerID);
		Assert.Equal("37352553134002", actual.GameRoundDatas.Single().PlaySessionID);
		Assert.Equal("37352530320002", actual.GameRoundDatas.Single().ParentSessionID);
		Assert.Equal("2024-03-27 06:18:29", actual.GameRoundDatas.Single().StartDate);
		Assert.Equal("2024-03-27 06:18:29", actual.GameRoundDatas.Single().EndDate);
		Assert.Equal("C", actual.GameRoundDatas.Single().Status);
		Assert.Equal("F", actual.GameRoundDatas.Single().Type);
		Assert.Equal(0.00m, actual.GameRoundDatas.Single().Bet);
		Assert.Equal(0.00m, actual.GameRoundDatas.Single().Win);
		Assert.Equal("VND2", actual.GameRoundDatas.Single().Currency);
		Assert.Equal(0.00m, actual.GameRoundDatas.Single().Jackpot);
	}
}
