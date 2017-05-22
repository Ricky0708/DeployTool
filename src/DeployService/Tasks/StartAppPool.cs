using DeployService.Model;
using IISOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public class StartAppPool : ISeqTask
    {
        private IOperatorProvider provider = new OperatorProvider();
        public string Invoke(DeployContext model)
        {
     

            int count = 0;
            while (true)
            {
                count += 1;
                try
                {
                    provider.StartAppPool(model.AppPoolName);
                    count = 0;
                    return $"Start app pool {model.AppPoolName} succeed";
                }
                catch (Exception ex)
                {
                    if (count > 5)
                    {
                        throw ex;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }
        }
    }
}
