using AutoMapper;
using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.DTOs;

namespace SocialMedia.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Security, SecurityDto>().ReverseMap();
        }
    }
}
