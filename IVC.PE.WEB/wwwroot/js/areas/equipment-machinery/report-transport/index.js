var Formula = function () {

    var mainDatatable = null;

    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-transporte-reporte/listar"),
            dataSrc: "",
            data: function (d) {

                d.year = $("#year_param").val();
                d.month = $("#month_param").val();
                d.providerId = $("#provider_param").val();

                delete d.columns;
            }
        },
        buttons: [
            
            
        ],
        columns: [
            {
                title: "Proveedor",
                data: "provider",
            },
            {
                title: "# Equipos",
                data : "qt"
            },
            {
                title: "Cantidad de Días",
                data: "days"
            },
            {
                title: "Monto (S/)",
                data: "str",

            },
            {
                title: "IGV (S/)",
                data: "str2",

            },
            {
                title: "Total (S/)",
                data: "str3",
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
        },
        reload: function () {
            mainDatatable.ajax.reload();
            select2.headers();
            select2.headers2();
            select2.headers3();
        }
    };

    var select2 = {
        init: function () {
            this.providers.init();
            this.headers;
            this.headers2;
            this.headers3;
            this.years.init();
        },
        years: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/años-equipos")
                }).done(function (result) {

                    $(".select2-years").select2({
                        data: result
                    });

                });
            }
        },
        headers: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/valorizacion-transporte/monto"),
                data: {
                    year : $("#year_param").val(),
                    month : $("#month_param").val(),
                    providerId :  $("#provider_param").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        },
        headers2: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/valorizacion-transporte/igv"),
                data: {
                    year: $("#year_param").val(),
                    month: $("#month_param").val(),
                    providerId: $("#provider_param").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header2").val(result);
                });
        },
        headers3: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/valorizacion-transporte/monto-total"),
                data: {
                    year: $("#year_param").val(),
                    month: $("#month_param").val(),
                    providerId: $("#provider_param").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header3").val(result);
                });
        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-transporte")
                }).done(function (result) {
                    $(".select2-providers").select2({
                        data: result
                    });

                });
            }
        },

    };

    var events = {
        init: function () {

            $("#year_param,#month_param,#provider_param").on("change", function () {
                mainDatatable.ajax.reload();
                select2.headers();
                select2.headers2();
                select2.headers3();
            });
        },
    };

    return {
        init: function () {
            datatable.init();
            select2.init();
            events.init();
        }
    }


}();

$(function () {
    Formula.init();
});