using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetBalanceAsyncUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalPlayerId = "4ucmqplxb029";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
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
            balance = 0.0m
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/current/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
            .GetBalanceAsync(
                apiUrl: apiUrl,
                secureLogin: secureLogin,
                secretKey: secretKey,
                externalPlayerId: externalPlayerId)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(response.balance, actual.Balance);
    }

    [Fact]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalPlayerId = "4ucmqplxb029";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
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
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/current/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
            .GetBalanceAsync(
                apiUrl: apiUrl,
                secureLogin: secureLogin,
                secretKey: secretKey,
                externalPlayerId: externalPlayerId)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
    }
}
