using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface IProductSubCategoryService
    {
        Task<ProductSubCategoryDto[]> GetAllAsync();
        Task<ProductSubCategoryDto> GetByIdAsync(int productSubCategoryDtoId);
        Task CreateAsync(ProductSubCategoryDto productSubCategoryDto);
        Task UpdateAsync(ProductSubCategoryDto productSubCategoryDto);
        Task DeleteAsync(int productSubCategoryDtoId);
    }
}
