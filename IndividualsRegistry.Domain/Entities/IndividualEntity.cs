namespace IndividualsRegistry.Domain.Entities;

public class IndividualEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Gender { get; set; }
    public required string PersonalId { get; set; }
    public required DateOnly BirthDate { get; set; }
    public City? City { get; set; }
    public ICollection<PhoneNumber>? PhoneNumbers { get; set; }
    public byte[]? Picture { get; set; }
    public ICollection<IndividualEntity>? RelatedIndividuals { get; set; }
}
