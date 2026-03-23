using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class WeatherStation
{
    [Key]
    public Guid WeatherStationId { get; set; }

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;
}
