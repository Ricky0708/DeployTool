using DeployService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StartWebSite : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            provider.StartWebSite(model.WebSite);
            return $"成功啟動站台 {model.WebSite}";
        }
    }
}
