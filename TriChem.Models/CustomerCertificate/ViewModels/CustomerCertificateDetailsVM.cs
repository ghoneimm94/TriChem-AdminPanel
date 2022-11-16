using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.CustomerCertificate.ViewModels
{
    public class CustomerCertificateDetailsVM
    {
        public int Id { get; set; }
        [Display(Name ="English Title")]
        [Required(ErrorMessage ="Enter English Title")]
        public string Title { get; set; }
        [Display(Name = "Arabic Title")]
        [Required(ErrorMessage = "Enter Arabic Title")]
        public string Title_Ar { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        [Display(Name = "File")]
        public string FilePath { get; set; }
    }
}
