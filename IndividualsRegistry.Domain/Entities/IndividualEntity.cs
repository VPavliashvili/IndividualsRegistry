using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Domain.Entities;

public class IndividualEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public required string Surname { get; set; }

    [Required]
    public required Gender Gender { get; set; }

    [Required]
    public required string PersonalId { get; set; }

    [Required]
    public required DateOnly BirthDate { get; set; }

    public int? CityId { get; set; }
    public ICollection<PhoneNumberEntity>? PhoneNumbers { get; set; }
    public byte[]? Picture { get; set; }
    public ICollection<IndividualEntity>? RelatedIndividuals { get; set; }
}
