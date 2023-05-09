var Stock = function () {

    var stocksDt = null;
    var detailDt = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var Id = null;

    var totalCost = 0;
    var totalIncome = 0;
    var totalDispatch = 0;
    var totalCurrent = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('es-PE');

    var stocksDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/existencias/listar"),
            data: function (d) {
                d.providerId = $("#provider_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                totalCost = 0;
                totalIncome = 0;
                totalDispatch = 0;
                totalCurrent = 0;

                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        totalCost += item.parcial;
                        totalIncome += item.totalEntries;
                        totalDispatch += item.totalDispatch;
                        totalCurrent += item.measureFloat;
                    });
                }

                return [
                    //$("#total_parcial").val(formatter.format(totalCost)),
                    $("#incomes").val(formatter.format(totalIncome)),
                    $("#dispatch").val(formatter.format(totalDispatch)),
                    $("#stock_ttl").val(formatter.format(totalCost))
                ]
            }
        },
        columns: [
            {
                title: "Familia",
                data: "supply.supplyFamily.name"
            },
            {
                title: "Grupo",
                data: "supply.supplyGroup.name"
            },
            {
                title: "Proveedor",
                data: "providers"
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
                title: "Und",
                data: "supply.measurementUnit.abbreviation"
            },
            {
                title: "Stock Mínimo",
                data: "minimumMeasure"
            },
            {
                title: "Ingresos",
                data: "income"
            },
            {
                title: "Despachos",
                data: "dispatch"
            },
            {
                title: "Stock Físico",
                data: "measure"
            },
            {
                title: "Precio Unitario (S/)",
                data: "unitPrice"
            },
            {
                title: "Parcial (S/)",
                data: "parcial",
                render: $.fn.dataTable.render.number(',', '.', 2)
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.supplyId}" class="btn btn-info btn-sm btn-icon btn-detail">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
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
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [6, 7, 8, 9, 10, 11] }
        ]
    };

    var detailDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/existencias/ordenes/listar"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Órden",
                data: "order.correlativeCodeStr"
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
                title: "Metrado Atendido",
                data: "measureInAttentionString"
            },
            {
                title: "Precio Unitario Soles",
                data: "unitPrice"
            },
            {
                title: "Parcial Soles",
                data: "parcial"
            },
            {
                title: "Proveedor",
                data: "order.provider.businessName"
            },
            {
                title: "Moneda",
                data: function (result) {
                    if (result.currency == 1)
                        return "Soles";
                    else
                        return "Dólares";
                }
            },
            {
                title: "Tipo de Cambio",
                data: "order.exchangeRate"
            },
            {
                title: "Precio Unitario Dólares",
                data: "dolarUnitPrice"
            },
            {
                title: "Parcial Dólares",
                data: "dolarParcial"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 8, 9, 10] }
        ]
    };


    var datatables = {
        init: function () {
            this.stocks.init();
            this.details.init();
        },
        stocks: {
            init: function () {
                stocksDt = $("#stocks_datatable").DataTable(stocksDtOptions);
                this.events();
            },
            reload: function () {
                stocksDt.ajax.reload();
            },
            events: function () {
                stocksDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                stocksDt.on("click",
                    ".btn-detail",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.details.reload();
                        $("#detail_modal").modal("show");
                    });
            }
        },
        details: {
            init: function () {
                detailDt = $("#detail_datatable").DataTable(detailDtOptions);
            },
            reload: function () {
                detailDt.ajax.reload();
            }
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/existencias/${id}`),
                    dataSrc: ""
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id']").val(result.id);
                    formElements.find("[name='Supply.Description']").val(result.supply.description);
                    formElements.find("[name='Supply.MeasurementUnit.Abbreviation']").val(result.supply.measurementUnit.abbreviation);
                    formElements.find("[name='MinimumMeasure']").val(result.minimumMeasure);
                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            edit: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                let id = $(formElement).find("[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/existencias/editar/${id}`),
                    type: "put",
                    data: data,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatables.stocks.reload();
                        _app.show.notification.edit.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            edit: function () {
                editForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.providers.init();
            this.supplyGroups.init();
            this.supplyFamilies.init();
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
            init: function (){
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        }
    };

    var validate = {
        init: function () {
            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });
        }
    };

    var events = {
        init: function () {

            $("#provider_filter").on("change", function () {
                datatables.stocks.reload();
            });

            $("#supply_group_filter").on("change", function () {
                datatables.stocks.reload();
                select2.providers.init();
            });

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.init();
                select2.providers.init();
                datatables.stocks.reload();
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            validate.init();
            modals.init();
            events.init();
        },
        actualizar: function () {
            $.ajax({
                url: _app.parseUrl(`/almacenes/existencias/actualizar`),
                type: "put"
            }).done(function (result) {
                console.log("listo parker");
            });
        }
    };
}();


$(function () {
    Stock.init();
});