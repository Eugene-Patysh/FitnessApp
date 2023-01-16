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
    public class TreatingTypeControllerTest
    {
        private readonly TreatingTypeValidator validator = new();
        private readonly TreatingTypeController treatingTypeController;

        public TreatingTypeControllerTest()
        {
            var _validator = new CustomValidator<TreatingTypeDto>(validator);
            var dbContext = DatabaseInMemory.CreateDbContext();
            var _treatingTypeService = new TreatingTypeService(dbContext, _validator);
            treatingTypeController = new TreatingTypeController(_treatingTypeService, _validator);
            HelpTestCreateFromArrayAsync();
        }

        internal async void HelpTestCreateFromArrayAsync()
        {
            TreatingTypeDto[] treatingTypes =
            {
                new TreatingTypeDto() { Title = "Fresh" },
                new TreatingTypeDto() { Title = "Fried" },
                new TreatingTypeDto() { Title = "Dried" }
            };

            foreach (var o in treatingTypes)
            {
                await treatingTypeController.CreateAsync(o);
            }
        }

        [Fact]
        public async Task GetAllAsync_HappyCase()
        {
            var treatingTypes = await treatingTypeController.GetAllAsync();

            Assert.NotNull(treatingTypes);
            Assert.NotEmpty(treatingTypes);
            Assert.True(treatingTypes.Count >= 3);
        }

        [Fact]
        public async Task GetPaginationAsync_HappyCase()
        {
            TreatingTypeDto[] treatingTypes =
             {
                new TreatingTypeDto() { Title = "xXXFresh" },
                new TreatingTypeDto() { Title = "FriXxXed" },
                new TreatingTypeDto() { Title = "DriedXXx" }
            };

            foreach (var o in treatingTypes)
            {
                await treatingTypeController.CreateAsync(o);
            }

            var allAfterPagination = await treatingTypeController.GetPaginationAsync(new PaginationRequest() { Query = "xxx", SortBy = "title", Ascending = true, Skip = 0, Take = 3 });
            var firstAfterPagination = allAfterPagination.Values.FirstOrDefault();

            Assert.NotNull(allAfterPagination);
            Assert.True(allAfterPagination.Total >= 3);
            Assert.NotNull(firstAfterPagination);
            Assert.True(firstAfterPagination?.Title.Equals("DriedXXx"));
        }

        [Fact]
        public async Task GetByIdAsync_HappyCase()
        {
            var treatingType1 = await treatingTypeController.GetByIdAsync(1);
            var treatingType2 = await treatingTypeController.GetByIdAsync(2); ;

            await Assert.ThrowsAnyAsync<ValidationException>(() => treatingTypeController.GetByIdAsync(null));
            await Assert.ThrowsAnyAsync<Exception>(() => treatingTypeController.GetByIdAsync(-3));
            await Assert.ThrowsAnyAsync<Exception>(() => treatingTypeController.GetByIdAsync(0));
            Assert.NotNull(treatingType1);
            Assert.True(treatingType1?.Id == 1);
            Assert.NotNull(treatingType2);
            Assert.True(treatingType2?.Id == 2);
        }

        [Fact]
        public async Task CreateAsync_HappyCase()
        {
            var treatingType = new TreatingTypeDto() { Id = null, Title = "FreshTT" };

            await treatingTypeController.CreateAsync(treatingType);
            var treatingTypes = await treatingTypeController.GetAllAsync();
            var createdTreatingType = treatingTypes.FirstOrDefault(o => o.Title == "FreshTT");

            Assert.True(validator.TestValidate(treatingType, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("AddTreatingType")).IsValid);
            Assert.NotNull(createdTreatingType);
            Assert.NotNull(createdTreatingType?.Id);
        }

        [Fact]
        public async Task UpdateAsync_HappyCase()
        {
            var treatingType = new TreatingTypeDto() { Id = 3, Title = "XXX" };
            await treatingTypeController.UpdateAsync(treatingType);
            var treatingTypes = await treatingTypeController.GetAllAsync();
            var updatedTreatingType = treatingTypes.FirstOrDefault(o => o.Title == "XXX");

            Assert.True(validator.TestValidate(treatingType, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets("UpdateTreatingType")).IsValid);
            Assert.NotNull(updatedTreatingType);
            Assert.True(updatedTreatingType?.Id.Equals(3));
            Assert.True(updatedTreatingType?.Title.Equals("XXX"));
        }

        [Fact]
        public async Task DeleteAsync_HappyCase()
        {
            var treatingType = new TreatingTypeDto() { Title = "Berries" };
            await treatingTypeController.CreateAsync(treatingType);
            var treatingTypes = await treatingTypeController.GetAllAsync();
            var forDelete = treatingTypes.FirstOrDefault(o => o.Title == "Berries");

            await treatingTypeController.DeleteAsync(forDelete?.Id);

            await Assert.ThrowsAnyAsync<Exception>(() => treatingTypeController.GetByIdAsync(forDelete?.Id));
            await Assert.ThrowsAnyAsync<ValidationException>(() => treatingTypeController.DeleteAsync(null));
            await Assert.ThrowsAnyAsync<ValidationException>(() => treatingTypeController.DeleteAsync(-3));
            await Assert.ThrowsAnyAsync<ValidationException>(() => treatingTypeController.DeleteAsync(0));
        }

        [Fact]
        public void Validate_SadCase()
        {
            TreatingTypeDto[] treatingTypes =
                {
                    new TreatingTypeDto() { Title = "" },
                    new TreatingTypeDto() { Title = null },
                    new TreatingTypeDto() { Title = "1fresh23" },
                    new TreatingTypeDto() { Title = " " },
                    new TreatingTypeDto() { Title = ",fresh;" },
                    new TreatingTypeDto() { Title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" }
                };

            TreatingTypeDto[] treatingTypes1 =
            {
                    new TreatingTypeDto() { Id = -3, Title = "Fresh" },
                    new TreatingTypeDto() { Id = 0, Title = "Dried" }
                };

            var treatingType1 = new TreatingTypeDto() { Id = 1, Title = "Fresh" };
            var treatingType2 = new TreatingTypeDto() { Id = null, Title = "Dried" };


            foreach (var o in treatingTypes)
            {
                Assert.False(validator.TestValidate(o).IsValid);
            }

            foreach (var o in treatingTypes1)
            {
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("AddTreatingType")).IsValid);
                Assert.False(validator.TestValidate(o, v => v.IncludeRuleSets("UpdateTreatingType")).IsValid);
            }

            Assert.False(validator.TestValidate(treatingType1, v => v.IncludeRuleSets("AddTreatingType")).IsValid);
            Assert.False(validator.TestValidate(treatingType2, v => v.IncludeRuleSets("UpdateTreatingType")).IsValid);
        }
    }
}
