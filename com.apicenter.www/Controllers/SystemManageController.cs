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
    public class SystemManageController : ApiBaseController
    {
        // GET: SystemManage
        public ActionResult Index()
        {
            return View();
        }

        [CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST")]
        public ActionResult List(string key)
        {
            IPagedList<api_system> result = new PagedList<api_system>();

            IPagedParam<api_system> param = new PagedParam<api_system>();
            param.model = new api_system()
            {
                Name = key
            };

            IResponse<IPagedList<api_system>> response = CallLogic<IPagedParam<api_system>, IPagedList<api_system>>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "GetPageList", param);
            if (response.Succeeded)
            {
                return View(response.Result);
            }

            return View(result);
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST_ADD")]
        public ActionResult Add()
        {
            ViewBag.Title = "添加";
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST_ADD")]
        public ActionResult Add(string id)
        {
            ViewBag.Title = "添加";

            api_system entity = Public_Add_Update(false);
            if (entity != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<api_system, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "Save", entity);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "Api System添加成功";
                    return Redirect("/SystemManage/List");
                }
                else
                {
                    ViewBag.Msg = "Api System添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }

            return View("Operate");
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST_EDIT")]
        public ActionResult Edit()
        {
            ViewBag.Title = "修改";

            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<api_system> response = CallLogic<int, api_system>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "Get", int.Parse(Request.Params["id"]));
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

        [HttpPost, CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST_EDIT")]
        public ActionResult Edit(string id)
        {
            ViewBag.Title = "修改";

            if (UpdateBefore())
            {
                api_system entity = Public_Add_Update(true);
                if (entity != null)
                {
                    //UpdateToDB逻辑调用
                    IResponse<BoolResult> response = CallLogic<api_system, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "Update", entity);
                    if (response.Succeeded && response.Result.Succeeded)
                    {
                        Session["msg"] = "Api System更新成功";

                        Response.Redirect(string.Format("/SystemManage/List{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                    }
                    else
                    {
                        ViewBag.Msg = "Api System更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                    }
                }
            }
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_SYSTEM_MANAGE_LIST_DELETE")]
        public ActionResult Deleted(string id)
        {

            BoolResult boolResult = new BoolResult();

            if (string.IsNullOrEmpty(id))
            {
                boolResult.Message = "参数错误";
                return Json(boolResult, JsonRequestBehavior.AllowGet);
            }

            ApiSystemExt param = new ApiSystemExt();
            param.UpdatedBy= AdminUser.AdminName;
            param.UpdatedOn = DateTime.Now;
            param.AdminSystemIds = id.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(a => Int32.Parse(a)).ToList();
            IResponse<BoolResult> response = CallLogic<ApiSystemExt, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiSystemManage", "Deleted", param);
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
        /// 绑定页面元素
        /// </summary>
        /// <param name="entity"></param>
        private void SetPageView(api_system entity)
        {
            ViewBag.Name = entity.Name;
            ViewBag.Description = entity.Description;
            ViewBag.SystemCode = entity.SystemCode;
            ViewBag.Url = entity.Url;
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
        private api_system Public_Add_Update(bool isUpdate)
        {
            //step2:保存更新对象赋值，共分2部分
            api_system  entity = new api_system();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                entity.Id = int.Parse(Request.Params["id"]);
                entity.UpdatedBy = AdminUser.AdminName;
                entity.UpdatedOn = DateTime.Now;
            }
            //part2:新增、更新共有赋值部分
            entity.Name = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            entity.SystemCode = Request.Params["SystemCode"] == null ? null : Request.Params["SystemCode"].Trim();
            entity.Url = Request.Params["Url"] == null ? null : Request.Params["Url"].Trim();
            entity.Description = Request.Params["Description"] == null ? null : Request.Params["Description"].Trim();
            entity.Deleted = false;
            entity.CreatedBy = AdminUser.AdminName;
            entity.CreatedOn = DateTime.Now;

            SetPageView(entity);

            bool validation = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                validation = false;
            }

            if (string.IsNullOrEmpty(entity.SystemCode))
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