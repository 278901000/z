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
    public partial class GuestOperate : WebFormAdminBase
    {
        protected string g_strTitle = "添加";

        protected string g_strName = "";
        protected string g_strCode = "";
        protected string g_strKey = "";
        protected string g_strDescription = "";
        
        protected string g_strMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (strType != "add" && strType != "update" && strType != "createKey")
            {
                Response.Redirect("/404.html");
            }

            if (Request.HttpMethod == "GET")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_ADD", true);
                    AddGET();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_EDIT", true);
                    g_strTitle = "修改";
                    UpdateGET();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_ADD", true);
                    AddPOST();
                }
                else if (strType == "update")
                {
                    CheckPermission("ADMIN_CENTER_AUTHENTICATION_GUEST_EDIT", true);
                    g_strTitle = "修改";
                    UpdatePOST();
                }
                else
                {
                    CreateKey();
                }
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
        private auth_guest Public_Add_Update(bool isUpdate)
        {
            g_strName = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            g_strKey = Request.Params["Key"] == null ? null : Request.Params["Key"].Trim();
            g_strCode = Request.Params["Code"] == null ? null : Request.Params["Code"].Trim();
            g_strDescription = Request.Params["Description"] == null ? null : Request.Params["Description"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(g_strName))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strKey))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strCode))
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "信息填写有误，请重新填写";
                return null;
            }

            //step2:保存更新对象赋值，共分2部分
            auth_guest _auth_guest = new auth_guest();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                _auth_guest.AuthGuestId = int.Parse(Request.Params["id"]);
                _auth_guest.UpdateBy = AdminUser.AdminName;
                _auth_guest.UpdateOn = DateTime.UtcNow;
            }
            //part2:新增、更新共有赋值部分
            _auth_guest.Name = g_strName;
            _auth_guest.Code = g_strCode;
            _auth_guest.AuthKey = g_strKey;
            _auth_guest.Description = g_strDescription;
            _auth_guest.Deleted = false;
            _auth_guest.CreateBy = AdminUser.AdminName;
            _auth_guest.CreateOn = DateTime.UtcNow;

            return _auth_guest;
        }

        private void AddGET()
        {
        }

        /// <summary>
        /// 添加-POST方式
        /// </summary>
        private void AddPOST()
        {
            auth_guest _auth_guest = Public_Add_Update(false);
            if (_auth_guest != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<auth_guest, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Save", _auth_guest);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "访客身份添加成功";
                    Response.Redirect("/Authentication/Guests.aspx");
                }
                else
                {
                    g_strMsg = "访客身份添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }
        }

        /// <summary>
        /// 更新-GET方式
        /// </summary>
        private void UpdateGET()
        {
            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<auth_guest> response = CallLogic<int, auth_guest>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    g_strName = response.Result.Name;
                    g_strKey = response.Result.AuthKey;
                    g_strCode = response.Result.Code;
                    g_strDescription = response.Result.Description;
                }
                else
                {
                    g_strMsg = "获取当前待编辑访客身份错误";
                }
            }
        }

        /// <summary>
        /// 更新-POST方式
        /// </summary>
        private void UpdatePOST()
        {
            if (UpdateBefore())
            {
                auth_guest _auth_guest = Public_Add_Update(true);
                if (_auth_guest != null)
                {
                    IResponse<auth_guest> responseTemp = CallLogic<int, auth_guest>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Get", _auth_guest.AuthGuestId);
                    if (responseTemp.Succeeded && responseTemp.Result != null)
                    {
                        //UpdateToDB逻辑调用
                        IResponse<BoolResult> response = CallLogic<auth_guest, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AuthGuestManage", "Update", _auth_guest);
                        if (response.Succeeded && response.Result.Succeeded)
                        {
                            Session["msg"] = "访客身份更新成功";
                            
                            Response.Redirect(string.Format("/Authentication/Guests.aspx{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                        }
                        else
                        {
                            g_strMsg = "访客身份更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                        }
                    }
                    else
                    {
                        g_strMsg = "管理员用户更新失败";
                    }
                }
            }
        }

        /// <summary>
        /// 生成GUID，用作KEY
        /// </summary>
        private void CreateKey()
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Succeeded = true;
            jsonResult.MsgType = JsonResult.MessageType.success.ToString();
            jsonResult.Result = Guid.NewGuid().ToString().Replace("-", "");

            Response.Write(jsonResult.JsonSerialize());
            Response.End();
        }
    }
}