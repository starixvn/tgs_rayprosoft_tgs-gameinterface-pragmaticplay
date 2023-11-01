using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class GetGameDetailUrlUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://api.prerelease-env.biz");
        var secureLogin = "secureLogin";
        var secretKey = "secretKey";
        var externalPlayerId = "4uihdiwocwwz";
        var roundId = "2914438390";
        var gameId = "vs50aladdin";
        var language = "zh";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["playerId"] = externalPlayerId,
            ["gameId"] = gameId,
            ["roundId"] = roundId,
            ["language"] = language
        };

        var hash = "58f785625c71ac48d0f6005338413bfb";
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
            url = "https://tgs.prerelease-env.biz/gs2c/parentRoundHistoryDetails.do?playSessionId=2914438390&stylename=tgs_tgstfw&hash=3a8652dfeba3894ab0e6fafe09a69559&language=zh"
        };
        var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, "/IntegrationService/v3/http/HistoryAPI/OpenHistoryExtended/")
            .WithContent(await requestcontent.ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, responseContent);

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeCryptographyUtensil,
            fakeHttpClient);

        var actual = await client.GetGameDetailUrlAsync(
            apiUrl: apiUrl,
            secureLogin: secureLogin,
            secretKey: secretKey,
            externalPlayerId: externalPlayerId,
            roundId: roundId,
            gameId: gameId,
            language: language);

        Assert.Equal(response.url, actual);
    }
}
