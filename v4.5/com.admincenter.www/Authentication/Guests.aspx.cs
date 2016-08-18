using AdminCenter.WebForm.Driver;
using Entity.AdminCenterDB;
using Foundation;
using Foundation.Data;
using Foundation.LogicInvoke;
using Logic.AdminCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.admincenter.www.Authentication
{
    public partial class Guests : WebFormAdminBase
    {
        protected IPagedList<auth_guest> g_AuthGuests = new PagedList<auth_guest>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_LIST", true);
                var temp = Session["msg"];
                GetList();
            }
            else
            {
                switch (Request.Params["type"])
                {
                    case "deleted":
                        CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_DELETE", true);
                        Deleted();
                        break;
                    case "disabled":
                        CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_DISABLE", true);
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

            IPagedParam<auth_guest> param = new PagedParam<auth_guest>();
            param.model = new auth_guest();
            param.model.Name = strKey;
            param.PageIndex = intPage;
            param.PageSize = intPageSize;

            IResponse<IPagedList<auth_guest>> response = CallLogic<IPagedParam<auth_guest>, IPagedList<auth_guest>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "GetPageList", param);
            if (response.Succeeded)
            {
                g_AuthGuests = response.Result;
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

            AuthGuestExt authGuestExt = new AuthGuestExt();
            authGuestExt.AuthGuestIds = ids;
            authGuestExt.UpdateBy = AdminUser.AdminName;
            authGuestExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AuthGuestExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Deleted", authGuestExt);
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

            AuthGuestExt authGuestExt = new AuthGuestExt();
            authGuestExt.AuthGuestIds = ids;
            authGuestExt.Disabled = bDisable;
            authGuestExt.UpdateBy = AdminUser.AdminName;
            authGuestExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AuthGuestExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Disabled", authGuestExt);
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