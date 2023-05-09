var LettersSent = function () {
    //var _adobeDCView = new AdobeDC.View({ clientId: adobeId, divId: "adobe-dc-view" });
    //var _adobeDCView = null;

    var lettersSentDatatable = null;
    var referencesDatatable = null;
    var addForm = null;
    var editForm = null;
    var importDataForm = null;
    var importFilesForm = null;

    var letterId = null;
    var isFromReference = false;

    var options = {
        responsive: true,
        //serverSide: true,
        //processing: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/cartas-enviadas/listar"),
            data: function (d) {
                d.docCharac = $("#status_filter").val();
                d.ig = $("#interest_group_filter").val();
                d.issuerTargetId = $("#letter_menu_nav li.kt-nav__item--active").data("id");
                //d.interestGroupId = $("#interest_group_filter").val();
                //d.status = $("#status_filter").val();
                //d.hasFile = $("#has_file").is(":checked");
                d.hasAnswer = $("#has_answer").val();
                //d.other = $("#letter_menu_nav li.kt-nav__item--active").data("other");
                delete d.columns;
            },
            dataSrc:""
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Asunto",
                data: "subject",
                orderable: false
            },
            {
                title: "Fecha",
                data: "letterDateStr",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Receptor",
                data: "issuerName",
                orderable: false,
                //render: function (data) {
                //    return data.map((x) => x.name).join(", ");
                //}
            },
            {
                title: "Referencia",
                data: "referenceLetters",
                orderable: false,
                //render: function (data) {
                //    return data.map((x) => x.name).join(", ");
                //}
            },
            {
                title: "Características del Documento",
                data: "docCharacteristics",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        if (data.length != 0) {
                            $.each(data.split('/'), function (index, value) {
                                var docstyle = value.split('-');
                                if (docstyle[1] === '0') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${docstyle[0]}</span></label></span>`;
                                }
                                if (docstyle[1] === '1') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${docstyle[0]}</span></label></span>`;
                                }
                                if (docstyle[1] === '2') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${docstyle[0]}</span></label></span>`;
                                }
                                if (docstyle[1] === '3') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${docstyle[0]}</span></label></span>`;
                                }
                                if (docstyle[1] === '4') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${docstyle[0]}</span></label></span>`;
                                }
                                if (docstyle[1] === '5') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${docstyle[0]}</span></label></span>`;
                                }
                            });
                        }
                    }
                    return tmp;
                }
            },
            //{
            //    title: "Plazo de Respuesta",
            //    data: "responseTermDays",
            //    orderable: false,
            //    render: function (data) {
            //        return data ? data + " días" : "---";
            //    }
            //},
            {
                title: "Áreas de Interés",
                data: "interestGroups",
                orderable: false,
                //render: function (data) {
                //    return data.map((x) => x.name).join(", ");
                //}
            },
            {
                title: "Reponsable Directo",
                data: "responsable",
                orderable: false,
                //render: function (data) {
                //    return data || "---";
                //}
            },
            {
                title: "Respuestas",
                data: "hasAnswers",
                render: function (data) {
                    var tmp = "";
                    if (data > 0) {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${data} Respuesta(s)</span></label></span>`;
                    } else {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">Enviada</span></label></span>`;
                    }
                    return tmp;
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.letterId}" class="btn btn-info btn-sm btn-icon btn-details" data-toggle="tooltip" title="Ver Referencias">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.letterId}" class="btn btn-secondary btn-sm btn-icon btn-view" data-toggle="tooltip" title="Ver Carta">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    if (row.hasAnswers > 0) {
                        tmp += `<button data-id="${row.letterId}" class="btn btn-info btn-sm btn-icon btn-answers" data-toggle="tooltip" title="Ver Respuestas">`;
                        tmp += `<i class="fa fa-ellipsis-v"></i></button> `;
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
                    tmp += `<button data-id="${row.letterId}" class="btn btn-info btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.letterId}" class="btn btn-danger btn-sm btn-icon btn-delete" data-toggle="tooltip" title="Eliminar">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var refOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/control-documentario/cartas-enviadas/listar-referencias`),
            data: function (d) {
                d.letterId = letterId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre",
                data: "name",
            },
            {
                title: "Fecha",
                data: "dateStr"
            },
            {
                title: "Asunto",
                data: "subject"
            },
            {
                title: "Emisor/Receptor",
                data: "issuerName"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            lettersSentDatatable = $("#letter_datatable").DataTable(options);
            this.initEvents();
            this.filesDt.init();
        },
        reload: function () {
            lettersSentDatatable.ajax.reload();
        },
        initEvents: function () {
            lettersSentDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            lettersSentDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                isFromReference = false;
                form.load.pdf(id);
            });

            lettersSentDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La carta enviada será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La carta enviada ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la carta enviada"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            lettersSentDatatable.on("click", ".btn-details", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                datatable.filesDt.reload(id);
                $("#detail_modal").modal("show");
            });

            lettersSentDatatable.on("click", ".btn-answers", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                isFromReference = false;
                select2.letAnswers.reload(id);
                $("#answers_modal").modal("show");
            });
        },
        filesDt: {
            init: function () {
                referencesDatatable = $("#references_datatable").DataTable(refOpt);
                this.events();
            },
            reload: function (id) {
                letterId = id;
                referencesDatatable.clear().draw();
                referencesDatatable.ajax.reload();
            },
            events: function () {
                referencesDatatable.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    isFromReference = true;
                    form.load.pdf(id);
                });
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Subject']").val(result.subject);
                        formElements.find("[name='Date']").datepicker("setDate", result.date);
                        //formElements.find("[name='ResponseTermDays']").val(result.responseTermDays);
                        formElements.find("[name='ReferenceIds']").val(result.referenceIds).trigger("change");
                        formElements.find("[name='Status']").val(result.status).trigger("change");
                        formElements.find("[name='IssuerTargetIds']").val(result.issuerTargetIds).trigger("change");
                        formElements.find("[name='InterestGroupIds']").val(result.interestGroupIds).trigger("change");
                        formElements.find("[name='EmployeeId']").val(result.employeeId).trigger("change");
                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }
                        $("#edit_modal").on("shown.bs.modal", function () {
                            if (!result.referenceIds.length) {
                                $("#Edit_ReferenceIds").select2();
                            }
                        });
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/${id}`)
                }).done(function (result) {
                    //select2.references.reload(id);
                    //$("#pdf_name").text(result.name);
                    //$("#select_letter").val(id);
                    //$("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    //$("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.name}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    //$(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    //if (isFromReference)
                    //    $("#detail_modal").modal("hide");
                    //$("#pdf_modal").modal("show");

                    select2.references.reload(id);
                    $("#select_letter").val(id);
                    $("#pdf_name").text(result.name);

                    if (isFromReference)
                        $("#detail_modal").modal("hide");

                    loadPdf(result.name, result.fileUrl, "pdf_views");

                    //_adobeDCView.previewFile({
                    //    content: { location: { url: result.fileUrl } },
                    //    metaData: { fileName: result.name }
                    //}, {});

                    //var adobeDCView = new AdobeDC.View({ clientId: adobeId, divId: "adobe-dc-view" });
                    //adobeDCView.previewFile({
                    //    content: { location: { url: result.fileUrl } },
                    //    metaData: { fileName: result.name }
                    //}, {});
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
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
                    url: _app.parseUrl("/control-documentario/cartas-enviadas/crear"),
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
                        events.loadTargets();
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
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/editar/${id}`),
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
                        datatable.reload();
                        events.loadTargets();
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
                        url: "/control-documentario/cartas-enviadas/importar-datos",
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
                        events.loadTargets();
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
                        url: "/control-documentario/cartas-enviadas/importar-archivos",
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
                $("#Add_ReferenceIds").val(null).trigger("change");
                $("#Add_Status").val(null).trigger("change");
                $("#Add_IssuerTargetIds").val(null).trigger("change");
                $("#Add_InterestGroupIds").val(null).trigger("change");
                $("#Add_EmployeeId").prop("selectedIndex", 0).trigger("change");
                //$("#Add_File").fileinput("clear");  
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_ReferenceIds").val(null).trigger("change");
                $("#Edit_Status").val(null).trigger("change");
                $("#Edit_IssuerTargetIds").val(null).trigger("change");
                $("#Edit_InterestGroupIds").val(null).trigger("change");
                $("#Edit_EmployeeId").prop("selectedIndex", 0).trigger("change");
                //$("#Edit_File").fileinput("clear");
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
            this.answers.init();
            this.status.init();
            this.letters.init();
            this.issuerTargets.init();
            this.interestGroups.init();
            this.employees.init();
            this.docCharac.init();
            this.references.init();
            this.letAnswers.init();
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
                        data: result
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
        employees: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-employees").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        docCharac: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas-caracteristicas-doc")
                }).done(function (result) {
                    $(".select2-status").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        references: {
            init: function () {
                $(".select2-references").select2();
            },
            reload: function (id) {
                $(".select2-references").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/cartas-referencias?id=${id}`)
                }).done(function (result) {
                    $(".select2-references").select2({
                        data: result
                    });
                });
            }
        },
        letAnswers: {
            init: function () {
                $(".select2-letanswers").select2();
            },
            reload: function (id) {
                $(".select2-letanswers").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/cartas-respondidas?id=${id}`)
                }).done(function (result) {
                    $(".select2-letanswers").select2({
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

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    if (isFromReference)
                        $("#detail_modal").modal("show");
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ReferenceIds']").attr("id", "Add_ReferenceIds");
            $("#edit_form [name='ReferenceIds']").attr("id", "Edit_ReferenceIds");
            $("#add_form [name='Status']").attr("id", "Add_Status");
            $("#edit_form [name='Status']").attr("id", "Edit_Status");
            $("#add_form [name='IssuerTargetIds']").attr("id", "Add_IssuerTargetIds");
            $("#edit_form [name='IssuerTargetIds']").attr("id", "Edit_IssuerTargetIds");
            $("#add_form [name='InterestGroupIds']").attr("id", "Add_InterestGroupIds");
            $("#edit_form [name='InterestGroupIds']").attr("id", "Edit_InterestGroupIds");
            $("#add_form [name='EmployeeId']").attr("id", "Add_EmployeeId");
            $("#edit_form [name='EmployeeId']").attr("id", "Edit_EmployeeId");
            $("#add_form [name='File']").attr("id", "Add_File");
            $("#edit_form [name='File']").attr("id", "Edit_File");

            $("#status_filter, #interest_group_filter, #has_file, #has_answer").on("change", function () {
                datatable.reload();
            });

            /* has_file
            $("#status_filter, #interest_group_filter, #has_file, #has_answer").on("change", function () {
                datatable.reload();
            });
            */
            $("#add_modal").on("shown.bs.modal", function () {
                //$(".select2-interest-groups").select2();
                //$(".select2-employees").select2();
                //$(".select2-status").select2();
                //$(".select2-letters").select2();
                //$(".select2-interest-groups").select2();
                //$(".select2-issuer-targets").select2();
                form.reset.add();
            });

            this.loadTargets();

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });
            $("#letter_menu_nav").on("scroll", function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    alert("end reached");
                }
            });

            $("#btn_LoadPdf").on("click", function () {
                var id = $("#select_letter").val();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-recibidas/${id}`)
                }).done(function (result) {
                    loadPdf(result.name, result.fileUrl,"pdf_views");
                })
            });

            $("#btn_LoadRefPdf").on("click", function () {
                var id = $("#select_reference").val();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-recibidas/${id}`)
                }).done(function (result) {
                    loadPdf(result.name, result.fileUrl,"pdf_views");
                })
            });

            $("#btn_LoadAnsPdf").on("click", function () {
                var id = $("#select_letanswer").val();
                console.log(id);
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-recibidas/${id}`)
                }).done(function (result) {
                    console.log(result);
                    loadPdf(result.name, result.fileUrl, "answers_pdf");
                    //$("#ans_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                })
            });

            $(".btn-exportar").on("click", function () {
                window.location = _app.parseUrl(`/control-documentario/cartas-enviadas/exportar`);
            });
        },
        loadTargets: function () {
            _app.loader.showOnElement("#letter_menu_nav");
            $.ajax({
                url: _app.parseUrl("/control-documentario/cartas-enviadas/receptores")
            }).done(function (result) {
                $.each(result, function (i, item) {
                    let element = $(`<li class="kt-nav__item col-2" data-id="${item.issuerTargetId}" data-name="${item.name}" data-acronym="${item.acronym}" data-other="${item.other}">
                        <a href="javascript:;" class="kt-nav__link" style="height: 100%;">
                            <i class="kt-nav__link-icon flaticon2-user"></i>
                            <span class="kt-nav__link-text">${(item.acronym || item.name)}</span>
                            <span class="kt-nav__link-badge">
                                <span class="kt-badge kt-badge--danger kt-badge--inline kt-badge--pill kt-badge--rounded">${item.totalCount}</span>
                            </span>
                        </a>
                        </li>`);
                    $("#letter_menu_nav").append(element);
                });
                $("#total_letters").text(result.length === 0 ? 0 : result.map(x => x.totalCount).reduce((a, b) => a + b, 0));
                $("#letter_menu_nav li").on("click", function () {
                    $('#letter_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                    $(this).addClass('kt-nav__item--active');
                    $("#datatable_portlet_title").text(`Listado de Cartas para ${$(this).data("name") || $(this).data("acronym")}`);
                    datatable.reload();
                });
                _app.loader.hideOnElement("#letter_menu_nav");
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
    LettersSent.init();
});