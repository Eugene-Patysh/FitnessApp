using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface IProductCategoryService
    {
        Task<ICollection<ProductCategoryDto>> GetAllAsync();
        Task<ProductCategoryDto> GetByIdAsync(int? productCategoryDtoId);
        Task CreateAsync(ProductCategoryDto productCategoryDto);
        Task UpdateAsync(ProductCategoryDto productCategoryDto);
        Task DeleteAsync(int? productCategoryDtoId);
    }
}
