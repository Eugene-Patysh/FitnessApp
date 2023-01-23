using FitnessApp.Data;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly ICustomValidator<ProductDto> _validator;

        public ProductService(ProductContext context, ICustomValidator<ProductDto> validator) : base(context)
        {
            _validator = validator;
        }

        /// <summary> Gets all products from DB. </summary>
        /// <returns> Returns collection of products. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<ProductDto>> GetAllAsync()
        {
            var productDbs = await _context.Products.ToListAsync().ConfigureAwait(false);

            return ProductBuilder.Build(productDbs) ?? throw new Exception($"There are not objects of products.");
        }

        /// <summary> Outputs paginated products from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of products. </returns>
        public async Task<PaginationResponse<ProductDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(c => c.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy)
                {
                    case "title": query = request.Ascending ? query.OrderBy(c => c.Title) : query.OrderByDescending(c => c.Title); break;
                }
            }

            var categoryDbs = await query.ToListAsync().ConfigureAwait(false);

            var total = categoryDbs.Count;
            var categoryDtos = ProductBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<ProductDto>
            {
                Total = total,
                Values = categoryDtos
            };
        }

        /// <summary> Gets product from DB by Id. </summary>
        /// <param name="productId"></param>
        /// <returns> Returns object of product with Id: <paramref name="productId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<ProductDto> GetByIdAsync(int? productId)
        {
            if (productId == null)
            {
                throw new ValidationException("Product Id can't be null.");
            }

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productId).ConfigureAwait(false);

            return ProductBuilder.Build(productDb);
        }

        /// <summary> Creates new product. </summary>
        /// <param name="productDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateAsync(ProductDto productDto)
        {
            _validator.Validate(productDto,"AddProduct");

            productDto.Created = DateTime.UtcNow;
            productDto.Updated = DateTime.UtcNow;

            await _context.Products.AddAsync(ProductBuilder.Build(productDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Product has not been created. {ex.Message}.");
            }
        }

        /// <summary> Updates product in DB. </summary>
        /// <param name="productDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task UpdateAsync(ProductDto productDto)
        {
            _validator.Validate(productDto, "UpdateProduct");

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productDto.Id).ConfigureAwait(false);

            if (productDb != null)
            {
                productDb.Title = productDto.Title;
                productDb.ProductSubCategoryId = productDto.ProductSubCategoryId;
                productDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        /// <summary> Deletes product from DB. </summary>
        /// <param name="productId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int? productId)
        {
            if (productId == null)
            {
                throw new ValidationException("Invalid product Id.");
            }

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productId).ConfigureAwait(false);

            if (productDb != null)
            {
                _context.Products.Remove(productDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
