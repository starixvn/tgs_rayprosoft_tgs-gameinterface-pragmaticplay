using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.PragmaticPlay.Protos;

namespace Tgs.GameInterface.PragmaticPlay.Client;

internal class GameInterfacePragmaticPlayClient : IGameInterfacePragmaticPlayClient
{
    private readonly GameInterfacePragmaticPlay.GameInterfacePragmaticPlayClient _client;

    public GameInterfacePragmaticPlayClient(GameInterfacePragmaticPlay.GameInterfacePragmaticPlayClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
            .CheckingServiceAsync(
                request: new CheckingServiceRequest
                {
                    MachineName = machineName
                },
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                OrderId = orderId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                OrderId = orderId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string orderId,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
            .FundStatusAsync(
                request: new FundStatusRequest
                {
                    ApiUrl = apiUrl.ToString(),
                    SecureLogin = secureLogin,
                    SecretKey = secretKey,
                    OrderId = orderId,
                },
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.State;
    }

    public async ValueTask<GameRecordReply> GameRecordByLiveAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
        => await _client
        .GameRecordByLiveAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                StartTime = startTime.ToTimestamp(),
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecordBySlotAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
        => await _client
        .GameRecordBySlotAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                StartTime = startTime.ToTimestamp(),
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecordByVirtualSportsAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
        => await _client
        .GameRecordByVirtualSportsAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                StartTime = startTime.ToTimestamp(),
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string externalGameId,
        string language,
        bool isMobile,
        string? lobbyUrl,
        CancellationToken cancellationToken = default)
        => await _client
        .GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                ExternalGameId = externalGameId,
                Language = language,
                IsMobile = isMobile,
                LobbyUrl = lobbyUrl
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string currencyCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                CurrencyCode = currencyCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerLogoutAsync(
            request: new PlayerLogoutRequest
            {
                ApiUrl = apiUrl.ToString(),
                SecureLogin = secureLogin,
                SecretKey = secretKey,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
