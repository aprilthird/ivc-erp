var Provider = function () {

    var skillsDataTable = null;
    var skillRensDataTable = null;
    var addForm = null;
    var editForm = null;    
    var addRenForm = null;
    var editRenForm = null;

    var isFromDetail = false;

    var addId = null;
    var renId = null;

    var skillsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/habilidades/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "DNI",
                data: "document"
            },
            {
                title: "Nombres y Apellidos",
                data: "professionalName"
            },
            {
                title: "Correo",
                data: "email"
            },
            {
                title: "Celular",
                data: "phoneNumber",
            },
            {
                title: "Profesion",
                data: "profession",
            },
            {
                title: "Colegiatura",
                data: "cipNumber",
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
                    if (data > 30) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${data} días</span>
								</label>
							</span>`;
                    }
                    else if (data > 15) {
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
                    tmp += `<button data-id="${row.skillId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.skillRenovationId}" class="btn btn-success btn-sm btn-icon btn-renovation">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.skillRenovationId}" data-sid="${row.skillId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
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
                    tmp += `<button data-id="${row.skillId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.skillId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
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

    var skillRensDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/habilidades/renovaciones/listar"),
            data: function (d) {
                d.skillRenovationId = addId;
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
                    tmp += `<button data-id="${row.id}" data-sid="${row.skillId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var dataTables = {
        init: function () {
            this.skillsDt.init();
            this.skillRensDt.init();
        },
        skillsDt: {
            init: function () {
                skillsDataTable = $("#skills_datatable").DataTable(skillsDt_options);
                this.events();
            },
            reload: function () {
                skillsDataTable.ajax.reload();
            },
            events: function () {
                skillsDataTable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        addId = id;
                        dataTables.skillRensDt.reload();
                        forms.load.detail(id);
                    });

                skillsDataTable.on("click",
                    ".btn-renovation",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        renId = id;
                        forms.load.addren(id);
                    });

                skillsDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let sid = $btn.data("sid");
                        let renid = $btn.data("id");
                        isFromDetail = false;
                        select2.skillRenovations.reload(sid, renid);
                    });

                skillsDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                skillsDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La habilidad y sus renovaciones serán eliminadas permanentemente.",
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
                                        url: _app.parseUrl(`/licitaciones/habilidades/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.skillsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La habilidad y sus renovaciones han sido eliminadas con éxito",
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
        skillRensDt: {
            init: function () {
                skillRensDataTable = $("#skillren_datatable").DataTable(skillRensDt_options);
                this.events();
            },
            reload: function () {
                skillRensDataTable.clear().draw();
                skillRensDataTable.ajax.reload();
            },
            events: function () {
                skillRensDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editren(id);
                    });

                skillRensDataTable.on("click",
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
                                        url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.skillsDt.reload();
                                            dataTables.skillRensDt.reload();
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

                skillRensDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let sid = $btn.data("sid");
                        let renid = $btn.data("id");
                        isFromDetail = true;
                        select2.skillRenovations.reload(sid, renid);
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
                    url: _app.parseUrl(`/licitaciones/habilidades/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProfessionalId']").val(result.professionalId);
                        formElements.find("[name='select_professional']").val(result.professionalId).trigger("change");
                        formElements.find("[name='select_select_professional']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });                
            },
            editren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_ren_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SkillId']").val(result.skillId);
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
                    url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        if (result.fileUrl != null) {

                            if (result.fileUrl.includes(".pdf")) {
                                var tmp = "";
                                tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                                $("#eye-view-pdf-img").html(tmp);
                                $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                            } else if (result.fileUrl.includes(".jpg")) {
                                var tmp = "";
                                tmp += `<img id="jpg_view" src="" style="width: 100%;">`
                                $("#eye-view-pdf-img").html(tmp);
                                $("#jpg_view").prop("src", result.fileUrl);
                                //$("#eye").html("src",result.fileUrl);
                            }
                        }
                        else if (result.fileUrl == null) {
                            var tmp = "";
                            tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                            $("#eye-view-pdf-img").html(tmp);
                        }
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
                    url: _app.parseUrl(`/licitaciones/habilidades/${id}`),

                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='professionalId']").val(result.professionalId);
                        formElements.find("[name='select_professional']").val(result.professionalId).trigger("change");
                        formElements.find("[name='select_professional']").attr("disabled", "disabled");
                        formElements.find("[name='SkillRenovation.Id']").val(result.skillRenovation.id);
                        //formElements.find("[name='select_users']").val(result.bondRenovation.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='SkillRenovation.CreateDate']").datepicker('setDate', result.skillRenovation.createDate);
                        formElements.find("[name='SkillRenovation.EndDate']").datepicker('setDate', result.skillRenovation.endDate);
                        formElements.find("[name='SkillRenovation.IsTheLast']").val(result.skillRenovation.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.skillRenovation.isTheLast.toString()).trigger("change");
                        if (result.skillRenovation.fileUrl) {
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
                    url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#add_ren_form");
                        formElements.find("[name='SkillId']").val(result.skillId);
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
                $(formElement).find("[name='ProfessionalId']").val($(formElement).find("[name='select_professional']").val());
                //$(formElement).find("[name='BondRenovation.Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='SkillRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl("/licitaciones/habilidades/crear"),
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
                        dataTables.skillsDt.reload();
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
                    url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/editar/${id}`),
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
                        dataTables.skillRensDt.reload();
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
                $(formElement).find("[name='SkillRenovationId']").val(renId);
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
                    url: _app.parseUrl(`/licitaciones/habilidades/renovaciones/crear`),
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
                        dataTables.skillsDt.reload();
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
                $(formElement).find("[name='ProfessionalId']").val($(formElement).find("[name='select_professional']").val());
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
                    url: _app.parseUrl(`/licitaciones/habilidades/editar/${id}`),
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
                        dataTables.skillsDt.reload();
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
                dataTables.skillRensDt.reload();
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
            this.professionals.init();
            this.skillRenovations.init();
            this.isTheLast.init();
        },
        professionals: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/profesionales-lista")
                }).done(function (result) {
                    $(".select2-professionals").select2({
                        data: result
                    });
                });
            }
        },
        skillRenovations: {
            init: function () {
                $(".select2-skill-renovations").select2();
            },
            reload: function (sid, renid) {
                console.log(sid);
                console.log(renid);
                $(".select2-skill-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/habilidades/renovaciones/${sid}`)
                }).done(function (result) {
                    $(".select2-skill-renovations").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_skillrenovations").val(renid).trigger("change");
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
                let id = $("#select_skillrenovations").val();
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