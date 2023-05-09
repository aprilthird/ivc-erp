var BudgetInput = function () {

    var mainDatatable = null;
    var detailDatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var codeResult = null;
    var filtro = false;
    var Id = null;

    var metered = 0;
    var total = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('es-PE');

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/meta-insumos/listar"),
            data: function (d) {
                d.measurementUnitId = $("#measurement_unit_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                d.workFrontId = $("#work_front_filter").val();
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
                //title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                //title: "Descripción",
                data: "supply.description"
            },
            {
                //title: "Unidad",
                data: "measurementUnit.abbreviation"
            },
            {
                //title: "Familia",
                data: "supply.supplyFamily.fullName"
            },
            {
                //title: "Grupo",
                data: "supply.supplyGroup.fullName"
            },
            {
                //title: "Fórmula",
                data: function (result) {
                    var code = result.projectFormula.code;
                    var name = result.projectFormula.name;
                    return code + " - " + name;
                }
            },
            {
                //title: "Titulo de Presupuesto",
                data: "budgetTitle.name"
            },
            {
                //title: "Metrado",
                data: function (result) {
                    var metered = result.metered;
                    if (metered != "0.00")
                        return metered;
                    else
                        return "";
                }
            },
            {
                //title: "P.U (Venta)",
                data: "unitPrice"
            },
            {
               // title: "Parcial",
                data: "parcial"
            },
            {
                //title: "Saldo",
                data: "currentMetered"
            },
            {
                //title: "Requerimientos Acumulados",
                data: "accumulatedRequestItems"
            },
            {
                //title: "MetradoAux",
                data: "meteredAux",
                visible: false
            },
            {
                //title: "Parcial Aux",
                data: "parcialAux",
                visible: false
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-detail">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    return tmp;
                }
            },
            {
                //title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
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


    var detailOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/meta-insumos/requerimientos/detalles"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Requerimiento",
                data: "request.correlativeCodeStr"
            },
            {
                title: "Insumo",
                data: "goalBudgetInput.supply.description"
            },
            {
                title: "Metrado",
                data: "measure"
            },
            {
                title: "Metrado Atendido",
                data: "measureInAttention"
            }
        ]
    };


    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            detailDatable = $("#detail_datatable").DataTable(detailOptions);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        detailReload: function () {
            detailDatable.ajax.reload();
        },
        recalculate: function () {

            $("#total_metered").val(formatter2.format(mainDatatable.column(13, { filter: 'applied' })
                .data()
                .reduce(function (a, b) {
                    return a + b;
                })));

            $("#total_parcial").val(formatter.format(mainDatatable.column(14, { filter: 'applied' })
                .data()
                .reduce(function (a, b) {
                    return a + b;
                })));

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
                ".btn-detail",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.detailReload();
                    $("#detail_modal").modal("show");
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
                                    url: _app.parseUrl(`/oficina-tecnica/meta-insumos/eliminar/${id}`),
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
                    url: _app.parseUrl(`/oficina-tecnica/meta-insumos/${id}`)
                })
                    .done(function (result) {
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
                    url: _app.parseUrl("/oficina-tecnica/meta-insumos/crear"),
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
                    url: _app.parseUrl(`/oficina-tecnica/meta-insumos/editar/${id}`),
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
                    url: "/oficina-tecnica/meta-insumos/importar-datos",
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
            this.title();
            this.formula();
            this.type();
            this.group();
            this.workfront();
            this.filters();
            //this.suma();
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
                    url: _app.parseUrl("/select/familias-de-insumos"),
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $(".select2-supply-groups").empty();
                $(".select2-supply-groups").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
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
        workfront: function () {
            $(".select2-workfront-filter").empty();
            $(".select2-workfront-filter").append(`<option>Todos</option>`);
            $.ajax({
                url: _app.parseUrl("/select/frentes-formula"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val()
                }
            })
                .done(function (result) {
                    $(".select2-work-fronts").select2({
                        data: result
                    });
                    $(".select2-workfront-filter").select2({
                        data: result
                    });
                });
        },
        filters: function () {
            $("#project_formula_filter").on("change", function () {
                select2.workfront();
                //datatable.reload();
                //select2.suma();
            });
            $("#budget_title_filter").on("change", function () {
                datatable.reload();
                //select2.suma();
            });
            $("#project_phase_filter").on("change", function () {
                datatable.reload();
                //select2.suma();
            });
            $("#work_front_filter").on("change", function () {
                datatable.reload();
                //select2.suma();
            });
            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.init();
                //select2.suma();
            });
        },
        suma: function () {

            if (filtro == true) {
                _app.loader.show();
            }

            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/meta-insumos/metrado"),
                data: {
                    measurementUnitId : $("#measurement_unit_filter").val(),
                    supplyFamilyId : $("#supply_family_filter").val(),
                    supplyGroupId : $("#supply_group_filter").val(),
                    projectFormulaId : $("#project_formula_filter").val(),
                    budgetTitleId : $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    code: codeResult
                }
            })
                .done(function (result) {
                    $("#total_metered").val(result);
                });

            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/meta-insumos/parcial"),
                data: {
                    measurementUnitId: $("#measurement_unit_filter").val(),
                    supplyFamilyId: $("#supply_family_filter").val(),
                    supplyGroupId: $("#supply_group_filter").val(),
                    projectFormulaId: $("#project_formula_filter").val(),
                    budgetTitleId: $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    code: codeResult
                }
            })
                .done(function (result) {
                    $("#total_parcial").val(result);

                    _app.loader.hide();
                });
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
            });
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/meta-insumos/excel-carga-masiva`;
            });

            $("#btn-refresh").on("click", function () {

                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Se actualizará los techos, esto puede tardar unos minutos",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, actualizar",
                    confirmButtonClass: "btn-danger",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: _app.parseUrl("/oficina-tecnica/meta-insumos/cargar"),
                                type: "post",
                                success: function (result) {
                                    datatable.reload();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "Se han actualizado los techos con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (result) {
                                    swal.fire({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn-danger",
                                        animation: false,
                                        customClass: 'animated tada',
                                        confirmButtonText: "Entendido",
                                        text: result.responseText
                                    });
                                }
                            });
                        });
                    }
                });

            });


            $("#btn-search").on("click", function () {
                /*
                if (filtro == false) {
                    filtro = true;
                    $("#btn-search").css('background-color', '#FF6347');
                    $("#btn-search").html('<i class="la la-search"></i><span>Cancelar</span>');
                } else {
                    filtro = false;
                    codeResult = null;
                    $("#btn-search").css('background-color', 'transparent');
                    $("#btn-search").html('<i class="la la-search"></i><span>Filtrar por texto</span>');
                }*/
                return datatable.recalculate();
                //select2.suma();
 /*
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/meta-insumos/filtro-codigo/${codeResult}`),
                    method: "get"
                }).always(function () {
                    _app.loader.hide();
                }).done(function (result) {
                    datatable.reload();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#load_data_alert_text").html(error.responseText);
                        $("#load_data_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
                */
            });

            $('#main_datatable').on('search.dt', function () {
                codeResult = $('.dataTables_filter input').val();
            });
            /*
            $.ajax({
                url: _app.parseUrl(`/oficina-tecnica/meta-insumos/actualizar`),
                method: "put"
            })
            */
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