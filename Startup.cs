using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BillboardsMVC5.Startup))]
namespace BillboardsMVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
