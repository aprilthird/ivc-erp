var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;

    var for05Datatable = null;
    var foldingDatatable = null;
    var importDataForm = null;
    var importFilesForm = null;

    var for05Id = null;
    var foldingId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/calidad/for05-colector-descarga/listar"),
            data: function (d) {
                d.sewerGroupId = $("#SewerGroup").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                //title: "# Certificado",
                data: "certificateNumber"
            },
            {
                //title: "Cuadrilla",
                data: "sewerManifold.productionDailyPart.sewerGroup.code"
            },
            {
                //title: "Tramo",
                data: "sewerManifold.name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "Direccion",
                data: "sewerManifold.address"
            },
            {
                //title: "Long. entre ejes H",
                data: function (result) {
                    resp = parseFloat(result.sewerManifold.lengthOfDigging);
                    return resp.toFixed(2);
                }
            },          
            {
                //title: "H. Zanja (m)",
                data: "sewerManifold.ditchHeight"
            },
            {
                //title: "H. Relleno (m)",
                data: "filling"
            },
            {
                //title: "# Capa Teórica",
                data: "theoreticalLayer"
            },
            {
                //title: "# Capa Real",
                data: "layerNumber"
            },
            {
                //title: "# Pruebas",
                data: "layersNumber"
            },
            {
                //title: "Fecha Ensayo",
                data: "testDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                //title: "Estado",
                data: "status"
            },
            {
                //title: "Fecha Envio",
                data: "shippingDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                //title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                }
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
        ]
    };

    var foldingDtOpt = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl(`/calidad/foldingfor05-for05-colector-descarga/listar`),
            data: function (d) {
                d.SewerManifoldFor05Id = for05Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "# Capa",
                data: "layerNumber"
            },
            {
                title: "Fecha_Ensayo",
                data: "testDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Código de Muestra",
                data: "fillingLaboratoryTest.recordNumber"
            },
            {
                title: "Máxima Densidad Seca",
                data: "fillingLaboratoryTest.maxDensity"
            },
            {
                title: "Optimo Contenido Humedad",
                data: "fillingLaboratoryTest.optimumMoisture"
            },
            {
                title: "Densidad Humeda",
                data: "wetDensity"
            },
            {
                title: "Humedad (%)",
                data: function (result) {
                    return result.moisturePercentage + "%";
                }
            },
            {
                title: "Densidad Seca",
                data: function (result) {
                    var resp = parseFloat(result.dryDensity).toFixed(3);
                    return resp;
                }
            },
            {
                title: "Porcentaje Compactación Requerido",
                data: function (result) {
                    return result.percentageRequiredCompaction + "%";
                }
            },
            {
                title: "Porcentaje Compactación Real",
                data: function (result) {
                    var resp = parseFloat(result.percentageRealCompaction).toFixed(1);
                    return resp + "%";
                }
            },
            {
                title: "Estado_Capa",
                data: "status"
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

    }

    var datatables = {
        init: function () {
            this.for05Dt.init();
            this.foldingDt.init();
        },
        for05Dt: {
            init: function () {
                for05Datatable = $("#for05s_datatable").DataTable(for05DtOpt);
                this.initEvents();
            },
            reload: function () {
                for05Datatable.ajax.reload();
                select2.index.longitud();
                select2.index.porcentaje();
                select2.index.registros();
                select2.index.tests();
            },
            initEvents: function () {

                for05Datatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        for05Id = id;
                        datatables.foldingDt.reload();
                        forms.load.detail(id);
                    });

                for05Datatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        $("#add_folding_form #FLT_MaxDensity").prop("disabled", true);
                        $("#add_folding_form #FLT_OptimumMoisture").prop("disabled", true);
                        forms.load.foldingFor05(id);
                    });

                for05Datatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                for05Datatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.pdf(id);
                    });

                for05Datatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Sewer Manifold For05 será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/calidad/for05-colector-descarga/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();
                                            select2.manifolds.init();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el archivo técnico"
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
                this.events();
            },
            reload: function () {
                foldingDatatable.ajax.reload();
                select2.index.longitud();
                select2.index.porcentaje();
                select2.index.registros();
                select2.index.tests();
            },
            events: function () {
                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editfolding(id);
                    });

                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Folding For05 será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/calidad/foldingfor05-for05-colector-descarga/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt.reload();
                                            datatables.for05Dt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el archivo técnico"
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
    var forms = {
        load: {
            edit: function (id) {
                select2.manifolds.edit(id);
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for05-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='select_manifold']").prop("disabled", true);
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='CertificateNumber']").val(result.certificateNumber);
                        $("#edit_form #SewerGroup_Code").val(result.sewerManifold.productionDailyPart.sewerGroup.code).prop("disabled", true);
                        formElements.find("[name='ShippingDate']").datepicker("setDate", result.shippingDate);
                        formElements.find("#SearchSelectManifold").hide();
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
                    url: _app.parseUrl(`/calidad/for05-colector-descarga/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.sewerManifold.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for05-colector-descarga/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='CertificateNumber']").val(result.certificateNumber);
                        formElements.find("[name='SewerManifold.ProductionDailyPart.SewerGroup.Code']").val(result.sewerManifold.productionDailyPart.sewerGroup.code);
                        formElements.find("[name='SewerManifold.Name']").val(result.sewerManifold.name);
                        formElements.find("[name='SewerManifold.Address']").val(result.sewerManifold.address);
                        formElements.find("[name='SewerManifold.LengthOfDigging']").val(result.sewerManifold.lengthOfDigging.toFixed(2));
                        formElements.find("[name='ShippingDate']").val(result.shippingDate);
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for05-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        console.log("Aquí");
                        console.log(result);
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='SewerManifoldFor05Id']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {
                select2.fillinglaboratorytests.edit(id);
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor05-for05-colector-descarga/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_filling_laboratory_test']").prop("disabled", true);
                        formElements.find("[name='SewerManifoldFor05Id']").val(result.sewerManifoldFor05Id);
                        formElements.find("[name='FillingLaboratoryTestId']").val(result.fillingLaboratoryTestId);
                        $("#edit_folding_form #FLT_MaxDensity").val(result.fillingLaboratoryTest.maxDensity).prop("disabled", true);
                        $("#edit_folding_form #FLT_OptimumMoisture").val(result.fillingLaboratoryTest.optimumMoisture).prop("disabled", true);
                        formElements.find("[name='WetDensity']").val(result.wetDensity);
                        formElements.find("[name='MoisturePercentage']").val(result.moisturePercentage);
                        formElements.find("[name='LayerNumber']").val(result.layerNumber);
                        formElements.find("[name='TestDate']").datepicker("setDate", result.testDate);
                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
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
                    url: _app.parseUrl("/calidad/for05-colector-descarga/crear"),
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
                        datatables.for05Dt.reload();
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
                    url: _app.parseUrl(`/calidad/for05-colector-descarga/editar/${id}`),
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
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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
                $(formElement).find("[name='FillingLaboratoryTestId']").val($(formElement).find("[name='select_filling_laboratory_test']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor05-for05-colector-descarga/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt.reload();
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
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor05-for05-colector-descarga/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt.reload();
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
                        url: "/calidad/for05-colector-descarga/importar-datos",
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
                        datatables.for05Dt.reload();
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
                        url: "/calidad/for05-colector-descarga/importar-archivos",
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
                        datatables.for05Dt.reload();
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
                addForm.reset();
                $("#add_form").trigger("reset");
                select2.manifolds.init();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                select2.manifolds.init();
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
                datatables.foldingDt.reload();
                $("#detail_modal").modal("show");
            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
                files: function () {
                    importFilesForm.reset();
                    $("#import_files_form").trigger("reset");
                    $("#import_files_alert").removeClass("show").addClass("d-none");
                }
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addFoldingForm = $("#add_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addfolding(formElement);
                }
            });

            editFoldingForm = $("#edit_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editfolding(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
                }
            });

            importFilesForm = $("#import_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.files(formElement);
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.manifolds.init();
            this.fillinglaboratorytests.init();
            this.sewergroups.init();
            this.index.longitud();
            this.index.porcentaje();
            this.index.registros();
            this.index.tests();
        },
        manifolds: {
            init: function () {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl("/select/colectores-descarga-ejecucion-for05-hasfor01")
                }).done(function (result) {
                    $("#add_form .select2-manifolds").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            },
            edit: function (id) {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-ejecucion-for05/${id}`)
                }).done(function (result) {
                    $("#edit_form .select2-manifolds").select2({
                        data: result
                    });
                });
            }
        },
        sewergroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/for05-cuadrillas")
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
                $("#SewerGroup").on("change", function () {
                    datatables.for05Dt.reload();
                });
            }
        },
        fillinglaboratorytests: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/pruebas-de-laboratorio-de-rellenos")
                }).done(function (result) {
                    $("#add_folding_form .select2-filling-laboratory-tests").select2({
                        data: result
                    });
                });
            },
            edit: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/pruebas-de-laboratorio-de-rellenos/${id}`)
                }).done(function (result) {
                    $("#edit_folding_form .select2-filling-laboratory-tests").select2({
                        data: result
                    });
                });
            }
        },
        index: {
            registros: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/for05-colector-descarga/total")
                }).done(function (result) {
                    $("#TotalRegister").val(result);
                });
            },
            longitud: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/for05-colector-descarga/suma-longitud")
                }).done(function (result) {
                    $("#LengthFor05").val(result);
                });
            },
            tests: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/foldingfor05-for05-colector-descarga/total")
                }).done(function (result) {
                    $("#TotalTest").val(result);
                });
            },
            porcentaje: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/for05-colector-descarga/porcentaje")
                }).done(function (result) {
                    $("#PercentageFor05").val(result);
                });
            }
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
                });

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.data();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.files();
                });

        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
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
                    $("#add_form #SewerGroup_Code").val(result.productionDailyPart.sewerGroup.code).prop("disabled", true);;
                    formElements.find("[name='SewerManifold.ProductionDailyPart.SewerGroup.WorkFrontHeadId']").val(result.productionDailyPart.sewerGroup.workFrontHeadId);
                });
            });
        },
        initLab: function () {
            $("#SearchFillingLaboratoryTest").on("click", function () {
                console.log("Me apretaste");
                let formElements = $("#add_folding_form");
                let id = formElements.find("[name='select_filling_laboratory_test']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/pruebas-de-laboratorio-de-relleno/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        $("#add_folding_form #FLT_MaxDensity").val(result.maxDensity).prop("disabled", true);
                        $("#add_folding_form #FLT_OptimumMoisture").val(result.optimumMoisture).prop("disabled", true);
                    });
            });
        },
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/calidad/for05-colector-descarga/excel-carga-masiva`;
            });
        },
        boom: function () {
            $("#btn-massive-delete").on("click", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Todo será eliminado permanentemente",
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
                                url: _app.parseUrl(`/calidad/for05-colector-descarga/todo-eliminar`),
                                type: "delete",
                                success: function (result) {
                                    datatables.for05Dt.reload();
                                    select2.manifolds.init();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "El archivo técnico ha sido eliminada con éxito",
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
                                        text: "Ocurrió un error al intentar eliminar el archivo técnico"
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            events.init();
            events.initLab();
            events.excel();
            events.boom();
            datatables.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    For05.init();
});