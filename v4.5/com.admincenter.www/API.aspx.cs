﻿using z.Foundation;
using z.AdminCenter.Entity;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdminCenter.WebForm.Driver;

namespace com.admincenter.www
{
    public partial class API : WebFormBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BoolResult result = new BoolResult();

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("sign", Request.Params["sign"]);
            nvc.Add("authCode", Request.Params["authCode"]);
            nvc.Add("authTime", Request.Params["authTime"]);

            IResponse<BoolResult> response = CallLogic<NameValueCollection, BoolResult>("z.AdminCenter.Logic", "z.AdminCenter.Logic.Authentication", "Verify", nvc);
            if(response.Succeeded)
            {
                if(response.Result.Succeeded)
                {
                    IResponse<IList<admin_user>> responseData = CallLogic<object, IList<admin_user>>("z.AdminCenter.Logic", "z.AdminCenter.Logic.AdminUserManage", "GetList", null);
                    if (responseData.Succeeded)
                    {
                        result.Result = responseData.Result;
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.Message = "获取数据异常:" + responseData.Message;
                    }
                }
                else
                {
                    result.Message = "身份验证失败:" + response.Result.Message;
                }
            }
            else
            {
                result.Message = "身份验证异常:" + response.Message;
            }

            Response.Write(result.JsonSerialize());
            Response.End();
        }
    }
}