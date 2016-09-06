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
    public partial class AdminGroups : WebFormAdminBase
    {
        protected IPagedList<admin_group> g_AdminGroups = new PagedList<admin_group>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                CheckPermission("ADMIN_CENTER_SETUP_GROUP_LIST", true);
                var temp = Session["msg"];
                GetList();
            }
            else
            {
                switch (Request.Params["type"])
                {
                    case "deleted":
                        CheckPermission("ADMIN_CENTER_SETUP_GROUP_DELETE", true);
                        Deleted();
                        break;
                    case "disabled":
                        CheckPermission("ADMIN_CENTER_SETUP_GROUP_DISABLE", true);
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

            IPagedParam<admin_group> param = new PagedParam<admin_group>();
            param.model = new admin_group();
            param.model.Name = strKey;
            param.PageIndex = intPage;
            param.PageSize = intPageSize;

            IResponse<IPagedList<admin_group>> response = CallLogic<IPagedParam<admin_group>, IPagedList<admin_group>>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminGroupManage", "GetPageList", param);
            if (response.Succeeded)
            {
                g_AdminGroups = response.Result;
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

            AdminGroupExt adminGroupExt = new AdminGroupExt();
            adminGroupExt.AdminGroupIds = ids;
            adminGroupExt.UpdateBy = AdminUser.AdminName;
            adminGroupExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminGroupExt, BoolResult>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminGroupManage", "Deleted", adminGroupExt);
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

            AdminGroupExt adminGroupExt = new AdminGroupExt();
            adminGroupExt.AdminGroupIds = ids;
            adminGroupExt.Disabled = bDisable;
            adminGroupExt.UpdateBy = AdminUser.AdminName;
            adminGroupExt.UpdateOn = DateTime.UtcNow;

            IResponse<BoolResult> response = CallLogic<AdminGroupExt, BoolResult>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminGroupManage", "Disabled", adminGroupExt);
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