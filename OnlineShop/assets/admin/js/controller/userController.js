var user = {
    init: function () {
        console.log('aaa')
        user.registerEvents();
    },
    // đăng kí sự kiện: registerEvents
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var self = this;
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                type: "POST",
                dataType: "json",
                success: function (resspon) {
                    if (resspon.status) {
                        $(self).text("Khóa")
                    }
                    else {
                        $(self).text("Kích hoạt")
                    }
                }
            })
        })
    }
}

user.init();