var Pdp = function () {

    var selectSMOption = new Option('--Seleccione un Tramo--', '', true, true);
    var selectWFHOption = new Option('--Seleccione un Jefe de Frente--', '', true, true);
    var selectWFOption = new Option('--Seleccione un Frente de Trabajo--', '', true, true);
    var selectSGOption = new Option('--Seleccione una Cuadrilla--', '', true, true);
    var selectPFOption = new Option('--Seleccione una Formula--', '', true, true);

    var pdpDatatable = null;

    var addForm = null;
    var editForm = null;

    var pdpDtOpts = {
        responsive: true,
        sScrollX: true,
        ajax: {
            url: _app.parseUrl("/produccion/pdp/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Formula",
                data: "projectFormula"
            },
            {
                title: "Fecha",
                data: "reportDateStr"
            },
            {
                title: "Jefe de Frente",
                data: "workFrontHead"
            },
            {
                title: "Frente",
                data: "workFront"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup"
            },
            {
                title: "Tramo",
                data: "sewerManifold"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: []
    };

    var datatables = {
        init: function () {
            this.pdpDt.init();
        },
        pdpDt: {
            init: function () {
                pdpDatatable = $("#pdps_datatable").DataTable(pdpDtOpts);
                this.events();
            },
            reload: function () {
                pdpDatatable.ajax.reload();
            },
            events: function () {
                pdpDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                pdpDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Parte Diario de Producción será eliminado permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/produccion/pdp/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Parte Diario de Producción ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar el Parte Diario de Producción."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        }
    };
    
    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/pdp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_formula']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_formula']").attr("disabled", "disabled");
                        formElements.find("[name='ReportDate']").val(result.reportDate);
                        formElements.find("[name='WorkFrontHeadId']").val(result.workFrontHeadId);
                        formElements.find("[name='select_workfronthead']").val(result.workFrontHeadId).trigger("change");
                        formElements.find("[name='WorkFrontId']").val(result.workFrontId);
                        formElements.find("[name='select_workfront']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        select2.sewergroups.edit(result.workFrontHeadId, result.sewerGroupId);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_sewermanifold']").val(result.sewerManifoldId).trigger("change");
                        formElements.find("[name='select_sewermanifold']").attr("disabled","disabled");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_workfronthead']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_workfront']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_sewermanifold']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/produccion/pdp/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.pdpDt.reload();
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
            },
            edit: function (formElement) {
                $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_workfronthead']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_workfront']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/produccion/pdp/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.pdpDt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            }
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
        }
    };

    var select2 = {
        init: function () {
            this.formulas.init();
            this.workfronts.init();
            this.workfrontheads.init();
            this.sewergroups.init();
            this.sewermanifolds.init();
        },
        formulas: {
            init: function () {
                $(".select2-formulas").select2();
            }
        },
        workfronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/frentes`)
                }).done(function (result) {
                    $(".select2-workfronts").append(selectWFOption).trigger('change');
                    $(".select2-workfronts").select2({
                        data: result
                    });
                })
            }
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-workfrontheads").append(selectWFHOption);
                    $(".select2-workfrontheads").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
                $(".select2-sewergroups").append(selectSGOption);
            },
            reload: function (id) {
                let wfh = id;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            },
            edit: function (wfhId, sgId) {
                let wfh = wfhId;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption);
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                    $(".select2-sewergroups").val(sgId).trigger('change');
                });
            }
        },
        sewermanifolds: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga`)
                }).done(function (result) {
                    $(".select2-sewermanifolds").append(selectSMOption).trigger('change');
                    $(".select2-sewermanifolds").select2({
                        data: result
                    });
                });
            }
        },
        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/formulas-proyecto`)
                }).done(function (result) {
                    $(".select2-formulas").append(selectPFOption).trigger('change');
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $(".select2-workfrontheads").on("change", function () {
                select2.sewergroups.reload(this.value);
            });

            $(".select2-formulas").on("change", function () {
                var txt = $(".select2-formulas option:selected").text();
                console.log(txt);
                if (txt.indexOf("F5/6") >= 0) {
                    console.log("entre");
                    $(".workfront_group").attr("hidden", false);
                } else {
                    $(".workfront_group").attr("hidden", true);
                }
            });
        }
    };

    return {
        init: function () {
            validate.init();
            modals.init();
            select2.init();
            events.init();
            datepickers.init();
            datatables.init();
        }
    };
}();

$(function () {
    Pdp.init();
});