namespace TriChem.Models.Category.SearchModels
{
    public class CategorySM : Paging.PagingSM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
