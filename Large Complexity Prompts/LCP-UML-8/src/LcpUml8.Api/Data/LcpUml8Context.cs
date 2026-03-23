using LcpUml8.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Data;

public class LcpUml8Context : DbContext
{
    public LcpUml8Context(DbContextOptions<LcpUml8Context> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<RawMaterial> RawMaterials => Set<RawMaterial>();
    public DbSet<SubAssembly> SubAssemblies => Set<SubAssembly>();
    public DbSet<FinishedProduct> FinishedProducts => Set<FinishedProduct>();
    public DbSet<BillOfMaterials> BillOfMaterials => Set<BillOfMaterials>();
    public DbSet<BillOfMaterialsItem> BillOfMaterialsItems => Set<BillOfMaterialsItem>();
    public DbSet<ChangeOrder> ChangeOrders => Set<ChangeOrder>();
    public DbSet<ComplianceCert> ComplianceCerts => Set<ComplianceCert>();
    public DbSet<SupplierContract> SupplierContracts => Set<SupplierContract>();
    public DbSet<EngineeringDesign> EngineeringDesigns => Set<EngineeringDesign>();
    public DbSet<CADModel> CadModels => Set<CADModel>();
    public DbSet<Drawing> Drawings => Set<Drawing>();
    public DbSet<Revision> Revisions => Set<Revision>();
    public DbSet<Engineer> Engineers => Set<Engineer>();
    public DbSet<ManufacturingProcess> ManufacturingProcesses => Set<ManufacturingProcess>();
    public DbSet<WorkCenter> WorkCenters => Set<WorkCenter>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<Operator> Operators => Set<Operator>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<OperationOperator> OperationOperators => Set<OperationOperator>();
    public DbSet<ShopFloorExecution> ShopFloorExecutions => Set<ShopFloorExecution>();
    public DbSet<QualityAssurance> QualityAssurances => Set<QualityAssurance>();
    public DbSet<InspectionPlan> InspectionPlans => Set<InspectionPlan>();
    public DbSet<InspectionResult> InspectionResults => Set<InspectionResult>();
    public DbSet<NonConformance> NonConformances => Set<NonConformance>();
    public DbSet<CorrectiveAction> CorrectiveActions => Set<CorrectiveAction>();
    public DbSet<InventoryLot> InventoryLots => Set<InventoryLot>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<CustomerOrder> CustomerOrders => Set<CustomerOrder>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<TraceabilityRecord> TraceabilityRecords => Set<TraceabilityRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Use table-per-type to mirror UML inheritance while preserving 3NF.
        modelBuilder.Entity<Product>().UseTptMappingStrategy();

        modelBuilder.Entity<BillOfMaterialsItem>()
            .HasOne(i => i.BillOfMaterials)
            .WithMany(b => b.Items)
            .HasForeignKey(i => i.BillOfMaterialsId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BillOfMaterialsItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComplianceCert>()
            .HasOne(c => c.Product)
            .WithMany(p => p.ComplianceCerts)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SupplierContract>()
            .HasOne(s => s.RawMaterial)
            .WithMany(r => r.SupplierContracts)
            .HasForeignKey(s => s.RawMaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Machine>()
            .HasOne(m => m.WorkCenter)
            .WithMany(w => w.Machines)
            .HasForeignKey(m => m.WorkCenterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Operation>()
            .HasOne(o => o.ManufacturingProcess)
            .WithMany(mp => mp.Operations)
            .HasForeignKey(o => o.ManufacturingProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Operation>()
            .HasOne(o => o.WorkCenter)
            .WithMany()
            .HasForeignKey(o => o.WorkCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Operation>()
            .HasOne(o => o.WorkOrder)
            .WithMany(wo => wo.Operations)
            .HasForeignKey(o => o.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OperationOperator>()
            .HasOne(oo => oo.Operation)
            .WithMany(o => o.OperationOperators)
            .HasForeignKey(oo => oo.OperationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OperationOperator>()
            .HasOne(oo => oo.Operator)
            .WithMany(op => op.OperationOperators)
            .HasForeignKey(oo => oo.OperatorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkOrder>()
            .HasOne(w => w.Product)
            .WithMany()
            .HasForeignKey(w => w.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InspectionPlan>()
            .HasOne(ip => ip.QualityAssurance)
            .WithMany(qa => qa.InspectionPlans)
            .HasForeignKey(ip => ip.QualityAssuranceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InspectionResult>()
            .HasOne(ir => ir.InspectionPlan)
            .WithMany(ip => ip.InspectionResults)
            .HasForeignKey(ir => ir.InspectionPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NonConformance>()
            .HasOne(nc => nc.InspectionResult)
            .WithMany(ir => ir.NonConformances)
            .HasForeignKey(nc => nc.InspectionResultId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CorrectiveAction>()
            .HasOne(ca => ca.NonConformance)
            .WithMany(nc => nc.CorrectiveActions)
            .HasForeignKey(ca => ca.NonConformanceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryLot>()
            .HasOne(il => il.Warehouse)
            .WithMany(w => w.InventoryLots)
            .HasForeignKey(il => il.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryLot>()
            .HasOne(il => il.RawMaterial)
            .WithMany(rm => rm.InventoryLots)
            .HasForeignKey(il => il.RawMaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryLot>()
            .HasOne(il => il.SubAssembly)
            .WithMany(sa => sa.InventoryLots)
            .HasForeignKey(il => il.SubAssemblyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerOrder>()
            .HasOne(co => co.FinishedProduct)
            .WithMany(fp => fp.CustomerOrders)
            .HasForeignKey(co => co.FinishedProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.CustomerOrder)
            .WithMany(co => co.Shipments)
            .HasForeignKey(s => s.CustomerOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TraceabilityRecord>()
            .HasOne(tr => tr.InventoryLot)
            .WithMany(il => il.TraceabilityRecords)
            .HasForeignKey(tr => tr.InventoryLotId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TraceabilityRecord>()
            .HasOne(tr => tr.WorkOrder)
            .WithMany(wo => wo.TraceabilityRecords)
            .HasForeignKey(tr => tr.WorkOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TraceabilityRecord>()
            .HasOne(tr => tr.InspectionResult)
            .WithMany(ir => ir.TraceabilityRecords)
            .HasForeignKey(tr => tr.InspectionResultId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TraceabilityRecord>()
            .HasOne(tr => tr.Shipment)
            .WithMany(s => s.TraceabilityRecords)
            .HasForeignKey(tr => tr.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
