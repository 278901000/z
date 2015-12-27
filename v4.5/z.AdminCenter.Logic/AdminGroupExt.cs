using z.AdminCenter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.AdminCenter.Logic
{
    /// <summary>
    /// AdminGroup对象扩展类
    /// </summary>
    [Serializable]
    public class AdminGroupExt : admin_group
    {
        /// <summary>
        /// 权限ID集合
        /// </summary>
        public string PermissionIds { get; set; }

        /// <summary>
        /// 批量删除时传值使用
        /// </summary>
        public List<int> AdminGroupIds
        {
            get;
            set;
        }
    }
}
