var user = {
    init: function () {
        user.registerEvent();
    },
    registerEvent: function () {
        $('.btn-user-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                datatype: "json",       // kiểu dữ liệu mong muốn được trả về từ server
                type: "POST",           // kiểu request muốn thực hiện
                success: function (response) {
                    if (response.status == true)
                    {
                        btn.text('Kích Hoạt');
                    }
                    else 
                    {
                        btn.text('Khóa');
                    }
                }
            })
        });
    }
}
user.init();