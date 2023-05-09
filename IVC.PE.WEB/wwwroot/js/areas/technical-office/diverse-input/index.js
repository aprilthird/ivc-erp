var DiverseInput = function () {

    var addForm = null;
    var editForm = null;
    var importForm = null;
    var diverseInputDataTable = null;
    var edit = false;


    var metered = 0;
    var total = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('es-PE');

    var diverseInputDtOpt = {
        responsive: true,
        serverSide: true,
        processing: true,
        scrollX: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                hide: [1, 2, 3]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            },
            {
                extend: 'copy',
                className: "btn-dark",
                text: "<i class='fa fa-copy'></i> Copiar"
            },
            {
                extend: 'excel',
                className: "btn-success",
                text: "<i class='fa fa-file-excel'></i> Excel"
            },
            {
                extend: 'csv',
                className: "btn-success",
                text: "<i class='fa fa-file-csv'></i> CSV"
            },
            {
                extend: 'pdf',
                className: "btn-danger",
                text: "<i class='fa fa-file-pdf'></i> PDF"
            },
            {
                extend: 'print',
                className: "btn-dark",
                text: "<i class='fa fa-print'></i> Imprimir"
            }
        ],
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/insumos-diversos/listar"),
            data: function (d) {
                d.workFrontId = $("#work_front_filter").val();
                d.projectPhaseId = $("#project_phase_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                delete d.columns;
            },
            complete: function (result) {
                console.log(result);
                metered = 0;
                total = 0;
                if (result.status == 200) {
                    metered = result.responseJSON.metered;
                    total = result.responseJSON.parcial;
                }

                return [$("#total_metered").val(formatter2.format(metered)),
                $("#total_parcial").val(formatter.format(total))];
            }
        },
        columns: [
            {
                //title: "Frente",
                data: "workFront.code"
            },
            {
                //title: "Fase",
                data: "projectPhase.description",
                visible: false
            },
            {
                //title: "Familia",
                data: "supply.supplyFamily.name",
                visible: false
            },
            {
                //title: "Grupo",
                data: "supply.supplyGroup.name",
                visible: false
            },
            {
                //title: "Item",
                data: "itemNumber"
            },
            {
                //title: "Descripción",
                data: "description"
            },
            {
                //title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                //title: "Insumo",
                data: "supply.description"
            },
            {
                //title: "Unidad",
                data: "measurementUnit.abbreviation"
            },
            {
                //title: "Metrado",
                data: "metered"
            },
            {
                //title: "Precio Unitario",
                data: "unitPrice",
                render: $.fn.dataTable.render.number(',', '.', 2)
            },
            {
                //title: "Parcial",
                data: "parcial",
                render: $.fn.dataTable.render.number(',', '.', 2)
            },
            {
                //title: "Código S10",
                data: "budgetInput.code"
            },
            {
                //title: "Opciones",
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
            { className: "dt-body-right", "targets": [9, 10, 11, 12] }
        ]
    };

    var datatable = {
        init: function () {
            diverseInputDataTable = $("#diverse_input_datatable").DataTable(diverseInputDtOpt);
            this.initEvents();
        },
        reload: function () {
            diverseInputDataTable.ajax.reload();
        },
        initEvents: function () {

            diverseInputDataTable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.edit(id);
                });

            diverseInputDataTable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Estás Seguro?",
                        text: "El Insumo DIverso será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/insumos-diversos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Insumo DIverso ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        })
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el Insumo Diverso"
                                        });
                                    }
                                });
                            });
                        },

                    })
                });

        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/insumos-diversos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_titles']").val(result.budgetTitleId).trigger("change");

                        formElements.find("[name='select_formulas']").val(result.projectFormulaId).trigger("change");

                        setTimeout(function () {
                            formElements.find("[name='select_work_fronts']").val(result.workFrontId).trigger("change");
                        }, 1500);

                        setTimeout(function () {
                            formElements.find("[name='select_phases']").val(result.projectPhaseId).trigger("change");
                        }, 2500);

                        formElements.find("[name='ItemNumber']").val(result.itemNumber);
                        formElements.find("[name='Metered']").val(result.metered);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='UnitPrice']").val(result.unitPrice);
                        formElements.find("[name='Parcial']").val(result.parcial);
                        formElements.find("[name='select_supplies']").val(result.supplyId).trigger("change");
                        formElements.find("[name='select_budget_inputs']").val(result.budgetInputId).trigger("change");
                        formElements.find("#Edit_SupplyFamilyId").val(result.supply.supplyFamilyId).trigger("change");
                        formElements.find("[name='select_units']").val(result.measurementUnitId).trigger("change");

                        setTimeout(function () {
                            formElements.find("#Edit_SupplyGroupId").val(result.supply.supplyGroupId).trigger("change");
                        }, 1500);

                        edit = true;
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
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_fronts']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phases']").val());
                $(formElement).find("[name='SupplyId']").val($(formElement).find("[name='select_supplies']").val());
                $(formElement).find("[name='BudgetInputId']").val($(formElement).find("[name='select_budget_inputs']").val());
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_units']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/insumos-diversos/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
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
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_fronts']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phases']").val());
                $(formElement).find("[name='SupplyId']").val($(formElement).find("[name='select_supplies']").val());
                $(formElement).find("[name='BudgetInputId']").val($(formElement).find("[name='select_budget_inputs']").val());
                $(formElement).find("[name='MeasurementUnitId']").val($(formElement).find("[name='select_units']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/insumos-diversos/editar/${id}`),
                    method: "put",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                    })
                    .done(function () {
                        datatable.reload();
                        $("#edit_modal").modal("hide");
                        edit = false;
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    })
            },
            import: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
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
                    url: "/oficina-tecnica/insumos-diversos/importar-datos",
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
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-phases").prop("selectedIndex", 0).trigger("change");
                $(".select2-supplies").prop("selectedIndex", 0).trigger("change");
                $(".select2-budget-inputs").prop("selectedIndex", 0).trigger("change");
                $(".select2-units").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                edit = false;
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-phases").prop("selectedIndex", 0).trigger("change");
                $(".select2-supplies").prop("selectedIndex", 0).trigger("change");
                $(".select2-budget-inputs").prop("selectedIndex", 0).trigger("change");
                $(".select2-units").prop("selectedIndex", 0).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_data_form").trigger("reset");
                $("#import_data_alert").removeClass("show").addClass("d-none");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
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

            importForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import();
                });

        }
    };

    var select2 = {
        init: function () {
            this.title();
            this.formula();
            this.filters();
            this.phase.init();
            this.phase.add();
            this.phase.edit();
            this.workfront.init();
            this.workfront.add();
            this.workfront.edit();
            this.supply.init();
            this.supply.edit();
            this.budgetInput.init();
            this.budgetInput.edit();
            this.supplyFamilies.init();
            this.supplyGroups.index();
            this.supplyGroups.init();
            this.supplyGroups.edit();
            this.unit.init();
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
        phase: {
            init: function() {
                $(".select2-projectphase-filter").empty();
                $(".select2-projectphase-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-frente-trabajo"),
                    data: {
                        workFrontId: $("#work_front_filter").val(),
                        projectFormulaId: $("#project_formula_filter").val()
                    }
                })
                    .done(function (result) {
                        $(".select2-projectphase-filter").select2({
                            data: result
                        });
                    });
            },
            add: function () {
                $("#Add_ProjectPhaseId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-frente-trabajo"),
                    data: {
                        workFrontId: $("#Add_WorkFrontId").val(),
                        projectFormulaId: $("#Add_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Add_ProjectPhaseId").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#Edit_ProjectPhaseId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-frente-trabajo"),
                    data: {
                        workFrontId: $("#Edit_WorkFrontId").val(),
                        projectFormulaId: $("#Edit_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Edit_ProjectPhaseId").select2({
                        data: result
                    });
                });
            }
        },
        workfront: {
            init: function() {
                $(".select2-workfront-filter").empty();
                $(".select2-workfront-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#project_formula_filter").val()
                    }
                }).done(function (result) {
                        $(".select2-workfront-filter").select2({
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
                        $("#Edit_WorkFrontId").select2({
                            data: result
                        });
                    });
            }
        },
        supply: {
            init: function () {
                $("#add_form").find(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-acero"),
                    data: {
                        supplyFamilyId: $("#Add_SupplyFamilyId").val(),
                        supplyGroupId: $("#Add_SupplyGroupId").val()
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
                        supplyFamilyId: $("#Edit_SupplyFamilyId").val(),
                        supplyGroupId: $("#Edit_SupplyGroupId").val()
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
                        familyId: $("#Add_SupplyFamilyId").val(),
                        groupId: $("#Add_SupplyGroupId").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#add_form").find(".select2-budget-inputs").select2({
                            data: result,
                            allowClear: true
                        });
                    });
            },
            edit: function () {
                $("#edit_form").find(".select2-budget-inputs").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-presupuesto"),
                    data: {
                        familyId: $("#Edit_SupplyFamilyId").val(),
                        groupId: $("#Edit_SupplyGroupId").val()
                    },
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#edit_form").find(".select2-budget-inputs").select2({
                            data: result,
                            allowClear: true
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
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
           index: function () {
                $(".select2-supply-groups-filter").empty();
                $(".select2-supply-groups-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#supply_family_filter").val()
                    }
                })
                    .done(function (result) {
                        $(".select2-supply-groups-filter").select2({
                            data: result
                        });
                    });
            },
            init: function () {
                $("#Add_SupplyGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#Add_SupplyFamilyId").val()
                    },
                    dataSrc: ""
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
                        supplyFamilyId: $("#Edit_SupplyFamilyId").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#Edit_SupplyGroupId").select2({
                        data: result
                    });
                });
            }
        },
        unit: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida"),
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-units").select2({
                        data: result
                    });
                });
            }
        },
        filters: function () {

            $("#add_form [name='select_formulas']").attr("id", "Add_ProjectFormulaId");
            $("#edit_form [name='select_formulas']").attr("id", "Edit_ProjectFormulaId");
            $("#add_form [name='select_titles']").attr("id", "Add_BudgetTitleId");
            $("#edit_form [name='select_titles']").attr("id", "Edit_BudgetTitleId");
            $("#add_form [name='select_work_fronts']").attr("id", "Add_WorkFrontId");
            $("#edit_form [name='select_work_fronts']").attr("id", "Edit_WorkFrontId");
            $("#add_form [name='select_phases']").attr("id", "Add_ProjectPhaseId");
            $("#edit_form [name='select_phases']").attr("id", "Edit_ProjectPhaseId");
            $("#add_form .select2-supply-families").attr("id", "Add_SupplyFamilyId");
            $("#edit_form .select2-supply-families").attr("id", "Edit_SupplyFamilyId");
            $("#add_form .select2-supply-groups").attr("id", "Add_SupplyGroupId");
            $("#edit_form .select2-supply-groups").attr("id", "Edit_SupplyGroupId");
            $("#add_form .select2-units").attr("id", "Add_Unitd");
            $("#edit_form .select2-units").attr("id", "Edit_UnitId");

            $("#project_formula_filter").on("change", function () {
                select2.workfront.init();
                datatable.reload();
            });

            $("#Add_ProjectFormulaId").on("change", function () {
                select2.workfront.add();
                select2.phase.add();
            });

            $("#Edit_ProjectFormulaId").on("change", function () {
                select2.workfront.edit();
            });

            $("#budget_title_filter").on("change", function () {
                datatable.reload();
            });
            $("#project_phase_filter").on("change", function () {
                datatable.reload();
            });
            $("#work_front_filter").on("change", function () {
                select2.phase.init();
                datatable.reload();
            });
            $("#Add_WorkFrontId").on("change", function () {
                select2.phase.add();
            });
            $("#Edit_WorkFrontId").on("change", function () {
                select2.phase.edit();
            });
            $("#Add_SupplyFamilyId").on("change", function () {
                select2.supplyGroups.init();
            });
            $("#Edit_SupplyFamilyId").on("change", function () {
                select2.supplyGroups.edit();
            });
            $("#add_form").find("#SearchSelectSupply").on("click", function () {
                select2.supply.init();
                select2.budgetInput.init();
            });
            $("#edit_form").find("#SearchSelectSupply").on("click", function () {
                select2.supply.edit();
                select2.budgetInput.edit();
            });
            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.index();
                datatable.reload();
            });
            $("#supply_group_filter").on("change", function () {
                datatable.reload();
            });
        },
    };

    var events = {
        init: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/insumos-diversos/excel-carga-masiva`;
            });
        }
    };

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    DiverseInput.init();
});