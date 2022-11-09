namespace LayerArchitectureExample.DataAccess.Core.Helpers;

using System.Runtime.CompilerServices;

public static class MethodNameHelper
{
    public static string GetCallerName([CallerMemberName] string caller = null)
    {
        return caller;
    }
}
