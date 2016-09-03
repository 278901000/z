<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupOperate.aspx.cs" Inherits="com.admincenter.www.SystemSetup.AdminGroupOperate" %>

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
                        <h3 class="page-title">管理员组管理 <small>管理所有管理员组、设置管理员组权限等</small>
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
                                        <a href="/SystemSetup/AdminGroups.aspx">管理员组列表
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/SystemSetup/AdminGroupOperate.aspx?type=add">添加管理员组
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
                                <a href="/SystemSetup/AdminGroups.aspx">管理员组管理
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
                                                管理员组名称<span class="required">*
                                                </span>
                                            </label>
                                            <div class="col-md-4">
                                                <div class="input-icon right">
                                                    <i class="fa"></i>
                                                    <input name="Name" value="<%= g_strName %>" type="text" class="form-control" placeholder="输入组名称">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-3 control-label">描述</label>
                                            <div class="col-md-4">
                                                <textarea name="Description" class="form-control" rows="3"><%= g_strDescription %></textarea>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-3">
                                                管理员组类型
                                            </label>
                                            <div class="col-md-3">
                                                <select class="form-control input-medium" id="GroupType" name="GroupType">
                                                    <option value="1">普通管理员</option>
                                                    <option value="0">超级管理员</option>
                                                </select>
                                                <span class="help-block">超级管理员无需勾选具体的权限
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-3">
                                                选择系统权限
                                            </label>
                                            <div class="col-md-3">
                                                <ul id="tree1" class="ztree" style="margin-top: 0; height: 200px; border: 1px solid #e5e5e5; background-color: white; overflow-y: auto; overflow-x: auto;"></ul>
                                                <input type="hidden" id="PermissionIds" name="PermissionIds" value="<%= g_strPermissionIds %>" />
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
        //BEGIN ZTree
        var setting = {
            view: {
                selectedMulti: false,
                showIcon: false
            },
            check: {
                enable: true,
                chkboxType: { "Y": "ps", "N": "s" }
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                beforeExpand: beforeExpand,
                onExpand: onExpand,
                onCheck: onCheck
            }
        };

        var zNodes = <%= g_strJson %>

        function onCheck(e, treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("tree1");
            var nodes = zTree.getCheckedNodes(true);

            var ids = '';
            for (var i = 0; i < nodes.length; i++) {
                ids += nodes[i].id + ",";
            }

            $('#PermissionIds').val(ids);
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
        //END ZTree
    </script>
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
                    }
                },

                messages: {
                    Name: {
                        required: '请输入组名称'
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
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //表单验证
            handleValidation();

            //权限树
            var treeObj = $.fn.zTree.init($("#tree1"), setting, zNodes);
            var strPermissionIds = '<%= g_strPermissionIds %>';
            if (strPermissionIds != '') {
                var permissionIds = strPermissionIds.split(",");
                for (var i = 0; i < permissionIds.length; i++) {
                    var node = treeObj.getNodeByParam("id", permissionIds[i], null);
                    if (node != null) {
                        treeObj.checkNode(node, true, false);
                        treeObj.expandNode(node, true, false, false);
                    }
                }
            }

            //选中管理员组
            $('#GroupType').val('<%= g_strType %>');

            //Active Menu
            ActiveMenu('ADMIN_CENTER_SETUP,ADMIN_CENTER_SETUP_GROUP_LIST')
        })
    </script>
</body>
<!-- END BODY -->
</html>

