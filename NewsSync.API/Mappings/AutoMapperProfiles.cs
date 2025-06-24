
using AutoMapper;
using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;


namespace NewsSync.API.Mappings
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