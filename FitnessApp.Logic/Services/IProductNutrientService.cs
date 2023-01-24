using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface IProductNutrientService
    {
        Task<ICollection<ProductNutrientDto>> GetAllAsync();
        Task<PaginationResponse<ProductNutrientDto>> GetPaginationAsync(PaginationRequest request);
        Task<ProductNutrientDto> GetByIdAsync(int? productNutrientId);
        Task CreateAsync(ProductNutrientDto productNutrientDto);
        Task UpdateAsync(ProductNutrientDto productNutrientDto);
        Task DeleteAsync(int? productNutrientId);
    }
}
