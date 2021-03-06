﻿using z.AdminCenter.Entity;
using z.Logic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.Foundation;
using System.Data;
using z.Foundation.Data;

namespace z.AdminCenter.Logic
{
    /// <summary>
    /// 权限管理基类
    /// </summary>
    public class AdminPermissionManageBase : NHLogicBase
    {
        /// <summary>
        /// 获取指定父级权限包含的所有权限列表（用于AdminPermissionManage的Deleted方法）
        /// </summary>
        /// <param name="intParentId"></param>
        /// <returns></returns>
        protected IList<admin_permission> GetAdminPermissionList(int intParentId)
        {
            List<admin_permission> result = new List<admin_permission>();

            IList<admin_permission> adminPermissionList = Repository.Find<admin_permission>(e => e.ParentId == intParentId && e.Deleted == false);
            if (adminPermissionList.Count > 0)
            {
                foreach (var item in adminPermissionList)
                {
                    result.AddRange(GetAdminPermissionList(item.AdminPermissionId));
                }

                result.AddRange(adminPermissionList);
            }

            return result;
        }

        #region 获取系统权限树结构

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="adminPermissionList"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected List<AdminPermissionTree> GetAdminPermissionTree(IList<admin_permission> adminPermissionList, int parentId)
        {
            List<AdminPermissionTree> adminPermissionTreeList = new List<AdminPermissionTree>();

            var adminPermissions = from record in adminPermissionList
                                   where record.ParentId == parentId
                                   select record;
            foreach (var obj in adminPermissions)
            {
                AdminPermissionTree adminPermissionTree = new AdminPermissionTree();
                obj.CopyTo(adminPermissionTree);
                adminPermissionTreeList.Add(adminPermissionTree);
                adminPermissionTree.ChildAdminPermissions.AddRange(GetAdminPermissionTree(adminPermissionList, adminPermissionTree.AdminPermissionId));
            }
            return adminPermissionTreeList;
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="adminPermissionList"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<AdminPermissionTree> GetAdminPermissionTree(List<DataRow> adminPermissionList, int parentId)
        {
            List<AdminPermissionTree> adminPermissionTreeList = new List<AdminPermissionTree>();

            var adminPermissions = from record in adminPermissionList
                                   where record.Field<int>("ParentId") == parentId
                                   select record;
            foreach (var obj in adminPermissions)
            {
                AdminPermissionTree adminPermissionTree = new AdminPermissionTree();
                adminPermissionTree.AdminPermissionId = obj.Field<int>("AdminPermissionId");
                adminPermissionTree.SystemId = obj.Field<int>("SystemId");
                adminPermissionTree.ParentId = obj.Field<int>("ParentId");
                adminPermissionTree.PermissionCode = obj.Field<string>("PermissionCode");
                adminPermissionTree.Name = obj.Field<string>("Name");
                adminPermissionTree.Img = obj.Field<string>("Img");
                adminPermissionTree.Description = obj.Field<string>("Description");
                adminPermissionTree.IsMenu = Convert.ToBoolean(obj["IsMenu"]);
                adminPermissionTree.IsLink = Convert.ToBoolean(obj["IsLink"]);
                adminPermissionTree.Url = obj.Field<string>("Url");
                adminPermissionTree.Target = obj.Field<string>("Target");
                adminPermissionTreeList.Add(adminPermissionTree);
                adminPermissionTree.ChildAdminPermissions.AddRange(GetAdminPermissionTree(adminPermissionList, adminPermissionTree.AdminPermissionId));
            }
            return adminPermissionTreeList;
        }

        #endregion

        #region 获取系统权限支持ZTree的JSON数据

        /// <summary>
        /// 获取支持ZTree的JSON结构
        /// </summary>
        /// <param name="adminPermissionTreeList"></param>
        /// <returns></returns>
        protected string GetAdminPermissionZTreeJson(List<AdminPermissionTree> adminPermissionTreeList)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            foreach (var obj in adminPermissionTreeList)
            {
                jsonBuilder.AppendFormat("{{ id: \"{0}\", name: \"{1}\", code: \"{2}\", parentId: \"{3}\"", obj.AdminPermissionId, obj.Name, obj.PermissionCode, obj.ParentId);
                if (obj.ChildAdminPermissions.Count > 0)
                {
                    jsonBuilder.AppendFormat(", children: [{0}", GetAdminPermissionZTreeJson(obj.ChildAdminPermissions));
                    jsonBuilder.Append("] ");
                }
                jsonBuilder.Append("}, ");
            }
            return jsonBuilder.ToString().Trim().TrimEnd(",".ToCharArray());
        }

        #endregion
    }
}
