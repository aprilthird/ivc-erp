var DrainageNetworkSummary = function () {

    var contractualDatatable = null;
    var stakingDatatable = null;
    var realDatatable = null;

    var options1 = {
        scrollX: true,
        scrollCollapse: true,
        responsive: false,
        serverSide: true,
        processing: true,
        buttons: [
            { extend: "copyHtml5", text: "<i class='fa fa-copy'></i> Copiar", className: " btn-dark" },
            { extend: "excelHtml5", text: "<i class='fa fa-file-excel'></i> Excel", className: "btn-success" },
            { extend: "csvHtml5", text: "<i class='fa fa-file-csv'></i> CSV", className: "btn-success" },
            { extend: "pdfHtml5", text: "<i class='fa fa-file-pdf'></i> PDF", className: "btn-danger", pageSize: "C2", orientation: "landscape" },
            { extend: "print", text: "<i class='fa fa-print'></i> Imprimir", className: "btn-dark" }
        ],
        ajax: {
            url: "",
            data: function (d) {
                delete d.columns;
            }
            //dataSrc: ""
        },
        columns: [
            {
                title: "Frente (Fase del Sistema)",
                data: "sewerGroup.workFront.systemPhase.code"
            },
            {
                title: "Frente de Trabajo (delimitado)",
                data: "sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Código de Habilitación",
                data: "qualificationZone.code"
            },
            {
                title: "Habilitación",
                data: "qualificationZone.name"
            },
            {
                title: "Dirección",
                data: "address"
            },
            {
                title: "Área Drenaje",
                data: "initialSewerDrainageArea"
            },
            {
                title: "BZ (i)",
                data: "initialSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "initialSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "waterUpCover",
                render: _app.render.measure
            },
            {
                title: "Salida",
                data: "waterUpOutput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "waterUpBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "initialSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "initialSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "initialSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "initialSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "finalSewerDrainageArea"
            },
            {
                title: "BZ (j)",
                data: "finalSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "finalSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "downstreamCover",
                render: _app.render.measure
            },
            {
                title: "Entrada",
                data: "downstreamInput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "downstreamBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "finalSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "finalSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "finalSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "finalSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "drainageArea"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Dist. Horizontal / Ejes",
                data: "horizontalDistanceOnAxis",
                render: _app.render.measure
            },
            {
                title: "Profunfidad Tramo",
                data: "averageDepthSewerLine",
                render: _app.render.measure
            },
            {
                title: "Pendiente %o",
                data: "slope",
                render: _app.render.measure
                //render: _app.render.percent
            },
            {
                title: "DN (mm)",
                data: "nominalDiameter",
                render: _app.render.measure
            },
            {
                title: "Tipo Tubería",
                data: "pipelineType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.type.VALUES);
                }
            },
            {
                title: "Clase",
                data: "pipelineClass",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.class.VALUES);
                }
            },
            {
                title: "Tipo Terreno",
                data: "terrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Long. Tubería Instalada",
                data: "installedPipelineLength",
                render: _app.render.measure
            },
            {
                title: "Long. Excavación",
                data: "excavationLength",
                render: _app.render.measure
            }
        ]
    };

    var options2 = {
        scrollX: true,
        scrollCollapse: true,
        responsive: false,
        serverSide: true,
        processing: true,
        buttons: [
            { extend: "copyHtml5", text: "<i class='fa fa-copy'></i> Copiar", className: " btn-dark" },
            { extend: "excelHtml5", text: "<i class='fa fa-file-excel'></i> Excel", className: "btn-success" },
            { extend: "csvHtml5", text: "<i class='fa fa-file-csv'></i> CSV", className: "btn-success" },
            { extend: "pdfHtml5", text: "<i class='fa fa-file-pdf'></i> PDF", className: "btn-danger", pageSize: "C2", orientation: "landscape" },
            { extend: "print", text: "<i class='fa fa-print'></i> Imprimir", className: "btn-dark" }
        ],
        ajax: {
            url: "",
            data: function (d) {
                delete d.columns;
            }
            //dataSrc: ""
        },
        columns: [
            {
                title: "Frente (Fase del Sistema)",
                data: "sewerGroup.workFront.systemPhase.code"
            },
            {
                title: "Frente de Trabajo (delimitado)",
                data: "sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Código de Habilitación",
                data: "qualificationZone.code"
            },
            {
                title: "Habilitación",
                data: "qualificationZone.name"
            },
            {
                title: "Dirección",
                data: "address"
            },
            {
                title: "Área Drenaje",
                data: "initialSewerDrainageArea"
            },
            {
                title: "BZ (i)",
                data: "initialSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "initialSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "waterUpCover",
                render: _app.render.measure
            },
            {
                title: "Salida",
                data: "waterUpOutput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "waterUpBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "initialSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "initialSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "initialSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "initialSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "finalSewerDrainageArea"
            },
            {
                title: "BZ (j)",
                data: "finalSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "finalSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "downstreamCover",
                render: _app.render.measure
            },
            {
                title: "Entrada",
                data: "downstreamInput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "downstreamBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "finalSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "finalSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "finalSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "finalSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "drainageArea"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Dist. Horizontal / Ejes",
                data: "horizontalDistanceOnAxis",
                render: _app.render.measure
            },
            {
                title: "Profunfidad Tramo",
                data: "averageDepthSewerLine",
                render: _app.render.measure
            },
            {
                title: "Pendiente %o",
                data: "slope",
                render: _app.render.measure
                //render: _app.render.percent
            },
            {
                title: "DN (mm)",
                data: "nominalDiameter",
                render: _app.render.measure
            },
            {
                title: "Tipo Tubería",
                data: "pipelineType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.type.VALUES);
                }
            },
            {
                title: "Clase",
                data: "pipelineClass",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.class.VALUES);
                }
            },
            {
                title: "Tipo Terreno",
                data: "terrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Long. Tubería Instalada",
                data: "installedPipelineLength",
                render: _app.render.measure
            },
            {
                title: "Long. Excavación",
                data: "excavationLength",
                render: _app.render.measure
            }
        ]
    };

    var options3 = {
        scrollX: true,
        scrollCollapse: true,
        responsive: false,
        ordering: false,
        serverSide: true,
        processing: true,
        buttons: [
            { extend: "copyHtml5", text: "<i class='fa fa-copy'></i> Copiar", className: " btn-dark" },
            { extend: "excelHtml5", text: "<i class='fa fa-file-excel'></i> Excel", className: "btn-success" },
            { extend: "csvHtml5", text: "<i class='fa fa-file-csv'></i> CSV", className: "btn-success" },
            { extend: "pdfHtml5", text: "<i class='fa fa-file-pdf'></i> PDF", className: "btn-danger", pageSize: "C2", orientation: "landscape" },
            { extend: "print", text: "<i class='fa fa-print'></i> Imprimir", className: "btn-dark" }
        ],
        ajax: {
            url: "",
            data: function (d) {
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.terrainType = $("#terrain_type_filter").val();
                d.hasFor47 = $("#has_for_47_filter").is(":checked");
                delete d.columns;
            }
        },
        rowCallback: function (row, data) {
            if (data.addedLately) {
                $(row).css("background-color", "#f2d3cb");
            }
        },
        columns: [
            {
                title: "Frente (Fase del Sistema)",
                data: "sewerGroup.workFront.systemPhase.code"
            },
            {
                title: "Frente de Trabajo (delimitado)",
                data: "sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Código de Habilitación",
                data: "qualificationZone.code"
            },
            {
                title: "Habilitación",
                data: "qualificationZone.name"
            },
            {
                title: "Dirección",
                data: "address"
            },
            {
                title: "Área Drenaje",
                data: "initialSewerDrainageArea"
            },
            {
                title: "BZ (i)",
                data: "initialSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "initialSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "waterUpCover",
                render: _app.render.measure
            },
            {
                title: "Salida",
                data: "waterUpOutput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "waterUpBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "initialSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "initialSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "initialSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "initialSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "finalSewerDrainageArea"
            },
            {
                title: "BZ (j)",
                data: "finalSewerBoxCode"
            },
            {
                title: "h BZ",
                data: "finalSewerBoxHeight",
                render: _app.render.measure
            },
            {
                title: "Tapa",
                data: "downstreamCover",
                render: _app.render.measure
            },
            {
                title: "Entrada",
                data: "downstreamInput",
                render: _app.render.measure
            },
            {
                title: "Fondo",
                data: "downstreamBottom",
                render: _app.render.measure
            },
            {
                title: "TT",
                data: "finalSewerBoxTerrainType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.terrain.type.VALUES);
                }
            },
            {
                title: "Tipo Buzón",
                data: "finalSewerBoxType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.sewer.box.type.VALUES);
                }
            },
            {
                title: "Diámetro",
                data: "finalSewerBoxInternalDiameter",
                render: _app.render.measure
            },
            {
                title: "Espesor",
                data: "finalSewerBoxThickness",
                render: _app.render.measure
            },
            {
                title: "Área Drenaje",
                data: "drainageArea"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Dist. Horizontal / Ejes",
                data: "horizontalDistanceOnAxis",
                render: _app.render.measure
            },
            {
                title: "Profunfidad Tramo",
                data: "averageDepthSewerLine",
                render: _app.render.measure
            },
            {
                title: "Pendiente %o",
                data: "slope",
                render: _app.render.measure
                //render: _app.render.percent
            },
            {
                title: "DN (mm)",
                data: "nominalDiameter",
                render: _app.render.measure
            },
            {
                title: "Tipo Tubería",
                data: "pipelineType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.type.VALUES);
                }
            },
            {
                title: "Clase",
                data: "pipelineClass",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.pipeline.class.VALUES);
                }
            },
            {
                title: "N",
                data: "excavationLengthPercentForNormal",
                render: _app.render.percent
            },
            {
                title: "SR",
                data: "excavationLengthPercentForSemirocous",
                render: _app.render.percent
            },
            {
                title: "R",
                data: "excavationLengthPercentForRocky",
                render: _app.render.percent
            },
            {
                title: "Long. Tubería Instalada",
                data: "installedPipelineLength",
                render: _app.render.measure
            },
            {
                title: "N",
                data: "excavationLengthForNormal",
                render: _app.render.measure
            },
            {
                title: "SR",
                data: "excavationLengthForSemirocous",
                render: _app.render.measure
            },
            {
                title: "R",
                data: "excavationLengthForRocky",
                render: _app.render.measure
            }
        ]
    };

    var datatable = {
        init: function () {
            this.contractual.init();
            this.staking.init();
            this.real.init();
        },
        reload: function () {
            this.contractual.reload();
            this.staking.reload();
            this.real.reload();
        },
        adjust: function () {
            this.contractual.adjust();
            this.staking.adjust();
            this.real.adjust();
        },
        contractual: {
            init: function () {
                options1.ajax.url = _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/listar?stage=1");
                contractualDatatable = $("#contractual_datatable").DataTable(options1);
            },
            reload: function () {
                contractualDatatable.ajax.reload();
            },
            adjust: function () {
                contractualDatatable.columns.adjust().draw();
            }
        },
        staking: {
            init: function () {
                options2.ajax.url = _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/listar?stage=2");
                stakingDatatable = $("#staking_datatable").DataTable(options2);
            },
            reload: function () {
                stakingDatatable.ajax.reload();
            },
            adjust: function () {
                stakingDatatable.columns.adjust().draw();
            }
        },
        real: {
            init: function () {
                options3.ajax.url = _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/listar?stage=3");
                realDatatable = $("#real_datatable").DataTable(options3);
            },
            reload: function () {
                realDatatable.ajax.reload();
            },
            adjust: function () {
                realDatatable.columns.adjust().draw();
            }
        }
    };

    var select2 = {
        init: function () {
            this.workFronts.init();
            this.sewerGroups.init();
            this.qualificationZones.init();
            this.terrainTypes.init();
            this.sewerBoxTypes.init();
            this.pipelineTypes.init();
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
        qualificationZones: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/habilitaciones")
                }).done(function (result) {
                    $(".select2-qualification-zones").select2({
                        data: result,
                        placeholder: "Habilitación"
                    });
                });
            }
        },
        terrainTypes: {
            init: function () {
                $(".select2-terrain-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        sewerBoxTypes: {
            init: function () {
                $(".select2-sewer-box-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        pipelineTypes: {
            init: function () {
                $(".select2-pipeline-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        }
    };

    var events = {
        init: function () {
            $(".nav-tabs").on("shown.bs.tab", function () {
                datatable.adjust();
            });

            $(".btn-export").on("click", function () {
                var $btn = $(this);
                $btn.addLoader();
                $.fileDownload(_app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/exportar"))
                    .always(function () {
                        $btn.removeLoader();
                    }).done(function () {
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    }).fail(function () {
                        toastr.error("No se pudo descargar el archivo", "Error");
                    });
                return false;
            });

            $("#sewer_group_filter, #terrain_type_filter, #has_for_47_filter").on("change", function () {
                datatable.real.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            select2.init();
        }
    };
}();

$(function () {
    DrainageNetworkSummary.init();
});