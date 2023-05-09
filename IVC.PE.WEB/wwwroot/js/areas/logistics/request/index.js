var Request = function () {
    var requestId = null;

    var newRequest = false;
    var isDeliveryEdit = false;
    var isItemsEdit = false;

    var limit = null;
    var inEdit = false;

    var requestsDatatable = null;
    var itemsDatatable = null;
    var itemsPreDatatable = null;

    var optionForm = null;
    var existingForm = null;
    var addItemForm = null;
    var editItemForm = null;
    var itemsForm = null;
    var obsForm = null;
    var requestAddForm = null;
    var requestEditForm = null;
    var loadForm = null;

    var supplyFamilyId = null;
    var projectFormulaId = null;

    var reqOpts = {
        responsive: true,
        processing: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15],
                hide: [0, 1, 12]
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
            url: _app.parseUrl("/logistica/requerimientos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "project.costCenter",
                visible: false
            },
            {
                title: "Proyecto",
                data: "project.abbreviation",
                visible: false
            },
            {
                title: "Código",
                data: "correlativeCodeStr"
            },
            {
                title: "Estado",
                data: "orderStatus",
                render: function (data) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">PRE-EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 3) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO</span>
								</label>
							</span>`;
                    } else if (data == 7) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">OBSERVADO</span>
								</label>
							</span>`;
                    } else if (data == 8) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO PARCIALMENTE</span>
								</label>
							</span>`;
                    }

                    else if (data == 9) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">PENDIENTE DE RESPUESTA</span>
								</label>
							</span>`;
                    }

                    else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">ANULADO</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Fórmula",
                data: "projectFormula.code"
            },
            {
                title: "Solicita",
                data: "requestUsernames"
            },
            {
                title: "Presupuesto",
                data: "budgetTitle.name"
            },
            {
                title: "Tipo de Solicitud",
                data: "requestTypeStr"
            },
            {
                title: "Grupos",
                data: "groupNames"
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
                title: "Lugar de Entrega",
                data: "warehouse.address",
                visible: false
            },
            {
                title: "Pre-Requerimientos",
                data: "preRequestNames"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-items" title="Ver Items">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-files" title="Ver Archivos">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                width: "8%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.orderStatus == 7) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-cancel" title="Anular">`;
                        tmp += `<i class="la la-times-circle"></i></button>`;
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title ="Editar">`;
                        tmp += `<i class="fa fa-edit"></i></button> `;
                    } else if (row.orderStatus != 7 && row.orderStatus != 4) {
                        tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-issue" title="Emitir">`;
                        tmp += `<i class="fas fa-arrow-right"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title ="Editar">`;
                        tmp += `<i class="fa fa-edit"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-request" title="Descargar PDF">`;
                    tmp += `<i class="fa fa-clipboard"></i></button> `;
                    */
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel" title="Descargar Excel">`;
                    tmp += `<i class="la la-file-excel-o"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var iteOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/requerimientos/detalles/listar"),
            data: function (d) {
                d.reqId = requestId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Pre-Requerimiento",
                data: function (data) {
                    var pre = data.preRequestItemId;

                    if (pre == null)
                        return "";
                    else
                        return data.preRequestItem.preRequest.correlativeCodeStr;
                }
            },
            {
                title: "Grupo",
                data: "supplyGroupStr"
            },
            {
                title: "Material",
                data: "supply.description"
            },
            {
                title: "Unidad",
                data: "measureUnit"
            },
            {
                title: "Solicitado",
                data: "preRequestItem.measure"
            },
            {
                title: "Acumulado",
                data: "preRequestItem.measureInAttention"
            },
            {
                title: "Metrado",
                data: "measure"
            },
            //{
            //    title: "Techo Max",
            //    data: "goalBudgetInput.metered"
            //},
            //{
            //    title: "Precio aprox.:",
            //    data: "goalBudgetInput.unitPrice"
            //},
            {
                title: "Para se usado en:",
                data: "workFront.code"
            },
            {
                title: "Observaciones",
                data: "observations"
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

    var preOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/pre-requerimientos/requerimientos/detalles/listar"),
            method: "post",
            data: function (d) {
                d.preReqIds = $(".select2-pre-requests").val();
                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                list = [];
                result.responseJSON.forEach(function (item) {
                    list.push(item.id)
                });
                console.log(list);
            }
        },
        columns: [
            {
                title: "Pre-Requerimiento",
                data: "preRequest.correlativeCodeStr"
            },
            {
                title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                title: "Insumo",
                data: "supply.description"
            },
            {
                title: "Unidad",
                data: "measurementUnitName"
            },
            {
                title: "Solicitado",
                data: "measure"
            },
            {
                title: "Acumulado",
                data: "measureInAttention"
            },
            {
                title: "Por Atender",
                data: "measureToAttent",
                width: "15%",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}" data-id="${row.id}" class="form-control" type="number" value="${data}">`;
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
                title: "Observaciones",
                data: "observations"
            },
            {
                title: "Validar",
                data: "validator"
                //data: "goalBudgetInput.currentMetered"
            }
        ],
        buttons: []
    };

    var datatables = {
        init: function () {
            this.reqDt.init();
            this.iteDt.init();
        },
        reqDt: {
            init: function () {
                requestsDatatable = $("#requests_datatable").DataTable(reqOpts);
                itemsPreDatatable = $("#pre_items_datatable").DataTable(preOpts);
                this.events();
            },
            reload: function () {
                requestsDatatable.ajax.reload();
            },
            reloadPre: function () {
                itemsPreDatatable.ajax.reload();
            },
            events: function () {
                requestsDatatable.on("click",
                    ".btn-items",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");

                        requestId = id;
                        datatables.iteDt.reload();
                        form.load.items.detail(id);
                    });

                requestsDatatable.on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        window.location = `/logistica/requerimientos/excel/${id}`;
                    });

                requestsDatatable.on("click",
                    ".btn-files",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.request.file(id);
                    });

                requestsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.request.edit(id);
                    });

                requestsDatatable.on("click",
                    ".btn-request",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                                               
                        let bUrl = _app.parseUrl(`/logistica/requerimientos/emitir-requerimiento-doc/${id}`);
                        window.open(`/logistica/requerimientos/descargar-requerimiento?url=${bUrl}`);
                    });

                requestsDatatable.on("click",
                    ".btn-issue",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Requerimiento será emitido",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, emitir Requerimiento",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/emitir/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            datatables.reqDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido emitido con éxito",
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
                                                text: "Ocurrió un error al intentar emitir el Requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                requestsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Requerimiento será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
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
                                            datatables.iteDt.reload();
                                            datatables.reqDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                requestsDatatable.on("click",
                    ".btn-cancel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Requerimiento será anulado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, anular",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/cancelar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido anulado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                            datatables.reqDt.reload();
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar anular el requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        iteDt: {
            init: function () {
                itemsDatatable = $("#items_datatable").DataTable(iteOpts);
                this.events();
            },
            reload: function () {
                itemsDatatable.ajax.reload();
                $.ajax({
                    url: _app.parseUrl(`/select/requerimientos/thecnical-spec/${requestId}`)
                }).done(function (result) {
                    tecDiv = document.querySelector("#thecnicalSpec");
                    tecDiv.innerHTML = "";
                    result.forEach(function (item) {
                        tecDiv.innerHTML += item.text + "<br/>";
                    })
                });
            },
            events: function () {
                itemsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.items.edit(id);
                    });

                itemsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Item será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/detalles/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.iteDt.reload();
                                            datatables.reqDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Item ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar al Item"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        }
    };

    var form = {
        load: {
            request: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#request_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='RequestType']").val(result.requestType).trigger("change");
                            formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId).trigger("change");
                            formElements.find("[name='RequestUserIds']").val(result.requestUserIds)
                            $(".select2-users").trigger("change");
                            formElements.find("[name='DeliveryDate']").datepicker('setDate', result.deliveryDate);
                            formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                            formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId).trigger("change");
                            formElements.find("[name='WarehouseId']").val(result.warehouseId).trigger("change");

                            $("#request_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                file: function (id) {
                    select2.files.reload(id);
                    $("#file_modal").modal("show");
                }
            },
            items: {
                detail: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#obs_form");
                            formElements.find("[name='Observations']").val(result.observations);
                            formElements.find("[name='QualityCertificate']").prop("checked", result.qualityCertificate);
                            formElements.find("[name='Blueprint']").prop("checked", result.blueprint);
                            formElements.find("[name='TechnicalInformation']").prop("checked", result.technicalInformation);
                            formElements.find("[name='CalibrationCertificate']").prop("checked", result.calibrationCertificate);
                            formElements.find("[name='Catalog']").prop("checked", result.catalog);
                            formElements.find("[name='Other']").prop("checked", result.other);
                            formElements.find("[name='OtherDescription']").val(result.otherDescription);

                            $("#correlative_name").text(result.correlativeCodeStr);
                            select2.files.existing(id);

                            supplyFamilyId = result.supplyFamilyId;
                            //select2.groups.reload();
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();
                            $("#items_add_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                add: function () {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${requestId}`)
                    })
                        .done(function (result) {

                            supplyFamilyId = result.supplyFamilyId;
                            //select2.groups.reload();
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();

                            $("#items_add_modal").modal("hide");
                            $("#add_item_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/detalles/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#edit_item_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='RequestId']").val(result.requestId);
                            formElements.find("[name='WorkFrontId']").val(result.workFrontId).trigger("change");
                            //formElements.find("[name='WorkFrontId']").attr("disabled", "disabled");
                            formElements.find("[name='Measure']").val(result.measure);
                            formElements.find("[name='Observations']").val(result.observations);
                            formElements.find("[name='UsedFor']").val(result.usedFor);
                            formElements.find("#Edit_GroupId").val(result.supply.supplyGroupId).trigger("change");
                            //formElements.find("#Edit_GroupId").attr("disabled", "disabled");

                            select2.supplies.reloadEdit();

                            //formElements.find("[name='SupplyId']").attr("disabled", "disabled");
                            inEdit = true;

                            $.ajax({
                                url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${result.supplyId}`)
                            }).done(function (result) {
                                formElements.find("[name='MeasureUnit']").val(result);
                            });
                            $("#items_add_modal").modal("hide");

                            setTimeout(function () {
                                formElements.find("[name='SupplyId']").val(result.supplyId).trigger("change");

                                $("#edit_item_modal").modal("show");
                                _app.loader.hide();
                            }, 1500);
                        })
                        .always(function () {

                        });
                },
                import: function () {
                    _app.loader.show();
                    $("#items_add_modal").modal("hide");
                    $("#import_modal").modal("show");
                    _app.loader.hide();
                }
            }
        },
        submit: {
            request: {
                add: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/logistica/requerimientos/crear"),
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
                            datatables.reqDt.reload();
                            requestId = result;
                            newRequest = true;
                            $("#request_add_modal").modal("hide");
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
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    //var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input, select").prop("disabled", true);
                    /*
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    */
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/editar/${id}`),
                        method: "put",
                        data: data,
                        contentType: false,
                        processData: false,
                        /*
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
                        */
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.reqDt.reload();
                            requestId = result;
                            newRequest = false;
                            inEdit = false;
                            $("#request_edit_modal").modal("hide");
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
                obs: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input, select").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    let id = requestId;
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/obs/${id}`),
                        method: "put",
                        data: data,
                        contentType: false,
                        processData: false,
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
                            $(formElement).find("input, select").prop("disabled", false);
                            $(".progress").empty().remove();
                            if (!emptyFile) {
                                $("#space-bar").remove();
                            }
                            $("#selectedFiles").html("");
                            $(".custom-file-label").html("Escoge un archivo");
                        })
                        .done(function (result) {
                            datatables.reqDt.reload();
                            $("#items_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#items_add_alert_text").html(error.responseText);
                                $("#items_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                existing: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    list.forEach(function (item) {
                        console.log(item);
                        let number = $(`#${item}`).val();
                        data.append('Items', item + "," + number);
                    });
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/logistica/requerimientos/crear-existente"),
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
                            datatables.reqDt.reload();
                            requestId = result;
                            newRequest = true;
                            $("#request_existing_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#request_existing_add_alert_text").html(error.responseText);
                                $("#request_existing_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                load: function (formElement) {
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
                        url: `/logistica/ordenes/importar`,
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
                        datatables.mainDt.reload();
                        $("#load_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#load_alert_text").html(error.responseText);
                            $("#load_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            },
            items: {
                add: function (formElement) {
                    $(formElement).find("[name='RequestId']").val(requestId);
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/detalles/crear`),
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
                            datatables.iteDt.reload();
                            _app.show.notification.add.success();
                            $("#add_item_modal").modal("hide");
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_item_alert_text").html(error.responseText);
                                $("#add_item_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                edit: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/detalles/editar/${id}`),
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
                            datatables.iteDt.reload();
                            _app.show.notification.add.success();
                            $("#edit_item_modal").modal("hide");
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#edit_item_alert_text").html(error.responseText);
                                $("#edit_item_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                import: function (formElement) {
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
                        url: `/logistica/requerimientos/importar-items/${requestId}`,
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
                        datatables.iteDt.reload();
                        $("#import_modal").modal("hide");
                        $("#items_add_modal").modal("show");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_alert_text").html(error.responseText);
                            $("#import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            request: {
                detail: function () {
                    requestAddForm.resetForm();
                    $("#request_add_form").trigger("reset");
                    $("#Add_RequestUserIds").val('').trigger("change");
                    $("#request_add_alert").removeClass("show").addClass("d-none");
                    $("#Add_WarehouseTypeId").val('').trigger("change");
                    if (newRequest) {
                        newRequest = false;
                        form.load.items.detail(requestId);
                    }
                },
                edit: function () {
                    requestEditForm.resetForm();
                    $("#Edit_WarehouseTypeId").val('').trigger("change");
                    $("#request_edit_form").trigger("reset");
                    $("#request_edit_alert").removeClass("show").addClass("d-none");
                },
                file: function () {
                    $("#gdocs_frame").attr("hidden", true);
                    $("#files_pdf").attr("hidden", true);
                },
                existing: function () {
                    existingForm.resetForm();
                    $(".select2-pre-requests").val('').trigger("change");
                    $("#request_existing_add_form").trigger("reset");
                    $("#request_existing_add_alert").removeClass("show").addClass("d-none");
                    if (newRequest) {
                        newRequest = false;
                        form.load.items.detail(requestId);
                        datatables.iteDt.reload();
                    }
                }
            },
            items: {
                add: function () {
                    //itemsForm.resetForm();
                    addItemForm.resetForm();
                    $("#add_item_form").trigger("reset");
                    $("#add_item_alert").removeClass("show").addClass("d-none");
                    $("#items_add_modal").modal("show");
                },
                edit: function () {
                    //itemsForm.resetForm();
                    editItemForm.resetForm();
                    $("#edit_item_form").trigger("reset");
                    $("#edit_item_alert").removeClass("show").addClass("d-none");
                    $("#items_add_modal").modal("show");
                    inEdit = false;
                },
                import: function () {
                    importForm.resetForm();
                    $("#import_form").trigger("reset");
                    $("#import_alert").removeClass("show").addClass("d-none");
                    $("#items_add_modal").modal("show");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.warehouseTypes();
            this.requestOrderTypes.init();
            this.budgetTitles.init();
            this.users.init();
            this.warehouses.init();
            this.groups.init();
            this.projects.init();
            this.families.init();
            this.formulas.init();
            this.workfronts();
            this.prerequirements();
            this.filters();
            this.types.init();
        },
        requestOrderTypes: {
            init: function () {
                $(".select2-request-order-types").select2();
            }
        },
        budgetTitles: {
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
        users: {
            init: function () {
                var pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/usuarios-proyecto?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            }
        },
        supplies: {
            init: function (gId) {
                $(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero?supplyGroupId=${gId}`)
                }).done(function (result) {
                    $(".select2-supplies").select2({
                        data: result
                    });
                    let id = $(".select2-supplies").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("[name='MeasureUnit']").val(result);
                    });
                });
            },
            reload: function () {
                let gId = $("#Add_GroupId").val();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero?supplyGroupId=${gId}`)
                }).done(function (result) {
                    $("#Add_SupplyId").empty();
                    $("#Add_SupplyId").select2({
                        data: result
                    });
                });
            },
            reloadEdit: function () {
                let gId = $("#Edit_GroupId").val();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero?supplyGroupId=${gId}`)
                }).done(function (result) {
                    $("#Edit_SupplyId").empty();
                    $("#Edit_SupplyId").select2({
                        data: result
                    });
                });
            }
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                });
            }
        },
        families: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-families").select2({
                        data: result
                    });
                });
            }
        },
        groups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
                        data: result
                    });
                    select2.supplies.init(result[0].id);
                });
            },
            //reload: function () {
            //    let fId = $("#family_input_filter").val();
            //    $.ajax({
            //        url: _app.parseUrl(`/select/grupos-de-insumos?familyId=${supplyFamilyId}`)
            //    }).done(function (result) {
            //        $(".select2-groups").empty();
            //        $(".select2-groups").select2({
            //            data: result
            //        });
            //    });
            //}
        },
        files: {
            init: function () {
                $(".select2-files").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/requerimientos/archivos-tspec/${id}`)
                }).done(function (result) {
                    $(".select2-files").empty();
                    $(".select2-files").select2({
                        data: result
                    });
                });
            },
            existing: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/requerimientos/archivos/${id}`)
                }).done(function (result) {
                    selDiv = document.querySelector("#existingFiles");
                    selDiv.innerHTML = "";
                    result.forEach(function (item) {
                        selDiv.innerHTML += item.text + "<br/>";
                    })
                });
            }
        },
        formulas: {
            init: function() {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                })
                    .done(function (result) {
                        $(".select2-project-formulas").select2({
                            data: result
                        });
                    });
            }
        },
        workfronts: function () {
            $.ajax({
                url: _app.parseUrl("/select/frentes-formula"),
                data: {
                    projectFormulaId: projectFormulaId
                }
            })
                .done(function (result) {
                    $('#add_item_form').find(".select2-work-fronts").empty();
                    $('#add_item_form').find(".select2-work-fronts").select2({
                        data: result
                    });
                    $('#edit_item_form').find(".select2-work-fronts").empty();
                    $('#edit_item_form').find(".select2-work-fronts").select2({
                        data: result
                    });
                });
        },
        prerequirements: function () {
            $(".select2-pre-requests").empty();
            console.log($("#type_filter").val());
            _app.loader.show();
            $.ajax({
                url: _app.parseUrl("/select/pre-requerimientos"),
                data: {
                    type: $("#type_filter").val()
                }
            }).done(function (result) {
                $(".select2-pre-requests").select2({
                    data: result
                });
                _app.loader.hide();
            });
        },
        warehouseTypes: function () {
            $.ajax({
                url: _app.parseUrl("/select/tipos-de-almacen")
            }).done(function (result) {
                $(".select2-warehouses-types").select2({
                    data: result
                });
            });
        },
        warehouses: {
            init: function () {
                this.add();
                this.edit();
                this.pre();
            },
            add: function () {
                $("#Add_WarehouseId").empty();
                let typeId = $("#Add_WarehouseTypeId").val();
                $.ajax({
                    url: _app.parseUrl("/select/almacenes"),
                    data: {
                        warehouseTypeId: typeId
                    }
                }).done(function (result) {
                    $("#Add_WarehouseId").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#Edit_WarehouseId").empty();
                let typeId = $("#Edit_WarehouseTypeId").val();
                $.ajax({
                    url: _app.parseUrl("/select/almacenes"),
                    data: {
                        warehouseTypeId: typeId
                    }
                }).done(function (result) {
                    $("#Edit_WarehouseId").select2({
                        data: result
                    });
                });
            },
            pre: function () {
                $("#Pre_WarehouseId").empty();
                let typeId = $("#Pre_WarehouseTypeId").val();
                $.ajax({
                    url: _app.parseUrl("/select/almacenes"),
                    data: {
                        warehouseTypeId: typeId
                    }
                }).done(function (result) {
                    $("#Pre_WarehouseId").select2({
                        data: result
                    });
                });
            }
        },
        filters: function() {
            $("#type_filter").on("change", function () {
                console.log("buenos dias");
                select2.prerequirements();
            });
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var validate = {
        init: function () {

            requestAddForm = $("#request_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.add(formElement);
                }
            });

            requestEditForm = $("#request_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.edit(formElement);
                }
            });

            itemsForm = $("#items_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    if (!isItemsEdit) {
                        form.submit.items.detail(formElement);
                    } else {
                        isItemsEdit = false;
                        form.submit.items.edit(formElement);
                    }
                    
                }
            });

            obsForm = $("#obs_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.obs(formElement);
                }
            });

            addItemForm = $("#add_item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.items.add(formElement);
                }
            });

            editItemForm = $("#edit_item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.items.edit(formElement);
                }
            });

            optionForm = $("#option_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            existingForm = $("#request_existing_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.existing(formElement);
                }
            });

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.items.import(formElement);
                }
            });

            loadForm = $("#load_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.load(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {

            $("#request_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.request.detail();
                });

            $("#request_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.request.edit();
                });

            $("#add_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.add();
                });

            $("#edit_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.edit();
                });

            $("#file_modal").on("hidden.bs.modal",
                function () {
                    form.reset.request.file();
                });

            $("#request_existing_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.request.existing();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.import();
                });
        }
    };

    var events = {
        init: function () {
            $("#request_add_form [name='RequestType']").attr("id", "Add_RequestType");
            $("#request_add_form [name='BudgetTitleId']").attr("id", "Add_BudgetTitleId");
            $("#request_add_form [name='RequestUserIds']").attr("id", "Add_RequestUserIds");
            $("#request_add_form [name='WarehouseId']").attr("id", "Add_WarehouseId");
            $("#request_add_form .select2-warehouses-types").attr("id", "Add_WarehouseTypeId");
            $("#request_add_form [name='ProjectFormulaId']").attr("id", "Add_ProjectFormulaId");

            $("#request_edit_form [name='RequestType']").attr("id", "Edit_RequestType");
            $("#request_edit_form [name='BudgetTitleId']").attr("id", "Edit_BudgetTitleId");
            $("#request_edit_form [name='RequestUserIds']").attr("id", "Edit_RequestUserIds");
            $("#request_edit_form [name='WarehouseId']").attr("id", "Edit_WarehouseId");
            $("#request_edit_form .select2-warehouses-types").attr("id", "Edit_WarehouseTypeId");
            $("#request_edit_form [name='ProjectFormulaId']").attr("id", "Edit_ProjectFormulaId");

            $("#add_item_form [name='SupplyId']").attr("id", "Add_SupplyId");
            $("#edit_item_form [name='SupplyId']").attr("id", "Edit_SupplyId");

            $("#add_item_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_item_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#add_item_form .select2-supply-groups").attr("id", "Add_GroupId");
            $("#edit_item_form .select2-supply-groups").attr("id", "Edit_GroupId");

            $("#request_existing_add_modal .select2-warehouses-types").attr("id", "Pre_WarehouseTypeId");
            $("#request_existing_add_modal [name='WarehouseId']").attr("id", "Pre_WarehouseId");

            $("#Add_SupplyId").on("change", function () {
                let id = this.value;
                $.ajax({
                    url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                }).done(function (result) {
                    $("#add_item_form [name='MeasureUnit']").val(result);
                });
            });

            $("#addNewItem").on("click", function () {
                form.load.items.add();
            });

            $("#Add_GroupId").on("change", function () {
                select2.supplies.reload();
                setTimeout(function () {
                    let id = $("#Add_SupplyId").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("#add_item_form [name='MeasureUnit']").val(result);
                    });
                }, 1000);
            });

            $("#Add_WarehouseTypeId").on("change", function () {
                select2.warehouses.add();
            });

            $("#Edit_GroupId").on("change", function () {
                if (inEdit == true)
                    select2.supplies.reloadEdit();
            });

            $("#Edit_SupplyId").on("change", function () {
                let id = this.value;
                if (inEdit == true) {
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("#edit_item_form [name='MeasureUnit']").val(result);
                    });
                }
            });

            $("#Edit_WarehouseTypeId").on("change", function () {
                select2.warehouses.edit();
            });

            $(".custom-file-input").on("change", function (e) {
                if (!e.target.files) return;

                selDiv = document.querySelector("#selectedFiles");
                selDiv.innerHTML = "";

                var files = e.target.files;
                for (var i = 0; i < files.length; i++) {
                    var f = files[i];
                    selDiv.innerHTML += f.name + "<br/>";
                }
            });

            $("#btn_LoadFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri.includes("pdf")) {
                    $("#gdocs_frame").attr("hidden", true);
                    $("#files_pdf").removeAttr("hidden");
                    loadPdf("Archivo", uri, "files_pdf");
                } else {
                    $("#files_pdf").attr("hidden", true);
                    $("#gdocs_frame").removeAttr("hidden");
                    let documentUrl = "";
                    documentUrl = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(uri));
                    $("#gdocs_frame").prop("src", documentUrl + "&embedded=true");
                }
            });

            $("#btn_downloadFile").on("click", function ()
            {
                var uri = $("#select_files").val();
                if (uri != null) {
                    window.location = `${uri}`;
                }
            });

            $("#btn_deleteFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri != null) {
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Archivo será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/logistica/requerimientos/archivo/eliminar?url=${uri}`),
                                    type: "delete",
                                        success: function (result) {
                                            select2.files.reload(requestId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Archivo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el archivo"
                                            });
                                        }
                                    });
                            });
                        }
                    });
                }
                
            });

            $(".select2-pre-requests").on("change", function () {
                datatables.reqDt.reloadPre();
            });

            $("#requerimiento_nuevo").on("click", function () {
                $("#option_add_modal").modal("hide");
                $("#request_add_modal").modal("show");
            });
            $("#requerimiento_existe").on("click", function () {
                $("#option_add_modal").modal("hide");
                $("#request_existing_add_modal").modal("show");
                select2.prerequirements();
            });

            $("#Pre_WarehouseTypeId").on("change", function () {
                select2.warehouses.pre();
            });

            $("#genEsxelSample").on("click", function () {
                window.location = _app.parseUrl(`/logistica/requerimientos/excel-modelo/${requestId}`);
            });

            $("#loadItems").on("click", function () {
                form.load.items.import();
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modals.init();
            datepicker.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    Request.init();
});