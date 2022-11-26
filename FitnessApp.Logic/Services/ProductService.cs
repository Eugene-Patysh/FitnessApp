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
    public class ProductService : BaseService, IProductService
    {
        public ProductService(ProductContext context) : base(context)
        {

        }
        public async Task<ProductDto[]> GetAllAsync()
        {
            var productDbs = await _context.Products.ToArrayAsync();

            return ProductBuilder.Build(productDbs);
        }
        public Task<ProductDto> GetByIdAsync(int productDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int productDtoId)
        {
            throw new NotImplementedException();
        }
    }
}
