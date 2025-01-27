using AutoMapper;
using IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;
using IndividualsRegistry.Application.Models;
using IndividualsRegistry.Domain.Entities;

namespace IndividualsRegistry.Application.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<CreateIndividualRequest, Individual>();
        CreateMap<PhoneNumber, PhoneNumberEntity>()
            .ForMember(dest => dest.Individual, opt => opt.Ignore())
            .ForMember(dest => dest.Individualid, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Individual, IndividualEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Picture, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers))
            .ForMember(
                dest => dest.RelatedIndividuals,
                opt => opt.MapFrom(src => src.RelatedIndividuals)
            );
    }
}
