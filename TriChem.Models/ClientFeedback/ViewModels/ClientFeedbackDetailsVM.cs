using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.ClientFeedback.ViewModels
{
    public class ClientFeedbackDetailsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Client Name")]
        [Display(Name = "Client English Name")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Enter Client Arabic Name")]
        [Display(Name = "Client Arabic Name")]
        public string ClientName_Ar { get; set; }
        [Required(ErrorMessage = "Enter Client English Position")]
        [Display(Name = "Client English Position")]
        public string ClientPosition { get; set; }
        [Required(ErrorMessage = "Enter Client Arabic Position")]
        [Display(Name = "Client Arabic Position")]
        public string ClientPosition_Ar { get; set; }
        [Required(ErrorMessage = "Enter English Message")]
        [Display(Name = "English Message")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Enter Arabic Message")]
        [Display(Name = "Arabic Message")]
        public string Message_Ar { get; set; }
        public string ImageURL { get; set; }
     
      
      
    }
}
