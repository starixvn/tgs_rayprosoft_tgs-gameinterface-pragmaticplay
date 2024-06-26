using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.PragmaticPlay.Controllers;

[Route("pptf")]
[ApiController]
public class PragmaticPlayController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "orderId":"696FDA6D74E14D6998CDDA11E68B2C3C",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public async ValueTask<FundResultViewModel> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundViewModel source)
	{
		var query = new FundInQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		var result = await service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			TransactionId: result.TransactionId,
			Balance: result.Balance,
			Resp: result.Resp);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "orderId":"5331E638E11245C3B321170FEBE378E8",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public async ValueTask<FundResultViewModel> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundViewModel source)
	{
		var query = new FundOutQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		var result = await service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			TransactionId: result.TransactionId,
			Balance: result.Balance,
			Resp: result.Resp);
	}

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "orderId":"5331E638E11245C3B321170FEBE378E8"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusViewModel source)
	{
		var query = new FundStatusQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			OrderId: source.OrderId);

		return service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Games the record by live asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "startTime": "2023-04-21T11:20:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/record/live")]
	public async ValueTask<GameRecordResultViewModel> GameRecordByLiveAsync(
		[FromServices] IGetService<GameRecordByLiveQuery, GameRecordResult> service,
		GameRecordViewModel source)
	{
		var query = new GameRecordByLiveQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			StartTime: source.StartTime);

		var result = await service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new GameRecordResultViewModel(
			NextTime: result.NextTime,
			GameRecordDatas: result.GameRecordDatas.Select(
				x => new GameRecordDataViewModel(
					ExternalRoundId: x.ExternalRoundId,
					ExternalGameId: x.ExternalGameId,
					PlayerCode: x.PlayerCode,
					IsClosed: x.IsClosed,
					IsCancel: x.IsCancel,
					ExternalCreatedOn: x.ExternalCreatedOn,
					ExternalClosedOn: x.ExternalClosedOn,
					Bet: x.Bet,
					Settle: x.Settle,
					WinLoss: x.WinLoss,
					AdditionalData: x.AdditionalData)));
	}

	/// <summary>
	/// Games the record by slot asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "startTime": "2023-04-21T13:30:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/record/slot")]
	public async ValueTask<GameRecordResultViewModel> GameRecordBySlotAsync(
		[FromServices] IGetService<GameRecordBySlotQuery, GameRecordResult> service,
		GameRecordViewModel source)
	{
		var query = new GameRecordBySlotQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			StartTime: source.StartTime);

		var result = await service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new GameRecordResultViewModel(
			NextTime: result.NextTime,
			GameRecordDatas: result.GameRecordDatas.Select(
				x => new GameRecordDataViewModel(
					ExternalRoundId: x.ExternalRoundId,
					ExternalGameId: x.ExternalGameId,
					PlayerCode: x.PlayerCode,
					IsClosed: x.IsClosed,
					IsCancel: x.IsCancel,
					ExternalCreatedOn: x.ExternalCreatedOn,
					ExternalClosedOn: x.ExternalClosedOn,
					Bet: x.Bet,
					Settle: x.Settle,
					WinLoss: x.WinLoss,
					AdditionalData: x.AdditionalData)));
	}

	/// <summary>
	/// Games the record by virtual sports asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "startTime": "2023-04-21T11:55:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/record/virtual-sports")]
	public async ValueTask<GameRecordResultViewModel> GameRecordByVirtualSportsAsync(
		[FromServices] IGetService<GameRecordByVirtualSportsQuery, GameRecordResult> service,
		GameRecordViewModel source)
	{
		var query = new GameRecordByVirtualSportsQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			StartTime: source.StartTime);

		var result = await service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new GameRecordResultViewModel(
			NextTime: result.NextTime,
			GameRecordDatas: result.GameRecordDatas.Select(
				x => new GameRecordDataViewModel(
					ExternalRoundId: x.ExternalRoundId,
					ExternalGameId: x.ExternalGameId,
					PlayerCode: x.PlayerCode,
					IsClosed: x.IsClosed,
					IsCancel: x.IsCancel,
					ExternalCreatedOn: x.ExternalCreatedOn,
					ExternalClosedOn: x.ExternalClosedOn,
					Bet: x.Bet,
					Settle: x.Settle,
					WinLoss: x.WinLoss,
					AdditionalData: x.AdditionalData)));
	}

	/// <summary>
	/// Games the record by virtual sports asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "externalRoundId": "2920181390",
	///             "externalGameId": "vswaysmonkey",
	///             "language": "zh"
	///         }
	/// </remarks>
	[HttpPost("game/record/detail")]
	public ValueTask<string> GameRecordDetailAsync(
		[FromServices] IGetService<GameRecordDetailQuery, string> service,
		GameRecordDetailViewModel source)
	{
		var query = new GameRecordDetailQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			ExternalRoundId: source.ExternalRoundId,
			ExternalGameId: source.ExternalGameId,
			Language: source.Language);

		return service
			.GetAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Games the record cancel round asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "externalRoundId": "2920181390",
	///             "externalGameId": "vswaysmonkey"
	///         }
	/// </remarks>
	[HttpPost("game/record/cancel")]
	public ValueTask GameRecordCancelRoundAsync(
		[FromServices] ICommandService<GameRecordCancelRoundCommand> service,
		GameRecordCancelRoundViewModel source)
		=> service.ExecuteAsync(
		command: new GameRecordCancelRoundCommand(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			ExternalRoundId: source.ExternalRoundId,
			ExternalGameId: source.ExternalGameId),
		cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "externalGameId":"vs20olympgate",
	///             "language": "zh",
	///             "isMobile": false,
	///             "lobbyUrl": "https://stg-t1.rpgp.cc/"
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
		GameUrlQueryViewModel source)
	{
		var query = new GameUrlQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			ExternalGameId: source.ExternalGameId,
			Language: source.Language,
			IsMobile: source.IsMobile,
			LobbyUrl: source.LobbyUrl);

		var result = await service
		 .GetAsync(
			 query: query,
			 cancellationToken: HttpContext.RequestAborted)
		 .ConfigureAwait(false);

		return new GameUrlResultViewModel(
			result.IsPlayerExist,
			result.GameUrl);
	}

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceViewModel source)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode);

		var result = await service
				.GetAsync(
					query: query,
					cancellationToken: HttpContext.RequestAborted)
				.ConfigureAwait(false);

		return new PlayerBalanceResultViewModel(
			result.IsPlayerExist,
			result.Balance);
	}

	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029",
	///             "currencyCode": "CNY"
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateViewModel source)
	{
		var command = new PlayerCreateCommand(
			ApiUrl: source.ApiUrl,
			SecureLogin: source.SecureLogin,
			SecretKey: source.SecretKey,
			PlayerCode: source.PlayerCode,
			CurrencyCode: source.CurrencyCode);
		return service.ExecuteAsync(
				command: command,
				cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Players the kick asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://api.prerelease-env.biz/",
	///             "secureLogin": "tgs_tgstfw",
	///             "secretKey": "087a9b53033942A6",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerKickAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutViewModel source)
		=> service
			.ExecuteAsync(
				command: new PlayerLogoutCommand(
					ApiUrl: source.ApiUrl,
					SecureLogin: source.SecureLogin,
					SecretKey: source.SecretKey,
					PlayerCode: source.PlayerCode),
				cancellationToken: HttpContext.RequestAborted);
}
