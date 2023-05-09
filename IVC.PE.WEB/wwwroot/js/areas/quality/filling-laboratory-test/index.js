var FillingLaboratoryTest = function () {

    var fillingLaboratoryTestDatatable = null;
    var addForm = null;
    var editForm = null;
    var importDataForm = null;
    var importFilesForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/calidad/pruebas-de-laboratorio-de-relleno/listar"),
            dataSrc: "",
            data: function (d) {
                d.materialType = $("#material_type_filter").val();
                d.originType = $("#origin_type_filter").val();
                d.hasFile = $("#has_file").is(":checked");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Material",
                data: "materialType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.fillingLaboratory.materialType.VALUES);
                }
            },/*
            {
                title: "Procedencia",
                data: "originType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.fillingLaboratory.originType.VALUES);
                }
            },*/
            {
                title: "Procedencia Prueba",
                data: "originTypeFillingLaboratory.originTypeFLName"
            },
            {
                title: "Ubicación",
                data: "ubication"
            },
            {
                title: "Muestra N°",
                data: "recordNumber"
            },
            {
                title: "N° Certificado",
                data: "serialNumber"
            },
            {
                title: "Certificado por",
                data: "certificateIssuer.name"
            },
            {
                title: "Fecha de Muestreo",
                data: "samplingDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Fecha de Ensayo",
                data: "testDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Cont. Material Humedad",
                data: "materialMoisture",
                render: _app.render.measure
            },
            {
                title: "Max. Densidad",
                data: "maxDensity",
                render: _app.render.measure
            },
            {
                title: "Optimo C. Humedad",
                data: "optimumMoisture",
                render: _app.render.measure
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
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
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
            fillingLaboratoryTestDatatable = $("#filling_laboratory_test_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            fillingLaboratoryTestDatatable.ajax.reload();
        },
        initEvents: function () {
            fillingLaboratoryTestDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            fillingLaboratoryTestDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
            fillingLaboratoryTestDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La prueba de laboratorio de relleno será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/calidad/pruebas-de-laboratorio-de-relleno/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La prueba de laboratorio de relleno ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la prueba de laboratorio de relleno"
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
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/pruebas-de-laboratorio-de-relleno/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='SerialNumber']").val(result.serialNumber);
                        formElements.find("[name='RecordNumber']").val(result.recordNumber);
                        formElements.find("[name='Ubication']").val(result.ubication);
                        formElements.find("[name='SamplingDate']").datepicker("setDate", result.samplingDate);
                        formElements.find("[name='TestDate']").datepicker("setDate", result.testDate);
                        formElements.find("[name='select_origin_type']").val(result.originTypeFillingLaboratoryId).trigger("change");
                        formElements.find("[name='MaterialType']").val(result.materialType).trigger("change");
                        formElements.find("[name='CertificateIssuerId']").val(result.certificateIssuerId).trigger("change");
                        formElements.find("[name='MaterialMoisture']").val(result.materialMoisture);
                        formElements.find("[name='MaxDensity']").val(result.maxDensity);
                        formElements.find("[name='OptimumMoisture']").val(result.optimumMoisture);
                        $(".search-manifold").hide();
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
                    url: _app.parseUrl(`/calidad/pruebas-de-laboratorio-de-relleno/${id}`)
                }).done(function (result) {
                    $("#pdf_serial_number").text(result.recordNumber);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='OriginTypeFillingLaboratoryId']").val($(formElement).find("[name='select_origin_type']").val());

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
                    url: _app.parseUrl("/calidad/pruebas-de-laboratorio-de-relleno/crear"),
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
                $(formElement).find("[name='OriginTypeFillingLaboratoryId']").val($(formElement).find("[name='select_origin_type']").val());
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
                    url: _app.parseUrl(`/calidad/pruebas-de-laboratorio-de-relleno/editar/${id}`),
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
                        url: "/calidad/pruebas-de-laboratorio-de-relleno/importar-datos",
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
                        url: "/calidad/pruebas-de-laboratorio-de-relleno/importar-archivos",
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
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
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
            this.materialTypes.init();
            this.originTypes.init();
            this.certificateIssuers.init();
            this.origins.init();
        },
        materialTypes: {
            init: function () {
                $(".select2-material-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        originTypes: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/origins-type-laboratory")
                }).done(function (result) {
                    $(".select2-origin-types").select2({
                        minimumResultsForSearch: -1,
                        data: result
                    });
                });
            }
        },
        certificateIssuers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/emisores-de-certificados")
                }).done(function (result) {
                    $(".select2-certificate-issuers").select2({
                        data: result
                    });
                });
            }
        },
        origins: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/origins-type-laboratory")
                }).done(function (result) {
                    $(".select2-origins").select2({
                        data: result
                    });
                });
            }
        },
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='CertificateIssuerId']").attr("id", "Add_CertificateIssuerId");
            $("#edit_form [name='CertificateIssuerId']").attr("id", "Edit_CertificateIssuerId");

            $("#material_type_filter, #origin_type_filter, #has_file").on("change", function () {
                datatable.reload();
            });
        },
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/calidad/pruebas-de-laboratorio-de-relleno/excel-carga-masiva`;
            });
        }
    };

    return {
        init: function () {
            events.init();
            events.excel();
            select2.init();
            datepicker.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    FillingLaboratoryTest.init();
});