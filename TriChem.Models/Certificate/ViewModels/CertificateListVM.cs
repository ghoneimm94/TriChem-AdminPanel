using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Certificate.ViewModels
{
    public class CertificateListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Display( Name = "Arabic Title")]
        public string Title_Ar { get; set; }

        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        public string FilePath { get; set; }

        [Display(Name = "Category Title")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Category Arabic Title")]
        public string CategoryTitle_Ar { get; set; }
        public int? CategoryId { get; set; }                
    }
}
