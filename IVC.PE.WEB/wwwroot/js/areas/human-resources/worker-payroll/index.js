var WorkerPayroll = function () {

    var selectWeekOption = new Option('--Seleccione una Semana--', ' ', true, true);

    var workerId = null;

    var conceptEditForm = null;
    var importConceptForm = null;

    var isBusy = false;
    var payrollDt = null;
    var conceptDt = null;

    var payrollOpts = {
        //autoWidth: true,
        //serverSide: true,
        responsive: true,
/*        scrollX: true,*/
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/obreros/planilla/listar`),
            data: function (d) {
                d.weekId = $("#week_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-check-double'></i> Importar Conceptos",
                className: " btn-secondary",
                action: function (e, dt, node, config) {
                    $("#import_concept_modal").modal("show");
                }
            },
            {
                text: "<i class='fa fa-file-excel'></i> Excel",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    let wId = $("#week_filter").val();
                    window.location = _app.parseUrl(`/recursos-humanos/obreros/planilla/planilla-excel/${wId}`);
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
                        url: _app.parseUrl(`/recursos-humanos/obreros/planilla/solicitar-autorizacion?weekId=${wId}&projectId=${pId}`),
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
                        error: function (error) {
                            console.log(error);
                            swal.fire({
                                type: "info",
                                title: "Error",
                                confirmButtonClass: "btn-info",
                                animation: false,
                                customClass: 'animated tada',
                                confirmButtonText: "Entendido",
                                text: error.responseText
                            });
                        },
                        complete: function () {
                            $(btn).removeLoader();
                        }
                    })
                }
            },
            {
                extend: 'colvisGroup',
                text: 'Ingresos',
                show: [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
                hide: [19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37]
            },
            {
                extend: 'colvisGroup',
                text: 'Descuentos',
                show: [19, 20, 21, 22, 23, 24, 25, 26, 27],
                hide: [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37]
            },
            {
                extend: 'colvisGroup',
                text: 'Aportes',
                show: [28, 29, 30, 31, 32],
                hide: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 33, 34, 35, 36, 37]
            },
            {
                extend: 'colvisGroup',
                text: 'Totales',
                show: [33, 34, 35, 36, 37],
                hide: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32]
            },
            {
                extend: 'colvisGroup',
                text: 'Ver Todos',
                show: ':hidden'
            }
        ],
        columns: [
            {
                title: "Trabajador",
                data: "worker.fullName",
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup",
            },
            {
                title: "Categoría",
                data: "worker.categoryDesc"
            },
            {
                title: "Salario",
                data: "salary"
            },
            {
                title: "Dominical",
                data: "sunday"
            },
            {
                title: "Asig.Escolar",
                data: "schoolAssignment"
            },
            {
                title: "Feriados",
                data: "holidays"
            },
            {
                title: "Desc.Médico",
                data: "medicalRest"
            },
            {
                title: "Lic.Paternidad",
                data: "paternalLeave"
            },
            {
                title: "H.E.60%",
                data: "extraHours60"
            },
            {
                title: "H.E.100%",
                data: "extraHours100"
            },
            {
                title: "Movilidad",
                data: "mobility"
            },
            {
                title: "Gratif.",
                data: "gratification"
            },
            {
                title: "Bonif.Extr.",
                data: "extraordinaryBonification"
            },
            {
                title: "Vacaciones",
                data: "vacations"
            },
            {
                title: "CTS",
                data: "cts"
            },
            {
                title: "BUC",
                data: "buc"
            },
            {
                title: "Lic.Con Goce",
                data: "paidLeave"
            },
            {
                title: "Subsidio Enf.",
                data: "medicalLeave"
            },
            {
                title: "ONP",
                data: "onp"
            },
            {
                title: "AFP-Fondo",
                data: "afpFund"
            },
            {
                title: "AFP-Com.Flujo",
                data: "afpFlowCommission"
            },
            {
                title: "AFP-Com.Mixta",
                data: "afpMixedCommission"
            },
            {
                title: "AFP-Seg.Invalidez",
                data: "afpDisabilityInsurance"
            },
            {
                title: "Conafovicer",
                data: "conafovicer"
            },
            {
                title: "Qta. Categoría",
                data: "fifthCategoryTaxes"
            },
            {
                title: "Ret.Judicial",
                data: "judicialRetention"
            },
            {
                title: "Cuota Sindical",
                data: "unionFee"
            },
            {
                title: "EsSalud",
                data: "esSalud"
            },
            {
                title: "AFP-Jub.Anticipada",
                data: "afpEarlyRetirement"
            },
            {
                title: "SCTR Salud",
                data: "sctrHealth"
            },
            {
                title: "SCTR Pensión",
                data: "sctrPension"
            },
            {
                title: "EsSalud+Vida",
                data: "esSaludMasVida"
            },
            {
                title: "Total Remuneración",
                data: "totalRem"
            },
            {
                title: "Total Descuentos",
                data: "totalDis"
            },
            {
                title: "Total Aportes",
                data: "totalCon"
            },
            {
                title: "Total Costos",
                data: "totalCos"
            },
            {
                title: "Total Neto",
                data: "totalNet"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-concept">`;
                    tmp += `<i class="fas fa-file-invoice"></i></button>`;
                    return tmp;
                }
            }
        ],
        initComplete: function (settings, json) {
            isBusy = false;
        }
    };
    var conceptOpts = {
        //autoWidth: true,
        //serverSide: true,
        responsive: true,
        /*        scrollX: true,*/
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/conceptos/obrero/listar`),
            data: function (d) {
                d.weekId = $("#week_filter").val();
                d.workerId = workerId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Código",
                data: "payrollConcept.code"
            },
            {
                title: "Concepto",
                data: "payrollConcept.description"
            },
            {
                title: "Importe",
                data: "value"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" data-hid="${row.payrollMovementHeaderId}" data-cid="${row.payrollConceptId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.payroll.init();
            this.concept.init();
        },
        payroll: {
            init: function () {
                payrollDt = $("#worker_payroll_datatable").DataTable(payrollOpts);
                this.events();
            },
            reload: function () {
                payrollDt.clear().draw();
                payrollDt.ajax.reload();
            },
            events: function () {
                payrollDt.on("click", ".btn-concept", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    workerId = id;
                    datatables.concept.reload();
                    $("#concept_modal").modal("show");
                });
            }
        },
        concept: {
            init: function () {
                conceptDt = $("#concepts_datatable").DataTable(conceptOpts);
                this.events();
            },
            reload: function () {
                conceptDt.clear().draw();
                conceptDt.ajax.reload();
            },
            events: function () {
                conceptDt.on("click", ".btn-edit", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let hid = $btn.data("hid");
                    let cid = $btn.data("cid");
                    form.load.concept.edit(id, hid, cid);
                });
            }
        }
    };

    var form = {
        load: {
            concept: {
                edit: function (id, hid, cid) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/conceptos/obrero?wid=${workerId}&did=${id}&hid=${hid}&cid=${cid}`)
                    })
                        .done(function (result) {
                            let formElements = $("#concept_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkerId']").val(result.workerId);
                            formElements.find("[name='PayrollMovementHeaderId']").val(result.payrollMovementHeaderId);
                            formElements.find("[name='PayrollConceptId']").val(result.payrollConceptId);
                            formElements.find("[name='Value']").val(result.value);
                            $("#concept_modal").modal('hide');
                            $("#concept_edit_modal").modal('show');
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            concept: {
                edit: function (formElement) {
                    console.log("llegue");
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/conceptos/obrero/editar`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                            datatables.concept.reload();
                            datatables.payroll.reload();
                            $("#concept_edit_modal").modal("hide");
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#concept_edit_alert_text").html(error.responseText);
                                $("#concept_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                },
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
                        url: "/recursos-humanos/obreros/planilla/importar/actualizaciones",
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
                        datatables.payroll.reload();
                        $("#import_concept_modal").modal("hide");
                        _app.show.notification.edit.success();
                        /*
                        if (result > 0)
                            window.location = `/recursos-humanos/obreros/importar/errores`;*/
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_concept_alert_text").html(error.responseText);
                            $("#import_concept_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            concept: {
                edit: function () {
                    conceptEditForm.resetForm();
                    $("#concept_edit_form").trigger("reset");
                    $("#concept_edit_form_alert").removeClass("show").addClass("d-none");
                    $("#concept_modal").modal('show');
                },
                import: function () {
                    conceptEditForm.resetForm();
                    $("#import_concept_form").trigger("reset");
                    $("#import_concept_form_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    }

    var validate = {
        init: function () {
            conceptEditForm = $("#concept_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.concept.edit(formElement);
                }
            });

            importConceptForm = $("#import_concept_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.concept.import(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#concept_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.concept.edit();
                });

            $("#import_concept_modal").on("hidden.bs.modal",
                function () {
                    form.reset.concept.import();
                });
        }
    }

    var select2 = {
        init: function () {
            //this.projects.init();
            this.weeks.init();
        },
        //projects: {
        //    init: function () {
        //        $.ajax({
        //            url: _app.parseUrl("/select/proyectos")
        //        }).done(function (result) {
        //            $(".select2-projects").select2({
        //                data: result
        //            });
        //        })
        //    }
        //},
        weeks: {
            init: function () {
                let pId = $("#project_general_filter").val();
                let year = $("#year_filter").val();
                $(".select2-weeks").append(selectWeekOption).trigger('change');
                $.ajax({
                    url: _app.parseUrl(`/select/semanas??projectId=${pId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-weeks").empty();
                $(".select2-weeks").append(selectWeekOption).trigger('change');
                let pId = $("#project_general_filter").val();
                let year = $("#year_filter").val();
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

            $("#year_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#week_filter").on("change", function (e) {
                if (!isBusy) {
                    isBusy = false;
                    datatables.payroll.reload();
                    events.getPayrollStatus();
                    events.getAuthStatus();
                }                
            });

            $("#conceptSample").on("click", function () {
                window.location = `/recursos-humanos/obreros/planilla/excel`;
            });
        },
        getPayrollStatus: function () {
            let id = $("#week_filter").val();
            $.ajax({
                url: _app.parseUrl(`/recursos-humanos/obreros/planilla/estado/${id}`)
            }).done(function (result) {
                if (result == 0) {
                    $("#payroll_status").removeClass('kt-badge--primary').removeClass('kt-badge--success').addClass('kt-badge--secondary').text("Sin Procesar");
                } else if (result == 1) {
                    $("#payroll_status").removeClass('kt-badge--secondary').removeClass('kt-badge--success').addClass('kt-badge--primary').text("En Proceso");
                } else {
                    $("#payroll_status").removeClass('kt-badge--primary').removeClass('kt-badge--secondary').addClass('kt-badge--success').text("Calculado");
                }
            });
        },
        getAuthStatus: function () {
            let id = $("#week_filter").val();
            $.ajax({
                url: _app.parseUrl(`/recursos-humanos/obreros/planilla/autorizacion/estado/${id}`)
            }).done(function (result) {
                $("#responsible").text(result.authResponsible);
                if (result.requested) {
                    if (!result.answered) {
                        $("#responsible_auth").removeClass('kt-badge--danger').removeClass('kt-badge--success').removeClass('kt-badge--dark').addClass('kt-badge--warning').text("Pendiente de Respuesta");
                    }
                    else if (result.authStatus) {
                        $("#responsible_auth").removeClass('kt-badge--danger').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--success').text("Autorizado");
                    } else {
                        $("#responsible_auth").removeClass('kt-badge--success').removeClass('kt-badge--warning').removeClass('kt-badge--dark').addClass('kt-badge--danger').text("Rechazado");
                    }
                }
                else {
                    $("#responsible_auth").removeClass('kt-badge--success').removeClass('kt-badge--danger').removeClass('kt-badge--warning').addClass('kt-badge--dark').text("No Alertado")
                }
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            events.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    WorkerPayroll.init();
});