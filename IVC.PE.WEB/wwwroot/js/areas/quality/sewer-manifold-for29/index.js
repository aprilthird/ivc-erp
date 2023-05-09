var For29 = function () {

    var addForm = null;
    var editForm = null;
    var for29Datatable = null;

    var for29DtOpt = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 3, 4],
                hide: [5, 6, 7, 8, 9, 10, 11, 12]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: [0, 1, 2, 3, 4, 5, 6, 7],
                hide: [8, 9, 10, 11, 12]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 3",
                show: ":hidden"
            }
        ],
        ajax: {
            url: _app.parseUrl("/calidad/for29-colector-descarga/listar"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "# Protocolo",
                data: "for01ProtocolNumber"
            },
            {
                //title: "Cuadrilla",
                data: "sewerManifold.productionDailyPart.sewerGroup.code"
            },
            {
                //title: "Dirección",
                data: "sewerManifold.address"
            },
            {
                //title: "Fecha de asfaltado",
                data: "asphaltDate"
            },
            {
                //title: "Nombre",
                data: "sewerManifold.name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "Asfaltado 2" (m²)",
                data: "pavement2InReview",
                visible: false
            },
            {
                //title: "Asfaltado 3" (m²)",
                data: "pavement3InReview",
                visible: false
            },
            {
                //title: "Asfaltado mixto (m²)",
                data: "pavement3InMixedReview",
                visible: false
            },
            {
                //title: "Long. entre ejes I",
                data: function (result) {
                    var resp = parseFloat(result.sewerManifold.lengthOfDigging).toFixed(2);
                    return resp;
                },
                visible: false
            },
            {
                //title: "Tipo de asfalto",
                data: function (result) {
                    var resp = parseInt(result.asphaltType);
                    if (resp == 1)
                        return "Asfalto en Caliente";
                    else if (resp == 2)
                        return "Mixto";
                },
                visible: false
            },
            {
                //title: "Espesor",
                data: function (result) {
                    var resp = parseInt(result.thickness);
                    if (resp == 1)
                        return "2\"";
                    else if (resp == 2)
                        return "3\"";
                },
                visible: false
            },
            {
                //title: "Area asfaltada (m²)",
                data: "asphaltArea",
                visible: false
            },
            {
                //title: "Area a Valorizar (m²)",
                data: "areaToValue",
                visible: false
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [5, 6, 7, 8, 9, 10, 11, 12] }
        ]
    };

    var datatable = {
        init: function () {
            this.for29Dt.init();
            this.initEvents();
        },
        for29Dt: {
            init: function () {
                for29Datatable = $("#for29s_datatable").DataTable(for29DtOpt);
            },
            reload: function () {
                for29Datatable.ajax.reload();
            }
        },
        initEvents: function () {
            for29Datatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.edit(id);
                });
            for29Datatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.pdf(id);
                })
            for29Datatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Estás Seguro?",
                        text: "El Sewer Manifold For29 será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/calidad/for29-colector-descarga/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.for29Dt.reload();
                                        select2.manifolds.init();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo técnico ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        })
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el archivo ténico"
                                        });
                                    }
                                });
                            });
                        },

                    })
                });
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                select2.manifolds.edit(id);
                $.ajax({
                    url: _app.parseUrl(`/calidad/for29-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_asphalt_type']").val(result.asphaltType).trigger("change");
                        formElements.find("[name='select_thickness']").val(result.thickness).trigger("change");
                        formElements.find("[name='AsphaltDate']").datepicker("setDate", result.asphaltDate);
                        formElements.find("[name='AsphaltArea']").val(result.asphaltArea);
                        formElements.find("[name='AreaToValue']").val(result.areaToValue);
                        formElements.find(".search-manifold").hide();
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
                    url: _app.parseUrl(`/calidad/for29-colector-descarga/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.sewerManifold.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='AsphaltType']").val($(formElement).find("[name='select_asphalt_type']").val());
                $(formElement).find("[name='Thickness']").val($(formElement).find("[name='select_thickness']").val());
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
                    url: _app.parseUrl("/calidad/for29-colector-descarga/crear"),
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
                        datatable.for29Dt.reload();
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
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='AsphaltType']").val($(formElement).find("[name='select_asphalt_type']").val());
                $(formElement).find("[name='Thickness']").val($(formElement).find("[name='select_thickness']").val());
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
                    url: _app.parseUrl(`/calidad/for29-colector-descarga/editar/${id}`),
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
                    .done(function () {
                        datatable.for29Dt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        $(formElement).find("[name='SewerManifold.LengthOfDigging']").prop("disabled", true);
                        _app.show.notification.edit.error();
                    })
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                select2.manifolds.init();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                select2.manifolds.init();
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

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                    $("#SearchSelectManifold").show();
                });

        }
    };

    var select2 = {
        init: function () {
            this.manifolds.init();
        },
        manifolds: {
            init: function () {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl("/select/colectores-descarga-ejecucion-for29-hasfor01")
                }).done(function (result) {
                    $(".select2-manifolds").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            },
            edit: function (id) {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-ejecucion-for29/${id}`)
                }).done(function (result) {
                    $(".select2-manifolds").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#SearchSelectManifold").on("click", function () {
                let formElements = $("#add_form");
                let id = formElements.find("[name='select_manifold']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/${id}`)
                }).done(function (result) {
                    console.log(result);
                    formElements.find("[name='SewerManifoldId']").val(result.id);
                });
            });
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            events.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    For29.init();
});