var MeasurementUnit = function () {

    var measurementUnitDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/unidades/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Abreviación",
                data: "abbreviation"
            },
            {
                title: "Nombre",
                data: "name"
            }
        ]
    };

    var datatable = {
        init: function () {
            measurementUnitDatatable = $("#measurement_unit_datatable").DataTable(options);
        },
        reload: function () {
            measurementUnitDatatable.ajax.reload();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(function () {
    MeasurementUnit.init();
});