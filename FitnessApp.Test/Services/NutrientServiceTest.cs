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
    public class NutrientServiceTest
    {
        private readonly NutrientValidator validator;
        private readonly INutrientService nutrientService;
        private readonly Mock<IStringLocalizer<SharedResource>> sharedLocalizer;

        public NutrientServiceTest()
        {
            sharedLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            validator = new(sharedLocalizer.Object);
            var _validator = new CustomValidator<NutrientDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            nutrientService = new NutrientService(dbContext, _validator, sharedLocalizer.Object);
            HelpTestCreateFromArray();
        }

        internal void HelpTestCreateFromArray()
        {
            NutrientDto[] nutrients =
            {
                new NutrientDto() { NutrientCategoryId = 1, Title = "Protein", DailyDose = 0.1 },
                new NutrientDto() { NutrientCategoryId = 2, Title = "Сalcium", DailyDose = 0.2 },
                new NutrientDto() { NutrientCategoryId = 3, Title = "Folacin", DailyDose = 0.3 }
            };

            foreach (var o in nutrients)
            {
                nutrientService.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            NutrientDto[] nutrients =
             {
                new NutrientDto() { NutrientCategoryId = 1, Title = "xXXProtein", DailyDose = 0.1 },
                new NutrientDto() { NutrientCategoryId = 2, Title = "СalcXxXium", DailyDose = 0.1 },
                new NutrientDto() { NutrientCategoryId = 3, Title = "FolacinXXx", DailyDose = 0.1 }
            };

            foreach (var o in nutrients)
            {
                await nutrientService.CreateAsync(o);
            }

            var allAfterPagination = await nutrientService.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total <= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("СalcXxXium"));
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var nutrient = new NutrientDto() { Id = null, NutrientCategoryId = 1, Title = "FolacinTT", DailyDose = 0.1 };

            await nutrientService.CreateAsync(nutrient);
            var nutrients = await nutrientService.GetAllAsync();
            var createdNutrient = nutrients.FirstOrDefault(o => o.Title == "FolacinTT");

            Assert.True(validator.TestValidate(nutrient, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddNutrient")).IsValid);
            Assert.NotNull(createdNutrient);
            Assert.NotNull(createdNutrient?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var nutrient = new NutrientDto() { Id = 3, NutrientCategoryId = 1, Title = "XXX", DailyDose = 0.1 };
            await nutrientService.UpdateAsync(nutrient);
            var nutrients = await nutrientService.GetAllAsync();
            var updatedNutrient = nutrients.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(nutrient, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateNutrient")).IsValid);
            Assert.NotNull(updatedNutrient);
            Assert.True(updatedNutrient?.Id.Equals(3));
            Assert.True(updatedNutrient?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var nutrient = new NutrientDto() { NutrientCategoryId = 1, Title = "Zinetum", DailyDose = 0.1 }; // цинк
            await nutrientService.CreateAsync(nutrient);
            var nutrients = await nutrientService.GetAllAsync();
            var forDelete = nutrients.FirstOrDefault(o => o.Title == "Zinetum");

            await nutrientService.DeleteAsync(forDelete?.Id);
            var deletedNutrient = await nutrientService.GetByIdAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => nutrientService.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientService.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => nutrientService.DeleteAsync(0));
            Assert.Null(deletedNutrient);
        }

        [Fact]
        public void Validate_SadCase()
        {
            NutrientDto[] nutrients =
            {
                new NutrientDto() { NutrientCategoryId = 1, Title = "", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 2, Title = null, DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 3, Title = "1folacin23", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 1, Title = " ", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 2, Title = ",folacin;", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 3, Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 0, Title = "Zinetum", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = null, Title = "Zinetum", DailyDose = 0.1 },
                new NutrientDto() { NutrientCategoryId = -1, Title = "Zinetum", DailyDose = 0.1  },
                new NutrientDto() { NutrientCategoryId = 1, Title = "Folacin", DailyDose = 0  },
                new NutrientDto() { NutrientCategoryId = 1, Title = "Folacin", DailyDose = -1  }
            };

            NutrientDto[] nutrients1 =
            {
                new NutrientDto() { NutrientCategoryId = 1, Id = -3, Title = "Zinetum", DailyDose = 0.1   },
                new NutrientDto() { NutrientCategoryId = 2,  Id = 0, Title = "Folacin", DailyDose = 0.1   }
            };

            var nutrient1 = new NutrientDto() { NutrientCategoryId = 1, Id = 1, Title = "Zinetum", DailyDose = 0.1 };
            var nutrient2 = new NutrientDto() { NutrientCategoryId = 2, Id = null, Title = "Folacin", DailyDose = 0.1 };


            foreach (var o in nutrients)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in nutrients1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddNutrient")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateNutrient")).IsValid);
            }

            Assert.False(validator.TestValidate(nutrient1, v => v.IncludeRuleSets("AddNutrient")).IsValid);
            Assert.False(validator.TestValidate(nutrient2, v => v.IncludeRuleSets("UpdateNutrient")).IsValid);
        }
    }
}
