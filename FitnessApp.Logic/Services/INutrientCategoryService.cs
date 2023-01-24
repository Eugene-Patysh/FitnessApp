using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface INutrientCategoryService
    {
        Task<ICollection<NutrientCategoryDto>> GetAllAsync();
        Task<PaginationResponse<NutrientCategoryDto>> GetPaginationAsync(PaginationRequest request);
        Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryId);
        Task CreateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task UpdateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task DeleteAsync(int? nutrientCategoryId);
    }
}
