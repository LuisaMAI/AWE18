using System.Web;
using System.Web.Mvc;

namespace AWE_SS18_Gruppe_1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
