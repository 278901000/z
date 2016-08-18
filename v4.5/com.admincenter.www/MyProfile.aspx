<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="com.admincenter.www.MyProfile" %>

<!DOCTYPE html>

<!-- 
Template Name: Metronic - Responsive Admin Dashboard Template build with Twitter Bootstrap 3.1.1
Version: 2.0.2
Author: KeenThemes
Website: http://www.keenthemes.com/
Contact: support@keenthemes.com
Purchase: http://themeforest.net/item/metronic-responsive-admin-dashboard-template/4021469?ref=keenthemes
License: You must have a valid license purchased only from themeforest(the above link) in order to legally use the theme for your project.
-->
<!--[if IE 8]> <html lang="en" class="ie8 no-js"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9 no-js"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en" class="no-js">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
    <meta charset="utf-8" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <!--#include File="/AdminTemplate/Header.html"-->
    <style type="text/css">
        .page-sidebar-closed .page-content {
            margin-left: 0 !important;
        }

        .page-content {
            margin-left: 0;
            margin-top: 0px;
            min-height: 600px;
            padding: 25px 20px 20px 20px;
        }
    </style>
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body class="page-sidebar-closed">
    <!-- BEGIN HEADER -->

    <!-- END HEADER -->
    <div class="clearfix">
    </div>
    <!-- BEGIN CONTAINER -->
    <div class="page-container">
        <!-- BEGIN SIDEBAR -->

        <!-- END SIDEBAR -->
        <!-- BEGIN CONTENT -->
        <div class="page-content-wrapper">
            <div class="page-content">
                <!-- BEGIN PAGE HEADER-->
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                        <h3 class="page-title">个人资料 <small>供用户修改密码、用户头像</small>
                        </h3>
                        <ul class="page-breadcrumb breadcrumb">
                            <li>
                                <i class="fa fa-home"></i>
                                <a href="<%= Server.UrlDecode(g_strTo) %>">Home
                                </a>
                                <i class="fa fa-angle-right"></i>
                            </li>
                            <li>
                                <a href="">个人资料修改
                                </a>
                            </li>
                        </ul>
                        <!-- END PAGE TITLE & BREADCRUMB-->
                    </div>
                </div>
                <!-- END PAGE HEADER-->
                <!-- BEGIN PAGE CONTENT-->
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN SAMPLE FORM PORTLET-->
                        <div class="portlet box blue">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>编辑个人资料
                                </div>
                            </div>
                            <div class="portlet-body form">
                                <form id="form1" class="form-horizontal" role="form" method="post" action="">
                                    <div class="form-body">
                                        <div class="alert alert-danger <%= string.IsNullOrEmpty(g_strMsg) ? "display-hide" : "" %>">
                                            <button class="close" data-close="alert"></button>
                                            <%= string.IsNullOrEmpty(g_strMsg) ? "信息填写有误，请重新填写" : g_strMsg %>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                旧密码<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="OldPassword" value="" type="password" class="form-control" placeholder="输入旧密码">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                新密码
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="NewPassword" value="" type="password" class="form-control" placeholder="输入新密码">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                重新输入新密码
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="ConfirmPassword" value="" type="password" class="form-control" placeholder="重新输入新密码">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                头像
                                            </label>
                                            <div class="col-md-4">
                                                <% string strBtnUploadStyle = "";
                                                   if (!string.IsNullOrEmpty(g_strLogo))
                                                   {
                                                       strBtnUploadStyle = "style=\"display:none\"";
                                                %>
                                                <div style="width: 200px; height: 200px; position: relative">
                                                    <a href="javascript:;" style="position: absolute; top: 5px; right: 5px;" class="close" onclick="DeleteImg($(this))">×</a>
                                                    <img style="border: 1px solid #e5e5e5;" src="<%= Foundation.UploadFile.GetThumbnail(g_strLogo, "s2") %>" />
                                                </div>
                                                <%
                                                   } %>
                                                <button id="btnUpload" type="button" class="btn blue start" <%= strBtnUploadStyle %>>
                                                    <i class="fa fa-upload"></i>
                                                    <span>上传头像
                                                    </span>
                                                </button>
                                                <input type="hidden" id="Logo" name="Logo" value="<%= g_strLogo %>" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-actions fluid">
                                        <div class="col-md-offset-3 col-md-9">
                                            <button type="submit" class="btn blue">Submit</button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <!-- END SAMPLE FORM PORTLET-->
                    </div>
                </div>
                <!-- END PAGE CONTENT-->
            </div>
        </div>
        <!-- END CONTENT -->
    </div>
    <!-- END CONTAINER -->
    <!-- BEGIN FOOTER -->
    <!--#include File="/AdminTemplate/Bottom.html"-->
    <!-- END FOOTER -->
    <!--#include File="/AdminTemplate/Footer.html"-->
    <script type="text/javascript">

        //Begin Validation
        var handleValidation = function () {
            var form = $('#form1');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                ignore: "",
                rules: {
                    OldPassword: {
                        required: true
                    }
                },

                invalidHandler: function (event, validator) { //display error alert on form submit              
                    success.hide();
                    error.show();
                    App.scrollTo(error, -200);
                },

                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').addClass('has-error'); // set error class to the control group   
                },

                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    //label
                    //    .closest('.form-group').removeClass('has-error'); // set success class to the control group
                    label.remove();
                },

                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    form.submit();
                }
            });
        }
        //END Validation

        $(document).ready(function () {
            handleValidation();

            //BEGIN UploadFile
            var btnUpload = $('#btnUpload');

            window.DeleteImg = function (obj) {
                //step1:清除图片路径
                //step2:删除图片控件
                //step3:显示上传按钮
                $('#Logo').val('');
                obj.parent().remove();
                btnUpload.show();
            }

            new AjaxUpload(btnUpload, {
                action: '/MyProfile.aspx?type=upload&' + location.search.replace('?', ''),
                data: { AjaxRequest: "true" },
                onChange: function (file, extension) {
                    var re = /png|jpg|gif|bmp/gi;
                    if (!re.test(extension)) {
                        alert("请上传图片格式文件!");
                        return false;
                    }
                },
                onSubmit: function (file, extension) {
                    //step1:隐藏上传按钮
                    //step2:显示loading动画
                    btnUpload.hide();
                    btnUpload.after('<img src="/Resource/img/loading.gif" />');
                },
                onComplete: function (file, response) {
                    //step1:隐藏loading动画
                    //step2:状态判断-成功：赋值图片路径，显示图片控件
                    //step3:状态判断-失败：显示上传按钮
                    //step4:消息显示
                    btnUpload.next('img').remove();

                    response = eval('(' + response + ')');
                    if (response.Succeeded) {
                        $('#Logo').val(response.Result[0]);

                        btnUpload.before('<div style="width:200px; height:200px; position:relative"><a href="javascript:;" style="position:absolute; top:5px; right:5px;" class="close" onclick="DeleteImg($(this))">×</a><img style="border:1px solid #e5e5e5;" src="' + response.Result[1] + '" /></div>');
                    }
                    else {
                        btnUpload.show();
                    }

                    ShowMsg(response.MsgType, response.Message);
                }
            });
            //END UploadFile
        })
    </script>
</body>
<!-- END BODY -->
</html>
