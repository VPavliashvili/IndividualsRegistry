using AutoMapper;
using IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;
using IndividualsRegistry.Application.Individuals.Commands.EditIndividual;
using IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;
using IndividualsRegistry.Application.Models;
using IndividualsRegistry.Domain.Entities;

namespace IndividualsRegistry.Application.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<PhoneNumberEntity, PhoneNumber>();
        CreateMap<IndividualEntity, Individual>()
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers))
            .ForMember(
                dest => dest.RelatedIndividuals,
                opt => opt.MapFrom(src => src.RelatedIndividuals)
            );
        CreateMap<IndividualEntity, GetFullIndividualInfoResponse>()
            .IncludeBase<IndividualEntity, Individual>();

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

        CreateMap<EditIndividualCommand, IndividualEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.individualId))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => src.request.Name != null))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.request.Name))
            .ForMember(
                dest => dest.Surname,
                opt => opt.Condition(src => src.request.Surname != null)
            )
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.request.Surname))
            .ForMember(
                dest => dest.Gender,
                opt => opt.Condition(src => src.request.Gender.HasValue)
            )
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.request.Gender!.Value))
            .ForMember(
                dest => dest.PersonalId,
                opt => opt.Condition(src => src.request.PersonalId != null)
            )
            .ForMember(dest => dest.PersonalId, opt => opt.MapFrom(src => src.request.PersonalId))
            .ForMember(
                dest => dest.BirthDate,
                opt => opt.Condition(src => src.request.BirthDate.HasValue)
            )
            .ForMember(
                dest => dest.BirthDate,
                opt => opt.MapFrom(src => src.request.BirthDate!.Value)
            )
            .ForMember(
                dest => dest.CityId,
                opt => opt.Condition(src => src.request.CityId.HasValue)
            )
            .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.request.CityId))
            .ForMember(
                dest => dest.PhoneNumbers,
                opt => opt.Condition(src => src.request.PhoneNumbers != null)
            )
            .ForMember(
                dest => dest.PhoneNumbers,
                opt => opt.MapFrom(src => src.request.PhoneNumbers)
            )
            .ForMember(dest => dest.Picture, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedIndividuals, opt => opt.Ignore());
    }
}
