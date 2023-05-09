var MachineryType = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/equipos-asignados/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Encargado",
                data: "equipmentMachineryAssignedUser.workFrontHead.user.name",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += row.equipmentMachineryAssignedUser.workFrontHead.user.name + " " + row.equipmentMachineryAssignedUser.workFrontHead.user.paternalSurname + " " + row.equipmentMachineryAssignedUser.workFrontHead.user.maternalSurname;
                    return tmp;
                }
            },
            {
                title: "Tipo de Equipo",
                data: "equipmentMachineryType.description"
            },
            {
                title: "Equipo",
                data: "equipmentMachineryType.description",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == "Maquinaria")
                        tmp += row.equipmentMachineryTypeType.description
                    else
                        tmp += row.equipmentMachineryTypeSoft.description;

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
                        text: "El encargado será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/equipos/equipos-asignados/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El encargado ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al encargado"
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
                    url: _app.parseUrl(`/equipos/equipos-asignados/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentMachineryAssignedUserId']").val(result.equipmentMachineryAssignedUserId);
                        formElements.find("[name='select_user']").val(result.equipmentMachineryAssignedUserId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeSoftId']").val(result.equipmentMachineryTypeSoftId);
                        formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeTypeId']").val(result.equipmentMachineryTypeTypeId);
                        formElements.find("[name='select_machinery_type_type']").val(result.equipmentMachineryTypeTypeId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeId']").val(result.equipmentMachineryTypeId);
                        formElements.find("[name='select_machinery']").val(result.equipmentMachineryTypeId).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='EquipmentMachineryAssignedUserId']").val($(formElement).find("[name='select_user']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/equipos-asignados/crear"),
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
                $(formElement).find("[name='EquipmentMachineryAssignedUserId']").val($(formElement).find("[name='select_user']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipos-asignados/editar/${id}`),
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
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='ProjectId']").val(null).trigger("change");

            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='ProjectId']").val(null).trigger("change");
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

    var select2 = {
        init: function () {
            this.styles.init();
            this.users.init();
            this.softs.init();
            this.machineries.init();
            this.machinerys.init();
        },
        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
        },
        users: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/asignados-equipos")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            }
        },
        softs: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-liviano")
                }).done(function (result) {
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            }
        },
        machinerys: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-lista")
                }).done(function (result) {
                    $(".select2-machinerys").select2({
                        data: result
                    });
                });
            }
        },
        machineries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-maquinaria")
                }).done(function (result) {
                    $(".select2-machineries").select2({
                        data: result
                    });
                });
            }
        },
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

            $(".select2-machinerys").on("change", function () {
                var txt = $(".select2-machinerys option:selected").text();
                if (txt.indexOf("Maquinaria") >= 0) {
                    $(".soft_group").attr("hidden", true);
                    $(".type_group").attr("hidden", false);
                } else if (txt.indexOf("Maquinaria") <= 0) {
                    $(".soft_group").attr("hidden", false);
                    $(".type_group").attr("hidden", true);

                }
            });


        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
        }
    };
}();

$(function () {
    MachineryType.init();
});