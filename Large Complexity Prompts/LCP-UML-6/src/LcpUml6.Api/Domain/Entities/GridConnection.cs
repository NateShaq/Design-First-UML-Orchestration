using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class GridConnection
{
    [Key]
    public Guid GridConnectionId { get; set; }

    [MaxLength(200)]
    public string Node { get; set; } = string.Empty;
}
