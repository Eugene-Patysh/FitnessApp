using FitnessApp.Data;
using FitnessApp.Data.Models;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        public ProductCategoryService(ProductContext context) : base(context)
        {

        }

        public ProductCategoryDto[] GetWithCondition(int pageNumber, int objectsNumber)
        {
            if (pageNumber <= 0 || objectsNumber <= 0)
            {
                return null;
            }
            var categoryDbs = _context.ProductCategories.OrderBy(_ => _.Title).Skip((pageNumber - 1) * objectsNumber).Take(objectsNumber).ToArray();
            return ProductCategoryBuilder.Build(categoryDbs);
        }

        public async Task<ProductCategoryDto[]> GetAllAsync() 
        {
            var categoryDbs = await _context.ProductCategories.ToArrayAsync().ConfigureAwait(false);

            return ProductCategoryBuilder.Build(categoryDbs);
        }

        public async Task<ProductCategoryDto> GetByIdAsync(int productCategoryDtoId)
        {
            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);
            return ProductCategoryBuilder.Build(categoryDb);
        }

        public async Task CreateAsync(ProductCategoryDto productCategoryDto)
        {
            await _context.ProductCategories.AddAsync(ProductCategoryBuilder.Build(productCategoryDto)).ConfigureAwait(false);
            productCategoryDto.Created = DateTime.UtcNow;
            productCategoryDto.Updated = DateTime.UtcNow;
            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { throw; }
        }

        public async Task UpdateAsync(ProductCategoryDto productCategoryDto)
        {
            if (productCategoryDto.Id <= 0 || string.IsNullOrEmpty(productCategoryDto.Title) || productCategoryDto.ProductSubCategories != null)
                {
                var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDto.Id).ConfigureAwait(false);
                if (categoryDb != null)
                {
                    categoryDb.Title = productCategoryDto.Title;
                    categoryDb.Updated = DateTime.UtcNow;
                }
            }
            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { throw; } //DbUpdateException DbUpdateConcurrencyException DbEntityValidationException NotSupportedException ObjectDisposedException InvalidOperationException
        }

        public async Task DeleteAsync(int productCategoryDtoId)
        {
            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);
            if (categoryDb != null)
            {
                _context.ProductCategories.Remove(categoryDb);
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
