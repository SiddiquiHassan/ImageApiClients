using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApiClientWebMvc.Startup))]
namespace ApiClientWebMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
