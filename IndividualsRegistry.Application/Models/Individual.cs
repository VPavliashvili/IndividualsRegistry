using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Application.Models;

public class Individual
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required Gender Gender { get; set; }

    public required string PersonalId { get; set; }

    public required DateOnly BirthDate { get; set; }

    public int? CityId { get; set; }
    public IEnumerable<PhoneNumber>? PhoneNumbers { get; set; }
    public IEnumerable<Individual>? RelatedIndividuals { get; set; }
}
