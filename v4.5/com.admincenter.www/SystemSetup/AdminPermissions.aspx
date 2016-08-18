<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPermissions.aspx.cs" Inherits="com.admincenter.www.SystemSetup.AdminPermissions" %>

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
                        <h3 class="page-title">系统权限管理 <small>管理、设置系统权限</small>
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
                                        <a href="/SystemSetup/AdminPermissions.aspx">系统权限列表
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/SystemSetup/AdminPermissionOperate.aspx?type=add">添加系统权限
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/SystemSetup/AdminPermissionSort.aspx">系统权限排序
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
                                <a href="/SystemSetup/AdminPermissions.aspx">系统权限管理
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
                                    <i class="fa fa-table"></i>系统权限列表
                                </div>
                                <div class="actions">
                                    <a href="/SystemSetup/AdminPermissionOperate.aspx?type=add" class="btn blue">
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
                                                <div style="float: right">
                                                    <select id="SystemId" name="SystemId" class="form-control input-inline">
                                                        <option value="">选择系统</option>
                                                        <% foreach (var obj in g_AdminSystems)
                                                           {
                                                               string strSelect = obj.AdminSystemId.ToString() == g_strSystemId ? "selected=\"selected\"" : "";
                                                        %>
                                                        <option value="<%= obj.AdminSystemId %>" <%= strSelect %>><%= obj.Name %></option>
                                                        <%
                                                           } %>
                                                    </select>
                                                    <input id="PermissionName" type="text" class="form-control input-inline input-medium" placeholder="选择权限" readonly="readonly" onclick="showMenu()">
                                                    <input type="hidden" id="PermissionId" name="PermissionId" value="<%= g_strPermissionId %>" />
                                                    <input id="txtSearch" name="key" value="<%= g_strKeyword %>" type="text" aria-controls="sample_2" class="form-control input-inline">
                                                    <button class="btn yellow" type="submit"><i class="fa fa-search"></i>搜索</button>
                                                    <input type="hidden" name="pagesize" value="<%= Request.Params["pagesize"] %>" />
                                                </div>

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
                                            <th>权限名称
                                            </th>
                                            <th>权限代码
                                            </th>
                                            <th>Is Menu
                                            </th>
                                            <th>Is Link
                                            </th>
                                            <th>是否禁用
                                            </th>
                                            <th>操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% foreach (var item in g_AdminPermissions.Rows)
                                           {
                                        %>
                                        <tr class="odd gradeX">
                                            <td>
                                                <input name="chkPrimaryKey" type="checkbox" class="checkboxes" value="<%= item.AdminPermissionId %>" />
                                            </td>
                                            <td><%= item.Name %></td>
                                            <td><%= item.PermissionCode %></td>
                                            <td><%= item.IsMenu ? "√" : "x" %></td>
                                            <td><%= item.IsLink ? "√" : "x" %></td>
                                            <td>
                                                <input name="chkDisabled" type="checkbox" class="make-switch" <%= item.Disabled ? "checked" : "" %> data-text-label="禁用" data-on="primary" data-off="info">
                                            </td>
                                            <td>
                                                <a href="/SystemSetup/AdminPermissionOperate.aspx?type=update&id=<%= item.AdminPermissionId %><%= string.IsNullOrEmpty(Request.QueryString.ToString()) ? "" : ("&custom=" + Server.UrlEncode(Request.QueryString.ToString())) %>" class="btn default btn-xs purple">
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
                                <% if (g_AdminPermissions.TotalPages > 1)
                                   {
                                       int intStart = (g_AdminPermissions.PageIndex - 1) * g_AdminPermissions.PageSize + 1;
                                       int intEnd = g_AdminPermissions.PageIndex * g_AdminPermissions.PageSize;
                                       if (intEnd > g_AdminPermissions.TotalCount)
                                       {
                                           intEnd = g_AdminPermissions.TotalCount;
                                       }
                                %>
                                <div class="row">
                                    <div class="col-md-5 col-sm-12">
                                        <div class="dataTables_info" id="sample_2_info">Showing <%= intStart %> to <%= intEnd %> of <%= g_AdminPermissions.TotalCount %> entries</div>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <div class="dataTables_paginate paging_bootstrap">
                                            <%= GetPager<admin_permission>(g_AdminPermissions) %>
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
    <div id="permissionTreePanel" style="display: none; position: absolute;">
        <ul id="tree1" class="ztree" style="margin-top: 0; width: 240px; border: 1px solid #e5e5e5; background-color: white; overflow-y: auto; overflow-x: auto;"></ul>
    </div>
    <script type="text/javascript">
        //BEGIN ZTREE
        var setting = {
            view: {
                selectedMulti: false,
                showIcon: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                beforeExpand: beforeExpand,
                onExpand: onExpand,
                onClick: onClick
            }
        };

        function onClick(e, treeId, treeNode) {
            $("#PermissionName").val(treeNode.name);
            $('#PermissionId').val(treeNode.id);
           
            hideMenu();
        }

        var curExpandNode = null;
        function beforeExpand(treeId, treeNode) {
            var pNode = curExpandNode ? curExpandNode.getParentNode() : null;
            var treeNodeP = treeNode.parentTId ? treeNode.getParentNode() : null;
            var zTree = $.fn.zTree.getZTreeObj(treeId);
            for (var i = 0, l = !treeNodeP ? 0 : treeNodeP.children.length; i < l; i++) {
                if (treeNode !== treeNodeP.children[i]) {
                    zTree.expandNode(treeNodeP.children[i], false);
                }
            }
            while (pNode) {
                if (pNode === treeNode) {
                    break;
                }
                pNode = pNode.getParentNode();
            }
            if (!pNode) {
                singlePath(treeId, treeNode);
            }
        }

        function singlePath(treeId, newNode) {
            if (newNode === curExpandNode) return;
            if (curExpandNode && curExpandNode.open == true) {
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                if (newNode.parentTId === curExpandNode.parentTId) {
                    zTree.expandNode(curExpandNode, false);
                } else {
                    var newParents = [];
                    while (newNode) {
                        newNode = newNode.getParentNode();
                        if (newNode === curExpandNode) {
                            newParents = null;
                            break;
                        } else if (newNode) {
                            newParents.push(newNode);
                        }
                    }
                    if (newParents != null) {
                        var oldNode = curExpandNode;
                        var oldParents = [];
                        while (oldNode) {
                            oldNode = oldNode.getParentNode();
                            if (oldNode) {
                                oldParents.push(oldNode);
                            }
                        }
                        if (newParents.length > 0) {
                            zTree.expandNode(oldParents[Math.abs(oldParents.length - newParents.length) - 1], false);
                        } else {
                            zTree.expandNode(oldParents[oldParents.length - 1], false);
                        }
                    }
                }
            }
        }

        function onExpand(event, treeId, treeNode) {
            curExpandNode = treeNode;
        }
        //END ZTREE
    </script>
    <script type="text/javascript">
        //BEGIN 根据系统ID获取对应的权限
        function GetPermissionBySystem(systemId, bInit) {
            $.ajax({
                url: "/SystemSetup/AdminPermissionOperate.aspx",
                type: "POST",
                data: { id: systemId, type: "getPermissionBySystem", AjaxRequest: "true" },
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.Succeeded) {
                        var zNodes = eval('(' + data.Result + ')');
                        var zTreeObj = $.fn.zTree.init($("#tree1"), setting, zNodes);

                        var nodes = zTreeObj.getNodes();
                        if (nodes.length > 0) {
                            var addNode = { id: "-1", name: "清除条件", code: "" };
                            zTreeObj.addNodes(null, addNode);

                            nodes = zTreeObj.getNodes();
                            zTreeObj.moveNode(nodes[0], nodes[nodes.length - 1], "prev");
                        }

                        if(bInit)
                        {
                            var permissionId = '<%= g_strPermissionId %>';
                            var node = zTreeObj.getNodeByParam("id", permissionId, null);

                            if (node != null) {
                                $("#PermissionName").val(node.name);
                                zTreeObj.selectNode(node);
                                curExpandNode = node.getParentNode();
                            }
                        }
                    }
                    else {
                        $('#tree1').html('');
                        ShowMsg(data.MsgType, data.Message);
                    }
                }
            })
        }
        //END 根据系统ID获取对应的权限

        //BEGIN 控制权限树层
        function showMenu() {
            var permissionName = $("#PermissionName");
            var offset = permissionName.offset();
            var outerHeight = permissionName.outerHeight();
            if ($("#permissionTreePanel").is(':visible')) {
                $("#permissionTreePanel").css({ left: offset.left + "px", top: offset.top + outerHeight + "px" });
            }
            else {
                $("#permissionTreePanel").css({ left: offset.left + "px", top: offset.top + outerHeight + "px" }).slideDown("fast");
            }
            $("body").bind("mousedown", onBodyDown);
        }

        function hideMenu() {
            $("#permissionTreePanel").fadeOut("fast");
            $("body").unbind("mousedown", onBodyDown);
        }

        function onBodyDown(event) {
            if (!(event.target.id == "PermissionName" || event.target.id == "permissionTreePanel" || $(event.target).parents("#permissionTreePanel").length > 0)) {
                hideMenu();
            }
        }

        $(window).resize(function () {
            if ($("#permissionTreePanel").is(':visible')) {
                showMenu();
            }
        });
        //END 控制权限树层
    </script>
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

            //BEGIN System_Change
            $('select[name="SystemId"]').change(function () {
                var id = $(this).val();
                if (id != '') {
                    GetPermissionBySystem(id, false);
                }
                else {
                    $('#tree1').html('');
                }

                $('#PermissionName').val('');
                $('#PermissionId').val('');
            })
            //BEGIN System_Change

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
                            url: "/SystemSetup/AdminPermissions.aspx",
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
            });

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
                                url: "/SystemSetup/AdminPermissions.aspx",
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
                            url: "/SystemSetup/AdminPermissions.aspx",
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
                                url: "/SystemSetup/AdminPermissions.aspx",
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

              <% if (!string.IsNullOrEmpty(g_strSystemId))
               { 
                %>
            GetPermissionBySystem('<%= g_strSystemId %>', true);
            <%
               } %>

            //Active Menu
            ActiveMenu('ADMIN_CENTER_SETUP,ADMIN_CENTER_SETUP_PERMISSION_LIST')
        })
    </script>
</body>
<!-- END BODY -->
</html>

