namespace LcpUml6.Api.Contracts.Requests;

public class CreateBatteryStorageRequest
{
    public Guid? AssetId { get; set; }
    public decimal CapacityMwh { get; set; }
    public string Chemistry { get; set; } = "Li-ion";
    public string? Notes { get; set; }
}
