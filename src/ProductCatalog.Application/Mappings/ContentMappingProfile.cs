using AutoMapper;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Mappings;

/// <summary>
/// AutoMapper profile for Content bounded context
/// </summary>
public class ContentMappingProfile : Profile
{
    public ContentMappingProfile()
    {
        // Subject mappings
        CreateMap<Subject, GetSubjectResponse>()
            .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.Topics ?? new List<Topic>()));

        CreateMap<AddSubjectRequest, Subject>()
            .ConstructUsing(src => new Subject(src.SubjectName));

        // Topic mappings
        CreateMap<Topic, GetTopicResponse>()
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectName : string.Empty));

        CreateMap<AddTopicRequest, Topic>()
            .ConstructUsing(src => new Topic(src.TopicName, src.SubjectId));

    }
}
