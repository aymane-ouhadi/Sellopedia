using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sellopedia.Startup))]
namespace Sellopedia
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
