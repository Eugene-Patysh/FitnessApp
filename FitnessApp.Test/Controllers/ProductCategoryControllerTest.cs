using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FitnessApp.Web.Controllers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace FitnessApp.Tests.Controllers
{
    public class ProductCategoryControllerTest
    {
        private readonly ProductCategoryValidator validator = new();
        private readonly ProductCategoryController productCategoryController;

        public ProductCategoryControllerTest()
        {
            var _validator = new CustomValidator<ProductCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _productCategoryService = new ProductCategoryService(dbContext, _validator);
            productCategoryController = new ProductCategoryController(_productCategoryService, _validator);
            HelpTestCreateFromArrayAsync();
        }

        internal async void HelpTestCreateFromArrayAsync()
        {
            ProductCategoryDto[] productCategories =
            {
                new ProductCategoryDto() { Title = "Meat" },
                new ProductCategoryDto() { Title = "Vegetables" },
                new ProductCategoryDto() { Title = "Fruits" }
             };

            foreach (var o in productCategories)
            {
                await productCategoryController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {

            var categories = await productCategoryController.GetAllAsync();

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
                await productCategoryController.CreateAsync(o);
            }

            var allAfterPagination = await productCategoryController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("FruitsXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var category1 = await productCategoryController.GetByIdAsync(1);
            var category2 = await productCategoryController.GetByIdAsync(2);;

            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryController.GetByIdAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => productCategoryController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productCategoryController.GetByIdAsync(0));
            Assert.NotNull(category1);
            Assert.True(category1?.Id == 1);
            Assert.NotNull(category2);
            Assert.True(category2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var category = new ProductCategoryDto() { Id = null, Title = "MeatTT" };

            await productCategoryController.CreateAsync(category);
            var categories = await productCategoryController.GetAllAsync();
            var createdCategory = categories.FirstOrDefault(o => o.Title == "MeatTT");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductCategory")).IsValid);
            Assert.NotNull(createdCategory);
            Assert.NotNull(createdCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var category = new ProductCategoryDto() { Id = 3, Title = "XXX" };
            await productCategoryController.UpdateAsync(category);
            var categories = await productCategoryController.GetAllAsync();
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
            await productCategoryController.CreateAsync(category);
            var categories = await productCategoryController.GetAllAsync();
            var forDelete = categories.FirstOrDefault(o => o.Title == "Berries");

            await productCategoryController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => productCategoryController.GetByIdAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productCategoryController.DeleteAsync(0));
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
