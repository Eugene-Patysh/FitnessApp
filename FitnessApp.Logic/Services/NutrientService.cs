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
    public class NutrientService : BaseService, INutrientService
    {
        public NutrientService(ProductContext context) : base(context)
        {

        }
        public async Task<NutrientDto[]> GetAllAsync()
        {
            var nutrientDbs = await _context.Nutrients.ToArrayAsync();

            return NutrientBuilder.Build(nutrientDbs);
        }
        public Task<NutrientDto> GetByIdAsync(int nutrientDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(NutrientDto nutrientDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(NutrientDto nutrientDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int nutrientDtoId)
        {
            throw new NotImplementedException();
        }  
    }
}
