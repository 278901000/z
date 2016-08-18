<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuestOperate.aspx.cs" Inherits="com.admincenter.www.Authentication.GuestOperate" %>

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
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body class="page-header-fixed">
    <!-- BEGIN HEADER -->
    <!--#include File="/AdminTemplate/Top.html"-->
    <!-- END HEADER -->
    <div class="clearfix">
    </div>
    <!-- BEGIN CONTAINER -->
    <div class="page-container">
        <!-- BEGIN SIDEBAR -->
        <!--#include File="/AdminTemplate/Navigation.html"-->
        <!-- END SIDEBAR -->
        <!-- BEGIN CONTENT -->
        <div class="page-content-wrapper">
            <div class="page-content">
                <!-- BEGIN PAGE HEADER-->
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                        <h3 class="page-title">访客身份管理 <small>管理所有访客身份</small>
                        </h3>
                        <ul class="page-breadcrumb breadcrumb">
                            <li class="btn-group">
                                <button type="button" class="btn blue dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true">
                                    <span>Actions
                                    </span>
                                    <i class="fa fa-angle-down"></i>
                                </button>
                                <ul class="dropdown-menu pull-right" role="menu">
                                    <li>
                                        <a href="/Authentication/Guests.aspx">访客身份列表
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/Authentication/GuestOperate.aspx?type=add">添加访客身份
                                        </a>
                                    </li>
                                    <li class="divider"></li>
                                    <li>
                                        <a href="javascript:history.go(-1)">返回上一个页面
                                        </a>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <i class="fa fa-home"></i>
                                <a href="/">Home
                                </a>
                                <i class="fa fa-angle-right"></i>
                            </li>
                            <li>
                                <a href="#">基础设置
                                </a>
                                <i class="fa fa-angle-right"></i>
                            </li>
                            <li>
                                <a href="/Authentication/Guests.aspx">访客身份管理
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
                                    <i class="fa fa-reorder"></i><%= g_strTitle %>
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
                                                访客身份名称<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="Name" value="<%= g_strName %>" type="text" class="form-control" placeholder="输入访客身份名称">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                Key<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <input id="txtKey" value="<%= g_strKey %>" type="text" class="form-control input-large input-inline " placeholder="输入Key" disabled>
                                                <span class="help-inline"><a id="createKey" href="javascript:createKey();">生成Key</a>
                                                </span>
                                                <input name="Key" value="<%= g_strKey %>" type="hidden" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                Code<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="Code" value="<%= g_strCode %>" type="text" class="form-control input-large" placeholder="输入Code">
                                                </div>
                                                <span class="help-block">确保此值唯一性.
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">描述</label>
                                            <div class="col-md-4">
                                                <textarea name="Description" class="form-control" rows="3"><%= g_strDescription %></textarea>
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
                    Name: {
                        required: true
                    },
                    Key: {
                        required: true
                    },
                    Code: {
                        required: true
                    }
                },

                messages: {
                    Name: {
                        required: '请输入访客身份名称'
                    },
                    Key: {
                        required: '请输入Key'
                    },
                    Code: {
                        required: '请输入Code'
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

        //BEGIN CreateKey
        function createKey() {
            $.ajax({
                url: "/Authentication/GuestOperate.aspx",
                type: "POST",
                data: { type: "createKey", AjaxRequest: "true" },
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.Succeeded) {
                        $('#txtKey').val(data.Result);
                        $('input[name="Key"]').val(data.Result);
                    }
                    else {
                        ShowMsg(data.MsgType, data.Message);
                    }
                }
            })
        }
        //END CreateKey
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //表单验证
            handleValidation();

            //Active Menu
            ActiveMenu('ADMIN_CENTER_AUTHENTICATION,ADMIN_CENTER_AUTHENTICATION_GUEST_LIST')
        })
    </script>
</body>
<!-- END BODY -->
</html>

