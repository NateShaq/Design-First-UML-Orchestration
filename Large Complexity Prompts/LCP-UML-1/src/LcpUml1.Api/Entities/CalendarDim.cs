using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class CalendarDim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DateId { get; set; }

    public DateOnly Ymd { get; set; }
}
