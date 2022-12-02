using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface ITreatingTypeService
    {
        Task<TreatingTypeDto[]> GetAllAsync();
        Task<TreatingTypeDto> GetByIdAsync(int treatingTypeDtoId);
        Task CreateAsync(TreatingTypeDto treatingTypeDto);
        Task UpdateAsync(TreatingTypeDto treatingTypeDto);
        Task DeleteAsync(int treatingTypeDtoId);
    }
}
