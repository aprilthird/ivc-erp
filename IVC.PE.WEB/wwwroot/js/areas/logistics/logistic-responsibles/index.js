var LogResponsibles = function () {

    var ResponsibleDatatable = null;

    var addForm = null;

    var resOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/responsables/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Proyecto",
                data: "project.abbreviation"
            },
            {
                title: "Autorización Primaria Pre-Requerimientos",
                data: "preRequestAuthNames"
            },
            {
                title: "Autorización secundaria de Pre-Requerimientos",
                data: "secondaryPreRequestAuthNames"
            },
            {
                title: "Informar Aprobación de Pre-Requerimientos",
                data: "preRequestOkNames"
            },
            {
                title: "Informar Rechazo de Pre-Requerimientos",
                data: "preRequestFailNames"
            },
            {
                title: "Revisión Requerimientos",
                data: "requestReviewNames"
            },
            {
                title: "Autorización Requerimientos",
                data: "requestAuthNames"
            },
            {
                title: "Informar Aprobación de Requerimientos",
                data: "requestOkNames"
            },
            {
                title: "Informar Rechazo de Requerimientos",
                data: "requestFailNames"
            },
            {
                title: "Revisión Órdenes",
                data: "orderReviewNames"
            },
            {
                title: "Autorización Órdenes",
                data: "orderAuthNames"
            },
            {
                title: "Informar Aprobación de Órdenes",
                data: "orderOkNames"
            },
            {
                title: "Informar Rechazo de Órdenes",
                data: "orderFailNames"
            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.projectId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.projectId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.resDt.init();
        },
        resDt: {
            init: function () {
                ResponsibleDatatable = $("#log_responsibles_datatable").DataTable(resOpts);
                this.events();
            },
            reload: function () {
                ResponsibleDatatable.ajax.reload();
            },
            events: function () {
                ResponsibleDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                ResponsibleDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "Los responsables serán eliminados permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlos",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/responsables/eliminar`),
                                        type: "delete",
                                        data: {
                                            id: id
                                        },
                                        success: function (result) {
                                            datatables.resDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "Los responsables han sido eliminados con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar a los responsables."
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
                    url: _app.parseUrl(`/logistica/responsables/${id}`)
                }).done(function (result) {
                    let formElements = $("#add_form");
                    formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                    formElements.find("[name='RequestAuthIds']").val(result.requestAuthIds).trigger("change");
                    formElements.find("[name='RequestReviewIds']").val(result.requestReviewIds).trigger("change");
                    formElements.find("[name='RequestOkIds']").val(result.requestOkIds).trigger("change");
                    formElements.find("[name='RequestFailIds']").val(result.requestFailIds).trigger("change");
                    formElements.find("[name='OrderAuthIds']").val(result.orderAuthIds).trigger("change");
                    formElements.find("[name='OrderReviewIds']").val(result.orderReviewIds).trigger("change");
                    formElements.find("[name='OrderOkIds']").val(result.orderOkIds).trigger("change");
                    formElements.find("[name='OrderFailIds']").val(result.orderFailIds).trigger("change");
                    formElements.find("[name='PreRequestAuthIds']").val(result.preRequestAuthIds).trigger("change");
                    formElements.find("[name='PreRequestOkIds']").val(result.preRequestOkIds).trigger("change");
                    formElements.find("[name='PreRequestFailIds']").val(result.preRequestFailIds).trigger("change");
                    formElements.find("[name='SecondaryPreRequestAuthIds']").val(result.secondaryPreRequestAuthIds).trigger("change");
                    $("#add_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/logistica/responsables/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatables.resDt.reload();
                        _app.show.notification.add.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
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
                //$(".select2-projects").val('').trigger("change");
                $(".select2-users").val('').trigger("change");
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
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });
        }
    };

    var select2 = {
        init: function () {
            this.users.init();
            this.projects.init();
        },
        users: {
            init: function () {
                var pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/usuarios-proyecto?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-users").select2({
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
                });
            }
        }
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    LogResponsibles.init();
});