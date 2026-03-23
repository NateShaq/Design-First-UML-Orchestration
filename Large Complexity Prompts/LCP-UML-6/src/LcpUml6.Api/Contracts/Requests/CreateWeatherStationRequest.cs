namespace LcpUml6.Api.Contracts.Requests;

public class CreateWeatherStationRequest
{
    public Guid? WeatherStationId { get; set; }
    public string Location { get; set; } = string.Empty;
}
