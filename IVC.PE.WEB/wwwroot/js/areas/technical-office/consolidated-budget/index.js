var ConsolidatedBudget = function () {

    var consolidatedBudgetDatatable = null;

    var summaryDtOpt = {
        responsive: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 7, 8, 13, 14],
                hide: [3, 4, 5, 6, 9, 10, 11, 12]
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
            url: _app.parseUrl("/oficina-tecnica/consolidado-presupuestos/listar"),
            dataSrc: ""
        },
        ordering: false,
        columns: [
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.numberItem;
                    var numberItem = res;
                    if (description.includes("COMPONENTE") || numberItem == "F16")
                        return `<b>${res}</b>`;
                    else if (numberItem == null || numberItem == "00")
                        return "";
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = description;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE"))
                        return `<b>${res}</b>`;
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.contractualAmount;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.deductiveAmount1;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.deductiveAmount2;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.deductiveAmount3;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.deductiveAmount4;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.deductives;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.netContractual;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                }
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.adicionalAmount1;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.adicionalAmount2;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.adicionalAmount3;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.adicionalAmount4;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.adicionals;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
                        return `<b>${res}</b>`;
                    else
                        return res;
                },
                visible: false
            },
            {
                data: function (result) {
                    var description = result.description;
                    var res = result.accumulatedAmount;
                    var numberItem = result.numberItem;
                    if (description.includes("COMPONENTE") || description.includes("Porcentaje"))
                        return "";
                    else if (numberItem == null || numberItem == "00")
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
                    url: _app.parseUrl("/oficina-tecnica/consolidado-presupuestos/generar"),
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
        }
    };

    return {
        init: function () {
            datatables.init();
            events.refresh();
        }
    };
}();

$(function () {
    ConsolidatedBudget.init();
});