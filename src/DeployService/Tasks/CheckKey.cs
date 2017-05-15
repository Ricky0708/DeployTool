using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;

namespace DeployService.Tasks
{
    public class CheckKey : ISeqTask
    {
        public string Invoke(DeployContext model)
        {
            if (model.AllowDeployKey != model.DeployKey)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            return "Authorized";
        }
    }
}
