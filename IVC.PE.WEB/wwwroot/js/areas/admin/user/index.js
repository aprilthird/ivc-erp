var user = function () {

    var userDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/usuarios/listar"),
            dataSrc: "",
            data: function (d) {
                d.roleId = $("#role_filter").val();
                d.workAreaId = $("#work_area_filter").val();
                d.workPositionId = $("#work_position_filter").val();
                d.projectId = $("#project_filter").val();
                delete d.columns;
            } 
        },
        columns: [
            {
                title: "Nombres y Apellidos",
                data: "fullName"
            },
            {
                title: "Correo",
                data: "email"
            },
            {
                title: "Teléfono",
                data: "phoneNumber"
            },
            {
                title: "Roles",
                data: "stringRoleNames"
            },
            {
                title: "Área de Trabajo",
                data: "workAreaEntity.intValue",
                render: function (data) {
                    return _app.render.label(data, _app.constants.employee.workArea.VALUES);
                }
            },
            {
                title: "Cargo en Obra",
                data: "workPosition.name",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Multi-Proyecto",
                data: "belongsToMainOffice",
                render: function (data, type, row) {
                    return `<span class="kt-switch kt-switch--icon">
								<label>
									<input class="switch-mainoffice" type="checkbox" data-id="${row.id}" ${(data ? "checked" : "")} name="">
									<span></span>
								</label>
							</span>`;
                }
            },
            {
                title: "Proyectos",
                data: "stringProjectNames"
            },
            {
                title: "Firma",
                data: "signatureUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Firma" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
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

    var datatable = {
        init: function () {
            userDatatable = $("#user_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            userDatatable.ajax.reload();
        },
        initEvents: function () {

            userDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                let url = $btn.data("url");
                let sname = $btn.data("name");
                form.load.pdf(id, url, sname);
            });

            userDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            userDatatable.on("change",
                ".switch-mainoffice",
                function () {
                    let $sw = $(this);
                    let id = $sw.data("id");
                    let checked = $sw.is(":checked");
                    $.ajax({
                        url: _app.parseUrl(`/admin/usuarios/actualizar-estado-oficina/${id}`),
                        method: "put",
                        data: {
                            belongsToMainOffice: checked
                        }
                    })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            _app.show.notification.edit.error();
                            $sw.prop("checked", !checked);
                        });
                });

            userDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El usuario será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/admin/usuarios/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El usuario ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el usuario"
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
            pdf: function (id, url, sname) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/usuarios/${id}`)
                }).done(function (result) {
                    if (result.signatureUrl != null) {

                        if (result.signatureUrl.includes(".pdf")) {
                            var tmp = "";
                            tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                            $("#eye-view-pdf-img").html(tmp);
                            $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.signatureUrl + "&embedded=true");

                        } else if (result.signatureUrl.includes(".jpg") || result.signatureUrl.includes(".jpeg")) {
                            var tmp = "";
                            tmp += `<img id="jpg_view" src="" style="width: 100%;">`
                            $("#eye-view-pdf-img").html(tmp);
                            $("#jpg_view").prop("src", result.signatureUrl);
                            //$("#eye").html("src",result.fileUrl);
                        }
                    }
                    else if (result.signatureUrl == null) {
                        var tmp = "";
                        tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                        $("#eye-view-pdf-img").html(tmp);
                    }

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/usuarios/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='MiddleName']").val(result.middleName);
                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        formElements.find("[name='select_work_area']").val(result.workAreaId).trigger("change");
                        formElements.find("[name='select_work_position']").val(result.workPositionId).trigger("change");
                        formElements.find("[name='WorkRoleId']").val(result.workRoleId).trigger("change");
                        formElements.find("[name='RoleIds']").val(result.roleIds).trigger("change");
                        formElements.find("[name='ProjectIds']").val(result.projectIds).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='WorkAreaId']").val($(formElement).find("[name='select_work_area']").val());
                $(formElement).find("[name='WorkPositionId']").val($(formElement).find("[name='select_work_position']").val());
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
                    url: _app.parseUrl("/admin/usuarios/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
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
                $(formElement).find("[name='WorkAreaId']").val($(formElement).find("[name='select_work_area']").val());
                $(formElement).find("[name='WorkPositionId']").val($(formElement).find("[name='select_work_position']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/usuarios/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
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
            import: {
                data: function (formElement) {
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
                        url: "/admin/usuarios/importar",
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
                files: function (formElement) {
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
                        url: "/control-documentario/cartas-recibidas/importar-archivos",
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
                        $("#import_files_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_files_alert_text").html(error.responseText);
                            $("#import_files_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='RoleIds']").val(null).trigger("change");
                $("#add_form [name='ProjectIds']").val(null).trigger("change");
                $("#add_form [name='WorkPositionId']").val(null).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='RoleIds']").val(null).trigger("change");
                $("#edit_form [name='ProjectIds']").val(null).trigger("change");
                $("#edit_form [name='WorkPositionId']").val(null).trigger("change");
            },
            import: function() {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.roles.init();
            this.workAreas.init();
            this.projects.init();
            this.workPositions.init();
            this.workRoles.init();
        },
        workPositions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cargos-laborales/1")
                }).done(function (result) {
                    $(".select2-work-positions").select2({
                        data: result
                    });
                });
            }
        },
        roles: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/roles")
                }).done(function (result) {
                    $(".select2-roles").select2({
                        data: result
                    });
                });
            }
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                    $(".select2-projects").select2({
                        data: result
                    });
                });
            }
        },
        workAreas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/areas-trabajo")
                }).done(function (result) {
                    $(".select2-work-areas").select2({
                        data: result
                    });
                });
            }
        },
        workRoles: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/roles-de-trabajo")
                }).done(function (result) {
                    $(".select2-work-roles").select2({
                        data: result
                    });
                });
            }
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
                    form.submit.import.data(formElement);
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
                    form.reset.import.data();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='select_rol']").attr("id", "Add_RoleIds");
            $("#edit_form [name='select_rol']").attr("id", "Edit_RoleIds");
            $("#add_form [name='select_project']").attr("id", "Add_ProjectIds");
            $("#edit_form [name='select_project']").attr("id", "Edit_ProjectIds");
            $("#add_form [name='select_work_area']").attr("id", "Add_WorkArea");
            $("#edit_form [name='select_work_area']").attr("id", "Edit_WorkArea");
            $("#add_form [name='select_work_position']").attr("id", "Add_WorkPositionId");
            $("#edit_form [name='select_work_position']").attr("id", "Edit_WorkPositionId");
            $("#add_form [name='WorkRoleId']").attr("id", "Add_WorkRoleId");
            $("#edit_form [name='WorkRoleId']").attr("id", "Edit_WorkRoleId");

            $("#edit_form [name='Password']").removeAttr("required");
            $("#edit_form [name='RepeatPassword']").removeAttr("required");
            $("#add_modal").on("shown.bs.modal", function () {
                $("#Add_RoleIds").select2();
                $("#Add_ProjectIds").select2();
            });
            $("#add_modal").on("show.bs.modal", function () {
                $("#add_form [name='Email']").val(null);
                $("#add_form [name='Password']").val(null);
            
            });
            $("#edit_modal").on("shown.bs.modal", function () {
                $("#Edit_RoleIds").select2();
                $("#Edit_ProjectIds").select2();
                $("#edit_form [name='Password']").val(null);
            });
            $("#role_filter").on("change", function () {
                datatable.reload();
            });
            $("#work_area_filter").on("change", function () {
                datatable.reload();
            });
            $("#work_position_filter").on("change", function () {
                datatable.reload();
            });

            $("#project_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            select2.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    user.init();
});