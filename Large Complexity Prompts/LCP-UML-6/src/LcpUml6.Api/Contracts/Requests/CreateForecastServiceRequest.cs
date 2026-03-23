namespace LcpUml6.Api.Contracts.Requests;

public class CreateForecastServiceRequest
{
    public Guid? ForecastServiceId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public Guid WeatherStationId { get; set; }
}
