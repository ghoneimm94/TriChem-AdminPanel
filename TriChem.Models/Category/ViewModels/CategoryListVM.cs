using System.Collections.Generic;
using TriChem.Models.Product.ViewModels;

namespace TriChem.Models.Category.ViewModels
{
    public class CategoryListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public string ImageURL { get; set; }
        public ICollection<string> ImageURLs { get; set; }
        public string Title_Ar { get; set; }
        public string Description_Ar { get; set; }
        //public ICollection<CertificateVM> Certificate { get; set; }
        public ICollection<ProductListVM> Product { get; set; }
    }
}
