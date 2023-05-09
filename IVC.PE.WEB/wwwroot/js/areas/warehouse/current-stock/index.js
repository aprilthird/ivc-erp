var Stock = function () {

    var stocksDt = null;
    var Id = null;

    var totalCost = 0;
    var totalEntries = 0;
    var totalRequest = 0;
    var totalMeasure = 0;
    var unitPrice = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('es-PE');

    var stocksDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/existencias-contables/listar"),
            data: function (d) {
                d.providerId = $("#provider_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                totalCost = 0;
                totalEntries = 0;
                totalRequest = 0;
                totalMeasure = 0;
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        totalCost += item.parcial;
                        totalEntries += item.totalEntry;
                        totalRequest += item.totalRequest;
                        totalMeasure += parseFloat(item.measureFloat);
                    });
                }
                return [
                    //$("#total_parcial").val(formatter.format(totalCost)),
                    $("#total_incomes").val(formatter.format(totalEntries)),
                    $("#total_outputs").val(formatter.format(totalRequest)),
                    $("#total_curr_stock").val(formatter.format(totalCost))
                ];
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
                title: "Ingresos",
                data: "entries"
            },
            {
                title: "Salida Contable",
                data: "requests"
            },
            {
                title: "Stock Contable",
                data: "measure"
            },
            {
                title: "P. U. (S/)",
                data: "unitPrice"
            },
            {
                title: "Monto (S/)",
                data: "parcialString",
                width: "10%"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [6, 7, 8, 9, 10] }
        ]
    };

    var datatables = {
        init: function () {
            this.stocks.init();
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
        }
    };

    var forms = {
        load: {

        },
        submit: {
           
        },
        reset: {

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
            init: function () {
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

        }
    };

    var modals = {
        init: function () {

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
        }
    };
}();


$(function () {
    Stock.init();
});