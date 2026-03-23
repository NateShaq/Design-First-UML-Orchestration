using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class ForecastService
{
    [Key]
    public Guid ForecastServiceId { get; set; }

    [MaxLength(120)]
    public string Provider { get; set; } = string.Empty;

    public Guid WeatherStationId { get; set; }
    public WeatherStation? WeatherStation { get; set; }
}
