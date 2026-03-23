## LCP-Vibe-9 Government Digital Identity & Public Services

Contents:
- `Api/` .NET 8 Web API with in-memory store for Identity Registry, Tax Collection, and Social Security Benefits.
- `Api/Database/schema.sql` relational model covering 30 domain classes.

Run locally:
1. `cd Api`
2. `dotnet restore` (requires internet access to NuGet)
3. `dotnet run`

Key endpoints (base `/api`):
- `identity/citizens`, `identity/registry`, `identity/biometrics/{citizenId}`
- `tax/assessments`, `tax/payments`, `tax/collection`
- `benefits` and `benefits/claims`
- `credentials/passport/*`, `credentials/drivers-license/*`
- `voting/voters`, `voting/elections`, `voting/ballot-requests`
- `operations/audit`, `operations/status`

Domain coverage (30 classes):
Citizen, IdentityRecord, IdentityRegistry, BiometricData, PublicRecord, Jurisdiction, AuditTrail, TaxPayer, TaxAssessment, TaxPayment, TaxCollection, SocialSecurityBenefits, BenefitClaim, BenefitPayment, ContributionHistory, PassportService, PassportApplication, Passport, DriversLicense, LicenseApplication, License, VotingRegistry, VoterRecord, Election, BallotRequest, ServiceRequest, Notification, DocumentVerification, AccessPolicy, ServiceStatus.
