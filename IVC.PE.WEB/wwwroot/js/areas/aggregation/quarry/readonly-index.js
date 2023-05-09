var Quarry = function () {

    var quarryDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/canteras/listar"),
            dataSrc: ""
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
            quarryDatatable = $("#quarry_datatable").DataTable(options);
        },
        reload: function () {
            quarryDatatable.ajax.reload();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(function () {
    Quarry.init();
});