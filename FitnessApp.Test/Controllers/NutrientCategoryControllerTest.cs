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
    public class NutrientCategoryControllerTest
    {
        private readonly NutrientCategoryValidator validator = new();
        private readonly NutrientCategoryController nutrientCategoryController;

        public NutrientCategoryControllerTest()
        {
            var _validator = new CustomValidator<NutrientCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _nutrientCategoryService= new NutrientCategoryService(dbContext, _validator);
            nutrientCategoryController = new NutrientCategoryController(_nutrientCategoryService, _validator);
            HelpTestCreateFromArrayAsync();
        }

        internal async void HelpTestCreateFromArrayAsync()
        {
            NutrientCategoryDto[] nutrientCategories =
            {
                new NutrientCategoryDto() { Title = "Macronutrients" },
                new NutrientCategoryDto() { Title = "Minerals" },
                new NutrientCategoryDto() { Title = "Vitamins" }
             };

            foreach (var o in nutrientCategories)
            {
                await nutrientCategoryController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {

            var categories = await nutrientCategoryController.GetAllAsync();

            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.True(categories.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            NutrientCategoryDto[] nutrientCategories =
             {
                new NutrientCategoryDto() { Title = "xXXMacronutrients" },
                new NutrientCategoryDto() { Title = "MineXxXrals" },
                new NutrientCategoryDto() { Title = "VitaminsXXx" }
            };

            foreach (var o in nutrientCategories)
            {
                await nutrientCategoryController.CreateAsync(o);
            }

            var allAfterPagination = await nutrientCategoryController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("MineXxXrals"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var category1 = await nutrientCategoryController.GetByIdAsync(1);
            var category2 = await nutrientCategoryController.GetByIdAsync(2); ;

            await Assert.ThrowsAnyAsync<ValidationException>(() => nutrientCategoryController.GetByIdAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryController.GetByIdAsync(0));
            Assert.NotNull(category1);
            Assert.True(category1?.Id == 1);
            Assert.NotNull(category2);
            Assert.True(category2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var category = new NutrientCategoryDto() { Id = null, Title = "MineralsTT" };

            await nutrientCategoryController.CreateAsync(category);
            var categories = await nutrientCategoryController.GetAllAsync();
            var createdCategory = categories.FirstOrDefault(o => o.Title == "MineralsTT");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddNutrientCategory")).IsValid);
            Assert.NotNull(createdCategory);
            Assert.NotNull(createdCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var category = new NutrientCategoryDto() { Id = 3, Title = "XXX" };
            await nutrientCategoryController.UpdateAsync(category);
            var categories = await nutrientCategoryController.GetAllAsync();
            var updatedCategory = categories.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateNutrientCategory")).IsValid);
            Assert.NotNull(updatedCategory);
            Assert.True(updatedCategory?.Id.Equals(3));
            Assert.True(updatedCategory?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var category = new NutrientCategoryDto() { Title = "Microelements" };
            await nutrientCategoryController.CreateAsync(category);
            var categories = await nutrientCategoryController.GetAllAsync();
            var forDelete = categories.FirstOrDefault(o => o.Title == "Microelements");

            await nutrientCategoryController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryController.GetByIdAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<ValidationException>(() => nutrientCategoryController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => nutrientCategoryController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => nutrientCategoryController.DeleteAsync(0));
        }

        [Fact]
        public void Validate_SadCase()
        {
            NutrientCategoryDto[] nutrientCategories =
                {
                    new NutrientCategoryDto() { Title = "" },
                    new NutrientCategoryDto() { Title = null },
                    new NutrientCategoryDto() { Title = "1vitamins23" },
                    new NutrientCategoryDto() { Title = " " },
                    new NutrientCategoryDto() { Title = ",vitamins;" },
                    new NutrientCategoryDto() { Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" }
                };

            NutrientCategoryDto[] nutrientCategories1 =
            {
                    new NutrientCategoryDto() { Id = -3, Title = "Vitamins" },
                    new NutrientCategoryDto() { Id = 0, Title = "Microelements" }
                };

            var category1 = new NutrientCategoryDto() { Id = 1, Title = "Vitamins" };
            var category2 = new NutrientCategoryDto() { Id = null, Title = "Microelements" };


            foreach (var o in nutrientCategories)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in nutrientCategories1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddNutrientCategory")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateNutrientCategory")).IsValid);
            }

            Assert.False(validator.TestValidate(category1, v => v.IncludeRuleSets("AddNutrientCategory")).IsValid);
            Assert.False(validator.TestValidate(category2, v => v.IncludeRuleSets("UpdateNutrientCategory")).IsValid);
        }
    }
}
