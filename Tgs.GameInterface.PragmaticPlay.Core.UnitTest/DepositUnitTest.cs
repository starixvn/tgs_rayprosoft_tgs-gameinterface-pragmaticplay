using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.PragmaticPlay.Core.Exceptions;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class DepositUnitTest
{
    [Fact]
    public async Task Exception()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalPlayerId = "4ucmqplxb029";
        var externalTransactionId = "018798981ce00000000107e79a837fe4";
        var amount = 4190;

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["externalTransactionId"] = externalTransactionId,
            ["amount"] = amount.ToString()
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
            error = "8",
            description = "交易已存在",
            transactionId = null as object,
            balance = null as object,
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        _ = await Assert
            .ThrowsAsync<GameInterfaceException>(() => client
                .DepositAsync(
                    apiUrl: apiUrl,
                    secureLogin: secureLogin,
                    secretKey: secretKey,
                    externalPlayerId: externalPlayerId,
                    externalTransactionId: externalTransactionId,
                    amount: amount)
                .AsTask())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalPlayerId = "4ucmqplxb029";
        var externalTransactionId = "018798981ce00000000107e79a837fe4";
        var amount = 4190;

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["externalTransactionId"] = externalTransactionId,
            ["amount"] = amount.ToString()
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
            transactionId = 226938708,
            balance = 4190.0m
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/")
            .WithContent(await requestcontent.ReadAsStringAsync().ConfigureAwait(false))
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client
            .DepositAsync(
                apiUrl: apiUrl,
                secureLogin: secureLogin,
                secretKey: secretKey,
                externalPlayerId: externalPlayerId,
                externalTransactionId: externalTransactionId,
                amount: amount)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(response.transactionId, actual.TransactionId);
        Assert.Equal(response.balance, actual.Balance);
    }
}
