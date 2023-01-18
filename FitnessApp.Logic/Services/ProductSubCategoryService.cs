using FitnessApp.Data;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services 
{
    public class ProductSubCategoryService : BaseService, IProductSubCategoryService
    {
        private readonly ICustomValidator<ProductSubCategoryDto> _validator;

        public ProductSubCategoryService(ProductContext context, ICustomValidator<ProductSubCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        /// <summary> Gets all product subcategories from DB. </summary>
        /// <returns> Returns collection of product subcategories. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<ProductSubCategoryDto>> GetAllAsync()
        {
            var subCategoryDbs = await _context.ProductSubCategories.ToListAsync().ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDbs) ?? throw new Exception($"There are not objects of product subcategories.");
        }

        /// <summary> Outputs paginated product subcategories from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of product subcategories. </returns>
        public async Task<PaginationResponse<ProductSubCategoryDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.ProductSubCategories.AsQueryable();

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
            var categoryDtos = ProductSubCategoryBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<ProductSubCategoryDto>
            {
                Total = total,
                Values = categoryDtos
            };
        }

        /// <summary> Gets product subcategory from DB by Id. </summary>
        /// <param name="productSubCategoryDtoId"></param>
        /// <returns> Returns object of product subcategory with Id: <paramref name="productSubCategoryDtoId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<ProductSubCategoryDto> GetByIdAsync(int? productSubCategoryDtoId)
        {
            if (productSubCategoryDtoId == null)
            {
                throw new ValidationException("Product subcategory Id can't be null.");
            }

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDtoId).ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDb);
        }

        /// <summary> Creates new product subcategory. </summary>
        /// <param name="productSubCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateAsync(ProductSubCategoryDto productSubCategoryDto)
        {
            _validator.Validate(productSubCategoryDto, "AddProductSubCategory");

            productSubCategoryDto.Created = DateTime.UtcNow;
            productSubCategoryDto.Updated = DateTime.UtcNow;
           
            await _context.ProductSubCategories.AddAsync(ProductSubCategoryBuilder.Build(productSubCategoryDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Product subcategory has not been created. {ex.Message}.");
            }
        }

        /// <summary> Updates product subcategory in DB. </summary>
        /// <param name="productSubCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task UpdateAsync(ProductSubCategoryDto productSubCategoryDto) 
        {
            _validator.Validate(productSubCategoryDto, "UpdateProductSubCategory");

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDto.Id).ConfigureAwait(false);

            if (subCategoryDb != null)
            {
                subCategoryDb.Title = productSubCategoryDto.Title;
                subCategoryDb.ProductCategoryId = productSubCategoryDto.ProductCategoryId;
                subCategoryDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product subcategory has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        /// <summary> Deletes product subcategory from DB. </summary>
        /// <param name="productSubCategoryDtoId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int? productSubCategoryDtoId)
        {
            if (productSubCategoryDtoId == null)
            {
                throw new ValidationException("Invalid product subcategory Id.");
            }

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDtoId).ConfigureAwait(false);

            if (subCategoryDb != null)
            {
                _context.ProductSubCategories.Remove(subCategoryDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product subcategory has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
