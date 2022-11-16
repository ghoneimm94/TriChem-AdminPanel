namespace TriChem.Models.ClientFeedback.SearchModels
{
    public class ClientFeedbackSM : Paging.PagingSM
    {
        public string ClientName { get; set; }
        public string ClientPosition { get; set; }
        public string Message { get; set; }
    }
}
