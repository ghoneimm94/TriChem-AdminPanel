using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Certificate.ViewModels
{
    public class CertificateDetailsVM
    {
        public int Id { get; set; }
        [Display(Name = "English Title")]
        [Required(ErrorMessage = "Enter English Title")]
        public string Title { get; set; }
        [Display(Name = "Arabic Title")]
        [Required(ErrorMessage ="Enter Arabic Title")]
        public string Title_Ar { get; set; }
        [Display(Name = "Certificate Image")]
        public string ImageURL { get; set; }
        [Display(Name = "Certificate")]
        public string FilePath { get; set; }

        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }
        public int? CategoryId { get; set; }
    }
}
