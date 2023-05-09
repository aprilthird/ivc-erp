var Formula = function () {

    var mainDatatable = null;
    var selectWOption = new Option('Seleccione una Semana', null, true, true);

    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-maquinaria-reporte/listar"),
            dataSrc: "",
            data: function (d) {
                d.week = $("#week_param").val();
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
                title: "Horometro acumulado",
                data: "acumulated"
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
            this.weeks.init();
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
        weeks: {
            init: function () {

                $.ajax({
                    url: _app.parseUrl(`/select/semanas-equipos`),
                    data: {
                        year: $("#year_param").val(),

                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {

                $.ajax({
                    url: _app.parseUrl(`/select/semanas-equipos`),
                    data: {
                        year: $("#year_param").val(),
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-weeks").append(selectWOption).trigger('change');
                    $(".select2-weeks").select2({
                        data: result
                    });
                })

                $(".select2-weeks").empty();
                this.init();
            }
        },
        headers: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/valorizacion-maquinaria/monto"),
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
                url: _app.parseUrl("/equipos/valorizacion-maquinaria/igv"),
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
                url: _app.parseUrl("/equipos/valorizacion-maquinaria/monto-total"),
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
                    url: _app.parseUrl("/select/proveedores-de-equipos-maquinaria")
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

            $("#year_param,#month_param,#provider_param,#week_param").on("change", function () {
                mainDatatable.ajax.reload();
                select2.headers();
                select2.headers2();
                select2.headers3();
            });

            $("#year_param").on("change", function () {
                mainDatatable.ajax.reload();
                select2.weeks.reload();
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