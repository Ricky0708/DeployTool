using DeployService.Model;
using IISOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StartWebSite : ISeqTask
    {
        private IOperatorProvider provider = new OperatorProvider();
        public string Invoke(DeployContext model)
        {
            provider.StartWebSite(model.WebSite);
            return $"成功啟動站台 {model.WebSite}";
        }
    }
}
