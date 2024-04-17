using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetLobbyGamesUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz");
		var secureLogin = "tgs_tgstfw";
		var secretKey = "087a9b53033942A6";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["categories"] = "all"
		};

		var hash = "b4706c2deaed7975ceec2e3eb6f9fb46";
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
			games = new
			{
				all = new[] {
					new
					{
						gameID= "vs20olympgate",
						gameIdNumeric= 1605284987,
						gameName= "Gates of Olympus",
						gameTypeID= "vs",
						jurisdictions=new string[]{
						"RS",
						"X1",
						"IT",
						"UA",
						"MT",
						"RO",
						"EE",
						"DE",
						"IM",
						"GR",
						"LV",
						"GG",
						"ZA",
						"99",
						"UK",
						"CO",
						"BG",
						"ES",
						"NL",
						"LT",
						"DK",
						"SE",
						"PT",
						"66",
						"CH",
						"IE",
						"ON",
						"BY",
						"CZ",
						"NO",
						"AT",
						"BE",
						"HR",
						"HU",
						"SK"
						},
				lines= 20,
				frbAvailable= true,
				variableFrbAvailable= true,
				features=new string[]{
					"ANTE",
					"BUY"
				},
				demoGameAvailable= true,
				sortingOrder= 1
					}
				}
			}
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/getLobbyGames/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetLobbyGamesAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(response.games.all.FirstOrDefault()!.gameID, actual.FirstOrDefault()!.GameID);
	}
}
