namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    string PlayerCode,
    string ExternalGameId,
    string Language,
    bool IsMobile,
    string? LobbyUrl);
