using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Accommodation.Startup))]
namespace Accommodation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
