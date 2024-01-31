namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GetLobbyGamesViewModel(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey);
