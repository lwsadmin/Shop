/// <reference path="G:\我的项目\Shop\Shop\Shop.AdminWeb\Scripts/jquery-1.10.2.min.js" />


function CheckAll(obj) {
    var ckText = $(obj).find("span").html();
    if (ckText == "全选") {
        $(obj).find("span").html("取消");
        $("input[name='subBox']").each(function () {
            //$(this).attr("checked", true);
            $(this).prop("checked", true);
        });
    } else {
        $(obj).find("span").html("全选");
        $("input[name='subBox']").each(function () {
            //$(this).attr("checked", false);
            $(this).prop("checked", false);
        });
    }
}

function CheckCount() {
    var size = $("input[name='subBox']:checked").size();
    if (size > 0)
        return size;
    else
        return false;
}

function SelectChild(obj) {
    var pid = $(obj).val();
    var flag = false;
    if ($(obj).is(":checked"))
        flag = true;
    $("tr[pid=" + pid + "]").each(function () {
        $(this).find("td").eq(0).find("input").prop("checked", flag);
    });
}

function Delete(obj, postUrl, callback) {
    debugger;
    if (obj == null) {
        var size = CheckCount();
        if (CheckCount() == false) {
            parent.layer.alert('请选择要操作的对象!', { title: "提示", icon: 6, time: 2000 });

        } else {
            debugger;
            parent.layer.confirm('确定要删除这' + size + '条数据吗?', {
                title: "提示", icon: 3,
                btn: ['确定', '取消'] //按钮
            }, function () {
                var Guids = "";
                $("input[name='subBox']:checked").each(function () {
                    Guids += $(this).val() + ",";
                });
                $.post(postUrl, { Guids: Guids }, function (data) {
                    parent.layer.closeAll('dialog'); //关闭信息框
                    if (data.succ) {
                        this.call(callback);
                        //window.location.href = redictUrl;
                    } else {
                        parent.layer.alert(data.msg, { title: "提示", icon: 2 });
                    }

                })
            });
        }

    } else {
        parent.layer.confirm('确定要删除这条数据吗?', {
            title: "提示",
            icon: 3,
            btn: ['确定', '取消'] //按钮
        }, function () {
            $.post(postUrl, { Guids: obj }, function (data) {
                parent.layer.closeAll('dialog'); //关闭信息框
                if (data.succ) {
                    this.call(callback)
                    // window.location.href = redictUrl;
                } else {
                    parent.layer.alert(data.msg, { title: "提示", icon: 2 });
                }
            })
        });
    }
}

