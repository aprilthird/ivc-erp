var Formulas = function () {


    var equipmentMachineryId = null;

    var formulasDatatable = null;
    var activitiesDatatable = null;
    var detailForm = null;
    var addActivityForm = null;
    var editActivityForm = null;

    var formulasDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/tipo-de-maquinaria/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo de equipos",
                data: "description"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
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
        ],
        buttons: []
    };
    var activitiesDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/actividades-de-maquinaria/listar"),
            data: function (d) {
                d.equipmentMachineryId = equipmentMachineryId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "description"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#detail_modal").modal("hide");
                    $("#add_activity_modal").modal("show");
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.formulasDt.init();
            this.activitiesDt.init();
        },
        formulasDt: {
            init: function () {
                formulasDatatable = $("#main_datatable").DataTable(formulasDtOpts);
                this.events();
            },
            reload: function () {
                formulasDatatable.ajax.reload();
            },
            events: function () {
                formulasDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                formulasDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tipo de equipo menor será eliminada permanentemente",
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
                                        url: _app.parseUrl(`/equipos/tipo-de-maquinaria/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.formulasDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tipo de equipo menor ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tipo de equipo menor"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                formulasDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.activitiesDt.reload(id);
                        forms.load.detail(id);
                    });


            }
        },
        activitiesDt: {
            init: function () {
                activitiesDatatable = $("#activities_datatable").DataTable(activitiesDtOpts);
                this.events();
            },
            reload: function (id) {
                equipmentMachineryId = id;
                activitiesDatatable.clear().draw();
                activitiesDatatable.ajax.reload();
            },
            events: function () {
                activitiesDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.editActivity(id);
                    });

                activitiesDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La actividad será eliminada permanentemente.",
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
                                        url: _app.parseUrl(`/equipos/actividades-de-maquinaria/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.activitiesDt.reload(equipmentMachineryId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La actividad ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la actividad."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },

    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/tipo-de-maquinaria/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Description']").val(result.description);

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/tipo-de-maquinaria/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);;
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='Description']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editActivity: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/actividades-de-maquinaria/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_activity_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentMachineryTypeTypeId']").val(result.equipmentMachineryId);
                        formElements.find("[name='Description']").val(result.description);
                        $("#detail_modal").modal("hide");
                        $("#edit_activity_modal").modal("show");
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
                    url: _app.parseUrl("/equipos/tipo-de-maquinaria/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.formulasDt.reload();
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
                    url: _app.parseUrl(`/equipos/tipo-de-maquinaria/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.formulasDt.reload();
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
            addActivity: function (formElement) {
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val(equipmentMachineryId);
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/actividades-de-maquinaria/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activitiesDt.reload(equipmentMachineryId);
                        $("#add_activity_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_activity_alert_text").html(error.responseText);
                            $("#add_activity_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editActivity: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/actividades-de-maquinaria/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activitiesDt.reload(equipmentMachineryId);
                        $("#edit_activity_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_activity_alert_text").html(error.responseText);
                            $("#edit_activity_alert").removeClass("d-none").addClass("show");
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
            },
            addActivity: function () {
                addActivityForm.reset();
                $("#detail_modal").modal("show");
                $("#add_activity_form").trigger("reset");
                $("#add_activity_alert").removeClass("show").addClass("d-none");
            },
            editActivity: function () {
                editActivityForm.reset();
                $("#detail_modal").modal("show");
                $("#edit_activity_form").trigger("reset");
                $("#edit_activity_alert").removeClass("show").addClass("d-none");
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addActivityForm = $("#add_activity_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addActivity(formElement);
                }
            });


            editActivityForm = $("#edit_activity_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editActivity(formElement);
                }
            });

        }
    };

    var modal = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#add_activity_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addActivity();
                });

            $("#edit_activity_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editActivity();
                });
        }
    };

    var select2 = {
        init: function () {
            this.sewergroups.init();
        },
        sewergroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas`)
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modal.init();
            select2.init();
        }
    };
}();

$(function () {
    Formulas.init();
});