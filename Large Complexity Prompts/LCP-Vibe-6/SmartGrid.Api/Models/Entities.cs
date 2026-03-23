namespace SmartGrid.Api.Models;

public class SolarFarm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal CapacityMw { get; set; }
    public string Location { get; set; } = string.Empty;
    public double AvailabilityFactor { get; set; }
}

public class WindTurbine
{
    public int Id { get; set; }
    public string Farm { get; set; } = string.Empty;
    public decimal RatedMw { get; set; }
    public decimal CurrentOutputMw { get; set; }
    public double WindSpeedMps { get; set; }
}

public class HydroPlant
{
    public int Id { get; set; }
    public string Reservoir { get; set; } = string.Empty;
    public decimal CapacityMw { get; set; }
    public decimal WaterLevelMeters { get; set; }
}

public class GeothermalPlant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal CapacityMw { get; set; }
    public double SteamPressureBar { get; set; }
}

public class BiomassPlant
{
    public int Id { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public decimal CapacityMw { get; set; }
    public double MoistureContentPercent { get; set; }
}

public class BatteryStorage
{
    public int Id { get; set; }
    public string Site { get; set; } = string.Empty;
    public decimal CapacityMwh { get; set; }
    public decimal StateOfChargePercent { get; set; }
}

public class EnergyStorageSystem
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal CapacityMwh { get; set; }
    public decimal MaxDischargeMw { get; set; }
}

public class SubstationMonitor
{
    public int Id { get; set; }
    public string SubstationName { get; set; } = string.Empty;
    public decimal VoltageKv { get; set; }
    public decimal FrequencyHz { get; set; }
    public double LoadFactor { get; set; }
}

public class LoadBalancer
{
    public int Id { get; set; }
    public string Region { get; set; } = string.Empty;
    public decimal CurrentLoadMw { get; set; }
    public decimal TargetLoadMw { get; set; }
}

public class DemandResponseEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal ShedGoalMw { get; set; }
}

public class GridStabilityIndex
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class GridAlert
{
    public int Id { get; set; }
    public DateTime RaisedAt { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class FaultRecord
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string Cause { get; set; } = string.Empty;
}

public class WeatherForecast
{
    public int Id { get; set; }
    public string Region { get; set; } = string.Empty;
    public DateTime ForecastDate { get; set; }
    public double WindSpeedMps { get; set; }
    public double SolarIrradianceWm2 { get; set; }
    public double TemperatureC { get; set; }
}

public class ForecastAccuracy
{
    public int Id { get; set; }
    public int ForecastId { get; set; }
    public double ErrorPercent { get; set; }
    public string Metric { get; set; } = string.Empty;
}

public class PowerGenerationRecord
{
    public int Id { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public int SourceId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Megawatts { get; set; }
}

public class PowerPurchaseAgreement
{
    public int Id { get; set; }
    public string Counterparty { get; set; } = string.Empty;
    public decimal ContractMw { get; set; }
    public decimal PricePerMwh { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class CustomerAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class SmartMeter
{
    public int Id { get; set; }
    public int CustomerAccountId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public DateTime InstalledOn { get; set; }
}

public class MeterReading
{
    public int Id { get; set; }
    public int SmartMeterId { get; set; }
    public DateTime ReadAt { get; set; }
    public decimal Kwh { get; set; }
}

public class TariffPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal RatePerKwh { get; set; }
    public bool TimeOfUse { get; set; }
}

public class BillingCycle
{
    public int Id { get; set; }
    public int CustomerAccountId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class UsageBilling
{
    public int Id { get; set; }
    public int BillingCycleId { get; set; }
    public decimal TotalKwh { get; set; }
    public decimal AmountDue { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public int BillingCycleId { get; set; }
    public DateTime IssuedOn { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public DateTime PaidOn { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
}

public class OutageTicket
{
    public int Id { get; set; }
    public int CustomerAccountId { get; set; }
    public DateTime OpenedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class MaintenanceSchedule
{
    public int Id { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public int AssetId { get; set; }
    public DateTime ScheduledFor { get; set; }
    public string Crew { get; set; } = string.Empty;
}

public class CrewAssignment
{
    public int Id { get; set; }
    public string CrewName { get; set; } = string.Empty;
    public string Skillset { get; set; } = string.Empty;
    public DateTime AssignedOn { get; set; }
}

public class Transformer
{
    public int Id { get; set; }
    public string Substation { get; set; } = string.Empty;
    public decimal RatingMva { get; set; }
    public decimal LoadPercent { get; set; }
}

public class ElectricVehicleCharger
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Ports { get; set; }
    public decimal MaxKw { get; set; }
}
