using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eCommerceProject.Startup))]
namespace eCommerceProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
