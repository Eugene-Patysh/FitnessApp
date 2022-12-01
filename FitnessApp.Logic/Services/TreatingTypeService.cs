using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public class TreatingTypeService : BaseService, ITreatingTypeService
    {
        public TreatingTypeService(ProductContext context) : base(context)
        {

        }
        public async Task<TreatingTypeDto[]> GetAllAsync()
        {
            var treatingTypesDbs = await _context.TreatingTypes.ToArrayAsync();

            return TreatingTypeBuilder.Build(treatingTypesDbs);
        }
        public Task<TreatingTypeDto> GetByIdAsync(int treatingTypeDtoId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(TreatingTypeDto treatingTypeDto)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(TreatingTypeDto treatingTypeDto)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int treatingTypeDtoId)
        {
            throw new NotImplementedException();
        }    
    }
}
