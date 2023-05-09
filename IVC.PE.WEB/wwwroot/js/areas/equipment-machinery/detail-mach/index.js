var Formula = function () {
    var eqid = null;
    var month = null;
    var year = null;
    var week = null;
    var sid = null;
    var mid = null;


    var mainDatatable = null;
    var foldingDataTable = null;
    var selectWOption = new Option('Seleccione una Semana', null, true, true);

    var detailForm = null;
    var options = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/equipos/costo-obra-maquinaria/listar"),
            dataSrc: "",
            data: function (d) {

                d.year = $("#year_param").val();
                d.month = $("#month_param").val();
                d.week = $("#week_param").val();
                d.providerId = $("#provider_param").val();
                d.phaseId = $("#phase_param").val();
                d.sgId = $("#sg_param").val();
                delete d.columns;
            }
        },
        buttons: [
            
            
        ],
        columns: [
            {
                title: "Año",
                data: "year",
            },
            {
                title: "Mes",
                data : "month"
            },
            {
                title: "Semana",
                data: "wk"
            },

            {
                title: "Proveedor",
                data: "tradeName"
            },
            {
                title: "Cuadrilla",
                data: "sgCode"
            },
            {
                title: "Fase",
                data: "code",

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
                data: "totalFormatted",
            },

            {
                title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-eqid="${row.equipmentProviderId}" data-month="${row.month}" data-year="${row.year}" data-week="${row.wk}" data-sg="${row.sewerGroupId}" data-mp="${row.machineryPhaseId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;

                    return tmp;
                }
            },
        ]
    };

    var detail_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/costo-obra-maquinaria/detalles-listar"),
            dataSrc: "",
            data: function (d) {
                d.providerId = eqid;
                d.year = year;
                d.month = month;
                d.week = week;
                d.sgId = sid;
                d.mId = mid;
                delete d.columns;
            }
        },
        buttons: [


        ],
        columns: [
            {
                title: "Equipo",
                data: "description",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += row.description + " " + row.brand;
                    return tmp;
                }
            },
            {
                title: "Monto",
                data: "ammountFormatted"
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
                select2.headers();
                select2.headers2();
                select2.headers3();
            },
            initEvents: function () {

                mainDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("eqid");
                        let m = $btn.data("month");
                        let y = $btn.data("year");
                        let w = $btn.data("week");
                        let sgid = $btn.data("sg");
                        let mpid = $btn.data("mp");
                        eqid = id;
                        month = m;
                        year = y;
                        week = w;
                        sid = sgid;
                        mid = mpid;
                        datatables.foldingDt.reload();
                        forms.load.detail();
                        console.log(eqid);
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
            this.headers;
            this.headers2;
            this.headers3;
            this.phases.init();
            this.sgs.init();
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
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/monto"),
                data: {
                    year : $("#year_param").val(),
                    month: $("#month_param").val(),
                    week: $("#week_param").val(),
                    providerId: $("#provider_param").val(),
                    phaseId: $("#phase_param").val(),
                    sgId: $("#sg_param").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        },
        headers2: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/igv"),
                data: {
                    year: $("#year_param").val(),
                    month: $("#month_param").val(),
                    week: $("#week_param").val(),
                    providerId: $("#provider_param").val(),
                    phaseId: $("#phase_param").val(),
                    sgId: $("#sg_param").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header2").val(result);
                });
        },
        headers3: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/monto-total"),
                data: {
                    year: $("#year_param").val(),
                    month: $("#month_param").val(),
                    week: $("#week_param").val(),
                    providerId: $("#provider_param").val(),
                    phaseId: $("#phase_param").val(),
                    sgId: $("#sg_param").val(),
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
        phases: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-maquinaria")
                }).done(function (result) {
                    $(".select2-phases").select2({
                        data: result
                    });

                });
            }
        },
        sgs: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas")
                }).done(function (result) {
                    $(".select2-sgs").select2({
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

            $("#year_param,#month_param,#provider_param,#phase_param,#sg_param,#week_param").on("change", function () {
                mainDatatable.ajax.reload();
                select2.headers();
                select2.headers2();
                select2.headers3();
            });

            $("#year_param").on("change", function () {
                mainDatatable.ajax.reload();
                select2.weeks.reload();
            });

            this.headers();
            this.headers2();
            this.headers3();
        },

        headers: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/monto"),
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        },
        headers2: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/igv"),
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header2").val(result);
                });
        },

          headers3: function () {
            $.ajax({
                url: _app.parseUrl("/equipos/costo-obra-maquinaria/monto-total"),
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header3").val(result);
                });
        }
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