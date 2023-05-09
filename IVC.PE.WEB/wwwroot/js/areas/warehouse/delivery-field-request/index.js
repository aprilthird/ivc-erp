var DeliveryFieldRequest = function () {

    var mainDatatable = null;
    var detailDatatable = null;
    var detailForm = null;
    var Id = null;

    var list = [];

    var totalCost = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/entregas/listar"),
            data: function (d) {
                d.budgetTitleId = $("#budget_title_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.year = $("#year_filter").val();
                d.month = $("#month_filter").val();
                d.weekId = $("#week_filter").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                totalCost = 0;
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        totalCost += item.cost;
                    });
                }

                api = this.api;
                return $("#total_parcial").val(formatter.format(totalCost));
            }
        },
        columns: [
            {
                title: "Código IVC",
                data: "goalBudgetInput.supply.fullCode"
            },
            {
                title: "Insumo",
                data: "goalBudgetInput.supply.description"
            },
            {
                title: "Unidad",
                data: "goalBudgetInput.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Despachado",
                data: "deliveredQuantity"
            },
            {
                title: "Techo",
                data: "techo"
            },
            {
                title: "Saldo A Pedir",
                data: "measureToRequest"
            },
            {
                title: "Costo (S/)",
                data: "costString"
            },
            {
                title: "Costo (S/)",
                data: "cost",
                visible: false
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.goalBudgetInput.supplyId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    */
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2, 3, 4, 5, 6] }
        ]
    };

    var detailOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/entregas/detalles/listar"),
            data: function (d) {
                d.supplyId = Id;

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Fecha de Despacho",
                data: "fieldRequest.deliveryDate"
            },
            {
                title: "N° Documento",
                data: "fieldRequest.documentNumber"
            },
            {
                title: "Fase",
                data: "projectPhase.code"
            },
            {
                title: "Unidad",
                data: "goalBudgetInput.measurementUnit.abbreviation"
            },
            {
                title: "Cantidad",
                data: "deliveredQuantity"
            },
            {
                title: "Monto (S/)",
                data: "parcial"
            },
            {
                title: "Archivo",
                width: "15%",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [4, 5] }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            detailDatatable = $("#detail_datatable").DataTable(detailOpts);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        detailReload: function () {
            detailDatatable.ajax.reload();
        },
        aux: function () {
            console.log(mainDatatable.column(7, { filter: 'applied' })
                .data()
                .reduce(function (a, b) {
                    return a + b;
                }));
        },
        initEvents: function () {

            mainDatatable.on("click",
                ".btn-details",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.detailReload();
                    $("#detail_modal").modal("show");
                });
        }
    };

    var form = {
        submit: {
            
        },
        reset: {
            detail: function () {
                detailForm.resetForm();
                $("#detail_form").trigger("reset");
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
        }
    };

    var validate = {
        init: function () {
            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.budgetTitle.init();
            this.formula.init();
            this.workFront.init();
            this.sewerGroup.init();
            this.warehouse.init();
            this.supplyFamily.init();
            this.supplyGroup.init();
            this.year.init();
            this.month.init();
            this.week.init();
        },
        budgetTitle: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budget-titles").select2({
                        data: result,
                        allowClear: false
                    });
                });
            }
        },
        formula: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                }).done(function (result) {
                    $(".select2-project-formulas").select2({
                        data: result
                    });
                });
            }
        },
        workFront: {
            init: function () {
                $("#work_front_filter").empty();
                $("#work_front_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#project_formula_filter").val()
                    }
                }).done(function (result) {
                    $("#work_front_filter").select2({
                        data: result
                    });
                });
            }
        },
        sewerGroup: {
            init: function () {
                $("#sewer_group_filter").empty();
                $("#sewer_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-frente"),
                    data: {
                        workFrontId: $("#work_front_filter").val()
                    }
                }).done(function (result) {
                    $("#sewer_group_filter").select2({
                        data: result
                    });
                });
            }
        },
        warehouse: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/almacenes")
                }).done(function (result) {
                    $(".select2-warehouses").select2({
                        data: result
                    });
                });
            }
        },
        supplyFamily: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroup: {
            init: function () {
                $("#supply_group_filter").empty();
                $("#supply_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $("#supply_group_filter").select2({
                        data: result
                    });
                });
            }
        },
        year: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/anios-proyecto")
                }).done(function (result) {
                    $("#year_filter").select2({
                        data: result
                    });
                });
            }
        },
        week: {
            init: function () {
                console.log($("#year_filter").val());
                $("#week_filter").empty();
                $("#week_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/semanas"),
                    data: {
                        year: $("#year_filter").val(),
                        month: $("#month_filter").val()
                    }
                }).done(function (result) {
                    $("#week_filter").select2({
                        data: result
                    });
                });
            }
        },
        month: {
            init: function () {
                $("#month_filter").select2({
                });
            }
        }
    };

    var modals = {
        init: function () {
            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });
        }
    };

    var events = {
        init: function () {

            $("#project_formula_filter").on("change", function () {
                select2.workFront.init();
                datatable.reload();
            });

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroup.init();
                datatable.reload();
            });

            $("#budget_title_filter").on("change", function () {
                datatable.reload();
            });

            $("#work_front_filter").on("change", function () {
                datatable.reload();
                select2.sewerGroup.init();
            });

            $("#sewer_group_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_group_filter").on("change", function () {
                datatable.reload();
            });

            $("#year_filter").on("change", function () {
                select2.week.init();
                datatable.reload();
            });

            $("#month_filter").on("change", function () {
                select2.week.init();
                datatable.reload();
            });

            $("#week_filter").on("change", function () {
                datatable.reload();
            });


            $('#main_datatable').on('search.dt', function () {
                console.log($('.dataTables_filter input').val());
                /*
                let res = mainDatatable.api().column(7)
                    .data()
                    .reduce(function (a, b) {
                        return a + b;
                    }, 0);
                */
                //return $("#total_parcial").val(formatter.format(res));
                //datatable.aux();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
        },
        test: function () {
            datatable.aux();
        }
    };
}();

$(function () {
    DeliveryFieldRequest.init();
});