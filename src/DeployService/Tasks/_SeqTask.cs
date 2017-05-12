using DeployService.Model;
using IISOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public abstract class _SeqTask
    {
        protected IOperatorProvider provider = new OperatorProvider();

        public abstract string Invoke(DeployContext model);
    }
}
