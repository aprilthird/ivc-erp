var ProjectPhase = function () {

    var projectPhaseDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;

    var importDataForm = null;

    var options = {
        responsive: true,
        //serverSide: true,
        //processing: true,
        ajax: {
            url: _app.parseUrl("/admin/fases/listar"),
            dataSrc: "",
            data: function (d) {
                d.formulaId = $("#formula_filter").val();
                //d.entryPositionId = $("#entry_position_filter").val();
                //d.currentPositionId = $("#current_position_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Formula",
                data: "formula"
            },
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Descripción Completa",
                data: "fullDescription"
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
        ]
    };

    var datatable = {
        init: function () {
            projectPhaseDatatable = $("#project_phase_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            projectPhaseDatatable.ajax.reload();
        },
        initEvents: function () {
            projectPhaseDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            projectPhaseDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La fase será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/admin/fases/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La fase ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la fase"
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
                    url: _app.parseUrl(`/admin/fases/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            import: {
                data: function (formElement) {
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
                        url: "/admin/fases/importar",
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
                        $("#import_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_data_alert_text").html(error.responseText);
                            $("#import_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },

            },
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/fases/crear"),
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
                    url: _app.parseUrl(`/admin/fases/editar/${id}`),
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
            //import: function (formElement) {
            //    let data = new FormData($(formElement).get(0));
            //    let $btn = $(formElement).find("button[type='submit']");
            //    $btn.addLoader();
            //    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
            //    $(formElement).find("input").prop("disabled", true);
            //    if (!emptyFile) {
            //        $(formElement).find(".custom-file").append(
            //            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
            //        $(".progress-bar").width("0%");
            //    }
            //    $.ajax({
            //        url: "/admin/fases/importar",
            //        type: "POST",
            //        contentType: false,
            //        processData: false,
            //        data: data,
            //        xhr: function () {
            //            var xhr = new window.XMLHttpRequest();
            //            if (!emptyFile) {
            //                xhr.upload.onprogress = function (evt) {
            //                    if (evt.lengthComputable) {
            //                        var percentComplete = evt.loaded / evt.total;
            //                        percentComplete = parseInt(percentComplete * 100);
            //                        $(".progress-bar").width(percentComplete + "%");
            //                    }
            //                };
            //            }
            //            return xhr;
            //        }
            //    }).always(function () {
            //        $btn.removeLoader();
            //        $(formElement).find("input").prop("disabled", false);
            //        $(".progress").empty().remove();
            //        if (!emptyFile) {
            //            $("#space-bar").remove();
            //        }
            //    }).done(function (result) {
            //        datatable.reload();
            //        $("#import_modal").modal("hide");
            //        _app.show.notification.addRange.success();
            //    }).fail(function (error) {
            //        if (error.responseText) {
            //            $("#import_alert_text").html(error.responseText);
            //            $("#import_alert").removeClass("d-none").addClass("show");
            //        }
            //        _app.show.notification.addRange.error();
            //    });
            //}
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
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.formulas.init();
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

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.data(formElement);
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

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.data();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ProjectFormulaId']").attr("id", "Add_ProjectFormulaId");
            $("#edit_form [name='ProjectFormulaId']").attr("id", "Edit_ProjectFormulaId");
            $("#formula_filter").on("change", function () {
                datatable.reload();
            });

            $("#btn-massive-load").on("click", function () {
                window.location = _app.parseUrl(`/admin/fases/importar-formato`);
            });

            $(".btn-exportar").on("click", function () {
                window.location = _app.parseUrl(`/admin/fases/exportar`);
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    ProjectPhase.init();
});