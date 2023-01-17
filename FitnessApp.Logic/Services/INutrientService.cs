using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface INutrientService
    {
        Task<ICollection<NutrientDto>> GetAllAsync();
        Task<PaginationResponse<NutrientDto>> GetPaginationAsync(PaginationRequest request);
        Task<NutrientDto> GetByIdAsync(int? nutrientDtoId);
        Task CreateAsync(NutrientDto nutrientDto);
        Task UpdateAsync(NutrientDto nutrientDto);
        Task DeleteAsync(int? nutrientDtoId);
    }
}
