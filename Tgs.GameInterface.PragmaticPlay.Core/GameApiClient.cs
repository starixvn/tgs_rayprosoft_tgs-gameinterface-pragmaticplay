using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.PragmaticPlay.Core.Exceptions;
using Tgs.GameInterface.PragmaticPlay.Core.Extensions;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core;

internal class GameApiClient(
    ILogger<GameApiClient> logger,
    ICryptographyUtensil cryptographyUtensil,
    HttpClient client) : IGameApiClient
{
    public async ValueTask CreatePlayerAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["currency"] = currencyCode
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/player/account/create/",
                parameters: parameters,
                logPrefix: "CreatePlayer",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!jsonNode["error"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());
        }
    }

    public async ValueTask<DepositResult> DepositAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalTransactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["externalTransactionId"] = externalTransactionId,
            ["amount"] = (amount).ToString()
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/",
                parameters: parameters,
                logPrefix: " Deposit",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return jsonNode["error"]!.GetValue<string>() != "0"
            ? throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString())
            : new DepositResult(
            TransactionId: jsonNode["transactionId"]!.GetValue<long>(),
            Balance: jsonNode["balance"]!.GetValue<decimal>(),
            Res: jsonNode.ToJsonString());
    }

    public async ValueTask<BalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/balance/current/",
                parameters: parameters,
                logPrefix: "GetBalance",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!jsonNode["error"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            return jsonNode["error"]!.GetValue<string>().Equals("17", StringComparison.OrdinalIgnoreCase)
                ? new BalanceResult(
                    IsPlayerExist: false,
                    Balance: 0.0m)
                : throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());
        }

        var balance = jsonNode["balance"]!.GetValue<decimal>();

        return new BalanceResult(
            IsPlayerExist: true,
            Balance: balance);
    }

    public async IAsyncEnumerable<GetCasinoGamesResult> GetCasinoGamesAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["options"] = "GetFrbDetails,GetLines,GetDataTypes,GetFeatures"
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/getCasinoGames/",
                parameters: parameters,
                logPrefix: " GetCasinoGames",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (jsonNode["error"]!.GetValue<string>() != "0")
        {
            throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());
        }

        var gameList = jsonNode["gameList"]!.AsArray();

        if (gameList.Count == 0)
        {
            yield break;
        }

        foreach (var game in gameList)
        {
            if (game is null)
            {
                continue;
            }
            yield return new GetCasinoGamesResult
            {
                GameID = game["gameID"]!.GetValue<string>(),
                GameName = game["gameName"]!.GetValue<string>(),
                GameTypeID = game["gameTypeID"]!.GetValue<string>(),
                TypeDescription = game["typeDescription"]!.GetValue<string>(),
                Technology = game["technology"]!.GetValue<string>(),
                Platform = game["platform"]!.GetValue<string>(),
                DemoGameAvailable = game["demoGameAvailable"]!.GetValue<bool>(),
                GameIdNumeric = game["gameIdNumeric"]!.GetValue<int>(),
                DataType = game["dataType"]!.GetValue<string>()
            };
        }
    }

    public async ValueTask<string> GetGameDetailUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string roundId,
        string gameId,
        string language,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["playerId"] = externalPlayerId,
            ["gameId"] = gameId,
            ["roundId"] = roundId,
            ["language"] = language
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/HistoryAPI/OpenHistoryExtended/",
                parameters: parameters,
                logPrefix: "OpenHistoryExtended",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["error"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString())
            : jsonNode["url"]!.GetValue<string>();
    }

    public async ValueTask<GameUrlResult> GetGameUrlAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalGameId,
        string language,
        bool isMobile,
        string? lobbyUrl,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["gameId"] = externalGameId,
            ["language"] = language,
            ["platform"] = isMobile ? "MOBILE" : "WEB"
        };

        if (!string.IsNullOrWhiteSpace(lobbyUrl))
            parameters["lobbyURL"] = lobbyUrl;

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/game/start/",
                parameters: parameters,
                logPrefix: "StartGame",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!jsonNode["error"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            return jsonNode["error"]!.GetValue<string>().Equals("17", StringComparison.OrdinalIgnoreCase)
                ? new GameUrlResult(
                    IsPlayerExist: false,
                    GameUrl: string.Empty)
                : throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());
        }

        var gameUrl = jsonNode["gameURL"]!.GetValue<string>()
            ?? throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());

        return new GameUrlResult(
            IsPlayerExist: true,
            GameUrl: gameUrl);
    }

    public async IAsyncEnumerable<GetLobbyGamesResult> GetLobbyGamesAsync(Uri apiUrl, string secureLogin, string secretKey, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["categories"] = "all"
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/getLobbyGames/",
                parameters: parameters,
                logPrefix: " GetLobbyGames",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (jsonNode["error"]!.GetValue<string>() != "0")
        {
            throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString());
        }

        var gameList = jsonNode["games"]!["all"]!.AsArray();

        if (gameList.Count == 0)
        {
            yield break;
        }

        foreach (var game in gameList)
        {
            if (game is null)
            {
                continue;
            }
            yield return new GetLobbyGamesResult
            {
                GameID = game["gameID"]!.GetValue<string>(),
                GameIdNumeric = game["gameIdNumeric"]!.GetValue<int>(),
                GameName = game["gameName"]!.GetValue<string>(),
                GameTypeID = game["gameTypeID"]!.GetValue<string>(),
                Jurisdictions = String.Join(",", JsonSerializer.Deserialize<IEnumerable<string>>(game["jurisdictions"]) ?? Enumerable.Empty<string>()),
                Lines = game["gameIdNumeric"]!.GetValue<int>(),
                FrbAvailable = game["frbAvailable"]!.GetValue<bool>(),
                VariableFrbAvailable = game["variableFrbAvailable"]!.GetValue<bool>(),
                Features = String.Join(",", JsonSerializer.Deserialize<IEnumerable<string>>(game["features"]) ?? Enumerable.Empty<string>()),
                DemoGameAvailable = game["demoGameAvailable"]!.GetValue<bool>(),
                SortingOrder = game["sortingOrder"]!.GetValue<int>(),
            };
        }
    }

    public async ValueTask LogoutAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        _ = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/game/session/terminate/",
                parameters: parameters,
                logPrefix: "Logout",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<bool> TransferStatusAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalTransactionId,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalTransactionId"] = externalTransactionId
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/status/",
                parameters: parameters,
                logPrefix: "FundOrderChecking",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return jsonNode["error"]!.ToString() != "0"
            || !jsonNode["status"]!.GetValue<string>().Equals("Success", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString())
            : true;
    }

    public async ValueTask<WithdrawalResult> WithdrawAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        string externalPlayerId,
        string externalTransactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["secureLogin"] = secureLogin,
            ["externalPlayerId"] = externalPlayerId,
            ["externalTransactionId"] = externalTransactionId,
            ["amount"] = (-amount).ToString()
        };

        var hash = cryptographyUtensil.GenerateHash(parameters, secretKey);
        parameters["hash"] = hash;

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "/IntegrationService/v3/http/CasinoGameAPI/balance/transfer/",
                parameters: parameters,
                logPrefix: " Withdraw",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return jsonNode["error"]!.GetValue<string>() != "0"
            ? throw new GameInterfaceException(
                error: jsonNode["error"]!.GetValue<string>(),
                description: jsonNode["description"]?.GetValue<string>() ?? string.Empty,
                json: jsonNode.ToJsonString())
            : new WithdrawalResult(
            TransactionId: jsonNode["transactionId"]!.GetValue<long>(),
            Balance: jsonNode["balance"]!.GetValue<decimal>(),
            Res: jsonNode.ToJsonString());
    }

    private async ValueTask<JsonNode> PostAsync(
        Uri apiUrl,
        string path,
        IEnumerable<KeyValuePair<string, string>> parameters,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();
        var url = new Uri(apiUrl, path);

        var sw = Stopwatch.StartNew();

        var content = new FormUrlEncodedContent(parameters);
        var httpResponseMessage = await client
            .PostAsync(
                requestUri: url,
                content: content,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers
                             + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
