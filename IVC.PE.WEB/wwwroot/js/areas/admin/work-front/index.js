var WorkFront = function () {

    var workFrontId = null;
    var formulaId = null;
    var tmpIds = null;

    var workFrontDatatable = null;
    var phaseDatatable = null;
    var sewergroupDatatable = null;

    var addForm = null;
    var editForm = null;
    var importForm = null;
    var frontImportForm = null;
    var phaseImportForm = null;
    var sewerGroupImportForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/frentes/listar"),
            dataSrc: "",
            data: function (d) {
                d.formulaId = $("#formula_filter_main").val();
                delete d.columns;
            } 
        },
        buttons: [],
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Fórmulas",
                data: "formulaCodes"
            },
            {
                title: "Carpeta",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-phases" data-toggle="tooltip" title="Fases">`;
                    tmp += `<i class="fa fa-list-alt"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-sewergroups" data-toggle="tooltip" title="Cuadrillas">`;
                    tmp += `<i class="fa fa-hard-hat"></i></button> `;
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
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };
    var phaseDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/frentes/fases/listar"),
            data: function (d) {
                d.workFrontId = workFrontId;
                d.formulaId = $("#formula_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Fórmula",
                data: "formulaCode"
            },
            {
                title: "Cod. Fase",
                data: "projectPhase.code"
            },
            {
                title: "Desc. Fase",
                data: "projectPhase.description"
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
            //{
            //    text: "<i class='fa fa-plus'></i> Importar",
            //    className: "btn-primary",
            //    action: function (e, dt, node, config) {
            //        $("#phase_modal").modal("hide");
            //        form.load.phase.import();
            //    }
            //}
        ]
    };
    var sewergroupDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/frentes/cuadrillas/listar"),
            data: function (d) {
                d.wfId = workFrontId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Cuadrilla",
                data: "sewerGroupCode"
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
                text: "<i class='fa fa-plus'></i> Importar",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#phase_modal").modal("hide");
                    form.load.sewergroup.import();
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            workFrontDatatable = $("#work_front_datatable").DataTable(options);
            this.phaseDt.init();
            this.sewergroupDt.init();
            this.initEvents();
        },
        reload: function () {
            workFrontDatatable.ajax.reload();
        },
        initEvents: function () {
            workFrontDatatable.on("click",
                ".btn-phases",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    workFrontId = id;
                    datatable.phaseDt.reload();
                    form.load.phase.detail();
                });

            workFrontDatatable.on("click",
                ".btn-sewergroups",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    workFrontId = id;
                    datatable.sewergroupDt.reload();
                    form.load.sewergroup.detail();
                });

            workFrontDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            workFrontDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El frente será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/admin/frentes/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El frente ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el frente"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        },
        phaseDt: {
            init: function () {
                phaseDatatable = $("#phases_datatable").DataTable(phaseDtOpts);
            },
            reload: function () {
                phaseDatatable.clear().draw();
                phaseDatatable.ajax.reload();
            }
        },
        sewergroupDt: {
            init: function () {
                sewergroupDatatable = $("#sewergroups_datatable").DataTable(sewergroupDtOpts);
            },
            reload: function () {
                sewergroupDatatable.clear().draw();
                sewergroupDatatable.ajax.reload();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/frentes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='FormulaIds']").val(result.formulaIds).trigger("change");
                        select2.phases.reload(result.formulaIds.toString(), result.projectPhaseIds);
                        tmpIds = result.projectPhaseIds;
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            front: {
                detail: function () {
                    $("#front_modal").modal("show");
                },
                import: function () {
                    $("#front_modal").modal("hide");
                    $("#front_import_modal").modal("show");
                }
            },
            phase: {
                detail: function () {
                    $("#phase_modal").modal("show");
                },
                import: function () {
                    $("#phase_modal").modal("hide");
                    $("#phase_import_modal").modal("show");
                }
            },
            sewergroup: {
                detail: function () {
                    $("#sewergroup_modal").modal("show");
                },
                import: function () {
                    $("#sewergroup_modal").modal("hide");
                    $("#sewergroup_import_modal").modal("show");
                }
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/frentes/crear"),
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
                    url: _app.parseUrl(`/admin/frentes/editar/${id}`),
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
                    url: "/admin/frentes/importar",
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
            front: {
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
                        url: "/admin/frentes/importar-solo-frentes",
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
                        //datatable.phaseDt.reload();
                        datatable.reload();
                        $("#front_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#front_import_alert_text").html(error.responseText);
                            $("#front_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            },
            phase: {
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
                        url: "/admin/frentes/fases/importar",
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
                        //datatable.phaseDt.reload();
                        datatable.reload();
                        $("#phase_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#phase_import_alert_text").html(error.responseText);
                            $("#phase_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            },
            sewergroup: {
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
                        url: "/admin/frentes/cuadrillas/importar",
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
                        datatable.sewergroupDt.reload();
                        $("#sewergroup_import_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#sewergroup_import_alert_text").html(error.responseText);
                            $("#sewergroup_import_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='ProjectId']").val(null).trigger("change");
                $(".select2-project-phases").val(null).trigger("change");
                $(".select2-formulas").val(null).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='ProjectId']").val(null).trigger("change");
                $(".select2-project-phases").val(null).trigger("change");
                $(".select2-formulas").val(null).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            front: {
                import: function () {
                    frontImportForm.resetForm();
                    $("#front_import_form").trigger("reset");
                    $("#front_import_alert").removeClass("show").addClass("d-none");
                }
            },
            phase: {
                import: function () {
                    phaseImportForm.resetForm();
                    $("#phase_import_form").trigger("reset");
                    $("#phase_import_alert").removeClass("show").addClass("d-none");
                }
            },
            sewergroup: {
                import: function () {
                    sewerGroupImportForm.resetForm();
                    $("#sewergroup_modal").modal("show");
                    $("#sewergroup_import_form").trigger("reset");
                    $("#sewergroup_import_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.projects.init();
            this.phases.init();
            this.formulas.init();
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
        phases: {
            init: function () {
                $(".select2-project-phases").select2();
            },
            reload: function (fIds, pIds) {
                $.ajax({
                    url: _app.parseUrl(`/select/fases-proyecto-formula?fIds=${fIds}`)
                }).done(function (result) {
                    $(".select2-project-phases").empty();
                    $(".select2-project-phases").select2({
                        data: result
                    }).val(pIds).trigger("change");
                    //if (tmpPIds != null) {
                    //    console.log(result);
                    //    console.log(tmpPIds);
                    //    $("#Edit_ProjectPhaseIds").val(tmpPIds).trigger('change');;
                        
                    //}
                });
            }
        },
        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                }).done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
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

            frontImportForm = $("#front_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.front.import(formElement);
                }
            });

            phaseImportForm = $("#phase_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.phase.import(formElement);
                }
            });

            sewerGroupImportForm = $("#sewergroup_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.sewergroup.import(formElement);
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

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });

            $("#front_import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.front.import();
                });


            $("#phase_import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.phase.import();
                });

            $("#sewergroup_import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.sewergroup.import();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");
            $("#add_form [name='FormulaIds']").attr("id", "Add_FormulaIds");
            $("#edit_form [name='FormulaIds']").attr("id", "Edit_FormulaIds");
            $("#add_form [name='ProjectPhaseIds']").attr("id", "Add_ProjectPhaseIds");
            $("#edit_form [name='ProjectPhaseIds']").attr("id", "Edit_ProjectPhaseIds");

            $("#formula_filter").on("change", function () {
                datatable.phaseDt.reload();
            });

            $("#formula_filter_main").on("change", function () {
                datatable.reload();
            });

            $("#dwnExcelFormatFront").on("click", function () {
                window.location = _app.parseUrl(`/admin/frentes/importar-formato`);
            });

            $("#dwnExcelFormat").on("click", function () {
                window.location = _app.parseUrl(`/admin/frentes/fases/importar-formato`);
            });

            $("#dwnWfSgExcelFormat").on("click", function () {
                window.location = _app.parseUrl(`/admin/frentes/cuadrillas/importar-formato`);
            });

            $(".btn-exportar").on("click", function () {
                window.location = _app.parseUrl(`/admin/frentes/exportar`);
            });

            //$(".select2-formulas").on('select2:select', function (e) {
            //    console.log(e.params.data);
            //    fIds.push(e.params.data.id);
            //    console.log(fIds.toString());
            //    //select2.phases.reload(this.value.toString(), null);
            //});

            $("#Add_FormulaIds").on("change", function () {
                let fIds = $("#Add_FormulaIds").select2('data').map(x => x.id);
                select2.phases.reload(fIds.toString(), null);
            });

            $("#Edit_FormulaIds").on("change", function () {
                let fIds = $("#Edit_FormulaIds").select2('data').map(x => x.id);
                select2.phases.reload(fIds.toString(), tmpIds);
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            select2.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    WorkFront.init();
});