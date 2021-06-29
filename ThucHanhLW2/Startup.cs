using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThucHanhLW2.Startup))]
namespace ThucHanhLW2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
