namespace LCP.Vibe.Api.Models;

public class DispatchCenter
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CoverageArea { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class EmergencyUnit
{
    public Guid Id { get; set; }
    public string UnitType { get; set; } = "Ambulance";
    public string Station { get; set; } = string.Empty;
    public bool Available { get; set; }
    public DateTime LastStatusUpdateUtc { get; set; }
}

public class ResponseTimeKPI
{
    public string District { get; set; } = string.Empty;
    public decimal AverageMinutes { get; set; }
    public decimal NinetyPercentileMinutes { get; set; }
    public DateTime PeriodEndingUtc { get; set; }
}

public class HospitalCapacity
{
    public Guid HospitalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ErBedsTotal { get; set; }
    public int ErBedsAvailable { get; set; }
    public int IcuBedsAvailable { get; set; }
}

public class EvacuationRoute
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Notes { get; set; } = string.Empty;
}

public class AlertNotification
{
    public Guid Id { get; set; }
    public string Audience { get; set; } = "citywide";
    public string Message { get; set; } = string.Empty;
    public DateTime SentUtc { get; set; }
    public string Channel { get; set; } = "sms";
}

public class ResourceInventory
{
    public Guid Id { get; set; }
    public string ResourceType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class ShelterLocation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool PetFriendly { get; set; }
}

public class CrisisDrill
{
    public Guid Id { get; set; }
    public string Scenario { get; set; } = string.Empty;
    public DateTime ScheduledFor { get; set; }
    public string LeadAgency { get; set; } = string.Empty;
}

public class MutualAidAgreement
{
    public Guid Id { get; set; }
    public string Partner { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
}

public class EmergencyReadiness
{
    public int ActiveUnits { get; set; }
    public decimal AverageResponseMinutes { get; set; }
    public int HospitalsAtCapacity { get; set; }
    public DateTime GeneratedAtUtc { get; set; }
}
