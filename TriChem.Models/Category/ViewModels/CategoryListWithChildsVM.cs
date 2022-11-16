using System.Collections.Generic;

namespace TriChem.Models.Category.ViewModels
{
    public class CategoryListWithChildsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public ICollection<ProductForCategoryVM> Product { get; set; }
    }

    public class ProductForCategoryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
    }
}
