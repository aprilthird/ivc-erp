var BondLoad = function () {

    var employeeDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/empleados/listar"),
            data: function (d) {
                d.workArea = $("#work_area_filter").val();
                d.entryPositionId = $("#entry_position_filter").val();
                d.currentPositionId = $("#current_position_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Fecha Ingreso",
                data: "entryDate"
            },
            {
                title: "Apellido Paterno",
                data: "paternalSurname"
            },
            {
                title: "Apellido Materno",
                data: "maternalSurname"
            },
            {
                title: "Primer Nombre",
                data: "name"
            },
            {
                title: "Segundo Nombre",
                data: "middleName"
            },
            {
                title: "Tipo Documento",
                data: "documentType",
                render: function () { return "DNI"; }
            },
            {
                title: "Documento",
                data: "document"
            },
            {
                title: "Fecha Nacimiento",
                data: "birthDate"
            },
            {
                title: "Teléfono",
                data: "phoneNumber"
            },
            {
                title: "Correo",
                data: "email"
            },
            {
                title: "Área de Trabajo",
                data: "workArea",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.employee.workArea.VALUES);
                }
            },
            {
                title: "Cargo Fase Contractual",
                data: "entryPosition.name"
            },
            {
                title: "Cargo en Obra",
                data: "currentPosition.name"
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

    var datatable = {
        init: function () {
            employeeDatatable = $("#employee_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            employeeDatatable.ajax.reload();
        },
        initEvents: function () {
            employeeDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            employeeDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El empleado será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/recursos-humanos/empleados/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El empleado ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al empleado"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/empleados/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Description']").val(result.description);
                        $("#edit_modal").modal("show");
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
                $(formElement).find("input, textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/recursos-humanos/empleados/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, textarea").prop("disabled", false);
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
                $(formElement).find("input, textarea").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/empleados/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, textarea").prop("disabled", false);
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
                    url: "/recursos-humanos/empleados/importar",
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
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.documentTypes.init();
            this.workAreas.init();
            this.employeePositions.init();
        },
        documentTypes: {
            init: function () {
                $(".select2-document-types").select2();
            }
        },
        workAreas: {
            init: function () {
                $(".select2-work-areas").select2();
            }
        },
        employeePositions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cargos-laborales/1")
                }).done(function (result) {
                    $(".select2-work-positions").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
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
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='DocumentType']").attr("id", "Add_DocumentType");
            $("#edit_form [name='DocumentType']").attr("id", "Edit_DocumentType");
            $("#add_form [name='CurrentPositionId']").attr("id", "Add_CurrentPositionId");
            $("#edit_form [name='CurrentPositionId']").attr("id", "Edit_CurrentPositionId");
            $("#add_form [name='EntryPositionId']").attr("id", "Add_CurrentPositionId");
            $("#edit_form [name='EntryPositionId']").attr("id", "Edit_CurrentPositionId");
            $("#add_form [name='WorkArea']").attr("id", "Add_WorkArea");
            $("#edit_form [name='WorkArea']").attr("id", "Edit_WorkArea");
            //$("#add_modal").on("shown.bs.modal", function () {
            //    $("#Add_RoleIds").select2();
            //});
            //$("#edit_modal").on("shown.bs.modal", function () {
            //    $("#Edit_RoleIds").select2();
            //});
            $("#work_area_filter, #entry_position_filter, #current_position_filter").on("change", function () {
                datatable.reload();
            });

            $("#newEntrySample").on("click", function () {
                window.location = `/recursos-humanos/empleados/importar/excel_nuevos`;
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datepicker.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    BondLoad.init();
});