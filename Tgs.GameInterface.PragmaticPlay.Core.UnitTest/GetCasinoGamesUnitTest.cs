using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.PragmaticPlay.Core.Exceptions;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetCasinoGamesUnitTest
{
	[Fact]
	public async Task Exception()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz/");
		var secureLogin = "tgs_tgstfw";
		var secretKey = "testKey";
		;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["options"] = "GetFrbDetails,GetLines,GetDataTypes,GetFeatures"
		};

		var hash = "5e997381ee36db3e328485512d5e5435";
		_ = fakeCryptographyUtensil
			.GenerateHash(
				Arg.Any<IDictionary<string, string>>(),
				Arg.Is(secretKey))
			.Returns(hash);

		parameters["hash"] = hash;

		var requestcontent = new FormUrlEncodedContent(parameters);

		var response = "{\"error\":\"2\",\"description\":\"Incorrect secure LOGIN and secure password combination\",\"gameList\":[]}";
		var responseContent = new StringContent(response, Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/getCasinoGames/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<GameInterfaceException>(
			() => client.GetCasinoGamesAsync(
				apiUrl: apiUrl,
				secureLogin: secureLogin,
				secretKey: secretKey)
			.ToArrayAsync()
			.AsTask());
	}

	[Fact]
	public async Task No_Data()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz/");
		var secureLogin = "tgs_tgstfw";
		var secretKey = "testKey";
		;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["options"] = "GetFrbDetails,GetLines,GetDataTypes,GetFeatures"
		};

		var hash = "5e997381ee36db3e328485512d5e5435";
		_ = fakeCryptographyUtensil
			.GenerateHash(
				Arg.Any<IDictionary<string, string>>(),
				Arg.Is(secretKey))
			.Returns(hash);

		parameters["hash"] = hash;

		var requestcontent = new FormUrlEncodedContent(parameters);

		var response = "{\"error\":\"0\",\"description\":\"OK\",\"gameList\":[]}";
		var responseContent = new StringContent(response, Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/getCasinoGames/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetCasinoGamesAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey)
			.ToArrayAsync()
			?? Enumerable.Empty<GetCasinoGamesResult>();

		Assert.NotNull(actual);
		Assert.False(actual.Any());
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz/");
		var secureLogin = "tgs_tgstfw";
		var secretKey = "testKey";
		;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = secureLogin,
			["options"] = "GetFrbDetails,GetLines,GetDataTypes,GetFeatures"
		};

		var hash = "5e997381ee36db3e328485512d5e5435";
		_ = fakeCryptographyUtensil
			.GenerateHash(
				Arg.Any<IDictionary<string, string>>(),
				Arg.Is(secretKey))
			.Returns(hash);

		parameters["hash"] = hash;

		var requestcontent = new FormUrlEncodedContent(parameters);

		var response = "{\"error\":\"0\",\"description\":\"OK\",\"gameList\":[{\"gameID\":\"vs20olympgate\",\"gameName\":\"Gates of Olympus\",\"gameTypeID\":\"vs\",\"typeDescription\":\"Video Slots\",\"technology\":\"html5\",\"platform\":\"MOBILE,DOWNLOAD,WEB\",\"demoGameAvailable\":true,\"aspectRatio\":\"16:9\",\"technologyID\":\"H5\",\"gameIdNumeric\":1605284987,\"jurisdictions\":[\"RS\",\"X1\",\"IT\",\"UA\",\"MT\",\"RO\",\"EE\",\"DE\",\"IM\",\"GR\",\"LV\",\"GG\",\"ZA\",\"99\",\"UK\",\"CO\",\"BG\",\"ES\",\"NL\",\"LT\",\"DK\",\"SE\",\"PT\",\"66\",\"CH\",\"IE\",\"ON\",\"BY\",\"CZ\",\"NO\",\"AT\",\"BE\"],\"frbAvailable\":true,\"variableFrbAvailable\":true,\"lines\":20,\"dataType\":\"RNG\",\"features\":[\"ANTE\",\"BUY\"]},{\"gameID\":\"vpdt11\",\"gameName\":\"Darts\",\"gameTypeID\":\"rgs-vsb\",\"typeDescription\":\"RGS - VSB\",\"technology\":\"html5\",\"platform\":\"MOBILE,DOWNLOAD,WEB\",\"demoGameAvailable\":false,\"aspectRatio\":\"16:9\",\"technologyID\":\"H5\",\"gameIdNumeric\":1644584790,\"jurisdictions\":[\"CO\",\"EE\",\"IE\",\"MT\",\"RO\",\"RS\",\"SE\",\"UA\",\"UK\",\"99\",\"BY\",\"IM\"],\"frbAvailable\":false,\"variableFrbAvailable\":false,\"lines\":1,\"dataType\":\"VSB\",\"features\":[]},{\"gameID\":\"1401\",\"gameName\":\"Live - BOOM CITY\",\"gameTypeID\":\"lg\",\"typeDescription\":\"Live games\",\"technology\":\"html5\",\"platform\":\"MOBILE,DOWNLOAD,WEB\",\"demoGameAvailable\":false,\"aspectRatio\":\"16:9\",\"technologyID\":\"H5\",\"gameIdNumeric\":1401,\"jurisdictions\":[\"99\",\"MT\",\"GG\",\"IE\",\"IM\",\"ON\",\"UA\",\"IT\",\"CH\",\"NL\",\"LT\",\"CO\",\"BY\",\"EE\",\"LV\",\"RS\",\"SE\",\"UK\",\"GR\",\"BG\",\"DK\"],\"frbAvailable\":false,\"variableFrbAvailable\":false,\"lines\":1,\"dataType\":\"LC\",\"features\":[]}]}";
		var responseContent = new StringContent(response, Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/getCasinoGames/")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetCasinoGamesAsync(
			apiUrl: apiUrl,
			secureLogin: secureLogin,
			secretKey: secretKey)
			.ToArrayAsync()
			?? Enumerable.Empty<GetCasinoGamesResult>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
	}
}
