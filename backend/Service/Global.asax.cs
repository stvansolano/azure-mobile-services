namespace Backend
{
    using System.Web;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register();
        }
    }
}