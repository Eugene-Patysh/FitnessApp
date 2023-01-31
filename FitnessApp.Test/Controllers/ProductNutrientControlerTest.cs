﻿using FitnessApp.Localization;
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
    public class ProductNutrientControlerTest
    {
        private readonly ProductNutrientValidator validator;
        private readonly ProductNutrientController productNutrientController;
        private readonly Mock<IStringLocalizer<SharedResource>> sharedLocalizer;

        public ProductNutrientControlerTest()
        {
            sharedLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            validator = new(sharedLocalizer.Object);
            var _validator = new CustomValidator<ProductNutrientDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _productNutrientService = new ProductNutrientService(dbContext, _validator, sharedLocalizer.Object);
            productNutrientController = new ProductNutrientController(_productNutrientService, _validator, sharedLocalizer.Object);
            HelpTestCreateFromArray();
        }

        internal async void HelpTestCreateFromArray()
        {
            ProductNutrientDto[] productNutrients =
            {
                new ProductNutrientDto() { ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                new ProductNutrientDto() { ProductId = 2, NutrientId = 2, TreatingTypeId = 2, Quality = 2.2 },
                new ProductNutrientDto() { ProductId = 3, NutrientId = 3, TreatingTypeId = 3, Quality = 3.3 }
            };

            foreach (var o in productNutrients)
            {
                await productNutrientController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var productNutrients = await productNutrientController.GetAllAsync();

            Assert.NotNull(productNutrients);
            Assert.NotEmpty(productNutrients);
            Assert.True(productNutrients.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            ProductNutrientDto[] productNutrients =
            {
                new ProductNutrientDto() { ProductId = 1, Product = new ProductDto() {Title = "xXXBanana", ProductSubCategoryId = 1}, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                new ProductNutrientDto() { ProductId = 2, Product = new ProductDto() {Title = "PotXxXato", ProductSubCategoryId = 2}, NutrientId = 2, TreatingTypeId = 2, Quality = 2.2 },
                new ProductNutrientDto() { ProductId = 3, Product = new ProductDto() {Title = "CucumberXXx", ProductSubCategoryId = 3}, NutrientId = 3, TreatingTypeId = 3, Quality = 3.3 }
            };

            foreach (var o in productNutrients)
            {
                await productNutrientController.CreateAsync(o);
            }

            var allAfterPagination = await productNutrientController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "productTitle", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Product.Title.Equals("CucumberXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var productNutrient1 = await productNutrientController.GetByIdAsync(1);
            var productNutrient2 = await productNutrientController.GetByIdAsync(2);

            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.GetByIdAsync(0));
            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.GetByIdAsync(null));
            Assert.NotNull(productNutrient1);
            Assert.True(productNutrient1?.Id == 1);
            Assert.NotNull(productNutrient2);
            Assert.True(productNutrient2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var productNutrient = new ProductNutrientDto() { ProductId = 4, NutrientId = 4, TreatingTypeId = 4, Quality = 4.4 };

            await productNutrientController.CreateAsync(productNutrient);
            var productNutrients = await productNutrientController.GetAllAsync();
            var createdProductNutrient = productNutrients.FirstOrDefault(o => o.Quality == 4.4);

            Assert.True(validator.TestValidate(productNutrient, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductNutrient")).IsValid);
            Assert.NotNull(createdProductNutrient);
            Assert.NotNull(createdProductNutrient?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var productNutrient = new ProductNutrientDto() { Id = 3, ProductId = 5, NutrientId = 5, TreatingTypeId = 5, Quality = 5.5 };
            await productNutrientController.UpdateAsync(productNutrient);
            var productNutrients = await productNutrientController.GetAllAsync();
            var updatedProductNutrient = productNutrients.FirstOrDefault(o => o.Quality == 5.5);

            Assert.True(validator.TestValidate(productNutrient, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateProductNutrient")).IsValid);
            Assert.NotNull(updatedProductNutrient);
            Assert.True(updatedProductNutrient?.Id.Equals(3));
            Assert.True(updatedProductNutrient?.Quality.Equals(5.5));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var productNutrient = new ProductNutrientDto() { ProductId = 5, NutrientId = 5, TreatingTypeId = 5, Quality = 5.5 };
            await productNutrientController.CreateAsync(productNutrient);
            var productNutrients = await productNutrientController.GetAllAsync();
            var forDelete = productNutrients.FirstOrDefault(o => o.Quality == 5.5);

            await productNutrientController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.DeleteAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productNutrientController.DeleteAsync(0));
        }

        [Fact]
        public void Validate_SadCase()
        {
            ProductNutrientDto[] productNutrients =
            {
                    new ProductNutrientDto() { ProductId = 0, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                    new ProductNutrientDto() { ProductId = 1, NutrientId = 0, TreatingTypeId = 1, Quality = 1.1 },
                    new ProductNutrientDto() { ProductId = 1, NutrientId = 1, TreatingTypeId = 0, Quality = 1.1 },
                    new ProductNutrientDto() { ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = -1.1 },
                    new ProductNutrientDto() { ProductId = -1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                    new ProductNutrientDto() { ProductId = 1, NutrientId = -1, TreatingTypeId = 1, Quality = 1.1 },
                    new ProductNutrientDto() { ProductId = 1, NutrientId = 1, TreatingTypeId = -1, Quality = 1.1 }
                };

            ProductNutrientDto[] productNutrients1 =
            {
                    new ProductNutrientDto() { Id = 0, ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                    new ProductNutrientDto() { Id = -1, ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 }
                };

            var productNutrient1 = new ProductNutrientDto() { Id = 1, ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 };
            var productNutrient2 = new ProductNutrientDto() { Id = null, ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 };

            foreach (var o in productNutrients)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in productNutrients1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddProductNutrient")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateProductNutrient")).IsValid);
            }

            Assert.False(validator.TestValidate(productNutrient1, v => v.IncludeRuleSets("AddProductNutrient")).IsValid);
            Assert.False(validator.TestValidate(productNutrient2, v => v.IncludeRuleSets("UpdateProductNutrient")).IsValid);
        }
    }
}
