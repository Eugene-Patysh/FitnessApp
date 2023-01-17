using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface IProductSubCategoryService
    {
        Task<ICollection<ProductSubCategoryDto>> GetAllAsync();
        Task<PaginationResponse<ProductSubCategoryDto>> GetPaginationAsync(PaginationRequest request);
        Task<ProductSubCategoryDto> GetByIdAsync(int? productSubCategoryDtoId);
        Task CreateAsync(ProductSubCategoryDto productSubCategoryDto);
        Task UpdateAsync(ProductSubCategoryDto productSubCategoryDto);
        Task DeleteAsync(int? productSubCategoryDtoId);
    }
}
