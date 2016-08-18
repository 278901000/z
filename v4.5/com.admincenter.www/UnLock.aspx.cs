using AdminCenter.WebForm.Driver;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;

namespace com.admincenter.www
{
    public partial class UnLock : WebFormBase
    {
        protected AdminUserExt g_AdminUserExt = null;
        protected string g_strMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string strTicket = Request.Params["code"] == null ? null : Request.Params["code"].Trim();
            string strTo = Request.Params["to"] == null ? "/" : Request.Params["to"].Trim();

            if (!string.IsNullOrEmpty(strTicket))
            {
                if (Request.HttpMethod == "GET")
                {
                    UnLockGet(strTicket, strTo);
                }
                else if (Request.HttpMethod == "POST")
                {
                    UnLockPost(strTicket, strTo);
                }
            }
            else
            {
                Response.Redirect("/404.html");
            }
        }

        private AdminUserExt UnLockGet(string strTicket, string strTo)
        {
            IResponse<BoolResult<AdminUserExt>> response = CallLogic<string, BoolResult<AdminUserExt>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "UnLockAuthentication", strTicket);
            if (response.Succeeded)
            {
                if (response.Result.Succeeded)
                {
                    g_AdminUserExt = response.Result.Result;

                    return response.Result.Result;
                }
                else
                {
                    switch (response.Result.Message)
                    {
                        case "LOGIN_FAIL":
                            Response.Redirect(string.Format("/Login.aspx?to={0}", strTo));
                            break;
                        case "UN_LOCK":
                            strTo = Server.UrlDecode(strTo);
                            Response.Redirect(strTo);
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

        private void UnLockPost(string strTicket, string strTo)
        {
            AdminUserExt adminUserExt = UnLockGet(strTicket, strTo);

            string strPassword = Request.Params["password"] == null ? null : Request.Params["password"].Trim();

            //表单验证
            bool validation = true;
            if (string.IsNullOrEmpty(strPassword))
            {
                validation = false;
            }

            if (validation)
            {
                AdminUserExt param = new AdminUserExt();
                param.Ticket = strTicket;
                param.AdminName = adminUserExt.AdminName;
                param.Password = strPassword.MD5Encrypt();

                IResponse<bool> response = CallLogic<AdminUserExt, bool>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "UnLock", param);
                if (response.Succeeded)
                {
                    if (response.Result)
                    {
                        strTo = Server.UrlDecode(strTo);
                        Response.Redirect(strTo);
                    }
                    else
                    {
                        g_strMsg = "解锁失败";
                    }
                }
                else
                {
                    Response.Redirect("/500.html");
                }
            }
            else
            {
                g_strMsg = "解锁失败";
            }
        }
    }
}