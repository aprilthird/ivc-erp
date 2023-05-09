var WeeklyTask = function () {

    var workerId = null;

    var editForm = null;
    var payrollImportForm = null;
    var weeklyTaskImportForm = null;
    var weeklyTaskFilesForm = null;

    var isTaskEdited = false;

    var selectWeekOpt = new Option('--Seleccione una Semana--', ' ', true, true);
    var selectSGOpt = new Option('--Seleccione una Cuadrilla--', ' ', true, true);
    var selectWFHOpt = new Option('--Seleccione un Jefe de Frente--', ' ', true, true);

    var weeklyDt = null;
    var dailyDt = null;

    var WeekOpts = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/tareo-semanal/listar`),
            dataSrc: "",
            data: function (d) {
                d.weekId = $("#week_filter").val();
                d.sewerGroupId = $("#search_sewer_group").val();
                d.workFrontHeadId = $("#search_work_front_head").val();
                delete d.columns;
            }
        },
        buttons: [
            {
                text: "<i class='fa fa-check-double'></i> Importar Planilla",
                className: " btn-secondary",
                action: function (e, dt, node, config) {
                    $("#payroll_import_modal").modal("show");
                }
            },
            {
                text: "<i class='fa fa-check-double'></i> Solicitar Autorización",
                className: " btn-dark",
                action: function (e, dt, node, config) {
                    var btn = e.currentTarget;
                    $(btn).addLoader();
                    let wId = $("#week_filter").val();
                    let pId = $("#project_general_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/tareo-semanal/solicitar-autorizacion?weekId=${wId}&projectId=${pId}`),
                        type: "post",
                        success: function (result) {
                            swal.fire({
                                type: "success",
                                title: "Completado",
                                text: result,
                                confirmButtonText: "Excelente"
                            });
                            $("#week_filter").trigger("change");
                        },
                        error: function (errormessage) {
                            console.log(errormessage);
                            swal.fire({
                                type: "info",
                                title: "Error",
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
                text: "<i class='fa fa-calculator'></i> Calcular Planilla",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    _app.loader.show();
                    let id = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/tareo-semanal/calcular?weekId=${id}`),
                        type: "post",
                        success: function (result) {
                            _app.loader.hide();
                            swal.fire({
                                type: "success",
                                title: "Completado",
                                text: result,
                                confirmButtonText: "Excelente"
                            });
                        },
                        error: function (error) {
                            _app.loader.hide();
                            swal.fire({
                                type: "error",
                                title: "Error",
                                confirmButtonClass: "btn-danger",
                                animation: false,
                                customClass: 'animated tada',
                                confirmButtonText: "Entendido",
                                text: error.responseText
                            });
                        }
                    })
                }
            },
            {
                text: "<i class='fa fa-file-excel'></i> Excel Tareo Semanal",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    let wId = $("#week_filter").val();
                    let sgId = $("#search_sewer_group").val();
                    let wfhId = $("#search_work_front_head").val();
                    window.location = _app.parseUrl(`/recursos-humanos/tareo-semanal/exportar-excel?weekId=${wId}&sewerGroupId=${sgId}&workFrontHeadId=${wfhId}`);
                }
            },
            {
                text: "<i class='fa fa-file-excel'></i> Importar Tareo Semanal",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#weekly_task_import_modal").modal("show");
                }
            }
        ],
        columns: [
            {
                data: "workerFullName"
            },
            {
                data: "workerTypeDocNumber"
            },
            {
                data: "sewerGroupCode"
            },
            {
                data: "workerDailyTasks.1.hours",
            },
            {
                data: "workerDailyTasks.1.hours60",
            },
            {
                data: "workerDailyTasks.1.hours100",
            },
            {
                data: "workerDailyTasks.1.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.2.hours",
            },
            {
                data: "workerDailyTasks.2.hours60",
            },
            {
                data: "workerDailyTasks.2.hours100",
            },
            {
                data: "workerDailyTasks.2.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.3.hours",
            },
            {
                data: "workerDailyTasks.3.hours60",
            },
            {
                data: "workerDailyTasks.3.hours100",
            },
            {
                data: "workerDailyTasks.3.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.4.hours",
            },
            {
                data: "workerDailyTasks.4.hours60",
            },
            {
                data: "workerDailyTasks.4..hours100",
            },
            {
                data: "workerDailyTasks.4.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.5.hours",
            },
            {
                data: "workerDailyTasks.5.hours60",
            },
            {
                data: "workerDailyTasks.5.hours100",
            },
            {
                data: "workerDailyTasks.5.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.6.hours",
            },
            {
                data: "workerDailyTasks.6..hours60",
            },
            {
                data: "workerDailyTasks.6.hours100",
            },
            {
                data: "workerDailyTasks.6.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: "workerDailyTasks.0.hours100",
            },
            {
                data: "workerDailyTasks.0.phase",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ],
        rowGroup: {
            dataSrc: "sewerGroupCode"
        }
    };
    var DayOpts = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/tareo-semanal/obrero/listar`),
            dataSrc: "",
            data: function (d) {
                d.weekId = $("#week_filter").val();
                d.workerId = workerId;
                delete d.columns;
            }
        },
        buttons: [],
        columns: [
            {
                title: "Fecha",
                data: "date",
            },
            {
                title: "Fase",
                data: "projectPhase.fullDescription",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "N",
                data: "hoursNormal"
            },
            {
                title: "60%",
                data: "hours60Percent"
            },
            {
                title: "100%",
                data: "hours100Percent"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
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
        ],
        rowGroup: {
            dataSrc: "sewerGroup.code"
        }
    };

    var datatables = {
        init: function () {
            this.weekly.init();
            this.daily.init();
        },
        weekly: {
            init: function () {
                weeklyDt = $("#weekly_task_datatable").DataTable(WeekOpts);
                this.events();
            },
            reload: function () {
                weeklyDt.ajax.reload();
            },
            events: function () {
                weeklyDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        workerId = $btn.data("id");
                        datatables.daily.reload();
                        $("#edit_dt_modal").modal('show');
                    });
            }
        },
        daily: {
            init: function () {
                dailyDt = $("#daily_task_datatable").DataTable(DayOpts);
                this.events();
            },
            reload: function () {
                dailyDt.clear().draw();
                dailyDt.ajax.reload();
            },
            events: function () {
                dailyDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#edit_dt_modal").modal('hide');
                        forms.load.edit(id);
                    });

                dailyDt.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tareo del obrero será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/recursos-humanos/tareo-diario/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.daily.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tareo del obrero ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tareo del obrero"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            } 
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-diario/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='Date']").val(result.date);
                        formElements.find("[name='WorkerId']").val(result.workerId);
                        formElements.find("[name='select_worker']").val(result.workerId).trigger("change");
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId)
                        formElements.find("[name='select_phase']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId)
                        formElements.find("[name='select-sewer-group']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='HoursNormal']").val(result.hoursNormal);
                        formElements.find("[name='HoursMedicalRest']").val(result.hoursMedicalRest);
                        formElements.find("[name='Hours60Percent']").val(result.hours60Percent);
                        formElements.find("[name='Hours100Percent']").val(result.hours100Percent);
                        formElements.find("[name='HoursHoliday']").val(result.hoursHoliday);
                        formElements.find("[name='HoursPaternityLeave']").val(result.hoursPaternityLeave);
                        formElements.find("[name='HoursPaidLeave']").val(result.hoursPaidLeave);
                        formElements.find("[name='MedicalLeave']").val(result.medicalLeave.toString()).trigger("change");
                        formElements.find("[name='UnPaidLeave']").val(result.unPaidLeave.toString()).trigger("change");
                        formElements.find("[name='LaborSuspension']").val(result.laborSuspension.toString()).trigger("change");
                        formElements.find("[name='NonAttendance']").val(result.nonAttendance.toString()).trigger("change");
                        formElements.find("[name='IsCeased']").val(result.isCeased.toString()).trigger("change");
                        formElements.find("[name='CeasedDate']").val(result.ceasedDate);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                /*$(formElement).find("[name='ProjectId']").val($("#project_filter").val());*/
                $(formElement).find("[name='Date']").val($("#day_filter").val());
                $(formElement).find("[name='WorkerId']").val($(formElement).find("[name='select_worker']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phase']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select-sewer-group']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='ProjectCalendarWeek.Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-diario/editar/${id}?weekly=true`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.daily.reload();
                        datatables.weekly.reload();
                        $("#edit_modal").modal('hide');
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            payroll: {
                import: function (formElement) {
                    let wId = $("#week_filter").val();
                    $(formElement).find("[name='WeekId']").val(wId);
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: `/recursos-humanos/tareo-semanal/importar-planilla`,
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();
                            if (!emptyFile) {
                                xhr.upload.onprogress = function (evt) {
                                    if (evt.lengthComputable) {
                                        var percentComplete = evt.loaded / evt.total;
                                        percentComplete = parseInt(percentComplete * 100);
                                        $(".progress-bar").width(percentComplete + "%");
                                    }
                                };
                            }
                            return xhr;
                        }
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        $("#payroll_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#payroll_import_alert_text").html(error.responseText);
                            $("#payroll_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            },
            weekly: {
                import: function (formElement) {
                    let wId = $("#week_filter").val();
                    $(formElement).find("[name='WeekId']").val(wId);
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: `/recursos-humanos/tareo-semanal/importar-tareo-excel`,
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();
                            if (!emptyFile) {
                                xhr.upload.onprogress = function (evt) {
                                    if (evt.lengthComputable) {
                                        var percentComplete = evt.loaded / evt.total;
                                        percentComplete = parseInt(percentComplete * 100);
                                        $(".progress-bar").width(percentComplete + "%");
                                    }
                                };
                            }
                            return xhr;
                        }
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        $("#weekly_task_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#weekly_task_import_alert_text").html(error.responseText);
                            $("#weekly_task_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },
                files: function (formElement) {
                    let wId = $("#week_filter").val();
                    $(formElement).find("[name='WeekId']").val(wId);
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: `/recursos-humanos/tareo-semanal/cargar-incidencias`,
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();
                            if (!emptyFile) {
                                xhr.upload.onprogress = function (evt) {
                                    if (evt.lengthComputable) {
                                        var percentComplete = evt.loaded / evt.total;
                                        percentComplete = parseInt(percentComplete * 100);
                                        $(".progress-bar").width(percentComplete + "%");
                                    }
                                };
                            }
                            return xhr;
                        }
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        $("#weekly_task_files_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#weekly_task_files_alert_text").html(error.responseText);
                            $("#weekly_task_files_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_dt_modal").modal('show');
            },
            payroll: {
                import: function () {
                    payrollImportForm.resetForm();
                    $("#payroll_import_form").trigger("reset");
                    $("#payroll_import_alert").removeClass("show").addClass("d-none");
                }
            },
            weekly: {
                import: function () {
                    weeklyTaskImportForm.resetForm();
                    $("#weekly_task_import_modal").trigger("reset");
                    $("#weekly_task_import_modal").removeClass("show").addClass("d-none");
                },
                files: function () {
                    weeklyTaskFilesForm.resetForm();
                    $("#weekly_task_files_modal").trigger("reset");
                    $("#weekly_task_files_modal").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var modals = {
        init: function () {
            //$("#edit_dt_modal").on("hidden.bs.modal",
            //    function () {
            //        if (isTaskEdited) {
            //            datatables.weekly.reload();
            //            isTaskEdited = false;
            //        }
            //    });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#payroll_import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.payroll.import();
                });

            $("#weekly_task_import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.weekly.import();
                });

            $("#weekly_task_files_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.weekly.files();
                });
        }
    };

    var validate = {
        init: function () {
            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            payrollImportForm = $("#payroll_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.payroll.import(formElement);
                }
            });

            weeklyTaskImportForm = $("#weekly_task_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.weekly.import(formElement);
                }
            });

            weeklyTaskFilesForm = $("#weekly_task_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.weekly.files(formElement);
                }
            });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            });
        }
    };

    var select2 = {
        init: function() {
            this.weeks.init();
            this.workfrontheads.init();
            this.sewergroups.init();
            this.workers.init();
            this.phases.init();
        },
        weeks: {
            init: function () {
                let projectId = $("#project_general_filter").val();
                let year = $("#year_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").empty();
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-weeks").empty();
                //let opt = new Option('--Seleccione una Semana--', ' ', true, true);
                $(".select2-weeks").append(selectWeekOpt).trigger('change');
                let projectId = $("#project_general_filter").val();
                let year = $("#year_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            }
        },
        workfrontheads: {
            init: function () {
                $(".select2-work-front-heads").append(selectWFHOpt).trigger('change');
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-work-front-heads").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                let pId = $("#project_general_filter").val();
                let wId = $("#search_work_front_head").val();
                let weekId = $("#week_filter").val();
                $(".select2-sewer-groups").append(selectSGOpt).trigger('change');
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-semana?projectId=${pId}&workFrontHeadId=${wId}&weekId=${weekId}`)
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-sewer-groups").empty();
                $(".select2-sewer-groups").append(selectSGOpt).trigger('change');
                let pId = $("#project_general_filter").val();
                let wId = $("#search_work_front_head").val();
                let weekId = $("#week_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-semana?projectId=${pId}&workFrontHeadId=${wId}&weekId=${weekId}`)
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                })
            }
        },        
        workers: {
            init: function () {
                let pId = $("#project_general_filter").val();
                let weekId = $("#week_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/obreros-semana?projectId=${pId}&weekId=${weekId}`)
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result
                    });
                })
            },
        },
        phases: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/fases-proyecto?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-phases").select2({
                        data: result,
                        allowClear: true
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("#search_work_front_head").on("change", function () {
                select2.sewergroups.reload();
            });

            $("#search_sewer_group").on("change", function () {
                datatables.weekly.reload();
            });

            $("#project_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#year_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#week_filter").on("change", function () {
                let weekId = $("#week_filter").val();
                let projectId = $("#project_general_filter").val();
                select2.sewergroups.reload();
                datatables.weekly.reload();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-semanal/autorizacion?pid=${projectId}&wid=${weekId}`)
                }).done(function (result) {
                    if (result != null) {
                        $("#responsible_1").text(result.responsible1FullName);
                        $("#responsible_2").text(result.responsible2FullName);
                        if (result.alertsSent) {
                            if (!result.userAnswered1) {
                                $("#responsible_1_auth").removeClass('kt-badge--danger').removeClass('kt-badge--success').removeClass('kt-badge--dark').addClass('kt-badge--warning').text("Pendiente de Respuesta");
                            }
                            else if (result.weeklyTaskAuth1) {
                                $("#responsible_1_auth").removeClass('kt-badge--danger').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--success').text("Autorizado");
                            } else {
                                $("#responsible_1_auth").removeClass('kt-badge--success').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--danger').text("Rechazado");
                            }

                            if (!result.userAnswered2) {
                                $("#responsible_2_auth").removeClass('kt-badge--danger').removeClass('kt-badge--success').removeClass('kt-badge--dark').addClass('kt-badge--warning').text("Pendiente de Respuesta");
                            }
                            else if (result.weeklyTaskAuth2) {
                                $("#responsible_2_auth").removeClass('kt-badge--danger').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--success').text("Autorizado");
                            } else {
                                $("#responsible_2_auth").removeClass('kt-badge--success').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--danger').text("Rechazado");
                            }
                        }
                        else {
                            $("#responsible_1_auth").removeClass('kt-badge--success').removeClass('kt-badge--danger').removeClass('kt-badge--warning').addClass('kt-badge--dark').text("No Alertado");
                            $("#responsible_2_auth").removeClass('kt-badge--success').removeClass('kt-badge--danger').removeClass('kt-badge--warning').addClass('kt-badge--dark').text("No Alertado");
                        }
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
            modals.init();
            validate.init();
            datepicker.init();
        }
    };
}();

$(function () {
    WeeklyTask.init();
});