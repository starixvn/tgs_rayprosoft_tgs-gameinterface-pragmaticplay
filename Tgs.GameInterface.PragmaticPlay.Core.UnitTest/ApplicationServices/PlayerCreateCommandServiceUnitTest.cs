using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeClient = Substitute.For<IGameApiClient>();

        var command = new PlayerCreateCommand(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            PlayerCode: "4ucmqplxb029",
            "CNY");

        var result = new BalanceResult(
            true,
            100m);
        _ = fakeClient
            .GetBalanceAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                secureLogin: Arg.Is(command.SecureLogin),
                secretKey: Arg.Is(command.SecretKey),
                externalPlayerId: Arg.Is(command.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new PlayerCreateCommandService(
            fakeClient);

        await client
          .ExecuteAsync(
             command)
         .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .CreatePlayerAsync(
               apiUrl: Arg.Is(command.ApiUrl),
               secureLogin: Arg.Is(command.SecureLogin),
               secretKey: Arg.Is(command.SecretKey),
               externalPlayerId: Arg.Is(command.PlayerCode),
               currencyCode: Arg.Is(command.CurrencyCode),
               cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
