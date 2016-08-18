function ShowMsg(msgType, msg)
{
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr[msgType](msg);
}

function ShowMsg_Force(msgType, msg) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "0",
        "extendedTimeOut": "0",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr[msgType](msg);
}

function ActiveMenu(code)
{
    var codeArr = code.split(',');
    $.each(codeArr, function (i, n) {
        $('[data-code="' + n + '"]').addClass('active');
    })
}

function formatDate(val) {
    if (val == null) return '';

    var pa = /.*\((.*)\)/;
    var unixtime = val.match(pa)[1].substring(0, 10);

    var getTime = function () {
        var ts = arguments[0] || 0;
        var t, y, m, d, h, i, s;
        t = ts ? new Date(ts * 1000) : new Date();
        y = t.getFullYear();
        m = t.getMonth() + 1;
        d = t.getDate();
        h = t.getHours();
        i = t.getMinutes();
        s = t.getSeconds();

        // 可根据需要在这里定义时间格式
        return y + '/' + (m < 10 ? '0' + m : m) + '/' + (d < 10 ? '0' + d : d) + ' ' + (h < 10 ? '0' + h : h) + ':' + (i < 10 ? '0' + i : i) + ':' + (s < 10 ? '0' + s : s);
    };

    return getTime(unixtime);
}

$.fn.inputCheck = function (bl, length) {//限制金额输入、兼容浏览器、屏蔽粘贴拖拽等
    $(this).keyup(function (e) {
        var keyCode = e.keyCode ? e.keyCode : e.which;
        if (bl == "int") {//浮点数
            var num = $(this).val().replace(/\D/g, '')
            if (Number(num) == 0) {
                $(this).val('');
            }
            else {
                $(this).val(Number(num))
            }
        } else if (bl == "decimal") {//整数
            var num = $(this).val().replace(/[^\d\.]/g, '');
            if (Number(num) == 0) {
                $(this).val(num);
            }
            else {
                if (num.substr(num.length - 1) == '.') {
                    if (num.substr(0, num.length - 1).indexOf('.') > -1) {
                        $(this).val(num.substr(0, num.length - 1));
                    }
                    else {
                        $(this).val(num);
                    }
                }
                else {
                    if (isNaN(num)) {
                        $(this).val('');
                    }
                    else {
                        var point = num.indexOf('.');
                        if (point > -1 && num.substr(point).length > length + 1) {
                            $(this).val(num.substr(0, point + length + 1));
                        }
                        else {
                            $(this).val(num);
                        }
                    }
                }
            }
        }
    });
}