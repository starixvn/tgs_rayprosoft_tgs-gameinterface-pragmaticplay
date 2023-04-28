namespace Tgs.GameInterface.PragmaticPlay.ViewModels;

public record GetCasinoGamesViewModel(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey);
