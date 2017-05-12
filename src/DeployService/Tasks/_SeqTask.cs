using DeployService.Model;
using IISOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployService.Tasks
{
    public interface ISeqTask
    {
        string Invoke(DeployContext model);
    }
}
