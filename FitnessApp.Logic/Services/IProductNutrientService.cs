using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface IProductNutrientService
    {
        Task<ProductNutrientDto[]> GetAllAsync();
        Task<ProductNutrientDto> GetByIdAsync(int? productNutrientDtoId);
        Task CreateAsync(ProductNutrientDto productNutrientDto);
        Task UpdateAsync(ProductNutrientDto productNutrientDto);
        Task DeleteAsync(int? productNutrientDtoId);
    }
}
