using DeployLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace DeploySenderFramework
{
    class Program
    {

        static void Main(string[] args)
        {
            //讀取設定檔
            var LocalFileFolder = ConfigurationManager.AppSettings["LocalFileFolder"]; //本機要部署的資料夾
            var LocalSaveZipFolder = ConfigurationManager.AppSettings["LocalSaveZipFolder"]; //壓縮檔的暫存資料夾
            var ServerSaveZipFolder = ConfigurationManager.AppSettings["ServerSaveZipFolder"]; //Server 歷史的部署檔儲存資料夾
            var serverDeployFolder = ConfigurationManager.AppSettings["serverDeployFolder"]; //Server 上要部署的資料夾
            var DeployUri = ConfigurationManager.AppSettings["DeployUri"]; //部署的api位置
            var DeployKey = ConfigurationManager.AppSettings["DeployKey"]; //驗證的key
            var ZipName = ConfigurationManager.AppSettings["ZipName"]; //壓縮檔的名稱
            Console.WriteLine("Enter doploy description：");
            var DeployDescription = Console.ReadLine().Replace(" ", "_");
            Console.WriteLine("Deploying...");

            DeployProvider provider = new DeployProvider(DeployUri);

            //刪除不要部署的檔案
            provider.DeleteFileBeforeZip(LocalFileFolder,
                ConfigurationManager.AppSettings["DeleteFiles_BeforeZip"]); //壓縮前排除的檔案

            //開始壓縮及部署
            if (provider.StartZipFile(LocalFileFolder, LocalSaveZipFolder + $"\\{ZipName}"))
            {
                Console.WriteLine(provider.Deploy(
                    provider.GetZipFile(LocalSaveZipFolder + $"\\{ZipName}"),
                    ZipName,
                    ServerSaveZipFolder,
                    serverDeployFolder, DeployDescription,
                    DeployKey));
            }
            else
            {
                Console.WriteLine("Zip Error");
            }
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}
