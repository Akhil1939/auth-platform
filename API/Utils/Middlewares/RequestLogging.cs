using System.Diagnostics;
using System.Text.Json;

namespace API.Utils.Middlewares;

public class RequestLogging
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogging> _logger;
    private readonly string _logFilePath = "Logs/api-requests.log";

    public RequestLogging(RequestDelegate next, ILogger<RequestLogging> logger)
    {
        _next = next;
        _logger = logger;

        // Ensure log folder exists
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
    }

    public async Task Invoke(HttpContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        HttpRequest request = context.Request;
        string? origin = context.Connection.RemoteIpAddress?.ToString();
        DateTime startTime = DateTime.UtcNow;

        await _next(context); // Continue the pipeline

        stopwatch.Stop();

        var logEntry = new
        {
            Timestamp = startTime.ToString("u"),
            request.Method,
            request.Path,
            Query = request.QueryString.ToString(),
            Origin = origin,
            context.Response.StatusCode,
            DurationMs = stopwatch.ElapsedMilliseconds
        };

        string logJson = JsonSerializer.Serialize(logEntry);

        await File.AppendAllTextAsync(_logFilePath, logJson + Environment.NewLine);
    }
}