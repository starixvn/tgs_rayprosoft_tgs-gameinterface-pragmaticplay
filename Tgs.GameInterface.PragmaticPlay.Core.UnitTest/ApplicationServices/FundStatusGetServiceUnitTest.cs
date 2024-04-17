using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeClient = Substitute.For<IGameApiClient>();

		var query = new FundStatusQuery(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			OrderId: "0187989073b00000000107e79a837fe4");

		_ = fakeClient
			.TransferStatusAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				secureLogin: Arg.Is(query.SecureLogin),
				secretKey: Arg.Is(query.SecretKey),
				externalTransactionId: Arg.Is(query.OrderId),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var client = new FundStatusGetService(
			fakeClient);

		var actual = await client.GetAsync(query);

		Assert.True(actual);
	}
}
