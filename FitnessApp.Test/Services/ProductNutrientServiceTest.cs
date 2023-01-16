using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace FitnessApp.Tests.Services
{
    public class ProductNutrientServiceTest
    {
        private readonly ProductNutrientValidator validator = new();
        private readonly IProductNutrientService productNutrientService;

        public ProductNutrientServiceTest()
        {
            var _validator = new CustomValidator<ProductNutrientDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            productNutrientService = new ProductNutrientService(dbContext, _validator);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            ProductNutrientDto[] productNutrients =
            {
                new ProductNutrientDto() { ProductId = 1, NutrientId = 1, TreatingTypeId = 1, Quality = 1.1 },
                new ProductNutrientDto() { ProductId = 2, NutrientId = 2, TreatingTypeId = 2, Quality = 2.2 },
                new ProductNutrientDto() { ProductId = 3, NutrientId = 3, TreatingTypeId = 3, Quality = 3.3 }
            };

            foreach (var o in productNutrients)
            {
                productNutrientService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var productNutrients = await productNutrientService.GetAllAsync();

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
                await productNutrientService.CreateAsync(o);
            }

            var allAfterPagination = await productNutrientService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "productTitle", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Product.Title.Equals("CucumberXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var productNutrient1 = await productNutrientService.GetByIdAsync(1);
            var productNutrient2 = await productNutrientService.GetByIdAsync(2);
            var productNutrient3 = await productNutrientService.GetByIdAsync(-3);
            var productNutrient4 = await productNutrientService.GetByIdAsync(0);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productNutrientService.GetByIdAsync(null));
            Assert.NotNull(productNutrient1);
            Assert.True(productNutrient1?.Id == 1);
            Assert.NotNull(productNutrient2);
            Assert.True(productNutrient2?.Id == 2);
            Assert.Null(productNutrient3);
            Assert.Null(productNutrient4);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var productNutrient = new ProductNutrientDto() { ProductId = 4, NutrientId = 4, TreatingTypeId = 4, Quality = 4.4 };

            await productNutrientService.CreateAsync(productNutrient);
            var productNutrients = await productNutrientService.GetAllAsync();
            var createdProductNutrient = productNutrients.FirstOrDefault(o => o.Quality == 4.4);

            Assert.True(validator.TestValidate(productNutrient, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddProductNutrient")).IsValid);
            Assert.NotNull(createdProductNutrient);
            Assert.NotNull(createdProductNutrient?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var productNutrient = new ProductNutrientDto() { Id = 3, ProductId = 5, NutrientId = 5, TreatingTypeId = 5, Quality = 5.5 };
            await productNutrientService.UpdateAsync(productNutrient);
            var productNutrients = await productNutrientService.GetAllAsync();
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
            await productNutrientService.CreateAsync(productNutrient);
            var productNutrients = await productNutrientService.GetAllAsync();
            var forDelete = productNutrients.FirstOrDefault(o => o.Quality == 5.5);

            await productNutrientService.DeleteAsync(forDelete?.Id);
            var deletedProductNutrient = await productNutrientService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<ValidationException>(() => productNutrientService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productNutrientService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => productNutrientService.DeleteAsync(0));
            Assert.Null(deletedProductNutrient);
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
