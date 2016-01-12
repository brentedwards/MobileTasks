using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MobileTasks.Server.Api.Startup))]

namespace MobileTasks.Server.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}