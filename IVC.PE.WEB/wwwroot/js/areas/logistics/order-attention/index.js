var OrderAttention = function () {

    var mainDatatable = null;
    var detailDatatable = null;
    var itemDatatable = null;

    var detailForm = null;
    var pdfForm = null;
    var itemForm = null;
    var closureForm = null;

    var Id = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/logistica/atencion-ordenes/listar"),
            data: function (d) {
                d.type = $("#type_filter").val();
                d.providerId = $("#provider_filter").val();
                d.attentionStatus = $("#attention-status_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Proyecto",
                data: "project"
            },
            {
                title: "N° de Órden",
                data: "correlativeCodeStr"
            },
            {
                title: "Atención",
                data: "status",
                render: function (data) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">POR ATENDER</span>
								</label>
							</span>`;
                    } else if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">PARCIAL</span>
								</label>
							</span>`;
                    } else if (data == 3) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">TOTAL</span>
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
                title: "Proveedor",
                data: "provider.businessName"
            },
            {
                title: "Fecha",
                data: "date",
                orderable: true,
                "sType": "date-uk",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Monto S/",
                data: "parcial"
            },
            {
                title: "Moneda US$",
                data: "dolarParcial"
            },
            {
                title: "Atención Monto S/",
                data: "parcialInAttention"
            },
            {
                title: "Atención Monto US$",
                data: "dolarParcialInAttention"
            },
            {
                title: "% de Atención",
                data: "percentageAttention"
            },
            {
                title: "Guías",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-detail" title="Guías Atendidas">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Items",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-item" title="Items Atendidas">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Motivo Cierre",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.closureReason != null && row.status == 3) {
                        tmp += `<button data-id="${row.id}" data-name="${row.closureReason}" class="btn btn-primary btn-sm btn-icon btn-read-closure" title="Motivo Cierre">`;
                        tmp += `<i class="la la-comment"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Cierre Orden",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.status != 3) {
                        tmp += `<button data-id="${row.id}" data-name="${row.closureReason}" class="btn btn-success btn-sm btn-icon btn-closure" title="Cerrar Order">`;
                        tmp += `<i class="la la-check-circle"></i></button> `;
                    }
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [5, 6, 7, 8] }
        ]
    }

    var detailOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/atencion-ordenes/detalles/listar"),
            dataSrc: "",
            data: function (d) {
                d.id = Id;
                delete d.columns;
            }
        },
        columns: [
            {
                title: "N° Guía de Remisión",
                data: "remissionGuideName"
            },
            {
                title: "Fecha de Entrega",
                data: "deliveryDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Atención Monto (S/)",
                data: "parcial"
            },
            {
                title: "Atención Monto (US$)",
                data: "dolarParcial"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2, 3] }
        ]
    }

    var itemOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/atencion-ordenes/items/listar"),
            dataSrc: "",
            data: function (d) {
                d.id = Id;
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código IVC",
                data: "supply.fullCode"
            },
            {
                title: "Descripción",
                data: "supply.description"
            },
            {
                title: "Unidad",
                data: "supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Orden",
                data: "measure"
            },
            {
                title: "Guias Acumuladas",
                data: "measureInAttention"
            },
            {
                title: "Metrado Restante",
                data: "measureToAttent"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5] }
        ]
    }

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            detailDatatable = $("#detail_datatable").DataTable(detailOptions);
            itemDatatable = $("#item_datatable").DataTable(itemOptions);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        detailReload: function () {
            detailDatatable.ajax.reload();
        },
        itemReload: function () {
            itemDatatable.ajax.reload();
        },
        initEvents: function () {

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
                ".btn-item",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.itemReload();
                    $("#item_modal").modal("show");
                });

            mainDatatable.on("click",
                ".btn-closure",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    let name = $btn.data("name");
                    $("#reason").val(name);
                    $("#closure_modal").modal("show");
                });

            mainDatatable.on("click",
                ".btn-read-closure",
                function () {
                    let $btn = $(this);
                    let name = $btn.data("name");
                    $("#reason_read").html(name);
                    $("#closure_read_modal").modal("show");
                });

            detailDatatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.pdf(id);
                });
        }
    };

    var forms = {
        load: {
            pdf: function (id) {
                _app.loader.show();

                $("#detail_modal").modal("hide");
                $.ajax({
                    url: _app.parseUrl(`/almacenes/ingreso-material/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.remissionGuideName);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.remissionGuideUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            closure: function (formElement) {
                let data = new FormData($(formElement).get(0));
                data.append('id', Id);
                data.append('reason', $("#reason").val());
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/logistica/atencion-ordenes/cerrar-orden/${Id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#closure_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#closure_alert_text").html(error.responseText);
                            $("#closure_alert").removeClass("d-none").addClass("show");
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
            item: function () {
                itemForm.resetForm();
                $("#item_form").trigger("reset");
                $("#item_alert").removeClass("show").addClass("d-none");
            },
            pdf: function () {
                $("#detail_modal").modal("show");
                pdfForm.resetForm();
                $("#pdf_form").trigger("reset");
                $("#pdf_alert").removeClass("show").addClass("d-none");
            },
            closure: function () {
                closureForm.resetForm();
                $("#closure_form").trigger("reset");
                $("#closure_alert").removeClass("show").addClass("d-none");
            },
            closureRead: function () {
                $("#reason_read").html('');
            }
        }
    };

    var select2 = {
        init: function () {
            this.providers.init();
            this.types.init();
            this.attentionStatus.init();
        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores-ordenes")
                }).done(function (result) {
                    $(".select2-provider-filter").select2({
                        data: result
                    });
                });
            }
        },
        types: {
            init: function () {
                $(".select2-type-filter").select2();
            }
        },
        attentionStatus: {
            init: function () {
                $(".select2-attention-status-filter").select2();
            }

        }
    };

    var validate = {
        init: function () {
            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            pdfForm = $("#pdf_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            itemForm = $("#item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            closureForm = $("#closure_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.closure(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.detail();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.pdf();
                });

            $("item_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.item();
                });

            $("closure_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.closure();
                });

            $("closure_read_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.closureRead();
                });
        }
    };

    var events = {
        init: function () {

            $("#type_filter").on("change", function () {
                datatable.reload();
            });

            $("#provider_filter").on("change", function () {
                datatable.reload();
            });

            $("#attention-status_filter").on("change", function () {
                datatable.reload();
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
        }
    };
}();

$(function () {
    OrderAttention.init();
});