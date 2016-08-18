using AdminCenter.WebForm.Driver;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;
using System.Text;
using System.Collections.Generic;
using z.AdminCenter.Entity;
using System.Linq;

namespace com.admincenter.www
{
    public partial class Login : WebFormBase
    {
        //登录消息
        protected string g_strMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                if (!string.IsNullOrEmpty(Request.Params["errorCode"]))
                {
                    switch (Request.Params["errorCode"])
                    {
                        case "ERROR_PARAM":
                            g_strMsg = "参数错误";
                            break;
                        case "UNAUTH_SYSTEM":
                            g_strMsg = "未授权的SystemCode";
                            break;
                        case "UNAUTH_KEY":
                            g_strMsg = "未授权的Key";
                            break;
                        case "LOGIN_FAIL":
                            g_strMsg = "用户会话已过期，请重新登录";
                            break;
                        case "OFFLINE":
                            g_strMsg = "您的账号已在别处登录，被迫下线";
                            break;
                        case "LOCKED":
                            g_strMsg = "用户会话已锁定，请解锁后操作";
                            break;
                        case "UNAUTH_USER":
                            g_strMsg = "用户未被授权登录当前系统";
                            break;
                        case "NOT_FOUND":
                            g_strMsg = "未找到请求的页面";
                            break;
                        case "SERVER_ERROR":
                            g_strMsg = "系统错误，请稍候重试";
                            break;
                    }
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                string username = Request.Params["username"] == null ? null : Request.Params["username"].Trim();
                string password = Request.Params["password"] == null ? null : Request.Params["password"].Trim();
                string remember = Request.Params["remember"] == null ? null : Request.Params["remember"].Trim();

                //Form表单验证
                bool validation = true;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    validation = false;
                }

                if (!validation)
                {
                    g_strMsg = "请输入您的用户名、密码.";
                    return;
                }

                //调用业务逻辑处理
                AdminUserExt adminUserExt = new AdminUserExt();
                adminUserExt.AdminName = username;
                adminUserExt.Password = password.MD5Encrypt();
                adminUserExt.RememberMe = remember;
                IResponse<BoolResult<AdminUserExt>> response = CallLogic<AdminUserExt, BoolResult<AdminUserExt>>("z.AdminCenter.Logic.dll", "z.AdminCenter.Logic.AdminAccountManage", "Login", adminUserExt);

                //业务逻辑处理结果集系列判断
                if (response.Succeeded)
                {
                    if (!response.Result.Succeeded)
                    {
                        g_strMsg = "请输入正确的用户名、密码.";
                    }
                    else
                    {
                        #region 跨域写Cookie、跳转

                        StringBuilder scriptsBuilder = new StringBuilder();

                        string strLocation = "/";
                        string strDomain = "";
                        if (!string.IsNullOrEmpty(Request.Params["to"]))
                        {
                            strLocation = Server.UrlDecode(Request.Params["to"]);

                            if (strLocation.IsMatch(@"^http[s]?://[^/\\]+"))
                            {
                                strDomain = strLocation.GetText(@"^http[s]?://[^/\\]+");
                            }
                        }

                        var CallBackUrlList = response.Result.Result.OwnAdminSystems.SelectMany(l => l.CallBackUrl.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).ToList();
                        if (!string.IsNullOrEmpty(strDomain))
                        {
                            var iIndex = CallBackUrlList.FindIndex(l => l.IndexOf(strDomain) > -1);
                            if (iIndex > -1)
                            {
                                scriptsBuilder.AppendFormat("<script src=\"{0}?code={1}&remember={2}\"></script>", CallBackUrlList[iIndex].TrimEnd('/'), response.Result.Result.Ticket, remember);

                                CallBackUrlList.RemoveAt(iIndex);
                            }
                        }

                        foreach (var callBackUrl in CallBackUrlList)
                        {
                            scriptsBuilder.AppendFormat("<script src=\"{0}?code={1}&remember={2}\" async=\"async\" ></script>", callBackUrl.TrimEnd('/'), response.Result.Result.Ticket, remember);
                        }

                        scriptsBuilder.AppendFormat("<script>setTimeout(function(){{ window.location.href='{0}' }}, {1});</script>", strLocation, Utility.GetConfigValue("JumpInterval"));

                        Response.Write(scriptsBuilder.ToString());

                        #endregion
                    }
                }
                else
                {
                    g_strMsg = "登录异常，请重试.";
                }
            }
        }
    }
}