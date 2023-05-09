var StockRoof = function () {
    var voucherId = null;

    var vouchersDt = null;
    var detailsDt = null;

    var addForm = null;
    var editForm = null;
    var addDetailForm = null;
    var editDetailForm = null;

    var vouchersDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/vales-ingreso/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Fecha",
                data: "voucherDate"
            },
            {
                title: "Orden de Compra",
                data: "referencePurchaseOrder"
            },
            {
                title: "Proveedor",
                data: "supplier"
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
            url: _app.parseUrl("/almacenes/vales-ingreso/detalle/listar"),
            data: function (d) {
                d.voucherId = voucherId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-pallet'></i> Agregar Material",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    let formElements = $("#add_detail_form");
                    formElements.find("[name='StockVoucherId']").val(voucherId);
                    $("#detail_modal").modal("hide");
                    $("#add_detail_modal").modal("show");
                }
            }
        ],
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
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    return tmp;
                }
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
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                vouchersDt.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El vale de ingreso y su detalle serán eliminados permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/almacenes/vales/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.vouchers.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El vale de ingreso y su detalle han sido eliminadas con éxito.",
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
                                                text: "Ocurrió un error al intentar eliminar el vale de ingreso."
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
                this.events();
            },
            reload: function () {
                detailsDt.clear();
                detailsDt.ajax.reload();
            },
            events: function () {
                detailsDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.editDetail(id);
                    });

                detailsDt.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El detalle del material será eliminado permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/almacenes/vale-ingresos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.details.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El material ha sido eliminado con éxito.",
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
                                                text: "Ocurrió un error al intentar eliminar el material."
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

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-ingreso/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='VoucherDate']").val(result.voucherDate);
                        formElements.find("[name='Supplier']").val(result.supplier);
                        formElements.find("[name='ReferencePurchaseOrder']").val(result.referencePurchaseOrder);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            details: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-ingreso/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='VoucherDate']").val(result.voucherDate);
                        formElements.find("[name='VoucherDate']").attr("disabled", true);
                        formElements.find("[name='Supplier']").val(result.supplier);
                        formElements.find("[name='Supplier']").attr("disabled", true);
                        formElements.find("[name='ReferencePurchaseOrder']").val(result.referencePurchaseOrder);
                        formElements.find("[name='ReferencePurchaseOrder']").attr("disabled", true);
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editDetail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-ingreso/detalle/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='StockVoucherId']").val(result.stockVoucherId);
                        formElements.find("[name='StockId']").val(result.stockId)
                        formElements.find("[name='select_stock']").val(result.stockId).trigger("change");
                        formElements.find("[name='SalePriceUnit']").val(result.salePriceUnit);
                        formElements.find("[name='Quantity']").val(result.quantity);
                        formElements.find("[name='CurrencyType']").val(result.currencyType);
                        $("#detail_modal").modal("hide");
                        $("#edit_detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/almacenes/vales-ingreso/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatables.vouchers.reload();
                        voucherId = result.id;
                        datatables.details.reload();
                        forms.load.details(voucherId);
                        _app.show.notification.add.success();
                    }).fail(function (error) {
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
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                let id = $(formElement).find("[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-ingreso/editar/${id}`),
                    type: "put",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatables.vouchers.reload();
                        _app.show.notification.edit.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            addDetail: function (formElement) {
                $(formElement).find("[name='StockId']").val($(formElement).find("[name='select_stock']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/almacenes/vales-ingreso/detalle/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_detail_modal").modal("hide");
                        datatables.details.reload();
                        $("#detail_modal").modal("show");
                        _app.show.notification.add.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#add_detail_alert_text").html(error.responseText);
                            $("#add_detail_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editDetail: function (formElement) {
                $(formElement).find("[name='StockId']").val($(formElement).find("[name='select_stock']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                let id = $(formElement).find("[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/vales-ingreso/detalle/editar/${id}`),
                    type: "put",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_detail_modal").modal("hide");
                        datatables.details.reload();
                        $("#detail_modal").modal("show");
                        _app.show.notification.add.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#edit_detail_alert_text").html(error.responseText);
                            $("#edit_detail_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            addDetail: function () {
                addDetailForm.resetForm();
                $("#add_detail_form").trigger("reset");
                $("#add_detail_alert").removeClass("show").addClass("d-none");
            },
            editDetail: function () {
                editDetailForm.resetForm();
                $("#edit_detail_form").trigger("reset");
                $("#edit_detail_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.stocks.init();
        },
        stocks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/materiales")
                }).done(function (result) {
                    $(".select2-stocks").select2({
                        data: result
                    });
                })
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addDetailForm = $("#add_detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addDetail(formElement, e);
                }
            });

            editDetailForm = $("#edit_detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editDetail(formElement, e);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#add_detail_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addDetail();
                });

            $("#edit_detail_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editDetail();
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
            select2.init();
            validate.init();
            modals.init();
            datepickers.init();
        }
    };
}();

$(function () {
    StockRoof.init();
});