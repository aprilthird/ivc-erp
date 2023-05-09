var Worker = function () {

    var workerDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var ceaseForm = null;
    var newEntryForm = null;

    var options = {
        responsive: true,
        //serverSide: true,
        //processing: true,
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
                    tmp += `<button data-id="${row.workerId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.isActive == true) {
                        tmp += `<button data-id="${row.workerId}" class="btn btn-warning btn-sm btn-icon btn-cease">`;
                        tmp += `<i class="fa fa-calendar-times"></i></button>`;
                    } else {
                        tmp += `<button data-id="${row.workerId}" class="btn btn-success btn-sm btn-icon btn-new-entry">`;
                        tmp += `<i class="fa fa-calendar-plus"></i></button>`;
                    }
                    tmp += `<button data-id="${row.workerId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            workerDatatable = $("#worker_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            workerDatatable.ajax.reload();
        },
        initEvents: function () {
            workerDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
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
                    form.load.cease(id);
                });

            workerDatatable.on("click",
                ".btn-new-entry",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.newentry(id);
                });
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='DocumentType']").val(result.documentType).trigger("change");
                        formElements.find("[name='Document']").val(result.document);
                        formElements.find("[name='BirthDate']").datepicker("setDate", result.birthDate);
                        formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='PensionFundAdministratorId']").val(result.pensionFundAdministratorId).trigger("change");
                        formElements.find("[name='PensionFundUniqueIdentificationCode']").val(result.pensionFundUniqueIdentificationCode);
                        formElements.find("[name='Category']").val(result.category).trigger("change");
                        formElements.find("[name='Origin']").val(result.origin).trigger("change");
                        formElements.find("[name='Workgroup']").val(result.workgroup).trigger("change");
                        formElements.find("[name='WorkerPositionId']").val(result.workerPositionId).trigger("change");
                        formElements.find("[name='NumberOfChildren']").val(result.numberOfChildren);
                        formElements.find("[name='JudicialRetentionFixedAmmount']").val(result.judicialRetentionFixedAmmount);
                        formElements.find("[name='JudicialRetentionPercentRate']").val(result.judicialRetentionPercentRate);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            cease: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#cease_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                        formElements.find("[name='EntryDate']").attr("disabled","disabled");
                        $("#cease_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            newentry: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#new_entry_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='DocumentType']").val(result.documentType);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='Category']").val(result.category).trigger("change");
                        formElements.find("[name='EntryDate']").datepicker("setDate", result.entryDate);
                        formElements.find("[name='Origin']").val(result.origin).trigger("change");
                        formElements.find("[name='Workgroup']").val(result.workgroup).trigger("change");
                        formElements.find("[name='WorkerPositionId']").val(result.workerPositionId).trigger("change");
                        $("#new_entry_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/recursos-humanos/obreros/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#add_modal").modal("hide");
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
            edit: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            import: function (formElement) {
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
                    url: "/recursos-humanos/obreros/importar",
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
                    $("#import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            cease: function (formElement) {
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                let cDate = $(formElement).find("input[name='CeaseDate']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/cesar?id=${id}&ceaseDate=${cDate}`),
                    method: "put"
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#cease_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#cease_alert_text").html(error.responseText);
                            $("#cease_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            newEntry: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/reingreso/${id}`),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#new_entry_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        $("#new_entry_alert_text").text(error.responseText);
                        $("#new_entry_alert").removeClass("d-none").addClass("show");
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
            cease: function () {
                ceaseForm.resetForm();
                $("#cease_form").trigger("reset");
                $("#cease_alert").removeClass("show").addClass("d-none");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            newEntry: function () {
                newEntryForm.resetForm();
                $("#new_entry_form").trigger("reset");
                $("#new_entry_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.documentTypes.init();
            this.categories.init();
            this.workerPositions.init();
            this.fundPension.init();
            this.projects.init();
            this.sewerGroups.init();
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
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
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

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            ceaseForm = $("#cease_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.cease(formElement);
                }
            });

            newEntryForm = $("#new_entry_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.newEntry(formElement);
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

            $("#cease_modal").on("hidden.bs.modal",
                function () {
                    form.reset.cease();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='DocumentType']").attr("id", "Add_DocumentType");
            $("#edit_form [name='DocumentType']").attr("id", "Edit_DocumentType");
            $("#add_form [name='WorkerPositionId']").attr("id", "Add_WorkerPositionId");
            $("#edit_form [name='WorkerPositionId']").attr("id", "Edit_WorkerPositionId");
            $("#add_form [name='Category']").attr("id", "Add_Category");
            $("#edit_form [name='Category']").attr("id", "Edit_Category");
            //$("#add_modal").on("shown.bs.modal", function () {
            //    $("#Add_RoleIds").select2();
            //});
            //$("#edit_modal").on("shown.bs.modal", function () {
            //    $("#Edit_RoleIds").select2();
            //});
            $("#category_filter, #origin_filter, #workgroup_filter, #status_filter").on("change", function () {
                datatable.reload();
            });
            
            $(".btn-export-workers").on("click", function () {
                window.location = `/recursos-humanos/obreros/exportar`;
            });

            $("#btn-massive-load").on("click", function () {
                window.location = `/recursos-humanos/obreros/excel-carga-masiva`;
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Worker.init();
});