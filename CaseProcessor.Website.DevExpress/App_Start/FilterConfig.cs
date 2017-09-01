using System.Web;
using System.Web.Mvc;

namespace CaseProcessor_Website_DevExpress {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}