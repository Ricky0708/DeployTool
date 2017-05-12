using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;
using System.IO.Compression;
using System.IO;

namespace DeployService.Tasks
{
    public class Deploy : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            using (ZipArchive archive = new ZipArchive(model.ZipStream))
            {
                foreach (var file in archive.Entries)
                {
                    string targetPath = Path.Combine(model.DeployFolder, file.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    file.ExtractToFile(targetPath, true);
                }
            }
            return $"部署成功";
        }
    }
}
