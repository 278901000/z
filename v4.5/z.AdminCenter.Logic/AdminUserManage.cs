using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z.AdminCenter.Entity;
using z.Logic.Base;
using z.Foundation;
using z.Foundation.Data;
using NHibernate;

namespace z.AdminCenter.Logic
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class AdminUserManage : NHLogicBase
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="param">主键ID</param>
        /// <returns></returns>
        public AdminUserExt Get(object param)
        {
            AdminUserExt adminUserExt = null;

            admin_user _admin_user = Repository.Get<admin_user>(param);
            if (_admin_user != null)
            {
                adminUserExt = new AdminUserExt();
                _admin_user.CopyTo(adminUserExt);

                IList<admin_user_permission> adminUserPermissionList = Repository.Find<admin_user_permission>(exp => exp.UserId == _admin_user.AdminUserId);
                StringBuilder permissionIds = new StringBuilder();
                foreach (var obj in adminUserPermissionList)
                {
                    permissionIds.AppendFormat("{0},", obj.PermissionId);
                }
                adminUserExt.PermissionIds = permissionIds.ToString().TrimEnd(',');

                IList<admin_user_group> _adminUserGroupList = Repository.Find<admin_user_group>(exp => exp.UserId == _admin_user.AdminUserId);
                StringBuilder groupIds = new StringBuilder();
                foreach (var obj in _adminUserGroupList)
                {
                    groupIds.AppendFormat("{0},", obj.GroupId);
                }
                adminUserExt.GroupIds = groupIds.ToString().TrimEnd(',');
            }

            return adminUserExt;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BoolResult Save(AdminUserExt param)
        {
            BoolResult result = new BoolResult();

            //保持对象的唯一性
            if (Repository.Exists<admin_user>(e => e.AdminName == param.AdminName && e.Deleted == false))
            {
                result.Succeeded = false;
                result.Message = "已存在相同的AdminName值";
                return result;
            }

            admin_user _admin_user = new admin_user();
            param.CopyTo(_admin_user);

            using (ISession session = NHibernateHelper<admin_user>.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    object obj = session.Save(_admin_user);
                    if (obj == null)
                    {
                        result.Succeeded = false;
                        result.Message = "保存对象失败";
                        return result;
                    }

                    //添加用户与权限的关系
                    string[] permissionIds = param.PermissionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var permissionId in permissionIds)
                    {
                        admin_user_permission _admin_user_permission = new admin_user_permission();
                        _admin_user_permission.UserId = (int)obj;
                        _admin_user_permission.PermissionId = int.Parse(permissionId);
                        session.Save(_admin_user_permission);
                    }

                    //添加用户与组的关系
                    string[] groupIds = param.GroupIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var groupId in groupIds)
                    {
                        admin_user_group _admin_user_group = new admin_user_group();
                        _admin_user_group.UserId = (int)obj;
                        _admin_user_group.GroupId = int.Parse(groupId);
                        session.Save(_admin_user_group);
                    }

                    transaction.Commit();
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
        public BoolResult Update(AdminUserExt param)
        {
            BoolResult result = new BoolResult();

            admin_user _admin_user = Repository.First<admin_user>(e => e.AdminUserId == param.AdminUserId && e.Deleted == false);
            if (_admin_user == null)
            {
                result.Succeeded = false;
                result.Message = "修改的对象不存在";
                return result;
            }

            //当更新时输入了密码，则认为需要修改该管理员的登录密码，否则密码维持不变
            if (!string.IsNullOrEmpty(param.Password))
            {
                _admin_user.Password = param.Password;
            }

            using (ISession session = NHibernateHelper<admin_user>.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    _admin_user.RealName = param.RealName;
                    _admin_user.Logo = param.Logo;
                    _admin_user.UpdateBy = param.UpdateBy;
                    _admin_user.UpdateOn = param.UpdateOn;
                    session.Update(_admin_user);

                    #region 比较该用户包含的新、旧组集合，新增需要新增的组，删除需要删除的组

                    IList<admin_user_group> adminUserGroupList = Repository.Find<admin_user_group>(exp => exp.UserId == _admin_user.AdminUserId);

                    #region 取出两个数组的差集(交换算法)

                    int[] oldGroupIdArray = adminUserGroupList.Select(exp => exp.GroupId).Distinct().ToArray();
                    string[] groupIds = string.IsNullOrEmpty(param.GroupIds) ? new string[] { } : param.GroupIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    int[] newGroupIdArray = Array.ConvertAll<string, int>(groupIds, delegate(string s) { return int.Parse(s); });

                    int oldGroupIdArraySize = oldGroupIdArray.Length;
                    int newGroupIdArraySize = newGroupIdArray.Length;
                    int end = oldGroupIdArraySize;
                    bool swap = false;

                    for (int i = 0; i < end; )
                    {
                        swap = false;
                        for (int j = i; j < newGroupIdArraySize; j++)
                        {
                            if (oldGroupIdArray[i] == newGroupIdArray[j])
                            {
                                int tmp = newGroupIdArray[i];
                                newGroupIdArray[i] = newGroupIdArray[j];
                                newGroupIdArray[j] = tmp;
                                swap = true;
                                break;
                            }
                        }
                        if (swap != true)
                        {
                            int tmp = oldGroupIdArray[i];
                            oldGroupIdArray[i] = oldGroupIdArray[--end];
                            oldGroupIdArray[end] = tmp;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    #endregion

                    //需删除的
                    List<int> deleteUserGroupIdArray = new List<int>();
                    for (int i = end; i < oldGroupIdArraySize; i++)
                    {
                        deleteUserGroupIdArray.Add(oldGroupIdArray[i]);
                    }

                    //需添加的
                    List<int> addUserGroupIdArray = new List<int>();
                    for (int i = end; i < newGroupIdArraySize; i++)
                    {
                        addUserGroupIdArray.Add(newGroupIdArray[i]);
                    }

                    //执行删除
                    var deleteUserGroupObj = from obj in adminUserGroupList
                                             where deleteUserGroupIdArray.Contains(obj.GroupId)
                                             select obj;
                    foreach (var obj in deleteUserGroupObj)
                    {
                        session.Delete(obj);
                    }

                    //执行添加
                    foreach (var obj in addUserGroupIdArray)
                    {
                        admin_user_group _admin_user_group = new admin_user_group();
                        _admin_user_group.UserId = _admin_user.AdminUserId;
                        _admin_user_group.GroupId = obj;
                        session.Save(_admin_user_group);
                    }

                    #endregion

                    #region 比较该用户包含的新、旧权限集合，新增需要新增的权限，删除需要删除的权限

                    IList<admin_user_permission> adminUserPermissionList = Repository.Find<admin_user_permission>(exp => exp.UserId == _admin_user.AdminUserId);

                    #region 取出两个数组的差集(交换算法)

                    int[] oldPermissionIdArray = adminUserPermissionList.Select(exp => exp.PermissionId).Distinct().ToArray();
                    string[] permissionIds = string.IsNullOrEmpty(param.PermissionIds) ? new string[] { } : param.PermissionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    int[] newPermissionIdArray = Array.ConvertAll<string, int>(permissionIds, delegate(string s) { return int.Parse(s); });

                    int oldPermissionIdArraySize = oldPermissionIdArray.Length;
                    int newPermissionIdArraySize = newPermissionIdArray.Length;
                    end = oldPermissionIdArraySize;
                    swap = false;

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
                    List<int> deleteUserPermissionIdArray = new List<int>();
                    for (int i = end; i < oldPermissionIdArraySize; i++)
                    {
                        deleteUserPermissionIdArray.Add(oldPermissionIdArray[i]);
                    }

                    //需添加的
                    List<int> addUserPermissionIdArray = new List<int>();
                    for (int i = end; i < newPermissionIdArraySize; i++)
                    {
                        addUserPermissionIdArray.Add(newPermissionIdArray[i]);
                    }

                    //执行删除
                    var deleteUserPermissionObj = from obj in adminUserPermissionList
                                                  where deleteUserPermissionIdArray.Contains(obj.PermissionId)
                                                  select obj;
                    foreach (var obj in deleteUserPermissionObj)
                    {
                        session.Delete(obj);
                    }

                    //执行添加
                    foreach (var obj in addUserPermissionIdArray)
                    {
                        admin_user_permission _admin_user_permission = new admin_user_permission();
                        _admin_user_permission.UserId = _admin_user.AdminUserId;
                        _admin_user_permission.PermissionId = obj;
                        session.Save(_admin_user_permission);
                    }

                    #endregion

                    transaction.Commit();

                    //主动更新用户会话
                    new AdminAccountManage().UpdateUserSession(_admin_user.AdminUserId);

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
        public BoolResult Disabled(AdminUserExt param)
        {
            BoolResult result = new BoolResult();

            IList<admin_user> adminUserList = Repository.Find<admin_user>(e => param.AdminUserIds.Contains(e.AdminUserId) && e.Disabled != param.Disabled && e.Deleted == false);
            if (adminUserList.Count == 0)
            {
                result.Succeeded = false;
                result.Message = string.Format("{0}的对象不存在", param.Disabled ? "禁用" : "启用");
                return result;
            }
            else
            {
                using (ISession session = NHibernateHelper<admin_user>.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {

                    try
                    {
                        int intRecord = 0;
                        foreach (var _admin_user in adminUserList)
                        {
                            _admin_user.Disabled = param.Disabled;
                            _admin_user.UpdateBy = param.UpdateBy;
                            _admin_user.UpdateOn = param.UpdateOn;
                            session.Update(_admin_user);

                            intRecord++;
                        }

                        transaction.Commit();

                        foreach (var _admin_user in adminUserList)
                        {
                            //主动更新用户会话
                            new AdminAccountManage().UpdateUserSession(_admin_user.AdminUserId);
                        }

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
        public BoolResult Deleted(AdminUserExt param)
        {
            BoolResult result = new BoolResult();

            IList<admin_user> adminUserList = Repository.Find<admin_user>(e => param.AdminUserIds.Contains(e.AdminUserId) && e.Deleted == false);
            if (adminUserList.Count == 0)
            {
                result.Succeeded = false;
                result.Message = "删除的对象不存在";
                return result;
            }
            else
            {
                using (ISession session = NHibernateHelper<admin_user>.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        int intRecord = 0;
                        foreach (var _admin_user in adminUserList)
                        {
                            //删除用户时需要删除用户与权限的关系、用户与用户组的关系、用户与隶属系统的关系

                            _admin_user.Deleted = true;
                            _admin_user.UpdateBy = param.UpdateBy;
                            _admin_user.UpdateOn = param.UpdateOn;
                            session.Update(_admin_user);

                            IList<admin_user_permission> adminUserPermissionList = Repository.Find<admin_user_permission>(e => e.UserId == _admin_user.AdminUserId);
                            foreach (var _admin_user_permission in adminUserPermissionList)
                            {
                                session.Delete(_admin_user_permission);
                            }

                            IList<admin_user_group> adminUserGroupList = Repository.Find<admin_user_group>(e => e.UserId == _admin_user.AdminUserId);
                            foreach (var _admin_user_group in adminUserGroupList)
                            {
                                session.Delete(_admin_user_group);
                            }

                            intRecord++;
                        }

                        transaction.Commit();

                        foreach (var _admin_user in adminUserList)
                        {
                            //主动更新用户会话
                            new AdminAccountManage().UpdateUserSession(_admin_user.AdminUserId);
                        }

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
        /// 分页获取列表（提供按AdminName、RealName进行模糊搜索，搜索时给AdminName属性赋值，不提供客户端自定义排序，默认按创建时间倒序排列）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IPagedList<admin_user> GetPageList(IPagedParam<admin_user> param)
        {
            IQueryable<admin_user> queryable = Repository.AsQueryable<admin_user>().Where(e => e.Deleted == false);
            if (!string.IsNullOrEmpty(param.model.AdminName))
            {
                queryable = queryable.Where(e => e.AdminName.Contains(param.model.AdminName) || e.RealName.Contains(param.model.AdminName));
            }
            queryable = queryable.OrderByDescending(e => e.CreateOn);

            return new PagedList<admin_user>(queryable, param.PageIndex, param.PageSize);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<admin_user> GetAllUsers()
        {
            return Repository.Find<admin_user>(e => e.Deleted == false).ToList();
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns></returns>
        public IList<admin_user> GetList()
        {
            return Repository.Find<admin_user>(e => e.Disabled == false && e.Deleted == false);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="param">主键ID</param>
        /// <returns></returns>
        public admin_user GetModel(object param)
        {
            return Repository.Get<admin_user>(param);
        }
    }
}
