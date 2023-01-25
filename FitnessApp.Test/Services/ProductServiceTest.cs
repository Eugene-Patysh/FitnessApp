using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace FitnessApp.Tests.Services
{
    public class ProductServiceTest
    {
        private readonly ProductValidator validator;
        private readonly IProductService productService;

        public ProductServiceTest()
        {
            validator = new();
            var _validator = new CustomValidator<ProductDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            productService = new ProductService(dbContext, _validator);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            ProductDto[] products =
            {
                    new ProductDto() { ProductSubCategoryId = 1, Title = "Banana" },
                    new ProductDto() { ProductSubCategoryId = 2, Title = "Potato" },
                    new ProductDto() { ProductSubCategoryId = 3, Title = "Cucumber" }
                };

            foreach (var o in products)
            {
                productService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var products = await productService.GetAllAsync();

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
                await productService.CreateAsync(o);
            }

            var allAfterPagination = await productService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("CucumberXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var product1 = await productService.GetByIdAsync(1);
            var product2 = await productService.GetByIdAsync(2);
            var product3 = await productService.GetByIdAsync(-3);
            var product4 = await productService.GetByIdAsync(0);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productService.GetByIdAsync(null));
            Assert.NotNull(product1);
            Assert.True(product1?.Id == 1);
            Assert.NotNull(product2);
            Assert.True(product2?.Id == 2);
            Assert.Null(product3);
            Assert.Null(product4);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var product = new ProductDto() { Id = null, ProductSubCategoryId = 1, Title = "BananaTT" };

            await productService.CreateAsync(product);
            var products = await productService.GetAllAsync();
            var createdProduct = products.FirstOrDefault(o => o.Title == "BananaTT");

            Assert.True(validator.TestValidate(product, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProduct")).IsValid);
            Assert.NotNull(createdProduct);
            Assert.NotNull(createdProduct?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var product = new ProductDto() { Id = 3, ProductSubCategoryId = 1, Title = "XXX" };
            await productService.UpdateAsync(product);
            var products = await productService.GetAllAsync();
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
            await productService.CreateAsync(product);
            var products = await productService.GetAllAsync();
            var forDelete = products.FirstOrDefault(o => o.Title == "Cucumber");

            await productService.DeleteAsync(forDelete?.Id);
            var deletedProduct = await productService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productService.DeleteAsync(0));
            Assert.Null(deletedProduct);
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
