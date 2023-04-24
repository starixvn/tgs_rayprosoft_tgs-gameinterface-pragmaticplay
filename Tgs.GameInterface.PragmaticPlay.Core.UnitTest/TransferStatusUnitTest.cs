using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.PragmaticPlay.Core.Exceptions;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class TransferStatusUnitTest
{
    [Fact]
    public async Task Exception()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalTransactionId = "018798981ce00000000107e79a837fe4";
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalTransactionId"] = externalTransactionId
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
            transactionId = null as object,
            status = "Not found",
            amount = null as object,
            balance = null as object,
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/status/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        _ = await Assert
            .ThrowsAsync<GameInterfaceException>(() => client
                .TransferStatusAsync(
                    apiUrl: apiUrl,
                    secureLogin: secureLogin,
                    secretKey: secretKey,
                    externalTransactionId: externalTransactionId)
                .AsTask())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalTransactionId = "018798981ce00000000107e79a837fe4";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalTransactionId"] = externalTransactionId,
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
            transactionId = 1908759,
            status = "Success",
            amount = "999.99",
            balance = 999.99
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/status/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
            .TransferStatusAsync(
                apiUrl: apiUrl,
                secureLogin: secureLogin,
                secretKey: secretKey,
                externalTransactionId: externalTransactionId)
            .ConfigureAwait(false);

        Assert.True(actual);
    }
}
