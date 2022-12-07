using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface INutrientService
    {
        Task<NutrientDto[]> GetAllAsync();
        Task<NutrientDto> GetByIdAsync(int? nutrientDtoId);
        Task CreateAsync(NutrientDto nutrientDto);
        Task UpdateAsync(NutrientDto nutrientDto);
        Task DeleteAsync(int? nutrientDtoId);
    }
}
