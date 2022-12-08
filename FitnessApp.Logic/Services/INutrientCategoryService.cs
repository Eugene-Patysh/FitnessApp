using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface INutrientCategoryService
    {
        Task<NutrientCategoryDto[]> GetAllAsync();
        Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryDtoId);
        Task CreateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task UpdateAsync(NutrientCategoryDto nutrientCategoryDto);
        Task DeleteAsync(int? nutrientCategoryDtoId);
    }
}
