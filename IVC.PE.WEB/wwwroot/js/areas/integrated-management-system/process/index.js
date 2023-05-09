var Proceso = function () {

    var addForm = null;
    var editForm = null;
    var procesoDatatable = null;

    var procesoDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/sistema-de-manejo-integrado/procesos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Proceso",
                data: "processName"
            },
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Jefe de Proceso",
                data: "userName"
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

    var datatables = {
        init: function () {
            this.procesoDt.init();
            this.initEvents();
        },
        procesoDt: {
            init: function () {
                procesoDatatable = $("#procesos_datatable").DataTable(procesoDtOpt);
            },
            reload: function () {
                procesoDatatable.ajax.reload();
            }
        },
        initEvents: function () {
            procesoDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.edit(id);
                });

            procesoDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Estás Seguro?",
                        text: "El Proceso será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/sistema-de-manejo-integrado/procesos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatables.procesoDt.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo técnico ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        })
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el archivo ténico"
                                        });
                                    }
                                });
                            });
                        },

                    })
                });
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/sistema-de-manejo-integrado/procesos/${id}`)
                })
                    .done(function (result) {
                        select2.processes.edit(result.userId);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProcessName']").val(result.processName);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='select_user']").prop("disabled", true);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/sistema-de-manejo-integrado/procesos/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.procesoDt.reload();
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
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/sistema-de-manejo-integrado/procesos/editar/${id}`),
                    method: "put",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                    })
                    .done(function () {
                        datatables.procesoDt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    })
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

    var select2 = {
        init: function () {
            this.processes.init();
        },
        processes: {
            init: function () {
                $(".select2-users").empty();
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            },
            edit: function (id) {
                $("#edit_form .select2-users").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/empleados/${id}`)
                }).done(function (result) {
                    $("#edit_form .select2-users").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };


    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Proceso.init();
});