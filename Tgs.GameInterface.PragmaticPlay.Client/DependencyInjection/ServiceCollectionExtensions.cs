using Microsoft.Extensions.Options;
using Tgs.GameInterface.PragmaticPlay.Client;
using Tgs.GameInterface.PragmaticPlay.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfacePragmaticPlayClient(
        this IServiceCollection services,
        Action<GameInterfacePragmaticPlayClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfacePragmaticPlayClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfacePragmaticPlay.GameInterfacePragmaticPlayClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfacePragmaticPlayClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };

                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfacePragmaticPlayClient, GameInterfacePragmaticPlayClient>();
}
