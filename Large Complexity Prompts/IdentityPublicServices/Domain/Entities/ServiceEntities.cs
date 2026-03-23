using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityPublicServices.Domain.Entities;

public class IdentityProvider
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string ProviderName { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string AssuranceLevel { get; set; } = string.Empty;

    public ICollection<AuthenticationService> AuthenticationServices { get; set; } = new List<AuthenticationService>();
    public ICollection<IdentityRegistry> IdentityRegistries { get; set; } = new List<IdentityRegistry>();
    public ICollection<VerificationRequest> VerificationRequests { get; set; } = new List<VerificationRequest>();
}

public class AuthenticationService
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(80)]
    public string Method { get; set; } = string.Empty;

    public int TokenTtl { get; set; }

    public Guid IdentityProviderId { get; set; }
    public IdentityProvider IdentityProvider { get; set; } = null!;

    public AuthorizationService? AuthorizationService { get; set; }
    public ServicePortal? ServicePortal { get; set; }
}

public class AuthorizationService
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string PolicyEngine { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string Decision { get; set; } = string.Empty;

    public Guid AuthenticationServiceId { get; set; }
    public AuthenticationService AuthenticationService { get; set; } = null!;

    public Guid? ConsentRecordId { get; set; }
    public ConsentRecord? ConsentRecord { get; set; }

    public ServicePortal? ServicePortal { get; set; }
}

public class DigitalWallet
{
    [Key]
    public Guid WalletId { get; set; }

    [Required, MaxLength(120)]
    public string DeviceBinding { get; set; } = string.Empty;

    public ICollection<Credential> Credentials { get; set; } = new List<Credential>();
    public Guid ServicePortalId { get; set; }
    public ServicePortal ServicePortal { get; set; } = null!;
}

public class Credential
{
    [Key]
    public Guid CredentialId { get; set; }

    [Required, MaxLength(60)]
    public string Type { get; set; } = string.Empty;

    public Guid? DigitalWalletId { get; set; }
    public DigitalWallet? DigitalWallet { get; set; }

    public PassportService? PassportService { get; set; }
    public DriversLicense? DriversLicense { get; set; }
}

public class VerificationRequest
{
    [Key]
    public Guid RequestId { get; set; }

    [Required, MaxLength(160)]
    public string Purpose { get; set; } = string.Empty;

    public Guid IdentityRegistryId { get; set; }
    public IdentityRegistry IdentityRegistry { get; set; } = null!;

    public Guid IdentityProviderId { get; set; }
    public IdentityProvider IdentityProvider { get; set; } = null!;

    public ICollection<VerificationResponse> Responses { get; set; } = new List<VerificationResponse>();
}

public class VerificationResponse
{
    [Key]
    public Guid ResponseId { get; set; }

    [Required, MaxLength(40)]
    public string Result { get; set; } = string.Empty;

    public Guid VerificationRequestId { get; set; }
    public VerificationRequest VerificationRequest { get; set; } = null!;
}

public class ServicePortal
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string PortalUrl { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string Channel { get; set; } = string.Empty;

    public ICollection<DigitalWallet> DigitalWallets { get; set; } = new List<DigitalWallet>();

    public Guid? AuthenticationServiceId { get; set; }
    public AuthenticationService? AuthenticationService { get; set; }

    public Guid? AuthorizationServiceId { get; set; }
    public AuthorizationService? AuthorizationService { get; set; }

    public NotificationService? NotificationService { get; set; }
}

public class NotificationService
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(60)]
    public string ChannelType { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string RetryPolicy { get; set; } = string.Empty;

    public Guid ServicePortalId { get; set; }
    public ServicePortal ServicePortal { get; set; } = null!;
}
