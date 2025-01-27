using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Application.Models;

public class PhoneNumber
{
    public required PhoneNumberType Type { get; set; }
    public required string Number { get; set; }
}
