using AdminCenter.WebForm.Driver;
using System;

namespace com.admincenter.www
{
    public partial class Lock : WebFormBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strTicket = Request.Params["code"] == null ? null : Request.Params["code"].Trim();
            string strTo = Request.Params["to"] == null ? "/" : Request.Params["to"].Trim();

            if (!string.IsNullOrEmpty(strTicket))
            {
                CallLogic<string, object>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "Lock", strTicket);
            }

            strTo = Server.UrlDecode(strTo);
            Response.Redirect(strTo);
        }
    }
}