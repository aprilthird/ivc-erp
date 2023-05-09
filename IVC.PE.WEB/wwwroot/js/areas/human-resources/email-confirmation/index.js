var Worker = function () {
    var workerDatatable = null;

    var workerOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/correos/listar"),
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
                title: "Estado",
                data: "isActive",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Activo</span>' : '<span class="kt-badge kt-badge--danger kt-badge--inline">Cesado</span>';
                }
            },
            {
                title: "Email",
                data: "email"
            },
            {
                title: "Estado Email",
                data: "emailConfirmed",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Confirmado</span>' : '<span class="kt-badge kt-badge--danger kt-badge--inline">Sin Confirmar</span>';
                }
            },
            {
                title: "Último Envío",
                data: "emailAlertSentDateTimeStr"
            },
            {
                title: "Confirmado El",
                data: "emailConfirmationDateTimeStr"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-primary btn-sm btn-icon btn-send">`;
                    tmp += `<i class="fas fa-paper-plane"></i></button> `;
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
                workerDatatable = $("#worker_datatable").DataTable(workerOpts);
                this.events();
            },
            reload: function() {
                workerDatatable.ajax.reload();
            },
            events: function () {
                workerDatatable.on("click",
                    ".btn-send",
                    function () {
                        let $btn = $(this);
                        $btn.addLoader();
                        let id = $btn.data("id");
                        $.ajax({
                            url: _app.parseUrl(`/recursos-humanos/correos/enviar-alerta?workerId=${id}`),
                            type: "post",
                            success: function (result) {
                                datatables.workers.reload();
                                swal.fire({
                                    type: "success",
                                    title: "Completado",
                                    text: "Alerta enviada con éxito",
                                    confirmButtonText: "Excelente"
                                });
                            },
                            error: function (errormessage) {
                                swal.fire({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn-danger",
                                    animation: false,
                                    customClass: 'animated tada',
                                    confirmButtonText: "Entendido",
                                    text: "Ocurrió un error al enviar la alerta al obrero"
                                });
                            },
                            complete: function () {
                                $btn.removeLoader();
                            }
                        });
                    });
            }
        }
    };

    var select2 = {
        init: function () {
            this.origins.init();
            this.workGroups.init();
            this.documentTypes.init();
            this.categories.init();
            this.workerPositions.init();
            this.fundPension.init();
            this.sewerGroups.init();
        },
        origins: {
            init: function () {
                $(".select2-origins").select2();
            }
        },
        workGroups: {
            init: function () {
                $(".select2-workgroups").select2();
            }
        },
        documentTypes: {
            init: function () {
                $(".select2-document-types").select2();
            }
        },
        categories: {
            init: function () {
                $(".select2-categories").select2();
            }
        },
        workerPositions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cargos-laborales/2")
                }).done(function (result) {
                    $(".select2-work-positions").select2({
                        data: result
                    });
                });
            }
        },
        fundPension: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/administradoras-pensiones")
                }).done(function (result) {
                    $(".select2-pension-fund-administrators").select2({
                        data: result
                    });
                });
            }
        },
        sewerGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas")
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("#category_filter, #origin_filter, #workgroup_filter, #status_filter").on("change", function () {
                datatables.workers.reload();
            });

            $("#btnSendAll").on("click", function () {
                let $btn = $(this);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/correos/envio-masivo`),
                    type: "post",
                    success: function (result) {
                        datatables.workers.reload();
                        swal.fire({
                            type: "success",
                            title: "Completado",
                            text: "Alertas masivas enviadas con éxito",
                            confirmButtonText: "Excelente"
                        });
                    },
                    error: function (errormessage) {
                        swal.fire({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn-danger",
                            animation: false,
                            customClass: 'animated tada',
                            confirmButtonText: "Entendido",
                            text: "Ocurrió un error al enviar las alertas masivas"
                        });
                    },
                    complete: function () {
                        $btn.removeLoader();
                    }
                });
            });
        }
    };

        return {
            init: function () {
                datatables.init();
                select2.init();
                events.init();
            }
        };
}();

$(function () {
    Worker.init();
});