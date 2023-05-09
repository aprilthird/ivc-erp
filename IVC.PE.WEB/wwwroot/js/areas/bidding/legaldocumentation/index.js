var Provider = function () {

    var legalDocumentationsDataTable = null;
    var legalDocsRensDataTable = null;
    var addForm = null;
    var editForm = null;    
    var addRenForm = null;
    var editRenForm = null;

    var isFromDetail = false;

    var docId = null;
    var renId = null;

    var legalDocsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/documentos-legal/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Codigo",
                data: "businessCode"
            },
            {
                title: "Razon Social",
                data: "businessName"
            },
            {
                title: "RUC",
                data: "businessRuc"
            },
            {
                title: "Tipo de Documento",
                data: "legalDocumentationType",
            },
            {
                title: "Días para Vencimiento",
                data: "validity",
                render: function (data, type, row) {
                    if (row.isTheLast) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">Amortizada</span>
								</label>
							</span>`;
                    }
                    if (data > 5) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${data} días</span>
								</label>
							</span>`;
                    } else if (data > 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${data} días</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${data} días</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Inicio Vigencia",
                data: "createDateString"
            },
            {
                title: "Fin Vigencia",
                data: "endDateString"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.legalDocumentationId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.legalDocumentationRenovationId}" class="btn btn-success btn-sm btn-icon btn-renovation">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.legalDocumentationRenovationId}" data-legaldocumentationid="${row.legalDocumentationId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.legalDocumentationId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.legalDocumentationId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            //{
            //    text: "<i class='fa fa-briefcase'></i> Reporte Excel Por Centro de Costos",
            //    className: "btn-primary",
            //    action: function (e, dt, node, config) {
            //        _app.loader.show();
            //        $.ajax({
            //            url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-proyecto`)
            //        }).always(function () {
            //            _app.loader.hide();
            //        }).done(function (result) {
            //            window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
            //        });
            //    }
            //},
            //{
            //    text: "<i class='fa fa-piggy-bank'></i> Reporte Excel Por Bancos",
            //    className: "btn-success",
            //    action: function (e, dt, node, config) {
            //        _app.loader.show();
            //        $.ajax({
            //            url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-banco`)
            //        }).always(function () {
            //            _app.loader.hide();
            //        }).done(function (result) {
            //            window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
            //        });
            //    }
            //},
            //{
            //    text: "<i class='fa fa-book'></i> Reporte Excel Histórico",
            //    className: "btn-info",
            //    action: function (e, dt, node, config) {
            //        _app.loader.show();
            //        $.ajax({
            //            url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-historico`)
            //        }).always(function () {
            //            _app.loader.hide();
            //        }).done(function (result) {
            //            window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
            //        });
            //    }
            //}
        ]
    };

    var legalDocsRensDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/documentos-legal/renovaciones/listar"),
            data: function (d) {
                d.legalDocumentationId = docId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Inicio Vigencia",
                data: "createDate"
            },
            {
                title: "Fin Vigencia",
                data: "endDate"
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
                    tmp += `<button data-id="${row.id}" data-legaldocumentationid="${row.legalDocumentationId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var dataTables = {
        init: function () {
            this.legalDocsDt.init();
            this.legalDocsRensDt.init();
        },
        legalDocsDt: {
            init: function () {
                legalDocumentationsDataTable = $("#legaldocumentations_datatable").DataTable(legalDocsDt_options);
                this.events();
            },
            reload: function () {
                legalDocumentationsDataTable.ajax.reload();
            },
            events: function () {
                legalDocumentationsDataTable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        docId = id;
                        dataTables.legalDocsRensDt.reload();
                        forms.load.detail(id);
                    });

                legalDocumentationsDataTable.on("click",
                    ".btn-renovation",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        renId = id;
                        forms.load.addren(id);
                    });

                legalDocumentationsDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let docid = $btn.data("legaldocumentationid");
                        let renid = $btn.data("id");
                        isFromDetail = false;
                        select2.legalDocumentationRenovations.reload(docid, renid);
                    });

               legalDocumentationsDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                legalDocumentationsDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La documentacion legal y sus renovaciones serán eliminadas permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/licitaciones/documentos-legal/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.legalDocsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La documentacion legal y sus renovaciones han sido eliminadas con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la carta fianza."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        legalDocsRensDt: {
            init: function () {
                legalDocsRensDataTable = $("#legaldocsren_datatable").DataTable(legalDocsRensDt_options);
                this.events();
            },
            reload: function () {
                legalDocsRensDataTable.clear().draw();
                legalDocsRensDataTable.ajax.reload();
            },
            events: function () {
                legalDocsRensDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editren(id);
                    });

                legalDocsRensDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La renovación será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.legalDocsDt.reload();
                                            dataTables.legalDocsRensDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La renovación ha sido eliminada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            console.log(errormessage);
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurrió un error al intentar eliminar la renovación. Motivo: ${errormessage.responseText}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                legalDocsRensDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let docid = $btn.data("legaldocumentationid");
                        let renid = $btn.data("id");
                        isFromDetail = true;
                        select2.legalDocumentationRenovations.reload(docid, renid);
                        $("#detail_modal").modal("hide");
                    });
            }
        },
    };

    var forms = {
        load: {
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/documentos-legal/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='select_business']").val(result.businessId).trigger("change");
                        formElements.find("[name='select_business']").attr("disabled", "disabled");
                        formElements.find("[name='LegaldocumentationTypeId']").val(result.legalDocumentationTypeId);
                        formElements.find("[name='select_legaldoctype']").val(result.legalDocumentationTypeId).trigger("change");
                        formElements.find("[name='select_legaldoctype']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });                
            },
            editren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_ren_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='LegalDocumentationId']").val(result.legalDocumentationId);
                        formElements.find("[name='LegalDocumentationTypeId']").val(result.legalDocumentationTypeId);
                        //formElements.find("[name='select_users']").val(result.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='CreateDate']").datepicker('setDate', result.createDate);
                        formElements.find("[name='EndDate']").datepicker('setDate', result.endDate);
                        formElements.find("[name='IsTheLast']").val(result.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.isTheLast.toString()).trigger("change");
                        $("#edit_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                        $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                        $("#pdf_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });   
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/documentos-legal/${id}`),

                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='select_business']").val(result.businessId).trigger("change");
                        formElements.find("[name='LegalDocumentationTypeId']").val(result.legalDocumentationTypeId);
                        formElements.find("[name='select_legaldoctype']").val(result.legalDocumentationTypeId).trigger("change");
                        formElements.find("[name='LegalDocumentationRenovation.Id']").val(result.legalDocumentationRenovation.id);
                        //formElements.find("[name='select_users']").val(result.bondRenovation.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='LegalDocumentationRenovation.CreateDate']").datepicker('setDate', result.legalDocumentationRenovation.createDate);
                        formElements.find("[name='LegalDocumentationRenovation.EndDate']").datepicker('setDate', result.legalDocumentationRenovation.endDate);
                        formElements.find("[name='LegalDocumentationRenovation.IsTheLast']").val(result.legalDocumentationRenovation.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.legalDocumentationRenovation.isTheLast.toString()).trigger("change");
                        if (result.legalDocumentationRenovation.fileUrl) {
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
            addren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#add_ren_form");
                        formElements.find("[name='LegalDocumentationId']").val(result.legalDocumentationId);
                        formElements.find("[name='CreateDate']").datepicker('setDate', result.endDate);
                        $("#add_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='LegalDocumentationTypeId']").val($(formElement).find("[name='select_legaldoctype']").val());
                //$(formElement).find("[name='BondRenovation.Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='LegalDocumentationRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl("/licitaciones/documentos-legal/crear"),
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
                        dataTables.legalDocsDt.reload();
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
            editren: function (formElement) {
                //$(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='IsTheLAst']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/editar/${id}`),
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
                        dataTables.legalDocsRensDt.reload();
                        $("#edit_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_ren_alert_text").html(error.responseText);
                            $("#edit_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addren: function (formElement) {
                console.log(renId);
                $(formElement).find("[name='LegalDocumentationRenovationId']").val(renId);
                //$(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
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
                    url: _app.parseUrl(`/licitaciones/documentos-legal/renovaciones/crear`),
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
                        dataTables.legalDocsDt.reload();
                        $("#add_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_ren_alert_text").html(error.responseText);
                            $("#add_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='LegalDocumentationTypeId']").val($(formElement).find("[name='select_legaldoctype']").val());
                //$(formElement).find("[name='BondRenovation.Responsibles']").val($(formElement).find("[name='select_users']").val());
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
                    url: _app.parseUrl(`/licitaciones/documentos-legal/editar/${id}`),
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
                    })
                    .done(function (result) {
                        dataTables.legalDocsDt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            editren: function () {
                editRenForm.reset();
                $("#edit_ren_form").trigger("reset");
                $("#edit_ren_alert").removeClass("show").addClass("d-none");
                dataTables.legalDocsRensDt.reload();
                $("#detail_modal").modal("show");
            },
            addren: function () {
                addRenForm.reset();
                $("#add_ren_form").trigger("reset");
                $("#add_ren_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            pdf: function () {
                if (isFromDetail)
                    $("#detail_modal").modal("show");
            }
        }
    };

    var select2 = {
        init: function () {
            this.businesses.init();
            this.legalDocumentationType.init();
            this.legalDocumentationRenovations.init();
            this.isTheLast.init();
        },
        businesses: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empresas")
                }).done(function (result) {
                    $(".select2-businesses").select2({
                        data: result
                    });
                });
            }
        },
        users: {
            init: function () {
                //$.ajax({
                //    url: _app.parseUrl("/select/usuarios")
                //}).done(function (result) {
                //    $(".select2-users").select2({
                //        data: result
                //    });
                //    $.ajax({
                //        url: _app.parseUrl("/finanzas/responsables/proyecto")
                //    }).done(function (result) {
                //        $(".select2-users").val(result.responsibles.toString().split(',')).trigger("change");
                //    });
                //});
                $.ajax({
                    url: _app.parseUrl("/licitaciones/responsables-de-empresa")
                }).done(function (result) {

                });
            }
        },
        legalDocumentationType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-documentos-legal")
                }).done(function (result) {
                    $(".select2-legaldocType").select2({
                        data: result
                    });
                });
            }
        },
        legalDocumentationRenovations: {
            init: function () {
                $(".select2-legal-documentation-renovations").select2();
            },
            reload: function (legaldocumentationid, renid) {
                console.log(legaldocumentationid);
                console.log(renid);
                $(".select2-legal-documentation-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/documentos-legal/renovaciones/${legaldocumentationid}`)
                }).done(function (result) {
                    $(".select2-legal-documentation-renovations").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_legaldocumentationsrenovations").val(renid).trigger("change");
                    $("#btn_LoadPdf").trigger("click");
                });
            }
        },
        isTheLast: {
            init: function () {
                $(".select2-isthelast").select2();
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.add(formElement);
                }
            });

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            editRenForm = $("#edit_ren_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editren(formElement);
                }
            });

            addRenForm = $("#add_ren_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addren(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editren();
                });

            $("#add_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addren();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.pdf();
                });
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#btn_LoadPdf").on("click", function () {
                let id = $("#select_legaldocumentationsrenovations").val();
                console.log(id);
                forms.load.pdf(id);
            });

            //$("#project_filter,#bank_filter,#supply_family_filter, #supply_group_filter").on("change", function () {
            //    dataTables.bondAddsDt.reload();
            //});
        }
    };

    return {
        init: function () {
            select2.init();
            dataTables.init();
            validate.init();
            modals.init();
            datepickers.init();
            events.init();
        }
    };
}();

$(function () {
    Provider.init();
});