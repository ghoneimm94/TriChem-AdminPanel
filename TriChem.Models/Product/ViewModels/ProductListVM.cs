using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Product.ViewModels
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }

        [Display(Name = "Typical Application")]
        public string TypicalApplication { get; set; }

        [Display(Name = "Product Features")]
        public string ProductFeatures { get; set; }
        public string Properties { get; set; }

        [Display(Name = "Data Sheet")]
        public string DataSheetPath { get; set; }

        [Display(Name = "Arabic Data Sheet")]
        public string DataSheetPath_Ar { get; set; }

        [Display(Name = "Certificate")]
        public string CertificatePath { get; set; }
        public int? CategoryId { get; set; }
        public string LinkId { get; set; }

        [Display(Name = "Arabic Title")]
        public string Title_Ar { get; set; }

        [Display(Name = "Arabic SubTitle")]
        public string SubTitle_Ar { get; set; }

        [Display(Name = "Arabic Description")]
        public string Description_Ar { get; set; }

        [Display(Name = "Arabic Typical Application")]
        public string TypicalApplication_Ar { get; set; }

        [Display(Name = "Arabic Product Features")]
        public string ProductFeatures_Ar { get; set; }

        [Display(Name = "Arabic Properties")]
        public string Properties_Ar { get; set; }

        [Display(Name = "Image")]
        public string ImageURL { get; set; }

        [Display(Name = "Rank")]
        public int Index { get; set; }
        //public CategoryListVM Category { get; set; }
        //public ICollection<ProductImageVM> ProductImage { get; set; }
    }
}
