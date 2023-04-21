using Tgs.GameInterface.OBet.Protos;
using Tgs.GameInterface.PragmaticPlay.Protos;

namespace Tgs.GameInterface.PragmaticPlay.Client;

public interface IGameInterfacePragmaticPlayClient
{
    ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string orderId,
        CancellationToken cancellationToken = default);



    ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string externalGameId,
        string language,
        bool isMobile,
        string? lobbyUrl,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);
}
