using DeployService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StopAppPool : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            provider.StopAppPool(model.AppPoolName);
            return $"成功停止 {model.AppPoolName} pool";
        }
    }
}
