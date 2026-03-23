using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Contracts.Requests;

public class CommitTransactionRequest
{
    [Required]
    public string TxId { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
