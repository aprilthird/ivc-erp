var FillingLaboratoryTest = function () {

    var fillingLaboratoryTestDatatable = null;
    
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
            },
            {
                title: "Procedencia",
                data: "originType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.fillingLaboratory.originType.VALUES);
                }
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
                data: "samplingDate"
            },
            {
                title: "Fecha de Ensayo",
                data: "testDate"
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
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
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
            fillingLaboratoryTestDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
        }
    };

    var form = {
        load: {
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
        }
    };

    var select2 = {
        init: function () {
            this.materialTypes.init();
            this.originTypes.init();
            this.certificateIssuers.init();
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
                $(".select2-origin-types").select2({
                    minimumResultsForSearch: -1
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
        }
    };

    var events = {
        init: function () {
            $("#material_type_filter, #origin_type_filter, #has_file").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
        }
    };
}();

$(function () {
    FillingLaboratoryTest.init();
});