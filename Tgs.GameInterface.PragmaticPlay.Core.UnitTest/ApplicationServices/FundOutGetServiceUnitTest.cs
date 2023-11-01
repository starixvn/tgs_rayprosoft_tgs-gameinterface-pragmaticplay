using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeClient = Substitute.For<IGameApiClient>();

        var query = new FundOutQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            PlayerCode: "4ucmqplxb029",
            OrderId: "018798981ce00000000107e79a837fe4",
            Amount: 4190);

        var result = new DepositResult(
            226938708,
            4190.0m,
            JsonSerializer.Serialize(new
            {
                error = "0",
                description = "OK",
                transactionId = 226938708,
                balance = 4190.0m
            }));
        _ = fakeClient
            .DepositAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                externalPlayerId: Arg.Is(query.PlayerCode),
                externalTransactionId: Arg.Is(query.OrderId),
                amount: Arg.Is(query.Amount),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new FundOutGetService(
            fakeClient);

        var actual = await client.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(result.TransactionId, actual.TransactionId);
        Assert.True(actual.Balance > 0);
    }
}
