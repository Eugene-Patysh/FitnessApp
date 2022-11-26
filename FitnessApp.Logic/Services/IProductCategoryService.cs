using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryDto[]> GetAllAsync();
        Task<ProductCategoryDto> GetByIdAsync(int productCategoryDtoId);
        Task CreateAsync(ProductCategoryDto productCategoryDto);
        Task UpdateAsync(ProductCategoryDto productCategoryDto);
        Task DeleteAsync(int productCategoryDtoId);
        //public void DeleteAsync(int productCategoryDtoId);
        //public Task<ProductCategoryDto> DeleteAsync(int productCategoryDtoId)
    }
}
