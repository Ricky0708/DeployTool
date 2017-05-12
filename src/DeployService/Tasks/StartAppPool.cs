using DeployService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StartAppPool : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            provider.StartAppPool(model.AppPoolName);
            return $"成功啟動 {model.AppPoolName} pool";
        }
    }
}
