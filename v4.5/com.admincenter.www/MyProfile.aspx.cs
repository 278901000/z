using AdminCenter.WebForm.Driver;
using Foundation;
using Foundation.Data;
using Foundation.LogicInvoke;
using Logic.AdminCenter;
using System;

namespace com.admincenter.www
{
    public partial class MyProfile : WebFormBase
    {
        protected string g_strLogo = "";
        protected string g_strTo = "";
        protected string g_strMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strTicket = Request.Params["code"] == null ? null : Request.Params["code"].Trim();
            g_strTo = Request.Params["to"] == null ? "/" : Request.Params["to"].Trim();
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (!string.IsNullOrEmpty(strTicket))
            {
                if (Request.HttpMethod == "GET")
                {
                    UpdateGET(strTicket, g_strTo);
                }
                else if (Request.HttpMethod == "POST")
                {
                    if (strType == "upload")
                    {
                        Upload(strTicket);
                    }
                    else
                    {
                        UpdatePOST(strTicket, g_strTo);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Params["AjaxRequest"]) && Request.Params["AjaxRequest"] == "true")
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = "未找到请求的页面";

                    Response.Write(jsonResult.JsonSerialize());
                    Response.End();
                }
                else
                {
                    Response.Redirect("/404.html");
                }
            }
        }

        /// <summary>
        /// 更新-GET方式
        /// </summary>
        private AdminUserExt UpdateGET(string strTicket, string strTo)
        {
            IResponse<BoolResult<AdminUserExt>> response = CallLogic<string[], BoolResult<AdminUserExt>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "BaseAuthentication", new string[] { strTicket, "UpdateProfile" });
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    g_strLogo = response.Result.Result.Logo;

                    return response.Result.Result;
                }
                else
                {
                    switch (response.Result.Message)
                    {
                        case "LOGIN_FAIL":
                        case "OFFLINE":
                            Response.Redirect(string.Format("/Login.aspx?to={0}&errorCode={1}", strTo, response.Result.Message));
                            break;
                        case "LOCKED":
                            Response.Redirect(string.Format("/UnLock.aspx?code={0}&to={1}", strTicket, strTo));
                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("/500.html");
            }

            return null;
        }

        /// <summary>
        /// 更新-POST方式
        /// </summary>
        private void UpdatePOST(string strTicket, string strTo)
        {
            AdminUserExt adminUserExt = UpdateGET(strTicket, strTo);
            string strOldLogo = adminUserExt.Logo;

            string g_strOldPassword = Request.Params["OldPassword"] == null ? null : Request.Params["OldPassword"].Trim();
            string g_strNewPassword = Request.Params["NewPassword"] == null ? null : Request.Params["NewPassword"].Trim();
            string g_strConfirmPassword = Request.Params["ConfirmPassword"] == null ? null : Request.Params["ConfirmPassword"].Trim();
            g_strLogo = Request.Params["Logo"] == null ? null : Request.Params["Logo"].Trim();

            //表单验证
            bool validation = true;
            if (string.IsNullOrEmpty(g_strOldPassword))
            {
                validation = false;
            }
            if (!string.IsNullOrEmpty(g_strNewPassword))
            {
                if (string.IsNullOrEmpty(g_strConfirmPassword))
                {
                    validation = false;
                }
                else if (g_strNewPassword != g_strConfirmPassword)
                {
                    validation = false;
                }
            }

            if (validation)
            {
                AdminUserExt param = new AdminUserExt();
                param.AdminUserId = adminUserExt.AdminUserId;
                param.Ticket = adminUserExt.Ticket;
                param.OldPassword = g_strOldPassword.MD5Encrypt();
                param.Password = g_strNewPassword;
                param.ConfirmPassword = g_strConfirmPassword;
                param.Logo = g_strLogo;
                param.UpdateBy = adminUserExt.AdminName;
                param.UpdateOn = DateTime.UtcNow;

                if (g_strLogo != strOldLogo)
                {
                    if (!string.IsNullOrEmpty(g_strLogo))
                    {
                        //移动文件到正式目录
                        param.Logo = UploadFile.Save("AdminUserLogo", g_strLogo);
                    }
                }

                IResponse<bool> responseUpdate = CallLogic<AdminUserExt, bool>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "UpdateProfile", param);
                if (responseUpdate.Succeeded)
                {
                    if (responseUpdate.Result)
                    {
                        if (g_strLogo != strOldLogo)
                        {
                            if (!string.IsNullOrEmpty(strOldLogo))
                            {
                                //删除旧文件
                                UploadFile.Delete("AdminUserLogo", strOldLogo);
                            }
                        }
                        strTo = Server.UrlDecode(strTo);
                        Response.Redirect(strTo);
                    }
                    else
                    {
                        g_strMsg = "用户资料更新失败";
                    }
                }
                else
                {
                    Response.Redirect("/500.html");
                }
            }
            else
            {
                g_strMsg = "参数错误";
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        private void Upload(string strTicket)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Succeeded = false;
            jsonResult.MsgType = JsonResult.MessageType.error.ToString();

            IResponse<BoolResult<AdminUserExt>> response = CallLogic<string[], BoolResult<AdminUserExt>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "BaseAuthentication", new string[] { strTicket, "UpdateProfile" });
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    try
                    {
                        string strUploadFileKey = "AdminUserLogo";
                        string path = string.Format("{0}/{1}", strUploadFileKey, DateTime.Now.ToString("yyyy/MM/dd"));
                        path = UploadFile.Upload(strUploadFileKey, path);

                        if (!string.IsNullOrEmpty(path))
                        {
                            jsonResult.Succeeded = true;
                            jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                            jsonResult.Result = new string[] { path, UploadFile.GetThumbnail(path, "s2") };
                            jsonResult.Message = "用户头像上传成功";
                        }
                        else
                        {
                            jsonResult.Message = "用户头像上传失败";
                        }
                    }
                    catch
                    {
                        jsonResult.Message = "用户头像上传失败";
                    }
                }
                else
                {
                    switch (response.Result.Message)
                    {
                        case "LOGIN_FAIL":
                            jsonResult.Message = "用户会话已过期，请重新登录";
                            break;
                        case "OFFLINE":
                            jsonResult.Message = "当前用户在别处登录，被迫下线，请及时修改密码";
                            break;
                        case "LOCKED":
                            jsonResult.Message = "用户会话已锁定，请解锁后操作";
                            break;
                    }
                }
            }
            else
            {
                jsonResult.Message = "系统错误，请稍候重试";
            }

            Response.Write(jsonResult.JsonSerialize());
            Response.End();
        }
    }
}