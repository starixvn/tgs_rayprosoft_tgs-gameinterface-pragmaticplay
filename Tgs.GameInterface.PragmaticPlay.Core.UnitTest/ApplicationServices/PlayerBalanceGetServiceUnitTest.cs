using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeClient = Substitute.For<IGameApiClient>();

		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			PlayerCode: "4ucmqplxb029");

		var result = new BalanceResult(
			true,
			100m);
		_ = fakeClient
			.GetBalanceAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				externalPlayerId: Arg.Is(query.PlayerCode),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var client = new PlayerBalanceGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.NotNull(actual);
		Assert.Equal(result.IsPlayerExist, actual.IsPlayerExist);
		Assert.Equal(result.Balance, actual.Balance);
	}
}
