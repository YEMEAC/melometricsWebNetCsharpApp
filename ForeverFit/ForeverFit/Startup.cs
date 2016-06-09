using ForeverFit.Controllers;
using ForeverFit.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ForeverFit.Startup))]
namespace ForeverFit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
