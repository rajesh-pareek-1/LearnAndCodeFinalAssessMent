using NewsSync.API.Application.DTOs;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public static class ArticleCategoryPredictor
    {
        public static CategoryResponseDto PredictCategory(Article article, List<CategoryResponseDto> categories)
        {
            var content = $"{article.Headline} {article.Description}".ToLower();

            foreach (var category in categories)
            {
                if (content.Contains(category.Name, StringComparison.CurrentCultureIgnoreCase))
                    return category;
            }

            return categories.FirstOrDefault(c => c.Name.Equals("General", StringComparison.OrdinalIgnoreCase)) ?? categories.First();
        }
    }
}
