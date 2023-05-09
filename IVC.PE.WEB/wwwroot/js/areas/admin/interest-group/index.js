var InterestGroup = function () {

    var interestGroupDatatable = null;
    var prevAddForm = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/grupos-de-interes/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            } 
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Proyecto",
                data: "project.abbreviation"
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
            interestGroupDatatable = $("#interest_group_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            interestGroupDatatable.ajax.reload();
        },
        initEvents: function () {
            interestGroupDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            interestGroupDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El grupo de interés será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/admin/grupos-de-interes/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El grupo de interés ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el grupo de interés"
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
                    url: _app.parseUrl(`/admin/grupos-de-interes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='UserIds']").val(result.userIds).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            prevAdd: function (formElement) {
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("select").prop("disabled", true);
                let projectId = $("#prev_project_id").val();
                let workArea = $("#prev_work_area").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/grupos-de-interes/usuarios?projectId=${projectId}&workArea=${workArea}`)
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("select").prop("disabled", false);
                    })
                    .done(function (result) {
                        $("#prev_add_modal").modal("hide");
                        $("#Add_UserIds").val(result).trigger("change");
                        $("#add_modal").modal("show");
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#prev_add_alert_text").html(error.responseText);
                            $("#prev_add_alert").removeClass("d-none").addClass("show");
                        }
                    });
            },
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/grupos-de-interes/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                    url: _app.parseUrl(`/admin/grupos-de-interes/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
            }
        },
        reset: {
            prevAdd: function () {
                prevAddForm.resetForm();
                $("#prev_add_form").trigger("reset");
                $("#prev_project_id").prop("selectedIndex", 0).trigger("change");
                $("#prev_work_area").prop("selectedIndex", 0).trigger("change");
            },
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='ProjectId']").val(null).trigger("change");
                $("#add_form [name='UserIds']").val(null).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='ProjectId']").val(null).trigger("change");
                $("#edit_form [name='UserIds']").val(null).trigger("change");
            }
        }
    };

    var select2 = {
        init: function () {
            this.projects.init();
            this.users.init();
            this.workAreas.init();
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result,
                        placeholder: "Proyecto"
                    });
                });
            }
        },
        users: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/usuarios")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            }
        },
        workAreas: {
            init: function () {
                $(".select2-work-areas").select2();
            }
        }
    };

    var validate = {
        init: function () {
            prevAddForm = $("#prev_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.prevAdd(formElement);
                }
            });

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
        }
    };

    var modals = {
        init: function () {
            $("#prev_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.prevAdd();
                });

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
            $("#add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");
            $("#add_form [name='UserIds']").attr("id", "Add_UserIds");
            $("#edit_form [name='UserIds']").attr("id", "Edit_UserIds");
            $("#add_modal").on("shown.bs.modal", function () {
                $("#Add_ProjectId").select2();
                $("#Add_UserIds").select2();
            });
            $("#edit_modal").on("shown.bs.modal", function () {
                $("#Edit_ProjectId").select2();
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
    InterestGroup.init();
});