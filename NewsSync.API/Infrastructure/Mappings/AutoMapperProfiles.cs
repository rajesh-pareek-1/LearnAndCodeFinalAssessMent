using AutoMapper;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Infrastructure.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticleResponseDto>();

            CreateMap<Category, CategoryCreateRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>();

            CreateMap<Notification, NotificationResponseDto>()
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Article));

            CreateMap<RegisterRequestDto, AppUser>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username));
        }
    }
}