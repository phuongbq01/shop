var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        $('#btnSend').off('click').on('click', function (e) {
            e.preventDefault();

            var name = $('#txtName').val();
            var mobile = $('#txtMobile').val();
            var address = $('#txtAddress').val();
            var email = $('#txtEmail').val();
            var content = $('#txtContent').val();

            $.ajax({
                url: 'Contact/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    address: address,
                    email: email,
                    content : content
                },

                success: function (res) {
                    if(res.status == true)
                    {
                        window.alert('Gửi Phản Hồi Thành Công');
                    }
                }
            });

        })
    }
}
contact.init();