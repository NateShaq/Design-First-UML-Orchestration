#!/bin/zsh

# Array of Tier 3 Complex Prompts from your documentation
# Each entry: "Project Name | Core Requirements | Specific Entities"
PROMPTS=(
    "Smart City Traffic & Infrastructure Grid | TrafficFlow, PublicTransit, EmergencyResponse | ElectricBuses, LightRail, AutonomousShuttles, TrafficSensor, SignalController, GridPowerMonitor, MaintenanceSchedule"
    "Omnichannel Retail Enterprise | ECommerce, BrickAndMortarPointOfSale, SupplyChainLogistics | PerishableGoods, LuxuryItems, DigitalDownloads, LoyaltyProgram, InventoryAudit, TaxCalculator, ReturnAuthorization"
    "Comprehensive Hospital Management & EMR | PatientRecords, PharmacyInventory, SurgicalScheduling | Inpatient, Outpatient, EmergencyCase, LabResult, RadiologyImage, InsuranceClaim, BillingInvoice"
    "Autonomous Vehicle Fleet Network | VehicleTelematics, PassengerBooking, RemoteOperatorSupport | SedanAV, VanAV, DeliveryDrone, LiDARProcessor, RoutePlanner, ChargingStation, IncidentReport"
    "Digital Banking & Wealth Management Engine | RetailBanking, InvestmentPortfolios, ComplianceMonitoring | SavingsAccount, CryptoWallet, StockPortfolio, TransactionLedger, RiskAssessment, KYCVerification, TaxDocument"
    "Smart Grid & Renewable Energy Distribution | PowerGeneration, GridStability, CustomerMetering | SolarFarm, WindTurbine, HydroPlant, BatteryStorage, SubstationMonitor, LoadBalancer, UsageBilling"
    "Global Higher Education ERP | StudentAdmissions, LearningManagement, AlumniRelations | Undergraduate, Postgraduate, ResearchFellow, CourseCurriculum, FinancialAidPackage, Transcript, FacultyGrant"
    "Multi-National Manufacturing PLM | EngineeringDesign, ShopFloorExecution, QualityAssurance | RawMaterial, SubAssembly, FinishedProduct, BillOfMaterials, ChangeOrder, ComplianceCert, SupplierContract"
    "Government Digital Identity & Public Services | IdentityRegistry, TaxCollection, SocialSecurityBenefits | PassportService, DriversLicense, VotingRegistry, BiometricData, AuditTrail, Jurisdiction, PublicRecord"
)

for i in {1..${#PROMPTS[@]}}; do
    # Extract prompt components
    IFS='|' read -r NAME REQS ENTITIES <<< "${PROMPTS[$i]}"
    
    echo "------------------------------------------------"
    echo "STARTING EXPERIMENT #$i: $NAME"
    echo "------------------------------------------------"

    # --- PART A: SINGLE SHOT (VIBE) ---
    VIBE_DIR="LCP-Vibe-$i"
    mkdir -p $VIBE_DIR
    
    # Reset Session to ensure no contamination
    rm -rf ~/.codex/sessions/* echo "[Vibe] Generating Project..."
    rm -rf ~/.codex/memories/*
    codex exec --sandbox workspace-write "Generate a C# .NET Web API and SQL schema for: $NAME. Requirements: $REQS. Define 30 distinct classes including $ENTITIES. Create folder: $VIBE_DIR. Add all code files and artifacts." 2>&1 | tee ./$VIBE_DIR/experiment_metadata.log

    echo "[Vibe] Auditing Build..."
    codex exec --sandbox workspace-write "Act as a .NET Build Engineer. Attempt a dry-run compilation of the C# code in ./$VIBE_DIR. List 'Missing Type' errors and count if 30 classes are defined." 2>&1 | tee -a ./$VIBE_DIR/build_audit.log


    # --- PART B: UML ANCHORED ---
    UML_DIR="LCP-UML-$i"
    mkdir -p $UML_DIR

    # Reset Session for the Anchored Path
    rm -rf ~/.codex/sessions/* echo "[UML] Step 1: Synthesis..."
    rm -rf ~/.codex/memories/*
    codex exec --sandbox workspace-write "Create PlantUML for: $NAME. Requirements: $REQS. Define 30 distinct classes including $ENTITIES. Explicitly define inheritance and composition. Name project: $UML_DIR." 2>&1 | tee ./$UML_DIR/experiment_metadata.log

    echo "[UML] Step 2: Academic Audit..."
    codex exec --sandbox workspace-write "Audit the PlantUML in ./$UML_DIR. Check: 1. 30 classes. 2. Concurrency protection for $ENTITIES (prevent Ghost Writes). 3. ACID compliance for workflows. 4. 3NF Normalization. Update the .puml file with corrections." 2>&1 | tee -a ./$UML_DIR/experiment_metadata.log

    echo "[UML] Step 3: Anchored Implementation..."
    codex exec --sandbox workspace-write "Using the VALIDATED PlantUML from $UML_DIR as a strict anchor, implement the C# .NET Web API and MS SQL Backend. Constraints: 3NF, [Timestamp] for Ghost Write protection, and IsolationLevel.Serializable for critical controllers. Write to ./$UML_DIR/src." 2>&1 | tee -a ./$UML_DIR/experiment_metadata.log

    echo "[UML] Step 4: Final Build Test..."
    codex exec --sandbox workspace-write "Act as a .NET Build Engineer. Attempt a Dry Run compilation of ./$UML_DIR/src. Identify missing NuGet packages or syntax errors. Report PASS/FAIL." 2>&1 | tee -a ./$UML_DIR/build_audit.log

done

echo "Benchmark Complete. 20 Folders Generated."