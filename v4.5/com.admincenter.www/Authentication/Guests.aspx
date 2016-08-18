<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Guests.aspx.cs" Inherits="com.admincenter.www.Authentication.Guests" %>

<%@ Import Namespace="Entity.AdminCenterDB" %>

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
                        <h3 class="page-title">访客身份管理 <small>管理所有访客身份、设置访客身份权限等</small>
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
                                <a href="#">身份认证
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
                        <!-- BEGIN EXAMPLE TABLE PORTLET-->
                        <div class="portlet box purple">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-table"></i>访客身份列表
                                </div>
                                <div class="actions">
                                    <a href="/Authentication/GuestOperate.aspx?type=add" class="btn blue">
                                        <i class="fa fa-pencil"></i>Add
                                    </a>
                                    <div class="btn-group">
                                        <a class="btn green" href="#" data-toggle="dropdown">
                                            <i class="fa fa-cogs"></i>Tools <i class="fa fa-angle-down"></i>
                                        </a>
                                        <ul class="dropdown-menu pull-right">
                                            <li>
                                                <a id="btnEnableAll" href="#">
                                                    <i class="fa fa-circle-o"></i>Enable
                                                </a>
                                            </li>
                                            <li>
                                                <a id="btnDisableAll" href="#">
                                                    <i class="fa fa-ban"></i>Disable
                                                </a>
                                            </li>
                                            <li>
                                                <a id="btnDeleteAll" href="#">
                                                    <i class="fa fa-trash-o"></i>Delete
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="row">
                                    <div class="col-md-2 col-sm-12">
                                        <div id="sample_2_length" class="dataTables_length">
                                            <label>
                                                <select id="selRecords" class="form-control input-xsmall">
                                                    <option>10</option>
                                                    <option>30</option>
                                                    <option>50</option>
                                                </select>
                                                records</label>
                                        </div>
                                    </div>
                                    <form id="searchForm" method="get" action="">
                                        <div class="col-md-10 col-sm-12">
                                            <div class="dataTables_filter" id="sample_2_filter">
                                                <label>
                                                    Search:
                                                <input id="txtSearch" name="key" value="<%= Request.Params["key"] %>" type="text" aria-controls="sample_2" class="form-control input-inline"></label>
                                                <input type="hidden" name="pagesize" value="<%= Request.Params["pagesize"] %>" />
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <table class="table table-striped table-bordered table-hover" id="sample_2">
                                    <thead>
                                        <tr>
                                            <th style="width: 8px;">
                                                <input type="checkbox" class="group-checkable" data-set="#sample_2 .checkboxes" />
                                            </th>
                                            <th>访客名称
                                            </th>
                                            <th>Code
                                            </th>
                                            <th>Key
                                            </th>
                                            <th>是否禁用
                                            </th>
                                            <th>操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% foreach (var item in g_AuthGuests.Rows)
                                           {
                                        %>
                                        <tr class="odd gradeX">
                                            <td>
                                                <input name="chkPrimaryKey" type="checkbox" class="checkboxes" value="<%= item.AuthGuestId %>" />
                                            </td>
                                            <td><%= item.Name %></td>
                                            <td><%= item.Code %></td>
                                            <td><%= item.AuthKey %></td>
                                            <td>
                                                <input name="chkDisabled" type="checkbox" class="make-switch" <%= item.Disabled ? "checked" : "" %> data-text-label="禁用" data-on="primary" data-off="info">
                                            </td>
                                            <td>
                                                <a href="/Authentication/GuestOperate.aspx?type=update&id=<%= item.AuthGuestId %><%= string.IsNullOrEmpty(Request.QueryString.ToString()) ? "" : ("&custom=" + Server.UrlEncode(Request.QueryString.ToString())) %>" class="btn default btn-xs purple">
                                                    <i class="fa fa-edit"></i>Edit
                                                </a>
                                                <a name="btnDelete" href="#" class="btn default btn-xs black">
                                                    <i class="fa fa-trash-o"></i>Delete
                                                </a>
                                            </td>
                                        </tr>
                                        <% } %>
                                    </tbody>
                                </table>
                                <% if (g_AuthGuests.TotalPages > 1)
                                   {
                                       int intStart = (g_AuthGuests.PageIndex - 1) * g_AuthGuests.PageSize + 1;
                                       int intEnd = g_AuthGuests.PageIndex * g_AuthGuests.PageSize;
                                       if (intEnd > g_AuthGuests.TotalCount)
                                       {
                                           intEnd = g_AuthGuests.TotalCount;
                                       }
                                %>
                                <div class="row">
                                    <div class="col-md-5 col-sm-12">
                                        <div class="dataTables_info" id="sample_2_info">Showing <%= intStart %> to <%= intEnd %> of <%= g_AuthGuests.TotalCount %> entries</div>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <div class="dataTables_paginate paging_bootstrap">
                                            <%= GetPager<auth_guest>(g_AuthGuests) %>
                                        </div>
                                    </div>
                                </div>
                                <% } %>
                            </div>
                        </div>
                        <!-- END EXAMPLE TABLE PORTLET-->
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
        $(document).ready(function () {
            //Begin 操作页面跳转至列表页面附带的消息显示
            <%= Session["msg"] != null ? "ShowMsg('success', '" + Session["msg"] + "');" : "" %>
            <% Session["msg"] = null; %>
            //End 操作页面跳转至列表页面附带的消息显示

            //Begin Table处理
            var table = $('#sample_2').dataTable({
                "bFilter": false,
                "bInfo": false,
                "bLengthChange": false,
                "bPaginate": false,
                "bSort": false,
                //"aoColumns": [
                //  { "bSortable": false },
                //  null,
                //  { "bSortable": false },
                //  null,
                //  { "bSortable": false },
                //  { "bSortable": false }
                //]
            });

            //阻止原有sort事件发生
            $('#sample_2 th').unbind('click.DT');
            table.fnSortListener($('.sorting'), 0);

            //绑定新的事件
            $('#sample_2 th').click(function (e) {

            });
            //End Table处理

            //Begin #selRecords
            $('#selRecords').change(function () {
                var pageSize = $(this).val();
                var queryString = location.search.replace('?', '');

                queryString = queryString.replace(/&?pagesize=(\d+)?/, '');
                queryString = queryString.replace(/^&+/, '');
                if (queryString != '') {
                    queryString += '&';
                }
                queryString += 'pagesize=' + pageSize;

                var url = 'http://' + location.host + location.pathname + '?' + queryString + location.hash;

                location.href = url;
            })
            $('#selRecords').val('<%= string.IsNullOrEmpty(Request.Params["pagesize"]) ? "10" : Request.Params["pagesize"] %>')
            //End #selRecords

            //Begin search
            $('#txtSearch').keydown(function (event) {
                if (event.keyCode == 13) {
                    $('#searchForm').submit();
                }
            })
            //End search

            //Begin checkbox
            $('.group-checkable').change(function () {
                var set = $(this).attr("data-set");
                var checked = $(this).is(":checked");
                $(set).each(function () {
                    if (checked) {
                        $(this).attr("checked", true);
                    } else {
                        $(this).attr("checked", false);
                    }
                });
                $.uniform.update(set);
            });
            //End checkbox

            //Begin Disable
            $('input[name="chkDisabled"]').on('switch-change', function (e, data) {
                var chkThis = $(data.el);
                var tr = chkThis.parents('tr');
                var disableId = tr.find(":checkbox").val();
                var disable = data.value;
                var disableText = disable ? "禁用" : "启用";
                bootbox.confirm("你确定要" + disableText + "当前项吗?", function (result) {
                    if (result == true) {
                        $.ajax({
                            url: "/Authentication/Guests.aspx",
                            type: "POST",
                            data: { id: disableId, disable: disable, type: "disabled", AjaxRequest: "true" },
                            success: function (data) {
                                data = eval('(' + data + ')');
                                if (!data.Succeeded) {
                                    chkThis.bootstrapSwitch('setState', !disable, true);
                                }
                                ShowMsg(data.MsgType, data.Message);
                            }
                        })
                    }
                    else {
                        chkThis.bootstrapSwitch('setState', !disable, true);
                    }
                });
            })

            $('#btnDisableAll').click(function () {
                funDisabled(true);
            });

            $('#btnEnableAll').click(function () {
                funDisabled(false);
            });

            function funDisabled(bDisable) {
                var checkedItems = $('input[name="chkPrimaryKey"]:checked');
                if (checkedItems.length <= 0) {
                    ShowMsg('info', '未选中任何项');
                }
                else {
                    var idArray = new Array();
                    $(checkedItems).each(function () {
                        idArray.push($(this).val());
                    })
                    var disableId = idArray.join(',');
                    var disableText = bDisable ? "禁用" : "启用";

                    bootbox.confirm("你确定要" + disableText + "当前选择的项吗?", function (result) {
                        if (result == true) {
                            $.ajax({
                                url: "/Authentication/Guests.aspx",
                                type: "POST",
                                data: { id: disableId, disable: bDisable, type: "disabled", AjaxRequest: "true" },
                                success: function (data) {
                                    data = eval('(' + data + ')');
                                    if (data.Succeeded) {
                                        $(checkedItems).each(function () {
                                            var chkDisabled = $(this).parents('tr').find('input[name="chkDisabled"]');
                                            if (bDisable) {
                                                chkDisabled.bootstrapSwitch('setState', true, true);
                                            }
                                            else {
                                                chkDisabled.bootstrapSwitch('setState', false, true);
                                            }
                                        })
                                    }
                                    ShowMsg(data.MsgType, data.Message);
                                }
                            })
                        }
                    });
                }
            }
            //End Disable

            //Begin Delete
            $('a[name="btnDelete"]').click(function () {
                var tr = $(this).parents('tr');
                var deleteId = tr.find('input[name="chkPrimaryKey"]').val();
                bootbox.confirm("你确定要删除当前项吗?", function (result) {
                    if (result == true) {
                        $.ajax({
                            url: "/Authentication/Guests.aspx",
                            type: "POST",
                            data: { id: deleteId, type: "deleted", AjaxRequest: "true" },
                            success: function (data) {
                                data = eval('(' + data + ')');
                                if (data.Succeeded) {
                                    tr.remove();
                                }
                                ShowMsg(data.MsgType, data.Message);
                            }
                        })
                    }
                });
            });

            $('#btnDeleteAll').click(function () {
                var checkedItems = $('input[name="chkPrimaryKey"]:checked');
                if (checkedItems.length <= 0) {
                    ShowMsg('info', '未选中任何项');
                }
                else {
                    var idArray = new Array();
                    $(checkedItems).each(function () {
                        idArray.push($(this).val());
                    })
                    var deleteId = idArray.join(',');

                    bootbox.confirm("你确定要删除当前选择的项吗?", function (result) {
                        if (result == true) {
                            $.ajax({
                                url: "/Authentication/Guests.aspx",
                                type: "POST",
                                data: { id: deleteId, type: "deleted", AjaxRequest: "true" },
                                success: function (data) {
                                    data = eval('(' + data + ')');
                                    if (data.Succeeded) {
                                        $(checkedItems).each(function () {
                                            $(this).parents('tr').remove();
                                        })
                                    }
                                    ShowMsg(data.MsgType, data.Message);
                                }
                            })
                        }
                    });
                }
            });
            //End Delete

            //Active Menu
            ActiveMenu('ADMIN_CENTER_AUTHENTICATION,ADMIN_CENTER_AUTHENTICATION_GUEST_LIST')
        })
    </script>
</body>
<!-- END BODY -->
</html>
