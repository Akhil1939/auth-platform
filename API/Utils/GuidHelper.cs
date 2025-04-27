namespace API.Utils;

public static class GuidHelper
{
    public static string NewId() => Guid.NewGuid().ToString("N");
}
