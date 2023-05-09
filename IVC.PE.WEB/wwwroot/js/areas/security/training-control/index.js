var TrainingControl = function () {

    var workerDatatable = null;
    var sessionDatatable = null;

    var workerOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/capacitaciones/control/listar"),
            data: function (d) {
                d.category = $("#category_filter").val();
                d.origin = $("#origin_filter").val();
                d.workgroup = $("#workgroup_filter").val();
                d.status = $("#status_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Trabajador",
                data: "fullName"
            },
            {
                title: "Tipo/Num Doc.",
                data: "docTypeNumber",
            },
            {
                title: "Fecha Ingreso",
                data: "entryDateStr"
            },
            {
                title: "Fecha Cese",
                data: "ceaseDateStr"
            },
            {
                title: "Categoría",
                data: "categoryDesc"
            },
            {
                title: "Origen",
                data: "originDesc"
            },
            {
                title: "Procedencia",
                data: "workgroupDesc"
            },
            {
                title: "Estado",
                data: "isActive",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Activo</span>' : '<span class="kt-badge kt-badge--danger kt-badge--inline">Cesado</span>';
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-details" data-toggle="tooltip" title="Detalle">`;
                    tmp += `<i class="fa fa-list"></i></button> `;
                    //if (row.isActive == true) {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-warning btn-sm btn-icon btn-cease" data-toggle="tooltip" title="Cesar">`;
                    //    tmp += `<i class="fa fa-calendar-times"></i></button>`;
                    //} else {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-success btn-sm btn-icon btn-new-entry" data-toggle="tooltip" title="Reingreso">`;
                    //    tmp += `<i class="fa fa-calendar-plus"></i></button>`;
                    //}
                    //tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    //tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var sessionOpts = {
        responsive: true,
        ajax: {
            url: "/",
            data: function (d) {
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fecha",
                data: "sessionDate"
            },
            {
                title: "Frente",
                data: "workFront",
            },
            {
                title: "Tutor",
                data: "user"
            },
            {
                title: "Categoría",
                data: "trainingCategory"
            },
            {
                title: "Tema",
                data: "trainingTopic"
            },
            {
                title: "Resultado",
                data: null,
                render: function (data, type, row) {
                    return `<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--${row.trainingResultStatus.colorClass}">${row.trainingResultStatus.colorStr}</span>`;
                }
            },
            {
                title: "Observaciones",
                data: "observation"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    //tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    //tmp += `<i class="fa fa-edit"></i></button> `;
                    //if (row.isActive == true) {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-warning btn-sm btn-icon btn-cease" data-toggle="tooltip" title="Cesar">`;
                    //    tmp += `<i class="fa fa-calendar-times"></i></button>`;
                    //} else {
                    //    tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-success btn-sm btn-icon btn-new-entry" data-toggle="tooltip" title="Reingreso">`;
                    //    tmp += `<i class="fa fa-calendar-plus"></i></button>`;
                    //}
                    //tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    //tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.workers.init();
        },
        workers: {
            init: function () {
                workerDatatable = $("#workers_datatable").DataTable(workerOpts);
                this.events();
            },
            reload: function () {
                workerDatatable.ajax.reload();
            },
            events: function () {
                workerDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.details(id);
                    });
            }
        },
        sessions: {
            init: function (id) {
                sessionOpts.ajax.url = _app.parseUrl(`/seguridad/capacitaciones/control/${id}/sesiones/listar`);
                sessionDatatable = $("#sessions_datatable").DataTable(sessionOpts);
            },
            reload: function () {
                sessionDatatable.ajax.reload();
            },
            destroy: function () {
                sessionDatatable.destroy();
            }
        }
    };

    var form = {
        load: {
            details: function (id) {
                datatables.sessions.init(id);
                $("#details_modal").modal("show");
            }
        },
        reset: {
            details: function () {

            }
        }
    };

    var modal = {
        init: function () {
            $("#details_modal").on("hidden.bs.modal",
                function () {
                    form.reset.details();
                    datatable.details.destroy();
                });
        }
    };

    return {
        init: function () {
            datatables.init();
            modal.init();
        }
    }
}();

$(function () {
    TrainingControl.init();
});