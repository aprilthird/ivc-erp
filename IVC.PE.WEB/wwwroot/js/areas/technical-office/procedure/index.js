var IsoStandard = function () {
    var procId = null;
    var isoStandardDatatable = null;
    var addForm = null;
    var editForm = null;
    var fileForm = null;
    var sgOption = new Option('--Seleccione una Formula--', null, true, true);
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/procedimiento/listar"),
            data: function (d) {
                
                d.processId = $("#process_filter").val();
                d.documentTypeId = $("#document_filter").val();
               
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [

            {
                title: "Proceso",
                data: "processes"
            },
            {
                title: "Tipo de Documento",
                data: "documentType"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Procedimiento",
                data: "fileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-qr">`;
                        tmp += `<i class="fa fa-qrcode"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-viewfiles" title="Ver Archivos">`;
                        tmp += `<i class="fa fa-paperclip"></i></button> `;
                    }
                    if (row.fileUrl2 != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-download">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
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
                    if (data != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-addfiles" title="Adjuntar Archivos">`;
                        tmp += `<i class="la la-upload"></i></button> `;
                    }
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
            isoStandardDatatable = $("#iso_standard_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            isoStandardDatatable.ajax.reload();
        },
        initEvents: function () {
            isoStandardDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            isoStandardDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });

            isoStandardDatatable.on("click",
                ".btn-qr",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    window.location = `/oficina-tecnica/procedimiento/qr/${id}`;
                });

            isoStandardDatatable.on("click", ".btn-download", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                window.location = `/oficina-tecnica/procedimiento/descargar/${id}`;
            });
            isoStandardDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Procedimiento será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Procedimiento sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el Procedimiento"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
            isoStandardDatatable.on("click",
                ".btn-addfiles",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    procId = id;
                    $("#add_file_form  [for='File']").text("Selecciona un archivo");
                    $("#add_file_modal").modal("show");
                });
            isoStandardDatatable.on("click",
                ".btn-viewfiles",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    requestId = id;
                    form.load.file(id);
                });
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        
                        formElements.find("[name='DocumentTypeId']").val(result.documentTypeId);
                        formElements.find("[name='select_document']").val(result.documentTypeId).trigger("change");
                        formElements.find("[name='Processes']").val(result.processes);
                        formElements.find("[name='select_processes']").val(result.processes).trigger("change");
                        $(".select2-processes").trigger('change');
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Code']").val(result.code);

                        
                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        if (result.fileUrl2) {
                            $("#edit_form [for='customFile2']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile2']").text("Selecciona un archivo");
                        }
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.title}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            file: function (id) {
                select2.files.reload(id);
                $("#file_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='DocumentTypeId']").val($(formElement).find("[name='select_document']").val());
                $(formElement).find("[name='Processes']").append($(formElement).find("[name='select_processes']").val());
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
                    url: _app.parseUrl("/oficina-tecnica/procedimiento/crear"),
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
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
                $(formElement).find("[name='DocumentTypeId']").val($(formElement).find("[name='select_document']").val());
                $(formElement).find("[name='Processes']").append($(formElement).find("[name='select_processes']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/editar/${id}`),
                    method: "put",
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
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
                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/adjuntar-archivo/${procId}`),
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#add_file_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#file_alert_text").html(error.responseText);
                            $("#file_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#Add_Processes").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_Processes").prop("selectedIndex", 0).trigger("change");
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

            fileForm = $("#add_file_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.file(formElement);
                }
            })
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
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
        }
    };
    var select2 = {
        init: function () {

            this.documents.init();
            this.processes.init();

        },
        documents: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({

                    url: _app.parseUrl(`/select/tipos-de-documento?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-documents").select2({
                        data: result
                    });
                });
            }
        },
        processes: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/procesos?projectId=${pId}`)
                }).done(function (result) {

                    $(".select2-processes").select2({
                        data: result
                    });
                });
            }
        },
        files: {
            init: function () {
                $(".select2-files").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/procedimiento/archivos/${id}`)
                }).done(function (result) {
                    $(".select2-files").empty();
                    $(".select2-files").select2({
                        data: result
                    });
                });
            },
        }
    };

    var events = {
        init: function () {



            $("#process_filter,#document_filter").on("change", function () {
                datatable.reload();
               
            });

            $("#add_form [name='Processes']").attr("id", "Add_Processes");
            $("#edit_form [name='Processes']").attr("id", "Edit_Processes");
            
            $("#btn_LoadFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri.includes("pdf")) {
                    $("#gdocs_frame").attr("hidden", true);
                    $("#files_pdf").removeAttr("hidden");
                    loadPdf("Archivo", uri, "files_pdf");
                } else {
                    $("#files_pdf").attr("hidden", true);
                    $("#gdocs_frame").removeAttr("hidden");
                    let documentUrl = "";
                    documentUrl = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(uri));
                    $("#gdocs_frame").prop("src", documentUrl + "&embedded=true");
                }
            });

            $("#btn_downloadFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri != null) {
                    window.location = `${uri}`;
                }
            });

            $("#btn_qr").on("click", function () {
                var uri = $("#select_files").val();
                if (uri != null) {
                    //$.ajax({
                    //    url: _app.parseUrl(`/oficina-tecnica/procedimiento/qr/archivo?url=${uri}`),
                    //    type: "get"

                    //});
                    window.location = `/oficina-tecnica/procedimiento/qr/archivo?url=${uri}`;
                }
                
            });
            
            $("#btn_deleteFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri != null) {
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Archivo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/procedimiento/archivo/eliminar?url=${uri}`),
                                    type: "delete",
                                    success: function (result) {
                                        select2.files.reload(requestId);
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Archivo ha sido eliminado con éxito",
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
                }

            });
           

           
        },

        

    };
    return {
        init: function () {
            select2.init();
            datatable.init();
            datepicker.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    IsoStandard.init();
});