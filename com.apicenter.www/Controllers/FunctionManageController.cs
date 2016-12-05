using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.AdminCenter.MVC.Driver;
using z.ApiCenter.Entity;
using z.ApiCenter.Logic;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;

namespace com.apicenter.www.Controllers
{
    public class FunctionManageController : ApiBaseController
    {
        // GET: FunctionManage
        public ActionResult Index()
        {
            return View();
        }

        [CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST")]
        public ActionResult List(string key)
        {
            IPagedList<api_function> result = new PagedList<api_function>();

            IPagedParam<api_function> param = new PagedParam<api_function>();
            param.model = new api_function()
            {
                Name = key
            };

            IResponse<IPagedList<api_function>> response = CallLogic<IPagedParam<api_function>, IPagedList<api_function>>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "GetPageList", param);
            if (response.Succeeded)
            {
                return View(response.Result);
            }

            return View(result);
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST_ADD")]
        public ActionResult Add()
        {
            ViewBag.Title = "添加";
            SetPageView(null);
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST_ADD")]
        public ActionResult Add(string id)
        {
            ViewBag.Title = "添加";

            api_function entity = Public_Add_Update(false);
            if (entity != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<api_function, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "Save", entity);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "Api System添加成功";
                    return Redirect("/FunctionManage/List");
                }
                else
                {
                    ViewBag.Msg = "Api System添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }

            return View("Operate");
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST_EDIT")]
        public ActionResult Edit()
        {
            ViewBag.Title = "修改";

            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<api_function> response = CallLogic<int, api_function>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    SetPageView(response.Result);
                }
                else
                {
                    ViewBag.Msg = "获取当前待编辑Api System错误";
                }
            }

            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST_EDIT")]
        public ActionResult Edit(string id)
        {
            ViewBag.Title = "修改";

            if (UpdateBefore())
            {
                api_function entity = Public_Add_Update(true);
                if (entity != null)
                {
                    //UpdateToDB逻辑调用
                    IResponse<BoolResult> response = CallLogic<api_function, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "Update", entity);
                    if (response.Succeeded && response.Result.Succeeded)
                    {
                        Session["msg"] = "Api System更新成功";

                        Response.Redirect(string.Format("/FunctionManage/List{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                    }
                    else
                    {
                        ViewBag.Msg = "Api System更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                    }
                }
            }
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_FUNCTION_MANAGE_LIST_DELETE")]
        public ActionResult Deleted(string id)
        {

            BoolResult boolResult = new BoolResult();

            if (string.IsNullOrEmpty(id))
            {
                boolResult.Message = "参数错误";
                return Json(boolResult, JsonRequestBehavior.AllowGet);
            }

            ApiFunctionExt param = new ApiFunctionExt();
            param.UpdatedBy = AdminUser.AdminName;
            param.UpdatedOn = DateTime.Now;
            param.FunctionIds = id.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(a => Int32.Parse(a)).ToList();
            IResponse<BoolResult> response = CallLogic<ApiFunctionExt, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "Deleted", param);
            if (response.Succeeded)
            {
                boolResult.Succeeded = true;
            }
            else
            {
                boolResult.Succeeded = false;
                boolResult.Message = "删除失败";
            }

            return Json(boolResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 根据系统获取功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetPermissionBySystem(int id)
        {
            IResponse<string> response = CallLogic<int, string>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "GetSpecifySystemPermissionZTreeJson", id);
            if (response.Succeeded)
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(new BoolResult() { Succeeded = false, Message = "获取失败" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 绑定页面元素
        /// </summary>
        /// <param name="entity"></param>
        private void SetPageView(api_function entity)
        {
            if (entity != null)
            {
                ViewBag.Name = entity.Name;
                ViewBag.FunctionCode = entity.FunctionCode;
                ViewBag.ParentId = entity.ParentId;
                ViewBag.SystemId = entity.SystemId;
            }else
            {
                ViewBag.ParentId = -1;
                ViewBag.SystemId = -1;
            }

            IResponse<IList<api_system>> responseSystem = CallLogic<object, IList<api_system>>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "GetAllAdminSystems", null);
            if (responseSystem.Succeeded)
            {
                ViewBag.Systems = responseSystem.Result;
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
                ViewBag.Msg = "参数错误";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加、更新操作共同代码（form数据验证 + 组合添加、更新所需的数据对象）
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns>当验证不成功时返回null</returns>
        private api_function Public_Add_Update(bool isUpdate)
        {
            //step2:保存更新对象赋值，共分2部分
            api_function entity = new api_function();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                entity.Id = int.Parse(Request.Params["id"]);
                entity.UpdatedBy = AdminUser.AdminName;
                entity.UpdatedOn = DateTime.Now;
            }
            //part2:新增、更新共有赋值部分
            entity.Name = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            entity.FunctionCode = Request.Params["FunctionCode"] == null ? null : Request.Params["FunctionCode"].Trim();
            entity.ParentId= Request.Params["ParentId"] == null ? -1 : Int32.Parse(Request.Params["ParentId"]);
            entity.SystemId= Request.Params["SystemId"] == null ? -1 : Int32.Parse(Request.Params["SystemId"]);

            entity.Deleted = false;
            entity.CreatedBy = AdminUser.AdminName;
            entity.CreatedOn = DateTime.Now;

            SetPageView(entity);

            bool validation = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                validation = false;
            }

            if (string.IsNullOrEmpty(entity.FunctionCode))
            {
                validation = false;
            }

            if (!validation)
            {
                ViewBag.Msg = "信息填写有误，请重新填写";
                return null;
            }

            return entity;
        }
    }
}