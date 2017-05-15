using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;
using System.IO;

namespace DeployService.Tasks
{
    public class SaveZip : ISeqTask
    {
        public string Invoke(DeployContext model)
        {
            using (FileStream fs = new FileStream(model.ZipFileName, FileMode.CreateNew))
            {
                model.ZipStream.CopyTo(fs);
                fs.Flush();
            };
            return $"Zip has been save to 『{model.ZipFileName}』";
        }
    }
}
