using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            PlayerCode: "4ucmqplxb029",
            ExternalGameId: "vs20olympgate",
            Language: "cn",
            IsMobile: true,
            LobbyUrl: "https://Fskalaslot.com");

        var result = new Models.GameUrlResult(
            IsPlayerExist: true,
            GameUrl: "https://tgs.prerelease-env.biz/gs2c/openGame.do?tc=anGBOOa6W12ArakDjjwC5E3jXtDvmj2C21cX1Dlod6MSsLn4Te9n8Wtw0cEGO3kx\u0026stylename=tgs_tgstfw\u0026dummy=");
        _ = fakeClient
            .GetGameUrlAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                externalPlayerId: Arg.Is(query.PlayerCode),
                externalGameId: Arg.Is(query.ExternalGameId),
                language: Arg.Is(query.Language),
                isMobile: Arg.Is(query.IsMobile),
                lobbyUrl: Arg.Is(query.LobbyUrl),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameUrlGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.IsPlayerExist, actual.IsPlayerExist);
        Assert.Equal(result.GameUrl, actual.GameUrl);
    }
}
