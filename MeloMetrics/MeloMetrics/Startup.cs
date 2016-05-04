using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MeloMetrics.Startup))]
namespace MeloMetrics
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
