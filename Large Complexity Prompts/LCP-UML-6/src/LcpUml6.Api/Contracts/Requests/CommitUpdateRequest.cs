using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Contracts.Requests;

public class CommitUpdateRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
