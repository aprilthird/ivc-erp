var StockRoof = function () {
    var voucherId = null;

    var vouchersDt = null;
    var detailsDt = null;

    var vouchersDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/vales-salida/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Fecha",
                data: "voucherDate"
            },
            {
                title: "Estado",
                data: "wasDelivered",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Entregado</span>' : '<span class="kt-badge kt-badge--info kt-badge--inline">Pendiente</span>';
                }
            },
            {
                title: "Fase",
                data: "projectPhase.fullDescription"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Recoge",
                data: "pickUpResponsible"
            },
            {
                title: "Observaciones",
                data: "observation"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-deliver">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    return tmp;
                }
            }
        ]
    };
    var detailsDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/vales-salida/detalle/listar"),
            data: function (d) {
                d.voucherId = voucherId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Código",
                data: "stock.code"
            },
            {
                title: "Descripción",
                data: "stock.description"
            },
            {
                title: "Unidades",
                data: "stock.unit"
            },
            {
                title: "Cantidad",
                data: "quantity"
            },
            {
                title: "P.U. (Orden)",
                data: "salePriceUnitStr"
            }
        ]
    };

    var datatables = {
        init: function () {
            this.vouchers.init();
            this.details.init();
        },
        vouchers: {
            init: function () {
                vouchersDt = $("#stockvouchers_datatable").DataTable(vouchersDtOptions);
                this.events();
            },
            reload: function () {
                vouchersDt.ajax.reload();
            },
            events: function () {
                vouchersDt.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        voucherId = $btn.data("id");
                        datatables.details.reload();
                        forms.load.details(voucherId);
                    });

                vouchersDt.on("click",
                    ".btn-deliver",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Pedido entregado?",
                            text: "El pedido se marcará como entregado.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, fue entregado.",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/almacenes/almacenes/vales-salida/entregado/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            datatables.vouchers.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El pedido ha sido marcado como Entregado.",
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
                                                text: "Ocurrió un error al intentar marcar como entregado el pedido."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        details: {
            init: function () {
                detailsDt = $("#details_datatable").DataTable(detailsDtOptions);
            },
            reload: function () {
                detailsDt.clear();
                detailsDt.ajax.reload();
            }
        }
    };

    var forms = {
        load: {
            details: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-salida/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='VoucherDate']").val(result.voucherDate);
                        formElements.find("[name='VoucherDate']").attr("disabled", true);
                        formElements.find("[name='WasDelivered']").val(result.wasDelivered.toString());
                        formElements.find("[name='select_wasdelivered']").val(result.wasDelivered.toString()).trigger("change");
                        formElements.find("[name='select_wasdelivered']").attr("disabled", true);
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        formElements.find("[name='select_sewergroup']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='select_sewergroup']").attr("disabled", true);
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId);
                        formElements.find("[name='select_projectphase']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='select_projectphase']").attr("disabled", true);
                        formElements.find("[name='PickUpResponsible']").val(result.pickUpResponsible);
                        formElements.find("[name='PickUpResponsible']").attr("disabled", true);
                        formElements.find("[name='Observation']").val(result.observation);
                        formElements.find("[name='Observation']").attr("disabled", true);
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
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
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            datepickers.init();
        }
    };
}();

$(function () {
    StockRoof.init();
});