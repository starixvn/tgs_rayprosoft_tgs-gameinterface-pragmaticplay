using System.Diagnostics;

namespace Tgs.GameInterface.PragmaticPlay.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.PragmaticPlay.Client.DiagnosticsInterceptor");
}
