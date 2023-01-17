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

        public async Task<ICollection<ProductSubCategoryDto>> GetAllAsync()
        {
            var subCategoryDbs = await _context.ProductSubCategories.ToListAsync().ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDbs);
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

        public async Task<ProductSubCategoryDto> GetByIdAsync(int? productSubCategoryDtoId)
        {
            if (productSubCategoryDtoId == null)
            {
                throw new ValidationException("Product subcategory Id can't be null.");
            }

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDtoId).ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDb);
        }

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
