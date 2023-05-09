var PreRequest = function () {

    var requestId = null;

    var newRequest = false;
    var isDeliveryEdit = false;
    var isItemsEdit = false;

    var limit = null;
    var inEdit = false;

    var deliveryPlacesDatatable = null;
    var requestsDatatable = null;
    var itemsDatatable = null;
    var summaryDatatable = null;

    var addItemForm = null;
    var editItemForm = null;
    var deliveryForm = null;
    var itemsForm = null;
    var summaryForm = null;
    var obsForm = null;
    var requestAddForm = null;

    var supplyFamilyId = null;
    var projectFormulaId = null;


    var reqOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/pre-requerimientos/listar"),
            dataSrc: ""
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
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">PROCESADO</span>
								</label>
							</span>`;
                    } else if (data == 4) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--secondary">PROCESADO PARCIALMENTE</span>
								</label>
							</span>`;
                    }
                    else if (data == 6) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">OBSERVADO</span>
								</label>
							</span>`;
                    } else if(data == 7) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">PENDIENTE DE RESPUESTA</span>
								</label>
							</span>`;
                    } else if (data == 8) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO</span>
								</label>
							</span>`;
                    } else if (data == 9) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO PARCIALMENTE</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">ANULADO</span>
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
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-items" title="Ver Items">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-files" title="Ver Archivos">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    if (row.observations != null && row.observations != "") {
                        tmp += `<button data-id="${row.id}" data-name="${row.observations}" class="btn btn-primary btn-sm btn-icon btn-read-observation" title="Motivo Observación">`;
                        tmp += `<i class="la la-comment"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.orderStatus != 6) {
                        tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-issue" title="Emitir">`;
                        tmp += `<i class="fas fa-arrow-right"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
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
            url: _app.parseUrl("/logistica/pre-requerimientos/detalles/listar"),
            data: function (d) {
                d.reqId = requestId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Grupo",
                data: function (result) {
                    var group = result.supplyGroupStr;

                    if (group == "")
                        return `<b style="color:rgb(255, 25, 25);" >Ingreso Manual</b>`;
                    else
                        return group;
                }
            },
            {
                title: "Material",
                data: function (result) {
                    var supply = result.supply.description;

                    if (supply == "")
                        return result.supplyName
                    else
                        return supply;
                }
            },
            {
                title: "Unidad",
                data: function (result) {
                    var unit = result.supply.measurementUnit.abbreviation;

                    if (unit == "")
                        return result.measurementUnitName;
                    else
                        return unit;
                }
            },
            {
                title: "Metrado",
                data: "measure"
            },
            /*
            {
                title: "Techo Max",
                data: "goalBudgetInput.metered"
            },
            {
                title: "Precio aprox.:",
                data: "goalBudgetInput.unitPrice"
            },
            */
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

    var datatables = {
        init: function () {
            this.preReqDt.init();
            this.iteDt.init();
        },
        preReqDt: {
            init: function () {
                requestsDatatable = $("#pre_requests_datatable").DataTable(reqOpts);
                this.events();
            },
            reload: function () {
                requestsDatatable.ajax.reload();
            },
            events: function () {
                requestsDatatable.on("click",
                    ".btn-items",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.items.detail(id);
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

                        let bUrl = _app.parseUrl(`/logistica/pre-requerimientos/emitir-requerimiento-doc/${id}`);
                        window.open(`/logistica/pre-requerimientos/descargar-requerimiento?url=${bUrl}`);
                    });

                requestsDatatable.on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        window.location = `/logistica/pre-requerimientos/excel/${id}`;
                    });

                requestsDatatable.on("click",
                    ".btn-issue",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Pre-Requerimiento será emitido",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, emitir Pre-Requerimiento",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/emitir/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            datatables.iteDt.reload();
                                            datatables.preReqDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Pre-Requerimiento ha sido emitido con éxito",
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
                                                text: "Ocurrió un error al intentar emitir el Pre-Requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                requestsDatatable.on("click",
                    ".btn-read-observation",
                    function () {
                        let $btn = $(this);
                        let name = $btn.data("name");
                        $("#observation_read").html(name);
                        $("#observation_read_modal").modal("show");
                    });


                requestsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Pre-Requerimiento será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.iteDt.reload();
                                            datatables.preReqDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Pre-Requerimiento ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Pre-Requerimiento"
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
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.iteDt.reload();
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
                        url: _app.parseUrl(`/logistica/pre-requerimientos/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#request_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='RequestType']").val(result.requestType).trigger("change");
                            formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId).trigger("change");
                            formElements.find("[name='PreRequestUserIds']").val(result.preRequestUserIds)
                            $(".select2-users").trigger("change");
                            formElements.find("[name='DeliveryDate']").datepicker('setDate', result.deliveryDate);
                            //formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                            formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId).trigger("change");
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
                        url: _app.parseUrl(`/logistica/pre-requerimientos/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#obs_form");
                            datatables.iteDt.reload();
                            $("#type").text(result.requestTypeStr);
                            $("#code").text(result.correlativeCodeStr);
                            $("#user").text(result.issuedUserName);
                            formElements.find("[name='Observations']").val(result.observations);
                            
                            //supplyFamilyId = result.supplyFamilyId;
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();
                            $("#detail_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                add: function () {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/${requestId}`)
                    })
                        .done(function (result) {

                            //supplyFamilyId = result.supplyFamilyId;

                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();

                            //setTimeout(() => {
                            //    select2.supplies.reload();
                            //}, 1000);

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
                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#edit_item_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='PreRequestId']").val(result.preRequestId);
                            formElements.find("[name='WorkFrontId']").val(result.workFrontId).trigger("change");
                            formElements.find("[name='Measure']").val(result.measure);
                            formElements.find("[name='Observations']").val(result.observations);
                            formElements.find("[name='UsedFor']").val(result.usedFor);
                            formElements.find("[name='SupplyManual']").val(result.supplyManual);

                            //select2.supplies.reloadEdit();

                            inEdit = true;

                            if (result.supplyManual == false) {
                                $.ajax({
                                    url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                                }).done(function (result) {
                                    formElements.find("[name='MeasurementUnitName']").val(result);
                                });

                                setTimeout(() => {
                                    formElements.find("[name='SupplyId']").val(result.supplyId).trigger("change");
                                }, 1000);

                            } else {
                                $("#Edit_SupplyManual").prop('checked', true);
                                $(".insumo").hide();
                                $(".manual").show();
                                formElements.find("[name='MeasurementUnitName']").val(result.measurementUnitName);
                                formElements.find("[name='SupplyName']").val(result.supplyName);
                            }

                            $("#detail_modal").modal("hide");
                            setTimeout(() => {
                                $("#edit_item_modal").modal("show");
                            }, 1000);
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                import: function () {
                    _app.loader.show();
                    $("#detail_modal").modal("hide");
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
                        url: _app.parseUrl("/logistica/pre-requerimientos/crear"),
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
                            datatables.preReqDt.reload();
                            requestId = result;
                            newRequest = true;
                            $("#request_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#request_add_alert_text").html(error.responseText);
                                $("#request_add_alert").removeClass("d-none").addClass("show");
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
                        url: _app.parseUrl(`/logistica/pre-requerimientos/editar/${id}`),
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
                            datatables.preReqDt.reload();
                            requestId = result;
                            newRequest = false;
                            inEdit = false;
                            $("#request_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#request_edit_alert_text").html(error.responseText);
                                $("#request_edit_alert").removeClass("d-none").addClass("show");
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
                        url: _app.parseUrl(`/logistica/pre-requerimientos/obs/${id}`),
                        method: "put",
                        data: data,
                        contentType: false,
                        processData: false
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
                            datatables.preReqDt.reload();
                            $("#detail_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#detail_alert_text").html(error.responseText);
                                $("#detail_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            },
            items: {
                add: function (formElement) {
                    $(formElement).find("[name='PreRequestId']").val(requestId);
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/crear`),
                        method: "post",
                        data: data,
                        contentType: false,
                        processData: false
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);

                            $(".manual").hide();
                            $(".insumo").show();
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
                    data.set('SupplyManual', $("#Edit_SupplyManual").val());
                    console.log(data);
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/editar/${id}`),
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
                            $("#Edit_SupplyManual").prop('checked', false);
                            $(".manual").hide();
                            $(".insumo").show();

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
                        url: `/logistica/pre-requerimientos/importar-items/${requestId}`,
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
                    $(".select2-users").val('').trigger("change");
                    $("#request_add_form").trigger("reset");
                    $("#request_add_alert").removeClass("show").addClass("d-none");
                    if (newRequest) {
                        newRequest = false;
                        form.load.items.detail(requestId);
                    }
                },
                edit: function () {
                    requestEditForm.resetForm();
                    $("#request_edit_form").trigger("reset");
                    $("#request_edit_alert").removeClass("show").addClass("d-none");
                },
                file: function () {
                    $("#gdocs_frame").attr("hidden", true);
                    $("#files_pdf").attr("hidden", true);
                }
            },
            items: {
                add: function () {
                    //itemsForm.resetForm();
                    addItemForm.resetForm();
                    $("#detail_form").trigger("reset");
                    $("#detail_alert").removeClass("show").addClass("d-none");
                    $("#detail_modal").modal("show");
                },
                edit: function () {
                    //itemsForm.resetForm();
                    editItemForm.resetForm();
                    $("#detail_form").trigger("reset");
                    $("#detail_alert").removeClass("show").addClass("d-none");
                    $("#detail_modal").modal("show");
                    inEdit = false;
                },
                import: function () {
                    importForm.resetForm();
                    $("#import_form").trigger("reset");
                    $("#import_alert").removeClass("show").addClass("d-none");
                    $("#detail_modal").modal("show");
                }
            },
            
            editItem: function () {
                editItemForm.resetForm();
                $("#edit_item_form").trigger("reset");
                $("#edit_item_alert").removeClass("show").addClass("d-none");
                $("#detail_modal").modal("show");
            }
            
        }
    };

    var select2 = {
        init: function () {
            this.requestOrderTypes.init();
            this.budgetTitles.init();
            this.users.init();
            this.supplies.init();
            this.projects.init();
            this.families.init();
            //this.groups.init();
            this.formulas.init();
            this.workfronts();
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
            init: function () {
                $(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero`)
                }).done(function (result) {
                    $(".select2-supplies").select2({
                        data: result
                    });
                    let id = $(".select2-supplies").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("[name='MeasurementUnitName']").val(result);
                    });
                    /*
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#add_item_form #limite").text("(límite: " + limit + ")");
                        $("#edit_item_form #limite").text("(límite: " + limit + ")");
                    });
                    */
                });
            },
            reload: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero`),
                    //data: {
                    //    supplyFamilyId: supplyFamilyId
                    //}
                }).done(function (result) {
                    $("#Add_SupplyId").empty();
                    $("#Add_SupplyId").select2({
                        data: result
                    });
                });
            },
            reloadEdit: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero`),
                    //data: {
                    //    supplyFamilyId: supplyFamilyId
                    //}
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
        //groups: {
        //    init: function () {
        //        $.ajax({
        //            url: _app.parseUrl("/select/grupos-de-insumos")
        //        }).done(function (result) {
        //            $(".select2-groups").select2({
        //                data: result
        //            });
        //        });
        //    },
        //    reload: function () {
        //        let fId = $("#family_input_filter").val();
        //        $.ajax({
        //            url: _app.parseUrl(`/select/grupos-de-insumos?familyId=${supplyFamilyId}`)
        //        }).done(function (result) {
        //            $(".select2-groups").empty();
        //            $(".select2-groups").select2({
        //                data: result
        //            });
        //        });
        //    }
        //},
        files: {
            init: function () {
                $(".select2-files").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/pre-requerimientos/archivos-tspec/${id}`)
                }).done(function (result) {
                    $(".select2-files").empty();
                    $(".select2-files").select2({
                        data: result
                    });
                });
            },
            existing: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/pre-requerimientos/archivos/${id}`)
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


            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.items.import(formElement);
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

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.import();
                });

            $("#edit_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.edit();
                });
        }
    };

    var events = {
        init: function () {
            $("#request_add_form [name='RequestType']").attr("id", "Add_RequestType");
            $("#request_add_form [name='BudgetTitleId']").attr("id", "Add_BudgetTitleId");
            $("#request_add_form [name='PreRequestUserIds']").attr("id", "Add_RequestUserIds");
            $("#request_add_form [name='ProjectFormulaId']").attr("id", "Add_ProjectFormulaId");

            $("#request_edit_form [name='RequestType']").attr("id", "Edit_RequestType");
            $("#request_edit_form [name='BudgetTitleId']").attr("id", "Edit_BudgetTitleId");
            $("#request_edit_form [name='PreRequestUserIds']").attr("id", "Edit_RequestUserIds");
            $("#request_edit_form [name='ProjectFormulaId']").attr("id", "Edit_ProjectFormulaId");

            $("#add_item_form [name='SupplyId']").attr("id", "Add_SupplyId");
            $("#edit_item_form [name='SupplyId']").attr("id", "Edit_SupplyId");

            $("#add_item_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_item_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#add_item_form [name='SupplyManual']").attr("id", "Add_SupplyManual");
            $("#edit_item_form [name='SupplyManual']").attr("id", "Edit_SupplyManual");

            $("#Add_SupplyId").on("change", function () {
                let id = this.value;
                console.log(id);
                $.ajax({
                    url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                }).done(function (result) {
                    $("#add_item_form [name='MeasurementUnitName']").val(result);
                });
                /*
                $.ajax({
                    url: _app.parseUrl(`/logistica/pre-requerimientos/insumo-meta/${id}`)
                }).done(function (result) {
                    limit = result;
                    $("#add_item_form #limite").text("(límite: " + limit + ")");
                });
                */
            });


            $("#addNewItem").on("click", function () {
                form.load.items.add();
            });

            /*
            $("#family_input_filter").on("change", function () {
                select2.groups.reload();
                select2.supplies.reload();
            });
*/
            //$("#Add_WorkFrontId").on("change", function () {
            //    select2.supplies.reload();
            //});

            /*
            $("#Add_GroupId").on("change", function () {
                select2.supplies.reload();

                setTimeout(() => {
                    let id = $("#Add_SupplyId").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/meta-insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("#add_item_form [name='MeasurementUnitName']").val(result);
                    });
                    /*
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#add_item_form #limite").text("(límite: " + limit + ")");
                    });
                }, 1000);
            });
                    */

            //$("#Edit_WorkFrontId").on("change", function () {
            //    if (inEdit == true)
            //        select2.supplies.reloadEdit();
            //});
            /*
            $("#Edit_GroupId").on("change", function () {
                if (inEdit == true)
                    select2.supplies.reloadEdit();
            });
            */
            $("#Edit_SupplyId").on("change", function () {
                let id = this.value;
                if (inEdit == true) {
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("#edit_item_form [name='MeasurementUnitName']").val(result);
                    });
                    /*
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#edit_item_form #limite").text("(límite: " + limit + ")");
                    });
                    */
                }
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

            $("#btn_downloadFile").on("click", function () {
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
                                    url: _app.parseUrl(`/logistica/pre-requerimientos/archivo/eliminar?url=${uri}`),
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

            $("#genEsxelSample").on("click", function () {
                window.location = _app.parseUrl(`/logistica/pre-requerimientos/excel-modelo/${requestId}`);
            });

            $("#loadItems").on("click", function () {
                form.load.items.import();
            });

            $(".manual").hide();

            $(".supply_manual").on("change", function () {
                var data = $(this);

                if ($(this).is(':checked')) {
                    data.val(true);
                    $(".insumo").hide();
                    $(".manual").show();
                }
                else {
                    data.val(false);
                    $(".manual").hide();
                    $(".insumo").show();
                }

                console.log(data.val());
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
    PreRequest.init();
});