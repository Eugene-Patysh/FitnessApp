using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        public ProductCategoryService(ProductContext context) : base(context)
        {

        }

        public async Task<ProductCategoryDto[]> GetAllAsync()
        {
            var categoryDbs = await _context.ProductCategories.ToArrayAsync();
            
            return ProductCategoryBuilder.Build(categoryDbs);
        }

        public async Task<ProductCategoryDto> GetByIdAsync(int productCategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(ProductCategoryDto productCategoryDto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ProductCategoryDto productCategoryDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int productCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
