using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Services
{
    public interface IProductService
    {
        Task<ICollection<ProductDto>> GetAllAsync();
        Task<PaginationResponse<ProductDto>> GetPaginationAsync(PaginationRequest request);
        Task<ProductDto> GetByIdAsync(int? productId);
        Task CreateAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int? productId);
    }
}
