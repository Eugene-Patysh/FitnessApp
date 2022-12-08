using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Data.Models
{
    public class NutrientCategoryDb
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<NutrientDb> Nutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
