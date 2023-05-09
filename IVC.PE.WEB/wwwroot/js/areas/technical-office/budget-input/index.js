var BudgetInput = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/insumos/listar"),
            data: function (d) {
                d.measurementUnitId = $("#measurement_unit_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código S10",
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
                title: "Familia",
                data: "supplyFamily.fullName"
            },
            {
                title: "Grupo",
                data: "supplyGroup.fullName"
            },
            {
                title: "Fórmula",
                data: function (result) {
                    var code = result.projectFormula.code;
                    var name = result.projectFormula.name;
                    return code + " - " + name;
                }
            },
            {
                title: "Titulo de Presupuesto",
                data: "budgetTitle.name"
            },
            {
                title: "Metrado",
                data: "metered"
            },
            {
                title: "P.U (Venta)",
                data: "saleUnitPrice"
            },
            {
                title: "Parcial",
                data: "parcial"
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
        "columnDefs": [
            { className: "dt-body-right", "targets": [7, 8, 9] }
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
                        text: "El insumo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/insumos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El insumo ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al insumo"
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
                    url: _app.parseUrl(`/oficina-tecnica/insumos/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='MeasurementUnitId']").val(result.measurementUnitId).trigger("change");
                        formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                        formElements.find("[name='SupplyGroupId']").val(result.supplyGroupId).trigger("change");
                        formElements.find("[name='SaleUnitPrice']").val(result.saleUnitPrice);
                        formElements.find("[name='GoalUnitPrice']").val(result.goalUnitPrice);
                        formElements.find("[name='select_titles']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_formulas']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_types']").val(result.budgetTypeId).trigger("change");
                        formElements.find("[name='select_groups']").val(result.group).trigger("change");
                        formElements.find("[name='Metered']").val(result.metered);
                        formElements.find("[name='Parcial']").val(result.parcial);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='BudgetTypeId'").val($(formElement).find("[name='select_types']").val());
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/insumos/crear"),
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
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='BudgetTypeId']").val($(formElement).find("[name='select_types']").val());
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/insumos/editar/${id}`),
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
            import: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='BudgetTypeId']").val($(formElement).find("[name='select_types']").val());
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: "/oficina-tecnica/insumos/importar-datos",
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyFile) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        return xhr;
                    }
                }).always(function () {
                    $btn.removeLoader();
                    $(formElement).find("input").prop("disabled", false);
                    $(".progress").empty().remove();
                    if (!emptyFile) {
                        $("#space-bar").remove();
                    }
                }).done(function (result) {
                    datatable.reload();
                    $("#import_data_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_data_alert_text").html(error.responseText);
                        $("#import_data_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#MeasurementUnitId").prop("selectedIndex", 0).trigger("change");
                $("#SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
                $("#SupplyGroupId").prop("selectedIndex", 0).trigger("change");
                $(".select2-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#MeasurementUnitId").prop("selectedIndex", 0).trigger("change");
                $("#SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
                $("#SupplyGroupId").prop("selectedIndex", 0).trigger("change");
                $("#select_types").prop("selectedIndex", 0).trigger("change");
                $("#select_titles").prop("selectedIndex", 0).trigger("change");
                $("#select_groups").prop("selectedIndex", 0).trigger("change");
                $("#select_formulas").prop("selectedIndex", 0).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_data_form").trigger("reset");
                $("#import_data_alert").removeClass("show").addClass("d-none");
                $(".select2-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
            }
        }
    };

    var select2 = {
        init: function () {
            this.measurementUnits.init();
            this.supplyFamilies.init();
            this.supplyGroups.init();
            this.supplyGroups.add();
            this.supplyGroups.edit();
            this.title();
            this.formula();
            this.type();
            this.group();
            this.parcial.input();
        },
        measurementUnits: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    $(".select2-measurement-units").select2({
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
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $(".select2-supply-groups-filter").empty();
                $(".select2-supply-groups-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyfamilyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-supply-groups-filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#Add_SupplyGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyfamilyId: $("#Add_SupplyFamilyId").val()
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
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyfamilyId: $("#Edit_SupplyFamilyId").val()
                    }
                }).done(function (result) {
                    $("#Edit_SupplyGroupId").select2({
                        data: result
                    });
                });
            }
        },
        title: function () {
            $.ajax({
                url: _app.parseUrl("/select/titulos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-titles").select2({
                        data: result
                    });
                    $(".select2-budgettitle-filter").select2({
                        data: result
                    });
                });
        },
        formula: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                    $(".select2-projectformula-filter").select2({
                        data: result
                    });
                });
        },
        type: function () {
            $.ajax({
                url: _app.parseUrl("/select/tipos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-types").select2({
                        data: result
                    });
                    $(".select2-budgettype-filter").select2({
                        data: result
                    });
                });
        },
        group: function () {
            $(".select2-groups").select2({

            });
        },
        parcial: {
            input: function () {
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/insumos/parciales"),
                    data: {
                        measurementUnitId: $("#measurement_unit_filter").val(),
                        supplyFamilyId: $("#supply_family_filter").val(),
                        supplyGroupId: $("#supply_group_filter").val(),
                        projectFormulaId: $("#project_formula_filter").val(),
                        budgetTitleId: $("#budget_title_filter").val()
                    }
                })
                    .done(function (result) {
                        $("#suma_parcial").val(result);
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

            importForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
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

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='MeasurementUnitId']").attr("id", "Add_MeasurementUnitId");
            $("#edit_form [name='MeasurementUnitId']").attr("id", "Edit_MeasurementUnitId");
            $("#add_form [name='SupplyFamilyId']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='SupplyFamilyId']").attr("id", "Edit_SupplyFamilyId");
            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");
            
            $("#measurement_unit_filter, #supply_family_filter, #supply_group_filter, #project_formula_filter, #budget_title_filter").on("change", function () {
                datatable.reload();
                select2.parcial.input();
            });
            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.init();
            });
            $("#Add_SupplyFamilyId").on("change", function () {
                select2.supplyGroups.add();
            });
            $("#Edit_SupplyFamilyId").on("change", function () {
                select2.supplyGroups.edit();
            });
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/insumos/excel-carga-masiva`;
            });
            this.deleteByFilters();
        },
        deleteByFilters: function () {
            $("#btn-delete-by-filters").on("click", function () {
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Los insumos filtrados serán eliminados permanentemente",
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
                                url: _app.parseUrl(`/oficina-tecnica/insumos/eliminar-filtro`),
                                data: {
                                    supplyFamilyId: $("#supply_family_filter").val(),
                                    supplyGroupId: $("#supply_group_filter").val()
                                },
                                type: "delete",
                                success: function (result) {
                                    datatable.reload();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "Los insumos han sido eliminados con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (result) {
                                    console.log(result.responseText);
                                    swal.fire({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn-danger",
                                        animation: false,
                                        customClass: "animated tada",
                                        confirmButtonClass: "Entendido",
                                        text: result.responseText
                                    });
                                }
                            });
                        });
                    }
                });
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
    BudgetInput.init();
});