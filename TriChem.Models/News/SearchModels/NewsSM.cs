namespace TriChem.Models.News.SearchModels
{
    public class NewsSM : Paging.PagingSM
    {
        public string Title { get; set; }
        public string Description { get; set; }        
        public string LinkId { get; set; }        
    }
}
