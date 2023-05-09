var MachineryType = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var softId = null;
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/proceso/listar"),
            dataSrc: ""
        },
        columns: [

            {
                title: "Proceso",
                data: "processName"
            },
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Jefe de Proceso",
                data: "userName"
            },
            //{
            //    title: "Folding",
            //    data: null,
            //    orderable: false,
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-folding">`;
            //        tmp += `<i class="fa fa-history"></i></button> `;
            //        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-details">`;
            //        tmp += `<i class="fa fa-th-list"></i></button> `;
            //        return tmp;
            //    }
            //},
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
    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/oficina-tecnica/proceso-tipo-de-documentos/listar`),
            data: function (d) {
                d.bpId = softId;
                d.versionId = $("#version_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Tipo de Documento",
                data: "DocumentType"
            },
            {
                title: "Código",
                data: "code"
            },


            {
                title: "Opciones",
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
            this.bprintDt.init();
            this.foldingDt.init();
        },
        bprintDt: {
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
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        softId = id;
                        datatable.foldingDt.reload();
                        form.load.foldingFor05(id);
                    });

                mainDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        softId = id;
                        datatable.foldingDt.reload();
                        form.load.detail(id);
                    });

                mainDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El proceso será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/proceso/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.bprintDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El proceso ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el proceso"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        foldingDt: {
            init: function () {
                foldingDatatable = $("#folding_datatable").DataTable(foldingDtOpt);
                this.initEvents();
            },
            reload: function () {
                foldingDatatable.ajax.reload();
            },
            initEvents: function () {
                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        form.load.editfolding(id);
                    });



                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Tipo de documento será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/proceso-tipo-de-documentos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.bprintDt.reload();
                                            datatable.foldingDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Tipo de documento sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Tipo de documento"
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
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='ProcessId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso-tipo-de-documentos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProcessId']").val(result.processId);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='DocumentType']").val(result.documentType);
                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso/${id}`)
                })
                    .done(function (result) {

                        select2.processes.edit(result.userId);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProcessName']").val(result.processName);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='select_user']").prop("disabled", true);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/proceso/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
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
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso/editar/${id}`),
                    method: "put",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
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
            addfolding: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso-tipo-de-documentos/crear`),
                    method: "post",
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
                    },
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
                        $("#add_folding_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding_alert_text").html(error.responseText);
                            $("#add_folding_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding: function (formElement) {

                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/proceso-tipo-de-documentos/editar/${id}`),
                    method: "put",
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
                    },
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
                        $("#edit_folding_modal").modal("hide");
                        _app.show.notification.add.success();

                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding_alert_text").html(error.responseText);
                            $("#edit_folding_alert").removeClass("d-none").addClass("show");
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
                $("#add_form [name='ProjectId']").val(null).trigger("change");

            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='ProjectId']").val(null).trigger("change");
            },

            addfolding: function () {
                addFoldingForm.reset();
                $("#add_folding_form").trigger("reset");
                $("#add_folding_alert").removeClass("show").addClass("d-none");

            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatable.foldingDt.reload();
                $("#detail_modal").modal("show");


            },
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

            addFoldingForm = $("#add_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addfolding(formElement);
                }
            });

            editFoldingForm = $("#edit_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.editfolding(formElement);
                }
            });

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

        }
    };

    var select2 = {
        init: function () {
            this.styles.init();
            this.processes.init();
        },
        processes: {
            init: function () {
                $(".select2-users").empty();
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            },
            edit: function (id) {
                $("#edit_form .select2-users").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/empleados/${id}`)
                }).done(function (result) {
                    $("#edit_form .select2-users").select2({
                        data: result
                    });
                });
            }
        },
        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
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

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    form.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    form.reset.editfolding();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");
            $("#add_modal").on("shown.bs.modal", function () {
                $("#Add_ProjectId").select2();
            });
            $("#edit_modal").on("shown.bs.modal", function () {
                $("#Edit_ProjectId").select2();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
        }
    };
}();

$(function () {
    MachineryType.init();
});