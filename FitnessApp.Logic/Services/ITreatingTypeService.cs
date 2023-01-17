using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface ITreatingTypeService
    {
        Task<ICollection<TreatingTypeDto>> GetAllAsync();
        Task<PaginationResponse<TreatingTypeDto>> GetPaginationAsync(PaginationRequest request);
        Task<TreatingTypeDto> GetByIdAsync(int? treatingTypeDtoId);
        Task CreateAsync(TreatingTypeDto treatingTypeDto);
        Task UpdateAsync(TreatingTypeDto treatingTypeDto);
        Task DeleteAsync(int? treatingTypeDtoId);
    }
}
