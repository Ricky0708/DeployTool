using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;
using IISOperator;

namespace DeployService.Tasks
{
    public class StopWebSite : ISeqTask
    {
        private IOperatorProvider provider = new OperatorProvider();
        public string Invoke(DeployContext model)
        {
            provider.StopWebSite(model.WebSite);
            return $"成功停止站台 {model.WebSite}";

        }
    }
}
