var Requests = function () {

    var requestsDt = null;

    var importModal = null;

    var requestsOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/requerimiento/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Nº Req.",
                data: null
            },
            {
                title: "Centro de Costo",
                data: null
            },
            {
                title: "Formula",
                data: null
            },
            {
                title: "Fase",
                data: null
            },
            {
                title: "Jefe de Frente",
                data: null
            },
            {
                title: "Cuadrilla",
                data: null
            },
            {
                title: "Capataz",
                data: null
            },
            {
                title: "Material",
                data: null
            },
            {
                title: "Vol (m3)",
                data: null
            },
            {
                title: "Atendido",
                data: null
            },
            {
                title: "Entrega",
                data: null
            },
            {
                title: "Turno",
                data: null
            },
            {
                title: "Estado",
                data: null
            },
            {
                title: "Registro",
                data: null
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

    var datatables = {
        init: function () {
            this.requests.init();
        },
        requests: {
            init: function () {
                requestsDt = $("#requests_datatable").DataTable(requestsOpts);
            },
            reload: function () {
                requestsDt.ajax.reload();
            }
        }
    };

    var forms = {
        submit: {
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
                    url: "/agregados/requerimientos/importar",
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
                    datatables.requests.reload();
                    $("#requests_import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#request_import_alert_text").html(error.responseText);
                        $("#request_import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            }
        },
        reset: {
            import: function () {
                importModal.resetForm();
                $("#request_import_form").trigger("reset");
                $("#request_import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            importModal = $("#request_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#requests_import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });
        }
    };

    var events = {
        init: function () {
            $("#importExcelFormat").on("click", function () {
                window.location = _app.parseUrl(`/agregados/requerimiento/formato-carga`);
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    Requests.init();
});