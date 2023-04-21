using System.Diagnostics;

namespace Tgs.GameInterface.PragmaticPlay;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.OBet.DiagnosticsInterceptor");
}
