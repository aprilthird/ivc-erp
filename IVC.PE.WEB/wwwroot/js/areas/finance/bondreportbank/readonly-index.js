var Formula = function () {

    var mainDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/reportebancofianza/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },

        rowGroup: {
            dataSrc: "bank.name",
            title: "Centro de costo",

        },


        columns: [
            {
                title: "Centro de Costos",
                data: "project.name"
            },
            {
                title: "Concepto",
                data: "bondType.name"
            },
            {
                title: "Numero de Fianza",
                data: "bondNumber"
            },
            {
                title: "Inicio",
                data: "dateInitial"
            },
            {
                title: "Vencimiento",
                data: "dateEnd"
            },
            {
                title: "Vence (días)",
                data: "daysToEnd"
            },
            {
                title: "Monto",
                data: "penAmmount2"
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