using AdminCenter.WebForm.Driver;
using Entity.AdminCenterDB;
using Foundation;
using Foundation.Data;
using Foundation.LogicInvoke;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminPermissionOperate : WebFormAdminBase
    {
        protected string g_strTitle = "添加";

        protected string g_strSystemId = "";
        protected string g_strParentId = "-1";
        protected string g_strPermissionCode = "";
        protected string g_strName = "";
        protected string g_strImg = "";
        protected string g_strIsMenu = "";
        protected string g_strIsLink = "";
        protected string g_strUrl = "";
        protected string g_strTarget = "_self";
        protected string g_strDescription = "";

        protected string g_strMsg = "";

        protected IList<admin_system> g_AdminSystems = new List<admin_system>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (strType != "add" && strType != "update" && strType != "getPermissionBySystem")
            {
                Response.Redirect("/404.html");
            }

            if (Request.HttpMethod == "GET")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_ADD", true);
                    AddGET();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_EDIT", true);
                    g_strTitle = "修改";
                    UpdateGET();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                if (strType == "getPermissionBySystem")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_GETPERMISSIONBYSYSTEM", true);
                    GetPermissionBySystem();
                }
                else if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_ADD", true);
                    AddPOST();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_EDIT", true);
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
            IResponse<IList<admin_system>> response = CallLogic<object, IList<admin_system>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "GetList", null);
            if (response.Succeeded)
            {
                g_AdminSystems = response.Result;
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
        /// 添加、更新操作共同代码（form数据验证 + 组合添加、更新所需的数据对象）
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns>当验证不成功时返回null</returns>
        private admin_permission Public_Add_Update(bool isUpdate)
        {
            int intSystemId = 0;
            int intParentId = -1;
            g_strSystemId = Request.Params["SystemId"] == null ? null : Request.Params["SystemId"].Trim();
            g_strParentId = Request.Params["ParentId"] == null ? null : Request.Params["ParentId"].Trim();
            g_strPermissionCode = Request.Params["PermissionCode"] == null ? null : Request.Params["PermissionCode"].Trim();
            g_strName = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            g_strImg = Request.Params["Img"] == null ? null : Request.Params["Img"].Trim();
            g_strIsMenu = Request.Params["IsMenu"] == null ? null : Request.Params["IsMenu"].Trim();
            g_strIsLink = Request.Params["IsLink"] == null ? null : Request.Params["IsLink"].Trim();
            g_strUrl = Request.Params["LinkUrl"] == null ? null : Request.Params["LinkUrl"].Trim();
            g_strTarget = Request.Params["Target"] == null ? null : Request.Params["Target"].Trim();
            g_strDescription = Request.Params["Description"] == null ? null : Request.Params["Description"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(g_strSystemId))
            {
                validation = false;
            }
            else if (!int.TryParse(g_strSystemId, out intSystemId))
            {
                validation = false;
            }
            else if (intSystemId <= 0)
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strParentId))
            {
                validation = false;
            }
            else if (!int.TryParse(g_strParentId, out intParentId))
            {
                validation = false;
            }
            else if (intParentId < -1)
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strPermissionCode))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strName))
            {
                validation = false;
            }
            if (!string.IsNullOrEmpty(g_strIsLink) && string.IsNullOrEmpty(g_strUrl))
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "信息填写有误，请重新填写";
                return null;
            }

            //step2:保存更新对象赋值，共分2部分
            admin_permission _admin_permission = new admin_permission();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                _admin_permission.AdminPermissionId = int.Parse(Request.Params["id"]);
                _admin_permission.UpdateBy = AdminUser.AdminName;
                _admin_permission.UpdateOn = DateTime.UtcNow;
            }
            //part2:新增、更新共有赋值部分
            _admin_permission.SystemId = intSystemId;
            _admin_permission.ParentId = intParentId;
            _admin_permission.PermissionCode = g_strPermissionCode;
            _admin_permission.Name = g_strName;
            _admin_permission.Img = g_strImg;
            _admin_permission.IsMenu = string.IsNullOrEmpty(g_strIsMenu) ? false : true;
            _admin_permission.IsLink = string.IsNullOrEmpty(g_strIsLink) ? false : true;
            if (!string.IsNullOrEmpty(g_strIsLink))
            {
                _admin_permission.Url = g_strUrl;
                _admin_permission.Target = g_strTarget;
            }
            if (!string.IsNullOrEmpty(g_strDescription))
            {
                _admin_permission.Description = g_strDescription;
            }
            _admin_permission.Disabled = false;
            _admin_permission.Deleted = false;
            _admin_permission.CreateBy = AdminUser.AdminName;
            _admin_permission.CreateOn = DateTime.UtcNow;

            return _admin_permission;
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
            admin_permission _admin_permission = Public_Add_Update(false);
            if (_admin_permission != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<admin_permission, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "Save", _admin_permission);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "系统权限添加成功";
                    Response.Redirect("/SystemSetup/AdminPermissions.aspx");
                }
                else
                {
                    g_strMsg = "系统权限添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
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
                IResponse<admin_permission> response = CallLogic<int, admin_permission>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    g_strSystemId = response.Result.SystemId.ToString();
                    g_strParentId = response.Result.ParentId.ToString();
                    g_strPermissionCode = response.Result.PermissionCode;
                    g_strName = response.Result.Name;
                    g_strImg = response.Result.Img;
                    g_strIsMenu = response.Result.IsMenu ? "on" : "";
                    g_strIsLink = response.Result.IsLink ? "on" : "";
                    g_strUrl = response.Result.Url;
                    g_strTarget = string.IsNullOrEmpty(response.Result.Target) ? "_self" : response.Result.Target;
                    g_strDescription = response.Result.Description;
                }
                else
                {
                    g_strMsg = "获取当前待编辑系统权限错误";
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
                admin_permission _admin_permission = Public_Add_Update(true);
                if (_admin_permission != null)
                {
                    //UpdateToDB逻辑调用
                    IResponse<BoolResult> response = CallLogic<admin_permission, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "Update", _admin_permission);
                    if (response.Succeeded && response.Result.Succeeded)
                    {
                        Session["msg"] = "系统权限更新成功";
                        Response.Redirect(string.Format("/SystemSetup/AdminPermissions.aspx{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                    }
                    else
                    {
                        g_strMsg = "系统权限更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                    }
                }
            }

            PageResource();
        }

        /// <summary>
        /// 根据系统Id获取系统包含的系统权限列表
        /// </summary>
        private void GetPermissionBySystem()
        {
            int intId = -1;
            string strId = Request.Params["id"] == null ? null : Request.Params["id"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(strId))
            {
                validation = false;
            }
            else if (!int.TryParse(strId, out intId) || intId <= 0)
            {
                validation = false;
            }

            if (!validation)
            {
                JsonResult jsonResult = new JsonResult();
                jsonResult.Succeeded = false;
                jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                jsonResult.Message = "参数错误";

                Response.Write(jsonResult.JsonSerialize());
                Response.End();
            }

            IResponse<string> response = CallLogic<int, string>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "GetSpecifySystemPermissionZTreeJson", intId);
            if (response.Succeeded)
            {
                JsonResult jsonResult = new JsonResult();
                jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                jsonResult.Succeeded = true;
                jsonResult.Result = response.Result;

                Response.Write(jsonResult.JsonSerialize());
            }
            else
            {
                JsonResult jsonResult = new JsonResult();
                jsonResult.Succeeded = false;
                jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                jsonResult.Message = "系统异常";

                Response.Write(jsonResult.JsonSerialize());
            }
            Response.End();
        }
    }
}