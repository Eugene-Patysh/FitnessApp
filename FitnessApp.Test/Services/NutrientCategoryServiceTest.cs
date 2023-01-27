using FitnessApp.Localization;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace FitnessApp.Tests.Services
{
    public class NutrientCategoryServiceTest
    {
        private readonly NutrientCategoryValidator validator;
        private readonly INutrientCategoryService nutrientCategoryService;
        private readonly Mock<IStringLocalizer<SharedResource>> sharedLocalizer;

        public NutrientCategoryServiceTest()
        {
            sharedLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            validator = new(sharedLocalizer.Object);
            var _validator = new CustomValidator<NutrientCategoryDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            nutrientCategoryService = new NutrientCategoryService(dbContext, _validator, sharedLocalizer.Object);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            NutrientCategoryDto[] nutrientCategories =
            {
                    new NutrientCategoryDto() { Title = "Macronutrients" },
                    new NutrientCategoryDto() { Title = "Minerals" },
                    new NutrientCategoryDto() { Title = "Vitamins" }
                };

            foreach (var o in nutrientCategories)
            {
                nutrientCategoryService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var categories = await nutrientCategoryService.GetAllAsync();

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
                await nutrientCategoryService.CreateAsync(o);
            }

            var allAfterPagination = await nutrientCategoryService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("MineXxXrals"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var category1 = await nutrientCategoryService.GetByIdAsync(1);
            var category2 = await nutrientCategoryService.GetByIdAsync(2);
            var category3 = await nutrientCategoryService.GetByIdAsync(-3);
            var category4 = await nutrientCategoryService.GetByIdAsync(0);

            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryService.GetByIdAsync(null));
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
            var category = new NutrientCategoryDto() { Id = null, Title = "MineralsTT" };

            await nutrientCategoryService.CreateAsync(category);
            var categories = await nutrientCategoryService.GetAllAsync();
            var createdCategory = categories.FirstOrDefault(o => o.Title == "MineralsTT");

            Assert.True(validator.TestValidate(category, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddNutrientCategory")).IsValid);
            Assert.NotNull(createdCategory);
            Assert.NotNull(createdCategory?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var category = new NutrientCategoryDto() { Id = 3, Title = "XXX" };
            await nutrientCategoryService.UpdateAsync(category);
            var categories = await nutrientCategoryService.GetAllAsync();
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
            await nutrientCategoryService.CreateAsync(category);
            var categories = await nutrientCategoryService.GetAllAsync();
            var forDelete = categories.FirstOrDefault(o => o.Title == "Microelements");

            await nutrientCategoryService.DeleteAsync(forDelete?.Id);
            var deletedCategory = await nutrientCategoryService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientCategoryService.DeleteAsync(0));
            Assert.Null(deletedCategory);
        }

        [Fact]
        public void Validate_SadCase()
        {
            NutrientCategoryDto[] nutrientCategories =
            {
                    new NutrientCategoryDto() { Title = "" },
                    new NutrientCategoryDto() { Title = null },
                    new NutrientCategoryDto() { Title = "1minerals23" },
                    new NutrientCategoryDto() { Title = " " },
                    new NutrientCategoryDto() { Title = ",minerals;" },
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
