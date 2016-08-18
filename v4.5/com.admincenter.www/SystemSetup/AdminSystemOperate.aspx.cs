using AdminCenter.WebForm.Driver;
using z.AdminCenter.Entity;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminSystemOperate : WebFormAdminBase
    {
        protected string g_strTitle = "添加";

        protected string g_strName = "";
        protected string g_strCode = "";
        protected string g_strKey = "";
        protected string g_strUrl = "";
        protected string g_strCallBackUrl = "";
        protected string g_strLogo = "";
        protected string g_strDescription = "";

        protected string g_strMsg = "";

        protected IList<admin_system> g_AdminSystems = new List<admin_system>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (strType != "add" && strType != "update" && strType != "createKey" && strType != "upload")
            {
                Response.Redirect("/404.html");
            }

            if (Request.HttpMethod == "GET")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_SYSTEM_ADD", true);
                    AddGET();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_SYSTEM_EDIT", true);
                    g_strTitle = "修改";
                    UpdateGET();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_SYSTEM_ADD", true);
                    AddPOST();
                }
                else if (strType == "update")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_SYSTEM_EDIT", true);
                    g_strTitle = "修改";
                    UpdatePOST();
                }
                else if (strType == "createKey")
                {
                    CreateKey();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_SYSTEM_UPLOAD", true);
                    Upload();
                }
            }
        }

        /// <summary>
        /// 页面上始终需要加载的资源数据
        /// </summary>
        private void PageResource()
        {
           
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
                g_strMsg = "参数错误";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加、更新操作共同代码（form数据验证 + 组合添加、更新所需的数据对象）
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns>当验证不成功时返回null</returns>
        private admin_system Public_Add_Update(bool isUpdate)
        {
            g_strName = Request.Params["Name"] == null ? null : Request.Params["Name"].Trim();
            g_strKey = Request.Params["Key"] == null ? null : Request.Params["Key"].Trim();
            g_strCode = Request.Params["Code"] == null ? null : Request.Params["Code"].Trim();
            g_strUrl = Request.Params["SystemURL"] == null ? null : Request.Params["SystemURL"].Trim();
            g_strCallBackUrl = Request.Params["CallBackURL"] == null ? null : Request.Params["CallBackURL"].Trim();
            g_strLogo = Request.Params["Logo"] == null ? null : Request.Params["Logo"].Trim();
            g_strDescription = Request.Params["Description"] == null ? null : Request.Params["Description"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(g_strName))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strKey))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strCode))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strUrl))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(g_strCallBackUrl))
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "信息填写有误，请重新填写";
                return null;
            }

            //step2:保存更新对象赋值，共分2部分
            admin_system _admin_system = new admin_system();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                _admin_system.AdminSystemId = int.Parse(Request.Params["id"]);
                _admin_system.UpdateBy = AdminUser.AdminName;
                _admin_system.UpdateOn = DateTime.UtcNow;
            }
            //part2:新增、更新共有赋值部分
            _admin_system.Name = g_strName;
            _admin_system.Code = g_strCode;
            _admin_system.SysKey = g_strKey;
            _admin_system.URL = g_strUrl;
            _admin_system.CallBackUrl = g_strCallBackUrl;
            _admin_system.Logo = g_strLogo;
            _admin_system.Description = g_strDescription;
            _admin_system.Deleted = false;
            _admin_system.CreateBy = AdminUser.AdminName;
            _admin_system.CreateOn = DateTime.UtcNow;

            return _admin_system;
        }

        private void AddGET()
        {
            PageResource();
        }

        /// <summary>
        /// 添加-POST方式
        /// </summary>
        private void AddPOST()
        {
            admin_system _admin_system = Public_Add_Update(false);
            if (_admin_system != null)
            {
                //移动文件到正式目录
                if (!string.IsNullOrEmpty(_admin_system.Logo))
                {
                    _admin_system.Logo = UploadFile.Save("AdminSystemLogo", _admin_system.Logo);
                }

                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<admin_system, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "Save", _admin_system);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "系统添加成功";
                    Response.Redirect("/SystemSetup/AdminSystems.aspx");
                }
                else
                {
                    g_strMsg = "系统添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                }
            }

            PageResource();
        }

        /// <summary>
        /// 更新-GET方式
        /// </summary>
        private void UpdateGET()
        {
            if (UpdateBefore())
            {
                //GetModel逻辑调用
                IResponse<admin_system> response = CallLogic<int, admin_system>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    g_strName = response.Result.Name;
                    g_strKey = response.Result.SysKey;
                    g_strCode = response.Result.Code;
                    g_strDescription = response.Result.Description;
                    g_strUrl = response.Result.URL;
                    g_strCallBackUrl = response.Result.CallBackUrl;
                    g_strLogo = response.Result.Logo;
                }
                else
                {
                    g_strMsg = "获取当前待编辑系统错误";
                }
            }

            PageResource();
        }

        /// <summary>
        /// 更新-POST方式
        /// </summary>
        private void UpdatePOST()
        {
            if (UpdateBefore())
            {
                admin_system _admin_system = Public_Add_Update(true);
                if (_admin_system != null)
                {
                    //判断是否有更新图片
                    //若更新了图片，则需要删除旧的图片
                    IResponse<admin_system> responseTemp = CallLogic<int, admin_system>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "Get", _admin_system.AdminSystemId);
                    if (responseTemp.Succeeded && responseTemp.Result != null)
                    {
                        if (_admin_system.Logo != responseTemp.Result.Logo)
                        {
                            if (!string.IsNullOrEmpty(_admin_system.Logo))
                            {
                                //移动文件到正式目录
                                _admin_system.Logo = UploadFile.Save("AdminSystemLogo", _admin_system.Logo);
                            }
                        }

                        //UpdateToDB逻辑调用
                        IResponse<BoolResult> response = CallLogic<admin_system, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "Update", _admin_system);
                        if (response.Succeeded && response.Result.Succeeded)
                        {
                            Session["msg"] = "系统更新成功";

                            //系统Logo不进行物理删除，由于该值有缓存，删除后将会出现短暂时间内系统Logo无法显示
                            //if (_admin_system.Logo != responseTemp.Result.Logo)
                            //{
                            //    if (!string.IsNullOrEmpty(responseTemp.Result.Logo))
                            //    {
                            //        //删除旧文件
                            //        UploadFile.Delete("AdminSystemLogo", responseTemp.Result.Logo);
                            //    }
                            //}

                            Response.Redirect(string.Format("/SystemSetup/AdminSystems.aspx{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                        }
                        else
                        {
                            g_strMsg = "系统更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
                        }
                    }
                    else
                    {
                        g_strMsg = "管理员用户更新失败";
                    }
                }
            }

            PageResource();
        }

        /// <summary>
        /// 生成GUID，用作KEY
        /// </summary>
        private void CreateKey()
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Succeeded = true;
            jsonResult.MsgType = JsonResult.MessageType.success.ToString();
            jsonResult.Result = Guid.NewGuid().ToString().Replace("-", "");

            Response.Write(jsonResult.JsonSerialize());
            Response.End();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        private void Upload()
        {
            JsonResult jsonResult = new JsonResult();

            try
            {
                string strUploadFileKey = "AdminSystemLogo";
                string path = string.Format("{0}/{1}", strUploadFileKey, DateTime.Now.ToString("yyyy/MM/dd"));
                path = UploadFile.Upload(strUploadFileKey, path);

                if (!string.IsNullOrEmpty(path))
                {
                    jsonResult.Succeeded = true;
                    jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                    jsonResult.Result = new string[] { path, UploadFile.GetThumbnail(path, "s1") };
                    jsonResult.Message = "系统Logo上传成功";
                }
                else
                {
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = "系统Logo上传失败";
                }
            }
            catch
            {
                jsonResult.Succeeded = false;
                jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                jsonResult.Message = "系统Logo上传失败";
            }

            Response.Write(jsonResult.JsonSerialize());
            Response.End();
        }
    }
}