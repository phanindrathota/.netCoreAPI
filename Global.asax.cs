using System.Web.Http;
using DotNet46ApiExample.App_Start;

namespace DotNet46ApiExample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
