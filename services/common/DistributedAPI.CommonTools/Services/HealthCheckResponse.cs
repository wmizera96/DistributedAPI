namespace DistributedAPI.CommonTools.Services;

public abstract record HealthCheckResponse(string Status);

public record LivenessHealthCheckResponse(bool Live, string Status) : HealthCheckResponse(Status);

public record ReadinessHealthCheckResponse(bool Ready, string Status) : HealthCheckResponse(Status);
