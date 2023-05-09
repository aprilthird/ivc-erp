var Supplier = function () {
    var select2 = {
        init: function () {
            this.types.init();
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var loadSuppliers = {
        init: function () {
            _app.loader.showOnElement("#result_container");
            $.ajax({
                url: "/libreria-tecnica/catalogo-proveedores/listar-proveedores",
            })
                .done(function (result) {
                    $("#result_container").html(result);
                });
        }
    };

    return {
        init: function () {
            loadSuppliers.init();
            select2.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Supplier.init();
});