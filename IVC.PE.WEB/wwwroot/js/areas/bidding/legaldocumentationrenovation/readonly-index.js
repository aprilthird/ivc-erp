var Formula = function () {

    var mainDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/renovaciones/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
        },
        reload: function () {
            mainDatatable.ajax.reload();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    }
}();

$(function () {
    Formula.init();
});