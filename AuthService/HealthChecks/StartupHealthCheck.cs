using Microsoft.Extensions.Diagnostics.HealthChecks;

public class StartupHealthCheck : IHealthCheck
{
    private volatile bool _isReady;
    public bool StartupCompleted {
        get => _isReady;
        set => _isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (StartupCompleted)
            return Task.FromResult(HealthCheckResult.Healthy("Service is READY!"));
        return Task.FromResult(HealthCheckResult.Unhealthy("Service is STARTUP!"));
    }
}
