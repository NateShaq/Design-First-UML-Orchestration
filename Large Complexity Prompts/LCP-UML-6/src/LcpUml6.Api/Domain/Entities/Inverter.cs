using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class Inverter
{
    [Key]
    public Guid InverterId { get; set; }

    [MaxLength(120)]
    public string Model { get; set; } = string.Empty;

    [MaxLength(50)]
    public string FirmwareVersion { get; set; } = "1.0.0";
}
