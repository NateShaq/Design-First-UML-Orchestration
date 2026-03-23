namespace LcpUml6.Api.Contracts.Requests;

public class CreateHydroPlantRequest
{
    public Guid? AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? GridConnectionId { get; set; }
    public Guid? MaintenanceScheduleId { get; set; }
    public string? Notes { get; set; }
}
