using System;
namespace LCP.Vibe8.Api.Models;

public record RawMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specification { get; set; } = string.Empty;
    public int SupplierId { get; set; }
}

public record SubAssembly
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = "A";
    public int BillOfMaterialsId { get; set; }
}

public record FinishedProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Revision { get; set; } = "R1";
    public int BillOfMaterialsId { get; set; }
}

public record BillOfMaterials
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int OwnerProductId { get; set; }
}

public record ChangeOrder
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft";
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime RequestedOn { get; set; }
}

public record ComplianceCert
{
    public int Id { get; set; }
    public string CertificateType { get; set; } = string.Empty;
    public DateTime IssuedOn { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string Issuer { get; set; } = string.Empty;
}

public record SupplierContract
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string Terms { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public record EngineeringDesign
{
    public int Id { get; set; }
    public string DesignNumber { get; set; } = string.Empty;
    public string LeadEngineer { get; set; } = string.Empty;
    public string LifecyclePhase { get; set; } = "Concept";
}

public record ShopFloorExecution
{
    public int Id { get; set; }
    public string Shift { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public string Status { get; set; } = "Ready";
}

public record QualityAssurance
{
    public int Id { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
    public string MaturityLevel { get; set; } = "Defined";
}

public record WorkOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public string Status { get; set; } = string.Empty;
}

public record RoutingStep
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string Operation { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public int MachineId { get; set; }
}

public record Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cell { get; set; } = string.Empty;
    public string Capability { get; set; } = string.Empty;
}

public record Tooling
{
    public int Id { get; set; }
    public string ToolNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime LastCalibration { get; set; }
}

public record OperatorProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BadgeId { get; set; } = string.Empty;
    public string QualificationLevel { get; set; } = string.Empty;
}

public record InspectionPlan
{
    public int Id { get; set; }
    public string PlanNumber { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string SamplingMethod { get; set; } = string.Empty;
}

public record InspectionResult
{
    public int Id { get; set; }
    public int InspectionPlanId { get; set; }
    public int WorkOrderId { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
}

public record NonConformanceReport
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
}

public record CorrectiveAction
{
    public int Id { get; set; }
    public int NonConformanceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Owner { get; set; } = string.Empty;
}

public record TestSpecification
{
    public int Id { get; set; }
    public string SpecNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
}

public record CalibrationRecord
{
    public int Id { get; set; }
    public int ToolingId { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Status { get; set; } = string.Empty;
}

public record MaintenanceSchedule
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public DateTime ScheduledFor { get; set; }
    public string Type { get; set; } = string.Empty;
}

public record ProductionBatch
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string LotNumber { get; set; } = string.Empty;
}

public record LotTracking
{
    public int Id { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public DateTime ManufacturedOn { get; set; }
    public DateTime? Expiration { get; set; }
}

public record InventoryItem
{
    public int Id { get; set; }
    public string ItemNumber { get; set; } = string.Empty;
    public int OnHand { get; set; }
    public int Reserved { get; set; }
}

public record WarehouseLocation
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public record CustomerOrder
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime RequestedShipDate { get; set; }
    public string Priority { get; set; } = "Normal";
}

public record Shipment
{
    public int Id { get; set; }
    public string ShipmentNumber { get; set; } = string.Empty;
    public DateTime ShippedOn { get; set; }
    public string Carrier { get; set; } = string.Empty;
}

public record PackagingInstruction
{
    public int Id { get; set; }
    public string InstructionNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public record TraceabilityRecord
{
    public int Id { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public int WorkOrderId { get; set; }
    public string Evidence { get; set; } = string.Empty;
}
