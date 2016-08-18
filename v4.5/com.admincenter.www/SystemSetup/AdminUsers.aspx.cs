using AdminCenter.WebForm.Driver;
using Entity.AdminCenterDB;
using Foundation;
using Foundation.Data;
using Foundation.LogicInvoke;
using Logic.AdminCenter;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminUsers : WebFormAdminBase
    {
        protected IPagedList<admin_user> g_AdminUsers = new PagedList<admin_user>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                CheckPermission("ADMIN_CENTER_SETUP_USER_LIST", true);
                var temp = Session["msg"];
                GetList();
            }
            else
            {
                switch (Request.Params["type"])
                {
                    case "deleted":
                        CheckPermission("ADMIN_CENTER_SETUP_USER_DELETE", true);
                        Deleted();
                        break;
                    case "disabled":
                        CheckPermission("ADMIN_CENTER_SETUP_USER_DISABLE", true);
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

        private void GetList()
        {
            int intPage = 1;
            string strPage = Request.Params["page"] == null ? null : Request.Params["page"].Trim();
            int intPageSize = 10;
            string strPageSize = Request.Params["pagesize"] == null ? null : Request.Params["pagesize"].Trim();
            string strKey = Request.Params["key"] == null ? null : Request.Params["key"].Trim();

            if (string.IsNullOrEmpty(strPage) || !int.TryParse(strPage, out intPage) || intPage <= 0)
            {
                intPage = 1;
            }
            if (string.IsNullOrEmpty(strPageSize) || !int.TryParse(strPageSize, out intPageSize) || (intPageSize != 10 && intPageSize != 30 && intPageSize != 50))
            {
                intPageSize = 10;
            }

            IPagedParam<admin_user> param = new PagedParam<admin_user>();
            param.model = new admin_user();
            param.model.AdminName = strKey;
            param.PageIndex = intPage;
            param.PageSize = intPageSize;

            IResponse<IPagedList<admin_user>> response = CallLogic<IPagedParam<admin_user>, IPagedList<admin_user>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "GetPageList", param);
            if (response.Succeeded)
            {
                g_AdminUsers = response.Result;
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

            AdminUserExt adminSystemExt = new AdminUserExt();
            adminSystemExt.AdminUserIds = ids;
            adminSystemExt.UpdateBy = AdminUser.AdminName;
            adminSystemExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminUserExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "Deleted", adminSystemExt);
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

            AdminUserExt adminUserExt = new AdminUserExt();
            adminUserExt.AdminUserIds = ids;
            adminUserExt.Disabled = bDisable;
            adminUserExt.UpdateBy = AdminUser.AdminName;
            adminUserExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminUserExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "Disabled", adminUserExt);
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