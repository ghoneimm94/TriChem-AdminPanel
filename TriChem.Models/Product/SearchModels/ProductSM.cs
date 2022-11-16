namespace TriChem.Models.Product.SearchModels
{
    public class ProductSM : Paging.PagingSM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string TypicalApplication { get; set; }
        public string ProductFeatures { get; set; }
        public string Properties { get; set; }
        public string DataSheetPath { get; set; }
        public string DataSheetPath_Ar { get; set; }
        public string CertificatePath { get; set; }
        public string CategoryTitle { get; set; }
        public int CategoryId { get; set; }
        public string LinkId { get; set; }
    }
}
