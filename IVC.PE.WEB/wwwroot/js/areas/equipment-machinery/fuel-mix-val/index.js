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

                    window.location = `/equipos/valorizacion-combustible-mixto/detalle-folding-excel-Junto?week=${param1}&year=${param2}`;
                }
            }

        ],
       
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
        datatable: {
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

            $("#week_param").on("change", function () {
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