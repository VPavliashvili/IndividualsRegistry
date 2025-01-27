using IndividualsRegistry.Application.Models;
using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public Gender? Gender { get; set; }
    public string? PersonalId { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? CityId { get; set; }
    public IEnumerable<PhoneNumber> PhoneNumbers { get; set; } = [];
}
