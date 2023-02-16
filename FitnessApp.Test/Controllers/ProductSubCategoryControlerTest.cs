using EventBus.Base.Standard;
using FitnessApp.Localization;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FitnessApp.Web.Controllers;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace FitnessApp.Tests.Controllers
{
    public class ProductSubCategoryControlerTest
    {
        private readonly ProductSubCategoryValidator validator;
        private readonly ProductSubCategoryController productSubCategoryController;
        private readonly Mock<IStringLocalizer<SharedResource>> sharedLocalizer;
        private readonly Mock<IEventBus> eventBus;

        public ProductSubCategoryControlerTest()
        {
            eventBus = new Mock<IEventBus>();
            sharedLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            validator = new(sharedLocalizer.Object);
            var _validator = new CustomValidator<ProductSubCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _productSubCategoryService = new ProductSubCategoryService(dbContext, _validator, sharedLocalizer.Object, eventBus.Object);
            productSubCategoryController = new ProductSubCategoryController(_productSubCategoryService, _validator, sharedLocalizer.Object, eventBus.Object);
            HelpTestCreateFromArrayAsync();
        }

        internal async void HelpTestCreateFromArrayAsync()
        {
            ProductSubCategoryDto[] productSubCategories =
            {
                new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "Exotic" },
                new ProductSubCategoryDto() { ProductCategoryId = 2, Title = "Tuberous" },
                new ProductSubCategoryDto() { ProductCategoryId = 3, Title = "Forest" }
             };

            foreach (var o in productSubCategories)
            {
                await productSubCategoryController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var subCategories = await productSubCategoryController.GetAllAsync();

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
                await productSubCategoryController.CreateAsync(o);
            }

            var allAfterPagination = await productSubCategoryController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("ForestXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var subCategory1 = await productSubCategoryController.GetByIdAsync(1);
            var subCategory2 = await productSubCategoryController.GetByIdAsync(2); ;

            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.GetByIdAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.GetByIdAsync(0));
            Assert.NotNull(subCategory1);
            Assert.True(subCategory1?.Id == 1);
            Assert.NotNull(subCategory2);
            Assert.True(subCategory2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { ProductCategoryId = 1, Id = null, Title = "ExoticTT" };

            await productSubCategoryController.CreateAsync(subCategory);
            var subCategories = await productSubCategoryController.GetAllAsync();
            var createdSubCategory = subCategories.FirstOrDefault(o => o.Title == "ExoticTT");

            Assert.True(validator.TestValidate(subCategory, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductSubCategory")).IsValid);
            Assert.NotNull(createdSubCategory);
            Assert.NotNull(createdSubCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { ProductCategoryId = 1, Id = 3, Title = "XXX" };
            await productSubCategoryController.UpdateAsync(subCategory);
            var subCategories = await productSubCategoryController.GetAllAsync();
            var updatedSubCategory = subCategories.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(subCategory, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateProductSubCategory")).IsValid);
            Assert.NotNull(updatedSubCategory);
            Assert.True(updatedSubCategory?.Id.Equals(3));
            Assert.True(updatedSubCategory?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var subCategory = new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "Berries" };

            await productSubCategoryController.CreateAsync(subCategory);
            var subCategories = await productSubCategoryController.GetAllAsync();
            var forDelete = subCategories.FirstOrDefault(o => o.Title == "Berries");

            await productSubCategoryController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.GetByIdAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productSubCategoryController.DeleteAsync(0));
        }

        [Fact]
        public void Validate_SadCase()
        {
            ProductSubCategoryDto[] productSubCategories =
                {
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = null },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "1meat23" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = " " },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = ",meat;" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                    new ProductSubCategoryDto() { ProductCategoryId = 0, Title = "Beans" },
                    new ProductSubCategoryDto() { ProductCategoryId = null, Title = "Beans" },
                    new ProductSubCategoryDto() { ProductCategoryId = -1, Title = "Beans" }
                };

            ProductSubCategoryDto[] productSubCategories1 =
            {
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Id = -3, Title = "Berries" },
                    new ProductSubCategoryDto() { ProductCategoryId = 1, Id = 0, Title = "Meat" }
                };

            var subCategory1 = new ProductSubCategoryDto() { ProductCategoryId = 1, Id = 1, Title = "Berries" };
            var subCategory2 = new ProductSubCategoryDto() { ProductCategoryId = 1, Id = null, Title = "Meat" };


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
