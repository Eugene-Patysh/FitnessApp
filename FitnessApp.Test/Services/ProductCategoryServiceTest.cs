using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using System.Globalization;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FitnessApp.Tests.Services
{
    public class ProductCategoryServiceTests
    {
        private readonly ProductCategoryValidator validator;
        private readonly IProductCategoryService productCategoryService;

        public ProductCategoryServiceTests()
        {
            validator = new();
            var _validator = new CustomValidator<ProductCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            productCategoryService = new ProductCategoryService(dbContext, _validator);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            ProductCategoryDto[] productCategories =
            {
                    new ProductCategoryDto() { Title = "Meat" },
                    new ProductCategoryDto() { Title = "Vegetables" },
                    new ProductCategoryDto() { Title = "Fruits" }
                };

            foreach (var o in productCategories)
            {
                productCategoryService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var categories = await productCategoryService.GetAllAsync();

            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.True(categories.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            ProductCategoryDto[] productCategories =
             {
                new ProductCategoryDto() { Title = "xXXMeat" },
                new ProductCategoryDto() { Title = "VegeXxXtables" },
                new ProductCategoryDto() { Title = "FruitsXXx" }
            };

            foreach (var o in productCategories)
            {
                await productCategoryService.CreateAsync(o);
            }

            var allAfterPagination = await productCategoryService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("FruitsXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var category1 = await productCategoryService.GetByIdAsync(1);
            var category2 = await productCategoryService.GetByIdAsync(2);
            var category3 = await productCategoryService.GetByIdAsync(-3);
            var category4 = await productCategoryService.GetByIdAsync(0);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryService.GetByIdAsync(null));
            Assert.NotNull(category1);
            Assert.True(category1?.Id == 1);
            Assert.NotNull(category2);
            Assert.True(category2?.Id == 2);
            Assert.Null(category3);
            Assert.Null(category4);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var category = new ProductCategoryDto() { Id = null, Title = "MeatTT" };

            await productCategoryService.CreateAsync(category);
            var categories = await productCategoryService.GetAllAsync();
            var createdCategory = categories.FirstOrDefault(o => o.Title == "MeatTT");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductCategory")).IsValid);
            Assert.NotNull(createdCategory);
            Assert.NotNull(createdCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var category = new ProductCategoryDto() { Id = 3, Title = "XXX" };
            await productCategoryService.UpdateAsync(category);
            var categories = await productCategoryService.GetAllAsync();
            var updatedCategory = categories.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateProductCategory")).IsValid);
            Assert.NotNull(updatedCategory);
            Assert.True(updatedCategory?.Id.Equals(3));
            Assert.True(updatedCategory?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var category = new ProductCategoryDto() { Title = "Berries" };
            await productCategoryService.CreateAsync(category);
            var categories = await productCategoryService.GetAllAsync();
            var forDelete = categories.FirstOrDefault(o => o.Title == "Berries");

            await productCategoryService.DeleteAsync(forDelete?.Id);
            var deletedCategory = await productCategoryService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryService.DeleteAsync(0));
            Assert.Null(deletedCategory);
        }

        [Fact]
        public void Validate_SadCase()
        {
            ProductCategoryDto[] productCategories =
            {
                    new ProductCategoryDto() { Title = "" },
                    new ProductCategoryDto() { Title = null },
                    new ProductCategoryDto() { Title = "1meat23" },
                    new ProductCategoryDto() { Title = " " },
                    new ProductCategoryDto() { Title = ",meat;" },
                    new ProductCategoryDto() { Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" }
                };

            ProductCategoryDto[] productCategories1 =
            {
                    new ProductCategoryDto() { Id = -3, Title = "Berries" },
                    new ProductCategoryDto() { Id = 0, Title = "Meat" }
                };

            var category1 = new ProductCategoryDto() { Id = 1, Title = "Berries" };
            var category2 = new ProductCategoryDto() { Id = null, Title = "Meat" };


            foreach (var o in productCategories)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in productCategories1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddProductCategory")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateProductCategory")).IsValid);
            }

            Assert.False(validator.TestValidate(category1, v => v.IncludeRuleSets("AddProductCategory")).IsValid);
            Assert.False(validator.TestValidate(category2, v => v.IncludeRuleSets("UpdateProductCategory")).IsValid);
        }
    }
}
