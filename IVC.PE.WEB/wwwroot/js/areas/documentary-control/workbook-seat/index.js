var Workbook = function () {

    var workbookDatatable = null;
    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var importDataForm = null;
    var importFilesForm = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/cuadernos-de-obra/listar"),
            data: function (d) {
                d.wroteBy = $("#workbook_menu_nav li.kt-nav__item--active").data("id");
                d.workbookId = $("#workbook_filter").val();
                d.type = $("#type_filter").val();
                d.status = $("#status_filter").val();
                d.hasFile = $("#has_file").is(":checked");
                d.hasAnswer = $("#has_answer").val();
                d.other = $("#letter_menu_nav li.kt-nav__item--active").data("other");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Número",
                data: "workbook.number"
            },
            {
                title: "Asiento",
                data: "number"
            },
            {
                title: "Asunto",
                data: "subject",
                orderable: false
            },
            {
                title: "Fecha",
                data: "date"
            },
            {
                title: "Tipo",
                data: "workbookType.color",
                render: function (data, type, row) {
                    if (data === 0) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${row.workbookType.description}</span></label></span>`;
                    }
                    if (data === 1) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${row.workbookType.description}</span></label></span>`;
                    }
                    if (data === 2) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${row.workbookType.description}</span></label></span>`;
                    }
                    if (data === 3) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${row.workbookType.description}</span></label></span>`;
                    }
                    if (data === 4) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${row.workbookType.description}</span></label></span>`;
                    }
                    if (data === 5) {
                        return `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${row.workbookType.description}</span></label></span>`;
                    }
                }

            },
            {
                title: "Escribe",
                data: "wroteBy",
                orderable: false,
                render: function (data) {
                    return _app.render.label(data, _app.constants.workbook.wroteBy.VALUES);
                }
            },
            //{
            //    title: "Plazo de Respuesta",
            //    data: "responseTermDays",
            //    orderable: false,
            //    render: function (data) {
            //        return data ? `${data} días` : "---";
            //    }
            //},
            //{
            //    title: "Estado",
            //    data: "status",
            //    orderable: false,
            //    render: function (data) {
            //        return _app.render.ribbon(data, _app.constants.workbook.status.VALUES);
            //    }
            //},
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.fileUrl) {
                        //tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        //tmp += `<i class="fa fa-file-word"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view-pdf">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            workbookDatatable = $("#workbook_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            workbookDatatable.ajax.reload();
        },
        initEvents: function () {

            workbookDatatable.on("click",
                ".btn-details",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    addId = id;
                    
                    form.load.detail(id);
                });

            workbookDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            workbookDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
            workbookDatatable.on("click", ".btn-view-pdf", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf2(id);
            });
            workbookDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El cuaderno de obra enviado será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El cuaderno de obra ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el cuaderno de obra"
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
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Detail']").val(result.detail);
                        formElements.find("[name='Detail']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Number']").val(result.number);
                        formElements.find("[name='Subject']").val(result.subject);
                        formElements.find("[name='Detail']").val(result.detail);
                        formElements.find("[name='Date']").datepicker("setDate", result.date);

                        formElements.find("[name='WorkbookTypeId']").val(result.workbookTypeId);
                        formElements.find("[name='select_type']").val(result.workbookTypeId).trigger("change");

                        formElements.find("[name='Status']").val(result.status).trigger("change");
                        formElements.find("[name='WroteBy']").val(result.wroteBy).trigger("change");
                        formElements.find("[name='WorkbookId']").val(result.workbookId).trigger("change");
                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
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
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.number);
                    //$("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl);
                    $("#pdf_frame").prop("src", "https://view.officeapps.live.com/op/embed.aspx?src=" + result.fileUrl);
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Cuaderno de Obra [${result.name}]: ` + "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdf2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.number);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    //$("#pdf_frame").prop("src", "https://view.officeapps.live.com/op/embed.aspx?src=" + result.fileUrl);
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Cuaderno de Obra [${result.name}]: ` + "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='WorkbookTypeId']").val($(formElement).find("[name='select_type']").val());
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
                    url: _app.parseUrl("/control-documentario/cuadernos-de-obra/crear"),
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
                $(formElement).find("[name='WorkbookTypeId']").val($(formElement).find("[name='select_type']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/editar/${id}`),
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
                        $(formElement).find("input, select").prop("disabled", false);
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
                        url: "/control-documentario/cuadernos-de-obra/importar-datos",
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
                files: function (formElement) {
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
                        url: "/control-documentario/cuadernos-de-obra/importar-archivos",
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
                        $("#import_files_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_files_alert_text").html(error.responseText);
                            $("#import_files_alert").removeClass("d-none").addClass("show");
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
                $("#Add_Type").prop("selectedIndex", 0).trigger("change");
                $("#Add_Status").prop("selectedIndex", 0).trigger("change");
                $("#Add_WroteBy").prop("selectedIndex", 0).trigger("change");
                $("#Add_WorkbookId").val(null).trigger("change");

            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_Type").prop("selectedIndex", 0).trigger("change");
                $("#Edit_Status").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WroteBy").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WorkbookId").val(null).trigger("change");
            },
            import: {
                data: function () {
                    importDataForm.resetForm();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
                files: function () {
                    importFilesForm.resetForm();
                    $("#import_files_form").trigger("reset");
                    $("#import_files_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.types.init();
            this.wroteBys.init();
            this.status.init();
            this.workbooks.init();
        },
        types: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-cuaderno")
                }).done(function (result) {
                    $(".select2-types").select2({
                        data: result
                    });
                });
            }
        },
        wroteBys: {
            init: function () {
                $(".select2-wrotebys").select2();
            }
        },
        status: {
            init: function () {
                $(".select2-status").select2();
            }
        },
        workbooks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cuadernos-de-obra")
                }).done(function (result) {
                    $(".select2-workbooks").select2({
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
            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
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

            importFilesForm = $("#import_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.files(formElement);
                }
            });
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

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.data();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.files();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='Type']").attr("id", "Add_Type");
            $("#edit_form [name='Type']").attr("id", "Edit_Type");
            $("#add_form [name='Status']").attr("id", "Add_Status");
            $("#edit_form [name='Status']").attr("id", "Edit_Status");
            $("#add_form [name='WroteBy']").attr("id", "Add_WroteBy");
            $("#edit_form [name='WroteBy']").attr("id", "Edit_WroteBy");
            $("#add_form [name='WorkbookId']").attr("id", "Add_WorkbookId");
            $("#edit_form [name='WorkbookId']").attr("id", "Edit_WorkbookId");

            $("#type_filter, #workbook_filter, #has_file, #has_answer").on("change", function () {
                datatable.reload();
            });

            $("#add_modal").on("shown.bs.modal", function () {
                //$(".select2-interest-groups").select2();
                //$(".select2-employees").select2();
                //$(".select2-status").select2();
                //$(".select2-letters").select2();
                //$(".select2-interest-groups").select2();
                //$(".select2-issuer-targets").select2();
                form.reset.add();
            });

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });
            $("#workbook_menu_nav li").on("click", function () {
                $('#workbook_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                $(this).addClass('kt-nav__item--active');
                $("#datatable_portlet_title").text(`Listado de Cartas para ${$(this).data("name") || $(this).data("acronym")}`);
                datatable.reload();
            });
            $("#workbook_menu_nav").on("scroll", function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    alert("end reached");
                }
            });

            $(".btn-exportar").on("click", function () {
                window.location = _app.parseUrl(`/control-documentario/cuadernos-de-obra/exportar`);
            });

            this.loadWriters();
        },
        loadWriters: function () {
            _app.loader.showOnElement("#workbook_menu_nav");
            $.ajax({
                url: _app.parseUrl("/control-documentario/cuadernos-de-obra/autores")
            }).done(function (result) {
                $.each(result, function (i, item) {
                    let element = $(`<li class="kt-nav__item col-2" data-id="${item.id}" data-name="${item.name}" data-file-url="${item.fileUrl}">
                        <a href="javascript:;" class="kt-nav__link" style="height: 100%;">
                            <i class="kt-nav__link-icon flaticon2-user"></i>
                            <span class="kt-nav__link-text">${item.name}</span>
                            <span class="kt-nav__link-badge">
                                <span class="kt-badge kt-badge--danger kt-badge--inline kt-badge--pill kt-badge--rounded">${item.totalCount}</span>
                            </span>
                        </a>
                        </li>`);
                    $("#workbook_menu_nav").append(element);
                });
                $("#total_workbooks").text(result.length === 0 ? 0 : result.map(x => x.totalCount).reduce((a, b) => a + b, 0));
                $("#workbook_menu_nav li").on("click", function () {
                    $('#workbook_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                    $(this).addClass('kt-nav__item--active');
                    $("#datatable_portlet_title").text(`Listado de Asientos de Cuaderno de Obra para ${$(this).data("name")}`);
                    datatable.reload();
                });
                _app.loader.hideOnElement("#workbook_menu_nav");
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datepicker.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Workbook.init();
});