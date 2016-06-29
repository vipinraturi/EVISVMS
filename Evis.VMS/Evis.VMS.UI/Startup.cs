using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Evis.VMS.UI.Startup))]
namespace Evis.VMS.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
