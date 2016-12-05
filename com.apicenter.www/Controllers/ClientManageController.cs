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
    public class ClientManageController : ApiBaseController
    {
        // GET: ClientManage
        public ActionResult Index()
        {
            return View();
        }

        [CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST")]
        public ActionResult List(string key)
        {
            IPagedList<api_client> result = new PagedList<api_client>();

            IPagedParam<api_client> param = new PagedParam<api_client>();
            param.model = new api_client()
            {
                Name = key
            };

            IResponse<IPagedList<api_client>> response = CallLogic<IPagedParam<api_client>, IPagedList<api_client>>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiClientManage", "GetPageList", param);
            if (response.Succeeded)
            {
                return View(response.Result);
            }

            return View(result);
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST_ADD")]
        public ActionResult Add()
        {
            ViewBag.Title = "添加";
            ViewBag.AccessKey = Guid.NewGuid().ToString().Replace("-", "");
            ViewBag.SecretKey = Guid.NewGuid().ToString().Replace("-", "");
            PageResource();
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST_ADD")]
        public ActionResult Add(string id)
        {
            ViewBag.Title = "添加";
            PageResource();
            ApiClientExt entity = Public_Add_Update(false);
            if (entity != null)
            {
                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<ApiClientExt, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiClientManage", "Save", entity);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "Api System添加成功";
                    return Redirect("/ClientManage/List");
                }
                else
                {
                    ViewBag.Msg = "Api System添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }

            return View("Operate");
        }

        [HttpGet, CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST_EDIT")]
        public ActionResult Edit()
        {
            ViewBag.Title = "修改";
            PageResource();
            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<ApiClientExt> response = CallLogic<int, ApiClientExt>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiClientManage", "GetClientAndFunction", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    SetPageView(response.Result);
                }
                else
                {
                    ViewBag.Msg = "获取当前待编辑Api Client错误";
                }
            }

            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST_EDIT")]
        public ActionResult Edit(string id)
        {
            ViewBag.Title = "修改";
            PageResource();
            if (UpdateBefore())
            {
                ApiClientExt entity = Public_Add_Update(true);
                if (entity != null)
                {
                    //UpdateToDB逻辑调用
                    IResponse<BoolResult> response = CallLogic<ApiClientExt, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiClientManage", "Update", entity);
                    if (response.Succeeded && response.Result.Succeeded)
                    {
                        Session["msg"] = "Api Client更新成功";

                        Response.Redirect(string.Format("/ClientManage/List{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                    }
                    else
                    {
                        ViewBag.Msg = "Api System更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                    }
                }
            }
            return View("Operate");
        }

        [HttpPost, CheckPermission("API_CENTER_SETUP_CLIENT_MANAGE_LIST_DELETE")]
        public ActionResult Deleted(string id)
        {

            BoolResult boolResult = new BoolResult();

            if (string.IsNullOrEmpty(id))
            {
                boolResult.Message = "参数错误";
                return Json(boolResult, JsonRequestBehavior.AllowGet);
            }

            ApiClientExt param = new ApiClientExt();
            param.UpdatedBy = AdminUser.AdminName;
            param.UpdatedOn = DateTime.Now;
            param.ClientIds = id.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(a => Int32.Parse(a)).ToList();
            IResponse<BoolResult> response = CallLogic<ApiClientExt, BoolResult>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiClientManage", "Deleted", param);
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


        private void PageResource()
        {
            IResponse<string> response1 = CallLogic<object, string>("z.ApiCenter.Logic.dll", "z.ApiCenter.Logic.ApiFunctionManage", "GetAllApiPermissionZTreeJson", null);
            if (response1.Succeeded)
            {
                ViewBag.zNodeJson = response1.Result;
            }
        }

        /// <summary>
        /// 绑定页面元素
        /// </summary>
        /// <param name="entity"></param>
        private void SetPageView(ApiClientExt entity)
        {
            ViewBag.Name = entity.Name;
            ViewBag.AccessKey = entity.AccessKey;
            ViewBag.SecretKey = entity.SecretKey;
            if (entity.FunctionIds != null && entity.FunctionIds.Count > 0)
            {
                ViewBag.PermissionIds = string.Join(",", entity.FunctionIds);
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
        private ApiClientExt Public_Add_Update(bool isUpdate)
        {
            //step2:保存更新对象赋值，共分2部分
            ApiClientExt entity = new ApiClientExt();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                entity.Id = int.Parse(Request.Params["id"]);
                entity.UpdatedBy = AdminUser.AdminName;
                entity.UpdatedOn = DateTime.Now;
            }
            //part2:新增、更新共有赋值部分
            entity.Name = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            entity.AccessKey = Request.Params["AccessKey"] == null ? null : Request.Params["AccessKey"].Trim();
            entity.SecretKey = Request.Params["SecretKey"] == null ? null : Request.Params["SecretKey"].Trim();
            if (Request.Params["PermissionIds"] != null)
            {
                entity.FunctionIds = Request.Params["PermissionIds"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(a => Int32.Parse(a)).ToList();
            }
            entity.Deleted = false;
            entity.CreatedBy = AdminUser.AdminName;
            entity.CreatedOn = DateTime.Now;

            SetPageView(entity);

            bool validation = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                validation = false;
            }

            if (string.IsNullOrEmpty(entity.AccessKey))
            {
                validation = false;
            }

            if (string.IsNullOrEmpty(entity.SecretKey))
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