﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.ApiCenter.Entity;

namespace z.ApiCenter.Logic
{
    public class ApiClientExt:api_client
    {
        /// <summary>
        /// 批量删除时传值使用
        /// </summary>
        public List<int> ClientIds
        {
            get;
            set;
        }

        public List<int> FunctionIds
        {
            get;set;
        }
    }
}
