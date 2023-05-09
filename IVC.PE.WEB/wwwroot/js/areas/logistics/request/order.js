var Request = function () {

    var mainDatatable = null;
    var itemsDatatable = null;
    var addForm = null;
    var editForm = null;
    var requestAddForm = null;
    var requestEditForm = null;
    var addItemForm = null;
    var editItemForm = null;

    var inEdit = false;

    var requestId = null;

    var requestEditForm = null;
    var projectFormulaId = null;
    var supplyFamilyId = null;

    var options = {
        responsive: true,
        initComplete: function () {
            select2.status.init();
        },
        ajax: {
            url: _app.parseUrl("/logistica/requerimientos/ordenes/listar"),
            dataSrc: "",
            data: function (d) {
                d.status = $("#status_filter").val();
                d.type = 1;
                d.user = $("#user_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
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
                title: "Estado",
                data: "orderStatus",
                render: function (data, type, row) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">PRE-EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">REQ. EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 3) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
								</label>
							</span>`;
                    } else if (data == 4) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">REQ. ANULADO</span>
								</label>
							</span>`;
                    } else if (data == 5) {
                        if (row.attentionStatus == 3)
                            return `<span class="kt-switch kt-switch--icon">
								    <label>
									    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                        <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/C GENERADA</span>
                                        <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ATENDIDO TOTALMENTE</span>
								    </label>
							    </span>`;
                        else if (row.attentionStatus == 2)
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/C GENERADA</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ATENDIDO PARCIALMENTE</span>
								</label>
							</span>`;
                        else
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/C GENERADA</span>
								</label>
							</span>`;
                    } else if (data == 6) {
                        if (row.attentionStatus == 3)
                            return `<span class="kt-switch kt-switch--icon">
								    <label>
									    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                        <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/S GENERADA</span>
                                        <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ATENDIDO TOTALMENTE</span>
								    </label>
							    </span>`;
                        else if (row.attentionStatus == 2)
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/S GENERADA</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ATENDIDO PARCIALMENTE</span>
								</label>
							</span>`;
                        else
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">REQ. APROBADO</span>
                                    <span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">O/S GENERADA</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">OBSERVADO</span>
								</label>
							</span>`;
                    }
                }
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
                data: "warehouse.address"
            },
            {
                title: "Carpeta",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-items" title="Ver Items">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-files" title="Ver Archivos">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel" title="Descargar Excel">`;
                    tmp += `<i class="la la-file-excel-o"></i></button> `;
                    return tmp;
                }
            },
            /*
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.orderStatus == 2) {
                        tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-approved">`;
                        tmp += `<i class="fas fa-arrow-right"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-cancel">`;
                        tmp += `<i class="la la-ban"></i></button>`;
                    }
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    
                    return tmp;
                }
            }
            */
        ]
    };

    var itemOpts = {
        responsive: true,
        paging: false,
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
                    var pre = data.preRequestItem.preRequestId;

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
                data: "goalBudgetInput.supply.description"
            },
            {
                title: "Unidad",
                data: "measureUnit"
            },
            {
                title: "Metrado",
                data: "measure"
            },
            {
                title: "Techo Max",
                data: "goalBudgetInput.metered"
            },
            {
                title: "Precio aprox.:",
                data: "goalBudgetInput.unitPrice"
            },
            {
                title: "Para se usado en:",
                data: "workFront.code"
            },
            {
                title: "Observaciones",
                data: "observations"
            },
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
        buttons: []
    };

    var datatables = {
        init: function () {
            this.mainDt.init();
            this.itemDt.init();
        },
        mainDt: {
            init: function () {
                mainDatatable = $("#main_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                mainDatatable.ajax.reload();
            },
            initEvents: function () {
                mainDatatable.on("change", ".select2-status", function () {
                    let id = $(this).data("id");
                    let value = $(this).val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${id}/actualizar-estado`),
                        method: "post",
                        data: {
                            status: value
                        }
                    }).done(function () {
                        this.reload();
                        _app.show.notification.edit.success();
                    }).fail(function () {
                        _app.show.notification.edit.error();
                    });
                });

                mainDatatable.on("click",
                    ".btn-items",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        datatables.itemDt.reload();
                        form.load.items.detail(id);
                    });

                mainDatatable.on("click",
                    ".btn-files",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.file(id);
                    });

                mainDatatable.on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        window.location = `/logistica/requerimientos/excel/${id}`;
                    });


                mainDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.request.edit(id);
                    });


                mainDatatable.on("click",
                    ".btn-approved",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Requerimiento será aprobado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, aprobarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/aprobar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido aprobado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                            datatables.mainDt.reload();
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar aprobar el requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
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
                                            datatables.mainDt.reload();
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
        itemDt: {
            init: function () {
                itemsDatatable = $("#items_datatable").DataTable(itemOpts);
                this.initEvents();
            },
            reload: function () {
                itemsDatatable.ajax.reload();
            },
            initEvents: function () {
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
                                            datatables.itemDt.reload();
                                            datatables.mainDt.reload();
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
                            formElements.find("[name='RequestDeliveryPlaceId']").val(result.requestDeliveryPlaceId).trigger("change");
                            $("#request_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
            },
            file: function (id) {
                select2.files.reload(id);
                $("#btn_deleteFile").attr("hidden", true);
                $("#file_modal").modal("show");
            },
            items: {
                detail: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${id}`)
                    })
                        .done(function (result) {
                            supplyFamilyId = result.supplyFamilyId;
                            select2.groups.reload();
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();
                            $("#items_add_modal").modal("show");
                        });
                    $.ajax({
                        url: _app.parseUrl(`/select/requerimientos/thecnical-spec/${id}`)
                    }).done(function (result) {
                        tecDiv = document.querySelector("#thecnicalSpec");
                        tecDiv.innerHTML = "";
                        result.forEach(function (item) {
                            tecDiv.innerHTML += item.text + "<br/>";
                        })
                    }).always(function () {
                        _app.loader.hide();
                    });

                    //select2.files.existing(id);

                    $("#detail_modal").modal("show");
                },
                add: function () {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/${requestId}`)
                    })
                        .done(function (result) {

                            supplyFamilyId = result.supplyFamilyId;
                            select2.groups.reload();
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();

                            $("#detail_modal").modal("hide");
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
                            formElements.find("[name='Measure']").val(result.measure);
                            formElements.find("[name='Observations']").val(result.observations);
                            formElements.find("[name='UsedFor']").val(result.usedFor);
                            formElements.find("#Edit_GroupId").val(result.goalBudgetInput.supply.supplyGroupId).trigger("change");

                            select2.budgetsInputs.reloadEdit();

                            formElements.find("[name='GoalBudgetInputId']").val(result.goalBudgetInputId).trigger("change");

                            inEdit = true;

                            console.log("workfront" + result.workFrontId);
                            console.log("group" + result.goalBudgetInput.supply.supplyGroupId);

                            $.ajax({
                                url: _app.parseUrl(`/oficina-tecnica/meta-insumos/obtener-unidad/${result.goalBudgetInputId}`)
                            }).done(function (result) {
                                formElements.find("[name='MeasureUnit']").val(result);
                            });
                            $.ajax({
                                url: _app.parseUrl(`/logistica/requerimientos/insumo-meta/${result.goalBudgetInputId}`)
                            }).done(function (result) {
                                limit = result;
                                formElements.find("#limite").text("(límite: " + limit + ")");
                            });

                            $("#detail_modal").modal("hide");
                            $("#edit_item_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            request: {
                edit: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/editar/${id}`),
                        method: "put",
                        data: data,
                        contentType: false,
                        processData: false,
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.mainDt.reload();
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
                            datatables.itemDt.reload();
                            datatables.mainDt.reload();
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
                            datatables.itemDt.reload();
                            datatables.mainDt.reload();
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
                }
            },
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
                        datatables.mainDt.reload();
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
                        datatables.mainDt.reload();
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
            request: {
                edit: function () {
                    requestEditForm.resetForm();
                    $("#request_edit_form").trigger("reset");
                    $("#request_edit_alert").removeClass("show").addClass("d-none");
                }
            },
            items: {
                add: function () {
                    //itemsForm.resetForm();
                    addItemForm.resetForm();
                    $("#items_add_form").trigger("reset");
                    $("#items_add_alert").removeClass("show").addClass("d-none");
                    $("#items_add_modal").modal("show");
                },
                edit: function () {
                    //itemsForm.resetForm();
                    editItemForm.resetForm();
                    $("#items_add_form").trigger("reset");
                    $("#items_add_alert").removeClass("show").addClass("d-none");
                    $("#items_add_modal").modal("show");
                    inEdit = false;
                }
            },
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $('[data-repeater-list]').empty();
                $('[data-repeater-create]').click();
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
            }
        }
    };

    var select2 = {
        init: function () {
            this.budgetTypes.init();
            this.types.init();
            this.supplyFamilies.init();
            this.budgetTitles.init();
            this.users.init();
            this.families.init();
            this.formulas();
            this.deliveryPlaces.init();
            this.groups.init();
            this.budgetsInputs.init();
            this.workfronts();
        },
        status: {
            init: function () {
                $(".select2-status").select2();

                $(".select2-status-filter").select2();
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
                    $(".select2-budget-title-filter").select2({
                        data: result
                    });
                });
            }
        },
        budgetsInputs: {
            init: function () {
                $(".select2-goal-budget-inputs").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-de-meta`)
                }).done(function (result) {
                    $(".select2-goal-budget-inputs").select2({
                        data: result
                    });
                    let id = $(".select2-goal-budget-inputs").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/meta-insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("[name='MeasureUnit']").val(result);
                    });
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#add_item_form #limite").text("(límite: " + limit + ")");
                        $("#edit_item_form #limite").text("(límite: " + limit + ")");
                    });
                });
            },
            reload: function () {
                let gId = $("#Add_GroupId").val();
                let wId = $("#Add_WorkFrontId").val();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-de-meta?groupId=${gId}&workFrontId=${wId}`)
                }).done(function (result) {
                    $("#Add_GoalBudgetInputId").empty();
                    $("#Add_GoalBudgetInputId").select2({
                        data: result
                    });
                });
            },
            reloadEdit: function () {
                let gId = $("#Edit_GroupId").val();
                let wId = $("#Edit_WorkFrontId").val();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-de-meta?groupId=${gId}&workFrontId=${wId}`)
                }).done(function (result) {
                    $("#Edit_GoalBudgetInputId").empty();
                    $("#Edit_GoalBudgetInputId").select2({
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

                    $(".select2-user-filter").select2({
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
        formulas: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
        },
        deliveryPlaces: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/requerimientos/lugares-entrega")
                }).done(function (result) {
                    $(".select2-delivery-places").select2({
                        data: result
                    });
                });
            }
        },
        budgetTypes: {
            init: function () {
                $(".select2-budget-types").select2();
            }
        },
        types: {
            init: function () {
                $(".select2-types").select2();
                $(".select2-type-filter").select2();
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
        groups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos")
                }).done(function (result) {
                    $(".select2-groups").select2({
                        data: result
                    });
                });
            },
            reload: function () {
                let fId = $("#family_input_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/grupos-de-insumos?familyId=${supplyFamilyId}`)
                }).done(function (result) {
                    $(".select2-groups").empty();
                    $(".select2-groups").select2({
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
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var validate = {
        init: function () {
            requestEditForm = $("#request_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.request.edit(formElement);
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
        }
    };

    var modals = {
        init: function () {
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
        }
    };

    var events = {
        init: function () {
            $("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#add_item_form [name='GoalBudgetInputId']").attr("id", "Add_GoalBudgetInputId");
            $("#edit_item_form [name='GoalBudgetInputId']").attr("id", "Edit_GoalBudgetInputId");

            $("#add_item_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_item_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#add_item_form #group_input_filter").attr("id", "Add_GroupId");
            $("#edit_item_form #group_input_filter").attr("id", "Edit_GroupId");


            $("#Add_GoalBudgetInputId").on("change", function () {
                let id = this.value;
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/meta-insumos/obtener-unidad/${id}`)
                }).done(function (result) {
                    $("#add_item_form [name='MeasureUnit']").val(result);
                });
                $.ajax({
                    url: _app.parseUrl(`/logistica/requerimientos/insumo-meta/${id}`)
                }).done(function (result) {
                    limit = result;
                    $("#add_item_form #limite").text("(límite: " + limit + ")");
                });
            });


            $("#addNewItem").on("click", function () {
                form.load.items.add();
            });

            /*
            $("#family_input_filter").on("change", function () {
                select2.groups.reload();
                select2.budgetsInputs.reload();
            });
*/
            $("#Add_WorkFrontId").on("change", function () {
                select2.budgetsInputs.reload();
            });

            $("#Add_GroupId").on("change", function () {
                select2.budgetsInputs.reload();
            });

            $("#Edit_WorkFrontId").on("change", function () {
                if (inEdit == true)
                    select2.budgetsInputs.reloadEdit();
            });

            $("#Edit_GroupId").on("change", function () {
                if (inEdit == true)
                    select2.budgetsInputs.reloadEdit();
            });

            $("#Edit_GoalBudgetInputId").on("change", function () {
                let id = this.value;
                if (inEdit == true) {
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/meta-insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("#edit_item_form [name='MeasureUnit']").val(result);
                    });
                    $.ajax({
                        url: _app.parseUrl(`/logistica/requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#edit_item_form #limite").text("(límite: " + limit + ")");
                    });
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
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatables.init();
            validate.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Request.init();
});