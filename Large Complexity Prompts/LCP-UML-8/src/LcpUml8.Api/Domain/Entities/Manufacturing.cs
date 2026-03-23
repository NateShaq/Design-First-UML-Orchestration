using System.ComponentModel.DataAnnotations;

namespace LcpUml8.Api.Domain.Entities;

public class ManufacturingProcess
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
}

public class WorkCenter
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public ICollection<Machine> Machines { get; set; } = new List<Machine>();
}

public class Machine
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(100)]
    public string AssetTag { get; set; } = string.Empty;

    [Required]
    public string WorkCenterId { get; set; } = string.Empty;
    public WorkCenter? WorkCenter { get; set; }
}

public class Operator
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string Badge { get; set; } = string.Empty;

    public ICollection<OperationOperator> OperationOperators { get; set; } = new List<OperationOperator>();
}

public class WorkOrder
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public DateTime ScheduleDate { get; set; }

    [Required]
    public string ProductId { get; set; } = string.Empty;
    public Product? Product { get; set; }

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    public ICollection<ChangeOrder> ChangeOrders { get; set; } = new List<ChangeOrder>();
    public ICollection<TraceabilityRecord> TraceabilityRecords { get; set; } = new List<TraceabilityRecord>();
}

public class Operation
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public int Seq { get; set; }

    public string? ManufacturingProcessId { get; set; }
    public ManufacturingProcess? ManufacturingProcess { get; set; }

    public string? WorkCenterId { get; set; }
    public WorkCenter? WorkCenter { get; set; }

    public string? WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }

    public ICollection<OperationOperator> OperationOperators { get; set; } = new List<OperationOperator>();
}

public class OperationOperator
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string OperationId { get; set; } = string.Empty;
    public Operation? Operation { get; set; }

    [Required]
    public string OperatorId { get; set; } = string.Empty;
    public Operator? Operator { get; set; }
}

public class ShopFloorExecution
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
