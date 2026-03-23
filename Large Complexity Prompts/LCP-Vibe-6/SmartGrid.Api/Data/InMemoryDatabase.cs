using SmartGrid.Api.Models;

namespace SmartGrid.Api.Data;

public class InMemoryDatabase
{
    public List<SolarFarm> SolarFarms { get; } = new();
    public List<WindTurbine> WindTurbines { get; } = new();
    public List<HydroPlant> HydroPlants { get; } = new();
    public List<GeothermalPlant> GeothermalPlants { get; } = new();
    public List<BiomassPlant> BiomassPlants { get; } = new();
    public List<BatteryStorage> BatteryStorages { get; } = new();
    public List<EnergyStorageSystem> EnergyStorageSystems { get; } = new();
    public List<SubstationMonitor> SubstationMonitors { get; } = new();
    public List<LoadBalancer> LoadBalancers { get; } = new();
    public List<DemandResponseEvent> DemandResponseEvents { get; } = new();
    public List<GridStabilityIndex> GridStability { get; } = new();
    public List<GridAlert> GridAlerts { get; } = new();
    public List<FaultRecord> FaultRecords { get; } = new();
    public List<WeatherForecast> Forecasts { get; } = new();
    public List<ForecastAccuracy> ForecastAccuracies { get; } = new();
    public List<PowerGenerationRecord> GenerationRecords { get; } = new();
    public List<PowerPurchaseAgreement> PowerPurchaseAgreements { get; } = new();
    public List<CustomerAccount> CustomerAccounts { get; } = new();
    public List<SmartMeter> SmartMeters { get; } = new();
    public List<MeterReading> MeterReadings { get; } = new();
    public List<TariffPlan> TariffPlans { get; } = new();
    public List<BillingCycle> BillingCycles { get; } = new();
    public List<UsageBilling> UsageBillings { get; } = new();
    public List<Invoice> Invoices { get; } = new();
    public List<Payment> Payments { get; } = new();
    public List<OutageTicket> OutageTickets { get; } = new();
    public List<MaintenanceSchedule> MaintenanceSchedules { get; } = new();
    public List<CrewAssignment> CrewAssignments { get; } = new();
    public List<Transformer> Transformers { get; } = new();
    public List<ElectricVehicleCharger> EvChargers { get; } = new();

    public InMemoryDatabase()
    {
        Seed();
    }

    private void Seed()
    {
        SolarFarms.Add(new SolarFarm { Id = 1, Name = "Sunrise Valley", CapacityMw = 120, Location = "TX-West", AvailabilityFactor = 0.94 });
        WindTurbines.Add(new WindTurbine { Id = 1, Farm = "Prairie Wind", RatedMw = 3.5m, CurrentOutputMw = 2.7m, WindSpeedMps = 11.4 });
        HydroPlants.Add(new HydroPlant { Id = 1, Reservoir = "Blue River", CapacityMw = 200, WaterLevelMeters = 52 });
        BatteryStorages.Add(new BatteryStorage { Id = 1, Site = "GridEdge", CapacityMwh = 400, StateOfChargePercent = 76 });
        SubstationMonitors.Add(new SubstationMonitor { Id = 1, SubstationName = "East-115", VoltageKv = 114.7m, FrequencyHz = 60.01m, LoadFactor = 0.83 });
        LoadBalancers.Add(new LoadBalancer { Id = 1, Region = "ERCOT-North", CurrentLoadMw = 1800, TargetLoadMw = 1750 });
        GridStability.Add(new GridStabilityIndex { Id = 1, Timestamp = DateTime.UtcNow, Value = 0.98, Status = "Stable" });
        GridAlerts.Add(new GridAlert { Id = 1, RaisedAt = DateTime.UtcNow.AddMinutes(-15), Severity = "Advisory", Message = "High wind ramp expected" });
        DemandResponseEvents.Add(new DemandResponseEvent { Id = 1, Name = "HeatWaveFlex", StartTime = DateTime.UtcNow.AddHours(2), EndTime = DateTime.UtcNow.AddHours(6), ShedGoalMw = 150 });
        Forecasts.Add(new WeatherForecast { Id = 1, Region = "TX-West", ForecastDate = DateTime.UtcNow.Date.AddDays(1), WindSpeedMps = 10.5, SolarIrradianceWm2 = 820, TemperatureC = 35 });
        ForecastAccuracies.Add(new ForecastAccuracy { Id = 1, ForecastId = 1, ErrorPercent = 4.5, Metric = "MAPE" });
        GenerationRecords.Add(new PowerGenerationRecord { Id = 1, SourceType = "SolarFarm", SourceId = 1, Timestamp = DateTime.UtcNow, Megawatts = 95.4m });
        PowerPurchaseAgreements.Add(new PowerPurchaseAgreement { Id = 1, Counterparty = "Municipal Utility", ContractMw = 50, PricePerMwh = 32.5m, EffectiveDate = DateTime.UtcNow.Date });
        CustomerAccounts.Add(new CustomerAccount { Id = 1, Name = "Riley Homes", Address = "100 Grid St, Austin, TX", Email = "billing@rileyhomes.com" });
        SmartMeters.Add(new SmartMeter { Id = 1, CustomerAccountId = 1, SerialNumber = "SM-4455001", InstalledOn = DateTime.UtcNow.AddYears(-1) });
        MeterReadings.Add(new MeterReading { Id = 1, SmartMeterId = 1, ReadAt = DateTime.UtcNow.AddHours(-1), Kwh = 14.3m });
        TariffPlans.Add(new TariffPlan { Id = 1, Name = "Residential TOU", RatePerKwh = 0.12m, TimeOfUse = true });
        BillingCycles.Add(new BillingCycle { Id = 1, CustomerAccountId = 1, PeriodStart = DateTime.UtcNow.AddMonths(-1), PeriodEnd = DateTime.UtcNow });
        UsageBillings.Add(new UsageBilling { Id = 1, BillingCycleId = 1, TotalKwh = 820, AmountDue = 98.4m });
        Invoices.Add(new Invoice { Id = 1, BillingCycleId = 1, IssuedOn = DateTime.UtcNow.Date, DueDate = DateTime.UtcNow.Date.AddDays(15), Amount = 98.4m });
        Payments.Add(new Payment { Id = 1, InvoiceId = 1, PaidOn = DateTime.UtcNow.Date.AddDays(-3), Amount = 98.4m, Method = "ACH" });
        OutageTickets.Add(new OutageTicket { Id = 1, CustomerAccountId = 1, OpenedAt = DateTime.UtcNow.AddDays(-10), Status = "Resolved" });
        MaintenanceSchedules.Add(new MaintenanceSchedule { Id = 1, AssetType = "WindTurbine", AssetId = 1, ScheduledFor = DateTime.UtcNow.AddDays(7), Crew = "Crew-A" });
        CrewAssignments.Add(new CrewAssignment { Id = 1, CrewName = "Crew-A", Skillset = "HV Work", AssignedOn = DateTime.UtcNow.AddDays(-1) });
        Transformers.Add(new Transformer { Id = 1, Substation = "East-115", RatingMva = 50, LoadPercent = 68 });
        EvChargers.Add(new ElectricVehicleCharger { Id = 1, Location = "City Garage A", Ports = 8, MaxKw = 350 });
    }
}
