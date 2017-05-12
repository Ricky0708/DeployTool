using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IISOperator;

[assembly: OwinStartup(typeof(DeployService.Startup))]

namespace DeployService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            // 啟用屬性路由
            config.MapHttpAttributeRoutes();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);
            Console.WriteLine("System started");
        }
    }
}
