var SupplyEntry = function () {

    var mainDatatable = null;
    var itemDatatable = null;
    var itemEditDatatable = null;
    var detailDatatable = null;
    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var Id = null;
    var importFilesForm = null;
    var importInvoicesForm = null;

    var list = [];

    var totalCost = 0;
    var totalDolarCost = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    });

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/ingreso-material/listar"),
            data: function (d) {
                d.providerId = $("#provider_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.fileStatus = $("#file_filter").val();
                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                totalCost = 0;
                totalDolarCost = 0;

                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {

                        totalCost += item.parcial;
                        totalDolarCost += item.dolarParcial;
                    });
                }

                return [$("#total_parcial").val(formatter.format(totalCost)),
                $("#total_dolar_parcial").val(formatter2.format(totalDolarCost))];
            }
        },
        columns: [
            {
                title: "N° Documento",
                data: "documentNumber"
            },
            {
                title: "Almacén",
                data: "warehouse.address"
            },
            {
                title: "Fecha de Ingreso",
                data: "deliveryDate"
            },
            {
                title: "Guía de Remisión",
                data: "remissionGuideName"
            },
            {
                title: "Proveedor",
                data: "order.provider.businessName"
            },
            {
                title: "Orden de Compra",
                data: "order.correlativeCodeStr"
            },
            {
                title: "Fecha Orden de Compra",
                data: "order.date"
            },
            {
                title: "Monto S/",
                data: "parcialString"
            },
            {
                title: "Monto US$",
                data: "dolarParcialString"
            },
            {
                title: "Monto S/ Aux",
                data: "parcial",
                visible: false
            },
            {
                title: "Monto US$ Aux",
                data: "dolarParcial",
                visible: false
            },
            {
                title: "Estado",
                data: "status",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.remissionGuideUrl == null) {
                        tmp += `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">ARCHIVO PENDIENTE</span>
								</label>
							</span>`;
                    }
                    if (data == 1) {
                        tmp += `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">EN PROCESO</span>
								</label>
							</span>`;
                    } else {
                        tmp += `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">INGRESADO</span>
								</label>
							</span>`;
                    }
                    
                    return tmp;
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
                    if (row.remissionGuideUrl != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    if (row.invoiceId != null) {
                        tmp += `<button data-id="${row.id}" data-url="${row.id}" class="btn btn-info btn-sm btn-icon btn-invoiceView">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    
                    return tmp;
                }
            },
            {
                title: "Grupo",
                data: "groups"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.status == 1) {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-confirm" title="Ingresar">`;
                        tmp += `<i class="la la-check-circle"></i></button> `;
                    } else {
                        tmp += `<div style="display:inline-block;width:44px"><button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-report" title="Vale de Guia">`;
                        tmp += `<i class="la la-file-text"></i></button></div>`;
                    }
                    if (row.status == 2) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-degrade" title="Degradar">`;
                        tmp += `<i class="la la-arrow-left"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [7, 8] }
        ]
    };

    var itemOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/ingreso-material/detalles/listar"),
            data: function (d) {
                d.ordId = $("#Add_OrderId").val();
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
                title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                title: "Insumo",
                data: "supply.description"
            },
            {
                title: "Und",
                data: "supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Solicitado",
                data: "measure"
            },
            {
                title: "Atención Acumulada",
                data: "measureInAttention"
            },
            {
                title: "Ingreso Actual",
                data: "measureToAttent",
                width: "15%",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}" data-id="${row.id}" class="form-control" type="number" value="${data}">`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Observaciones",
                data: "observations",
                render: function (data, type, row) {
                    var tmp = `<textarea id="${row.id}_obs" data-id="${row.id}" class="form-control" type="text" style="font-size:13px;">`;
                    tmp += `</textarea>`;
                    return tmp;
                }
            }
        ],
        buttons: []
    };

    var detailOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/ingreso-material/detalles/listar"),
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
                title: "Código IVC",
                data: "orderItem.supply.fullCode"
            },
            {
                title: "Familia",
                data: "orderItem.supply.supplyFamily.name"
            },
            {
                title: "Grupo",
                data: "orderItem.supply.supplyGroup.name"
            },
            {
                title: "Insumo",
                data: "orderItem.supply.description"
            },
            {
                title: "Und",
                data: "orderItem.supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Solicitado",
                data: "orderItem.measure"
            },
            {
                title: "Atención Acumulado",
                data: "previousAttention"
            },
            {
                title: "Atención Actual",
                data: "measure",
                width: "15%",
                render: function (data, type, row) {
                    if (row.isEditable == true) {
                        var tmp = `<input id="${row.id}" data-id="${row.id}" class="form-control" type="number" value="${data}">`;
                        tmp += `</input>`;
                        return tmp;
                    } else {
                        return data;
                    }
                }
            },
            {
                title: "QR",
                data: "id",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isEditable == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-qr">`;
                        tmp += `<i class="fa fa-qrcode"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Observaciones",
                data: "observations",
                render: function (data, type, row) {
                    var tmp = `<textarea id="${row.id}_obs" data-id="${row.id}" class="form-control" type="text" style="font-size:13px;">`;
                    tmp += `${data}</textarea>`;
                    return tmp;
                }
            }
        ],
        buttons: []
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            itemDatatable = $("#items_datatable").DataTable(itemOpts);
            detailDatatable = $("#detail_datatable").DataTable(detailOpts);
            //itemEditDatatable = $("#itemsEdit_datatable").DataTable(itemEditOpts);
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
        detailReload: function () {
            detailDatatable.ajax.reload();
        },
        initEvents: function () {

            mainDatatable.on("click",
                ".btn-report",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");

                    window.location = `/almacenes/ingreso-material/vale/${id}`;
                });

            detailDatatable.on("click",
                ".btn-qr",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    window.location = `/almacenes/ingreso-material/qr/${id}`;
                });

            mainDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            mainDatatable.on("click",
                ".btn-details",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    $.ajax({
                        url: _app.parseUrl(`/almacenes/ingreso-material/${Id}`)
                    }).done(function (result) {
                        $("#detail_name").text(result.remissionGuideName);
                        datatable.detailReload();
                        $("#detail_modal").modal("show");
                    });
                });

            mainDatatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    form.load.pdf(id);
                });

            mainDatatable.on("click",
                ".btn-invoiceView",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    form.load.pdf2(id);
                });

            mainDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Ingreso de Material será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/almacenes/ingreso-material/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Ingreso de Material ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el Ingreso de Material"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            mainDatatable.on("click",
                ".btn-confirm",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Ingreso será confirmado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, confirmar",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/almacenes/ingreso-material/confirmar/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Ingreso ha sido confirmado con éxito",
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
                                            text: errormessage.responseText
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            mainDatatable.on("click",
                ".btn-degrade",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    requestId = id;
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El siguiente Ingreso por Compra será degradado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, degradar",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/almacenes/ingreso-material/degradar/${id}`),
                                    type: "put",
                                    success: function (result) {
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Ingreso por Compra ha sido degradado con éxito",
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
                                            text: "Ocurrió un error al intentar degradar el Ingreso por Compra"
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
                    url: _app.parseUrl(`/almacenes/ingreso-material/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_order']").val(result.orderId).trigger("change");
                        formElements.find("[name='select_warehouse']").val(result.warehouseId).trigger("change");
                        formElements.find("[name='select_invoice']").val(result.invoiceId).trigger("change");
                        formElements.find("[name='provider_name']").val(result.order.provider.businessName);
                        formElements.find("[name='RemissionGuide']").val(result.remissionGuide);
                        formElements.find("[name='DeliveryDate']").datepicker('setDate', result.deliveryDate);

                        formElements.find("[name='RemissionGuideName']").val(result.remissionGuideName);

                        if (result.remissionGuideUrl) {
                            $("#edit_form [for='RemissionGuide']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='RemissionGuide']").text("Selecciona un archivo");
                        }

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/ingreso-material/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.remissionGuideName );
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.remissionGuideUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdf2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/ingreso-material/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.remissionGuideName);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.invoice.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='OrderId']").val($(formElement).find("[name='select_order']").val());
                $(formElement).find("[name='WarehouseId']").val($(formElement).find("[name='select_warehouse']").val());
                $(formElement).find("[name='InvoiceId']").val($(formElement).find("[name='select_invoice']").val());
                let data = new FormData($(formElement).get(0));

                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                list.forEach(function (item) {
                    let number = $(`#${item}`).val();
                    //let price = $(`#${item}_price`).val();
                    let obs = $(`#${item}_obs`).val();
                    data.append('Items', item + "|" + number + "|" + obs);
                });
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);

                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/almacenes/ingreso-material/crear"),
                    method: "post",
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
                $(formElement).find("[name='OrderId']").val($(formElement).find("[name='select_order']").val());
                $(formElement).find("[name='WarehouseId']").val($(formElement).find("[name='select_warehouse']").val());
                $(formElement).find("[name='InvoiceId']").val($(formElement).find("[name='select_invoice']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/almacenes/ingreso-material/editar/${id}`),
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
            detail: function (formElement) {
                let data = new FormData($(formElement).get(0));

                _app.loader.show();
                list.forEach(function (item) {
                    let number = $(`#${item}`).val();
                    //let price = $(`#${item}_price`).val();
                    //let glosa = $(`#${item}_glosa`).val();
                    let obs = $(`#${item}_obs`).val();
                    data.append('Items', item + "|" + number + "|" + obs);
                });
                //let $btn = $(formElement).find("button[type='submit']");
                //$btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/almacenes/ingreso-material/editar-items/${Id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        //$btn.removeLoader();

                        _app.loader.hide();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        $("#detail_modal").modal("hide");
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
                    url: `/almacenes/ingreso-material/importar`,
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
                    $("#import_modal").modal("hide");
                    datatable.reload();
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            files: function (formElement) {
                console.log("files");
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
                    url: "/almacenes/ingreso-material/importar-archivos",
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
                    $("#import_files_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_files_alert_text").html(error.responseText);
                        $("#import_files_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            invoice: function (formElement) {
                console.log("files");
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
                    url: "/almacenes/ingreso-material/importar-facturas",
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
                    $("#import_invoices_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_invoices_alert_text").html(error.responseText);
                        $("#import_invoices_alert").removeClass("d-none").addClass("show");
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
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $("[name='provider_name']").attr('disabled', 'disabled');

                $("#add_form [for='customFile']").text("Selecciona un archivo");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $("[name='provider_name']").attr('disabled', 'disabled');
            },
            files: function () {
                importFilesForm.reset();
                $("#import_files_form").trigger("reset");
                $("#import_files_alert").removeClass("show").addClass("d-none");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            importInvoices: function () {
                importInvoicesForm.resetForm();
                $("#import_invoices_form").trigger("reset");
                $("#import_invoices_alert").removeClass("show").addClass("d-none");
            },
            detail: function () {
                detailForm.resetForm();
                $("#detail_form").trigger("reset");
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            importFilesForm = $("#import_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.files(formElement);
                }
            });

            importInvoicesForm = $("#import_invoices_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.invoice(formElement);
                }
            });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var select2 = {
        init: function () {
            this.providers.init();
            this.supplyGroups.init();
            this.supplyFamilies.init();
            this.warehouse.init();
            this.order.init();
            this.fileStatus.init();
            this.invoice.init();
        },
        providers: {
            init: function () {
                $(".select2-providers").empty();
                $(".select2-providers").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores-grupos"),
                    data: {
                        supplyGroupId: $("#supply_group_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-providers").select2({
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
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
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
        warehouse: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/almacenes"),
                }).done(function (result) {
                    $(".select2-warehouses").select2({
                        data: result
                    });
                });
            }
        },
        order: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/ordenes"),
                    data: {
                        orderType: 1
                    }
                }).done(function (result) {
                    $(".select2-orders").select2({
                        data: result
                    });
                });
            }
        },
        fileStatus: {
            init: function () {
                $(".select2-file-filter").select2();
            }
        },
        invoice: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/facturas"),
                }).done(function (result) {
                    $(".select2-invoices").select2({
                        data: result
                    });
                });
            }
        },
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

            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    console.log("filessaasd");
                    form.reset.files();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='BudgetTitleId']").attr("id", "Add_BudgetTitleId");
            $("#edit_form [name='BudgetTitleId']").attr("id", "Edit_BudgetTitleId");

            $("#add_form [name='ProjectFormulaId']").attr("id", "Add_ProjectFormulaId");
            $("#edit_form [name='ProjectFormulaId']").attr("id", "Edit_ProjectFormulaId");

            $("#add_form [name='WorkFrontId']").attr("id", "Add_WorkFrontId");
            $("#edit_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");

            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");

            $("#add_form [name='select_order']").attr("id", "Add_OrderId");
            $("#edit_form [name='select_order']").attr("id", "Edit_OrderId");

            $("#add_form [name='provider_name']").attr("id", "Add_ProviderId");
            $("#edit_form [name='provider_name']").attr("id", "Edit_ProviderId");

            $("#Add_OrderId").on("change", function () {
                datatable.itemReload();
                events.provider.add();
            });
            
            $("#add_button").on("click", function () {

                _app.loader.show();
                datatable.itemReload();
                events.provider.add();

            });

            $("#genEsxelSample").on("click", function () {
                window.location = _app.parseUrl(`/almacenes/ingreso-material/excel-modelo`);
            });

            $("#invoiceExcel").on("click", function () {
                window.location = _app.parseUrl(`/almacenes/ingreso-material/excel-modelo-facturas`);
            });

            $("#pdf_download").on("click", function () {
                window.location = _app.parseUrl(`/almacenes/ingreso-material/descargar/${Id}`);
            });


            $("#submit_detail").on("click", function () {
                form.submit.detail();
            });

            $("#provider_filter").on("change", function () {
                datatable.reload();
            });

            $("#supply_group_filter").on("change", function () {
                datatable.reload();
                select2.providers.init();
            });

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.init();
                select2.providers.init();
                datatable.reload();
            });

            $("#file_filter").on("change", function () {
                datatable.reload();
            });
            /*
            $.ajax({
                url: _app.parseUrl(`/almacenes/ingreso-material/correos`),
                method: "get"
            })
                .done(function (result) {
                    console.log("listo");
                })*/
        },
        provider: {
            add: function () {
                let id = $("#Add_OrderId").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/${id}`),
                }).done(function (result) {
                    console.log(result);

                    $("#Add_ProviderId").val(result.provider.businessName);
                }).always(function () {
                    _app.loader.hide();
                });
            }
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    SupplyEntry.init();
});