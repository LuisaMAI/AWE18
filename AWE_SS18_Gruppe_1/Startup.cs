using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AWE_SS18_Gruppe_1.Startup))]
namespace AWE_SS18_Gruppe_1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
