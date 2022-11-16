using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Filters;

namespace TriChem.AdminPanel
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeUserAttribute());
        }
    }
}
