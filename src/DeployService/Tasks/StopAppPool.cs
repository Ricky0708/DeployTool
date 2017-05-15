using DeployService.Model;
using IISOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StopAppPool : ISeqTask
    {
        private IOperatorProvider provider = new OperatorProvider();
        public string Invoke(DeployContext model)
        {
            provider.StopAppPool(model.AppPoolName);
            return $"Stop app pool {model.AppPoolName} succeed";
        }
    }
}
