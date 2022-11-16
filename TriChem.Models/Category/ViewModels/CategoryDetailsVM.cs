using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Category.ViewModels
{
    public class CategoryDetailsVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Enter English Title")]
        [Display(Name ="English Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter Arabic Title")]
        [Display(Name = "Arabic Title")]
        public string Title_Ar { get; set; }

        public string Description { get; set; }

        [Display(Name = "Arabic Description")]
        public string Description_Ar { get; set; }

        [Display(Name = "Images")]
        public List<string> ImageURLs { get; set; }

        //[Display(Name = "Image")]
        //public string ImageURL { get; set; }

        //public virtual ICollection<Certificate> Certificate { get; set; }
        //public virtual ICollection<Product> Product { get; set; }
    }
}
