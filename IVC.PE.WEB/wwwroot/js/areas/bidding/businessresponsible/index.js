﻿var Formula = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/responsables-de-empresa/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Empresas",
                data: "businessName",
            },
            {
                title: "Responsable(s)",
                data: "userNames"
            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.businessId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.businessId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        initEvents: function () {
            mainDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            mainDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El responsable será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/licitaciones/responsables-de-empresa/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El responsable ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el responsable"
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
                    url: _app.parseUrl(`/licitaciones/responsables-de-empresa/${id}`)
                }).done(function (result) {
                    console.log(result);
                    let formElements = $("#edit_form");
                    formElements.find("[name='BusinessId']").val(result.businessId);
                    formElements.find("[name='select_business']").val(result.businessId).trigger("change");
                    formElements.find("[name='Responsibles']").val(result.responsibles.toString());
                    formElements.find("[name='select_users']").val(result.responsibles.toString().split(',')).trigger("change");
                    formElements.find("[name='SendEmail']").val(result.sendEmail.toString());
                    formElements.find("[name='select_sendEmail']").val(result.sendEmail.toString()).trigger("change");
                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='SendEmail']").val($(formElement).find("[name='select_sendEmail']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/licitaciones/responsables-de-empresa/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.add.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='SendEmail']").val($(formElement).find("[name='select_sendEmail']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/responsables-de-empresa/editar`),
                    type: "put",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.edit.success();
                    }).fail(function (error) {
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
                //$("#add_form [name='BusinessId']").val(null).trigger("change");
                //$("#add_form [name='Responsibles']").val(null).trigger("change");
                //$("#add_form [name='SendEmail']").prop("checked", true);
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                //$("#edit_form [name='BusinessId']").val(null).trigger("change");
                //$("#edit_form [name='Responsibles']").val(null).trigger("change");
                //$("#edit_form [name='SendEmail']").prop("checked", true);
            }
        }
    };


    var select2 = {
        init: function () {
            this.businesses.init();
            this.users.init();
            this.sendEmail.init();
        },
        businesses: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empresas")
                }).done(function (result) {
                    $(".select2-businesses").select2({
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
        sendEmail: {
            init: function () {
                $(".select2-sendEmail").select2();
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

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
        }
    }
}();

$(function () {
    Formula.init();
   
});