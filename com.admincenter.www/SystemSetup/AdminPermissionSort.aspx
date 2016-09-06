<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPermissionSort.aspx.cs" Inherits="com.admincenter.www.SystemSetup.AdminPermissionSort" %>

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
        /*ul.ztree {
            border-top: 1px solid gray;
            width: 220px;
            height: 260px;
            overflow-y: scroll;
            overflow-x: auto;
        }*/

        .ztree li span {
            line-height: 16px;
        }

        .ztree li a input.rename {
            height: 16px;
        }

        .ztree li a.curSelectedNode {
            height: 18px;
        }

        .ztree li span.button.add {
            margin-left: 2px;
            margin-right: -1px;
            background-position: -144px 0;
            vertical-align: top;
            *vertical-align: middle;
        }

        .ztree li span.button.addCategory {
            margin-left: 2px;
            margin-right: -1px;
            vertical-align: top;
            *vertical-align: middle;
            background: url('http://<%= AdminCenterDomain %>/Resource/plugins/ztree/css/zTreeStyle/img/addCategory.png') no-repeat;
        }

        .ztree li span.button.addProperty {
            margin-left: 2px;
            margin-right: -1px;
            vertical-align: top;
            *vertical-align: middle;
            background: url('http://<%= AdminCenterDomain %>/Resource/plugins/ztree/css/zTreeStyle/img/addProperty.png') no-repeat;
        }

        .ztree li span.button.uploadCategoryIcon {
            margin-left: 2px;
            margin-right: -1px;
            vertical-align: top;
            *vertical-align: middle;
            background: url('http://<%= AdminCenterDomain %>/Resource/plugins/ztree/css/zTreeStyle/img/uploadCategoryIcon.png') no-repeat;
        }

        .ztree li span.button.addPropertyValue {
            margin-left: 2px;
            margin-right: -1px;
            vertical-align: top;
            *vertical-align: middle;
            background: url('http://<%= AdminCenterDomain %>/Resource/plugins/ztree/css/zTreeStyle/img/addPropertyValue.png') no-repeat;
        }

        .ztree li span.button.copyProperty {
            margin-left: 2px;
            margin-right: -1px;
            vertical-align: top;
            *vertical-align: middle;
            background: url('http://<%= AdminCenterDomain %>/Resource/plugins/ztree/css/zTreeStyle/img/copyProperty.png') no-repeat;
        }

        .ztree li input[type="checkbox"] {
            margin: 0 1px 0 0;
            vertical-align: middle;
            cursor: pointer;
        }
    </style>
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
                        <div class="portlet blue box">
                            <div class="portlet-title">
                                <div class="caption">
                                    系统权限排序
                                </div>
                            </div>
                            <div class="portlet-body">
                                <ul id="tree1" class="ztree"></ul>
                            </div>
                        </div>
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
        var setting = {
            view: {
                selectedMulti: false,
                showIcon: false
            },
            edit: {
                drag: {
                    isCopy: false,
                    isMove: true,
                    inner: false,
                    prev: dropPrev,
                    next: dropNext
                },
                enable: true,
                showRemoveBtn: false,
                showRenameBtn: false
            },
            callback: {
                beforeExpand: beforeExpand,
                onExpand: onExpand,
                beforeDrag: beforeDrag,
                beforeDrop: beforeDrop
            }
        };

        var zNodes = <%= g_strJson %>;

        //BEGIN 展开单个节点
        var curExpandNode;

        //var curExpandNode1 = null;
        //var curExpandNode2 = null;
        function beforeExpand(treeId, treeNode) {
            //var curExpandNode = curExpandNode1;
            //switch (treeId) {
            //    case 'tree1':
            //        curExpandNode = curExpandNode1;
            //        break;
            //    case 'tree2':
            //        curExpandNode = curExpandNode2;
            //        break;
            //}

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
            //var curExpandNode = curExpandNode1;
            //switch (treeId) {
            //    case 'tree1':
            //        curExpandNode = curExpandNode1;
            //        break;
            //    case 'tree2':
            //        curExpandNode = curExpandNode2;
            //        break;
            //}

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
            //switch (treeId) {
            //    case 'tree1':
            //        curExpandNode1 = newNode;
            //        break;
            //    case 'tree2':
            //        curExpandNode2 = newNode;
            //        break;
            //}
            curExpandNode = newNode;
        }

        function onExpand(event, treeId, treeNode) {
            //switch (treeId) {
            //    case 'tree1':
            //        curExpandNode1 = treeNode;
            //        break;
            //    case 'tree2':
            //        curExpandNode2 = treeNode;
            //        break;
            //}
            curExpandNode = treeNode;
        }
        //END 展开单个节点


        //BEGIN 拖动节点
        var curDragNode;

        function dropPrev(treeId, nodes, targetNode) {
            if (curDragNode.parentId != targetNode.parentId) {
                return false;
            }
            return true;
        }

        function dropNext(treeId, nodes, targetNode) {
            if (curDragNode.parentId != targetNode.parentId) {
                return false;
            }
            return true;
        }

        function beforeDrag(treeId, treeNodes) {
            curDragNode = treeNodes[0];
            return true;
        }

        function beforeDrop(treeId, treeNodes, targetNode, moveType, isCopy) {
            var returnValue = false;
            $.ajax({
                url: "/SystemSetup/AdminPermissionSort.aspx",
                type: "POST",
                data: { type: "sort", dragPermissionId: curDragNode.id, targetPermissionId: targetNode.id, moveType: moveType, AjaxRequest: "true" },
                async: false,
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.Succeeded) {
                        returnValue = true;
                    }
                    ShowMsg(data.MsgType, data.Message);
                }
            })
            return returnValue;
        }
        //END 拖动节点
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.fn.zTree.init($("#tree1"), setting, zNodes);

            //Active Menu
            ActiveMenu('ADMIN_CENTER_SETUP,ADMIN_CENTER_SETUP_PERMISSION_LIST')
        });

    </script>
</body>
<!-- END BODY -->
</html>

