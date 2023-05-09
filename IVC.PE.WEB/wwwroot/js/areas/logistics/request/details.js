var RequestItem = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/logistica/requerimientos/${requestId}/detalles/listar`),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Material",
                data: "budgetInput.fullDescription"
            },
            {
                title: "UND",
                data: "budgetInput.measurementUnit.abbreviation"
            },
            {
                title: "Metrado",
                data: "measure",
                render: _app.render.measure
            },
            {
                title: "Fase",
                data: "projectPhase.fullDescription"
            },
            {
                title: "Grupo",
                data: "supplyGroup.fullName"
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
                        text: "El elemento será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/logistica/requerimientos/${requestId}/detalles/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
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
                                            text: "Ocurrió un error al intentar eliminar al elemento"
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
                    url: _app.parseUrl(`/logistica/requerimientos/${requestId}/detalles/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Measure']").val(result.measure);
                        formElements.find("[name='Observations']").val(result.observations);
                        formElements.find("[name='BudgetInputId']").val(result.budgetInputId).trigger("change");
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='SupplyGroupId']").val(result.supplyGroupId);
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
                    url: _app.parseUrl(`/logistica/requerimientos/${requestId}/detalles/crear`),
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
                    url: _app.parseUrl(`/logistica/requerimientos/${requestId}/detalles/editar/${id}`),
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
                $("#Add_BudgetInputId").prop("selectedIndex", 0).trigger("change");
                $("#Add_ProjectPhaseId").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_BudgetInputId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_ProjectPhaseId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            }
        }
    };

    var select2 = {
        init: function () {
            this.budgetInputs.init();
            this.projectPhases.init();
            this.supplyGroups.init();
        },
        budgetInputs: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budget-inputs").select2({
                        data: result
                    });
                });
            }
        },
        projectPhases: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto")
                }).done(function (result) {
                    $(".select2-project-phases").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
                        data: result
                    });
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='BudgetInputId']").attr("id", "Add_BudgetInputId");
            $("#edit_form [name='BudgetInputId']").attr("id", "Edit_BudgetInputId");
            $("#add_form [name='ProjectPhaseId']").attr("id", "Add_ProjectPhaseId");
            $("#edit_form [name='ProjectPhaseId']").attr("id", "Edit_ProjectPhaseId");
            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");

            $("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatable.reload();
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
            datepicker.init();
        }
    };
}();

$(function () {
    RequestItem.init();
});