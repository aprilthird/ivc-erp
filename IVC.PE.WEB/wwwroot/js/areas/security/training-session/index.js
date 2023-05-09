var TrainingSession = function () {

    var sessionDatatable = null;
    var resultDatatable = null;

    var addForm = null;
    var editForm = null;

    var sessionOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/capacitaciones/sesiones/listar"),
            data: function (d) {
                d.trainingCategoryId = $("#category_filter").val();
                d.trainingTopicId = $("#topic_filter").val();
                d.userId = $("#user_filter").val();
                d.workFrontId = $("#workfront_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Fecha",
                data: "sessionDate"
            },
            {
                title: "Frente",
                data: "workFront.code",
            },
            {
                title: "Tutor",
                data: "user.fullName"
            },
            {
                title: "Categoría",
                data: "trainingTopic.trainingCategory.name"
            },
            {
                title: "Tema",
                data: "trainingTopic.name"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-warning btn-sm btn-icon btn-details" data-toggle="tooltip" title="Detalle">`;
                    tmp += `<i class="fa fa-list"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var resultOpts = {
        responsive: true,
        ajax: {
            url: "/",
            data: function (d) {
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Asistente",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    if (row.workerId) return row.worker.fullName;
                    if (row.employeeId) return row.employee.fullName;
                    return "";
                }
            },
            {
                title: "Documento",
                data: null,
                render: function (data, type, row) {
                    if (row.workerId) return `${row.worker.documentTypeStr} - ${row.worker.document}`;
                    if (row.employeeId) return `${row.employee.documentTypeStr} - ${row.employee.document}`;
                    return "";
                }
            },
            {
                title: "Resultado",
                data: null,
                render: function (data, type, row) {
                    return `<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--${row.trainingResultStatus.colorClass}">${row.trainingResultStatus.colorStr}</span>`;
                }
            },
            {
                title: "Observaciones",
                data: "observation"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    //tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    //tmp += `<i class="fa fa-edit"></i></button> `;
                    //if (row.isActive == true) {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-warning btn-sm btn-icon btn-cease" data-toggle="tooltip" title="Cesar">`;
                    //    tmp += `<i class="fa fa-calendar-times"></i></button>`;
                    //} else {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-success btn-sm btn-icon btn-new-entry" data-toggle="tooltip" title="Reingreso">`;
                    //    tmp += `<i class="fa fa-calendar-plus"></i></button>`;
                    //}
                    //tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    //tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };


    var datatables = {
        init: function () {
            this.sessions.init();
        },
        sessions: {
            init: function () {
                sessionDatatable = $("#sessions_datatable").DataTable(sessionOpts);
                this.events();
            },
            reload: function () {
                sessionDatatable.ajax.reload();
            },
            events: function () {
                sessionDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });
                sessionDatatable.on("click",
                        ".btn-details",
                        function () {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            form.load.details(id);
                        });
                sessionDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La sesión será eliminada permanentemente",
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
                                        url: _app.parseUrl(`/seguridad/capacitaciones/sesiones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.sessions.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La sesión ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la sesión"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }   
        },
        results: {
            init: function (id) {
                resultOpts.ajax.url = _app.parseUrl(`/seguridad/capacitaciones/sesiones/${id}/resultados/listar`);
                resultDatatable = $("#results_datatable").DataTable(resultOpts);
            },
            reload: function () {
                resultDatatable.ajax.reload();
            },
            destroy: function () {
                resultDatatable.destroy();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/seguridad/capacitaciones/sesiones/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='TrainingCategoryId']").val(result.trainingCategoryId).trigger("change");
                        formElements.find("[name='TrainingTopicId']").val(result.trainingTopicId).trigger("change");
                        formElements.find("[name='UserId']").val(result.userId).trigger("change");
                        formElements.find("[name='WorkFrontId']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='SessionDate']").datepicker("setDate", result.sessionDate).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            details: function (id) {
                datatables.results.init(id);
                $("#details_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/seguridad/capacitaciones/sesiones/crear"),
                    method: "post",
                    data: data,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.sessions.reload();
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
                    url: _app.parseUrl(`/seguridad/capacitaciones/sesiones/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.sessions.reload();
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
            details: function () {

            }
        }
    }

    var select2 = {
        init: function () {
            this.categories.init();
            this.topics.init();
            this.users.init();
            this.workfronts.init();
        },
        categories: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/categorias-de-capacitaciones")
                }).done(function (result) {
                    $(".select2-categories").select2({
                        data: result
                    });
                });
            }
        },
        topics: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/temas-de-capacitaciones")
                }).done(function (result) {
                    $(".select2-topics").select2({
                        data: result
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
        workfronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes")
                }).done(function (result) {
                    $(".select2-workfronts").select2({
                        data: result
                    });
                });
            }
        }
    }

    var modal = {
        init: function () {
            $("#details_modal").on("hidden.bs.modal",
                function () {
                    form.reset.details();
                    datatable.results.destroy();
                });
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
        }
    };

    var datepicker = {
        init: function () {
            $("input [name='SessionDate']").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='TrainingCategoryId']").attr("id", "Add_TrainingCategoryId");
            $("#edit_form [name='TrainingCategoryId']").attr("id", "Edit_TrainingCategoryId");
            $("#add_form [name='TrainingTopicId']").attr("id", "Add_TrainingTopicId");
            $("#edit_form [name='TrainingTopicId']").attr("id", "Edit_TrainingTopicId");
            $("#add_form [name='UserId']").attr("id", "Add_UserId");
            $("#edit_form [name='UserId']").attr("id", "Edit_UserId");
            $("#add_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#category_filter, #topic_filter, #user_filter, #workfront_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatables.init();
            select2.init();
            datepicker.init();
            validate.init();
        }
    }
}();

$(function () {
    TrainingSession.init();
});