var WorkerInvoice = function () {

    var selectWeekOption = new Option('--Seleccione una Semana--', ' ', true, true);

    var workerDatatable = null;

    var wkId = null;

    var workerOptions = {
        responsive: false,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/obreros/boletas/listar`),
            dataSrc: "",
            data: function (d) {
                d.weekId = wkId;
                delete d.columns;
            }
        },
        buttons: [
            {
                text: "<i class='fas fa-paper-plane'></i> Envio masivo",
                className: " btn-dark",
                action: function (e, dt, node, config) {
                    var btn = e.currentTarget;
                    $(btn).addLoader();
                    let wId = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/boletas/envio-masivo-prueba-t/${wId}`),
                        type: "post",
                        success: function (result) {
                            swal.fire({
                                type: "success",
                                title: "Completado",
                                text: result,
                                confirmButtonText: "Excelente"
                            });
                        },
                        error: function (errormessage) {
                            swal.fire({
                                type: "info",
                                title: "Completado",
                                confirmButtonClass: "btn-info",
                                animation: false,
                                customClass: 'animated tada',
                                confirmButtonText: "Entendido",
                                text: errormessage
                            });
                        },
                        complete: function () {
                            $(btn).removeLoader();
                        }
                    })
                }
            },

            {
                text: "<i class='fas fa-paper-plane'></i> Envio masivo Restante",
                className: " btn-dark",
                action: function (e, dt, node, config) {
                    var btn = e.currentTarget;
                    $(btn).addLoader();
                    let wId = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/boletas/envio-masivo-prueba-restante/${wId}`),
                        type: "post",
                        success: function (result) {
                            swal.fire({
                                type: "success",
                                title: "Completado",
                                text: result,
                                confirmButtonText: "Excelente"
                            });
                        },
                        error: function (errormessage) {
                            swal.fire({
                                type: "info",
                                title: "Completado",
                                confirmButtonClass: "btn-info",
                                animation: false,
                                customClass: 'animated tada',
                                confirmButtonText: "Entendido",
                                text: errormessage
                            });
                        },
                        complete: function () {
                            $(btn).removeLoader();
                        }
                    })
                }
            },

            {
                text: "<i class='fa fa-file-excel'></i> Informe de Envios",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    let wId = $("#week_filter").val();
                    window.location = _app.parseUrl(`/recursos-humanos/obreros/boletas/informe-sunafil?weekId=${wId}`);
                }
            }
        ],
        columns: [
            {
                title: "Nro. Documento",
                data: "document",
            },
            {
                title: "Trabajador",
                data: "fullName",
            },
            {
                title: "Estado",
                data: "isActive",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Activo</span>' : '<span class="kt-badge kt-badge--danger kt-badge--inline">Cesado</span>';
                }
            },
            {
                title: "Correo Electrónico",
                data: "email"
            },
            {
                title: "Fecha de Envío",
                data: "dateSendedStr",
                render: function (data, type, row) {
                    return data || row.observation;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-success btn-sm btn-icon btn-invoice">`;
                    tmp += `<i class="fa fa-file-invoice-dollar"></i></button> `;
                    tmp += `<button data-id="${row.workerId}" class="btn btn-primary btn-sm btn-icon btn-download">`;
                    tmp += `<i class="fa fa-download"></i></button> `;
                    tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-send">`;
                    tmp += `<i class="fas fa-paper-plane"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.workersDt.init();
        },
        workersDt: {
            init: function () {
                workerDatatable = $("#worker_invoices_datatable").DataTable(workerOptions);
                this.events();
            },
            reload: function () {
                workerDatatable.clear().draw();
                workerDatatable.ajax.reload();
            },
            events: function () {
                workerDatatable.on("click",
                    ".btn-invoice",
                    function () {
                        if (wkId != null) {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            window.open(_app.parseUrl(`/recursos-humanos/obreros/boletas/generar-boleta/${wkId}/${id}`), '_blank');
                        }
                    });

                workerDatatable.on("click",
                    ".btn-download",
                    function () {
                        if (wkId != null) {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            var url = _app.parseUrl(`/recursos-humanos/obreros/boletas/generar-boleta/${wkId}/${id}`);
                            var fn = "boleta";
                            window.location = _app.parseUrl(`/recursos-humanos/obreros/boletas/decargar-boleta?url=${url}&filename=${fn}`);
                        }
                    });

                workerDatatable.on("click",
                    ".btn-send",
                    function () {
                        let $btn = $(this);
                        $btn.addLoader();
                        let id = $btn.data("id");
                        $.ajax({
                            url: _app.parseUrl(`/recursos-humanos/obreros/boletas/enviar-boleta?weekId=${wkId}&workerId=${id}`),
                            success: function (result) {
                                datatables.workersDt.reload();
                                swal.fire({
                                    type: "success",
                                    title: "Completado",
                                    text: "Boleta enviada con éxito",
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
                                    text: "Ocurrió un error al enviar la boleta al obrero"
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
            this.projects.init();
            this.weeks.init();
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                })
            }
        },
        weeks: {
            init: function () {
                let year = $("#year_filter").val();
                $(".select2-weeks").append(selectWeekOption).trigger("change");
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                let year = $("#year_filter").val();
                let pId = $("#project_filter").val();
                $(".select2-weeks").empty();
                $(".select2-weeks").append(selectWeekOption).trigger("change");
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${pId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("#project_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#year_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#btnLoadWeek").on("click", function () {
                datatables.workersDt.reload();
            });


            $("#week_filter").on("change", function (e) {
                wkId = $("#week_filter").val();
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
    WorkerInvoice.init();
});