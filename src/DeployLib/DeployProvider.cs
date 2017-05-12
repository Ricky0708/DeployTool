using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;

namespace DeployLib
{
    public class DeployProvider
    {
        private readonly string _uri;

        /// <summary>
        /// 部署的提供者
        /// </summary>
        /// <param name="uri">遠端的web api</param>
        public DeployProvider(string uri)
        {
            _uri = uri;
        }
        /// <summary>
        /// 部署到遠端
        /// </summary>
        /// <param name="zipStream">zip檔案串流</param>
        /// <param name="fileName">post給遠端的檔案名稱(限zip)</param>
        /// <param name="serverSaveZipFolder">遠端備份zip檔案資料夾</param>
        /// <param name="serverDeployFolder">遠端發佈的檔案路徑</param>
        /// <param name="deployDescription">部署的描述</param>
        /// <param name="deployKey">部署的key</param>
        /// <returns></returns>
        public string Deploy(Stream zipStream, string fileName, string serverSaveZipFolder, string serverDeployFolder, string deployDescription, string deployKey)
        {
            //自動組合檔案名稱
            if (fileName.LastIndexOf(".zip") == -1)
            {
                fileName += ".zip";
            }
            //部署到遠端
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(zipStream);
                    content.Add(streamContent, "ZipFile", fileName);
                    content.Add(new StringContent(serverSaveZipFolder), "SaveZipFolder");
                    content.Add(new StringContent(serverDeployFolder), "DeployFolder");
                    content.Add(new StringContent(deployDescription), "DeployDescription");
                    content.Add(new StringContent(deployKey), "DeployKey");
                    var result = client.PostAsync(_uri, content);
                    var n = result.Result;
                    return n.Content.ReadAsStringAsync().Result;
                }
            }
        }
        /// <summary>
        /// 取得部署的zip檔
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Stream GetZipFile(string path)
        {
            Stream result = null;
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                result = sr.BaseStream;
            }
            return result;
        }

        /// <summary>
        /// 刪除不要部署的檔案
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="allFileNames"></param>
        public void DeleteFileBeforeZip(string sourcePath, string allFileNames)
        {
            var fileNames = allFileNames.Split(',');
            foreach (var fileName in fileNames)
            {
                var filePath = (sourcePath + "\\" + fileName).Replace(@"\\", "\\");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
         
        }

        /// <summary>
        /// 將要部署的檔案壓縮為zip
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="zipPath"></param>
        /// <returns></returns>
        public bool StartZipFile(string sourcePath, string zipPath)
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(sourcePath, zipPath);
            return true;
        }
    }
}
