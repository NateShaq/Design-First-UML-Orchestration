namespace LcpUml6.Api.Contracts.Requests;

public class CreateMaintenanceScheduleRequest
{
    public Guid? MaintenanceScheduleId { get; set; }
    public DateTime NextServiceDate { get; set; }
    public string? Notes { get; set; }
}
