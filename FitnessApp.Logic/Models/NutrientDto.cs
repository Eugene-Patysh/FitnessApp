using FitnessApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Models
{
    public class NutrientDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double DailyDose { get; set; }
        public NutrientCategoryDto NutrientCategory { get; set; }
        public int NutrientCategoryId { get; set; }
        public virtual ICollection<ProductNutrientDto> ProductNutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
