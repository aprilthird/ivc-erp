var Formula = function () {

    var mainDatatable = null;

    var selectWOption = new Option('Seleccione una Semana', null, true, true);

    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-maquinaria/listar"),
            dataSrc: "",
            data: function (d) {
                d.week = $("#week_param").val();
                d.year = $("#year_param").val();
                d.providerId = $("#provider_param").val();
                d.month = $("#month_param").val();
                delete d.columns;
            }
        },
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 4, 6,7,10,11,12,13,14],
                hide: [3,5, 8, 9]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: ':hidden'
            },
            {
            text: "<i class='fa fa-briefcase'></i>Reporte Excel Semanal",
            className: "btn-primary",
            action: function (e, dt, node, config) {
                let param1 = $("#week_param").val();
                let param2 = $("#year_param").val();
                let param3 = $("#provider_param").val();
                if (param1 != null)
                    window.location = `/equipos/valorizacion-maquinaria/detalle-folding-excel?week=${param1}&year=${param2}&providerId=${param3}`;

                }
            },
            {
                text: "<i class='fa fa-briefcase'></i>Reporte Excel Mes",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    let param1 = $("#month_param").val();
                    let param2 = $("#year_param").val();
                    let param3 = $("#provider_param").val();
                    window.location = `/equipos/valorizacion-maquinaria/detalle-folding-excel-mes?month=${param1}&year=${param2}&providerId=${param3}`;
                }
            }

        ]



        ,
        columns: [
            {
                title: "Proveedor",
                data: "tradeName"
            },
            {
                title: "Tipo de Equipo",
                data: "description"
            },
            {
                title: "Nombre del Equipo",
                data: "brand",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += row.brand + "-" + row.model + "-" + row.plate;

                    return tmp;
                },
            },
            {
                title: "Mes",
                data: "month",
                visible: false

            },
            {
                title: "Semana",
                data: "week"
            },
            {
                title: "Asignado",
                data:"userName"
            },
            {
                title: "Fase",
                data: "code",
                visible: false
            },
            {
                title: "Cuadrilla",
                data: "sewerCode"
            },
            {
                title: "Operador",
                data: "actualName"
            },
            {
                title: "Horometro Inicial",
                data: "initHorometer",
                visible: false,
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                title: "Horometro Final",
                data: "endHorometer",
                visible: false,
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                title: "Horometro Acumulado (hm)",
                data: "acumulatedHorometer",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            //{
            //    title: "Cantidad de Días",
            //    data: "foldingNumber"
            //},
            {
                title: "Precio Unitario (hm) (S/)",
                data: "unitPriceFormatted"
            },
            {
                title: "Monto (S/)",
                data: "ammountFormatted"
            },
            {
                title: "IGV (S/)",
                data: "igvFormatted"
            },
            {
                title: "Total (S/)",
                data: "totalAmmountFormatted"
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
        },
        reload: function () {
            mainDatatable.ajax.reload();
        }
    };

    var select2 = {
        init: function () {
            this.providers.init();
            this.headers;
            this.headers2;
            this.headers3;
            this.years.init();
            this.weeks.init();

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
                url: _app.parseUrl("/equipos/valorizacion-maquinaria/monto"),
                data: {
                    week: $("#week_param").val(),
                    year: $("#year_param").val(),
                    month: $("#month_param").val(),
                    providerId: $("#provider_param").val(),
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
                    week: $("#week_param").val(),
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
                    week: $("#week_param").val(),
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

            $("#week_param,#year_param,#provider_param,#month_param").on("change", function () {
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