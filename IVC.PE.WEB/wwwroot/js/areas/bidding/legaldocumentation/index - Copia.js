var Formula = function () {

    var mainDatatable = null;
    var detailDatatables = {};
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var addFileForm = null;
    var providerId = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/agregarfianza/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Detalle",
                width: "5%",
                className: "details-control",
                orderable: false,
                data: null,
                defaultContent: "<i class='flaticon2-next'></i>"
            },
            {
                title: "Garante",
                data: "bondGuarantor.name"
            },
            {
                title: "Título",
                data: "budgetTitle.name"
            },
            {
                title: "Tipo de Fianza",
                data: "bondType.name"
            },
            {
                title: "Entidad Bancaria",
                data: "bank.name"
            },
            {
                title: "Numero de Fianza",
                data: "bondNumber"
            },
            {
                title: "Renovación",
                data: "bondRenovation.name"
            },
            {
                title: "Moneda",
                data: "currencyType"
            },
            {
                title: "Monto",
                data: "penAmmount"
            },
            {
                title: "Plazo",
                data: "daysLimitTerm"
            },
            {
                title: "Fecha de Creación",
                data: "dateInitial"
            },
            {
                title: "Fecha de Vencimiento",
                data: "dateEnd"
            },
            {
                title: "Contra Garantía",
                data: "guaranteeDesc"
            },
            {
                title: "Encargado",
                data: "employeeAsigned"
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
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-add-file">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };



    var genOptionsDetails = function (id) {
        var result = {
            responsive: true,
            ajax: {
                url: _app.parseUrl(`/finanzas/agregarfianza/${id}/archivos/listar`),
                dataSrc: ""
            },
            rowCallback: function (row, data) {
            },
            columns: [
                {
                    title: "Tipo",
                    data: "type",
                    render: function (data) {
                        if (data == 1)
                            return "Tributario";
                        if (data == 2)
                            return "Legal";
                        if (data == 3)
                            return "Comercial";
                        return "---";
                    }
                },
                {
                    title: "Archivo",
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var tmp = "";
                        tmp += `<button data-url="${row.fileUrl}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
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
                        tmp += `<button data-provider-id="${id}" data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-file-delete">`;
                        tmp += `<i class="fa fa-trash"></i></button>`;
                        return tmp;
                    }
                }
            ]
        };

        return result;
    };


    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        initEvents: function () {
            mainDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            mainDatatable.on("click",
                ".btn-add-file",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    providerId = id;
                    $("#add_file_modal").modal("show");
                });

            mainDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La formula será eliminado permanentemente",
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
                                    url: _app.parseUrl(`finanzas/agregarfianza/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La fianza ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la fianza"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        },
        child: {
            init: function (data) {
                var optionsDetail = genOptionsDetails(data.id);
                var $table = $(`<table id="items_${data.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
                detailDatatables[data.id] = $table.DataTable(optionsDetail);
                this.initEvents($table);
                return $table;
            },
            reload: function (id) {
                var dt = detailDatatables[id];
                if (dt) {
                    dt.ajax.reload();
                }
            },
            initEvents: function (table) {
                table.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let url = $btn.data("url");
                    form.load.file.pdf(url);
                });

                table.on("click", ".btn-file-delete", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let providerId = $btn.data("providerId");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El archivo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/finanzas/agregarfianzas/${providerId}/archivos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.child.reload(providerId);
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al archivo"
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

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/agregarfianza/${id}`)
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id']").val(result.id);
                    formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                    formElements.find("[name='BondGuarantorId']").val(result.bondGuarantorId).trigger("change");
                    formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId).trigger("change");

                    formElements.find("[name='BondTypeId']").val(result.bondTypeId).trigger("change");
                    formElements.find("[name='BankId']").val(result.bankId).trigger("change");
                    formElements.find("[name='BondNumber']").val(result.bondNumber);
                    formElements.find("[name='BondRenovationId']").val(result.bondRenovationId).trigger("change");
                    formElements.find("[name='EmployeeAsigned']").val(result.employeeAsigned);
                    formElements.find("[name='currenyType']").val(result.currencyType);

                    formElements.find("[name='PenAmmount']").val(result.penAmmount);
                    //formElements.find("[name='UsdAmmount']").val(result.usdAmmount);
                    formElements.find("[name='daysLimitTerm']").val(result.daysLimitTerm);
                    formElements.find("[name='CreateDate']").val(result.createDate);
                    formElements.find("[name='guaranteeDesc']").val(result.guaranteeDesc);
                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            file: {
                pdf: function (url) {
                    $("#preview_name").text("Empresa");
                    $("#preview_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                    $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${url}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(url)));
                    $(".btn-mailto").data("name", "Empresa").data("url", "https://docs.google.com/gview?url=" + encodeURI(url));
                    $("#preview_modal").modal("show");
                }
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/finanzas/agregarfianza/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.add.success();
                    }).fail(function (error) {
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
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/agregarfianza/editar/${id}`),
                    type: "put",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.edit.success();
                    }).fail(function (error) {
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
                    url: "/logistica/empresas/importar",
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
            file: {
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
                        url: _app.parseUrl(`/logistica/empresas/${providerId}/archivos/crear`),
                        method: "post",
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
                            datatable.child.reload(providerId);
                            $("#add_file_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_file_alert_text").html(error.responseText);
                                $("#add_file_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='IsDirectCost']").prop("checked", true);
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='IsDirectCost']").prop("checked", true);
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            file: {
                add: function () {
                    addFileForm.resetForm();
                    $("#add_file_form").trigger("reset");
                    $("#add_file_form [name='Type']").prop("selectedIndex", 0).trigger("change");
                }
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


            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            addFileForm = $("#add_file_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.file.add(formElement);
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

            $("#add_file_modal").on("hidden.bs.modal",
                function () {
                    form.reset.file.add();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });
        }
    };


    var select2 = {
        init: function () {
            this.answers.init();
            this.status.init();
            this.letters.init();
            this.issuerTargets.init();
            this.interestGroups.init();
            this.employees.init();
            this.budgetType.init();
            this.bondType.init();
            this.projects.init();
            this.banks.init();
            this.renovations.init();
            this.guarantors.init();
            this.currencies.init();
        },
        answers: {
            init: function () {
                $(".select2-answers").select2();
            }
        },
        status: {
            init: function () {
                $(".select2-status").select2();
            }
        },
        letters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas")
                }).done(function (result) {
                    $(".select2-letters").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        issuerTargets: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/entidades-emisoras-receptoras-de-cartas")
                }).done(function (result) {
                    $(".select2-issuer-targets").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        interestGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-interes")
                }).done(function (result) {
                    $(".select2-interest-groups").select2({
                        data: result
                    });
                });
            }
        },
        banks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/bancos")
                }).done(function (result) {
                    $(".select2-banks").select2({
                        data: result,
                        allowClear: true
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
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        employees: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados2")
                }).done(function (result) {
                    $(".select2-employees").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },

        currencies: {
            init: function () {
                $.ajax({
                  //  url: _app.parseUrl("/select/empleados2")
                }).done(function () {
                    $(".select2-currencies").select2({
                        data: [{"id":"PEN","text":"SOLES"},{"id":"USD","text":"DOLARES"}],
                        allowClear: true
                    });
                });
            }
        },
        budgetType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budgetType").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        bondType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-fianza")
                }).done(function (result) {
                    $(".select2-bondType").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        renovations: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/renovaciones")
                }).done(function (result) {
                    $(".select2-renovations").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        guarantors: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/garantes")
                }).done(function (result) {
                    $(".select2-guarantors").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        }

    };

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
        }
    }
}();

$(function () {
    Formula.init();
});