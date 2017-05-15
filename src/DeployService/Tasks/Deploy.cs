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
    public class Deploy : ISeqTask
    {
        public string Invoke(DeployContext model)
        {
            using (ZipArchive archive = new ZipArchive(model.ZipStream))
            {
                foreach (var file in archive.Entries)
                {
                    int count = 0;
                    while (true)
                    {
                        count += 1;
                        try
                        {
                            string targetPath = Path.Combine(model.DeployFolder, file.FullName);
                            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                            file.ExtractToFile(targetPath, true);
                            count = 0;
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (count > 5)
                            {
                                throw ex;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            return $"Deploy succeed";
        }
    }
}
