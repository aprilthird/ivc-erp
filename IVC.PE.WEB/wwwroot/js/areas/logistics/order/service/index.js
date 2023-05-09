var Order = function () {

    var mainDatatable = null;
    var itemDatatable = null;
    var itemEditDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var addFormRepeater = null;
    var editFormRepeater = null;
    var requestId = null;
    var list = [];
    var Id = null;

    var options = {
        responsive: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 3, 4, 5, 7, 8, 9],
                hide: [2, 6, 10, 11, 12, 13, 14, 15]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            }
        ],
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/servicio/listar"),
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "costCenter"
            },
            {
                title: "Proyecto",
                data: "abbreviation"
            },
            {
                title: "Presupuesto",
                data: "budgetTitle",
                visible: false
            },
            {
                title: "Estado",
                data: "status",
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
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADA</span>
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
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">PARCIALMENTE APROBADA</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">ANULADA</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "N° de Órden",
                data: "correlativeCodeStr"
            },
            {
                title: "Proveedor",
                data: "provider.businessName"
            },
            {
                title: "N° de Cotización",
                data: "quotationNumber",
                visible: false
            },
            {
                title: "Fecha",
                data: "date",
                render: function (data) {
                    return data || "---";
                }//7
            },
            {
                title: "Monto S/",
                data: "parcial"
            },
            {
                title: "Monto en Divisa",
                data: "dolarParcial"
            },
            {
                title: "Forma de Pago",
                data: "paymentMethod",
                visible: false
            },
            {
                title: "Tiempo de Entrega",
                data: "deliveryTime",
                render: function (data) {
                    return data || "---";
                },
                visible: false
            },
            {
                title: "Lugar de Entrega",
                data: "warehouse.address",
                visible: false
            },
            {
                title: "Cotización (Adjunto)",
                data: "priceFileUrl",
                render: function (data) {
                    var tmp = "";
                    if (data) {
                        tmp += `<button data-url="${data}" data-name="Cotización" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    else {
                        tmp = "Ningún adjunto";
                    }
                    return tmp;
                },
                visible: false
            },
            {
                title: "Sustento (Adjunto)",
                data: "supportFileUrl",
                render: function (data) {
                    var tmp = "";
                    if (data) {
                        tmp += `<button data-url="${data}" data-name="Sustento" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    else {
                        tmp = "Ningún adjunto";
                    }
                    return tmp;
                },
                visible: false
            },
            {
                title: "N° de Requerimiento",
                data: "requestsName",
                visible: false
            },
            {
                title: "Opciones",
                data: null,
                width: "8%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-issue" title="Emitir">`;
                    tmp += `<i class="fas fa-arrow-right"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel" title="Descargar Excel">`;
                    tmp += `<i class="la la-file-excel-o"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    if (row.observations != null && row.observations != "") {
                        tmp += `<button data-id="${row.id}" data-name="${row.observations}" class="btn btn-primary btn-sm btn-icon btn-read-observation" title="Motivo Observación">`;
                        tmp += `<i class="la la-comment"></i></button> `;
                    }
                    return tmp;
                }
            }
        ]
    };

    var itemOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/requerimientos/ordenes/detalles/listar"),
            method: "post",
            data: function (d) {
                d.reqIds = $("#Add_RequestIds").val();
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
                title: "Requerimiento",
                data: "request.correlativeCodeStr"
            },
            {
                title: "Código IVC - Insumo",
                data: function (result) {
                    return result.goalBudgetInput.supply.fullCode + " - " + result.goalBudgetInput.supply.description;
                }
            },
            {
                title: "Glosa",
                data: "glosa",
                render: function (data, type, row) {
                    var tmp = `<textarea id="${row.id}_glosa" data-id="${row.id}" class="form-control" type="text" style="font-size:13px;">`;
                    tmp += `</textarea>`;
                    return tmp;
                }
            },
            {
                title: "Unidad",
                data: "measureUnit"
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
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Precio Venta Unitario",
                width: "15%",
                data: "observations",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}_price" data-id="${row.id}" class="form-control" type="number" >`;
                    tmp += `</input>`;

                    tmp += `<input id="${row.id}_obs" data-id="${row.id}" class="form-control" value="${data}" hidden>`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Observaciones",
                data: "observations"
            }
        ],
        buttons: []
    };

    var itemEditOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/servicio/detalles/listar"),
            method: "get",
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
        "autoWidth": false,
        columns: [
            {
                title: "Requerimiento",
                data: "requestCode"
            },
            {
                title: "Código IVC - Insumo",
                data: function (result) {
                    return result.supply.fullCode + " - " + result.supply.description;
                }
            },
            {
                title: "Glosa",
                data: "glosa",
                render: function (data, type, row) {
                    var tmp = `<textarea id="${row.id}_glosa" data-id="${row.id}" class="form-control" type="text" style="font-size:13px;">`;
                    tmp += `${data}</textarea>`;
                    return tmp;
                }
            },
            {
                title: "Und",
                data: "supply.measurementUnit.abbreviation"
            },
            {
                title: "Solicitado",
                data: "requestMeasure"
            },
            {
                title: "Acumulado",
                data: "requestMeasureInAttention"
            },
            {
                title: "Por Atender",
                data: "measure",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}" data-id="${row.id}" class="form-control" value="${data}" style="width:100%">`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Precio Venta Unitario",
                data: "unitPrice",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}_price" data-id="${row.id}" class="form-control" value="${data}"  style="width:100%">`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Parcial",
                data: "parcial"
            },
            {
                title: "Observaciones",
                data: "requestObservation"
            }
        ],
        buttons: []
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            itemDatatable = $("#items_datatable").DataTable(itemOpts);
            itemEditDatatable = $("#itemsEdit_datatable").DataTable(itemEditOpts);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        itemReload: function () {
            itemDatatable.ajax.reload();
        },
        itemEditReload: function () {
            itemEditDatatable.ajax.reload();
        },
        initEvents: function () {
            mainDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let url = $btn.data("url");
                let name = $btn.data("name");
                form.load.pdf(name, url);
            });

            mainDatatable.on("click",
                ".btn-excel",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    requestId = id;
                    window.location = `/logistica/ordenes/excel/${id}`;
                });

            mainDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    form.load.edit(id);
                });

            mainDatatable.on("click",
                ".btn-issue",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La Orden de servicio será emitido",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, emitir Orden",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/logistica/ordenes/emitir/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La Orden de servicio ha sido emitida con éxito",
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
                                            text: "Ocurrió un error al intentar emitir la Orden de servicio"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            mainDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La órden de servicio será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/logistica/ordenes/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La órden de servicio ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la órden de servicio"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            mainDatatable.on("click",
                ".btn-read-observation",
                function () {
                    let $btn = $(this);
                    let name = $btn.data("name");
                    $("#observation_read").html(name);
                    $("#observation_read_modal").modal("show");
                });

        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");

                        select2.providers.edit(result.requestIds);
                        formElements.find("[name='RequestIds']").val(result.requestIds).trigger("change");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='CorrelativeCode']").val(result.correlativeCode);
                        formElements.find("[name='QuotationNumber']").val(result.quotationNumber);
                        formElements.find("[name='BillTo']").val(result.billTo);
                        formElements.find("[name='PaymentMethod']").val(result.paymentMethod);
                        formElements.find("[name='Warranty']").val(result.warranty);
                        formElements.find("[name='DeliveryPlace']").val(result.deliveryPlace);
                        formElements.find("[name='Observations']").val(result.observations);
                        formElements.find("[name='Date']").datepicker("setDate", result.date);
                        formElements.find("[name='DeliveryTime']").val(result.deliveryTime);

                        $("#request_names").text(result.requestsName);

                        datatable.itemEditReload();

                        setTimeout(function () {
                            formElements.find("#Edit_ProviderId").val(result.providerId).trigger("change");
                        }, 4500);

                        if (result.correlativeCodeManual == false) {
                            $(".manual").hide();
                        } else {
                            $("#Edit_CorrelativeCodeManual").prop('checked', true);
                            $(".manual").show();
                            formElements.find("[name='CorrelativeCode']").val(result.correlativeCode);
                            formElements.find("[name='CorrelativeCodeSuffix']").val(result.correlativeCodeSuffix);
                        }
                        if (result.warehouseCheckbox == false) {
                            $(".new_warehouse").hide();
                            $(".start_warehouse").show();
                        } else {
                            $("#Edit_WarehouseCheckbox").prop('checked', true);
                            $(".new_warehouse").show();
                            $(".start_warehouse").hide();
                            formElements.find("[name='ManualWarehouse']").val(result.manualWarehouse);
                        }

                        formElements.find("#Edit_Currency").val(result.currency).trigger("change");
                        formElements.find("[name='QualityCertificate']").prop("checked", result.qualityCertificate);
                        formElements.find("[name='Blueprint']").prop("checked", result.blueprint);
                        formElements.find("[name='SecurityDocument']").prop("checked", result.securityDocument);
                        formElements.find("[name='CalibrationCertificate']").prop("checked", result.calibrationCertificate);
                        formElements.find("[name='CatalogAndStorageCriteria']").prop("checked", result.catalogAndStorageCriteria);
                        formElements.find("[name='Other']").prop("checked", result.other);
                        formElements.find("[name='OtherDescription']").val(result.otherDescription);
                        formElements.find("[name='Conditions']").val(result.conditions);
                        formElements.find("[name='ExchangeRate']").val(result.exchangeRate);

                        if (result.priceFileUrl) {
                            $("#edit_form [for='PriceFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='PriceFile']").text("Selecciona un archivo");
                        }
                        if (result.supportFileUrl) {
                            $("#edit_form [for='SupportFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='SupportFile']").text("Selecciona un archivo");
                        }
                        if (result.orderItems) {
                            editFormRepeater.setList(result.orderItems);
                            Array.from(document.querySelectorAll("#edit_form [data-repeater-item]"))
                                .forEach((el, i) => {
                                    var requestItem = el.querySelector(".select2-request-items");
                                    $(requestItem).val(result.orderItems[i].requestItemId).trigger("change");
                                    var measurementUnit = el.querySelector(".select2-measurement-units");
                                    $(measurementUnit).val(result.orderItems[i].measurementUnitId).trigger("change");
                                    var supply = el.querySelector(".select2-supplies");
                                    $(supply).val(result.orderItems[i].supplyId).trigger("change");
                                    el.querySelector(`input[name='OrderItems[${i}][Id]']`).value = result.orderItems[i].id;
                                    el.querySelector(`input[name='OrderItems[${i}][Measure]']`).value = result.orderItems[i].measure;
                                    el.querySelector(`input[name='OrderItems[${i}][UnitPrice]']`).value = result.orderItems[i].unitPrice;
                                    el.querySelector(`input[name='OrderItems[${i}][Comment]']`).value = result.orderItems[i].comment;
                                });
                        }

                        $("#edit_modal").modal("show");
                        _app.loader.hide();
                    });
            },
            pdf: function (name, url) {
                $("#preview_name").text(name);
                $("#preview_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${url}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(url)));
                $(".btn-mailto").data("name", name).data("url", "https://docs.google.com/gview?url=" + encodeURI(url));
                $("#preview_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let number = $(`#${item}`).val();
                    let price = $(`#${item}_price`).val();
                    let glosa = $(`#${item}_glosa`).val();
                    let obs = $(`#${item}_obs`).val();
                    data.append('Items', item + "|" + number + "|" + price + "|" + glosa + "|" + obs);
                });
                console.log(data);
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select, textarea").prop("disabled", true);
                var emptyFile = $(formElement).find("[name='PriceFile']").get(0).files.length === 0
                    || $(formElement).find("[name='SupportFile']").get(0).files.length === 0;
                if (!emptyFile) {
                    $(formElement).find("[name='Observations']").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/logistica/ordenes/servicio/crear"),
                    method: "post",
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
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let number = $(`#${item}`).val();
                    let price = $(`#${item}_price`).val();
                    let glosa = $(`#${item}_glosa`).val();
                    data.append('Items', item + "|" + number + "|" + price + "|" + glosa);
                });
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select, textarea").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select, textarea").prop("disabled", false);
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
                    url: "/logistica/ordenes/servicio/importar",
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
                    $("#import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
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
                $("#Add_RequestIds").val('').trigger("change");
                $("#Add_ProviderId").prop("selectedIndex", 0).trigger("change");
                $("#Add_Currency").prop("selectedIndex", 0).trigger("change");
                $(".manual").hide();
                $(".new_warehouse").hide();
                $(".start_warehouse").show();
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_RequestIds").prop("selectedIndex", 0).trigger("change");
                $("#Edit_ProviderId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_Currency").prop("selectedIndex", 0).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.currencies.init();
            this.requests.init();
            this.providers.init();
            this.providers.edit();
            //this.requestItems.fetchAndInit();
            this.warehouses.init();
            this.supplies.fetchAndInit();
            this.measurementUnits.fetchAndInit();
        },
        currencies: {
            init: function () {
                $(".select2-currencies").select2();
            }
        },
        requests: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/requerimientos"),
                    data: {
                        reqType: 2,
                        type: 1
                    }
                }).done(function (result) {
                    $(".select2-requests").select2({
                        data: result
                    });
                });
            }
        },
        providers: {
            init: function () {
                let list = [];
                list = $("#Add_RequestIds").val();
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-requerimientos"),
                    method: "post",
                    data: {
                        reqIds: list
                    }
                }).done(function (result) {
                    $("#Add_ProviderId").select2({
                        data: result
                    });
                });
            },
            edit: function (reqs) {
                let list = [];
                list = reqs;
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-requerimientos"),
                    method: "post",
                    data: {
                        reqIds: list
                    }
                }).done(function (result) {
                    $("#Edit_ProviderId").select2({
                        data: result
                    });
                });
            }
        },
        measurementUnits: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-measurement-units").select2({
                    data: select2.measurementUnits.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    select2.measurementUnits.data = result;
                    callback();
                });
            }
        },
        requestItems: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-request-items").select2({
                    data: select2.requestItems.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl(`/select/requerimientos/${requestId}/elementos`)
                }).done(function (result) {
                    select2.requestItems.data = result;
                    callback();
                });
            }
        },
        supplies: {
            data: null,
            fetchAndInit: function () {
                this.fetch(this.init);
            },
            init: function () {
                $(".select2-supplies").select2({
                    data: select2.supplies.data
                });
            },
            fetch: function (callback) {
                $.ajax({
                    url: _app.parseUrl(`/select/insumos`)
                }).done(function (result) {
                    select2.supplies.data = result;
                    callback();
                });
            }
        },
        warehouses: {
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

            importForm = $("#import_form").validate({
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

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='RequestIds']").attr("id", "Add_RequestIds");
            $("#edit_form [name='RequestIds']").attr("id", "Edit_RequestIds");
            $("#add_form [name='ProviderId']").attr("id", "Add_ProviderId");
            $("#edit_form [name='ProviderId']").attr("id", "Edit_ProviderId");
            $("#add_form [name='Currency']").attr("id", "Add_Currency");
            $("#edit_form [name='Currency']").attr("id", "Edit_Currency");
            $("#add_form [name='WarehouseId']").attr("id", "Add_WarehouseId");
            $("#edit_form [name='WarehouseId']").attr("id", "Edit_WarehouseId");
            $("#add_form .repeater-items").attr("id", "add_repeater_items");
            $("#edit_form .repeater-items").attr("id", "edit_repeater_items");
            $("#add_form [name='Liquidation']").attr("id", "Add_Liquidation");
            $("#edit_form [name='Liquidation']").attr("id", "Edit_Liquidation");
            $("#add_form [name='WarehouseCheckbox']").attr("id", "Add_WarehouseCheckbox");
            $("#edit_form [name='WarehouseCheckbox']").attr("id", "Edit_WarehouseCheckbox");
            $("#add_form [name='CorrelativeCodeManual']").attr("id", "Add_CorrelativeCodeManual");
            $("#edit_form [name='CorrelativeCodeManual']").attr("id", "Edit_CorrelativeCodeManual");

            addFormRepeater = $('#add_repeater_items').repeater({
                initEmpty: false,
                isFirstItemUndeletable: true,
                defaultValues: {
                },
                show: function () {
                    $(this).slideDown();
                    Array.from(document.querySelectorAll("#add_form [data-repeater-item]"))
                        .forEach((el, i) => {
                            var requestItem = el.querySelector(".select2-request-items");
                            requestItem.id = `Add_Request_Item_${i}`;
                            var measurementUnit = el.querySelector(".select2-measurement-units");
                            measurementUnit.id = `Add_Measurement_Unit_${i}`;
                            var supply = el.querySelector(".select2-supplies");
                            supply.id = `Add_Supply_${i}`;
                        });
                    select2.requestItems.init();
                    select2.measurementUnits.init();
                    select2.supplies.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
            });

            editFormRepeater = $('#edit_repeater_items').repeater({
                initEmpty: true,
                isFirstItemUndeletable: false,
                show: function () {
                    $(this).slideDown();
                    Array.from(document.querySelectorAll("#edit_form [data-repeater-item]"))
                        .forEach((el, i) => {
                            var requestItem = el.querySelector(".select2-request-items");
                            requestItem.id = `Edit_Request_Item_${i}`;
                            var measurementUnit = el.querySelector(".select2-measurement-units");
                            measurementUnit.id = `Edit_Measurement_Unit_${i}`;
                            var supply = el.querySelector(".select2-supplies");
                            supply.id = `Edit_Supply_${i}`;
                        });
                    select2.requestItems.init();
                    select2.measurementUnits.init();
                    select2.supplies.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
            });

            $("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatable.reload();
            });

            $("#Add_RequestIds").on("change", function () {
                select2.providers.init();
                datatable.itemReload();
            });

            $("#addNewOrder").on("click", function () {
                select2.requests.init();
            });

            $(".manual").hide();

            $(".correlative_code_manual").on("change", function () {
                var data = $(this);

                if ($(this).is(':checked')) {
                    data.val(true);
                    $(".manual").show();
                }
                else {
                    data.val(false);
                    $(".manual").hide();
                }

                console.log(data.val());
            });

            $(".liquidation").on("change", function () {
                var data = $(this);

                if ($(this).is(':checked')) {
                    data.val(true);
                }
                else {
                    data.val(false);
                }

                console.log(data.val() + "parker");
            });

            $(".new_warehouse").hide();

            $(".manual_warehouse").on("change", function () {
                var data = $(this);

                if ($(this).is(':checked')) {
                    data.val(true);
                    $(".new_warehouse").show();
                    $(".start_warehouse").hide();
                }
                else {
                    data.val(false);
                    $(".new_warehouse").hide();
                    $(".start_warehouse").show();
                }

                console.log(data.val());
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
            datepicker.init();
        }
    };
}();

$(function () {
    Order.init();
});