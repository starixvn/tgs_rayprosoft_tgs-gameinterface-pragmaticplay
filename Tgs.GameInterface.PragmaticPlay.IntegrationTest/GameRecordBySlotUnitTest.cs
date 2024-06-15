using System.Text.Json;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.IntegrationTest;
public class GameRecordBySlotUnitTest
{
	[Fact]
	public async Task IsCancel()
	{
		var apiUrl = new Uri("https://api.prerelease-env.biz/");
		var secureLogin = "pgpx_mopx";
		var secretKey = "8ftvq9qETZkGy2Px";
		var startTime = new DateTimeOffset(2024, 06, 04, 13, 25, 00, TimeSpan.FromHours(+8));

		var gameRecordResult = new GameRecordResult(
			NextTime: DateTimeOffset.FromUnixTimeMilliseconds(1717480200000),
			GameRecordDatas: [
				new (
					ExternalRoundId: "620757986000",
					ExternalGameId: "vswayscharms",
					PlayerCode: "4udvzfxyzowl",
					IsClosed: true,
					IsCancel: true,
					ExternalCreatedOn: new DateTimeOffset( DateTime.Parse("2024-06-04 05:25:25"),TimeSpan.Zero),
					ExternalClosedOn: new DateTimeOffset( DateTime.Parse("2024-06-04 05:25:25"),TimeSpan.Zero),
					Bet: 6.0m,
					Settle: 0.0m,
					WinLoss: -6.0m,
					AdditionalData: JsonSerializer.Serialize(new{
						playSessionID=620757986000,
						parentSessionID=default(string)})
					)
			]);

		var web = new GameInterfacePragmaticPlayApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameRecordBySlotQuery, GameRecordResult>>();

			var query = new GameRecordBySlotQuery(
				ApiUrl: apiUrl,
				SecureLogin: secureLogin,
				SecretKey: secretKey,
				StartTime: startTime);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameRecordResult);

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfacePragmaticPlayClient(new Protos.GameInterfacePragmaticPlay.GameInterfacePragmaticPlayClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameRecordBySlotAsync(apiUrl, secureLogin, secretKey, startTime);

		Assert.NotNull(actual);
		Assert.Equal(gameRecordResult.NextTime, actual.NextTime.ToDateTimeOffset());
		var data = actual.Items.First();
		Assert.NotNull(data);
		Assert.Equal("4udvzfxyzowl", data.PlayerCode);
		Assert.Equal("620757986000", data.ExternalRoundId);
		Assert.Equal("vswayscharms", data.ExternalGameId);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-04 05:25:25"), TimeSpan.Zero), data.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-04 05:25:25"), TimeSpan.Zero), data.ExternalClosedOn.ToDateTimeOffset());
		Assert.True(data.IsClosed);
		Assert.True(data.IsCancel);
		Assert.Equal(6.0m, data.Bet.ToDecimal().GetValueOrDefault(0.0m));
		Assert.Equal(0.0m, data.Settle.ToDecimal().GetValueOrDefault(0.0m));
		Assert.Equal(-6.0m, data.WinLoss.ToDecimal().GetValueOrDefault(0.0m));
		Assert.NotEmpty(data.AdditionalData);
	}
}
