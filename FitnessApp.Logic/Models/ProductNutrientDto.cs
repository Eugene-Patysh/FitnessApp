using FitnessApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Models
{
    public class ProductNutrientDto
    {
        public int? Id { get; set; }
        public double Quality { get; set; }
        public ProductDto Product { get; set; }
        public int? ProductId { get; set; }
        public NutrientDto Nutrient { get; set; }
        public int? NutrientId { get; set; }
        public TreatingTypeDto TreatingType { get; set; }
        public int? TreatingTypeId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
