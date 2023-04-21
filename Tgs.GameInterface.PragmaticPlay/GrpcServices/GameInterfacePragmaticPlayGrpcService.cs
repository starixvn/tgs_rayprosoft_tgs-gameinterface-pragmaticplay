using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;
using Tgs.GameInterface.PragmaticPlay.Protos;
using Tgs.ServiceUtility;
using GameRecordData = Tgs.GameInterface.PragmaticPlay.Protos.GameRecordData;

namespace Tgs.GameInterface.PragmaticPlay.GrpcServices
{
    public class GameInterfacePragmaticPlayGrpcService : GameInterfacePragmaticPlay.GameInterfacePragmaticPlayBase
    {
        private readonly IGetService<FundInQuery, FundResult> _fundInService;
        private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IGetService<GameRecordByLiveQuery, GameRecordResult> _gameRecordByLiveService;
        private readonly IGetService<GameRecordBySlotQuery, GameRecordResult> _gameRecordBySlotService;
        private readonly IGetService<GameRecordByVirtualSportsQuery, GameRecordResult> _gameRecordByVirtualSportsService;
        private readonly IGetService<GameRecordDetailQuery, string> _gameRecordDetailService;
        private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
        private readonly ILogger<GameInterfacePragmaticPlayGrpcService> _logger;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
        private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

        public GameInterfacePragmaticPlayGrpcService(
            ILogger<GameInterfacePragmaticPlayGrpcService> logger,
            IGetService<FundInQuery, FundResult> fundInService,
            IGetService<FundOutQuery, FundResult> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IGetService<GameRecordByLiveQuery, GameRecordResult> gameRecordByLiveService,
            IGetService<GameRecordBySlotQuery, GameRecordResult> gameRecordBySlotService,
            IGetService<GameRecordByVirtualSportsQuery, GameRecordResult> gameRecordByVirtualSportsService,
            IGetService<GameRecordDetailQuery, string> gameRecordDetailService,
            IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerCreateCommand> playerCreateService,
            ICommandService<PlayerLogoutCommand> PlayerLogoutService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameRecordByLiveService = gameRecordByLiveService ?? throw new ArgumentNullException(nameof(gameRecordByLiveService));
            _gameRecordBySlotService = gameRecordBySlotService ?? throw new ArgumentNullException(nameof(gameRecordBySlotService));
            _gameRecordByVirtualSportsService = gameRecordByVirtualSportsService ?? throw new ArgumentNullException(nameof(gameRecordByVirtualSportsService));
            _gameRecordDetailService = gameRecordDetailService ?? throw new ArgumentNullException(nameof(gameRecordDetailService));
            _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
            _playerLogoutService = PlayerLogoutService ?? throw new ArgumentNullException(nameof(PlayerLogoutService));
        }

        public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
        {
            _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

            return Task.FromResult(new CheckingServiceReply
            {
                MachineName = Environment.MachineName
            });
        }

        public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
        {
            var query = new FundInQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                OrderId: request.OrderId,
                Amount: request.Amount);

            var result = await _fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TransactionId = result.TransactionId,
                Balance = result.Balance,
                Res = result.Res
            };
        }

        public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
        {
            var query = new FundOutQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                OrderId: request.OrderId,
                Amount: request.Amount);

            var result = await _fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TransactionId = result.TransactionId,
                Balance = result.Balance,
                Res = result.Res
            };
        }

        public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                OrderId: request.OrderId);

            var result = await _fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = result
            };
        }

        public override async Task<GameRecordReply> GameRecordByLive(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordByLiveQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                StartTime: request.StartTime.ToDateTimeOffset());

            var result = await _gameRecordByLiveService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            var reply = new GameRecordReply()
            {
                NextTime = result.NextTime.ToTimestamp()
            };
            if (result.GameRecordDatas.Any())
            {
                reply.Items.AddRange(result.GameRecordDatas.Select(
                    x => new GameRecordData
                    {
                        ExternalRoundId = x.ExternalRoundId,
                        PlayerCode = x.PlayerCode,
                        IsClosed = x.IsClosed,
                        ExternalCreatedOn = x.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = x.ExternalClosedOn.ToTimestamp(),
                        Bet = x.Bet,
                        Settle = x.Settle,
                        WinLoss = x.WinLoss,
                        AdditionalData = x.AdditionalData
                    }
                    ));
            }

            return reply;
        }

        public override async Task<GameRecordReply> GameRecordBySlot(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordBySlotQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                StartTime: request.StartTime.ToDateTimeOffset());

            var result = await _gameRecordBySlotService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            var reply = new GameRecordReply()
            {
                NextTime = result.NextTime.ToTimestamp()
            };
            if (result.GameRecordDatas.Any())
            {
                reply.Items.AddRange(result.GameRecordDatas.Select(
                    x => new GameRecordData
                    {
                        ExternalRoundId = x.ExternalRoundId,
                        PlayerCode = x.PlayerCode,
                        IsClosed = x.IsClosed,
                        ExternalCreatedOn = x.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = x.ExternalClosedOn.ToTimestamp(),
                        Bet = x.Bet,
                        Settle = x.Settle,
                        WinLoss = x.WinLoss,
                        AdditionalData = x.AdditionalData
                    }
                    ));
            }

            return reply;
        }

        public override async Task<GameRecordReply> GameRecordByVirtualSports(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordByVirtualSportsQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                StartTime: request.StartTime.ToDateTimeOffset());

            var result = await _gameRecordByVirtualSportsService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            var reply = new GameRecordReply()
            {
                NextTime = result.NextTime.ToTimestamp()
            };
            if (result.GameRecordDatas.Any())
            {
                reply.Items.AddRange(result.GameRecordDatas.Select(
                    x => new GameRecordData
                    {
                        ExternalRoundId = x.ExternalRoundId,
                        PlayerCode = x.PlayerCode,
                        IsClosed = x.IsClosed,
                        ExternalCreatedOn = x.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = x.ExternalClosedOn.ToTimestamp(),
                        Bet = x.Bet,
                        Settle = x.Settle,
                        WinLoss = x.WinLoss,
                        AdditionalData = x.AdditionalData
                    }
                    ));
            }

            return reply;
        }

        public override async Task<GameRecordDetailReply> GameRecordDetail(GameRecordDetailRequest request, ServerCallContext context)
        {
            var query = new GameRecordDetailQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                ExternalRoundId: request.ExternalRoundId,
                ExternalGameId: request.ExternalGameId,
                Language: request.Language);

            var result = await _gameRecordDetailService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            return new GameRecordDetailReply
            {
                Url = result
            };
        }

        public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
        {
            var query = new GameUrlQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                ExternalGameId: request.ExternalGameId,
                Language: request.Language,
                IsMobile: request.IsMobile,
                LobbyUrl: request.LobbyUrl);

            var result = await _gameUrlService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            return new GameUrlReply
            {
                IsPlayerExist = result.IsPlayerExist,
                GameUrl = result.GameUrl
            };
        }

        public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode);

            var result = await _playerBalanceService
             .GetAsync(
                 query: query,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            return new PlayerBalanceReply
            {
                IsPlayerExist = result.IsPlayerExist,
                Balance = result.Balance
            };
        }

        public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                CurrencyCode: request.CurrencyCode);

            await _playerCreateService
             .ExecuteAsync(
                 command: command,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }

        public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
        {
            var command = new PlayerLogoutCommand(
                ApiUrl: new(request.ApiUrl),
                SecureLogin: request.SecureLogin,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode);

            await _playerLogoutService
             .ExecuteAsync(
                 command: command,
                 cancellationToken: context.CancellationToken)
             .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}
