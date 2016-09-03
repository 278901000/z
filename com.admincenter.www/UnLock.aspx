<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnLock.aspx.cs" Inherits="com.admincenter.www.UnLock" %>

<%@ Import Namespace="z.AdminCenter.Logic" %>

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
<body>
    <div class="page-lock">
        <div class="page-logo">
            <a class="brand" href="index.html">
                <img src="/Resource/img/logo.png" alt="logo" />
            </a>
        </div>
        <div class="page-body">
            <% string strUserLogo = "/Resource/img/user-logo.gif";
               if (!string.IsNullOrEmpty(g_AdminUserExt.Logo))
               {
                   strUserLogo = z.Foundation.UploadFile.GetThumbnail(g_AdminUserExt.Logo, "s2");
               } 
            %>
            <img class="page-lock-img" src="<%= strUserLogo %>" alt="">
            <div class="page-lock-info">
                <h1><%= g_AdminUserExt.AdminName %></h1>
                <span class="email"><%= g_AdminUserExt.RealName %>&nbsp;&nbsp;<span style="color: orange"><%= g_strMsg %></span>
                </span>
                <span class="locked">Locked
                </span>
                <form class="form-inline" action="" method="post">
                    <div class="input-group input-medium">
                        <input type="password" name="password" class="form-control" placeholder="Password">
                        <span class="input-group-btn">
                            <button type="submit" class="btn blue icn-only"><i class="m-icon-swapright m-icon-white"></i></button>
                        </span>
                    </div>

                    <!-- /input-group -->
                    <div class="relogin">
                        <a href="/Login.aspx">Not <%= g_AdminUserExt.AdminName %> ?
                        </a>
                    </div>
                </form>
            </div>
        </div>
        <div class="page-footer">
            2015 &copy; AdminCenter.
        </div>
    </div>
    <!--#include File="/AdminTemplate/Footer.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
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
