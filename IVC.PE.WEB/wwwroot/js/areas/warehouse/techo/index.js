var Techo = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/techos/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Código IVC",
                data: "code"
            },
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Unidad",
                data: "measurementUnit.abbreviation"
            },
            {
                title: "Metrado Inicial",
                data: "metered"
            },
            {
                title: "Entregas Acumuladas",
                data: "warehouseAccumulatedMetered"
            },
            {
                title: "Saldo",
                data: "warehouseCurrentMetered"
            }
            /*
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
            */
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5] }
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
                        text: "El techo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/almacenes/techos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El techo ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el techo"
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
                    url: _app.parseUrl(`/almacenes/techos/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_budget_title']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_project_formula']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_work_front']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='select_supply_group']").val(result.supplyGroupId).trigger("change");
                        formElements.find("[name='select_supply_family']").val(result.supplyFamilyId).trigger("change");
                        formElements.find("[name='select_measurement_unit']").val(result.measurementUnitId).trigger("change");
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='Metered']").val(result.metered);

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budget_title']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_front']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formula']").val());
                $(formElement).find("[name='SupplyGroupId']").val($(formElement).find("[name='select_supply_group']").val());
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_supply_family']").val());
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_measurement_unit']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/almacenes/techos/crear"),
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
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budget_title']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_front']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formula']").val());
                $(formElement).find("[name='SupplyGroupId']").val($(formElement).find("[name='select_supply_group']").val());
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_supply_family']").val());
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_measurement_unit']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/techos/editar/${id}`),
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
                $(".select2-Techo-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $(".select2-Techo-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
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
            this.budgetTitle.init();
            this.formulas.init();
            this.workFront.init();
            this.supplyGroup.init();
            this.supplyFamily.init();
            this.measurementUnit.init();
        },
        budgetTitle: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budget-titles").select2({
                        data: result
                    });
                });
            }
        },
        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                }).done(function (result) {
                    $(".select2-project-formulas").select2({
                        data: result
                    });
                });
            }
        },
        workFront: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes")
                }).done(function (result) {
                    $(".select2-work-fronts").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#Add_WorkFrontId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#Add_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Add_WorkFrontId").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#Edit_WorkFrontId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#Edit_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Edit_SupplyGroupId").select2({
                        data: result
                    });
                });
            },
            filter: function () {
                $("#work_front_filter").empty();
                $("#work_front_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#project_formula_filter").val()
                    }
                }).done(function (result) {
                    $("#work_front_filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroup: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#Add_SupplyGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#Add_SupplyFamilyId").val()
                    }
                }).done(function (result) {
                    $("#Add_SupplyGroupId").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#Edit_SupplyGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#Edit_SupplyFamilyId").val()
                    }
                }).done(function (result) {
                    $("#Edit_SupplyGroupId").select2({
                        data: result
                    });
                });
            },
            filter: function () {
                $("#supply_group_filter").empty();
                $("#supply_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $("#supply_group_filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyFamily: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        measurementUnit: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    $(".select2-measurement-units").select2({
                        data: result
                    });
                });
            }
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
            $("#add_form [name='BudgetTitleId']").attr("id", "Add_BudgetTitleId");
            $("#edit_form [name='BudgetTitleId']").attr("id", "Edit_BudgetTitleId");

            $("#add_form [name='ProjectFormulaId']").attr("id", "Add_ProjectFormulaId");
            $("#edit_form [name='ProjectFormulaId']").attr("id", "Edit_ProjectFormulaId");

            $("#add_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");

            $("#add_form [name='SupplyFamilyId']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='SupplyFamilyId']").attr("id", "Edit_SupplyFamilyId");

            $("#add_form [name='MeasurementUnitId']").attr("id", "Add_MeasurementUnitId");
            $("#edit_form [name='MeasurementUnitId']").attr("id", "Edit_MeasurementUnitId");

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroup.filter();
            });

            $("#project_formula_filter").on("change", function () {
                select2.workFront.filter();
            });

            $("#Add_SupplyFamilyId").on("change", function () {
                select2.supplyGroup.add();
            });

            $("#Add_ProjectFormulaId").on("change", function () {
                select2.workFront.add();
            });

            $("#Edit_SupplyFamilyId").on("change", function () {
                select2.supplyGroup.edit();
            });

            $("#Edit_ProjectFormulaId").on("change", function () {
                select2.workFront.edit();
            });

            $("#budget_title_filter").on("change", function () {
                datatable.reload();
            });

            $("#project_formula_filter").on("change", function () {
                datatable.reload();
            });

            $("#work_front_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_family_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_group_filter").on("change", function () {
                datatable.reload();
            });
            /*
            $('#main_datatable').on('search.dt', function () {
                var value = $('.dataTables_filter input').val();
                console.log(value); // <-- the value
            });
            */
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
    Techo.init();
});