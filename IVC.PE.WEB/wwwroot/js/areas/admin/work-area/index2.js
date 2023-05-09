var area = function () {

    var areaDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/area-trabajo/listar"),
            dataSrc: "",

        },
        columns: [
            {
                title: "Nombre",
                data: "Name"
            },
            {
                title: "Nombre Normalizado",
                data: "NormalizedName"
            },
            {
                tittle: "Int Value",
                data: "IntValue"
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
            areaDatatable = $("#work_area_datatable").Datatable(options);
            this.initEvents();
        },
        reload: function () {
            areaDatatable.ajax.reload();
        },

        initEvents: function () {
            areaDatatable.on("click", ".btn-edit", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.edit(id);
            });

            areaDatatable.on("click", ".btn-delete", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "El usuario será eliminado permanentemente",
                    type: "warning",
                    showCancelButtonn: true,
                    confirmButtonText: "Sí,eliminarlo",
                    confirmButtonClass: "btn-danger",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: _app.parseUrl(`/admin/area-trabajo/eliminar/${id}`),
                                type: "delete",
                                sucess: function (result) {
                                    datatable.reload();
                                    swal.fire({
                                        type: "sucess",
                                        title: "Completado",
                                        text: "El usuario ha sidoi eliminado correctamente",
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
                        })
                    }
                });
            })
        }




    };


    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/area-trabajo/${id}`)
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id'").val(result.id);
                    formElements.find("[name='Name'").val(result.name);
                    formElements.find("[name='NormalizedName'").val(result.normalizedName);
                    formElements.find("[name='IntValue'").val(result.intValue);
                    $("#edit_modal").modal("show");


                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input,select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/area-trabajo/crear"),
                    method: "post",
                    data: data
                }).always(function () {
                    $btn.removeLoader();
                    $(formElemennt).find("input,select").prop("disabled", false);
                }).done(function (result) {
                    datatable.reload();
                    $("#add_modal").modal("hide");
                    _app.show.notification.add.sucess();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#add_alert_text").html(error.responseText);
                        $("#add_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.add.error();
                });
            },
            edit: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit'");
                $btn.addLoader();
                $(formElement).find("input,select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/area-trabajo/editar/${id}`),
                    method: "put",
                    data: data
                }).always(function () {
                    $btn.removeLoader();
                    $(formElement).find("input,select").prop("disabled", false);
                }).done(function (result) {
                    datatable.reload();
                    $("#edit_modal").modal("hide");
                    _app.show.notification.edit.sucess();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#edit_alert_text").html(error.responseText);
                        $("#edit_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.edit.error();
                });
            },

        }
    };

    return {
        init: function () {

            datatable.init();
        }
    };

}();

$(function () {
    area.init();
});