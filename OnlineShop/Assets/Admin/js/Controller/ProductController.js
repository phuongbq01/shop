var user = {
    init: function () {
        user.registerEvent();
    },
    registerEvent: function () {
        $('.btn-product-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Admin/Product/ChangeStatus",
                data: { id: id },
                datatype: "json",
                type: "POST",
                success: function (response) {
                    if(response.status == true)
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