var Formula = function () {
    var foldingId = null;

    var mainDatatable = null;
    var foldingDataTable = null;
    var selectWOption = new Option('Seleccione una Semana', null, true, true);
    var detailForm = null;
    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-combustible-transporte/listar"),
            dataSrc: "",
            data: function (d) {

                d.year = $("#year_param").val();
                d.week = $("#week_param").val();
                d.providerId = $("#provider_param").val();
                d.transportId = $("#transport_param").val();
                delete d.columns;
            }
        },
        buttons: [
            {
                text: "<i class='fa fa-briefcase'></i>Reporte Excel",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    let param1 = $("#week_param").val();
                    let param2 = $("#year_param").val();

                    window.location = `/equipos/valorizacion-combustible-transporte/detalle-folding-excel?week=${param1}&year=${param2}`;
                }
            },
            //{
            //    text: "<i class='fa fa-briefcase'></i>Reporte Excel Junto",
            //    className: "btn-primary",
            //    action: function (e, dt, node, config) {
            //        let param1 = $("#week_param").val();
            //        let param2 = $("#year_param").val();

            //        window.location = `/equipos/valorizacion-combustible-transporte/detalle-folding-excel-Junto?week=${param1}&year=${param2}`;
            //    }
            //}
            
        ],
        columns: [
            {
                title: "Tipo de Equipo",
                data: "description",
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
                title: "Proveedor",
                data: "tradeName"
            },
            {
                title: "Condición de Servicio",
                data: "serviceConditionDesc"
            },
            {
                title: "Operador",
                data: "actualName"
            },
            {
                title: "Encargado",
                data: "username"
            },

            {
                title: "Lunes (gln)",
                data: "monday"
            },
            {
                title: "Martes (gln)",
                data: "tuesday"
            },
            {
                title: "Miercoles (gln)",
                data: "wednesday"
            },
            {
                title: "Jueves (gln)",
                data: "thursday"
            },
            {
                title: "Viernes (gln)",
                data: "friday"
            },
            {
                title: "Sabado (gln)",
                data: "saturday"
            },
            {
                title: "Domingo (gln)",
                data: "sunday"
            },
            {
                title: "Total Consumo (gln)",
                data: "totalGallon"
            },
            {
                title: "Kilometraje Inicial",
                data: "initMileage"
            },
            {
                title: "Kilometraje Final",
                data: "endMileage"
            },
            {
                title: "Total Kilometraje",
                data: "dif"
            },
            {
                title: "Ratio de Consumo (km/gln)",
                data: "kGallon",
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
                title: "Ratio de Consumo (gln/km)",
                data: "gKilometer",
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
                title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-fuelid="${row.fuelId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;

                    return tmp;
                }
            },
        ]
    };

    var detail_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/valorizacion-combustible-transporte/detalles-listar"),
            dataSrc: "",
            data: function (d) {
                d.foldingId = foldingId
                d.week = $("#week_param").val();
                
                delete d.columns;
            }
        },
        buttons: [


        ],
        columns: [
            {
                title: "Fecha",
                data: "partDateString"
            },
            {
                title: "Total Consumo (gln)",
                data: "gallon"
            },
            {
                title: "Precio x gln",
                data: "price"
            },
            {
                title: "Total S/",
                data: "totalFormatted"
            },
         
        ]
    };
    var datatables = {
        init: function () {
            this.datatable.init();
            this.foldingDt.init();
        },
        datatable : {
            init: function () {
                mainDatatable = $("#main_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                mainDatatable.ajax.reload();

            },
            initEvents: function () {

                mainDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("fuelid");

                        foldingId = id;


                        datatables.foldingDt.reload();
                        forms.load.detail();

                    });   
            }
        },
        foldingDt: {
            init: function () {
                foldingDataTable = $("#folding_datatable").DataTable(detail_options);
            },
            reload: function () {
                foldingDataTable.ajax.reload();

            },
        },

    };

    var forms = {
        load: {

            detail: function () {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte/detail`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

     
        },
     
    };
    var select2 = {
        init: function () {
            this.providers.init();
            this.phases.init();
            this.weeks.init();
            this.transports.init();
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
        transports: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/equipos-transporte-filtro")
                }).done(function (result) {
                    $(".select2-transports").select2({
                        data: result
                    });

                });
            }
        },
        phases: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-transporte")
                }).done(function (result) {
                    $(".select2-phases").select2({
                        data: result
                    });

                });
            }
        },
    };

    var validate = {
        init: function () {
            

            detailForm = $("#detail_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });



        }
    };

    var events = {
        init: function () {

            $("#week_param,#provider_param,#transport_param").on("change", function () {
                mainDatatable.ajax.reload();
                foldingDataTable.ajax.reload();
            });

            $("#year_param").on("change", function () {
                select2.weeks.reload();

            });


        },

        
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            events.init();
            validate.init();
        }
    }


}();

$(function () {
    Formula.init();
});