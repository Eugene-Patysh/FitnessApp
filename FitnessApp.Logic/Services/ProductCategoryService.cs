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
            //var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDto.Id).ConfigureAwait(false);
            //Не прокатит, т.к. нет Id.

            var categoryDb = new ProductCategoryDb() //Если надо изменять все поля включая коллекции и ссылочные типы данных
            {                                        //Тогда лучше сделать метод ReBuild во всех билдерах
                Id = productCategoryDto.Id,
                Title = productCategoryDto.Title,
                Created = productCategoryDto.Created,
                Updated = productCategoryDto.Updated
            };
            await _context.ProductCategories.AddAsync(categoryDb).ConfigureAwait(false);
            _context.SaveChanges();
            return;
        }
        public async Task UpdateAsync(ProductCategoryDto productCategoryDto)
        {
            var categoryDb = await _context.ProductCategories.FirstOrDefaultAsync(_ => _.Id == productCategoryDto.Id).ConfigureAwait(false);
            if (categoryDb != null)
            {
                categoryDb.Title = productCategoryDto.Title; //Надо ли изменять все поля включая коллекции и ссылочные типы данных
            }                                               // Как меняется дата?
            _context.SaveChanges();
            return;
        }
        public async Task DeleteAsync(int productCategoryDtoId)
        {
            var categoryDb = await _context.ProductCategories.FirstOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);
            if (categoryDb != null)
            {
                _context.ProductCategories.Remove(categoryDb);
            }
            _context.SaveChanges();
            return;
        }
        //public async void DeleteAsync(int productCategoryDtoId)
        //{
        //    var categoryDb = await _context.ProductCategories.FirstOrDefaultAsync(_ => _.Id == productCategoryDtoId);
        //    if (categoryDb != null)
        //    {
        //        _context.ProductCategories.Remove(categoryDb);
        //    }
        //    _context.SaveChanges();
        //}

        //public async Task<ProductCategoryDto> DeleteAsync(int productCategoryDtoId)
        //{
        //    var categoryDb = await _context.ProductCategories.FirstOrDefaultAsync(_ => _.Id == productCategoryDtoId);
        //    if (categoryDb != null)
        //    {
        //        _context.ProductCategories.Remove(categoryDb);
        //    }
        //    _context.SaveChanges();
        //    return ProductCategoryBuilder.Build(categoryDb);
        //}
    }
}
