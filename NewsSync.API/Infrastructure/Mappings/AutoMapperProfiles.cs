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
            CreateMap<Category, CategoryCreateRequestDto>().ReverseMap();
        }
    }
}