using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface INutrientCategoryService
    {
        Task<ICollection<NutrientCategoryDto>> GetAllAsync();
        Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryDtoId);
        Task CreateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task UpdateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task DeleteAsync(int? nutrientCategoryDtoId);
    }
}
