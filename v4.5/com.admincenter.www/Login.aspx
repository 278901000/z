<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="com.admincenter.www.Login" %>

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
<!-- BEGIN BODY -->
<body class="login">
    <!-- BEGIN LOGO -->
    <div class="logo">
        <a href="/">
            <img src="/Resource/img/logo.png" alt="" />
        </a>
    </div>
    <!-- END LOGO -->
    <!-- BEGIN LOGIN -->
    <div class="content">
        <!-- BEGIN LOGIN FORM -->
        <form class="login-form" action="" method="post">
            <h3 class="form-title">登录</h3>
            <div class="alert alert-danger <%= string.IsNullOrEmpty(g_strMsg) ? "display-hide" : "" %>">
                <button class="close" data-close="alert"></button>
                <span><%= string.IsNullOrEmpty(g_strMsg) ? "请输入用户名、密码." : g_strMsg %>
                </span>
            </div>
            <div class="form-group">
                <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                <label class="control-label visible-ie8 visible-ie9">用户名</label>
                <div class="input-icon">
                    <i class="fa fa-user"></i>
                    <input class="form-control placeholder-no-fix" type="text" autocomplete="off" placeholder="用户名" name="username" />
                </div>
            </div>
            <div class="form-group">
                <label class="control-label visible-ie8 visible-ie9">密码</label>
                <div class="input-icon">
                    <i class="fa fa-lock"></i>
                    <input class="form-control placeholder-no-fix" type="password" autocomplete="off" placeholder="密码" name="password" />
                </div>
            </div>
            <div class="form-actions">
                <label class="checkbox">
                    <input type="checkbox" name="remember" />
                    Remember me
                </label>
                <button type="submit" class="btn blue pull-right">
                    Login <i class="m-icon-swapright m-icon-white"></i>
                </button>
            </div>
        </form>
        <!-- END LOGIN FORM -->
    </div>
    <!-- END LOGIN -->
    <!-- BEGIN COPYRIGHT -->
    <div class="copyright">
        2015 &copy; AdminCenter.
    </div>
    <!-- END COPYRIGHT -->
    <!--#include File="/AdminTemplate/Footer.html"-->
    <script type="text/javascript">
        //Begin Validation
        var handleValidation = function () {

            $('.login-form').validate({
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    username: {
                        required: true
                    },
                    password: {
                        required: true
                    },
                    remember: {
                        required: false
                    }
                },

                messages: {
                    username: {
                        required: "用户名不能为空"
                    },
                    password: {
                        required: "密码不能为空"
                    }
                },

                invalidHandler: function (event, validator) { //display error alert on form submit   
                    $('.alert-danger', $('.login-form')).show();
                },

                highlight: function (element) { // hightlight error inputs
                    $(element)
	                    .closest('.form-group').addClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    label.closest('.form-group').removeClass('has-error');
                    label.remove();
                },

                errorPlacement: function (error, element) {
                    error.insertAfter(element.closest('.input-icon'));
                },

                submitHandler: function (form) {
                    form.submit(); // form validation success, call ajax form submit
                }
            });

            $('.login-form input').keypress(function (e) {
                if (e.which == 13) {
                    if ($('.login-form').validate().form()) {
                        $('.login-form').submit(); //form validation success, call ajax form submit
                    }
                    return false;
                }
            });
        }
        //END Validation

        $(document).ready(function () {
            handleValidation();

            $.backstretch([
		        "/Resource/img/bg/1.jpg",
		        "/Resource/img/bg/2.jpg",
		        "/Resource/img/bg/3.jpg",
		        "/Resource/img/bg/4.jpg"
            ], {
                fade: 1000,
                duration: 8000
            });
        })
    </script>
</body>
<!-- END BODY -->
</html>
