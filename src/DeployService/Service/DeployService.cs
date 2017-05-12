using System;
using System.IO;
using System.IO.Compression;
using IISOperator;
namespace DeployService.Service
{
    public interface IDeployService
    {
        /// <summary>
        /// 停止站台
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        bool StopWebSite(string webSite);
        /// <summary>
        /// 停止pool
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        bool StopAppPool(string appPoolName);
        /// <summary>
        /// 部署
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="deployFolder"></param>
        /// <returns></returns>
        bool Deploy(Stream zipStream, string deployFolder);
        /// <summary>
        /// 啟動站台
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        bool StartWebSite(string webSite);
        /// <summary>
        /// 啟動pool
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        bool StartAppPool(string appPoolName);
        /// <summary>
        /// 儲存zip
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="savePath"></param>
        /// <param name="descrption"></param>
        /// <returns></returns>
        bool SaveZipFile(Stream zipStream, string savePath);
    }
    public class DeployService : IDeployService
    {
        IOperatorProvider provider = new OperatorProvider();
        /// <summary>
        /// <see cref="IDeployService.StopWebSite(string)(string)"/>
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        public bool StopWebSite(string webSite)
        {
            provider.StopWebSite(webSite);
            return true;
        }
        /// <summary>
        /// <see cref="IDeployService.StopAppPool(string)"/>
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        public bool StopAppPool(string appPoolName)
        {
            provider.StopAppPool(appPoolName);
            return true;
        }
        /// <summary>
        /// <see cref="IDeployService.Deploy(Stream, string)"/>
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="deployFolder"></param>
        /// <returns></returns>
        public bool Deploy(Stream zipStream, string deployFolder)
        {
            using (ZipArchive archive = new ZipArchive(zipStream))
            {
                foreach (var file in archive.Entries)
                {
                    file.ExtractToFile(Path.Combine(deployFolder, file.FullName), true);
                }
            }
            return true;
        }
        /// <summary>
        /// <see cref="IDeployService.StartAppPool(string)"/>
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        public bool StartAppPool(string appPoolName)
        {
            provider.StartAppPool(appPoolName);
            return true;
        }
        /// <summary>
        /// <see cref="IDeployService.StartWebSite(string)"/>
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        public bool StartWebSite(string webSite)
        {
            provider.StartWebSite(webSite);
            return true;
        }
        /// <summary>
        /// <see cref="IDeployService.SaveZipFile(Stream, string, string)"/>
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="savePath"></param>
        /// <param name="descrption"></param>
        /// <returns></returns>
        public bool SaveZipFile(Stream zipStream, string savePath)
        {

            using (FileStream fs = new FileStream(savePath, FileMode.CreateNew))
            {
                zipStream.CopyTo(fs);
                fs.Flush();
            };
            return true;
        }
    }
}
