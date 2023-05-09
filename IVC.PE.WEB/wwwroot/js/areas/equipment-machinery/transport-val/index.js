var Formula = function () {

    var mainDatatable = null;

    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-transporte/listar"),
            dataSrc: "",
            data: function (d) {

                d.year = $("#year_param").val();
                d.month = $("#month_param").val();
                d.providerId = $("#provider_param").val();

                delete d.columns;
            }
        },
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 4, 6],
                hide: [3, 4, 5, 7, 8, 9]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: ':hidden'
            },
            {
            text: "<i class='fa fa-briefcase'></i>Reporte Excel",
            className: "btn-primary",
            action: function (e, dt, node, config) {
                let param1 = $("#year_param").val();
                let param2 = $("#month_param").val();
                let param3 = $("#provider_param").val();
                window.location = `/equipos/valorizacion-transporte/detalle-folding-excel?year=${param1}&month=${param2}&providerId=${param3}`;
            }
        }],
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
                    tmp += row.brand + "-" + row.model + "-" + row.equipmentPlate;

                    return tmp;
                },
            },
            {
                title: "Mes",
                data: "month",
                visible: false

            },
            {
                title: "Asignado",
                data: "userName",
                visible: false
            },
            {
                title: "Fase",
                data: "code",
                visible: false
            },
            {
                title: "Operador",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1)
                        tmp += row.operatorName;
                    else if (data == 3)
                        tmp += row.fromOtherName;
                    else if (data == 2)
                        tmp += row.workerName + " " + row.workerPaternalSurName + " " + row.workerMaternalSurName;

                    return tmp;
                },
            },
            {
                title: "Kilometraje Inicial",
                data: "lastInitMileage",
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
                title: "Kilometraje Final",
                data: "lastEndMileage",
                
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
                title: "Kilometraje Acumulado (km)",
                data: "acumulatedMileage",
                
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
                title: "Cantidad de Días",
                data: "foldingNumber"
            },
            {
                title: "Precio Unitario (día) (S/)",
                data: "unitPriceFormatted",

            },
            {
                title: "Monto (S/)",
                data: "ammountFormatted",

            },
            {
                title: "IGV (S/)",
                data: "igvFormatted",

            },
            {
                title: "Total (S/)",
                data: "totalAmmountFormatted",
            
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