using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CaseProcessor_Website_DevExpress.Startup))]

// Files related to ASP.NET Identity duplicate the Microsoft ASP.NET Identity file structure and contain initial Microsoft comments.

namespace CaseProcessor_Website_DevExpress
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}