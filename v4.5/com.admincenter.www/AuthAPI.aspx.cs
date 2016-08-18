using AdminCenter.WebForm.Driver;
using Foundation.Data;
using Foundation.LogicInvoke;
using System;
using System.Collections.Specialized;

namespace com.admincenter.www
{
    public partial class AuthAPI : WebFormBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                IResponse<BoolResult> response = CallLogic<NameValueCollection, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminAccountManage", "Authentication", Request.Form);
                if (response.Succeeded)
                {
                    Response.Write(response.Result.Message);
                }
                else
                {
                    Response.Write("SERVER_ERROR");
                }
            }
            else
            {
                Response.Write("NOT_FOUND");
            }

            Response.End();
        }
    }
}