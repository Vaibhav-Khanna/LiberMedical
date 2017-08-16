using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(libermedical.Server.Startup))]

namespace libermedical.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}