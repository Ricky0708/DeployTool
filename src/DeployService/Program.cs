using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeployService
{
    class Program
    {
        static void Main(string[] args)
        {
     

            string baseAddress = ConfigurationManager.AppSettings["ListenUrl"];
            WebApp.Start<Startup>(url: baseAddress);
            Console.ReadLine();
        }
    }
}
