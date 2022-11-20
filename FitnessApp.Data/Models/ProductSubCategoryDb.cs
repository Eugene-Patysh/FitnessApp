using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessApp.Data.Models
{
    public class ProductSubCategoryDb
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategoryDb ProductCategory { get; set; }
        public ICollection<ProductDb> Products { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
