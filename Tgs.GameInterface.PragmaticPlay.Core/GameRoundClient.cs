using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.PragmaticPlay.Core.Extensions;
using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core;

internal class GameRoundClient : IGameRoundClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameRoundClient> _logger;

    public GameRoundClient(
        ILogger<GameRoundClient> logger,
        HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<GameRoundsResult<GameRoundByLiveData>> GetGameRoundByLiveAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.LC.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var url = new Uri(apiUrl, $"/IntegrationService/v3/DataFeeds/gamerounds/?{query}");

        var data = await GetCsvAsync(
                apiUrl: url,
                logPrefix: $"GameRounds.{query["dataType"]!}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
        {
            _logger.LogWarning("The game provider responded with an error.({data})", data);
            return new GameRoundsResult<GameRoundByLiveData>(
                DateTimeOffset.UnixEpoch,
                Enumerable.Empty<GameRoundByLiveData>());
        }

        using var stringReader = new StringReader(data);
        var firstLine = await stringReader
            .ReadLineAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var timepoint = firstLine!.Split('=')[1];
        var nextTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timepoint));

        using var csvReader = new CsvReader(stringReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });

        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("null");

        var gameRecords = csvReader
            .GetRecords<GameRoundByLiveData>().ToArray();

        return new GameRoundsResult<GameRoundByLiveData>(nextTime, gameRecords);
    }

    public async ValueTask<GameRoundsResult<GameRoundBySlotData>> GetGameRoundBySlotAsync(
            Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.RNG.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var url = new Uri(apiUrl, $"/IntegrationService/v3/DataFeeds/gamerounds/?{query}");

        var data = await GetCsvAsync(
                apiUrl: url,
                logPrefix: $"GameRounds.{query["dataType"]!}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
        {
            _logger.LogWarning("The game provider responded with an error.({data})", data);
            return new GameRoundsResult<GameRoundBySlotData>(
                DateTimeOffset.UnixEpoch,
                Enumerable.Empty<GameRoundBySlotData>());
        }

        using var stringReader = new StringReader(data);
        var firstLine = await stringReader
            .ReadLineAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var timepoint = firstLine!.Split('=')[1];
        var nextTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timepoint));

        using var csvReader = new CsvReader(stringReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });

        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("null");

        var gameRecords = csvReader
            .GetRecords<GameRoundBySlotData>().ToArray();

        return new GameRoundsResult<GameRoundBySlotData>(nextTime, gameRecords);
    }

    public async ValueTask<GameRoundsResult<GameRoundByVirtualSportsData>> GetGameRoundByVirtualSportsAsync(
        Uri apiUrl,
        string secureLogin,
        string secretKey,
        DateTimeOffset startTime,
        CancellationToken cancellationToken = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["login"] = secureLogin;
        query["password"] = secretKey;
        query["dataType"] = DataType.VSB.ToString();
        query["timepoint"] = startTime.ToUnixTimeMilliseconds().ToString();
        query["options"] = "addRoundDetails";

        var url = new Uri(apiUrl, $"/IntegrationService/v3/DataFeeds/gamerounds/?{query}");

        var data = await GetCsvAsync(
                apiUrl: url,
                logPrefix: $"GameRounds.{query["dataType"]!}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
        {
            _logger.LogWarning("The game provider responded with an error.({data})", data);
            return new GameRoundsResult<GameRoundByVirtualSportsData>(
                DateTimeOffset.UnixEpoch,
                Enumerable.Empty<GameRoundByVirtualSportsData>());
        }

        using var stringReader = new StringReader(data);
        var firstLine = await stringReader
            .ReadLineAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var timepoint = firstLine!.Split('=')[1];
        var nextTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timepoint));

        using var csvReader = new CsvReader(stringReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });

        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("null");

        var gameRecords = csvReader
            .GetRecords<GameRoundByVirtualSportsData>().ToArray();

        return new GameRoundsResult<GameRoundByVirtualSportsData>(nextTime, gameRecords);
    }

    private async ValueTask<string?> GetCsvAsync(
        Uri apiUrl,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var httpResponseMessage = await _client
        .GetAsync(
                requestUri: apiUrl,
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
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return responseMessage;
    }
}
