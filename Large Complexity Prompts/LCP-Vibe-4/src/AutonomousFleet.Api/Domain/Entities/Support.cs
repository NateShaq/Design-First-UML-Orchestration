namespace AutonomousFleet.Api.Domain.Entities;

public record RemoteOperator
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CertificationLevel { get; init; } = "L2";
}

public record RemoteOperatorSupportSession
{
    public Guid Id { get; set; }
    public RemoteOperator Operator { get; set; } = new();
    public string VehicleId { get; set; } = string.Empty;
    public DateTimeOffset StartedAt { get; set; }
    public string Status { get; set; } = "Pending";
}

public record RemoteAssistanceRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public string Reason { get; init; } = "Escalation";
    public string Priority { get; init; } = "Medium";
    public string ContextSnapshotUrl { get; init; } = string.Empty;
}
