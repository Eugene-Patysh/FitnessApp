using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace FitnessApp.Tests.Services
{
    public class ProductSubCategoryServiceTest
    {
        private readonly ProductSubCategoryValidator validator;
        private readonly IProductSubCategoryService productSubCategoryService;

        public ProductSubCategoryServiceTest()
        {
            validator = new();
            var _validator = new CustomValidator<ProductSubCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            productSubCategoryService = new ProductSubCategoryService(dbContext, _validator);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            ProductSubCategoryDto[] productSubCategories =
            {
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "Exotic" },
                    new ProductSubCategoryDto() { ProductCategoryId = 2, Title = "Tuberous" },
                    new ProductSubCategoryDto() { ProductCategoryId = 3, Title = "Forest" }
                };

            foreach (var o in productSubCategories)
            {
                productSubCategoryService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var subCategories = await productSubCategoryService.GetAllAsync();

            Assert.NotNull(subCategories);
            Assert.NotEmpty(subCategories);
            Assert.True(subCategories.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            ProductSubCategoryDto[] productSubCategories =
             {
                new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "xXXExotic" },
                new ProductSubCategoryDto() { ProductCategoryId = 2, Title = "TubeXxXrous" },
                new ProductSubCategoryDto() { ProductCategoryId = 3, Title = "ForestXXx" }
            };

            foreach (var o in productSubCategories)
            {
                await productSubCategoryService.CreateAsync(o);
            }

            var allAfterPagination = await productSubCategoryService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("ForestXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var subCategory1 = await productSubCategoryService.GetByIdAsync(1);
            var subCategory2 = await productSubCategoryService.GetByIdAsync(2);
            var subCategory3 = await productSubCategoryService.GetByIdAsync(-3);
            var subCategory4 = await productSubCategoryService.GetByIdAsync(0);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productSubCategoryService.GetByIdAsync(null));
            Assert.NotNull(subCategory1);
            Assert.True(subCategory1?.Id == 1);
            Assert.NotNull(subCategory2);
            Assert.True(subCategory2?.Id == 2);
            Assert.Null(subCategory3);
            Assert.Null(subCategory4);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { Id = null, ProductCategoryId = 1, Title = "ExoticTT" };

            await productSubCategoryService.CreateAsync(subCategory);
            var subCategories = await productSubCategoryService.GetAllAsync();
            var createdSubCategory = subCategories.FirstOrDefault(o => o.Title == "ExoticTT");

            Assert.True(validator.TestValidate(subCategory, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductSubCategory")).IsValid);
            Assert.NotNull(createdSubCategory);
            Assert.NotNull(createdSubCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { Id = 3, ProductCategoryId = 1, Title = "XXX" };
            await productSubCategoryService.UpdateAsync(subCategory);
            var subCategories = await productSubCategoryService.GetAllAsync();
            var updatedSubCategory = subCategories.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(subCategory, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateProductSubCategory")).IsValid);
            Assert.NotNull(updatedSubCategory);
            Assert.True(updatedSubCategory?.Id.Equals(3));
            Assert.True(updatedSubCategory?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "Beans" };
            await productSubCategoryService.CreateAsync(subCategory);
            var subCategories = await productSubCategoryService.GetAllAsync();
            var forDelete = subCategories.FirstOrDefault(o => o.Title == "Beans");

            await productSubCategoryService.DeleteAsync(forDelete?.Id);
            var deletedSubCategory = await productSubCategoryService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productSubCategoryService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productSubCategoryService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productSubCategoryService.DeleteAsync(0));
            Assert.Null(deletedSubCategory);
        }

        [Fact]
        public void Validate_SadCase()
        {
            ProductSubCategoryDto[] productSubCategories =
            {
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = null },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "1exotic23" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = " " },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = ",exotic;" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                    new ProductSubCategoryDto() { ProductCategoryId = 0, Title = "Beans" },
                    new ProductSubCategoryDto() { ProductCategoryId = null, Title = "Beans" },
                    new ProductSubCategoryDto() { ProductCategoryId = -1, Title = "Beans" }

                };

            ProductSubCategoryDto[] productSubCategories1 =
            {
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Id = -3, Title = "Beans" },
                    new ProductSubCategoryDto() { ProductCategoryId = 2,  Id = 0, Title = "Exotic" }
                };

            var subCategory1 = new ProductSubCategoryDto() { ProductCategoryId = 1, Id = 1, Title = "Beans" };
            var subCategory2 = new ProductSubCategoryDto() { ProductCategoryId = 2, Id = null, Title = "Exotic" };


            foreach (var o in productSubCategories)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in productSubCategories1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddProductSubCategory")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateProductSubCategory")).IsValid);
            }

            Assert.False(validator.TestValidate(subCategory1, v => v.IncludeRuleSets("AddProductSubCategory")).IsValid);
            Assert.False(validator.TestValidate(subCategory2, v => v.IncludeRuleSets("UpdateProductSubCategory")).IsValid);
        }
    }
}
