var PayrollMovementWeek = function () {

    var addForm = null;
    var importDayForm = null;
    var importWorkersForm = null;
    var copyForm = null;
    var workerDailyTaskDataTable = null;

    var selectAllOption = new Option('--Todos--', null, true, true);

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/tareo-diario/listar"),
            dataSrc: "",
            data: function (d) {
                d.dateTask = $("#day_filter").val(); 
                d.sewerGroupId = $("#search_sewer_group").val();
                d.workFrontHeadId = $("#search_work_front_head").val();
                d.projectId = $("#project_general_filter").val();
                delete d.columns;
            }
        },
        buttons: [
            {
                text: "<i class='fa fa-file-excel'></i> Excel Formato Tareo",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    let date = $("#day_filter").val();
                    let sg = $("#search_sewer_group").val();
                    let wfh = $("#search_work_front_head").val();
                    let pr = $("#project_general_filter").val();
                    window.location = _app.parseUrl(`/recursos-humanos/tareo-diario/descargar-obreros-habiles?taskDate=${date}&projectId=${pr}&workFrontHeadId=${wfh}&sewerGroupId=${sg}`);
                }
            },
            {
                text: "<i class='fas fa-upload'></i> Cargar Tareo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#import_day_modal").modal("show");
                }
            }
        ],
        columns: [
            {
                title: "Nro.Doc.",
                data: "workerDocument"
            },
            {
                title: "Trabajador",
                data: "workerFullName",
                width: "20%"
            },
            {
                title: "Categoría",
                data: "workerCategoryStr",
                width: "5%"
            },
            {
                title: "Fase",
                data: "projectPhaseFullDescription",
                width: "20%",
                render: function (data, type, row) {
                    return data || "0 NO ASIGNADO";
                }
            },
            {
                title: "Cuadrilla",
                data: "sewerGroupCode"
            },
            {
                title: "N",
                data: "hoursNormal",
                render: function (data, type, row) {
                    if (row.medicalLeave)
                        return "SM";
                    if (row.unPaidLeave)
                        return "PS";
                    if (row.laborSuspension)
                        return "S";
                    if (row.nonAttendance)
                        return "F";
                    if (row.hoursPaidLeave > 0)
                        return "PG " + row.hoursPaidLeave.toFixed(2);
                    if (row.hoursHoliday > 0)
                        return "FE";
                    if (row.hoursMedicalRest)
                        return "DM";
                    return data;
                }
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
            dataSrc: "sewerGroupCode"
        }
    };

    var datatable = {
        init: function () {
            workerDailyTaskDataTable = $("#worker_daily_task_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            workerDailyTaskDataTable.clear().draw();
            workerDailyTaskDataTable.ajax.reload();
        },
        initEvents: function () {
            workerDailyTaskDataTable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            workerDailyTaskDataTable.on("click",
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
                                        datatable.reload();
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
    };

    var form = {
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
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='HoursNormal']").val(result.hoursNormal);
                        //formElements.find("[name='HoursMedicalRest']").val(result.hoursMedicalRest);
                        formElements.find("[name='Hours60Percent']").val(result.hours60Percent);
                        formElements.find("[name='Hours100Percent']").val(result.hours100Percent);
                        //formElements.find("[name='HoursHoliday']").val(result.hoursHoliday);
                        //formElements.find("[name='HoursPaternityLeave']").val(result.hoursPaternityLeave);
                        //formElements.find("[name='HoursPaidLeave']").val(result.hoursPaidLeave);
                        //formElements.find("[name='MedicalLeave']").val(result.medicalLeave.toString()).trigger("change");
                        //formElements.find("[name='UnPaidLeave']").val(result.unPaidLeave.toString()).trigger("change");
                        //formElements.find("[name='LaborSuspension']").val(result.laborSuspension.toString()).trigger("change");
                        //formElements.find("[name='NonAttendance']").val(result.nonAttendance.toString()).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($("#project_general_filter").val());
                $(formElement).find("[name='Date']").val($("#day_filter").val());
                $(formElement).find("[name='WorkerId']").val($(formElement).find("[name='select_worker']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/recursos-humanos/tareo-diario/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#add_modal").modal('hide');
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            importday: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($("#project_general_filter").val());
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
                    url: `/recursos-humanos/tareo-diario/importar-dia`,
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
                    datatable.reload();
                    $("#import_day_modal").modal("hide");
                    _app.show.notification.addRange.success();
                    if(result == 1)
                        window.location = `/recursos-humanos/tareo-diario/descargar-excel-errores`;
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            importworkers: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($("#project_general_filter").val());
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
                    url: `/recursos-humanos/tareo-diario/importar-obreros`,
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
                    datatable.reload();
                    $("#import_workers_modal").modal("hide");
                    _app.show.notification.addRange.success();
                    if (result == 1)
                        window.location = `/recursos-humanos/tareo-diario/descargar-excel-errores`;
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_workers_text").html(error.responseText);
                        $("#import_workers_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            edit: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='ProjectCalendarWeek.Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-diario/editar/${id}?weekly=false`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
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
            copy: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($("#project_general_filter").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-diario/copiar-dia`),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#copy_modal").modal('hide');
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#copy_alert_text").html(error.responseText);
                            $("#copy_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            copy: function () {
                copyForm.resetForm();
                $("#copy_form").trigger("reset");
                $("#copy_alert").removeClass("show").addClass("d-none");
            },
            importday: function () {
                //importDayForm.resetForm();
                importDayForm.reset();
                $("#import_day_form").trigger("reset");
                $("#import_day_alert").removeClass("show").addClass("d-none");
            },
            importworkers: function () {
                importWorkersForm.resetForm();
                $("#import_workers_form").trigger("reset");
                $("#import_workers_alert").removeClass("show").addClass("d-none");
            }
        }

    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });

            importDayForm = $("#import_day_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.importday(formElement);
                }
            });

            importWorkersForm = $("#import_workers_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.importworkers(formElement);
                }
            });

            copyForm = $("#copy_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.copy(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });

            $("#import_day_modal").on("hidden.bs.modal",
                function () {
                    form.reset.importday();
                });

            $("#import_workers_modal").on("hidden.bs.modal",
                function () {
                    form.reset.importworkers();
                });

            $("#copy_modal").on("hidden.bs.modal",
                function () {
                    form.reset.copy();
                });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate','today')
        }
    };

    var select2 = {
        init: function () {
            this.workers.init();
            this.phases.init();
            this.workfrontheads.init();
            this.sewergroups.init();
        },
        workers: {
            init: function () {
                let today = $("#day_filter").val();
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/obreros-tareo?dateTask=${today}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-workers").empty();
                let today = $("#day_filter").val();
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/obreros-tareo?dateTask=${today}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result
                    });
                })
            }
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
        },
        workfrontheads: {
            init: function () {
                let pId = $("#project_general_filter").val();
                let today = $("#day_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente-activos?dateStr=${today}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-work-front-heads").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                let today = $("#day_filter").val();
                let pId = $("#project_general_filter").val();
                let wId = $("#search_work_front_head").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?reportDate=${today}&projectId=${pId}&workFrontHeadId=${wId}`)
                }).done(function (result) {
                    $(".select2-sewer-groups").append(selectAllOption).trigger('change');
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-sewer-groups").empty();
                let today = $("#day_filter").val();
                let pId = $("#project_general_filter").val();
                let wId = $("#search_work_front_head").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?reportDate=${today}&projectId=${pId}&workFrontHeadId=${wId}`)
                }).done(function (result) {
                    $(".select2-sewer-groups").append(selectAllOption).trigger('change');
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("#day_filter").on("change", function () {
                select2.workers.reload();
                datatable.reload();
            });

            $("#search_work_front_head").on("change", function () {
                select2.sewergroups.reload();
            });

            $("#search_sewer_group").on("change", function () {
                datatable.reload();
                let sg = $("#search_sewer_group").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/tareo-diario/cuadrilla/${sg}`)
                }).done(function (result) {
                    if (result != null) {
                        if (result.projectCollaboratorId != null)
                            $("#sewer_group_collab").text(result.projectCollaborator.provider.businessName);
                        else
                            $("#sewer_group_collab").text("Casa");
                        $("#sewer_group_resp").text(result.employeeWorkerName);
                    } else {
                        $("#sewer_group_collab").text("");
                        $("#sewer_group_resp").text("");
                    }
                });
            });

            $("#workerImportExcel").on("click", function () {
                window.location = `/recursos-humanos/tareo-diario/descargar-formato-carga-obreros`;
            });
        }
    };

    return {
        init: function () {
            datepicker.init();
            select2.init();
            validate.init();
            events.init();
            modals.init();
            datatable.init();
        }
    };
}();

$(function () {
    PayrollMovementWeek.init();
});