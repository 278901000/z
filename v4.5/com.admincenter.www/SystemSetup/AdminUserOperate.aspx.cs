using AdminCenter.WebForm.Driver;
using z.AdminCenter.Entity;
using z.Foundation;
using z.Foundation.Data;
using z.Foundation.LogicInvoke;
using z.AdminCenter.Logic;
using System;
using System.Collections.Generic;

namespace com.admincenter.www.SystemSetup
{
    public partial class AdminUserOperate : WebFormAdminBase
    {
        protected string g_strTitle = "添加";

        protected string g_strAdminName = "";
        protected string g_strPassword = "";
        protected string g_strRealName = "";
        protected string g_strLogo = "";
        protected string g_strGroupIds = "";
        protected string g_strPermissionIds = "";

        protected IList<admin_group> g_AdminGroupList = new List<admin_group>();
        protected IList<admin_system> g_AdminSystemList = new List<admin_system>();
        protected string g_strJson = "";

        protected string g_strMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Request.Params["type"] == null ? null : Request.Params["type"].Trim();

            if (strType != "add" && strType != "update" && strType != "upload")
            {
                Response.Redirect("/404.html");
            }

            if (Request.HttpMethod == "GET")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_USER_ADD", true);
                    AddGET();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_USER_EDIT", true);
                    g_strTitle = "修改";
                    UpdateGET();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                if (strType == "add")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_USER_ADD", true);
                    AddPOST();
                }
                else if (strType == "update")
                {
                    CheckPermission("ADMIN_CENTER_SETUP_USER_EDIT", true);
                    g_strTitle = "修改";
                    UpdatePOST();
                }
                else
                {
                    CheckPermission("ADMIN_CENTER_SETUP_USER_UPLOAD", true);
                    Upload();
                }
            }
        }

        /// <summary>
        /// 页面上始终需要加载的资源数据
        /// </summary>
        private void PageResource()
        {
            IResponse<string> response1 = CallLogic<object, string>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminPermissionManage", "GetAllSystemPermissionZTreeJson", null);
            if (response1.Succeeded)
            {
                g_strJson = response1.Result;
            }

            IResponse<IList<admin_group>> response2 = CallLogic<object, IList<admin_group>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminGroupManage", "GetList", null);
            if (response2.Succeeded)
            {
                g_AdminGroupList = response2.Result;
            }

            IResponse<IList<admin_system>> response3 = CallLogic<object, IList<admin_system>>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminSystemManage", "GetList", null);
            if (response3.Succeeded)
            {
                g_AdminSystemList = response3.Result;
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
                g_strMsg = "参数错误";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加、更新操作共同代码（form数据验证 + 管理员用户合添加、更新所需的数据对象）
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns>当验证不成功时返回null</returns>
        private AdminUserExt Public_Add_Update(bool isUpdate)
        {
            g_strAdminName = Request.Params["AdminName"] == null ? null : Request.Params["AdminName"].Trim();
            g_strPassword = Request.Params["Password"] == null ? null : Request.Params["Password"].Trim();
            g_strRealName = Request.Params["RealName"] == null ? null : Request.Params["RealName"].Trim();
            g_strLogo = Request.Params["Logo"] == null ? null : Request.Params["Logo"].Trim();
            g_strGroupIds = Request.Params["GroupIds"] == null ? null : Request.Params["GroupIds"].Trim();
            g_strPermissionIds = Request.Params["PermissionIds"] == null ? null : Request.Params["PermissionIds"].Trim();

            bool validation = true;
            if (string.IsNullOrEmpty(g_strAdminName))
            {
                validation = false;
            }
            if (!isUpdate && string.IsNullOrEmpty(g_strPassword))
            {
                validation = false;
            }

            if (!validation)
            {
                g_strMsg = "信息填写有误，请重新填写";
                return null;
            }

            //step2:保存更新对象赋值，共分2部分
            AdminUserExt _admin_user = new AdminUserExt();

            //part1:更新独立赋值部分
            if (isUpdate)
            {
                _admin_user.AdminUserId = int.Parse(Request.Params["id"]);
                _admin_user.UpdateBy = AdminUser.AdminName;
                _admin_user.UpdateOn = DateTime.UtcNow;
            }
            //part2:新增、更新共有赋值部分
            _admin_user.AdminName = g_strAdminName;
            _admin_user.Password = g_strPassword.MD5Encrypt();
            _admin_user.RealName = g_strRealName;
            _admin_user.Logo = g_strLogo;
            _admin_user.Disabled = false;
            _admin_user.Deleted = false;
            _admin_user.CreateBy = AdminUser.AdminName;
            _admin_user.CreateOn = DateTime.UtcNow;

            _admin_user.GroupIds = g_strGroupIds ?? "";
            _admin_user.PermissionIds = g_strPermissionIds;

            return _admin_user;
        }

        /// <summary>
        /// 添加-GET方式
        /// </summary>
        private void AddGET()
        {
            PageResource();
        }

        /// <summary>
        /// 添加-POST方式
        /// </summary>
        private void AddPOST()
        {
            AdminUserExt _admin_user = Public_Add_Update(false);
            if (_admin_user != null)
            {
                //移动文件到正式目录
                if (!string.IsNullOrEmpty(_admin_user.Logo))
                {
                    _admin_user.Logo = UploadFile.Save("AdminUserLogo", _admin_user.Logo);
                }

                //AddToDB逻辑调用
                IResponse<BoolResult> response = CallLogic<AdminUserExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "Save", _admin_user);
                if (response.Succeeded && response.Result.Succeeded)
                {
                    Session["Msg"] = "管理员用户添加成功";
                    Response.Redirect("/SystemSetup/AdminUsers.aspx");
                }
                else
                {
                    g_strMsg = "管理员用户添加失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
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
                IResponse<AdminUserExt> response = CallLogic<int, AdminUserExt>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "Get", int.Parse(Request.Params["id"]));
                if (response.Succeeded && response.Result != null)
                {
                    g_strAdminName = response.Result.AdminName;
                    g_strRealName = response.Result.RealName;
                    g_strLogo = response.Result.Logo;
                    g_strGroupIds = response.Result.GroupIds;
                    g_strPermissionIds = response.Result.PermissionIds;
                }
                else
                {
                    g_strMsg = "获取当前待编辑管理员用户错误";
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
                AdminUserExt _admin_user = Public_Add_Update(true);
                if (_admin_user != null)
                {
                    //判断是否有更新图片
                    //若更新了图片，则需要删除旧的图片
                    IResponse<admin_user> responseTemp = CallLogic<int, admin_user>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "GetModel", _admin_user.AdminUserId);
                    if (responseTemp.Succeeded && responseTemp.Result != null)
                    {
                        if (_admin_user.Logo != responseTemp.Result.Logo)
                        {
                            if (!string.IsNullOrEmpty(_admin_user.Logo))
                            {
                                //移动文件到正式目录
                                _admin_user.Logo = UploadFile.Save("AdminUserLogo", _admin_user.Logo);
                            }
                        }

                        //UpdateToDB逻辑调用
                        IResponse<BoolResult> response = CallLogic<AdminUserExt, BoolResult>("Logic.AdminCenter.dll", "Logic.AdminCenter.AdminUserManage", "Update", _admin_user);
                        if (response.Succeeded && response.Result.Succeeded)
                        {
                            Session["msg"] = "管理员用户更新成功";

                            if (_admin_user.Logo != responseTemp.Result.Logo)
                            {
                                if (!string.IsNullOrEmpty(responseTemp.Result.Logo))
                                {
                                    //删除旧文件
                                    UploadFile.Delete("AdminUserLogo", responseTemp.Result.Logo);
                                }
                            }

                            Response.Redirect(string.Format("/SystemSetup/AdminUsers.aspx{0}", string.IsNullOrEmpty(Request.Params["custom"]) ? "" : ("?" + Server.UrlDecode(Request.Params["custom"]))));
                        }
                        else
                        {
                            g_strMsg = "管理员用户更新失败" + (string.IsNullOrEmpty(response.Result.Message) ? "" : ("-" + response.Result.Message));
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
        /// 文件上传
        /// </summary>
        private void Upload()
        {
            JsonResult jsonResult = new JsonResult();

            try
            {
                string strUploadFileKey = "AdminUserLogo";
                string path = string.Format("{0}/{1}", strUploadFileKey, DateTime.Now.ToString("yyyy/MM/dd"));
                path = UploadFile.Upload(strUploadFileKey, path);

                if (!string.IsNullOrEmpty(path))
                {
                    jsonResult.Succeeded = true;
                    jsonResult.MsgType = JsonResult.MessageType.success.ToString();
                    jsonResult.Result = new string[] { path, UploadFile.GetThumbnail(path, "s2") };
                    jsonResult.Message = "用户头像上传成功";
                }
                else
                {
                    jsonResult.Succeeded = false;
                    jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                    jsonResult.Message = "用户头像上传失败";
                }
            }
            catch
            {
                jsonResult.Succeeded = false;
                jsonResult.MsgType = JsonResult.MessageType.error.ToString();
                jsonResult.Message = "用户头像上传失败";
            }

            Response.Write(jsonResult.JsonSerialize());
            Response.End();
        }
    }
}