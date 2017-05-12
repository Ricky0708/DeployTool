using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;

namespace DeployService.Tasks
{
    public class StopWebSite : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            provider.StopWebSite(model.WebSite);
            return $"成功停止站台 {model.WebSite}";

        }
    }
}
