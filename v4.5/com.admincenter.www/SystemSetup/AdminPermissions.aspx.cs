using AdminCenter.WebForm.Driver;
using z.AdminCenter.Entity;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminPermissions : WebFormAdminBase
    {
        protected IPagedList<admin_permission> g_AdminPermissions = new PagedList<admin_permission>();
        protected IList<admin_system> g_AdminSystems = new List<admin_system>();

        protected string g_strSystemId = "";
        protected string g_strPermissionId = "";
        protected string g_strKeyword = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_LIST", true);
                var temp = Session["msg"];
                GetList();
                PageResource();
            }
            else
            {
                switch (Request.Params["type"])
                {
                    case "deleted":
                        CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_DELETE", true);
                        Deleted();
                        break;
                    case "disabled":
                        CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_DISABLE", true);
                        Disabled();
                        break;
                    default:
                        JsonResult jsonResult = new JsonResult();
                        jsonResult.Succeeded = false;
                        jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                        jsonResult.Message = "URL不存在";

                        Response.Write(jsonResult.JsonSerialize());
                        Response.End();
                        break;
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

        private void GetList()
        {
            int intPage = 1;
            int intPageSize = 10;
            int intSystemId = 0;
            int intPermissionId = 0;
            string strPage = Request.Params["page"] == null ? null : Request.Params["page"].Trim();
            string strPageSize = Request.Params["pagesize"] == null ? null : Request.Params["pagesize"].Trim();
            g_strSystemId = Request.Params["SystemId"] == null ? null : Request.Params["SystemId"].Trim();
            g_strPermissionId = Request.Params["PermissionId"] == null ? null : Request.Params["PermissionId"].Trim();
            g_strKeyword = Request.Params["key"] == null ? null : Request.Params["key"].Trim();
            
            if (string.IsNullOrEmpty(strPage) || !int.TryParse(strPage, out intPage) || intPage <= 0)
            {
                intPage = 1;
            }
            if (string.IsNullOrEmpty(strPageSize) || !int.TryParse(strPageSize, out intPageSize) || (intPageSize != 10 && intPageSize != 30 && intPageSize != 50))
            {
                intPageSize = 10;
            }
            int.TryParse(g_strSystemId, out intSystemId);
            int.TryParse(g_strPermissionId, out intPermissionId);

            IPagedParam<admin_permission> param = new PagedParam<admin_permission>();
            param.model = new admin_permission();
            param.model.SystemId = intSystemId;
            param.model.AdminPermissionId = intPermissionId;
            param.model.Name = g_strKeyword;
            param.PageIndex = intPage;
            param.PageSize = intPageSize;

            IResponse<IPagedList<admin_permission>> response = CallLogic<IPagedParam<admin_permission>, IPagedList<admin_permission>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "GetPageList", param);
            if (response.Succeeded)
            {
                g_AdminPermissions = response.Result;
            }
        }

        private void Deleted()
        {
            int intId = -1;
            List<int> ids = new List<int>();
            string strIds = Request.Params["id"] == null ? null : Request.Params["id"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(strIds))
            {
                validation = false;
            }
            else
            {
                string[] arrIds = strIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arrIds.Length == 0)
                {
                    validation = false;
                }
                else
                {
                    foreach (var item in arrIds)
                    {
                        if (!int.TryParse(item, out intId) || intId <= 0)
                        {
                            validation = false;
                            break;
                        }
                        else
                        {
                            ids.Add(intId);
                        }
                    }
                }
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

            AdminPermissionExt adminPermissionExt = new AdminPermissionExt();
            adminPermissionExt.AdminPermissionIds = ids;
            adminPermissionExt.UpdateBy = AdminUser.AdminName;
            adminPermissionExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminPermissionExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "Deleted", adminPermissionExt);
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                    jsonResult.Succeeded = true;
                    jsonResult.Message = "删除成功，记录数：" + response.Result.Result;

                    Response.Write(jsonResult.JsonSerialize());
                }
                else
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = "删除失败";

                    Response.Write(jsonResult.JsonSerialize());
                }
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

        private void Disabled()
        {
            int intId = -1;
            bool bDisable = true;
            List<int> ids = new List<int>();
            string strIds = Request.Params["id"] == null ? null : Request.Params["id"].Trim();
            string strDisable = Request.Params["disable"] == null ? null : Request.Params["disable"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(strIds))
            {
                validation = false;
            }
            else
            {
                string[] arrIds = strIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arrIds.Length == 0)
                {
                    validation = false;
                }
                else
                {
                    foreach (var item in arrIds)
                    {
                        if (!int.TryParse(item, out intId) || intId <= 0)
                        {
                            validation = false;
                            break;
                        }
                        else
                        {
                            ids.Add(intId);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(strDisable))
            {
                validation = false;
            }
            else if (!bool.TryParse(strDisable, out bDisable))
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

            AdminPermissionExt adminPermissionExt = new AdminPermissionExt();
            adminPermissionExt.AdminPermissionIds = ids;
            adminPermissionExt.Disabled = bDisable;
            adminPermissionExt.UpdateBy = AdminUser.AdminName;
            adminPermissionExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminPermissionExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "Disabled", adminPermissionExt);
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                    jsonResult.Succeeded = true;
                    jsonResult.Message = (bDisable ? "禁用" : "启用") + "成功，记录数：" + response.Result.Result;

                    Response.Write(jsonResult.JsonSerialize());
                }
                else
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = (bDisable ? "禁用" : "启用") + "失败";

                    Response.Write(jsonResult.JsonSerialize());
                }
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