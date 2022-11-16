using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Product.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter product title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Enter product sub title")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Enter product description")]
        public string Description { get; set; }

        [Display(Name = "Typical Application")]
        [Required(ErrorMessage = "Enter product typical application")]
        public string TypicalApplication { get; set; }

        [Display(Name = "Product Features")]
        [Required(ErrorMessage = "Enter product features")]
        public string ProductFeatures { get; set; }
        [Required(ErrorMessage = "Enter product properties")]
        public string Properties { get; set; }

        [Display(Name = "Arabic Title")]
        [Required(ErrorMessage = "Enter product arabic title")]
        public string Title_Ar { get; set; }

        [Display(Name = "Arabic SubTitle")]
        [Required(ErrorMessage = "Enter product arabic sub title")]
        public string SubTitle_Ar { get; set; }

        [Display(Name = "Arabic Description")]
        [Required(ErrorMessage = "Enter product arabic description")]
        public string Description_Ar { get; set; }

        [Display(Name = "Arabic Typical Application")]
        [Required(ErrorMessage = "Enter product arabic typical application")]
        public string TypicalApplication_Ar { get; set; }

        [Display(Name = "Arabic Product Features")]
        [Required(ErrorMessage = "Enter product arabic features")]
        public string ProductFeatures_Ar { get; set; }

        [Display(Name = "Arabic Properties")]
        [Required(ErrorMessage = "Enter product arabic properties")]
        public string Properties_Ar { get; set; }

        [Display(Name = "Data Sheet")]
        public string DataSheetPath { get; set; }


        [Display(Name = "Arabic Data Sheet")]
        public string DataSheetPath_Ar { get; set; }

        [Display(Name = "Certificate")]
        public string CertificatePath { get; set; }

        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Rank")]
        [Required(ErrorMessage = "enter product rank")]
        public int Index { get; set; }

        [Required(ErrorMessage = "select product category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Related Products")]
        [Required(ErrorMessage = "select related products")]
        public ICollection<string> LinkId { get; set; }

        [Display(Name = "Images")]
        public ICollection<string> ImageURLs { get; set; }
    }
}
