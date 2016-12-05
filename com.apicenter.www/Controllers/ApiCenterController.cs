using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.ApiCenter.Logic;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;

namespace com.apicenter.www.Controllers
{
    public class ApiCenterController : Controller
    {
        // GET: Api
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult VerifySign(string Access_Key, string Sign, long? Timestamp, string Access_Function)
        {
            BoolResult boolResult = new BoolResult();

            if (Timestamp == null)
            {
                boolResult.Message = "Timestamp不能为空";
                return Json(boolResult, JsonRequestBehavior.AllowGet);
            }

            VerifySignParam param = new VerifySignParam()
            {
                Access_Key = Access_Key,
                Timestamp = Timestamp.Value,
                Sign = Sign,
                Access_Function = Access_Function,
                UrlParams = new Dictionary<string, string>()
            };

            if (Request.HttpMethod.ToLower() == "get")
            {
                foreach (var paramKey in Request.QueryString.Keys)
                {
                    param.UrlParams.Add(paramKey.ToString(), Request.QueryString[paramKey.ToString()]);
                }
            }
            else
            {
                foreach (var paramKey in Request.Form.Keys)
                {
                    param.UrlParams.Add(paramKey.ToString(), Request.Form[paramKey.ToString()]);
                }
            }

            IResponse<BoolResult> resposne = CallLogic<VerifySignParam, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiManage", "VerifySign", param);
            if (resposne.Succeeded)
            {
                boolResult = resposne.Result;
            }
            else
            {
                boolResult.Message = "系统异常，请联系管理员";
            }

            return Json(boolResult, JsonRequestBehavior.AllowGet);
        }

        #region 业务逻辑调用方法

        /// <summary>
        /// 业务逻辑调用
        /// </summary>
        /// <param name="assemblyName">程序集名称(e.g. com.buygego.Logic.dll)</param>
        /// <param name="className">类名(e.g. com.buygego.Logic.ClassName)</param>
        /// <param name="methodName">方法名(e.g. MethodName)</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IResponse CallLogic(string assemblyName, string className, string methodName, object param)
        {
            IRequest request = new Request()
            {
                Target = assemblyName,
                Class = className,
                Method = methodName,
                Parameter = param
            };
            return LogicInvoker.Instance.Call(request);
        }

        /// <summary>
        /// 业务逻辑调用
        /// </summary>
        /// <typeparam name="T1">参数类型</typeparam>
        /// <typeparam name="T2">结果集类型</typeparam>
        /// <param name="assemblyName">程序集名称(e.g. com.buygego.Logic.dll)</param>
        /// <param name="className">类名(e.g. com.buygego.Logic.ClassName)</param>
        /// <param name="methodName">方法名(e.g. MethodName)</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IResponse<T2> CallLogic<T1, T2>(string assemblyName, string className, string methodName, T1 param)
        {
            IRequest<T1> request = new Request<T1>()
            {
                Target = assemblyName,
                Class = className,
                Method = methodName,
                Parameter = param
            };
            return LogicInvoker.Instance.Call<T1, T2>(request);
        }
        #endregion
    }
}