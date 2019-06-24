var cart = {
    init: function () {
        cart.registerEvents();
    },
    registerEvents: function () {
        // Sự kiện cho button Update
        $('#btnUpdate').off('click').on('click', function () {
            var listproduct = $('.txtQuantity');    // lấy ra danh sách texbox Quantity
            var cartList = [];  
            $.each(listproduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID:$(item).data('id')
                    }
                });
            });

            $.ajax({
                url: 'Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if(res.status ==true)
                    {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });


        // Sự kiện cho button DeleteAll
        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: 'Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if(res.status == true)
                    {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });


        // sự kiện cho button Delete
        $('.btn-Delete').off('click').on('click', function (e) {
            e.preventDefault();

            $.ajax({
                url: 'Cart/Delete',
                data: { id: $(this).data('id') },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });


        // Sự kiện cho button Payment
        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        })
    }
}
cart.init();