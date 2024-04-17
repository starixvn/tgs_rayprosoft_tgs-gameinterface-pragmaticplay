using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetGameUrlUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var playerId = "4ucmqplxb029";
		var externalGameId = "vs20olympgate";
		var language = "cn";
		var isMobile = true;
		var lobbyUrl = "https://Fskalaslot.com";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["externalPlayerId"] = playerId,
			["gameId"] = externalGameId,
			["language"] = language,
			["platform"] = isMobile ? "MOBILE" : "WEB",
			["lobbyURL"] = lobbyUrl
		};

		var hash = "20700251965b2b47d56254e973f82dcd";
		_ = fakeCryptographyUtensil
			.GenerateHash(
				Arg.Any<IDictionary<string, string>>(),
				Arg.Is(secretKey))
			.Returns(hash);

		parameters["hash"] = hash;

		var requestcontent = new FormUrlEncodedContent(parameters);

		var response = new
		{
			error = "0",
			description = "OK",
			gameURL = "https://tgs.prerelease-env.biz/gs2c/openGame.do?tc=anGBOOa6W12ArakDjjwC5E3jXtDvmj2C21cX1Dlod6MSsLn4Te9n8Wtw0cEGO3kx\u0026stylename=tgs_tgstfw\u0026dummy="
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/game/start/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetGameUrlAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			externalPlayerId: playerId,
			externalGameId: externalGameId,
			language: language,
			isMobile: isMobile,
			lobbyUrl: lobbyUrl);

		Assert.NotNull(actual);
		Assert.Equal(response.gameURL, actual.GameUrl);
	}

	[Fact]
	public async Task Player_Does_Not_Exist()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "secureLogin";
		var secretKey = "secretKey";
		var playerId = "4ucmqplxb029";
		var externalGameId = "vs20olympgate";
		var language = "cn";
		var isMobile = true;

		var lobbyUrl = "https://Fskalaslot.com";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["externalPlayerId"] = playerId,
			["gameId"] = externalGameId,
			["language"] = language,
			["platform"] = isMobile ? "MOBILE" : "WEB",
			["lobbyURL"] = lobbyUrl
		};

		var hash = "20700251965b2b47d56254e973f82dcd";
		_ = fakeCryptographyUtensil
			.GenerateHash(
				Arg.Any<IDictionary<string, string>>(),
				Arg.Is(secretKey))
			.Returns(hash);

		parameters["hash"] = hash;

		var requestcontent = new FormUrlEncodedContent(parameters);

		var response = new
		{
			error = "17",
			description = "Player not found",
			gameURL = null as object
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/game/start/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetGameUrlAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey,
			externalPlayerId: playerId,
			externalGameId: externalGameId,
			language: language,
			isMobile: isMobile,
			lobbyUrl: lobbyUrl);

		Assert.NotNull(actual);
		Assert.False(actual.IsPlayerExist);
	}
}
