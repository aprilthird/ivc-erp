var BudgetTitle = function () {

    var mainDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/titulos-de-presupuesto/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name",
            },
            {
                title: "Abreviación",
                data: "abbreviation",
            },
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
    BudgetTitle.init();
});