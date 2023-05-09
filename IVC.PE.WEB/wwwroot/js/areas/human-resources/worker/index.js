var Worker = function () {
    var workerId = null;

    var workerDatatable = null;
    var periodDatatable = null;
    var fixedConceptDatatable = null;

    var workerAddForm = null;
    var workerEditForm = null;
    var workerCeaseForm = null;
    var workerNewEntryForm = null;
    var periodAddForm = null;
    var periodEditForm = null;
    var fixedConceptAddForm = null;
    var fixedConceptEditForm = null;

    var importNewEntryForm = null;
    var importReEntryForm = null;
    var importCeaseForm = null;
    var importUpdateForm = null;

    var tregUpForm = null;

    var workerOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/obreros/listar"),
            data: function (d) {
                d.category = $("#category_filter").val();
                d.origin = $("#origin_filter").val();
                d.workgroup = $("#workgroup_filter").val();
                d.status = $("#status_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fas fa-download'></i> Exportar",
                className: " btn-success",
                action: function (e, dt, node, config) {
                    let category = $("#category_filter").val();
                    let origin = $("#origin_filter").val();
                    let workgroup = $("#workgroup_filter").val();
                    let status = $("#status_filter").val();
                    window.location = _app.parseUrl(`/recursos-humanos/obreros/exportar?status=${status}&category=${category}&origin=${origin}&workgroup=${workgroup}`);
                }
            },
        ],
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
                title: "Carpeta",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" data-name="${row.fullName}" class="btn btn-info btn-sm btn-icon btn-periods" data-toggle="tooltip" title="Períodos Laborales">`;
                    tmp += `<i class="fa fa-list-alt"></i></button> `;
                    tmp += `<button data-id="${row.workerId}" data-name="${row.fullName}" class="btn btn-success btn-sm btn-icon btn-concepts" data-toggle="tooltip" title="Conceptos Fijos">`;
                    tmp += `<i class="fa fa-list-alt"></i></button> `;
                    tmp += `<button data-id="${row.workerId}" class="btn btn-primary btn-sm btn-icon btn-photocheck" data-toggle="tooltip" title="Fotocheck">`;
                    tmp += `<i class="fa fa-id-badge"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.isActive == true) {
                        tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-warning btn-sm btn-icon btn-cease" data-toggle="tooltip" title="Cesar">`;
                        tmp += `<i class="fa fa-calendar-times"></i></button>`;
                    } else {
                        tmp += `<button data-id="${row.workerWorkPeriodId}" data-name="${row.fullName}" class="btn btn-success btn-sm btn-icon btn-new-entry" data-toggle="tooltip" title="Reingreso">`;
                        tmp += `<i class="fa fa-calendar-plus"></i></button>`;
                    }
                    tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var periodOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/obreros/periodos-laborales/listar"),
            data: function (d) {
                d.workerId = workerId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Agregar Período",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#period_modal").modal("hide");
                    $("#period_add_modal").modal("show");
                }
            }
        ],
        columns: [
            {
                title: "Proyecto",
                data: "project.abbreviation"
            },
            {
                title: "Fecha Ingreso",
                data: "entryDate",
            },
            {
                title: "Fecha Cese",
                data: "ceaseDate"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    if (meta.row == 0) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                        tmp += `<i class="fa fa-edit"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                        tmp += `<i class="fa fa-trash"></i></button>`;
                    }
                    return tmp;
                }
            }
        ]
    };
    var fixeConceptOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/obreros/conceptos-fijos/listar"),
            data: function (d) {
                d.workerId = workerId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Agregar Concepto",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#fixed_concept_modal").modal("hide");
                    $("#fixed_concept_add_modal").modal("show");
                }
            }
        ],
        columns: [
            {
                title: "Concepto",
                data: "payrollConcept.description"
            },
            {
                title: "Tipo",
                data: "payrollConcept.categoryName"
            },
            {
                title: "Importe",
                data: "fixedValue",
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.workerDt.init();
            this.periodDt.init();
            this.fixedConceptDt.init();
        },
        workerDt: {
            init: function () {
                workerDatatable = $("#worker_datatable").DataTable(workerOpts);
                this.events();    
            },
            reload: function() {
                workerDatatable.ajax.reload();
            },
            events: function () {
                workerDatatable.on("click",
                    ".btn-periods",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        workerId = id;
                        datatables.periodDt.reload();
                        $("#worker_name").html(name);
                        $("#period_modal").modal("show");
                    });

                workerDatatable.on("click",
                    ".btn-concepts",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        workerId = id;
                        datatables.fixedConceptDt.reload();
                        $("#worker_name_cf").html(name);
                        $("#fixed_concept_modal").modal("show");
                    });

                workerDatatable.on("click",
                    ".btn-photocheck",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        workerId = id;
                        window.open(`/recursos-humanos/obreros/fotocheck/${workerId}`,`_blank`);
                    });

                workerDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.worker.edit(id);
                    });

                workerDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El obrero será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/recursos-humanos/obreros/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El obrero ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar al obrero"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                workerDatatable.on("click",
                    ".btn-cease",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        $("#worker_name").html(name);
                        forms.load.worker.cease(id);
                    });

                workerDatatable.on("click",
                    ".btn-new-entry",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        $("#worker_name").html(name);
                        forms.load.worker.newEntry(id);
                    });
            }
        },
        periodDt: {
            init: function () {
                periodDatatable = $("#periods_datatable").DataTable(periodOpts);
                this.events();
            },
            reload: function () {
                periodDatatable.ajax.reload();
            },
            events: function () {
                periodDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#period_modal").modal("hide");
                        forms.load.period.edit(id);
                    });

                periodDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El período laboral será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.periodDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El período laboral ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar al período"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        fixedConceptDt: {
            init: function () {
                fixedConceptDatatable = $("#fixed_concepts_datatable").DataTable(fixeConceptOpts);
                this.events();
            },
            reload: function () {
                fixedConceptDatatable.ajax.reload();
            },
            events: function () {
                fixedConceptDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#fixed_concept_modal").modal("hide");
                        forms.load.fixedConcept.edit(id);
                    });

                fixedConceptDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El concepto fijo será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/recursos-humanos/obreros/conceptos-fijos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.fixedConceptDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El concepto fijo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el concepto"
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
            worker: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#worker_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkPeriodId']").val(result.workPeriodId);
                            formElements.find("[name='WorkPeriod.ProjectId']").val(result.workPeriod.projectId).trigger("change");
                            formElements.find("[name='Name']").val(result.name);
                            formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                            formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                            formElements.find("[name='Email']").val(result.email);
                            formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                            formElements.find("[name='DocumentType']").val(result.documentType).trigger("change");
                            formElements.find("[name='BankId']").val(result.bankId).trigger("change");
                            formElements.find("[name='Document']").val(result.document);
                            formElements.find("[name='WorkPeriod.EntryDate']").datepicker("setDate", result.workPeriod.entryDate);
                            formElements.find("[name='WorkPeriod.Category']").val(result.workPeriod.category).trigger("change");
                            formElements.find("[name='BirthDate']").datepicker("setDate", result.birthDate);
                            formElements.find("[name='WorkPeriod.WorkerPositionId']").val(result.workPeriod.workerPositionId).trigger("change");
                            formElements.find("[name='WorkPeriod.Origin']").val(result.workPeriod.origin).trigger("change");
                            formElements.find("[name='WorkPeriod.Workgroup']").val(result.workPeriod.workgroup).trigger("change");
                            formElements.find("[name='WorkPeriod.PensionFundAdministratorId']").val(result.workPeriod.pensionFundAdministratorId).trigger("change");
                            formElements.find("[name='WorkPeriod.PensionFundUniqueIdentificationCode']").val(result.workPeriod.pensionFundUniqueIdentificationCode);
                            formElements.find("[name='WorkPeriod.NumberOfChildren']").val(result.workPeriod.numberOfChildren);
                            formElements.find("[name='WorkPeriod.JudicialRetentionFixedAmmount']").val(result.workPeriod.judicialRetentionFixedAmmount);
                            formElements.find("[name='WorkPeriod.JudicialRetentionPercentRate']").val(result.workPeriod.judicialRetentionPercentRate);
                            $("#worker_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                cease: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#worker_cease_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                            formElements.find("[name='CeaseDate']").datepicker("setDate", result.ceaseDate);
                            $("#worker_cease_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                newEntry: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#worker_new_entry_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkerId']").val(result.workerId);
                            formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                            //formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                            formElements.find("[name='Category']").val(result.category).trigger("change");
                            formElements.find("[name='WorkerPositionId']").val(result.workerPositionId).trigger("change");
                            formElements.find("[name='Origin']").val(result.origin).trigger("change");
                            formElements.find("[name='Workgroup']").val(result.workgroup).trigger("change");
                            formElements.find("[name='PensionFundAdministratorId']").val(result.pensionFundAdministratorId).trigger("change");
                            formElements.find("[name='PensionFundUniqueIdentificationCode']").val(result.pensionFundUniqueIdentificationCode);
                            formElements.find("[name='NumberOfChildren']").val(result.numberOfChildren);
                            formElements.find("[name='JudicialRetentionFixedAmmount']").val(result.judicialRetentionFixedAmmount);
                            formElements.find("[name='JudicialRetentionPercentRate']").val(result.judicialRetentionPercentRate);
                            $("#worker_new_entry_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            },
            period: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#period_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkerId']").val(result.workerId);
                            formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                            formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                            formElements.find("[name='CeaseDate']").datepicker("setDate", result.ceaseDate);
                            formElements.find("[name='Category']").val(result.category).trigger("change");
                            formElements.find("[name='WorkerPositionId']").val(result.workerPositionId).trigger("change");
                            formElements.find("[name='Origin']").val(result.origin).trigger("change");
                            formElements.find("[name='Workgroup']").val(result.workgroup).trigger("change");
                            formElements.find("[name='PensionFundAdministratorId']").val(result.pensionFundAdministratorId).trigger("change");
                            formElements.find("[name='PensionFundUniqueIdentificationCode']").val(result.pensionFundUniqueIdentificationCode);
                            formElements.find("[name='NumberOfChildren']").val(result.numberOfChildren);
                            formElements.find("[name='JudicialRetentionFixedAmmount']").val(result.judicialRetentionFixedAmmount);
                            formElements.find("[name='JudicialRetentionPercentRate']").val(result.judicialRetentionPercentRate);
                            $("#period_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            },
            fixedConcept: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/conceptos-fijos/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#fixed_concept_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkerId']").val(result.workerId);
                            formElements.find("[name='PayrollConceptId']").val(result.payrollConceptId).trigger("change");
                            formElements.find("[name='FixedValue']").val(result.fixedValue);
                            $("#fixed_concept_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            worker: {
                add: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input, select").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: _app.parseUrl("/recursos-humanos/obreros/crear"),
                        method: "post",
                        data: data,
                        contentType: false,
                        processData: false,
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
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                            $(".progress").empty().remove();
                            if (!emptyFile) {
                                $("#space-bar").remove();
                            }
                        })
                        .done(function (result) {
                            datatables.workerDt.reload();
                            $("#worker_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#worker_add_alert_text").html(error.responseText);
                                $("#worker_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                edit: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input, select").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/editar/${id}`),
                        method: "put",
                        data: data,
                        contentType: false,
                        processData: false,
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
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                            $(".progress").empty().remove();
                            if (!emptyFile) {
                                $("#space-bar").remove();
                            }
                        })
                        .done(function (result) {
                            datatables.workerDt.reload();
                            $("#worker_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#worker_edit_alert_text").html(error.responseText);
                                $("#worker_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                },
                cease: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/cesar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.workerDt.reload();
                            $("#worker_cease_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#worker_cease_alert_text").html(error.responseText);
                                $("#worker_cease_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                },
                newEntry: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/recursos-humanos/obreros/periodos-laborales/crear"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.workerDt.reload();
                            $("#worker_new_entry_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#worker_new_entry_alert_text").html(error.responseText);
                                $("#worker_new_entry_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            },
            period: {
                add: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/recursos-humanos/obreros/periodos-laborales/crear"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.periodDt.reload();
                            $("#period_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#period_add_alert_text").html(error.responseText);
                                $("#period_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                edit: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/periodos-laborales/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.periodDt.reload();
                            $("#period_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#period_edit_alert_text").html(error.responseText);
                                $("#period_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            },
            fixedConcept: {
                add: function (formElement) {
                    $(formElement).find("input[name='WorkerId']").val(workerId);
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/recursos-humanos/obreros/conceptos-fijos/crear"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.fixedConceptDt.reload();
                            $("#fixed_concept_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#period_add_alert_text").html(error.responseText);
                                $("#period_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                edit: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/conceptos-fijos/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.fixedConceptDt.reload();
                            $("#fixed_concept_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#fixed_concept_edit_alert_text").html(error.responseText);
                                $("#fixed_concept_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            },
            treg: {
                up: function (idate, edate) {
                    $("#treg_up_modal").modal('hide');
                    window.location = _app.parseUrl(`/recursos-humanos/obreros/altas?idate=${idate}&edate=${edate}`);
                }
            },
            import: {
                newEntries: function (formElement) {
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
                        url: "/recursos-humanos/obreros/importar/nuevos",
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
                        datatables.workerDt.reload();
                        $("#import_new_entry_modal").modal("hide");
                        _app.show.notification.addRange.success();
                        if (result > 0)
                            window.location = `/recursos-humanos/obreros/importar/errores`;
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_new_entry_alert_text").html(error.responseText);
                            $("#import_new_entry_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },
                reEntries: function (formElement) {
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
                        url: "/recursos-humanos/obreros/importar/reingresos",
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
                        datatables.workerDt.reload();
                        $("#import_re_entry_modal").modal("hide");
                        _app.show.notification.addRange.success();
                        if (result > 0)
                            window.location = `/recursos-humanos/obreros/importar/errores`;
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_re_entry_alert_text").html(error.responseText);
                            $("#import_re_entry_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },
                ceases: function (formElement) {
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
                        url: "/recursos-humanos/obreros/importar/ceses",
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
                        datatables.workerDt.reload();
                        $("#import_cease_modal").modal("hide");
                        _app.show.notification.addRange.success();
                        if (result > 0)
                            window.location = `/recursos-humanos/obreros/importar/errores`;
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_cease_alert_text").html(error.responseText);
                            $("#import_cease_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },
                updates: function (formElement) {
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
                        url: "/recursos-humanos/obreros/importar/actualizaciones",
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
                        datatables.workerDt.reload();
                        $("#import_update_modal").modal("hide");
                        _app.show.notification.addRange.success();
                        if (result > 0)
                            window.location = `/recursos-humanos/obreros/importar/errores`;
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_update_alert_text").html(error.responseText);
                            $("#import_update_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            worker: {
                add: function () {
                    workerAddForm.resetForm();
                    $("#worker_add_form").trigger("reset");
                    $("#worker_add_alert").removeClass("show").addClass("d-none");
                },
                edit: function () {
                    workerEditForm.resetForm();
                    $("#worker_edit_form").trigger("reset");
                    $("#worker_edit_alert").removeClass("show").addClass("d-none");
                },
                cease: function () {
                    workerCeaseForm.resetForm();
                    $("#worker_cease_form").trigger("reset");
                    $("#worker_cease_alert").removeClass("show").addClass("d-none");
                },
                newEntry: function () {
                    workerNewEntryForm.resetForm();
                    $("#worker_new_entry_form").trigger("reset");
                    $("#worker_new_entry_alert").removeClass("show").addClass("d-none");
                }
            },
            period: {
                add: function () {
                    periodAddForm.resetForm();
                    $("#period_add_form").trigger("reset");
                    $("#period_add_alert").removeClass("show").addClass("d-none");
                },
                edit: function () {
                    periodEditForm.resetForm();
                    $("#period_edit_form").trigger("reset");
                    $("#perio_edit_alert").removeClass("show").addClass("d-none");
                }
            },
            fixedConcept: {
                add: function () {
                    fixedConceptAddForm.resetForm();
                    $("#fixed_concept_add_form").trigger("reset");
                    $("#fixed_add_alert").removeClass("show").addClass("d-none");
                    $("#fixed_concept_modal").modal("show");
                },
                edit: function () {
                    fixedConceptEditForm.resetForm();
                    $("#fixed_concept_edit_form").trigger("reset");
                    $("#fixed_edit_alert").removeClass("show").addClass("d-none");
                    $("#fixed_concept_modal").modal("show");
                }
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
            //this.projects.init();
            this.sewerGroups.init();
            this.concepts.init();
            this.banks.init();
            this.gender.init();
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
        //projects: {
        //    init: function () {
        //        $.ajax({
        //            url: _app.parseUrl("/select/proyectos/usuario")
        //        }).done(function (result) {
        //            $(".select2-projects").select2({
        //                data: result
        //            });
        //        });
        //    }
        //},
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
        },
        concepts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/conceptos-fijos?laborRegime=2")
                }).done(function (result) {
                    $(".select2-concepts").select2({
                        data: result
                    });
                })
            }
        },
        banks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/bancos")
                }).done(function (result) {
                    $(".select2-banks").select2({
                        data: result
                    });
                })
            }
        },
        gender: {
            init: function () {
                $(".select2-gender").select2();
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var validate = {
        init: function () {
            workerAddForm = $("#worker_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.worker.add(formElement);
                }
            });

            workerEditForm = $("#worker_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.worker.edit(formElement);
                }
            });

            workerCeaseForm = $("#worker_cease_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.worker.cease(formElement);
                }
            });

            workerNewEntryForm = $("#worker_new_entry_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.worker.newEntry(formElement);
                }
            });

            periodAddForm = $("#period_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.period.add(formElement);
                }
            });

            periodEditForm = $("#period_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.period.edit(formElement);
                }
            });

            fixedConceptAddForm = $("#fixed_concept_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.fixedConcept.add(formElement);
                }
            });

            fixedConceptEditForm = $("#fixed_concept_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.fixedConcept.edit(formElement);
                }
            });

            tregUpForm = $("#treg_up_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    let idate = $("#tregUpSince").val();
                    let edate = $("#tregUpUntil").val();
                    forms.submit.treg.up(idate, edate);
                }
            });

            importNewEntryForm = $("#import_new_entry_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.newEntries(formElement);
                }
            });

            importReEntryForm = $("#import_re_entry_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.reEntries(formElement);
                }
            });

            importCeaseForm = $("#import_cease_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.ceases(formElement);
                }
            });

            importUpdateForm = $("#import_update_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.updates(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#worker_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.worker.add();
                });

            $("#worker_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.worker.edit();
                });

            $("#worker_cease_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.worker.cease();
                });

            $("#worker_new_entry_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.worker.newEntry();
                });

            $("#period_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.period.add();
                });

            $("#period_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.period.edit();
                });

            $("#fixed_concept_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.fixedConcept.add();
                });

            $("#fixed_concept_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.fixedConcept.edit();
                });
        }
    };

    var events = {
        init: function () {
            $("#worker_add_form [name='WorkPeriod.ProjectId']").attr("id", "Add_WorkPeriod_ProjectId");
            $("#worker_edit_form [name='WorkPeriod.ProjectId']").attr("id", "Edit_WorkPeriod_ProjectId");

            $("#worker_add_form [name='DocumentType']").attr("id", "Add_DocumentType");
            $("#worker_edit_form [name='DocumentType']").attr("id", "Edit_DocumentType");

            $("#worker_add_form [name='Gender']").attr("id", "Add_Gender");
            $("#worker_edit_form [name='Gender']").attr("id", "Edit_Gender");

            $("#worker_add_form [name='WorkPeriod.Category']").attr("id", "Add_WorkPeriod_Category");
            $("#worker_edit_form [name='WorkPeriod.Category']").attr("id", "Edit_WorkPeriod_Category");

            $("#worker_add_form [name='WorkPeriod.WorkerPositionId']").attr("id", "Add_WorkPeriod_WorkerPositionId");
            $("#worker_edit_form [name='WorkPeriod.WorkerPositionId']").attr("id", "Edit_WorkPeriod_WorkerPositionId");

            $("#worker_add_form [name='WorkPeriod.Origin']").attr("id", "Add_WorkPeriod_Origin");
            $("#worker_edit_form [name='WorkPeriod.Origin']").attr("id", "Edit_WorkPeriod_Origin");

            $("#worker_add_form [name='WorkPeriod.Workgroup']").attr("id", "Add_WorkPeriod_Workgroup");
            $("#worker_edit_form [name='WorkPeriod.Workgroup']").attr("id", "Edit_WorkPeriod_Workgroup");

            $("#worker_add_form [name='WorkPeriod.PensionFundAdministratorId']").attr("id", "Add_WorkPeriod_PensionFundAdministratorId");
            $("#worker_edit_form [name='WorkPeriod.PensionFundAdministratorId']").attr("id", "Edit_WorkPeriod_PensionFundAdministratorId");

            $("#worker_add_form [name='BankId']").attr("id", "Add_BankId");
            $("#worker_edit_form [name='BankId']").attr("id", "Edit_BankId");

            $("#period_add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#period_edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");

            $("#period_add_form [name='WorkPeriod.Category']").attr("id", "Add_Category");
            $("#period_edit_form [name='WorkPeriod.Category']").attr("id", "Edit_Category");

            $("#period_add_form [name='WorkerPositionId']").attr("id", "Add_WorkerPositionId");
            $("#period_edit_form [name='WorkerPositionId']").attr("id", "Edit_WorkerPositionId");

            $("#period_add_form [name='Origin']").attr("id", "Add_Origin");
            $("#period_edit_form [name='Origin']").attr("id", "Edit_Origin");

            $("#period_add_form [name='Workgroup']").attr("id", "Add_Workgroup");
            $("#period_edit_form [name='Workgroup']").attr("id", "Edit_Workgroup");

            $("#period_add_form [name='PensionFundAdministratorId']").attr("id", "Add_PensionFundAdministratorId");
            $("#period_edit_form [name='PensionFundAdministratorId']").attr("id", "Edit_PensionFundAdministratorId");

            $("#fixed_concept_add_form [name='PayrollConceptId']").attr("id", "Add_PayrollConceptId");
            $("#fixed_concept_edit_form [name='PayrollConceptId']").attr("id", "Edit_PayrollConceptId");

            $("#category_filter, #origin_filter, #workgroup_filter, #status_filter").on("change", function () {
                datatables.workerDt.reload();
            });
            
            $(".btn-export-workers").on("click", function () {
                window.location = `/recursos-humanos/obreros/exportar`;
            });

            //$("#btnNewImport").on("click", function () {
            //    $("#import_new_entry_modal").modal("show");
            //});
            //$("#btnReImport").on("click", function () {
            //    $("#import_re_entry_modal").modal("show");
            //});
            //$("#btnCeaseImport").on("click", function () {
            //    $("#import_cease_modal").modal("show");
            //});

            $("#newEntrySample").on("click", function () {
                window.location = `/recursos-humanos/obreros/importar/excel_nuevos`;
            });
            $("#reEntrySample").on("click", function () {
                window.location = `/recursos-humanos/obreros/importar/excel_reingresos`;
            });
            $("#ceaseSample").on("click", function () {
                window.location = `/recursos-humanos/obreros/importar/excel_ceses`;
            });
            $("#updateSample").on("click", function () {
                window.location = `/recursos-humanos/obreros/importar/actualizaciones`;
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            datepicker.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    Worker.init();
});