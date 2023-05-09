var SupplyGroup = function () {

    var supplyGroupDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/grupos/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Nombre",
                data: "name"
            }
        ]
    };

    var datatable = {
        init: function () {
            supplyGroupDatatable = $("#supply_group_datatable").DataTable(options);
        },
        reload: function () {
            supplyGroupDatatable.ajax.reload();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(function () {
    SupplyGroup.init();
});