using FitnessApp.Data.Models;
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
    public class ProductControlerTest
    {
        private readonly ProductValidator validator;
        private readonly ProductController productController;

        public ProductControlerTest()
        {
            validator = new();
            var _validator = new CustomValidator<ProductDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _productService = new ProductService(dbContext, _validator);
            productController = new ProductController(_productService, _validator);
            HelpTestCreateFromArrayAsync();
        }

        internal async void HelpTestCreateFromArrayAsync()
        {
            ProductDto[] products =
            {
                new ProductDto() { ProductSubCategoryId = 1, Title = "Banana" },
                new ProductDto() { ProductSubCategoryId = 2, Title = "Potato" },
                new ProductDto() { ProductSubCategoryId = 3, Title = "Cucumber" }
             };

            foreach (var o in products)
            {
                await productController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var products = await productController.GetAllAsync();

            Assert.NotNull(products);
            Assert.NotEmpty(products);
            Assert.True(products.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            ProductDto[] products =
             {
                new ProductDto() { ProductSubCategoryId = 1, Title = "xXXBanana" },
                new ProductDto() { ProductSubCategoryId = 2, Title = "PotXxXato" },
                new ProductDto() { ProductSubCategoryId = 3, Title = "CucumberXXx" }
            };

            foreach (var o in products)
            {
                await productController.CreateAsync(o);
            }

            var allAfterPagination = await productController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("CucumberXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var product1 = await productController.GetByIdAsync(1);
            var product2 = await productController.GetByIdAsync(2); ;

            await Assert.ThrowsAnyAsync<ValidationException>(() => productController.GetByIdAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => productController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => productController.GetByIdAsync(0));
            Assert.NotNull(product1);
            Assert.True(product1?.Id == 1);
            Assert.NotNull(product2);
            Assert.True(product2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var product = new ProductDto() { ProductSubCategoryId = 1, Id = null, Title = "BananaTT" };

            await productController.CreateAsync(product);
            var products = await productController.GetAllAsync();
            var createdProduct = products.FirstOrDefault(o => o.Title == "BananaTT");

            Assert.True(validator.TestValidate(product, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProduct")).IsValid);
            Assert.NotNull(createdProduct);
            Assert.NotNull(createdProduct?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var product = new ProductDto() { ProductSubCategoryId = 1, Id = 3, Title = "XXX" };
            await productController.UpdateAsync(product);
            var products = await productController.GetAllAsync();
            var updatedProduct = products.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(product, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateProduct")).IsValid);
            Assert.NotNull(updatedProduct);
            Assert.True(updatedProduct?.Id.Equals(3));
            Assert.True(updatedProduct?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var product = new ProductDto() { ProductSubCategoryId = 1, Title = "Cucumber" };

            await productController.CreateAsync(product);
            var products = await productController.GetAllAsync();
            var forDelete = products.FirstOrDefault(o => o.Title == "Cucumber");

            await productController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => productController.GetByIdAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productController.DeleteAsync(0));
        }

        [Fact]
        public void Validate_SadCase()
        {
            ProductDto[] products =
                {
                    new ProductDto() { ProductSubCategoryId = 1, Title = "" },
                    new ProductDto() { ProductSubCategoryId = 1, Title = null },
                    new ProductDto() { ProductSubCategoryId = 1, Title = "1banana23" },
                    new ProductDto() { ProductSubCategoryId = 1, Title = " " },
                    new ProductDto() { ProductSubCategoryId = 1, Title = ",banana;" },
                    new ProductDto() { ProductSubCategoryId = 1, Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                    new ProductDto() { ProductSubCategoryId = 0, Title = "Cucumber" },
                    new ProductDto() { ProductSubCategoryId = null, Title = "Cucumber" },
                    new ProductDto() { ProductSubCategoryId = -1, Title = "Cucumber" }
                };

            ProductDto[] products1 =
            {
                    new ProductDto() { ProductSubCategoryId = 1, Id = -3, Title = "Cucumber" },
                    new ProductDto() { ProductSubCategoryId = 2,  Id = 0, Title = "Banana" }
                };

            var product1 = new ProductDto() { ProductSubCategoryId = 1, Id = 1, Title = "Cucumber" };
            var product2 = new ProductDto() { ProductSubCategoryId = 2, Id = null, Title = "Banana" };


            foreach (var o in products)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in products1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddProduct")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateProduct")).IsValid);
            }

            Assert.False(validator.TestValidate(product1, v => v.IncludeRuleSets("AddProduct")).IsValid);
            Assert.False(validator.TestValidate(product2, v => v.IncludeRuleSets("UpdateProduct")).IsValid);
        }
    }
}
