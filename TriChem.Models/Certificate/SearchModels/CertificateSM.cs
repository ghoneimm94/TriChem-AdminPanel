namespace TriChem.Models.Certificate.SearchModels
{
    public class CertificateSM : Paging.PagingSM
    {
        public string Title { get; set; }        
        public string CategoryTitle { get; set; }
        public int? CategoryId { get; set; }
    }
}
