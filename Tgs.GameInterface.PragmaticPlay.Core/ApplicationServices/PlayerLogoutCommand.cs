namespace Tgs.GameInterface.PragmaticPlay.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string SecureLogin,
    string SecretKey,
    string PlayerCode);
