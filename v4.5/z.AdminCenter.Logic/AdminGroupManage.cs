using z.AdminCenter.Entity;
using z.Logic.Base;
using z.Foundation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.Foundation;
using NHibernate;
using NHibernate.Linq;

namespace z.AdminCenter.Logic
{
    /// <summary>
    /// 用户组管理
    /// </summary>
    public class AdminGroupManage : NHLogicBase
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="param">主键ID</param>
        /// <returns></returns>
        public AdminGroupExt Get(object param)
        {
            AdminGroupExt adminGroupExt = null;

            admin_group _admin_group = Repository.Get<admin_group>(param);
            if (_admin_group != null)
            {
                adminGroupExt = new AdminGroupExt();
                _admin_group.CopyTo(adminGroupExt);

                IList<admin_group_permission> adminGroupPermissionList = Repository.Find<admin_group_permission>(exp => exp.GroupId == _admin_group.AdminGroupId);
                StringBuilder permissionIds = new StringBuilder();
                foreach (var obj in adminGroupPermissionList)
                {
                    permissionIds.AppendFormat("{0},", obj.PermissionId);
                }
                adminGroupExt.PermissionIds = permissionIds.ToString().TrimEnd(',');
            }

            return adminGroupExt;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BoolResult Save(AdminGroupExt param)
        {
            BoolResult result = new BoolResult();

            //保持对象的唯一性
            if (Repository.Exists<admin_group>(e => e.Name == param.Name && e.Deleted == false))
            {
                result.Succeeded = false;
                result.Message = "已存在相同的Name值";
                return result;
            }

            admin_group _admin_group = new admin_group();
            param.CopyTo(_admin_group);

            using (ISession session = NHibernateHelper<admin_group>.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    object obj = session.Save(_admin_group);
                    if (obj == null)
                    {
                        result.Succeeded = false;
                        result.Message = "保存对象失败";
                        return result;
                    }

                    //由于超级管理员类型用户组拥有所有权限，故仅当用户组为非超级管理员类型时才有必要保存用户组对应的权限
                    if (param.Type == 1)
                    {
                        //添加组与权限的关系
                        string[] permissionIds = param.PermissionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (var permissionId in permissionIds)
                        {
                            admin_group_permission _admin_group_permission = new admin_group_permission();
                            _admin_group_permission.GroupId = (int)obj;
                            _admin_group_permission.PermissionId = int.Parse(permissionId);
                            session.Save(_admin_group_permission);
                        }
                    }

                    transaction.Commit();

                    //主动更新用户会话
                    new AdminAccountManage().UpdateUserSession();

                    result.Succeeded = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return result;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BoolResult Update(AdminGroupExt param)
        {
            BoolResult result = new BoolResult();

            admin_group _admin_group = Repository.First<admin_group>(e => e.AdminGroupId == param.AdminGroupId && e.Deleted == false);
            if (_admin_group == null)
            {
                result.Succeeded = false;
                result.Message = "修改的对象不存在";
                return result;
            }

            if (_admin_group.Name != param.Name)
            {
                //保持对象的唯一性
                if (Repository.Exists<admin_group>(e => e.Name == param.Name && e.Deleted == false))
                {
                    result.Succeeded = false;
                    result.Message = "已存在相同的Name值";
                    return result;
                }
            }

            using (ISession session = NHibernateHelper<admin_group>.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    _admin_group.Name = param.Name;
                    _admin_group.Description = param.Description;
                    _admin_group.Type = param.Type;
                    _admin_group.UpdateBy = param.UpdateBy;
                    _admin_group.UpdateOn = param.UpdateOn;
                    session.Update(_admin_group);

                    IList<admin_group_permission> adminGroupPermissionList = Repository.Find<admin_group_permission>(e => e.GroupId == _admin_group.AdminGroupId);
                    if (_admin_group.Type == 0)
                    {
                        //当用户组角色更新为超级管理员类型时，删除该组与权限的所有对应关系
                        foreach (var obj in adminGroupPermissionList)
                        {
                            session.Delete(obj);
                        }
                    }
                    else
                    {
                        #region 当用户组角色更新为普通管理员类型时，比较该组包含的新、旧权限集合，新增需要新增的权限，删除需要删除的权限

                        #region 取出两个数组的差集(交换算法)

                        int[] oldPermissionIdArray = adminGroupPermissionList.Select(exp => exp.PermissionId).Distinct().ToArray();
                        string[] permissionIds = param.PermissionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                        int[] newPermissionIdArray = Array.ConvertAll<string, int>(permissionIds, delegate(string s) { return int.Parse(s); });

                        int oldPermissionIdArraySize = oldPermissionIdArray.Length;
                        int newPermissionIdArraySize = newPermissionIdArray.Length;
                        int end = oldPermissionIdArraySize;
                        bool swap = false;

                        for (int i = 0; i < end; )
                        {
                            swap = false;
                            for (int j = i; j < newPermissionIdArraySize; j++)
                            {
                                if (oldPermissionIdArray[i] == newPermissionIdArray[j])
                                {
                                    int tmp = newPermissionIdArray[i];
                                    newPermissionIdArray[i] = newPermissionIdArray[j];
                                    newPermissionIdArray[j] = tmp;
                                    swap = true;
                                    break;
                                }
                            }
                            if (swap != true)
                            {
                                int tmp = oldPermissionIdArray[i];
                                oldPermissionIdArray[i] = oldPermissionIdArray[--end];
                                oldPermissionIdArray[end] = tmp;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        #endregion

                        //需删除的
                        List<int> deleteGroupPermissionIdArray = new List<int>();
                        for (int i = end; i < oldPermissionIdArraySize; i++)
                        {
                            deleteGroupPermissionIdArray.Add(oldPermissionIdArray[i]);
                        }

                        //需添加的
                        List<int> addGroupPermissionIdArray = new List<int>();
                        for (int i = end; i < newPermissionIdArraySize; i++)
                        {
                            addGroupPermissionIdArray.Add(newPermissionIdArray[i]);
                        }

                        //执行删除
                        var deleteGroupPermissionObj = from obj in adminGroupPermissionList
                                        where deleteGroupPermissionIdArray.Contains(obj.PermissionId)
                                        select obj;
                        foreach (var obj in deleteGroupPermissionObj)
                        {
                            session.Delete(obj);
                        }

                        //执行添加
                        foreach (var obj in addGroupPermissionIdArray)
                        {
                            admin_group_permission _admin_group_permission = new admin_group_permission();
                            _admin_group_permission.GroupId = _admin_group.AdminGroupId;
                            _admin_group_permission.PermissionId = obj;
                            session.Save(_admin_group_permission);
                        }

                        #endregion
                    }

                    transaction.Commit();

                    //主动更新用户会话
                    new AdminAccountManage().UpdateUserSession();

                    result.Succeeded = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            
            return result;
        }

        /// <summary>
        /// 单个、批量禁用/启用对象
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BoolResult Disabled(AdminGroupExt param)
        {
            BoolResult result = new BoolResult();

            IList<admin_group> adminGroupList = Repository.Find<admin_group>(e => param.AdminGroupIds.Contains(e.AdminGroupId) && e.Disabled != param.Disabled && e.Deleted == false);
            if (adminGroupList.Count == 0)
            {
                result.Succeeded = false;
                result.Message = string.Format("{0}的对象不存在", param.Disabled ? "禁用" : "启用");
                return result;
            }
            else
            {
                using (ISession session = NHibernateHelper<admin_group>.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        int intRecord = 0;
                        foreach (var _admin_group in adminGroupList)
                        {
                            _admin_group.Disabled = param.Disabled;
                            _admin_group.UpdateBy = param.UpdateBy;
                            _admin_group.UpdateOn = param.UpdateOn;
                            session.Update(_admin_group);

                            intRecord++;
                        }

                        transaction.Commit();

                        //主动更新用户会话
                        new AdminAccountManage().UpdateUserSession();

                        result.Succeeded = true;
                        result.Result = intRecord;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 单个、批量删除对象
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BoolResult Deleted(AdminGroupExt param)
        {
            BoolResult result = new BoolResult();

            IList<admin_group> adminGroupList = Repository.Find<admin_group>(e => param.AdminGroupIds.Contains(e.AdminGroupId) && e.Deleted == false);
            if (adminGroupList.Count == 0)
            {
                result.Succeeded = false;
                result.Message = "删除的对象不存在";
                return result;
            }
            else
            {
                using (ISession session = NHibernateHelper<admin_group>.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        int intRecord = 0;
                        foreach (var _admin_group in adminGroupList)
                        {
                            //删除用户组时需要删除用户组与权限的关系、用户组与用户的关系

                            _admin_group.Deleted = true;
                            _admin_group.UpdateBy = param.UpdateBy;
                            _admin_group.UpdateOn = param.UpdateOn;
                            session.Update(_admin_group);

                            IList<admin_group_permission> adminGroupPermissionList = Repository.Find<admin_group_permission>(e => e.GroupId == _admin_group.AdminGroupId);
                            foreach (var _admin_group_permission in adminGroupPermissionList)
                            {
                                session.Delete(_admin_group_permission);
                            }

                            IList<admin_user_group> adminUserGroupList = Repository.Find<admin_user_group>(e => e.GroupId == _admin_group.AdminGroupId);
                            foreach (var _admin_user_group in adminUserGroupList)
                            {
                                session.Delete(_admin_user_group);
                            }

                            intRecord++;
                        }

                        transaction.Commit();

                        //主动更新用户会话
                        new AdminAccountManage().UpdateUserSession();

                        result.Succeeded = true;
                        result.Result = intRecord;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 分页获取列表（提供按Name进行模糊搜索，搜索时给Name属性赋值，不提供客户端自定义排序，默认按创建时间倒序排列）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IPagedList<admin_group> GetPageList(IPagedParam<admin_group> param)
        {
            IQueryable<admin_group> queryable = Repository.AsQueryable<admin_group>().Where(e => e.Deleted == false);
            if (!string.IsNullOrEmpty(param.model.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(param.model.Name));
            }
            queryable = queryable.OrderByDescending(e => e.CreateOn);

            return new PagedList<admin_group>(queryable, param.PageIndex, param.PageSize);
        }

        /// <summary>
        /// 获取全部列表（用于添加/更新用户页面，选择用户所属的组）
        /// </summary>
        /// <returns></returns>
        public IList<admin_group> GetList()
        {
            IQueryable<admin_group> queryable = Repository.AsQueryable<admin_group>().Where(e => e.Disabled == false && e.Deleted == false);
            queryable = queryable.OrderBy(e => e.CreateOn);
            return queryable.ToList();
        }
    }
}
