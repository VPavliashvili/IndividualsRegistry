using System.ComponentModel.DataAnnotations;
using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Domain.Entities;

public class PhoneNumberEntity
{
    public int Id { get; set; }
    public PhoneNumberType Type { get; set; }

    [StringLength(50, MinimumLength = 2)]
    public required string Number { get; set; }
    public int? Individualid { get; set; }

    public virtual IndividualEntity? Individual { get; set; }
}
