var SteelVariable = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/variables-acero/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                //title: "Código IVC",
                data: "budgetInput.code"
            },
            {
                //title: "Descripción",
                data: "budgetInput.description"
            },
            {
                //title: "Descripción",
                data: "budgetInput.saleUnitPrice"
            },
            {
                //title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                //title: "Descripción",
                data: "supply.description"
            },
            {
                //title: "pulg.",
                data: function (result) {
                    var res = result.rodDiameterInch;
                    if (res == '"')
                        return "";
                    else
                        return res;
                }
            },
            {
                //title: "mm",
                data: function (result) {
                    var res = result.rodDiameterMilimeters;
                    if (res == 0)
                        return "";
                    else
                        return res;
                }
            },
            {
                //title: "mm",
                data: "section"
            },
            {
                //title: "mm",
                data: "perimeter"
            },
            {
                //title: "mm",
                data: function (result) {
                    var res = result.nominalWeight;
                    return res;
                }
            },
            {
                //title: "mm",
                data: "pricePerRod"
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
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2, 5, 6, 7, 8, 9, 10] }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#steelvariable_datatable").DataTable(options);
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
                        text: "La variable acero será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/variables-acero/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La variable de acero ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la variable de acero"
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
                    url: _app.parseUrl(`/oficina-tecnica/variables-acero/${id}`)
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id']").val(result.id);
                    formElements.find("[name='RodDiameterInch']").val(result.rodDiameterInch);
                    formElements.find("[name='RodDiameterMilimeters']").val(result.rodDiameterMilimeters);
                    formElements.find("[name='Section']").val(result.section);
                    formElements.find("[name='Perimeter']").val(result.perimeter);
                    formElements.find("[name='NominalWeight']").val(result.nominalWeight);
                    formElements.find("[name='select_supplies']").val(result.supplyId).trigger("change");
                    formElements.find("[name='select_budget_inputs']").val(result.budgetInputId).trigger("change");
                    formElements.find("#select_supply_families_filter").val(result.supply.supplyFamilyId).trigger("change");
                    formElements.find("#select_supply_groups_filter").val(result.supply.supplyGroupId).trigger("change");

                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SupplyId']").val($(formElement).find("[name='select_supplies']").val());
                $(formElement).find("[name='BudgetInputId']").val($(formElement).find("[name='select_budget_inputs']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/variables-acero/crear"),
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
                $(formElement).find("[name='SupplyId']").val($(formElement).find("[name='select_supplies']").val());
                $(formElement).find("[name='BudgetInputId']").val($(formElement).find("[name='select_budget_inputs']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/variables-acero/editar/${id}`),
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
                $(".select2-supplies").prop("selectedIndex", 0).trigger("change");
                $(".select2-budget-inputs").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-supplies").prop("selectedIndex", 0).trigger("change");
                $(".select2-budget-inputs").prop("selectedIndex", 0).trigger("change");
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

    var select2 = {
        init: function () {
            this.supply.init();
            this.supply.edit();
            this.budgetInput.init();
            this.budgetInput.edit();
            this.supplyFamilies.init();
            this.supplyGroups.init();
            this.supplyGroups.edit();
            this.filters();
        },
        supply: {
            init: function () {
                $("#add_form").find(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-acero"),
                    data: {
                        supplyFamilyId: $("#add_form").find("#select_supply_families_filter").val(),
                        supplyGroupId: $("#add_form").find("#select_supply_groups_filter").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#add_form").find(".select2-supplies").select2({
                            data: result
                        });
                    });
            },
            edit: function () {
                $("#edit_form").find(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-acero"),
                    data: {
                        supplyFamilyId: $("#edit_form").find("#select_supply_families_filter").val(),
                        supplyGroupId: $("#edit_form").find("#select_supply_groups_filter").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#edit_form").find(".select2-supplies").select2({
                            data: result
                        });
                    });
            }
        },
        budgetInput: {
            init: function () {
                $("#add_form").find(".select2-budget-inputs").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-presupuesto"),
                    data: {
                        familyId: $("#add_form").find("#select_supply_families_filter").val(),
                        groupId: $("#add_form").find("#select_supply_groups_filter").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#add_form").find(".select2-budget-inputs").select2({
                            data: result
                        });
                    });
            },
            edit: function () {
                $("#edit_form").find(".select2-budget-inputs").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-presupuesto"),
                    data: {
                        familyId: $("#edit_form").find("#select_supply_families_filter").val(),
                        groupId: $("#edit_form").find("#select_supply_groups_filter").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#edit_form").find(".select2-budget-inputs").select2({
                            data: result
                        });
                    });
            }
        },
        supplyFamilies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families-filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $("#add_form").find(".select2-supply-groups-filter").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#add_form").find("#select_supply_families_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#add_form").find(".select2-supply-groups-filter").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#edit_form").find(".select2-supply-groups-filter").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#edit_form").find("#select_supply_families_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#edit_form").find(".select2-supply-groups-filter").select2({
                        data: result
                    });
                });
            }
        },
        filters: function(){
            $("#add_form").find(".select2-supply-families-filter").on("change", function () {
                select2.supplyGroups.init();
            });
            $("#add_form").find("#SearchSelectSupply").on("click", function () {
                select2.supply.init();
                select2.budgetInput.init();
            });
            /*
            $("#edit_form").find(".select2-supply-families-filter").on("change", function () {
                select2.supplyGroups.edit();
            });
            */
            $("#edit_form").find("#SearchSelectSupply").on("click", function () {
                select2.supply.edit();
                select2.budgetInput.edit();
            });
        }
    };

    var events = {
        refresh: function () {
            $("#btn-refresh").on("click", function () {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/variables-acero/actualizar"),
                    method: "put",
                    contentType: false,
                    processData: false
                })
                    .done(function () {
                        datatable.reload();
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            });
        }
    };

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            events.refresh();
            select2.init();
        }
    }
}();

$(function () {
    SteelVariable.init();
});