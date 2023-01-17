using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface IProductNutrientService
    {
        Task<ICollection<ProductNutrientDto>> GetAllAsync();
        Task<ProductNutrientDto> GetByIdAsync(int? productNutrientDtoId);
        Task CreateAsync(ProductNutrientDto productNutrientDto);
        Task UpdateAsync(ProductNutrientDto productNutrientDto);
        Task DeleteAsync(int? productNutrientDtoId);
    }
}
