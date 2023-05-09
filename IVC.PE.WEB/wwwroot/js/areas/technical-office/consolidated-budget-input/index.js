var ConsolidatedBudget = function () {

    var consolidatedBudgetDatatable = null;

    var summaryDtOpt = {
        responsive: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 6, 7, 11, 12],
                hide: [3, 4, 5, 8, 9, 10]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            },
            {
                extend: 'copy',
                className: "btn-dark",
                text: "<i class='fa fa-copy'></i> Copiar"
            },
            {
                extend: 'excel',
                className: "btn-success",
                text: "<i class='fa fa-file-excel'></i> Excel"
            },
            {
                extend: 'csv',
                className: "btn-success",
                text: "<i class='fa fa-file-csv'></i> CSV"
            },
            {
                extend: 'pdf',
                className: "btn-danger",
                text: "<i class='fa fa-file-pdf'></i> PDF"
            },
            {
                extend: 'print',
                className: "btn-dark",
                text: "<i class='fa fa-print'></i> Imprimir"
            }
        ],
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/consolidado-insumos/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        ordering: false,
        columns: [
            {
                data: function (result) {
                    var contractualAmount = result.contractualAmount;
                    var res = result.numberItem;
                    var numberItem = res;
                    if (numberItem == null)
                        return "";
                    else if (numberItem != null && contractualAmount == "0.00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var contractualAmount = result.contractualAmount;
                    var description = result.description;
                    var res = description;
                    var numberItem = result.numberItem;
                    if (numberItem == null)
                        return `<b>${res}</b>`;
                    else if (numberItem != null && contractualAmount == "0.00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var res = result.contractualAmount;
                    var contractualAmount = result.contractualAmount;
                    var numberItem = result.numberItem;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.deductiveAmount1;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.deductiveAmount2;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.deductiveAmount3;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.deductives;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.netContractual;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.adicionalAmount1;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.adicionalAmount2;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.adicionalAmount3;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.adicionals;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var numberItem = result.numberItem;
                    var res = result.accumulatedAmount;
                    var contractualAmount = result.contractualAmount;
                    if (contractualAmount == "0.00")
                        return "";
                    else if (numberItem == null)
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12] }
        ]
    };

    var datatables = {
        init: function () {
            this.consolidatedBudgetDt.init();
        },
        consolidatedBudgetDt: {
            init: function () {
                consolidatedBudgetDatatable = $("#consolidateds_datatable").DataTable(summaryDtOpt);
            },
            reload: function () {
                consolidatedBudgetDatatable.ajax.reload();
            }
        }
    };

    var events = {
        refresh: function () {
            $("#btn-refresh").on("click", function () {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/consolidado-insumos/generar"),
                    method: "post",
                    contentType: false,
                    processData: false
                })
                    .done(function () {
                        datatables.consolidatedBudgetDt.reload();
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            });
        },
        filters: function () {
            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.init();
            });
            $("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatables.consolidatedBudgetDt.reload();
            });
        }
    };

    var select2 = {
        init: function () {
            this.supplyFamilies.init();
            this.supplyGroups.init();
        },
        supplyFamilies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families-filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $(".select2-supply-groups-filter").empty();
                $(".select2-supply-groups-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyfamilyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-supply-groups-filter").select2({
                        data: result
                    });
                });
            }
        }
    };

    return {
        init: function () {
            datatables.init();
            events.refresh();
            events.filters();
            select2.init();
        }
    };
}();

$(function () {
    ConsolidatedBudget.init();
});