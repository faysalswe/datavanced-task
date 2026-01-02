using System.Diagnostics;
using HealthcareApp.Data;
using HealthcareApp.Models;

namespace HealthcareApp.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            // Only audit API calls
            var path = context.Request.Path.Value ?? "";
            if (path.StartsWith("/api/"))
            {
                var userRole = context.Items["UserRole"]?.ToString() ?? "Anonymous";
                
                var auditLog = new AuditLog
                {
                    UserRole = userRole,
                    Action = context.Request.Method,
                    Endpoint = path,
                    QueryString = context.Request.QueryString.Value ?? "",
                    Timestamp = DateTime.UtcNow,
                    ResponseTime = (int)stopwatch.ElapsedMilliseconds
                };

                dbContext.AuditLogs.Add(auditLog);
                await dbContext.SaveChangesAsync();

                // Log slow queries (> 300ms)
                if (stopwatch.ElapsedMilliseconds > 300)
                {
                    Console.WriteLine($"[SLOW QUERY] {path} took {stopwatch.ElapsedMilliseconds}ms");
                }
            }
        }
    }
}
