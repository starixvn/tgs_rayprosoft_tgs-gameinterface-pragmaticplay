using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class GameRecordDetailServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordDetailQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            PlayerCode: "4ucmqplxb029",
            ExternalRoundId: "2914438390",
            ExternalGameId: "vs50aladdin",
            Language: "zh");

        var client = new GameRecordDetailService(
            fakeClient);

        _ = await client.GetAsync(query);

        _ = await fakeClient
            .Received(1)
            .GetGameDetailUrlAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            secureLogin: Arg.Is(query.SecureLogin),
            secretKey: Arg.Is(query.SecretKey),
            externalPlayerId: Arg.Is(query.PlayerCode),
            roundId: Arg.Is(query.ExternalRoundId),
            gameId: Arg.Is(query.ExternalGameId),
            language: Arg.Is(query.Language),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
