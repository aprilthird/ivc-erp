var Request = function () {

    var mainDatatable = null;
    var deliveryPlacesDatatable = null;

    var addForm = null;
    var editForm = null;
    var deliveryForm = null;

    var addFormRepeater = null;
    var editFormRepeater = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/requerimientos/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "project.costCenter"
            },
            {
                title: "Proyecto",
                data: "project.abbreviation"
            },
            {
                title: "Código",
                data: "correlativeCodeStr"
            },
            {
                title: "Presupuesto",
                data: "budgetType",
                render: function (data) {
                    return _app.render.label(data, _app.constants.budget.type.VALUES);
                }
            },
            {
                title: "Familia",
                data: "supplyFamily.fullName"
            },
            {
                title: "Fecha de Emisión",
                data: "issueDate",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Fecha de Entrega",
                data: "deliveryDate",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Lugar de Entreg",
                data: "deliveryPlace"
            },
            {
                title: "Adjuntos (Área Técnica)",
                data: null,
                render: function () {
                    return "Ningún adjunto";
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
    var delOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/lugar-entrega-requerimientos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Descripción",
                data: "description"
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
        buttons:[]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            this.initEvents();
            this.delDt.init();
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
                        text: "El requerimiento será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/logistica/requerimientos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El requerimiento ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al requerimiento"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

        },
        delDt: {
            init: function () {
                deliveryPlacesDatatable = $("#delivery_places_datatable").DataTable(delOpts);
            },
            reload: function () {
                deliveryPlacesDatatable.ajax.reload();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/requerimientos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='CorrelativeCode']").val(result.correlativeCode);
                        formElements.find("[name='DeliveryPlace']").val(result.deliveryPlace);
                        formElements.find("[name='Observations']").val(result.observations);
                        formElements.find("[name='BudgetType']").val(result.budgetType).trigger("change");
                        formElements.find("[name='Type']").val(result.type).trigger("change");
                        formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                        formElements.find("[name='IssueDate']").datepicker("setDate", result.issueDate);
                        formElements.find("[name='DeliveryDate']").datepicker("setDate", result.deliveryDate);

                        if (result.requestItems) {
                            editFormRepeater.setList(result.requestItems);
                            Array.from(document.querySelectorAll("#edit_form [data-repeater-item]"))
                                .forEach((el, i) => {
                                    var budgetInput = el.querySelector(".select2-budget-inputs");
                                    $(budgetInput).val(result.requestItems[i].budgetInputId).trigger("change");
                                    var measurementUnit = el.querySelector(".select2-measurement-units");
                                    $(measurementUnit).val(result.requestItems[i].measurementUnitId).trigger("change");
                                    var projectPhase = el.querySelector(".select2-project-phases");
                                    $(projectPhase).val(result.requestItems[i].projectPhaseIds).trigger("change");
                                    var supplyGroup = el.querySelector(".select2-supply-groups");
                                    $(supplyGroup).val(result.requestItems[i].supplyGroupId).trigger("change");
                                    el.querySelector(`input[name='RequestItems[${i}][Id]']`).value = result.requestItems[i].id;
                                    el.querySelector(`input[name='RequestItems[${i}][Measure]']`).value = result.requestItems[i].measure;
                                    el.querySelector(`input[name='RequestItems[${i}][Observations]']`).value = result.requestItems[i].observations;
                                });
                        }

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
                    url: _app.parseUrl("/logistica/requerimientos/crear"),
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
                    url: _app.parseUrl(`/logistica/requerimientos/editar/${id}`),
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
            },
            delivery: {
                add: function (formElement) {
                    console.log("llego");
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/logistica/lugar-entrega-requerimientos/crear"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatable.delDt.reload();
                            deliveryForm.reset();
                            $("#delivery_place_form").trigger("reset");
                            //$("#add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_alert_text").html(error.responseText);
                                $("#add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#Add_BudgetType").prop("selectedIndex", 0).trigger("change");
                $("#Add_Type").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_BudgetType").prop("selectedIndex", 0).trigger("change");
                $("#Edit_Type").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            },
            delivery: {
                add: function () {
                    deliveryForm.reset();
                    $("#delivery_place_form").trigger("reset");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.budgetTypes.init();
            this.types.init();
            this.measurementUnits.fetchAndInit();
            this.supplyFamilies.fetchAndInit();
            this.budgetInputs.fetchAndInit();
            this.projectPhases.fetchAndInit();
            this.supplyGroups.fetchAndInit();
        },
        budgetTypes: {
            init: function () {
                $(".select2-budget-types").select2();
            }
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        },
        measurementUnits: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-measurement-units").select2({
                    data: select2.measurementUnits.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    select2.measurementUnits.data = result;
                    callback();
                });
            }
        },
        supplyFamilies: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-supply-families").select2({
                    data: select2.supplyFamilies.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    select2.supplyFamilies.data = result;
                    callback();
                });
            }
        },
        budgetInputs: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-budget-inputs").select2({
                    data: select2.budgetInputs.data,
                    minimumInputLength: 3
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-presupuesto")
                }).done(function (result) {
                    select2.budgetInputs.data = result;
                    callback();
                });
            }
        },
        projectPhases: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-project-phases").select2({
                    data: select2.projectPhases.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto")
                }).done(function (result) {
                    select2.projectPhases.data = result;
                    callback();
                });
            }
        },
        supplyGroups: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-supply-groups").select2({
                    data: select2.supplyGroups.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos")
                }).done(function (result) {
                    select2.supplyGroups.data = result;
                    callback();
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

            deliveryForm = $("#delivery_place_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.delivery.add(formElement);
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

            $("#delivery_place_modal").on("hidden.bs.modal",
                function () {
                    form.reset.delivery.add();
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
            $("#add_form [name='BudgetType']").attr("id", "Add_BudgetType");
            $("#edit_form [name='BudgetType']").attr("id", "Edit_BudgetType");
            $("#add_form [name='Type']").attr("id", "Add_Type");
            $("#edit_form [name='Type']").attr("id", "Edit_Type");
            $("#add_form [name='SupplyFamilyId']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='SupplyFamilyId']").attr("id", "Edit_SupplyFamilyId");
            $("#add_form .repeater-items").attr("id", "add_repeater_items");
            $("#edit_form .repeater-items").attr("id", "edit_repeater_items");

            addFormRepeater = $('#add_repeater_items').repeater({
                initEmpty: false,
                isFirstItemUndeletable: true,
                defaultValues: {
                },
                show: function () {
                    $(this).slideDown();
                    Array.from(document.querySelectorAll("#add_form [data-repeater-item]"))
                        .forEach((el, i) => {
                            var budgetInput = el.querySelector(".select2-budget-inputs");
                            budgetInput.id = `Add_Budget_Input_${i}`;
                            var measurementUnit = el.querySelector(".select2-measurement-units");
                            measurementUnit.id = `Add_Measurement_Unit_${i}`;
                            var projectPhase = el.querySelector(".select2-project-phases");
                            projectPhase.id = `Add_Project_Phase_${i}`;
                            var supplyGroup = el.querySelector(".select2-supply-groups");
                            supplyGroup.id = `Add_Supply_Group_${i}`;
                        });
                    select2.budgetInputs.init();
                    select2.measurementUnits.init();
                    select2.projectPhases.init();
                    select2.supplyGroups.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
            });

            editFormRepeater = $('#edit_repeater_items').repeater({
                initEmpty: true,
                isFirstItemUndeletable: false,
                show: function () {
                    $(this).slideDown();
                    Array.from(document.querySelectorAll("#edit_form [data-repeater-item]"))
                        .forEach((el, i) => {
                            var budgetInput = el.querySelector(".select2-budget-inputs");
                            budgetInput.id = `Edit_Budget_Input_${i}`;
                            var measurementUnit = el.querySelector(".select2-measurement-units");
                            measurementUnit.id = `Edit_Measurement_Unit_${i}`;
                            var projectPhase = el.querySelector(".select2-project-phases");
                            projectPhase.id = `Edit_Project_Phase_${i}`;
                            var supplyGroup = el.querySelector(".select2-supply-groups");
                            supplyGroup.id = `Edit_Supply_Group_${i}`;
                        });
                    select2.budgetInputs.init();
                    select2.measurementUnits.init();
                    select2.projectPhases.init();
                    select2.supplyGroups.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
            });

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
    Request.init();
});