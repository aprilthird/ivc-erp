var Formulas = function () {

    var formulaId = null;

    var formulasDatatable = null;
    var activitiesDatatable = null;
    var sewergroupsDatatable = null;
    var phaseDatatable = null;

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var sewerForm = null;
    var addActivityForm = null;
    var addSewerGroupForm = null;
    var editActivityForm = null;
    var phaseImportForm = null;

    var formulasDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/formulas/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Formula",
                data: "name"
            },
            {
                title: "Agrupación",
                data: function (result) {
                    var resp = result.group;
                    if (resp == 0)
                        return "Sin Grupo"
                    else if (resp == 1)
                        return "Componente Principal"
                    else if (resp == 2)
                        return "Otros Componentes"
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-sewergroups">`;
                    tmp += `<i class="fa fa-hard-hat"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-phases">`;
                    tmp += `<i class="fa fa-list-alt"></i></button> `;
                    return tmp;
                }
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
    var activitiesDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/actividades-formula/listar"),
            data: function (d) {
                d.formulaId = formulaId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "description"
            },
            {
                title: "Unidad",
                data: "measurementUnit.abbreviation"
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
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#detail_modal").modal("hide");
                    $("#add_activity_modal").modal("show");
                }
            }
        ]
    };
    var sewergroupsDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/cuadrillas-formula/listar"),
            data: function (d) {
                d.formulaId = formulaId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#sewergroup_modal").modal("hide");
                    forms.load.addSewerGroup(formulaId);
                }
            }
        ]
    };
    var phaseDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/formulas/fases/listar"),
            data: function (d) {
                d.formulaId = formulaId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Fase",
                data: "description"
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Importar",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#phase_modal").modal("hide");
                    forms.load.phase.import();
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.formulasDt.init();
            this.activitiesDt.init();
            this.sewerGroupDt.init();
            this.phaseDt.init();
        },
        formulasDt: {
            init: function () {
                formulasDatatable = $("#formulas_datatable").DataTable(formulasDtOpts);
                this.events();
            },
            reload: function () {
                formulasDatatable.ajax.reload();
            },
            events: function () {
                formulasDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.activitiesDt.reload(id);
                        forms.load.detail(id);
                    });

                formulasDatatable.on("click",
                    ".btn-sewergroups",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.sewerGroupDt.reload(id);
                        forms.load.sewergroups(id);
                    });

                formulasDatatable.on("click",
                    ".btn-phases",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        formulaId = id;
                        datatables.phaseDt.reload();
                        forms.load.phase.detail();
                    });

                formulasDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                formulasDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La formula con sus actividades y cuadrillas serán eliminadas permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/admin/formulas/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.formulasDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La formula con sus actividades y cuadrillas han sido eliminadas con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la formula con sus actividades y cuadrillas."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        activitiesDt: {
            init: function () {
                activitiesDatatable = $("#activities_datatable").DataTable(activitiesDtOpts);
                this.events();
            },
            reload: function (id) {
                formulaId = id;
                activitiesDatatable.clear().draw();
                activitiesDatatable.ajax.reload();
            },
            events: function () {
                activitiesDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.editActivity(id);
                    });

                activitiesDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La actividad será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/admin/actividades-formula/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.activitiesDt.reload(formulaId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La actividad ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la actividad."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        sewerGroupDt: {
            init: function () {
                sewergroupsDatatable = $("#sewergroups_datatable").DataTable(sewergroupsDtOpts);
                this.events();
            },
            reload: function (id) {
                formulaId = id;
                sewergroupsDatatable.clear().draw();
                sewergroupsDatatable.ajax.reload();
            },
            events: function () {
                sewergroupsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La cuadrilla será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/admin/cuadrillas-formula/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.sewerGroupDt.reload(formulaId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La cuadrilla ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la cuadrilla."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        phaseDt: {
            init: function () {
                phaseDatatable = $("#phases_datatable").DataTable(phaseDtOpts);
            },
            reload: function () {
                phaseDatatable.clear().draw();
                phaseDatatable.ajax.reload();
            },
            events: function () {
                phaseDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La fase se eliminará de la presente formula.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/admin/formulas/fases/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.formulasDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La fase ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la fase de la formula."
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
                    url: _app.parseUrl(`/admin/formulas/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='select_groups']").val(result.group).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/formulas/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Code']").attr("disabled", "disabled");
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Name']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            sewergroups: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/formulas/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#sewergroup_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Code']").attr("disabled", "disabled");
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Name']").attr("disabled", "disabled");
                        $("#sewergroup_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            addSewerGroup: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/cuadrillas-formula/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#add_sewergroup_form");
                        formElements.find("[name='FormulaId']").val(result.formulaId);
                        formElements.find("[name='SewerGroupIds']").val(result.sewerGroupIds);
                        formElements.find("[name='select_sewergroup']").val(result.sewerGroupIds);
                        $(".select2-sewergroups").trigger('change');
                        $("#add_sewergroup_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editActivity: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/actividades-formula/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_activity_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='MeasurementUnitId']").val(result.measurementUnitId);
                        formElements.find("[name='select_unit']").val(result.measurementUnitId).trigger("change");
                        $("#detail_modal").modal("hide");
                        $("#edit_activity_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            phase: {
                detail: function () {
                    $("#phase_modal").modal("show");
                },
                import: function () {
                    $("#phase_modal").modal("hide");
                    $("#phase_import_modal").modal("show");
                }
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/formulas/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.formulasDt.reload();
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
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/formulas/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.formulasDt.reload();
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
            },
            addActivity: function (formElement) {
                $(formElement).find("[name='ProjectFormulaId']").val(formulaId);
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_unit']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/actividades-formula/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activitiesDt.reload(formulaId);
                        $("#add_activity_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_activity_alert_text").html(error.responseText);
                            $("#add_activity_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addSewerGroup: function (formElement) {
                $(formElement).find("[name='ProjectFormulaId']").val(formulaId);
                console.log($(formElement).find("[name='select_sewergroup']").val());
                $(formElement).find("[name='SewerGroupIds']").append($(formElement).find("[name='select_sewergroup']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/cuadrillas-formula/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.sewerGroupDt.reload(formulaId);
                        $("#add_sewergroup_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_sewergroup_alert_text").html(error.responseText);
                            $("#add_sewergroup_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editActivity: function (formElement) {
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_unit']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/actividades-formula/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activitiesDt.reload(formulaId);
                        $("#edit_activity_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_activity_alert_text").html(error.responseText);
                            $("#edit_activity_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            phase: {
                import: function (formElement) {
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
                        url: "/admin/formulas/fases/importar",
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
                        datatables.phaseDt.reload();
                        $("#phase_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#variables_import_alert_text").html(error.responseText);
                            $("#variables_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
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
            },
            addActivity: function () {
                addActivityForm.reset();
                $("#detail_modal").modal("show");
                $("#add_activity_form").trigger("reset");
                $("#add_activity_alert").removeClass("show").addClass("d-none");
            },
            addSewerGroup: function () {
                addSewerGroupForm.reset();
                $("#sewergroup_modal").modal("show");
                $("#add_sewergroup_form").trigger("reset");
                $("#add_sewergroup_alert").removeClass("show").addClass("d-none");
            },
            editActivity: function () {
                editActivityForm.reset();
                $("#detail_modal").modal("show");
                $("#edit_activity_form").trigger("reset");
                $("#edit_activity_alert").removeClass("show").addClass("d-none");
            },
            phase: {
                import: function () {
                    phaseImportForm.resetForm();
                    $("#phase_import_form").trigger("reset");
                    $("#phase_import_alert").removeClass("show").addClass("d-none");
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addActivityForm = $("#add_activity_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addActivity(formElement);
                }
            });

            addSewerGroupForm = $("#add_sewergroup_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addSewerGroup(formElement);
                }
            });

            editActivityForm = $("#edit_activity_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editActivity(formElement);
                }
            });

            sewerForm = $("#sewergroup_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            phaseImportForm = $("#phase_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.phase.import(formElement);
                }
            });
        }
    };

    var modal = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#add_activity_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addActivity();
                });

            $("#add_sewergroup_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addSewerGroup();
                });

            $("#edit_activity_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editActivity();
                });

            $("#phase_import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.phase.import();
                });
        }
    };

    var select2 = {
        init: function () {
            this.sewergroups.init();
            this.measurementUnits.init();
            this.group();
        },
        sewergroups: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        measurementUnits: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/unidades-de-medida`)
                }).done(function (result) {
                    $(".select2-units").select2({
                        data: result
                    });
                })
            }
        },
        group: function () {
            $(".select2-groups").select2({
                allowClear: true
            });
        },
    };

    var events = {
        init: function () {
            $("#importExcelFormat").on("click", function () {
                window.location = _app.parseUrl(`/admin/formulas/fases/modelo-excel`);
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modal.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    Formulas.init();
});