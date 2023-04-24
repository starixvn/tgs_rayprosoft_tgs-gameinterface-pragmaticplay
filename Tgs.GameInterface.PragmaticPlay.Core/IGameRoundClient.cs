using Tgs.GameInterface.PragmaticPlay.Core.Models;

namespace Tgs.GameInterface.PragmaticPlay.Core;

public interface IGameRoundClient
{
    ValueTask<GameRoundsResult<GameRoundByLiveData>> GetGameRoundByLiveAsync(
    Uri apiUrl,
    string secureLogin,
    string secretKey,
    DateTimeOffset startTime,
    CancellationToken cancellationToken = default);

    ValueTask<GameRoundsResult<GameRoundBySlotData>> GetGameRoundBySlotAsync(
        Uri apiUrl,
    string secureLogin,
    string secretKey,
    DateTimeOffset startTime,
    CancellationToken cancellationToken = default);

    ValueTask<GameRoundsResult<GameRoundByVirtualSportsData>> GetGameRoundByVirtualSportsAsync(
    Uri apiUrl,
    string secureLogin,
    string secretKey,
    DateTimeOffset startTime,
    CancellationToken cancellationToken = default);
}
