var ValidationFieldRequest = function () {

    var mainDatatable = null;
    var detailDatatable = null;
    var detailForm = null;
    var Id = null;

    var list = [];

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/validacion-pedidos-campo/listar"),
            data: function (d) {
                d.budgetTitleId = $("#budget_title_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "N° Documento",
                data: "documentNumber"
            },
            {
                title: "Presupuesto",
                data: "budgetTitle.name"
            },
            {
                title: "Fecha de Entrega",
                data: "deliveryDate"
            },
            {
                title: "Fórmula",
                data: "formulas"
            },
            {
                title: "Jefe de Frente",
                data: "sewerGroup.workFrontHead.user.fullName"
            },
            {
                title: "Frente",
                data: "workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Grupo",
                data:"groups"
            }, {
                title: "Estado",
                data: "status",
                render: function (data) {
                    if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">EMITIDO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">REQUIERE VALIDAR ITEMS</span>
								</label>
							</span>`;
                    }
                    else if (data == 3) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">EMITIDO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">LISTO PARA VALIDAR</span>
								</label>
							</span>`;
                    } 
                    else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">VALIDADO</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    */
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
                    if (row.status == 3) {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-validated" title="Validar">`;
                        tmp += `<i class="la la-check-circle"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var detailOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/folding-pedidos-campo/listar"),
            data: function (d) {
                d.id = Id;

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                list = [];
                result.responseJSON.forEach(function (item) {
                    list.push(item.id)
                });
            }
        },
        columns: [
            {
                title: "Grupo",
                data: "goalBudgetInput.supply.supplyGroup.name"
            },
            {
                title: "Fase",
                data: "projectPhase.code"
            },
            {
                title: "Código IVC",
                data: "goalBudgetInput.supply.fullCode"
            },
            {
                title: "Insumo",
                data: "goalBudgetInput.supply.description"
            },
            {
                title: "Und",
                data: "goalBudgetInput.supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Solicitado",
                data: "quantity"
            },
            {
                title: "Ratio de Consumo Acumulado",
                data: "quantity"
            },
            {
                title: "Metrado Validado",
                data: "validatedQuantity",
                width: "15%",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}" data-id="${row.id}" class="form-control"value="${data}">`;
                    /*
                    tmp += `<option value='1' ${data == 1 ? "selected" : ""}> POR ATENDER</option>`;
                    tmp += `<option value='2' ${data == 2 ? "selected" : ""}> PARCIAL</option>`;
                    tmp += `<option value='3' ${data == 3 ? "selected" : ""}> TOTAL</option>`;
                    tmp += `<option value='4' ${data == 4 ? "selected" : ""}> ANULADA</option>`;
                    tmp += `<option value='5' ${data == 5 ? "selected" : ""}> MODIFICADA</option>`;
                    */
                    tmp += `</input>`;
                    return tmp;
                }
            },
        ],
        buttons: []
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            detailDatatable = $("#detail_datatable").DataTable(detailOpts);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        detailReload: function () {
            detailDatatable.ajax.reload();
        },
        initEvents: function () {

            mainDatatable.on("click",
                ".btn-details",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.detailReload();
                    $("#detail_modal").modal("show");
                });

            mainDatatable.on("click",
                ".btn-validated",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Pedido de Campo será validado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, validar Pedido de Campo",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/almacenes/validacion-pedidos-campo/validar/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Pedido de Campo ha sido validado con éxito",
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
                                            text: "Ocurrió un error al intentar validar el Pedido de Campo"
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
        submit: {
            saveItems: function (formElement) {
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let number = $(`#${item}`).val();
                    data.append('Items', item + "|" + number);
                });
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                console.log(data);
                $.ajax({
                    url: _app.parseUrl(`/almacenes/validacion-pedidos-campo/listo/${Id}`),
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
                        $("#detail_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#detail_alert_text").html(error.responseText);
                            $("#detail_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            detail: function () {
                detailForm.resetForm();
                $("#detail_form").trigger("reset");
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
        }
    };

    var validate = {
        init: function () {
            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.saveItems(formElement);
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.budgetTitle.init();
            this.formula.init();
            this.workFront.init();
            this.sewerGroup.init();
            this.warehouse.init();
            this.supplyFamily.init();
            this.supplyGroup.init();
        },
        budgetTitle: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budget-titles").select2({
                        data: result,
                        allowClear: false
                    });
                });
            }
        },
        formula: {
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
        sewerGroup: {
            init: function () {
                $("#sewer_group_filter").empty();
                $("#sewer_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-frente"),
                    data: {
                        workFrontId: $("#work_front_filter").val()
                    }
                }).done(function (result) {
                    $("#sewer_group_filter").select2({
                        data: result
                    });
                });
            }
        },
        warehouse: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/almacenes")
                }).done(function (result) {
                    $(".select2-warehouses").select2({
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
        supplyGroup: {
            init: function () {
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
        }
    };

    var modals = {
        init: function () {
            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });
        }
    };

    var events = {
        init: function () {
            $("#project_formula_filter").on("change", function () {
                select2.workFront.init();
                datatable.reload();
            });

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroup.init();
                datatable.reload();
            });

            $("#budget_title_filter").on("change", function () {
                datatable.reload();
            });

            $("#project_formula_filter").on("change", function () {
                datatable.reload();
            });

            $("#work_front_filter").on("change", function () {
                datatable.reload();
                select2.sewerGroup.init();
            });

            $("#sewer_group_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_family_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
        }
    };
}();

$(function () {
    ValidationFieldRequest.init();
});