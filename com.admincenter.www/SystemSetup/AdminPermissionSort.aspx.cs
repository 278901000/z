using AdminCenter.WebForm.Driver;
using z.AdminCenter.Entity;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminPermissionSort : WebFormAdminBase
    {
        protected string g_strJson = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckPermission("ADMIN_CENTER_SETUP_PERMISSION_SORT", true);

            if (Request.HttpMethod == "GET")
            {
                GetList();
            }
            else if (Request.HttpMethod == "POST")
            {
                switch (Request.Params["type"])
                {
                    case "sort":
                        Sort();
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
            IResponse<string> response = CallLogic<object, string>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminPermissionManage", "GetSortPermissionZTreeJson", null);
            if (response.Succeeded)
            {
                g_strJson = response.Result;
            }
            g_strJson = string.IsNullOrEmpty(g_strJson) ? "[]" : g_strJson;
        }

        private void Sort()
        {
            int intDragPermissionId = -1;
            string strDragPermissionId = Request.Params["dragPermissionId"] == null ? null : Request.Params["dragPermissionId"].Trim();
            int intTargetPermissionId = -1;
            string strTargetPermissionId = Request.Params["targetPermissionId"] == null ? null : Request.Params["targetPermissionId"].Trim();
            string strMoveType = Request.Params["moveType"] == null ? null : Request.Params["moveType"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(strDragPermissionId))
            {
                validation = false;
            }
            else if (!int.TryParse(strDragPermissionId, out intDragPermissionId))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(strTargetPermissionId))
            {
                validation = false;
            }
            else if (!int.TryParse(strTargetPermissionId, out intTargetPermissionId))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(strMoveType))
            {
                validation = false;
            }
            else if (strMoveType != "next" && strMoveType != "prev")
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

            PermissionSort permissionSort = new PermissionSort() { DragNode = new admin_permission(), TargetNode = new admin_permission() };
            permissionSort.DragNode.AdminPermissionId = intDragPermissionId;
            permissionSort.DragNode.UpdateBy = AdminUser.AdminName;
            permissionSort.DragNode.UpdateOn = DateTime.UtcNow;
            permissionSort.TargetNode.AdminPermissionId = intTargetPermissionId;
            permissionSort.TargetNode.UpdateBy = AdminUser.AdminName;
            permissionSort.TargetNode.UpdateOn = DateTime.UtcNow;
            permissionSort.Next = strMoveType == "next" ? true : false;
            IResponse<BoolResult> response = CallLogic<PermissionSort, BoolResult>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminPermissionManage", "Sort", permissionSort);
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                    jsonResult.Succeeded = true;
                    jsonResult.Message = "排序成功";

                    Response.Write(jsonResult.JsonSerialize());
                }
                else
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = "排序失败";

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