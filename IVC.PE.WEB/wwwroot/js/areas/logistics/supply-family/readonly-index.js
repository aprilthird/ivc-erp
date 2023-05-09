var SupplyFamily = function () {

    var supplyFamilyDatatable = null;
    
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/familias/listar"),
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
            supplyFamilyDatatable = $("#supply_family_datatable").DataTable(options);
        },
        reload: function () {
            supplyFamilyDatatable.ajax.reload();
        }
    };
    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(function () {
    SupplyFamily.init();
});