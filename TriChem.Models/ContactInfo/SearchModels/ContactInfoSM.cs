namespace TriChem.Models.ContactInfo.SearchModels
{
    public class ContactInfoSM : Paging.PagingSM
    {
        public string Code { get; set; }
        public string Info { get; set; }
        public string Description { get; set; }
    }
}
