using AdminCenter.WebForm.Driver;
using Entity.AdminCenterDB;
using Foundation.Data;
using Foundation.LogicInvoke;
using Logic.AdminCenter;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminGroupOperate : WebFormAdminBase
    {
        protected string g_strTitle = "添加";

        protected string g_strName = "";
        protected string g_strType = "1";
        protected string g_strDescription = "";
        protected string g_strPermissionIds = "";

        protected IList<admin_system> g_AdminSystemList = new List<admin_system>();
        protected string g_strJson = "";

        protected string g_strMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (strType != "add" && strType != "update")
            {
                Response.Redirect("/404.html");
            }

            if (Request.HttpMethod == "GET")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_GROUP_ADD", true);
                    AddGET();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_GROUP_EDIT", true);
                    g_strTitle = "修改";
                    UpdateGET();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_GROUP_ADD", true);
                    AddPOST();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_GROUP_EDIT", true);
                    g_strTitle = "修改";
                    UpdatePOST();
                }
            }
        }

        /// <summary>
        /// 页面上始终需要加载的资源数据
        /// </summary>
        private void PageResource()
        {
            IResponse<string> response = CallLogic<object, string>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "GetAllSystemPermissionZTreeJson", null);
            if (response.Succeeded)
            {
                g_strJson = response.Result;
            }

            IResponse<IList<admin_system>> response2 = CallLogic<object, IList<admin_system>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "GetList", null);
            if (response2.Succeeded)
            {
                g_AdminSystemList = response2.Result;
            }
        }

        /// <summary>
        /// 更新前（URL有效性验证）
        /// </summary>
        private bool UpdateBefore()
        {
            int intId = -1;
            string strId = Request.Params["id"] == null ? null : Request.Params["id"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(strId))
            {
                validation = false;
            }
            else if (!int.TryParse(strId, out intId))
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "参数错误";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加、更新操作共同代码（form数据验证 + 管理员组合添加、更新所需的数据对象）
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns>当验证不成功时返回null</returns>
        private AdminGroupExt Public_Add_Update(bool isUpdate)
        {
            int intType = 1;
            g_strName = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            g_strType = Request.Params["GroupType"] == null ? null : Request.Params["GroupType"].Trim();
            g_strDescription = Request.Params["Description"] == null ? null : Request.Params["Description"].Trim();
            g_strPermissionIds = Request.Params["PermissionIds"] == null ? null : Request.Params["PermissionIds"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(g_strName))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strType))
            {
                validation = false;
            }
            else if (!int.TryParse(g_strType, out intType))
            {
                validation = false;
            }
            else if (intType != 0 && intType != 1)
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "信息填写有误，请重新填写";
                return null;
            }

            //step2:保存更新对象赋值，共分2部分
            AdminGroupExt _admin_group = new AdminGroupExt();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                _admin_group.AdminGroupId = int.Parse(Request.Params["id"]);
                _admin_group.UpdateBy = AdminUser.AdminName;
                _admin_group.UpdateOn = DateTime.UtcNow;
            }
            //part2:新增、更新共有赋值部分
            _admin_group.Name = g_strName;
            _admin_group.Type = intType;
            _admin_group.Description = g_strDescription;
            _admin_group.Disabled = false;
            _admin_group.Deleted = false;
            _admin_group.CreateBy = AdminUser.AdminName;
            _admin_group.CreateOn = DateTime.UtcNow;

            _admin_group.PermissionIds = g_strPermissionIds;

            return _admin_group;
        }

        /// <summary>
        /// 添加-GET方式
        /// </summary>
        private void AddGET()
        {
            PageResource();
        }

        /// <summary>
        /// 添加-POST方式
        /// </summary>
        private void AddPOST()
        {
            AdminGroupExt _admin_group = Public_Add_Update(false);
            if (_admin_group != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<AdminGroupExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminGroupManage", "Save", _admin_group);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "管理员组添加成功";
                    Response.Redirect("/SystemSetup/AdminGroups.aspx");
                }
                else
                {
                    g_strMsg = "管理员组添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }

            PageResource();
        }

        /// <summary>
        /// 更新-GET方式
        /// </summary>
        private void UpdateGET()
        {
            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<AdminGroupExt> response = CallLogic<int, AdminGroupExt>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminGroupManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    g_strName = response.Result.Name;
                    g_strType = response.Result.Type.ToString();
                    g_strDescription = response.Result.Description;
                    g_strPermissionIds = response.Result.PermissionIds;
                }
                else
                {
                    g_strMsg = "获取当前待编辑管理员组错误";
                }
            }

            PageResource();
        }

        /// <summary>
        /// 更新-POST方式
        /// </summary>
        private void UpdatePOST()
        {
            if (UpdateBefore())
            {
                AdminGroupExt _admin_group = Public_Add_Update(true);
                if (_admin_group != null)
                {
                    //UpdateToDB逻辑调用
                    IResponse<BoolResult> response = CallLogic<AdminGroupExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminGroupManage", "Update", _admin_group);
                    if (response.Succeeded && response.Result.Succeeded)
                    {
                        Session["msg"] = "管理员组更新成功";
                        Response.Redirect(string.Format("/SystemSetup/AdminGroups.aspx{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                    }
                    else
                    {
                        g_strMsg = "管理员组更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                    }
                }
            }

            PageResource();
        }
    }
}