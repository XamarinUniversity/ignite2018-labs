using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(build2018_mycircle.Startup))]

namespace build2018_mycircle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}