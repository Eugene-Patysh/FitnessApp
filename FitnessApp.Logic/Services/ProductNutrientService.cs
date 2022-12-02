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
    public class ProductNutrientService : BaseService, IProductNutrientService
    {
        public ProductNutrientService(ProductContext context) : base(context)
        {

        }
        public async Task<ProductNutrientDto[]> GetAllAsync()
        {
            var productNutrientDbs = await _context.ProductNutrients.ToArrayAsync();

            return ProductNutrientBuilder.Build(productNutrientDbs);
        }
        public Task<ProductNutrientDto> GetByIdAsync(int productNutrientDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(ProductNutrientDto productNutrientDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(ProductNutrientDto productNutrientDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int productNutrientDtoId)
        {
            throw new NotImplementedException();
        }  
    }
}
