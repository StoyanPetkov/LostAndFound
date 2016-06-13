using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LF.Startup))]
namespace LF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
