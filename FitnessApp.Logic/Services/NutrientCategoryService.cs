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
    public class NutrientCategoryService : BaseService, INutrientCategoryService
    {
        public NutrientCategoryService(ProductContext context) : base(context)
        {

        }
        public async Task<NutrientCategoryDto[]> GetAllAsync()
        {
            var nutrientCategoryDbs = await _context.NutrientCategories.ToArrayAsync();

            return NutrientCategoryBuilder.Build(nutrientCategoryDbs);
        }
        public Task<NutrientCategoryDto> GetByIdAsync(int nutrientCategoryDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(NutrientCategoryDto nutrientCategoryDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(NutrientCategoryDto nutrientCategoryDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int nutrientCategoryDtoId)
        {
            throw new NotImplementedException();
        }   
    }
}
