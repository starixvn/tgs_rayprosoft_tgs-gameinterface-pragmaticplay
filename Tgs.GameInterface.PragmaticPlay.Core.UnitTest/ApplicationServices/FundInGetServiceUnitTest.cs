using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeClient = Substitute.For<IGameApiClient>();

        var query = new FundInQuery(
            ApiUrl: new Uri("https://api.prerelease-env.biz/"),
            SecureLogin: "secureLogin",
            SecretKey: "secretKey",
            PlayerCode: "4ucmqplxb029",
            OrderId: "0187989073b00000000107e79a837fe4",
            Amount: 14509m);

        var result = new WithdrawalResult(
            226937739,
            0,
            JsonSerializer.Serialize(new
            {
                error = "0",
                description = "OK",
                transactionId = 226937739,
                balance = 0.0m
            }));
        _ = fakeClient
            .WithdrawAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                secureLogin: Arg.Is(query.SecureLogin),
                secretKey: Arg.Is(query.SecretKey),
                externalPlayerId: Arg.Is(query.PlayerCode),
                externalTransactionId: Arg.Is(query.OrderId),
                amount: Arg.Is(query.Amount),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new FundInGetService(
            fakeClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(result.TransactionId, actual.TransactionId);
    }
}
