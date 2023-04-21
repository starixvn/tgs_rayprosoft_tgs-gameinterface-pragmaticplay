using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core;

public interface IGameApiClient
{
    ValueTask CreatePlayerAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<DepositResult> DepositAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalTransactionId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<BalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        CancellationToken cancellationToken = default);

    ValueTask<string> GetGameDetailUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string roundId,
        string gameId,
        string language,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlResult> GetGameUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalGameId,
        string language,
        bool isMobile,
        string? lobbyUrl,
        CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        CancellationToken cancellationToken = default);

    ValueTask<bool> TransferStatusAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalTransactionId,
        CancellationToken cancellationToken = default);

    ValueTask<WithdrawalResult> WithdrawAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalTransactionId,
        decimal amount,
        CancellationToken cancellationToken = default);
}
