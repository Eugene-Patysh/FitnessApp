using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public interface IProductService
    {
        Task<ProductDto[]> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int productDtoId);
        Task CreateAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int productDtoId);
    }
}
