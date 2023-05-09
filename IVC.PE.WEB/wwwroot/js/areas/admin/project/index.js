var Project = function () {

    var projectDatatable = null;
    var habilitationDatatable = null;
    var addForm = null;
    var editForm = null;
    var habilitationAddForm = null;
    var pdfForm = null;
    var habilitationEditForm = null;

    var projectId = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/proyectos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Centro de Costo",
                data: "costCenter"
            },
            {
                title: "Abreviación",
                data: "abbreviation",
                render: function (data) {
                    return data || "-";
                }
            },
            {
                title: "Carpeta",
                data: null,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-habilitation">`;
                    tmp += `<i class="fa fa-map-marked"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Ejecutor",
                data: "business.tradename"
            },
            {
                title: "Logo",
                data: "logoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Proyecto" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
            {
                title: "Firma",
                data: "invoiceSignatureUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Proyecto" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
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
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var habilitationOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/admin/habilitaciones-proyecto/listar"),
            dataSrc: "",
            data: function (d) {
                d.projectId = projectId;
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Codigo",
                data: "locationCode"
            },
            {
                title: "Habilitación",
                data: "description"
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
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#habilitation_modal").modal("hide");
                    $("#habilitation_add_modal").modal("show");
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            projectDatatable = $("#project_datatable").DataTable(options);
            this.initEvents();
            this.habilitationDt.init();
        },
        reload: function () {
            projectDatatable.ajax.reload();
        },
        initEvents: function () {
            projectDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            projectDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                let url = $btn.data("url");
                let sname = $btn.data("name");
                form.load.pdf(id, url, sname);
            });

            projectDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El proyecto será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/admin/proyectos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El proyecto ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el proyecto"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            projectDatatable.on("click",
                ".btn-habilitation",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.habilitation(id);
                });
        },
        habilitationDt: {
            init: function () {
                habilitationDatatable = $("#habilitations_datatable").DataTable(habilitationOpts);
                this.events();
            },
            reload: function (id) {
                projectId = id;
                habilitationDatatable.clear().draw();
                habilitationDatatable.ajax.reload();
            },
            events: function () {
                habilitationDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.habilitationEdit(id);
                    });

                habilitationDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La habilitación será eliminada permanentemente",
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
                                        url: _app.parseUrl(`/admin/habilitaciones-proyecto/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.habilitationDt.reload(projectId);
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La habilitación ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la habilitación"
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
            pdf: function (id, url, sname) {

                if (url != null) {

                    if (url.includes(".pdf")) {
                        var tmp = "";
                        tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                        $("#eye-view-pdf-img").html(tmp);
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");

                    } else if (url.includes(".jpg") || url.includes(".jpeg") || url.includes(".png")) {
                        var tmp = "";
                        tmp += `<img id="jpg_view" src="" style="width: 100%;">`
                        $("#eye-view-pdf-img").html(tmp);
                        $("#jpg_view").prop("src", url);
                        //$("#eye").html("src",result.fileUrl);
                    }
                }
                else if (result.logoUrl == null) {
                    var tmp = "";
                    tmp += `<iframe id="pdf_frame" src="" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen style="width: 100%; height: 600px;"></iframe> `;
                    $("#eye-view-pdf-img").html(tmp);
                }

                $("#pdf_modal").modal("show");



                //_app.loader.show();
                //$.ajax({
                //    url: _app.parseUrl(`/admin/proyectos/${id}`)
                //}).done(function (result) {
                    
                //}).always(function () {
                //    _app.loader.hide();
                //});
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/proyectos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='CostCenter']").val(result.costCenter);
                        formElements.find("[name='Abbreviation']").val(result.abbreviation);
                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='EstablishmentCode']").val(result.establishmentCode);
                        formElements.find("[name='select_business']").val(result.businessId).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            habilitation: function (id) {
                datatable.habilitationDt.reload(id);
                $("#habilitation_modal").modal("show");
            },
            habilitationEdit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/admin/habilitaciones-proyecto/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#habilitation_edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='LocationCode']").val(result.locationCode);
                        formElements.find("[name='Description']").val(result.description);
                        $("#habilitation_modal").modal("hide");
                        $("#habilitation_edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
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
                    url: _app.parseUrl("/admin/proyectos/crear"),
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
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
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
                    url: _app.parseUrl(`/admin/proyectos/editar/${id}`),
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
            habilitationAdd: function (formElement) {
                $(formElement).find("[name='ProjectId']").val(projectId);
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/admin/habilitaciones-proyecto/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.habilitationDt.reload(projectId);
                        $("#habilitation_add_modal").modal("hide");
                        _app.show.notification.add.success();                        
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#habilitation_add_alert_text").html(error.responseText);
                            $("#habilitation_add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            habilitationEdit: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/admin/habilitaciones-proyecto/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.habilitationDt.reload(projectId);
                        $("#habilitation_edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#habilitation_edit_alert_text").html(error.responseText);
                            $("#habilitation_edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
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
            habilitationAdd: function () {
                habilitationAddForm.resetForm();
                $("#habilitation_modal").modal("show");
                $("#habilitation_add_form").trigger("reset");
                $("#habilitation_add_alert").removeClass("show").addClass("d-none");
            },
            habilitationEdit: function () {
                habilitationEditForm.resetForm();
                $("#habilitation_modal").modal("show");
                $("#habilitation_edit_form").trigger("reset");
                $("#habilitation_edit_alert").removeClass("show").addClass("d-none");
            },
            pdf: function () {
                pdfForm.resetForm();
                $("#habilitation_modal").modal("show");
                $("#habilitation_edit_form").trigger("reset");
                $("#habilitation_edit_alert").removeClass("show").addClass("d-none");
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

            habilitationAddForm = $("#habilitation_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.habilitationAdd(formElement);
                }
            });

            habilitationEditForm = $("#habilitation_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.habilitationEdit(formElement);
                }
            });

            pdfForm = $("#pdf_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.pdf(formElement);
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

            $("#habilitation_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.habilitationAdd();
                });

            $("#habilitation_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.habilitationEdit();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    form.reset.pdf();
                });
        }
    };
    var select2 = {
        init: function () {


            this.businesses.init();

        },
           
        businesses: {
            init: function () {
                
                $.ajax({

                    url: _app.parseUrl(`/select/empresas`)
                }).done(function (result) {
                    $(".select2-businesses").select2({
                        data: result
                    });
                });
            }
        },
   




    };
    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
        }
    };
}();

$(function () {
    Project.init();
});
