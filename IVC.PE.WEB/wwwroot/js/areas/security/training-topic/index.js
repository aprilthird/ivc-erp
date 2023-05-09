var TrainingTopic = function () {

    var topicDatatable = null;
    var topicId = null;

    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/capacitaciones/temas/listar"),
            dataSrc: "",
            data: function (d) {
                d.trainingCategoryId = $("#training_category_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Categoría",
                data: "trainingCategory.name"
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
                    tmp += `<button data-id="${row.id}" data-name="${row.name}" class="btn btn-primary btn-sm btn-icon btn-files">`;
                    tmp += `<i class="fa fa-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" data-name="${row.name}" type="button" class="btn btn-info btn-sm btn-icon btn-preview">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;

                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var fileOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/capacitaciones/temas/archivos/listar"),
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Archivo",
                data: "text",
                render: function (data) {
                    return data.split('/').pop();
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" type="button" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };


    var datatable = {
        init: function () {
            this.topics.init();
        },
        topics: {
            init: function () {
                topicDatatable = $("#topics_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                topicDatatable.ajax.reload();
            },
            initEvents: function () {
                topicDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });
                topicDatatable.on("click",
                    ".btn-files",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        form.load.files(id, name);
                    });
                topicDatatable.on("click",
                    ".btn-preview",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        form.load.preview(id, name);
                    });
                topicDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tema será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/seguridad/capacitaciones/temas/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tema ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tema"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        files: {
            init: function (id) {
                fileOpts.ajax.url = _app.parseUrl(`/seguridad/capacitaciones/temas/${id}/archivos`);
                filesDatatable = $("#files_datatable").DataTable(fileOpts);
                this.events();
            },
            reload: function () {
                filesDatatable.ajax.reload();
            },
            destroy: function () {
                filesDatatable.destroy();
            },
            events: function () {
                filesDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El archivo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/seguridad/capacitaciones/temas/${topicId}/archivos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.files.reload();
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
                                                text: "Ocurrió un error al intentar eliminar el archivo"
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
                    url: _app.parseUrl(`/seguridad/capacitaciones/temas/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='TrainingCategoryId']").val(result.trainingCategoryId).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            files: function (id, name) {
                topicId = id;
                datatable.files.init(id);
                $("#file_modal_title").html(`Archivos para tema ${name}`);
                $("#files_modal").modal("show");
            },
            preview: function (id, name) {
                $("#preview_modal_title").text(`Ver archivos para tema ${name}`);
                select2.files.init(id);
                $("#preview_modal").modal("show");  
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/seguridad/capacitaciones/temas/crear"),
                    method: "post",
                    data: data,
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
                    url: _app.parseUrl(`/seguridad/capacitaciones/temas/editar/${id}`),
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
            file: function (formElement) {
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
                    url: _app.parseUrl(`/seguridad/capacitaciones/temas/${topicId}/archivos`),
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatable.files.reload();
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#files_alert_text").html(error.responseText);
                            $("#files_alert").removeClass("d-none").addClass("show");
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
            files: function () {
                filesForm.resetForm();
                $("#files_form").trigger("reset");
                $("#files_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.categories.init();
        },
        categories: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/categorias-de-capacitaciones")
                }).done(function (result) {
                    $(".select2-training-categories").select2({
                        data: result
                    });
                });
            }
        },
        files: {
            init: function (id) {
                $.ajax({
                    url: _app.parseUrl("/select/archivos-de-capacitaciones?id=" + id)
                }).done(function (result) {
                    $(".select2-files").select2({
                        data: result
                    }).on("change", function () {
                        let documentUrl = $(this).val();
                        if (document) {
                            if (documentUrl.includes("pdf")) {
                                let url = "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(documentUrl));
                                $("#preview_frame").prop("src", url + "&embedded=true");
                                $("#preview_frame").removeClass("d-none");
                                $("#image_preview").addClass("d-none");
                                $("#video_preview").addClass("d-none");
                                //loadPdf(title, fileUrl, "pdf_views");
                                //$("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${title}]: ` + documentUrl);
                                //$(".btn-mailto").data("name", title).data("url", documentUrl);
                                //$("#adobe_modal").modal("show");
                            }
                            else if (documentUrl.includes("mp4")) {
                                $("#video_src").prop("src", documentUrl);
                                $("#video_preview").removeClass("d-none");
                                $("#image_preview").addClass("d-none");
                                $("#preview_frame").addClass("d-none");
                            }
                            else if (documentUrl.includes("jpg") || documentUrl.includes("png")) {
                                $("#image_preview").prop("src", documentUrl);
                                $("#image_preview").removeClass("d-none");
                                $("#video_preview").addClass("d-none");
                                $("#preview_frame").addClass("d-none");
                            }
                            else {
                                let url = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(documentUrl));
                                $("#preview_frame").prop("src", url + "&embedded=true");
                                $("#preview_frame").removeClass("d-none");
                                $("#image_preview").addClass("d-none");
                                $("#video_preview").addClass("d-none");

                                //$("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${title}]: ` + documentUrl);
                                //$(".btn-mailto").data("name", title).data("url", documentUrl);
                                //$("#preview_modal").modal("show");
                            }
                        }
                    }).trigger("change");
                });
            },
            empty: function () {
                $(".select2-files").empty();
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

            filesForm = $("#file_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.file(formElement);
                }
            })
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

            $("#files_modal").on("hidden.bs.modal",
                function () {
                    form.reset.files();
                    datatable.files.destroy();
                });

            $("#preview_modal").on("hidden.bs.modal",
                function () {
                    select2.files.empty()
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='TrainingCategoryId']").attr("id", "Add_TrainingCategoryId");
            $("#edit_form [name='TrainingCategoryId']").attr("id", "Edit_TrainingCategoryId");

            $("#training_category_filter").on("change", function () {
                datatable.reload();
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
    TrainingTopic.init();
});