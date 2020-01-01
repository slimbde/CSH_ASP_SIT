using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIT.Startup))]
namespace SIT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
