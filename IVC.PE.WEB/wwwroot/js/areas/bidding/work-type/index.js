﻿var doctype = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/tipos-de-obra/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name",
            },
            {
                title: "Color",
                data: "pillColor",
                render: function (data, type, row) {
                    if (data === 0) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${row.pillColorStr}</span></label></span>`;
                    }
                    if (data === 1) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${row.pillColorStr}</span></label></span>`;
                    }
                    if (data === 2) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${row.pillColorStr}</span></label></span>`;
                    }
                    if (data === 3) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${row.pillColorStr}</span></label></span>`;
                    }
                    if (data === 4) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${row.pillColorStr}</span></label></span>`;
                    }
                    if (data === 5) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${row.pillColorStr}</span></label></span>`;
                    }
                }

            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
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
                        text: "El tipo de obra será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/licitaciones/tipos-de-obra/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El tipo de obra ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar El tipo de Obra"
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
                    url: _app.parseUrl(`/licitaciones/tipos-de-obra/${id}`)
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id']").val(result.id);
                    formElements.find("[name='Name']").val(result.name);

                    formElements.find("[name='PillColor']").val(result.pillColor);
                    formElements.find("[name='select_style']").val(result.pillColor).trigger("change");

                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='PillColor']").val($(formElement).find("[name='select_style']").val());

                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/licitaciones/tipos-de-obra/crear"),
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
                $(formElement).find("[name='PillColor']").val($(formElement).find("[name='select_style']").val());

                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/tipos-de-obra/editar/${id}`),
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
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='IsDirectCost']").prop("checked", true);
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='IsDirectCost']").prop("checked", true);
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
        }
    }
}();

$(function () {
    doctype.init();
});