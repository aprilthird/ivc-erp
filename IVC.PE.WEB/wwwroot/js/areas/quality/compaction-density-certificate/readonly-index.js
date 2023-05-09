var CompactionDensityCertificate = function () {

    var sewerLineDatatable = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/calidad/certificado-densidad-compactacion/listar"),
            data: function (d) {
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.hasCertificate = $("#has_certificate").is(":checked");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Detalle",
                width: "5%",
                className: "details-control",
                orderable: false,
                data: null,
                defaultContent: "<i class='flaticon2-next'></i>"
            },
            {
                title: "Frente",
                data: "sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Ubicación",
                data: "address"
            },
            {
                title: "Tramo",
                data: "name"
            },
            {
                title: "Registro N°",
                data: "compactionDensityCertificate.serialNumber"
            },
            {
                title: "Fecha de Término",
                data: "compactionDensityCertificate.executionDate"
            },
            {
                title: "Clase de Material",
                data: "compactionDensityCertificate.materialType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.fillingLaboratory.materialType.VALUES);
                }
            },
            {
                title: "Cantera",
                data: "compactionDensityCertificate.quarry.name"
            },
            {
                title: "Profunfidad Prom. Tramo",
                data: "averageDepthSewerLine",
                render: _app.render.measure
            },
            {
                title: "DN (mm)",
                data: "nominalDiameter",
                render: _app.render.measure
            },
            {
                title: "Capas",
                data: "layers"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.compactionDensityCertificate.fileUrl) {
                        tmp += `<button data-id="${row.compactionDensityCertificate.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            }
        ]
    };

    var optionsDetail = {
        responsive: true,
        ajax: {
            url: "",
            dataSrc: ""
        },
        rowCallback: function (row, data) {
            //if (data.id) {
            //    if (data.firstResult > 210 && data.secondResult > 210) {
            //        $(row).css("background-color", "#f2d3cb");
            //    }
            //    else {
            //        $(row).css("background-color", "#ccffcc");
            //    }
            //}
        },
        columns: [
            {
                title: "Capa",
                data: "layer",
                render: function (data, type, row) {
                    return row.latest ? "Rasante" : data;
                }
            },
            {
                title: "Fecha de Ensayo",
                data: "testDate"
            },
            {
                title: "Muestra N°",
                data: "fillingLaboratoryTest.recordNumber"
            },
            {
                title: "Densidad Húmeda",
                data: "wetDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "% Humedad",
                data: "moisture",
                render: function (data) {
                    return data.toMeasure(1);
                }
            },
            {
                title: "Densidad Seca",
                data: "dryDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "Max. Densidad Seca",
                data: "fillingLaboratoryTest.maxDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "Opt. Contenido Humedad",
                data: "fillingLaboratoryTest.optimumMoisture",
                render: function (data) {
                    return data.toMeasure(1);
                }
            },
            {
                title: "% Densidad",
                data: "densityPercentage",
                className: "font-weight-bold",
                render: function (data) {
                    return data.toMeasure(1);
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            sewerLineDatatable = $("#sewer_line_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            sewerLineDatatable.ajax.reload();
        },
        initEvents: function () {
            sewerLineDatatable.on("click", "td.details-control", function () {
                var tr = $(this).closest("tr");
                var row = sewerLineDatatable.row(tr);
                console.log(row);
                if (row.child.isShown()) {
                    row.child.hide();
                    $(this).html("<i class='flaticon2-next'></i>");
                    tr.removeClass('shown');
                }
                else {
                    row.child(datatable.child.init(row.data())).show();
                    $(this).html("<i class='flaticon2-down'></i>");
                    tr.addClass('shown');
                }
            });

            sewerLineDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
        },
        child: {
            init: function (data) {
                optionsDetail.ajax.url = _app.parseUrl(`/calidad/certificado-densidad-compactacion/detalle/${data.id}/listar`);
                var $table = $(`<table id="certificate_${data.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
                $table.DataTable(optionsDetail);
                this.initEvents($table);
                return $table;
            },
            initEvents: function (table) {
                table.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.pdf(id);
                });
            }
        }
    };

    var form = {
        load: {
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/certificado-densidad-compactacion/${id}`)
                }).done(function (result) {
                    $("#pdf_serial_number").text(result.serialNumber);
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
            this.workFronts.init();
            this.sewerGroups.init();
            this.materialTypes.init();
            this.quarries.init();
            this.fillingLaboratoryTests.init();
        },
        workFronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes")
                }).done(function (result) {
                    $(".select2-work-fronts")
                        .on("change", function () {
                            select2.sewerGroups.init(`#${$(this).attr("id").replace("work_front", "sewer_group")}`, $(this).val());
                        })
                        .select2({
                            data: result,
                            placeholder: "Frente"
                        }).trigger("change");
                });
            }
        },
        sewerGroups: {
            init: function (selector, workFrontId) {
                if (selector)
                    $(selector).empty();
                else {
                    selector = ".select2-sewer-groups";
                }
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?type=1&workFrontId=${workFrontId}`)
                }).done(function (result) {
                    result.unshift({
                        text: "Todas",
                        id: "Todas"
                    });
                    $(selector).select2({
                        data: result,
                        placeholder: "Cuadrilla"
                    }).trigger("change");
                });
            }
        },
        materialTypes: {
            init: function () {
                $(".select2-material-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        quarries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/canteras")
                }).done(function (result) {
                    $(".select2-quarries").select2({
                        data: result
                    });
                });
            }
        },
        fillingLaboratoryTests: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/pruebas-de-laboratorio-de-rellenos")
                }).done(function (result) {
                    $(".select2-filling-laboratory-tests").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#sewer_group_filter, #has_certificate").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datepicker.init();
            datatable.init();
        }
    };
}();

$(function () {
    CompactionDensityCertificate.init();
});