var WorkAreaItem = function () {

    var itemsDatatable = null;
    var topicId = null;

    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/elementos-de-trabajo/listar"),
            dataSrc: "",
            data: function (d) {
                d.workAreaId = $("#work_area_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Área de Trabajo",
                data: "workArea.name"
            },
            {
                title: "Controlador",
                data: "controller"
            },
            {
                title: "Acción",
                data: "action"
            },
            {
                title: "Padre",
                data: "parent",
                render: function (data, type, row) {
                    if (row.parent) {
                        return row.parent.name;
                    } else { return "No tiene"; }
                }
            },
            {
                title: "Rol",
                data: "roleId",
                render: function (data) {
                    return data || "No asignado";
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
            this.items.init();
        },
        items: {
            init: function () {
                itemsDatatable = $("#items_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                itemsDatatable.ajax.reload();
            },
            initEvents: function () {
                itemsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });
                itemsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El elemento será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/admin/elementos-de-trabajo/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.items.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El elemento ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el elemento"
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

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/elementos-de-trabajo/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Controller']").val(result.controller);
                        formElements.find("[name='Action']").val(result.action);
                        formElements.find("[name='WorkAreaId']").val(result.workAreaId).trigger("change");
                        formElements.find("[name='ParentId']").val(result.parentId).trigger("change");
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
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/elementos-de-trabajo/crear"),
                    method: "post",
                    data: data,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.items.reload();
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
                    url: _app.parseUrl(`/admin/elementos-de-trabajo/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.items.reload();
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

    var select2 = {
        init: function () {
            this.areas.init();
            this.items.init();
            this.roles.init();
        },
        areas: {
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
        items: {
            init: function () {
                $(".select2-work-items").select2({
                    ajax: {
                        url: _app.parseUrl("/select/elementos-de-trabajo"),
                        allowClear: true,
                        dataType: 'json',
                        placeholder: "Selecciona un elemento",
                        //data: function (params) {
                        //    var query = {
                        //        search: params.term,
                        //    }

                        //    // Query parameters will be ?search=[term]&type=public
                        //    return query;
                        //},
                        processResults: function (data) {
                            // Transforms the top-level key of the response object from 'items' to 'results'
                            return {
                                results: data
                            };
                        }
                    }
                });
            }
        },
        roles: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/roles")
                }).done(function (result) {
                    $(".select2-roles").select2({
                        allowClear: true,
                        data: result,
                        placeholder: "Selecciona un rol"
                    }).val(null).trigger("change");
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
            $("#add_form [name='WorkAreaId']").attr("id", "Add_WorkAreaId");
            $("#edit_form [name='WorkAreaId']").attr("id", "Edit_WorkAreaId");
            $("#add_form [name='ParentId']").attr("id", "Add_ParentId");
            $("#edit_form [name='ParentId']").attr("id", "Edit_ParentId");
            $("#add_form [name='RoleId']").attr("id", "Add_RoleId");
            $("#edit_form [name='RoleId']").attr("id", "Edit_RoleId");

            $("input[name='IsItemGroup']").on("change", function (e) {
                let isChecked = $(this).is(":checked")
                $(this).closest("input[name='Controller']").prop("disabled", isChecked);
                $(this).closest("input[name='Action']").prop("disabled", isChecked);
                $(this).closest("select[name='RoleId']").prop("disabled", isChecked);
            });

            $("#work_area_filter").on("change", function () {
                datatable.items.reload();
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
    WorkAreaItem.init();
});