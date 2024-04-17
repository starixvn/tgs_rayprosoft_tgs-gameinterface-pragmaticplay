using System.Reflection;
using Tgs.GameInterface.PragmaticPlay.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceCore(
		this IServiceCollection services)
		=> services
		.AddHttpClient<IGameApiClient, GameApiClient>()
		.Services
		.AddHttpClient<IGameRoundClient, GameRoundClient>()
		.Services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddSingleton<ICryptographyUtensil, CryptographyUtensil>();
}
