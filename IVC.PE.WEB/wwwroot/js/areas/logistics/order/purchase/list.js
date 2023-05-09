var Order = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;
    var loadPdfForm = null;
    var importForm = null;
    var itemEditDatatable = null;
    var itemDatatable = null;
    var list = [];
    var Id = null;

    var options = {
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
            url: _app.parseUrl("/logistica/ordenes/compra/listado/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.providerId = $("#provider_filter").val();

                delete d.columns;
            }
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "costCenter",
                orderable: false
            },
            {
                title: "Proyecto",
                data: "abbreviation",
                orderable: false
            },
            {
                title: "Presupuesto",
                data: "budgetTitle",
                visible: false,
                orderable: false
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
                    } else if (data == 8) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">PARCIALMENTE APROBADA</span>
								</label>
							</span>`;
                    } else if (data == 7) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">OBSERVADO</span>
								</label>
						    </span>`;
                    } else if (data == 9) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">PENDIENTE DE RESPUESTA</span>
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
                data: "provider.businessName",
                orderable: false
            },
            {
                title: "N° de Cotización",
                data: "quotationNumber",
                visible: false,
                orderable: false
            },
            {
                title: "Fecha",
                data: "date",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Monto S/",
                data: "parcial",
                orderable: false
            },
            {
                title: "Monto en Divisa",
                data: "dolarParcial",
                orderable: false
            },
            {
                title: "Forma de Pago",
                data: "paymentMethod",
                visible: false,
                orderable: false
            },
            {
                title: "Tiempo de Entrega",
                data: "deliveryTime",
                visible: false,
                orderable: false
            },
            {
                title: "Lugar de Entrega",
                data: "warehouse.address",
                visible: false,
                orderable: false
            },
            {
                //12
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
                visible: false,
                orderable: false
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    //tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-temporal" title="TEMPORAL">`;
                    //tmp += `<i class="bi bi-activity"></i></button> `
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-items" title="Ver Items">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    if (row.status != 3 && row.status != 4 && row.status != 8 && row.status != 9) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title="Editar">`;
                        tmp += `<i class="fa fa-edit"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel" title="Descargar Excel">`;
                    tmp += `<i class="la la-file-excel-o"></i></button> `;
                    if (row.status == 2) {
                        //tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-approved" title="Aprobar">`;
                        //tmp += `<i class="la la-check-circle"></"></i></button> `;
                        //tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-cancel" title="Anular">`;
                        //tmp += `<i class="la la-times-circle"></i></button>`;
                    } else if (row.status == 8) {
                        //tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-approved" title="Aprobar">`;
                        //tmp += `<i class="la la-check-circle"></"></i></button> `;
                    } else if (row.status == 3) {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-load-pdf" title="Cargar Pdf">`;
                        tmp += `<i class="la la-upload"></"></i></button> `;
                    }
                    if (row.pdfFileUrl != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-pdf" title="Descargar Pdf">`;
                        tmp += `<i class="la la-file-pdf-o"></"></i></button> `;
                    }
                    return tmp;
                },
                orderable: false
            }
        ]
    };

    var itemEditOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/compra/detalles/listar"),
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

    var itemOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/compra/detalles/listar"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
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
                data: "glosa"
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
                data: "measure"
            },
            {
                title: "Precio Venta Unitario",
                data: "unitPrice"
            },
            {
                title: "Precio Venta Unitario c/ Dsctos.",
                data: "unitPriceDiscount"
            },
            {
                title: "Descuentos",
                data: function (result) {
                    return "Financiero: " + result.discount.financialDiscount + "%</br>" +
                           "Item:       " + result.discount.itemDiscount + "%</br>" +
                           "Adicional:  " + result.discount.additionalDiscount + "%</br>" +
                           "I.G.V:      " + result.discount.igv + "%</br>" +
                           "I.S.C:      " + result.discount.isc + "%";
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
            itemEditDatatable = $("#itemsEdit_datatable").DataTable(itemEditOpts);
            itemDatatable = $("#items_datatable").DataTable(itemOpts);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        itemEditReload: function () {
            itemEditDatatable.ajax.reload();
        },
        itemReload: function () {
            console.log("asdasd");
            itemDatatable.ajax.reload();
        },
        initEvents: function () {
            mainDatatable.on("click",
                ".btn-view",
                function () {
                let $btn = $(this);
                let url = $btn.data("url");
                let name = $btn.data("name");
                form.load.pdf(name, url);
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
                ".btn-items",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    form.load.items(id);
                });

            mainDatatable.on("click",
                ".btn-excel",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    window.location = `/logistica/ordenes/excel/${id}`;
                });

            mainDatatable.on("click",
                ".btn-load-pdf",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    $("#pdf_load_modal").modal("show");
                });

            mainDatatable.on("click",
                ".btn-pdf",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    window.location = `/logistica/ordenes/pdf/${id}`;
                });

            /*
            mainDatatable.on("change", ".select2-status", function () {
                let id = $(this).data("id");
                let value = $(this).val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/compra/${id}/actualizar-estado`),
                    method: "post",
                    data: {
                        status: value
                    }
                }).done(function () {
                    datatable.reload();
                    _app.show.notification.edit.success();
                }).fail(function () {
                    _app.show.notification.edit.error();
                });
            });
            */
            mainDatatable.on("click",
                ".btn-approved",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La Orden de Compra será aprobado",
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
                                    url: _app.parseUrl(`/logistica/ordenes/aprobar/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La Orden de Compra ha sido aprobado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                        datatable.reload();
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar aprobar la Orden de Compra"
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
                        text: "La Orden de Compra será anulado",
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
                                    url: _app.parseUrl(`/logistica/ordenes/cancelar/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La Orden de Compra ha sido anulado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                        datatable.reload();
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar anular la Orden de Compra"
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
            items: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#item_modal");

                        datatable.itemReload();
                        formElements.find("[name='QualityCertificate']").prop("checked", result.qualityCertificate);
                        formElements.find("[name='Blueprint']").prop("checked", result.blueprint);
                        formElements.find("[name='SecurityDocument']").prop("checked", result.securityDocument);
                        formElements.find("[name='CalibrationCertificate']").prop("checked", result.calibrationCertificate);
                        formElements.find("[name='CatalogAndStorageCriteria']").prop("checked", result.catalogAndStorageCriteria);
                        formElements.find("[name='Other']").prop("checked", result.other);
                        console.log(result.otherDescription);
                        if (result.otherDescription == null)
                            formElements.find("[name='OtherDescription']").val(" ");
                        else
                            formElements.find("[name='OtherDescription']").val(result.otherDescription);

                        $("#item_modal").modal("show");

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
            load: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select, textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/cargar-pdf/${Id}`),
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
                        $("#pdf_load_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#pdf_load_alert_text").html(error.responseText);
                            $("#pdf_load_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_RequestId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_ProviderId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_Currency").prop("selectedIndex", 0).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            loadPDF: function () {
                loadPdfForm.resetForm();
                $("#pdf_load_form").trigger("reset");
                $("#pdf_load_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.currencies.init();
            this.requests.init();
            this.providers.init();
            this.providers.edit();
            this.warehouses.init();
            this.supplies.fetchAndInit();
            this.measurementUnits.fetchAndInit();
            this.providers.filter();
            this.supplyFamily.init();
            this.supplyGroup.init();
            this.supplyGroup.add();
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
                        reqType: 1
                    }
                }).done(function (result) {
                    $(".select2-requests").select2({
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
                    $(".select2-families-filter").select2({
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
            },
            add: function () {
                $("#supply_group_add").empty();
                $("#supply_group_add").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_add").val()
                    }
                }).done(function (result) {
                    $("#supply_group_add").select2({
                        data: result
                    });
                });
            }
        },
        providers: {
            filter: function () {
                $(".select2-providers-filter").empty();
                $(".select2-providers-filter").append(`<option>Todos</option>`);
                let id = $("#supply_group_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/lista-proveedores-grupos?supplyGroupId=${id}`),
                }).done(function (result) {
                    $(".select2-providers-filter").select2({
                        data: result
                    });
                });
            },
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

            loadPdfForm = $("#pdf_load_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.load(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });

            $("#pdf_load_modal").on("hidden.bs.modal",
                function () {
                    form.reset.loadPDF();
                });
            $("item_modal").on("hidden.bs.modal",
                function () {
                    
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

            $("#add_form [name='CorrelativeCodeManual']").attr("id", "Add_CorrelativeCodeManual");
            $("#edit_form [name='CorrelativeCodeManual']").attr("id", "Edit_CorrelativeCodeManual");

            $("#supply_family_filter").on("change", function () {             
                select2.supplyGroup.init();
                datatable.reload();
            });

            $("#supply_group_filter").on("change", function () {
                select2.providers.filter();
                datatable.reload();              
            });

            $("#provider_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_family_add").on("change", function () {
                select2.supplyGroup.add();
            });

            $("#supply_group_add").on("change", function () {
                select2.providers.add();
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