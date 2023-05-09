var Supplier = function () {
    var form = {
        submit: {
            add: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/libreria-tecnica/catalogo-proveedores/crear-proveedor"),
                    method: "post",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        return xhr;
                    }
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        }
    };

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

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });
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
            validate.init();
        }
    };
}();

$(function () {
    Supplier.init();
});