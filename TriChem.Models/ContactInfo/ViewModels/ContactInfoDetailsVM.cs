using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.ContactInfo.ViewModels
{
    public class ContactInfoDetailsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Enter Information")]
        public string Info { get; set; }
        [Display(Name = "English Description")]
        [Required(ErrorMessage = "Enter English Description")]
        public string Description { get; set; }
        [Display(Name = "Arabic Description")]
        [Required(ErrorMessage = "Enter Arabic Description")]
        public string Description_Ar { get; set; }
    }
}
