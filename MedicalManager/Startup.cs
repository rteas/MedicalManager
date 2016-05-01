using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalManager.Startup))]
namespace MedicalManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
