using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.PragmaticPlay.Core;
using Tgs.GameInterface.PragmaticPlay.Core.Models;
using Tgs.GameInterface.PragmaticPlay.ViewModels;

namespace Tgs.GameInterface.PragmaticPlay.Controllers;

[Route("[controller]")]
[ApiController]
public class ExportController : ControllerBase
{
    /// <summary>
    /// Gets the casino games asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.prerelease-env.biz/",
    ///             "secureLogin": "tgs_tgstfw",
    ///             "secretKey": "testKey"
    ///         }
    /// </remarks>
    [Produces("text/csv")]
    [HttpPost]
    public async ValueTask<ActionResult> GetCasinoGamesAsync(
        [FromServices] IGameApiClient client,
        GetCasinoGamesViewModel source)
    {
        var result = await client.GetCasinoGamesAsync(
            apiUrl: source.ApiUrl,
            secureLogin: source.SecureLogin,
            secretKey: source.SecretKey,
            cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                 cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
        using MemoryStream stream = new MemoryStream();
        using TextWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
        using CsvWriter csv = new CsvWriter(textWriter, config);
        csv.WriteHeader<GetCasinoGamesResult>();
        csv.NextRecord();
        csv.WriteRecords(result);
        textWriter.Flush();
        return File(
            fileContents: stream.ToArray(),
            contentType: "text/csv",
            fileDownloadName: $"PP_CasinoGames_{DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(8)):yyyyMMddHHmmss}.csv");
    }
}
