using System;
using System.Collections.Generic;
using System.Linq;
using LCP.Vibe8.Api.Models;

namespace LCP.Vibe8.Api.Data;

public class PlmRepository
{
    public List<RawMaterial> RawMaterials { get; } = new();
    public List<SubAssembly> SubAssemblies { get; } = new();
    public List<FinishedProduct> FinishedProducts { get; } = new();
    public List<WorkOrder> WorkOrders { get; } = new();
    public List<InspectionResult> InspectionResults { get; } = new();
    public List<ChangeOrder> ChangeOrders { get; } = new();

    public PlmRepository()
    {
        Seed();
    }

    private void Seed()
    {
        var steel = new RawMaterial { Id = 1, Name = "Stainless Steel", Specification = "304L", SupplierId = 101 };
        RawMaterials.Add(steel);

        var frame = new SubAssembly
        {
            Id = 10,
            Name = "Chassis Frame",
            Version = "A",
            BillOfMaterialsId = 900
        };
        SubAssemblies.Add(frame);

        FinishedProducts.Add(new FinishedProduct
        {
            Id = 100,
            Name = "Industrial Pump",
            Sku = "PUMP-IND-01",
            Revision = "R1",
            BillOfMaterialsId = 901
        });

        WorkOrders.Add(new WorkOrder
        {
            Id = 5000,
            ProductId = 100,
            Quantity = 25,
            ScheduledStart = DateTime.UtcNow.AddDays(2),
            ScheduledEnd = DateTime.UtcNow.AddDays(5),
            Status = "Planned"
        });

        InspectionResults.Add(new InspectionResult
        {
            Id = 7500,
            InspectionPlanId = 300,
            WorkOrderId = 5000,
            Result = "Pass",
            RecordedAt = DateTime.UtcNow
        });

        ChangeOrders.Add(new ChangeOrder
        {
            Id = 2000,
            Title = "Update pump impeller tolerance",
            Status = "Draft",
            RequestedBy = "design.engineer@acme.com",
            RequestedOn = DateTime.UtcNow.AddDays(-1)
        });
    }

    public ChangeOrder AddChangeOrder(ChangeOrder change)
    {
        change.Id = ChangeOrders.Count == 0 ? 1 : ChangeOrders.Max(c => c.Id) + 1;
        change.RequestedOn = DateTime.UtcNow;
        ChangeOrders.Add(change);
        return change;
    }
}
