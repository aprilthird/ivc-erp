var WeeklyAdvance = function () {

    var addForm = null;
    var editForm = null;

    var weeklyAdvanceId = null;

    var weeklyAdvanceDatatable = null;
    var importDataForm = null;

    var weeklyAdvanceDtOpt = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24, 25, 26],
                hide: [15, 16, 17, 18, 19]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            },
            {
                extend: 'copy',
                className: "btn-dark",
                text: "<i class='fa fa-copy'></i> Copiar"
            },
            {
                extend: 'excel',
                className: "btn-success",
                text: "<i class='fa fa-file-excel'></i> Excel"
            },
            {
                extend: 'csv',
                className: "btn-success",
                text: "<i class='fa fa-file-csv'></i> CSV"
            },
            {
                extend: 'pdf',
                className: "btn-danger",
                text: "<i class='fa fa-file-pdf'></i> PDF"
            },
            {
                extend: 'print',
                className: "btn-dark",
                text: "<i class='fa fa-print'></i> Imprimir"
            }
        ],
        ajax: {
            url: _app.parseUrl("/produccion/avance-semanal/listar"),
            data: function (d) {
                d.projectCalendarWeekId = $("#calendar_filter").val();
                d.workFrontHeadId = $("#work_front_head_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                //title: "Formula",
                data: function (result) {
                    var code = result.projectFormula.code;
                    var name = result.projectFormula.name;
                    return code + " - " + name;
                }
            },
            {
                //title: "Semana",
                data: function (result) {
                    var start = result.projectCalendarWeek.weekStart;
                    var end = result.projectCalendarWeek.weekEnd;
                    return start + " " + end;
                }
            },
            {
                //title: "Jefe de Frente",
                data: "workFrontHead.user.auxFullName"
            },
            {
                //title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                //title: "Cuadrilla",
                data: "totalNetBudget"
            },
            {
                //title: "Cuadrilla",
                data: "accumulatedBudget"
            },
            {
                //title: "Cuadrilla",
                data: "percentageAdvance"
            },
            {
                //tile: "Cant. Obreros OP",
                data: "workersNumberOP"
            },
            {
                //tile: "Cant. Obreros OF",
                data: "workersNumberOF"
            },
            {
                //tile: "Cant. Obreros PE",
                data: "workersNumberPE"
            },
            {
                //tile: "Cant. Obreros Total",
                data: "workerNumberTotal"
            },
            {
                //title: "Venta MO",
                data: "saleMO"
            },
            {
                //title: "Venta EQ",
                data: "saleEQ"
            },
            {
                //title: "Venta S/C",
                data: "saleSubcontract"
            },
            {
                //title: "Venta MAT.",
                data: "saleMaterials"
            },
            {
                //title: "Venta Total",
                data: "saleTotal"
            },
            {
                //title: "Meta MO",
                data: "goalMO",
                visible: false
            },
            {
                //title: "Meta EQ",
                data: "goalEQ",
                visible: false
            },
            {
                //title: "Meta S/C",
                data: "goalSubcontract",
                visible: false
            },
            {
                //title: "Meta MAT.",
                data: "goalMaterials",
                visible: false
            },
            {
                //title: "Meta Total",
                data: "goalTotal",
                visible: false
            },
            {
                //title: "Costo MO",
                data: "costMO"
            },
            {
                //title: "Costo EQ",
                data: "costEQ"
            },
            {
                //title: "Costo S/C",
                data: "costSubcontract"
            },
            {
                //title: "Costo MAT.",
                data: "costMaterials"
            },
            {
                //title: "Costo Total",
                data: "costTotal"
            },
            {
                //title: "Margen Actual",
                data: "marginActual"
            },
            {
                //title: "Margen Acumulado",
                data: "marginAccumulated"
            },
            {
                //title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-folder"></i></button> `;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26] }
        ]
    };

    var foldingDtOpt = {
        reponsive: true,
        ajax: {
            url: _app.parseUrl(`/produccion/folding-avance-semanal/listar`),
            data: function (d) {
                d.weeklyAdvanceId = weeklyAdvanceId;
                delete d.columns;
            },
            dataSrc: ""
        },
        "ordering": false,
        buttons: [],
        columns: [
            {
                title: "Item",
                data: "numberItem"
            },
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Unidad",
                data: "unit"
            },
            {
                title: "Avance Actual",
                data: "actualAdvance"
            },
            {
                title: "MO",
                data: "contractualMO"
            },
            {
                title: "EQ",
                data: "contractualEQ"
            },
            {
                title: "S/C",
                data: "contractualSubcontract"
            },
            {
                title: "Materiales",
                data: "contractualMaterials"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 7] }
        ]
    }

    var datatable = {
        init: function () {
            this.weeklyAdvanceDt.init();
            this.foldingDt.init();
        },
        weeklyAdvanceDt: {
            init: function () {
                weeklyAdvanceDatatable = $("#weekly_advance_datatable").DataTable(weeklyAdvanceDtOpt);
                this.initEvents();
            },
            reload: function () {
                weeklyAdvanceDatatable.ajax.reload();
            },
            initEvents: function () {

                weeklyAdvanceDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        weeklyAdvanceId = id;
                        datatable.foldingDt.reload();
                        forms.load.detail(id);
                    });
            }
        },
        foldingDt: {
            init: function () {
                foldingDatatable = $("#folding_datatable").DataTable(foldingDtOpt);
            },
            reload: function () {
                foldingDatatable.ajax.reload();
            },
            events: function () {

            }
        }
    };

    var forms = {
        load: {
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/avance-semanal/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        $("#week_name").text("del " + result.projectCalendarWeek.weekStart + " al " + result.projectCalendarWeek.weekEnd);
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            import: {
                data: function (formElement) {
                    $(formElement).find("[name='ProjectCalendarWeekId']").val($(formElement).find("[name='select_calendars']").val());
                    $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                    $(formElement).find("[name='WorkFrontHeadId'").val($(formElement).find("[name='select_front_heads']").val());
                    $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_groups']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: "/produccion/avance-semanal/importar-datos",
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();
                            if (!emptyFile) {
                                xhr.upload.onprogress = function (evt) {
                                    if (evt.lengthComputable) {
                                        var percentComplete = evt.loaded / evt.total;
                                        percentComplete = parseInt(percentComplete * 100);
                                        $(".progress-bar").width(percentComplete + "%");
                                    }
                                };
                            }
                            return xhr;
                        }
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        datatable.weeklyAdvanceDt.reload();
                        $("#import_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_data_alert_text").html(error.responseText);
                            $("#import_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.data();
                });
        }
    };

    var select2 = {
        init: function () {
            this.calendars.init();
            this.formulas.init();
            this.workFrontHeads.init();
            this.sewerGroups.init();
            this.sewerGroups.add();
            this.filters();
            this.formula();
        },
        calendars: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/todo-semanas`)
                }).done(function (result) {
                    $(".select2-calendars").select2({
                        data: result
                    });
                    $(".select2-calendar-filter").select2({
                        data: result
                    });
                })
            }
        },
        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/formulas-proyecto`)
                }).done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                })
            }
        },
        workFrontHeads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-front-heads").select2({
                        data: result
                    });
                    $(".select2-workfronthead-filter").select2({
                        data: result
                    });
                })
            }
        },
        sewerGroups: {
            init: function () {
                $(".select2-sewergroup-filter").empty();
                $(".select2-sewergroup-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente`),
                    data: {
                        workFrontHeadId: $("#work_front_head_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                    $(".select2-sewergroup-filter").select2({
                        data: result
                    });
                })
            },
            add: function () {
                let formElements = $("#add_form");
                $("[name='select_front_heads']").on("change", function () {
                    $(".select2-sewer-groups").empty();
                    $.ajax({
                        url: _app.parseUrl(`/select/cuadrillas-jefe-frente`),
                        data: {
                            workFrontHeadId: $("[name='select_front_heads']").val()
                        },
                        dataSrc: ""
                    }).done(function (result) {
                        $(".select2-sewer-groups").select2({
                            data: result
                        });
                    })
                });
            }
        },
        formula: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-projectformula-filter").select2({
                        data: result
                    });
                });
        },
        filters: function (){
            $("#calendar_filter").on("change", function () {
                datatable.weeklyAdvanceDt.reload();
            });
            $("#work_front_head_filter").on("change", function () {
                select2.sewerGroups.init();
                datatable.weeklyAdvanceDt.reload();
            });
            $("#sewer_group_filter").on("change", function () {
                datatable.weeklyAdvanceDt.reload();
            });
            $("#project_formula_filter").on("change", function () {
                datatable.weeklyAdvanceDt.reload();
            });
        }
    }

    var events = {
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/produccion/avance-semanal/excel-carga-masiva`;
            });
        },
    }

    return {
        init: function () {
            datatable.init();
            select2.init();
            validate.init();
            modals.init();
            events.excel();
        }
    };

}();

$(function () {
    WeeklyAdvance.init();
});