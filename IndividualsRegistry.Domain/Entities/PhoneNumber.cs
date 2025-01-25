using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Domain.Entities;

public class PhoneNumber
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public PhoneNumberType Type { get; set; }

    [StringLength(50, MinimumLength = 2)]
    public required string Number { get; set; }

    [ForeignKey("IndividualId")]
    public int Individualid { get; set; }
}
