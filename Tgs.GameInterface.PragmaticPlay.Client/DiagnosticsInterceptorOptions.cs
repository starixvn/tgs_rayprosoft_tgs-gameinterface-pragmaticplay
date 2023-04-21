using System.Diagnostics;

namespace Tgs.GameInterface.PragmaticPlay.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.OBet.Client.DiagnosticsInterceptor");
}
