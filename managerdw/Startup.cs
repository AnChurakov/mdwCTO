using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(managerdw.Startup))]
namespace managerdw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
