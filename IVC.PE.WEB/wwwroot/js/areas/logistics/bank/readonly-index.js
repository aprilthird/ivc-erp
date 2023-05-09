var Bank = function () {

    var bankDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/bancos/listar"),
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
            bankDatatable = $("#bank_datatable").DataTable(options);
        },
        reload: function () {
            bankDatatable.ajax.reload();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    }
}();

$(function () {
    Bank.init();
});