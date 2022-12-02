using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services 
{
    public class ProductSubCategoryService : BaseService, IProductSubCategoryService
    {
        public ProductSubCategoryService(ProductContext context) : base(context)
        {

        }
        public async Task<ProductSubCategoryDto[]> GetAllAsync()
        {
            var subCategoryDbs = await _context.ProductSubCategories.ToArrayAsync();

            return ProductSubCategoryBuilder.Build(subCategoryDbs);
        }
        public Task<ProductSubCategoryDto> GetByIdAsync(int productSubCategoryDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(ProductSubCategoryDto productSubCategoryDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(ProductSubCategoryDto productSubCategoryDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int productSubCategoryDtoId)
        {
            throw new NotImplementedException();
        }
    }
}
