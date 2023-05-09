var AttendFieldRequest = function () {

    var mainDatatable = null;
    var detailDatatable = null;
    var detailForm = null;
    var loadForm = null;
    var pdfForm = null;
    var Id = null;

    var list = [];

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/atencion-pedidos-campo/listar"),
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
                data: "userName"
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
                data: "groups"
            }, {
                title: "Estado",
                data: "status",
                render: function (data) {
                    if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">EMITIDO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">REQUIERE ATENDER ITEMS</span>
								</label>
							</span>`;
                    }
                    /*
                    else if (data == 4) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">VALIDADO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">REQUIERE ATENDER ITEMS</span>
								</label>
							</span>`;
                    }
                    else if (data == 5) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">VALIDADO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">LISTO PARA ATENDER</span>
								</label>
							</span>`;
                    }*/
                    else if (data == 5) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">EMITIDO</span>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">LISTO PARA ATENDER</span>
								</label>
							</span>`;
                    }
                    else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ATENDIDO</span>
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
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.status == 5) {
                        tmp += `<button data-id="${row.id}" data-url="${row.fileUrl}" class="btn btn-success btn-sm btn-icon btn-load" title="Cargar">`;
                        tmp += `<i class="la la-upload"></i></button> `;
                    }
                    if (row.fileUrl != null) {
                        tmp += `<button data-url="${row.fileUrl}" data-name="${row.documentNumber}" class="btn btn-info btn-sm btn-icon btn-view" title="PDF">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }

                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-report" title="Vale de Pedido">`;
                    tmp += `<i class="la la-file-text"></i></button>`;
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
                title: "Metrado Despachado",
                data: "deliveredQuantity",
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
            {
                title: "Stock Almacén",
                data: "stock"
            },
            {
                title: "Techo",
                data: "techo"
            }
            /*,
            {
                title: "Entrega Acumulada",
                data: "validatedQuantity"
            }*/
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
                ".btn-report",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");

                    window.location = `/almacenes/atencion-pedidos-campo/vale/${id}`;
                });

            mainDatatable.on("click",
                ".btn-details",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.detailReload();
                    $("#detail_modal").modal("show");
                });
            /*
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
                                    url: _app.parseUrl(`/almacenes/atencion-pedidos-campo/atender/${id}`),
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
                */
            mainDatatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let url = $btn.data("url");
                    let name = $btn.data("name");
                    form.load.pdf(name, url);
                });

            mainDatatable.on("click",
                ".btn-load",
                function () {
                    let $btn = $(this);
                    let url = $btn.data("url");

                    Id = $btn.data("id");

                    if (url) {
                        $("#load_form [for='File']").text("Reemplazar archivo subido");
                    }
                    else {
                        $("#load_form [for='File']").text("Selecciona un archivo");
                    }

                    $("#load_modal").modal("show");
                });
        }
    };

    var form = {
        load: {
            load: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#load_form");

                        if (result.fileUrl) {
                            $("#load_form [for='File']").text("Reemplazar archivo subido");
                            $("#load_form [for='file']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#load_form [for='File']").text("Selecciona un archivo");
                            $("#load_form [for='file']").text("Selecciona un archivo");
                        }

                        $("#load_modal").modal("show");
                        _app.loader.hide();
                    });
            },
            pdf: function (name, url) {
                $("#pdf_name").text(name);
                $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                $(".btn-mailto").data("name", name).data("url", "https://docs.google.com/gview?url=" + encodeURI(url));
                $("#pdf_modal").modal("show");
            }
        },
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
                    url: _app.parseUrl(`/almacenes/atencion-pedidos-campo/listo/${Id}`),
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
            },
            loadPdf: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select, textarea").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/almacenes/atencion-pedidos-campo/cargar-guia/${Id}`),
                    method: "put",
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select, textarea").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#load_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#load_alert_text").html(error.responseText);
                            $("#load_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            detail: function () {
                detailForm.resetForm();
                $("#detail_form").trigger("reset");
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
            load: function () {
                loadForm.resetForm();
                $("#load_form").trigger("reset");
                $("#load_alert").removeClass("show").addClass("d-none");

                $("#load_form [for='file']").text("Selecciona un archivo");
            },
            pdf: function () {
                pdfForm.resetForm();
                $("#pdf_form").trigger("reset");
                $("#pdf_alert").removeClass("show").addClass("d-none");
            }
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
            loadForm = $("#load_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.loadPdf(formElement);
                }
            });
            pdfForm = $("#pdf_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
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

            $("#load_modal").on("hidden.bs.modal",
                function () {
                    form.reset.load();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    form.reset.pdf();
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
    AttendFieldRequest.init();
});