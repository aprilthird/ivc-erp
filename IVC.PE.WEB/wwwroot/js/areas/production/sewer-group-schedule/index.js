var Schedule = function () {

    var selectSMOption = new Option('--Seleccione un Tramo--', '', true, true);
    var selectSGOption = new Option('--Seleccione una Cuadrilla--', null, true, true);

    var scheduleDatatable = null;
    var activityDatatable = null;

    var f4Datatable = null;
    var f56Datatable = null;
    var f6Datatable = null;

    var scheduleAddForm = null;
    var activityForm = null;
    var activityAddForm = null;
    var activityEditForm = null;

    var scheduleId = null;
    var sewerManifoldId = null;
    var weekId = null;
    var sewerGroupId = null;

    var isNew = false;
    var isActivity = false;
    var isBusy = false;
    var isIssued = false;
    var isActEdit = false;

    var scheduleOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/programaciones/listar"),
            data: function (d) {
                d.weekId = $("#week_filter").val();
                d.workFrontHeadId = $("#workfronthead_filter").val();
                d.sewerGroupId = $("#sewergroup_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Semana",
                data: "projectCalendarWeek.yearWeekNumber"
            },
            {
                title: "Jefe de Frente",
                data: "workFrontHead.user.fullName"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Tramos",
                data: "description"
            },
            {
                title: "Estado",
                data: "isIssued",
                render: function (data, type, row) {
                    return (data == true) ? '<span class="kt-badge kt-badge--success kt-badge--inline">Emitido</span>' : '<span class="kt-badge kt-badge--danger kt-badge--inline">Pendiente</span>';
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-activity">`;
                    tmp += `<i class="fa fa-ellipsis-v"></i></button>`;
                    if (row.isIssued) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-issued">`;
                        tmp += `<i class="fa fa-unlock-alt"></i></button>`;
                    }
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-book'></i> Reporte Excel",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/produccion/programacion-diaria/reporte-excel`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/produccion/programacion-diaria/actividades-excel?tempData=${result}`;
                    });
                }
            }
        ]
    };
    var activityOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/programacion-diaria/listar"),
            data: function (d) {
                d.scheduleId = scheduleId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "projectFormulaActivity.description"
            },
            {
                title: "Tramo",
                data: "sewerManifold.name"
            },
            {
                title: "Lunes",
                data: "footageMonday"
            },
            {
                title: "Martes",
                data: "footageTuesday"
            },
            {
                title: "Miércoles",
                data: "footageWednesday"
            },
            {
                title: "Jueves",
                data: "footageThrusday"
            },
            {
                title: "Viernes",
                data: "footageFriday"
            },
            {
                title: "Sábado",
                data: "footageSaturday"
            },
            {
                title: "Total",
                data: "footageTotal",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `${data} ${row.projectFormulaActivity.measurementUnit.abbreviation}`;
                    return tmp;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-smid="${row.sewerManifoldId}" class="btn btn-info btn-sm btn-icon btn-addActSm">`;
                    tmp += `<i class="fa fa-plus"></i></button>`;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo Tramo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#activity_modal").modal("hide");
                    //$("#activity_add_modal").modal("show");
                    forms.load.activityAdd(scheduleId);
                }
            },
            {
                text: "<i class='fa fa-plus'></i> Emitir",
                className: "btn-danger",
                action: function (e, dt, node, config) {
                    forms.submit.scheduleIssued(scheduleId, true);
                }
            }
        ]
    };

    var f4Opts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/programacion-diaria/f4-listar"),
            data: function (d) {
                d.scheduleId = scheduleId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "projectFormulaActivity.description"
            },
            {
                title: "Tramo",
                data: "sewerManifold.name"
            },
            {
                title: "Lunes",
                data: "footageMonday"
            },
            {
                title: "Martes",
                data: "footageTuesday"
            },
            {
                title: "Miércoles",
                data: "footageWednesday"
            },
            {
                title: "Jueves",
                data: "footageThrusday"
            },
            {
                title: "Viernes",
                data: "footageFriday"
            },
            {
                title: "Sábado",
                data: "footageSaturday"
            },
            {
                title: "Total",
                data: "footageTotal",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `${data} ${row.projectFormulaActivity.measurementUnit.abbreviation}`;
                    return tmp;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-smid="${row.sewerManifoldId}" class="btn btn-info btn-sm btn-icon btn-addActSm">`;
                    tmp += `<i class="fa fa-plus"></i></button>`;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo Tramo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    //$("#activity_modal").modal("hide");
                    //forms.load.f4ActivityAdd(scheduleId);
                }
            },
            {
                text: "<i class='fa fa-plus'></i> Emitir",
                className: "btn-danger",
                action: function (e, dt, node, config) {
                    //forms.submit.scheduleIssued(scheduleId, true);
                }
            }
        ]
    };
    var f56Opts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/programacion-diaria/f56-listar"),
            data: function (d) {
                d.scheduleId = scheduleId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "projectFormulaActivity.description"
            },
            {
                title: "Tramo",
                data: "sewerManifold.name"
            },
            {
                title: "Lunes",
                data: "footageMonday"
            },
            {
                title: "Martes",
                data: "footageTuesday"
            },
            {
                title: "Miércoles",
                data: "footageWednesday"
            },
            {
                title: "Jueves",
                data: "footageThrusday"
            },
            {
                title: "Viernes",
                data: "footageFriday"
            },
            {
                title: "Sábado",
                data: "footageSaturday"
            },
            {
                title: "Total",
                data: "footageTotal",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `${data} ${row.projectFormulaActivity.measurementUnit.abbreviation}`;
                    return tmp;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-smid="${row.sewerManifoldId}" class="btn btn-info btn-sm btn-icon btn-addActSm">`;
                    tmp += `<i class="fa fa-plus"></i></button>`;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo Tramo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    //$("#activity_modal").modal("hide");
                    //forms.load.f4ActivityAdd(scheduleId);
                }
            },
            {
                text: "<i class='fa fa-plus'></i> Nuevo Circuito",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    //$("#activity_modal").modal("hide");
                    //forms.load.f4ActivityAdd(scheduleId);
                }
            },
            {
                text: "<i class='fa fa-plus'></i> Emitir",
                className: "btn-danger",
                action: function (e, dt, node, config) {
                    //forms.submit.scheduleIssued(scheduleId, true);
                }
            }
        ]
    };
    var f6Opts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/programacion-diaria/f6-listar"),
            data: function (d) {
                d.scheduleId = scheduleId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Actividad",
                data: "projectFormulaActivity.description"
            },
            {
                title: "Tramo",
                data: "sewerManifold.name"
            },
            {
                title: "Lunes",
                data: "footageMonday"
            },
            {
                title: "Martes",
                data: "footageTuesday"
            },
            {
                title: "Miércoles",
                data: "footageWednesday"
            },
            {
                title: "Jueves",
                data: "footageThrusday"
            },
            {
                title: "Viernes",
                data: "footageFriday"
            },
            {
                title: "Sábado",
                data: "footageSaturday"
            },
            {
                title: "Total",
                data: "footageTotal",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `${data} ${row.projectFormulaActivity.measurementUnit.abbreviation}`;
                    return tmp;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-smid="${row.sewerManifoldId}" class="btn btn-info btn-sm btn-icon btn-addActSm">`;
                    tmp += `<i class="fa fa-plus"></i></button>`;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo Tramo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    //$("#activity_modal").modal("hide");
                    //forms.load.f4ActivityAdd(scheduleId);
                }
            },
            {
                text: "<i class='fa fa-plus'></i> Emitir",
                className: "btn-danger",
                action: function (e, dt, node, config) {
                    //forms.submit.scheduleIssued(scheduleId, true);
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.scheduleDt.init();
            this.activityDt.init();
            this.f4Dt.init();
            this.f56Dt.init();
            this.f6Dt.init();
        },
        scheduleDt: {
            init: function () {
                scheduleDatatable = $("#schedules_datatable").DataTable(scheduleOpts);
                this.events();
            },
            reload: function () {
                scheduleDatatable.ajax.reload();
                isBusy = false;
            },
            events: function () {
                scheduleDatatable.on("click",
                    ".btn-activity",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        scheduleId = id;
                        //forms.load.activity(id);
                        forms.load.formulaCheck(id);
                    });

                scheduleDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La programación será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/produccion/programaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.scheduleDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La programación ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la programación."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                scheduleDatatable.on("click",
                    ".btn-issued",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La programación será marcada como Pendiente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, marcar como Pendiente",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/produccion/programaciones/emitir/${id}`),
                                        method: "put",
                                        data: {
                                            issued: false
                                        },
                                        success: function (result) {
                                            datatables.scheduleDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La programación esta ahora como Pendiente.",
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
                                                text: "Ocurrió un error al intentar marcar como Pendiente la programación."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        activityDt: {
            init: function () {
                activityDatatable = $("#activities_datatable").DataTable(activityOpts);
                this.events();
            },
            reload: function (id) {
                scheduleId = id;
                activityDatatable.clear().draw();
                activityDatatable.ajax.reload();
                if (isIssued) {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                } else {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                }
            },
            events: function () {
                activityDatatable.on("click",
                    ".btn-addActSm",
                    function () {
                        let $btn = $(this);
                        let smid = $btn.data("smid");
                        $("#activity_modal").modal("hide");
                        forms.load.activitySmAdd(scheduleId, smid);
                    });

                activityDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#activity_modal").modal("hide");
                        forms.load.activityEdit(id);
                    });

                scheduleDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La actividad será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/produccion/programacion-diaria/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.scheduleDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La actividad ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la actividad."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        f4Dt: {
            init: function () {
                f4Datatable = $("#f4s_datatable").DataTable(f4Opts);
            },
            reload: function (id) {
                scheduleId = id;
                activityDatatable.clear().draw();
                activityDatatable.ajax.reload();
                if (isIssued) {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                } else {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                }
            }
        },
        f56Dt: {
            init: function () {
                f56Datatable = $("#f56s_datatable").DataTable(f56Opts);
            },
            reload: function (id) {
                scheduleId = id;
                activityDatatable.clear().draw();
                activityDatatable.ajax.reload();
                if (isIssued) {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                } else {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                }
            }
        },
        f6Dt: {
            init: function () {
                f6Datatable = $("#f6s_datatable").DataTable(f6Opts);
            },
            reload: function (id) {
                scheduleId = id;
                activityDatatable.clear().draw();
                activityDatatable.ajax.reload();
                if (isIssued) {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                } else {
                    activityDatatable.columns(9).visible(!isIssued);
                    activityDatatable.columns(10).visible(!isIssued);
                    activityDatatable.buttons(0).enable(!isIssued);
                    activityDatatable.buttons(1).enable(!isIssued);
                }
            }
        }
    };

    var forms = {
        load: {
            formulaCheck: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/formula-check/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result.fs);
                        switch (result.fs) {
                            case 'F1':
                                Swal.fire("Formula 1 - en desarrollo"); break;
                            case 'F2':
                                Swal.fire("Formula 2 - en desarrollo"); break;
                            case 'F3':
                                Swal.fire("Formula 3 - en desarrollo"); break;
                            case 'F4':
                                $("#f4_modal").modal("show");
                                break;
                            case 'F3/4':
                                Swal.fire("Formula 3/4 - en desarrollo"); break;
                            case 'F5':
                                Swal.fire("Formula 5 - en desarrollo"); break;
                            case 'F6':
                                $("#f6_modal").modal("show");
                                break;
                            case 'F5/6':
                                $("#f56_modal").modal("show");
                                break;
                            case 'F7':
                                forms.load.f7(result.schedule);
                                break;
                            default:
                                Swal.fire("Formula en desarrollo.");
                        }
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            f7: function (result) {
                isActivity = true;
                let formElements = $("#activity_form");
                formElements.find("[name='Id']").val(result.id);
                formElements.find("[name='ProjectCalendarWeekId']").val(result.projectCalendarWeekId);
                weekId = result.projectCalendarWeekId;
                formElements.find("[name='select_week']").val(result.projectCalendarWeekId).trigger("change");
                formElements.find("[name='select_week']").attr("disabled", "disabled");
                formElements.find("[name='WorkFrontHeadId']").val(result.workFrontHeadId);
                formElements.find("[name='select_work_front_head']").val(result.workFrontHeadId).trigger("change");
                formElements.find("[name='select_work_front_head']").attr("disabled", "disabled");
                formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                sewerGroupId = result.sewerGroupId;
                select2.sewergroups.edit(result.workFrontHeadId, result.sewerGroupId);
                formElements.find("[name='select_sewergroups']").attr("disabled", "disabled");
                isIssued = result.isIssued;
                datatables.activityDt.reload(result.id);
                $("#activity_modal").modal("show");
            },
            activity: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        isActivity = true;
                        let formElements = $("#activity_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectCalendarWeekId']").val(result.projectCalendarWeekId);
                        weekId = result.projectCalendarWeekId;
                        formElements.find("[name='select_week']").val(result.projectCalendarWeekId).trigger("change");
                        formElements.find("[name='select_week']").attr("disabled", "disabled");
                        formElements.find("[name='WorkFrontHeadId']").val(result.workFrontHeadId);
                        formElements.find("[name='select_work_front_head']").val(result.workFrontHeadId).trigger("change");
                        formElements.find("[name='select_work_front_head']").attr("disabled", "disabled");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        sewerGroupId = result.sewerGroupId;
                        select2.sewergroups.edit(result.workFrontHeadId, result.sewerGroupId);
                        formElements.find("[name='select_sewergroups']").attr("disabled", "disabled");
                        isIssued = result.isIssued;
                        datatables.activityDt.reload(result.id);
                        $("#activity_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            activityAdd: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        isActivity = true;
                        isActEdit = false;
                        let formElements = $("#activity_add_form");
                        formElements.find("[name='SewerGroupDailyScheduleId']").val(result.id);
                        select2.activities.reload(result.sewerGroupId);
                        $("#activity_add_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            activitySmAdd: function (id, smId) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        isActivity = true;
                        isActEdit = false;
                        let formElements = $("#activity_add_form");
                        formElements.find("[name='SewerGroupScheduleId']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(smId);
                        formElements.find("[name='select_sewer_manifold']").val(smId).trigger("change");
                        $("#searchSewerManifoldBtn").trigger("click");
                        select2.activities.reload(result.sewerGroupId);
                        $("#activity_add_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            activityList: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programacion-diaria/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        var tmp = "";
                        tmp += `<tr>`;
                        tmp += `<td>${result.sewerManifold.name}</td>`;
                        tmp += `<td>${result.projectFormulaActivity.description}</td>`;
                        tmp += `<td>${result.footageMonday}</td>`;
                        tmp += `<td>${result.footageTuesday}</td>`;
                        tmp += `<td>${result.footageWednesday}</td>`;
                        tmp += `<td>${result.footageThrusday}</td>`;
                        tmp += `<td>${result.footageFriday}</td>`;
                        tmp += `<td>${result.footageSaturday}</td>`;
                        tmp += `<td>${result.footageTotal} ${result.projectFormulaActivity.measurementUnit.abbreviation}</td>`;
                        tmp += `</tr>`;
                        $("#tbl_activities").append(tmp);
                        let formElements = $("#activity_add_form");
                        formElements.find("[name='ProjectFormulaActivityId']").val("");
                        formElements.find("[name='select_activity']").val("").trigger("change");
                        formElements.find("[name='FootageMonday']").val("");
                        formElements.find("[name='FootageTuesday']").val("");
                        formElements.find("[name='FootageWednesday']").val("");
                        formElements.find("[name='FootageThrusday']").val("");
                        formElements.find("[name='FootageFriday']").val("");
                        formElements.find("[name='FootageSaturday']").val("");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            activityEdit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programacion-diaria/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        isActivity = true;
                        isActEdit = true;
                        let formElements = $("#activity_edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_sewer_manifold']").val(result.sewerManifoldId).trigger("change");
                        $("#searchSewerManifoldBtn").trigger("click");
                        select2.activities.edit(sewerGroupId, result.projectFormulaActivityId);
                        formElements.find("[name='FootageMonday']").val(result.footageMonday);
                        formElements.find("[name='FootageTuesday']").val(result.footageTuesday);
                        formElements.find("[name='FootageWednesday']").val(result.footageWednesday);
                        formElements.find("[name='FootageThrusday']").val(result.footageThrusday);
                        formElements.find("[name='FootageFriday']").val(result.footageFriday);
                        formElements.find("[name='FootageSaturday']").val(result.footageSaturday);
                        $("#activity_edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            }
        },
        submit: {
            scheduleAdd: function (formElement) {
                $(formElement).find("[name='ProjectCalendarWeekId']").val($(formElement).find("[name='select_week']").val());
                $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_work_front_head']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroups']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        isNew = true;
                        scheduleId = result;
                        datatables.scheduleDt.reload();
                        $("#schedule_add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#schedule_add_alert_text").html(error.responseText);
                            $("#schedule_add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },

            scheduleIssued: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programaciones/emitir/${id}`),
                    method: "put",
                    data: {
                        issued: true
                    }
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function (result) {
                        datatables.scheduleDt.reload();
                        $("#activity_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#activity_alert_text").html(error.responseText);
                            $("#activity_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            activityAdd: function (formElement) {
                $(formElement).find("[name='SewerGroupScheduleId']").val(scheduleId);
                $(formElement).find("[name='ProjectFormulaActivityId']").val($(formElement).find("[name='select_activity']").val());
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_sewer_manifold']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/produccion/programacion-diaria/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activityDt.reload(scheduleId);
                        //$("#activity_add_modal").modal("hide");
                        forms.load.activityList(result);
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#activity_add_alert_text").html(error.responseText);
                            $("#activity_add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            activityEdit: function (formElement) {
                $(formElement).find("[name='SewerGroupScheduleId']").val(scheduleId);
                $(formElement).find("[name='ProjectFormulaActivityId']").val($(formElement).find("[name='select_activity']").val());
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_sewer_manifold']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/produccion/programacion-diaria/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.activityDt.reload(scheduleId);
                        $("#activity_edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#activity_edit_alert_text").html(error.responseText);
                            $("#activity_edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            scheduleAdd: function () {
                scheduleAddForm.reset();
                if (isNew) {
                    isNew = false;
                    forms.load.activity(scheduleId);
                }
                $("#schedule_add_form").trigger("reset");
                $("#schedule_add_alert").removeClass("show").addClass("d-none");
            },
            activity: function () {
                isActivity = false;
                activityForm.reset();
                $("#activity_alert").removeClass("show").addClass("d-none");
            },
            activityAdd: function () {
                activityAddForm.reset();
                $("#tbl_activities").empty();
                $("#activity_modal").modal("show");
                $("#activity_add_form").trigger("reset");
                $("#activity_add_alert").removeClass("show").addClass("d-none");
            },
            activityEdit: function () {
                activityEditForm.reset();
                $("#activity_modal").modal("show");
                $("#activity_edit_form").trigger("reset");
                $("#activity_edit_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            scheduleAddForm = $("#schedule_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.scheduleAdd(formElement);
                }
            });

            activityForm = $("#activity_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            activityAddForm = $("#activity_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.activityAdd(formElement);
                }
            });

            activityEditForm = $("#activity_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.activityEdit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#schedule_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.scheduleAdd();
                });

            $("#activity_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.activity();
                });

            $("#activity_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.activityAdd();
                });

            $("#activity_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.activityAdd();
                });
        }
    };

    var select2 = {
        init: function () {
            this.weeks.init();
            this.workfrontheads.init();
            this.sewergroups.init();
            this.habilitations.init();
            this.activities.init();
            this.sewerManifolds.init();
        },
        weeks: {
            init: function () {
                //let projectId = $("#project_general_filter").val();
                //let year = new Date().getFullYear();
                //$.ajax({
                //    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                //}).done(function (result) {
                //    $(".select2-weeks").select2({
                //        data: result
                //    });
                //});
                $.ajax({
                    url: _app.parseUrl(`/select/semanas-futuras`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                });
            }
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-work-front-heads").select2({
                        data: result
                    });
                    $(".select2-work-front-heads2").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
                $(".select2-sewergroups2").select2();
            },
            reload: function (id) {
                let wfh = id;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            },
            reloadFilter: function (id) {
                let wfh = id;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups2").empty();
                    $(".select2-sewergroups2").append(selectSGOption).trigger('change');
                    $(".select2-sewergroups2").select2({
                        data: result
                    });
                });
            },
            edit: function (wfhId, sgId) {
                let wfh = wfhId;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption);
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                    $(".select2-sewergroups").val(sgId).trigger('change');
                });
            }
        },
        habilitations: {
            init: function () {
                $(".select2-habilitations").select2();
            },
            reload: function(id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/habilitaciones-cuadrilla?sewerGroupId=${sg}`)
                }).done(function (result) {
                    $(".select2-habilitations").empty();
                    $(".select2-habilitations").select2({
                        data: result
                    });
                });
            },
            edit: function (sgId, habId) {
                let sg = sgId;
                $.ajax({
                    url: _app.parseUrl(`/select/habilitaciones-cuadrilla?sewerGroupId=${sg}`)
                }).done(function (result) {
                    $(".select2-habilitations").empty();
                    $(".select2-habilitations").select2({
                        data: result
                    });
                    $(".select2-habilitations").val(habId).trigger('change');
                });
            }
        },
        activities: {
            init: function () {
                $(".select2-activities").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/actividades-cuadrilla?sewerGroupId=${sg}`)
                }).done(function (result) {
                    $(".select2-activities").empty();
                    $(".select2-activities").select2({
                        data: result
                    });
                });
            },
            edit: function (sgId, actId) {
                let sg = sgId;
                $.ajax({
                    url: _app.parseUrl(`/select/actividades-cuadrilla?sewerGroupId=${sg}`)
                }).done(function (result) {
                    $(".select2-activities").empty();
                    $(".select2-activities").select2({
                        data: result
                    });
                    $(".select2-activities").val(actId).trigger('change');
                });
            }
        },
        sewerManifolds: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga`)
                }).done(function (result) {
                    $(".select2-sewer-manifolds").append(selectSMOption);
                    $(".select2-sewer-manifolds").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $(".select2-work-front-heads").on("change", function () {
                select2.sewergroups.reload(this.value);
            });

            $(".select2-work-front-heads2").on("change", function () {
                select2.sewergroups.reloadFilter(this.value);
            });

            $(".select2-sewergroups").on("change", function () {
                if (!isActivity) {
                    select2.habilitations.reload(this.value);
                    select2.activities.reload(this.value);
                }
            });

            $(".select2-sewer-manifolds").on("change", function () {
                sewerManifoldId = this.value;
            });

            $("#searchSewerManifoldBtn").on("click", function () {
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/programaciones/${sewerManifoldId}`)
                }).done(function (result) {
                    let formElements = null;
                    if (!isActEdit) {
                        formElements = $("#activity_add_form");
                    } else {
                        formElements = $("#activity_edit_form");
                    }
                    formElements.find("[name='sm_digging']").html(result.lengthOfDiggingStr);
                    formElements.find("[name='sm_installing']").html(result.lengthOfPipeInstalledStr);
                    formElements.find("[name='sm_terrain_type']").html(result.terrainTypeStr);
                    formElements.find("[name='sm_layers']").html(result.numberOfLayersStr);
                    formElements.find("[name='sm_height']").html(result.ditchHeightStr);
                    formElements.find("[name='sm_filling']").html(result.lengthOfFillingStr);
                    formElements.find("[name='sm_height_i']").html(result.sewerBoxStartHeightStr);
                    formElements.find("[name='sm_terrain_type_i']").html(result.sewerBoxStartTerrainTypeStr);
                    formElements.find("[name='sm_height_j']").html(result.sewerBoxEndHeightStr);
                    formElements.find("[name='sm_terrain_type_j']").html(result.sewerBoxEndTerrainTypeStr);
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/cuadrillas-colector-descarga-cyr/programaciones?sgId=${sewerGroupId}&wId=${weekId}&ditch=${result.ditchHeight}`)
                    }).done(function (result) {
                        formElements.find("[name='min_inst_long']").html(result.toFixed(2));
                    });
                });
            });

            $("#week_filter").on("change", function () {
                if (!isBusy) {
                    isBusy = true;
                    datatables.scheduleDt.reload();
                }
            });

            $("#workfronthead_filter").on("change", function () {
                if (!isBusy) {
                    isBusy = true;
                    datatables.scheduleDt.reload();
                }
            });

            $("#sewergroup_filter").on("change", function () {
                if (!isBusy) {
                    isBusy = true;
                    datatables.scheduleDt.reload();
                }
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modals.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    Schedule.init();
});