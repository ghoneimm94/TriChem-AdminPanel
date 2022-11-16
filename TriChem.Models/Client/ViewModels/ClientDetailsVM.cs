using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.Client.ViewModels
{
    public class ClientDetailsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter English Title")]
        [Display(Name = "English Title")]
        public string Title { get; set; }
        public string ImageURL { get; set; }
        [Required(ErrorMessage = "Enter Arabic Title")]
        [Display(Name ="Arabic Title")]
        public string Title_Ar { get; set; }
    }
}
