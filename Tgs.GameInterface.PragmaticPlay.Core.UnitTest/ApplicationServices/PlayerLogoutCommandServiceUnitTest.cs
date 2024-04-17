using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeClient = Substitute.For<IGameApiClient>();

		var command = new PlayerLogoutCommand(
			ApiUrl: new Uri("https://api.prerelease-env.biz/"),
			SecureLogin: "secureLogin",
			SecretKey: "secretKey",
			PlayerCode: "4ucmqplxb029");

		var client = new PlayerLogoutCommandService(
			fakeClient);

		await client.ExecuteAsync(command);

		await fakeClient
			.Received(1)
			.LogoutAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			secureLogin: Arg.Is(command.SecureLogin),
			secretKey: Arg.Is(command.SecretKey),
			externalPlayerId: Arg.Is(command.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
