<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPermissionOperate.aspx.cs" Inherits="com.admincenter.www.SystemSetup.AdminPermissionOperate" %>

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
                                            <label class="control-label col-md-3">
                                                所属系统<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-3">
                                                <select class="form-control" id="SystemId" name="SystemId">
                                                    <option value="" data-code="">选择系统</option>
                                                    <% foreach (var item in g_AdminSystems)
                                                       {
                                                    %>
                                                    <option value="<%= item.AdminSystemId %>" data-code="<%= item.Code %>"><%= item.Name %></option>
                                                    <%
                                                       } %>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-3">父级权限</label>
                                            <div class="col-md-4">
                                                <input id="txtParentPermission" type="text" class="form-control input-inline input-medium" placeholder="顶级权限" readonly="readonly" onclick="showMenu()">
                                                <input type="hidden" name="ParentId" value="<%= g_strParentId %>" />
                                                <span class="help-block">顶级权限：表示当前权限没有任何父级权限
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                权限代码<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="PermissionCode" value="<%= g_strPermissionCode %>" type="text" class="form-control" placeholder="输入权限代码">
                                                </div>
                                                <span class="help-block">确保此值唯一性.
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                权限名称<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="Name" value="<%= g_strName %>" type="text" class="form-control" placeholder="输入权限名称">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                导航图片样式
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="Img" value="<%= g_strImg %>" type="text" class="form-control" placeholder="输入导航图片样式">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">是否为导航菜单</label>
                                            <div class="col-md-9">
                                                <div class="checkbox-list">
                                                    <label class="checkbox-inline">
                                                        <input id="IsMenu" type="checkbox" name="IsMenu">
                                                        选中时"是导航菜单"
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">是否为链结</label>
                                            <div class="col-md-9">
                                                <div class="checkbox-list">
                                                    <label class="checkbox-inline">
                                                        <input id="IsLink" type="checkbox" name="IsLink">
                                                        选中时"是链结"
                                                    </label>
                                                    <span class="help-block">此项只有在选中导航菜单时有效
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">
                                                链结URL
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input id="LinkUrl" name="LinkUrl" value="<%= g_strUrl %>" type="text" class="form-control" placeholder="输入链结URL">
                                                    <span class="help-block">此项只有在选中链结时有效
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-3">
                                                链结打开方式
                                            </label>
                                            <div class="col-md-2">
                                                <select class="form-control" id="Target" name="Target">
                                                    <option value="_self">_self</option>
                                                    <option value="_blank">_blank</option>
                                                    <option value="_parent">_parent</option>
                                                    <option value="_top">_top</option>
                                                </select>
                                                <span class="help-block">此项只有在选中链结时有效
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
    <div id="permissionTreePanel" style="display: none; position: absolute;">
        <ul id="tree1" class="ztree" style="margin-top: 0; width: 240px; border: 1px solid #e5e5e5; background-color: white; overflow-y: auto; overflow-x: auto;"></ul>
    </div>
    <script type="text/javascript">

        //BEGIN Validation
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
                    SystemId: {
                        required: true
                    },
                    PermissionCode: {
                        required: true
                    },
                    Name: {
                        required: true
                    }
                },

                messages: {
                    SystemId: {
                        required: '请选择系统'
                    },
                    PermissionCode: {
                        required: '请输入权限代码'
                    },
                    Name: {
                        required: '请输入权限名称'
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

        //BEGIN SelectParentPermission
        function showMenu() {
            var txtParentPermission = $("#txtParentPermission");
            var offset = txtParentPermission.offset();
            var outerHeight = txtParentPermission.outerHeight();
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
            if (!(event.target.id == "txtParentPermission" || event.target.id == "permissionTreePanel" || $(event.target).parents("#permissionTreePanel").length > 0)) {
                hideMenu();
            }
        }

        $(window).resize(function () {
            if ($("#permissionTreePanel").is(':visible')) {
                showMenu();
            }
        });
        //END SelectParentPermission

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
            $("#txtParentPermission").val(treeNode.name);
            $('input[name="ParentId"]').val(treeNode.id);
            if (treeNode.code != '') {
                $('input[name="PermissionCode"]').val(treeNode.code + '_');
            }
            else {
                var code = $('select[name="SystemId"]').find("option:selected").attr('data-code');
                code = code == '' ? '' : (code + '_');
                $('input[name="PermissionCode"]').val(code);
            }

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

        function GetPermissionBySystem(id, b) {
            $.ajax({
                url: "/SystemSetup/AdminPermissionOperate.aspx",
                type: "POST",
                data: { id: id, type: "getPermissionBySystem", AjaxRequest: "true" },
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.Succeeded) {
                        var zNodes = eval('(' + data.Result + ')');
                        var zTreeObj = $.fn.zTree.init($("#tree1"), setting, zNodes);

                        var nodes = zTreeObj.getNodes();
                        if (nodes.length > 0) {
                            var addNode = { id: "-1", name: "顶级权限", code: "" };
                            zTreeObj.addNodes(null, addNode);

                            nodes = zTreeObj.getNodes();
                            zTreeObj.moveNode(nodes[0], nodes[nodes.length - 1], "prev");
                        }

                        if (b) {
                            var parentId = '<%= g_strParentId %>';
                            var node = zTreeObj.getNodeByParam("id", parentId, null);
                            if (node != null) {
                                $("#txtParentPermission").val(node.name);
                                zTreeObj.selectNode(node);
                            }
                            else {
                                $("#txtParentPermission").val("顶级权限");
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

        $(document).ready(function () {
            handleValidation();

            //BEGIN System_Change
            $('select[name="SystemId"]').change(function () {
                var id = $(this).val();
                if (id != '') {
                    GetPermissionBySystem(id, false);
                }
                else {
                    $('#tree1').html('');
                }

                $('#txtParentPermission').val('顶级权限');

                var code = $(this).find("option:selected").attr('data-code');
                code = code == '' ? '' : (code + '_');
                $('input[name="PermissionCode"]').val(code);
            })
            //BEGIN System_Change

            //BEGIN IsLink_Select
            $('#IsLink').click(function () {
                if ($('#IsLink').is(":checked")) {
                    $('#LinkUrl').rules('add', { required: true });//, url: true
                }
                else {
                    $('#LinkUrl').rules('add', { required: false });
                }
            })
            //END IsLink_Select

            //BEGIN JS_InitPageDate
            $('#SystemId').val('<%= g_strSystemId %>');

            var isMenu = '<%= g_strIsMenu %>';
            if (isMenu != null && isMenu != '') {
                $('#IsMenu').click();
            }

            var isLink = '<%= g_strIsLink %>';
            if (isLink != null && isLink != '') {
                $('#IsLink').click();
            }

            $('#Target').val('<%= g_strTarget %>');

            <% if (!string.IsNullOrEmpty(g_strSystemId))
               { 
                %>
            GetPermissionBySystem('<%= g_strSystemId %>', true);
            <%
               } %>

            //END JS_InitPageDate

            //Active Menu
            ActiveMenu('ADMIN_CENTER_SETUP,ADMIN_CENTER_SETUP_PERMISSION_LIST')
        })
    </script>
</body>
<!-- END BODY -->
</html>
