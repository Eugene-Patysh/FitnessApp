using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Data.Models
{
    public class TreatingTypeDb
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ProductNutrientDb> ProductNutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
