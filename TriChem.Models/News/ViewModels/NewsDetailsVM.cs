using System.ComponentModel.DataAnnotations;

namespace TriChem.Models.News.ViewModels
{
    public class NewsDetailsVM
    {
        public int Id { get; set; }
        [Display(Name = "English Title")]
        [Required(ErrorMessage ="Enter English Title")]
        public string Title { get; set; }
        [Display(Name = "English Description")]
        [Required(ErrorMessage = "Enter English Description")]
        public string Description { get; set; }
        [Display(Name = "Arabic Title")]
        [Required(ErrorMessage = "Enter Arabic Title")]
        public string Title_Ar { get; set; }
        [Display(Name = "Arabic Description")]
        [Required(ErrorMessage = "Enter Arabic Description")]
        public string Description_Ar { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        [Display(Name ="Link")]
        [Required(ErrorMessage = "Enter Link")]
        public string LinkId { get; set; }        
    }
}
