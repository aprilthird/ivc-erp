var SewerGroup = function () {

    var sewerGroupDatatable = null;
    var habilitationDatatable = null;
    var periodDatatable = null;

    var addForm = null;
    var editForm = null;
    var importForm = null;
    var habilitationAddForm = null;
    var periodAddForm = null;


    var tmpId = null;
    var sewerGroupId = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/admin/cuadrillas/listar"),
            data: function (d) {
                d.workComponent = $("#work_component_filter").val();
                d.workStructure = $("#work_structure_filter").val();
                d.destination = $("#destination_filter").val();
                delete d.columns;
            }
        },
        columns: [
            //{
            //    title: "Código",
            //    data: "code"
            //},
            {
                title: "Nombre",
                data: "code"
            },
            {
                title: "Jefe de Frente",
                data: "workFrontHead.user.fullName",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Componente de Obra",
                data: "workComponent",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.workComponent.VALUES);
                }
            },
            {
                title: "Estructura",
                data: "workStructure",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.workStructure.VALUES);
                }
            },
            {
                title: "Grupo de Destino",
                data: "destination",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.destination.VALUES);
                }
            },
            {
                title: "Colaborador",
                data: "projectCollaborator.provider.businessName",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Encargado",
                data: "employeeWorkerName",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Tramos",
                data: null,
                render: function (data, type, row) {
                    return ` `;
                }
            },
            {
                title: "Activo",
                data: "isActive",
                render: function (data, type, row) {
                    return `<span class="kt-switch kt-switch--icon">
								<label>
									<input class="switch-active" type="checkbox" data-id="${row.id}" ${(data ? "checked" : "")} name="">
									<span></span>
								</label>
							</span>`;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-habilitation">`;
                    tmp += `<i class="fa fa-map-marked"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };
    var optionsNew = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/cuadrillas/listar"),
            data: function (d) {
                d.workComponent = $("#work_component_filter").val();
                d.workStructure = $("#work_structure_filter").val();
                d.destination = $("#destination_filter").val();
                d.isActive = $("#active_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre",
                data: "code"
            },
            {
                title: "Jefe de Frente",
                data: "workFrontHead",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Componente de Obra",
                data: "workComponent",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.workComponent.VALUES);
                }
            },
            {
                title: "Estructura",
                data: "workStructure",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.workStructure.VALUES);
                }
            },
            {
                title: "Grupo de Destino",
                data: "destination",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.group.destination.VALUES);
                }
            },
            {
                title: "Colaborador",
                data: "providerBusinessName"
            },
            {
                title: "Encargado",
                data: "responsable"
            },
            {
                title: "Activo",
                data: "isActive",
                render: function (data, type, row) {
                    return (data == true) ?
                        '<span class="kt-badge kt-badge--success kt-badge--inline">Activo</span>' :
                        '<span class="kt-badge kt-badge--danger kt-badge--inline">Inactivo</span>';
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-habilitation">`;
                    tmp += `<i class="fa fa-map-marked"></i></button> `;
                    tmp += `<button data-id="${row.id}" data-name="${row.code}" class="btn btn-secondary btn-sm btn-icon btn-period">`;
                    tmp += `<i class="fa fa-calendar-alt"></i></button>`;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (!row.isActive) {
                        tmp += `<button data-id="${row.periodId}" data-name="${row.code}" class="btn btn-success btn-sm btn-icon btn-new-period" data-toggle="tooltip" title="Nueva Vigencia">`;
                        tmp += `<i class="fa fa-calendar-plus"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };
    var habilitationOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/habilitaciones-cuadrilla/listar"),
            data: function (d) {
                d.sewerGroupId = sewerGroupId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "projectHabilitation.locationCode"
            },
            {
                title: "Habilitación",
                data: "projectHabilitation.description"
            },
            {
                title: "Opciones",
                width: "10%",
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
                    $("#habilitation_modal").modal("hide");
                    form.load.habilitationAdd(sewerGroupId);
                }
            }
        ]
    };
    var periodOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/cuadrillas/periodos/listar"),
            data: function (d) {
                d.sewerGroupId = sewerGroupId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Jefe de Frente",
                data: "workFrontHead.user.fullName"
            },
            {
                title: "Inicio Vigencia",
                data: "dateStart"
            },
            {
                title: "Fin Vigencia",
                data: "dateEnd"
            },
            {
                title: "Carpeta",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ],
        buttons: [
        ]
    };

    var datatable = {
        init: function () {
            sewerGroupDatatable = $("#sewer_group_datatable").DataTable(optionsNew);
            this.initEvents();
            this.habilitationDt.init();
            this.periodDt.init();
        },
        reload: function () {
            sewerGroupDatatable.ajax.reload();
        },
        initEvents: function () {
            sewerGroupDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            sewerGroupDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La cuadrilla será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/admin/cuadrillas/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La cuadrilla ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la cuadrilla"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            sewerGroupDatatable.on("click",
                ".btn-habilitation",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.habilitation(id);
                });

            sewerGroupDatatable.on("click",
                ".btn-period",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let name = $btn.data("name");
                    $("#period_sgname").html(name);
                    form.load.period(id);
                });

            sewerGroupDatatable.on("click",
                ".btn-new-period",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let name = $btn.data("name");
                    form.load.periodAdd(id, name);
                });
        },
        habilitationDt: {
            init: function () {
                habilitationDatatable = $("#habilitations_datatable").DataTable(habilitationOpts);
                this.events();
            },
            reload: function (id) {
                sewerGroupId = id;
                habilitationDatatable.clear().draw();
                habilitationDatatable.ajax.reload();
            },
            events: function () {
                habilitationDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La habilitación será eliminada permanentemente",
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
                                        url: _app.parseUrl(`/admin/habilitaciones-cuadrilla/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.habilitationDt.reload(sewerGroupId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La habilitación ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la habilitación"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        periodDt: {
            init: function () {
                periodDatatable = $("#periods_datatable").DataTable(periodOpts);
            },
            reload: function (id) {
                sewerGroupId = id;
                periodDatatable.clear().draw();
                periodDatatable.ajax.reload();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/cuadrillas/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Period.Id']").val(result.period.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Period.WorkFrontIds']").val(result.period.workFrontIds).trigger("change");
                        formElements.find("[name='Period.WorkFrontHeadId']").val(result.period.workFrontHeadId).trigger("change");
                        formElements.find("[name='Period.WorkComponent']").val(result.period.workComponent).trigger("change");
                        formElements.find("[name='Period.WorkStructure']").val(result.period.workStructure).trigger("change");
                        formElements.find("[name='Period.Destination']").val(result.period.destination).trigger("change");
                        formElements.find("[name='Period.ForemanId']").val(result.period.foremanId).trigger("change");
                        formElements.find("[name='Period.DateStart']").datepicker("setDate", result.period.dateStart);
                        formElements.find("[name='Period.DateEnd']").datepicker("setDate", result.period.dateEnd);

                        formElements.find("[name='Period.ProviderId']").val(result.period.providerId).trigger("change");
                        $(".select2-providers").trigger("change");
                        select2.projectCollaborators.reloadEdit(result.period.providerId, result.period.projectCollaboratorId);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            habilitation: function (id) {
                datatable.habilitationDt.reload(id);
                $("#habilitation_modal").modal("show");
            },
            habilitationAdd: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/habilitaciones-cuadrilla/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#habilitation_add_form");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        formElements.find("[name='ProjectHabilitationIds']").val(result.habilitationIds);
                        formElements.find("[name='select_habilitation']").val(result.habilitationIds);
                        $(".select2-habilitations").trigger('change');
                        $("#habilitation_add_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            period: function (id) {
                datatable.periodDt.reload(id);
                $("#period_modal").modal("show");
            },
            periodAdd: function (id, name) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/cuadrillas/periodos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#period_add_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        formElements.find("[name='WorkFrontIds']").val(result.workFrontIds).trigger("change");
                        formElements.find("[name='WorkFrontHeadId']").val(result.workFrontHeadId).trigger("change");
                        formElements.find("[name='WorkComponent']").val(result.workComponent).trigger("change");
                        formElements.find("[name='WorkStructure']").val(result.workStructure).trigger("change");
                        formElements.find("[name='Destination']").val(result.destination).trigger("change");
                        formElements.find("[name='ForemanId']").val(result.foremanId).trigger("change");
                        //formElements.find("[name='DateStart']").datepicker("setDate", result.dateStart);
                        //formElements.find("[name='DateEnd']").datepicker("setDate", result.dateEnd);

                        formElements.find("[name='ProviderId']").val(result.providerId).trigger("change");
                        $(".select2-providers").trigger("change");
                        select2.projectCollaborators.reloadEdit(result.providerId, result.projectCollaboratorId);

                        $("#new_period_sgname").html(name);
                        $("#period_add_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
            
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/cuadrillas/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $("#Add_Destination").trigger("change");
                    })
                    .done(function (result) {
                        datatable.reload();
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
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/cuadrillas/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $("#Edit_Destination").trigger("change");
                    })
                    .done(function (result) {
                        datatable.reload();
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
                    url: "/admin/cuadrillas/importar",
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
                    datatable.reload();
                    $("#import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            habilitationAdd: function (formElement) {
                $(formElement).find("[name='SewerGroupId']").val(sewerGroupId);
                console.log($(formElement).find("[name='select_habilitation']").val());
                $(formElement).find("[name='HabilitationIds']").append($(formElement).find("[name='select_habilitation']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/habilitaciones-cuadrilla/crear"),
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
                        datatable.habilitationDt.reload(sewerGroupId);
                        $("#habilitation_add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#habilitation_add_alert_text").html(error.responseText);
                            $("#habilitation_add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            periodAdd: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/cuadrillas/periodos/crear"),
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
                        datatable.reload();
                        $("#period_add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#period_add_alert_text").html(error.responseText);
                            $("#period_add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("Add_WorkFrontId").val(null).trigger("change");
                $("Add_WorkFrontHeadId").val(null).trigger("change");
                $("#Add_Destination").prop("selectedIndex", 0).trigger("change");
                $("#Add_WorkComponent").prop("selectedIndex", 0).trigger("change");
                $("#Add_WorkStructure").prop("selectedIndex", 0).trigger("change");
                $("#Add_ProviderId").val(null).trigger("change");
                $("#Add_ProjectCollaboratorId").val(null).trigger("change");
                $("#Add_ForemanId").val(null).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("Edit_WorkFrontId").val(null).trigger("change");
                $("Edit_WorkFrontHeadId").val(null).trigger("change");
                $("#Edit_Destination").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WorkComponent").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WorkStructure").prop("selectedIndex", 0).trigger("change");
                $("#Edit_ProviderId").val(null).trigger("change");
                $("#Edit_ProjectCollaboratorId").val(null).trigger("change");
                $("#Edit_ForemanId").val(null).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            habilitationAdd: function () {
                habilitationAddForm.resetForm();
                $("#habilitation_modal").modal("show");
                $("#habilitation_add_form").trigger("reset");
                $("#habilitation_add_alert").removeClass("show").addClass("d-none");
            },
            periodAdd: function () {
                periodAddForm.reset();
                $("#period_add_form").trigger("reset");
                $("#period_add_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.destination.init();
            this.workComponents.init();
            this.workStructures.init();
            this.workFronts.init();
            this.workFrontHeads.init();
            this.providers.init();
            this.workers.init();
            this.projectHabilitations.init();
            this.actives.init();
        },
        destination: {
            init: function () {
                $(".select2-destinations")
                    .on("change", function () {
                        let id = $(this).attr("id");
                        let groupId = id.replace("Destination", "ProviderId");
                        let collaboratorId = id.replace("Destination", "ProjectCollaboratorId");
                        let workerId = id.replace("Destination", "ForemanId");
                        let val = parseInt($(this).val());
                        if (val === _app.constants.sewer.group.destination.LOCAL) {
                            $(`#${groupId}`).prop("disabled", true);
                            $(`#${collaboratorId}`).prop("disabled", true);
                            $(`#${workerId}`).prop("disabled", false);
                        }
                        if (val === _app.constants.sewer.group.destination.COLLABORATOR) {
                            $(`#${groupId}`).prop("disabled", false);
                            $(`#${collaboratorId}`).prop("disabled", false);
                            $(`#${workerId}`).prop("disabled", true);
                        }
                    }).select2();
                $("#Add_Destination").trigger("change");
                $("#Edit_Destination").trigger("change");
            }
        },
        workComponents: {
            init: function () {
                $(".select2-work-components").select2();
            }
        },
        workStructures: {
            init: function () {
                $(".select2-work-structures").select2();
            }
        },
        workFronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes")
                }).done(function (result) {
                    $(".select2-work-fronts").select2({
                        data: result,
                        allowClear: true
                    }).val(null).trigger("change");
                });
            }
        },
        workFrontHeads: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-work-front-heads").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores?familyCode=01020&groupCode=200")
                }).done(function (result) {
                    $(".select2-providers")
                        .select2({
                            data: result,
                            allowClear: true
                        }).val(null).trigger("change");
                });
            }
        },
        projectCollaborators: {
            init: function () {
                $(".select2-project-collaborators").select2();
            },
            reload: function(pId) {
                $.ajax({
                    url: _app.parseUrl(`/select/colaboradores?providerId=${pId}`)
                }).done(function (result) {
                    $(".select2-project-collaborators").select2({
                        data: result,
                        allowClear: true
                    });
                });
            },
            reloadEdit: function (pId, cId) {
                $.ajax({
                    url: _app.parseUrl(`/select/colaboradores?providerId=${pId}`)
                }).done(function (result) {
                    $(".select2-project-collaborators").select2({
                        data: result,
                        allowClear: true
                    }).val(cId).trigger("change");
                });
            }
        },
        workers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/obreros-empleados")
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result,
                        allowClear: true
                    }).val(null).trigger("change");
                });
            }
        },
        projectHabilitations: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/habilitaciones-proyecto`)
                }).done(function (result) {
                    $(".select2-habilitations").select2({
                        data: result
                    });
                });
            }
        },
        actives: {
            init: function () {
                $(".select2-actives").select2();
            }
        },
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            habilitationAddForm = $("#habilitation_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.habilitationAdd(formElement);
                }
            });

            periodAddForm = $("#period_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.periodAdd(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });

            $("#habilitation_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.habilitationAdd();
                });

            $("#period_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.periodAdd();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");
            $("#add_form [name='Period.WorkFrontIds']").attr("id", "Add_WorkFrontIds");
            $("#edit_form [name='Period.WorkFrontIds']").attr("id", "Edit_WorkFrontIds");
            $("#add_form [name='Period.WorkFrontHeadId']").attr("id", "Add_WorkFrontHeadId");
            $("#edit_form [name='Period.WorkFrontHeadId']").attr("id", "Edit_WorkFrontHeadId");
            $("#add_form [name='Period.Destination']").attr("id", "Add_Destination");
            $("#edit_form [name='Period.Destination']").attr("id", "Edit_Destination");
            $("#add_form [name='Period.WorkComponent']").attr("id", "Add_WorkComponent");
            $("#edit_form [name='Period.WorkComponent']").attr("id", "Edit_WorkComponent");
            $("#add_form [name='Period.WorkStructure']").attr("id", "Add_WorkStructure");
            $("#edit_form [name='Period.WorkStructure']").attr("id", "Edit_WorkStructure");
            $("#add_form [name='Period.ProviderId']").attr("id", "Add_ProviderId");
            $("#edit_form [name='Period.ProviderId']").attr("id", "Edit_ProviderId");
            $("#add_form [name='Period.ProjectCollaboratorId']").attr("id", "Add_ProjectCollaboratorId");
            $("#edit_form [name='Period.ProjectCollaboratorId']").attr("id", "Edit_ProjectCollaboratorId");
            $("#add_form [name='Period.ForemanId']").attr("id", "Add_ForemanId");
            $("#edit_form [name='Period.ForemanId']").attr("id", "Edit_ForemanId");
            $("#work_component_filter, #work_structure_filter, #destination_filter, #active_filter").on("change", function () {
                datatable.reload();
            });

            $(".select2-providers").on("change", function () {
                select2.projectCollaborators.reload(this.value);
            });

            $("#dwnImportFormat").on("click", function () {
                window.location = _app.parseUrl(`/admin/cuadrillas/importar-formato`);
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    SewerGroup.init();
});