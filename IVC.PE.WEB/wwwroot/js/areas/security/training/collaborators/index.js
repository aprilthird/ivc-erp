var Collaborators = function () {

    var collabsDatatable = null;

    var collabsDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/capacitacion/colaboradores/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Nro. Document",
                data: "document"
            },
            {
                title: "Ap. Paterno",
                data: "paternalSurname"
            },
            {
                title: "Ap. Materno",
                data: "maternalSurname"
            },
            {
                title: "Nombres",
                data: "name"
            },
            {
                title: "Capacitaciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.collabsDt.init();
        },
        collabsDt: {
            init: function () {
                collabsDatatable = $("#collabs_datatable").DataTable(collabsDtOpts);
            },
            reload: function () {
                collabsDatatable.ajax.reload();
            }
        }
    };




    return {
        init: function () {
            datatables.init();
        }
    }
}();

$(function () {
    Collaborators.init();
});