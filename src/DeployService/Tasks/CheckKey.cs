﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeployService.Model;

namespace DeployService.Tasks
{
    public class CheckKey : _SeqTask
    {
        public override string Invoke(DeployContext model)
        {
            if (model.AllowDeployKey != model.DeployKey)
            {
                throw new UnauthorizedAccessException("授權失敗");
            }
            return "已授權";
        }
    }
}
