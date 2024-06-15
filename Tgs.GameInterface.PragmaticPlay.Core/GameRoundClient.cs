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

internal class GameRoundClient(
	ILogger<GameRoundClient> logger,
	HttpClient client) : IGameRoundClient
{
	public async ValueTask<GameRoundsResult<GameRoundByLiveData>> GetGameRoundByLiveAsync(Uri apiUrl, string secureLogin, string secretKey, DateTimeOffset startTime, CancellationToken cancellationToken = default)
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
			logPrefix: nameof(GetGameRoundByLiveAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
		{
			logger.LogWarning("The game provider responded with an error.({data})", data);
			return new GameRoundsResult<GameRoundByLiveData>(
				DateTimeOffset.UnixEpoch,
				[]);
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

	public async ValueTask<GameRoundsResult<GameRoundBySlotData>> GetGameRoundBySlotAsync(Uri apiUrl, string secureLogin, string secretKey, DateTimeOffset startTime, CancellationToken cancellationToken = default)
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
			logPrefix: nameof(GetGameRoundBySlotAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
		{
			logger.LogWarning("The game provider responded with an error.({data})", data);
			return new GameRoundsResult<GameRoundBySlotData>(
				DateTimeOffset.UnixEpoch,
				[]);
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

	public async ValueTask<GameRoundsResult<GameRoundByVirtualSportsData>> GetGameRoundByVirtualSportsAsync(Uri apiUrl, string secureLogin, string secretKey, DateTimeOffset startTime, CancellationToken cancellationToken = default)
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
			logPrefix: nameof(GetGameRoundByVirtualSportsAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (string.IsNullOrEmpty(data) || !data.StartsWith("timepoint"))
		{
			logger.LogWarning("The game provider responded with an error.({data})", data);
			return new GameRoundsResult<GameRoundByVirtualSportsData>(
				DateTimeOffset.UnixEpoch,
				[]);
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

	private async ValueTask<string?> GetCsvAsync(Uri apiUrl, string logPrefix, CancellationToken cancellationToken = default)
	{
		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client.GetAsync(
			requestUri: apiUrl,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};

		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return responseMessage;
	}
}
