var ManifoldNetworkSummary = function () {

    var projectDatatable = null;
    var reviewDatatable = null;
    var executionDatatable = null;

    var proDtOption = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 5, 6, 10, 14, 15, 19, 20, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32],
                hide: [2, 3, 4, 7, 8, 9, 11, 12, 13, 16, 17, 18, 21, 26, 33]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: [0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33],
                hide: [7, 8, 9, 16, 17, 18]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 3',
                show: ':hidden'
            },
            {
                extend: "excelHtml5",
                text: "<i class='fa fa-file-excel'></i> Excel",
                className: "btn-success"
            }
        ],
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/colectores-descarga/proyecto"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "Dirección",
                data: "address"
            },
            {
                //title: "Nº",
                data: "startCode"
            },
            {
                //title: "Cota Tapa",
                data: "startCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada",
                data: "startArrivalLevelStr",
                visible: false
            },
            {
                //title: "Cota Fondo",
                data: "startBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ",
                data: "startHeightStr"
            },
            {
                //title: "TT",
                data: "startTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón",
                data: "startSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro",
                data: "startDiameterStr",
                visible: false
            },
            {
                //title: "Espesor",
                data: "startThicknessStr",
                visible: false
            },
            {
                //title: "Nº",
                data: "endCode"
            },
            {
                //title: "Cota Tapa",
                data: "endCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada",
                data: "endArrivalLevelStr",
                visible: false
            },
            {
                //title: "Cota Fondo",
                data: "endBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ",
                data: "endHeightStr"
            },
            {
                //title: "TT",
                data: "endTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón",
                data: "endSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro",
                data: "endDiameterStr",
                visible: false
            },
            {
                //title: "Espesor",
                data: "endThicknessStr",
                visible: false
            },
            {
                //title: "Nombre",
                data: "name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "h Zanja",
                data: "ditchHeightStr"
            },
            {
                //title: "%",
                data: "ditchLevelPercentStr",
                visible: false
            },
            {
                //title: "DN (mm)",
                data: "pipelineDiameter"
            },
            {
                //title: "Tipo de Tubería",
                data: "pipelineTypeStr"
            },
            {
                //title: "Clase",
                data: "pipelineClassStr"
            },
            {
                //title: "Long. Entre Ejes H",
                data: "lengthBetweenHAxlesStr"
            },
            {
                //title: "Long. Entre Ejes I",
                data: "lengthBetweenIAxlesStr",
                visible: false
            },
            {
                //title: "Long.Tubería Instalada",
                data: "lengthOfPipelineInstalledStr"
            },
            {
                //title: "TT",
                data: "terrainTypeStr"
            },
            {
                //title: "Long. Excavada",
                data: "lengthOfDiggingStr"
            },
            {
                //title: `Asfalto 2"(m2)`,
                data: "pavement2InStr"
            },
            {
                //title: `Asfalto 3"(m2)`,
                data: "pavement3InStr"
            },
            {
                //title: `Asfalto 3" Mixto(m2)`,
                data: "pavement3InMixedStr"
            },
            {
                //title: "Ancho",
                data: "pavementWidthStr",
                visible: false
            }
        ],
        "columnDefs": [
            { className: 'dt-body-right', 'targets': [2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 17, 18, 20, 21, 22, 25, 26, 27, 29, 30, 31, 32, 33] }
        ]
    };

    var rewDtOption = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 5, 6, 10, 14, 15, 19, 20, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32],
                hide: [2, 3, 4, 7, 8, 9, 11, 12, 13, 16, 17, 18, 21, 26, 33]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: [0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33],
                hide: [7, 8, 9, 16, 17, 18]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 3',
                show: ':hidden'
            },
            {
                extend: "excelHtml5",
                text: "<i class='fa fa-file-excel'></i> Excel",
                className: "btn-success"
            }
        ],
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/colectores-descarga/replanteo"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "Dirección",
                data: "address"
            },
            {
                //title: "Nº",
                data: "startCode"
            },
            {
                //title: "Cota Tapa",
                data: "startCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada",
                data: "startArrivalLevelStr",
                visible: false
            },
            {
                //title: "Cota Fondo",
                data: "startBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ",
                data: "startHeightStr"
            },
            {
                //title: "TT",
                data: "startTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón",
                data: "startSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro",
                data: "startDiameterStr",
                visible: false
            },
            {
                //title: "Espesor",
                data: "startThicknessStr",
                visible: false
            },
            {
                //title: "Nº",
                data: "endCode"
            },
            {
                //title: "Cota Tapa",
                data: "endCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada",
                data: "endArrivalLevelStr",
                visible: false
            },
            {
                //title: "Cota Fondo",
                data: "endBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ",
                data: "endHeightStr"
            },
            {
                //title: "TT",
                data: "endTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón",
                data: "endSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro",
                data: "endDiameterStr",
                visible: false
            },
            {
                //title: "Espesor",
                data: "endThicknessStr",
                visible: false
            },
            {
                //title: "Nombre",
                data: "name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "h Zanja",
                data: "ditchHeightStr"
            },
            {
                //title: "%",
                data: "ditchLevelPercentStr",
                visible: false
            },
            {
                //title: "DN (mm)",
                data: "pipelineDiameter"
            },
            {
                //title: "Tipo de Tubería",
                data: "pipelineTypeStr"
            },
            {
                //title: "Clase",
                data: "pipelineClassStr"
            },
            {
                //title: "Long. Entre Ejes H",
                data: "lengthBetweenHAxlesStr"
            },
            {
                //title: "Long. Entre Ejes I",
                data: "lengthBetweenIAxlesStr",
                visible: false
            },
            {
                //title: "Long.Tubería Instalada",
                data: "lengthOfPipelineInstalledStr"
            },
            {
                //title: "TT",
                data: "terrainTypeStr"
            },
            {
                //title: "Long. Excavada",
                data: "lengthOfDiggingStr"
            },
            {
                //title: `Asfalto 2"(m2)`,
                data: "pavement2InStr"
            },
            {
                //title: `Asfalto 3"(m2)`,
                data: "pavement3InStr"
            },
            {
                //title: `Asfalto 3" Mixto(m2)`,
                data: "pavement3InMixedStr"
            },
            {
                //title: "Ancho",
                data: "pavementWidthStr",
                visible: false
            },
            {
                //title: "cartas solicitud",
                data: "hasRequestLetters",
                render: function (data, type, row) {
                    var tmp = ``;
                    if (data) {
                        tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-req-view" data-toggle="tooltip" title="Solicitudes">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "cartas aprobacion",
                data: "hasApprovalLetters",
                render: function (data, type, row) {
                    var tmp = ``;
                    if (data) {
                        tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-app-view" data-toggle="tooltip" title="Aprobaciones">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: 'dt-body-right', 'targets': [2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 17, 18, 20, 21, 22, 25, 26, 27, 29, 30, 31, 32, 33] }
        ]
    };

    var exeDtOption = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 4, 8, 9, 13, 17, 18, 22, 23, 25, 26, 27, 28],
                hide: [5, 6, 7, 10, 11, 12, 14, 15, 16, 19, 20, 21, 24, 29, 30, 31, 32, 33, 34]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 14, 15, 16, 17, 18, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34],
                hide: [10, 11, 12, 19, 20, 21]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 3',
                show: ':hidden'
            },
            {
                extend: "excelHtml5",
                text: "<i class='fa fa-file-excel'></i> Excel",
                className: "btn-success"
            }
            //,{
            //    text: "<i class='fa fa-plus'></i> Actualizar",
            //    className: "btn-primary",
            //    action: function (e, dt, node, config) {
            //        _app.loader.show();
            //        $.ajax({
            //            url: _app.parseUrl("/oficina-tecnica/colectores-descarga/ejecucion/actualizar")
            //        })
            //            .always(function () {
            //                _app.loader.hide();
            //            })
            //            .done(function (result) {
            //                swal.fire({
            //                    type: "success",
            //                    title: "Completado",
            //                    confirmButtonClass: "btn-info",
            //                    animation: false,
            //                    customClass: 'animated tada',
            //                    confirmButtonText: "Entendido",
            //                    text: error.responseText
            //                });
            //            })
            //            .fail(function (error) {
            //                swal.fire({
            //                    type: "error",
            //                    title: "Error",
            //                    confirmButtonClass: "btn-danger",
            //                    animation: false,
            //                    customClass: 'animated tada',
            //                    confirmButtonText: "Entendido",
            //                    text: error.responseText
            //                });
            //            });
            //    }
            //}
        ],
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/colectores-descarga/ejecucion"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "Jefe de Frente", 0
                data: "workFrontHead"
            },
            { 
                //title: "Frente", 1
                data: "workFront"
            },
            {
                //title: "Cuadrilla", 2
                data: "sewerGroup"
            },
            {
                //title: "Dirección", 3
                data: "address"
            },
            {
                //title: "Nº", 4
                data: "startCode"
            },
            {
                //title: "Cota Tapa", 5
                data: "startCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada", 6
                data: "startArrivalLevelStr",
                visible: false
            },
            {
                //title: "Cota Fondo", 7
                data: "startBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ", 8
                data: "startHeightStr"
            },
            {
                //title: "TT", 9
                data: "startTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón", 10
                data: "startSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro", 11
                data: "startDiameterStr",
                visible: false
            },
            {
                //title: "Espesor", 12
                data: "startThicknessStr",
                visible: false
            },
            {
                //title: "Nº", 13
                data: "endCode"
            },
            {
                //title: "Cota Tapa", 14
                data: "endCoverLevelStr",
                visible: false
            },
            {
                //title: "Cota Llegada", 15
                data: "endArrivalLevelStr",
                visible: false
            },
            { 
                //title: "Cota Fondo", 16
                data: "endBottomLevelStr",
                visible: false
            },
            {
                //title: "h BZ", 17
                data: "endHeightStr"
            },
            {
                //title: "TT", 18
                data: "endTerrainTypeStr"
            },
            {
                //title: "Tipo de Buzón", 19
                data: "endSewerBoxTypeStr",
                visible: false
            },
            {
                //title: "Diámetro", 20
                data: "endDiameterStr",
                visible: false
            },
            {
                //title: "Espesor", 21
                data: "endThicknessStr",
                visible: false
            },
            {
                //title: "Nombre", 22
                data: "name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "h Zanja", 23
                data: "ditchHeightStr"
            },
            {
                //title: "%", 24
                data: "ditchLevelPercentStr",
                visible: false
            },
            {
                //title: "DN (mm)", 25
                data: "pipelineDiameter"
            },
            {
                //title: "Tipo de Tubería", 26
                data: "pipelineTypeStr"
            },
            {
                //title: "Clase", 27
                data: "pipelineClassStr"
            },
            {
                //title: "Long. Entre Ejes H", 28
                data: "lengthBetweenHAxlesStr"
            },
            {
                //title: "Long. Entre Ejes I", 29
                data: "lengthBetweenIAxlesStr",
                visible: false
            },
            {
                //title: "Long.Tubería Instalada", 30
                data: "lengthOfPipelineInstalledStr",
                visible: false
            },
            {
                //title: "Long. Excavación", 31
                data: "lengthOfDiggingStr",
                visible: false
            },
            {
                //title: "Tipo de Terreno % N", 32
                data: "lengthOfDiggingNPercentStr",
                visible: false,
                render: function (data, type, row) {
                    if (data == "") {
                        if (row.terrainTypeStr == "N")
                            return "100.00";
                        else
                            return "0.00";
                    }
                    return data;
                }
            },
            {
                //title: "Tipo de Terreno % SR", 33
                data: "lengthOfDiggingSRPercentStr",
                visible: false,
                render: function (data, type, row) {
                    if (data == "") {
                        if (row.terrainTypeStr == "SR")
                            return "100.00";
                        else
                            return "0.00";
                    }
                    return data;
                }
            },
            {
                //title: "Tipo de Terreno % R", 34
                data: "lengthOfDiggingRPercentStr",
                visible: false,
                render: function (data, type, row) {
                    if (data == "") {
                        if (row.terrainTypeStr == "R")
                            return "100.00";
                        else
                            return "0.00";
                    }
                    return data;
                }
            },
            {
                //title: "Tipo de Terreno N", 35
                data: "lengthOfDiggingNPercentStr",
                render: function (data, type, row) {
                    if(data == "") {
                        if (row.terrainTypeStr == "N")
                            return row.lengthOfDiggingStr;
                        else
                            return "0.00";
                    }
                    var result = row.lengthOfDigging * row.lengthOfDiggingNPercent / 100.0;
                    return `${result.toFixed(2)}`;
                }
            },
            {
                //title: "Tipo de Terreno SR", 36
                data: "lengthOfDiggingSRPercentStr",
                render: function (data, type, row) {
                    if (data == "") {
                        if (row.terrainTypeStr == "SR")
                            return row.lengthOfDiggingStr;
                        else
                            return "0.00";
                    }
                    var result = row.lengthOfDigging * row.lengthOfDiggingSRPercent / 100.0;
                    return `${result.toFixed(2)}`;
                }
            },
            {
                //title: "Tipo de Terreno R", 37
                data: "lengthOfDiggingRPercentStr",
                render: function (data, type, row) {
                    if (data == "") {
                        if (row.terrainTypeStr == "R")
                            return row.lengthOfDiggingStr;
                        else
                            return "0.00";
                    }
                    var result = row.lengthOfDigging * row.lengthOfDiggingRPercent / 100.0;
                    return `${result.toFixed(2)}`;
                }
            },
            {
                //title: "For-37A", 38
                data: "for37aFileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-forname="For37A del Tramo " data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="For37A">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "For-47", 39
                data: "for47FileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-forname="For47 del Tramo " data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="For47">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "For-01", 40
                data: "for01FileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-forname="For01 del Tramo " data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="For01">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "For-05", 41
                data: "for05FileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-forname="For05 del Tramo " data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="For05">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "For-29", 42
                data: "for29FileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-forname="For29 del Tramo " data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="For29">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Espejo", 43
                data: "for01MirrorTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-test="Espejo" data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-video" data-toggle="tooltip" title="For01-Espejo">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Bola/Mandril", 44
                data: "for01MonkeyBallTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-test="Bola/Mandril" data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-video" data-toggle="tooltip" title="For01-Bola Mandril">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Cámara Zoom", 45
                data: "for01ZoomTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-file="${data}" data-test="Cámara Zoom" data-name="${row.name}" class="btn btn-secondary btn-sm btn-icon btn-video" data-toggle="tooltip" title="Zoom">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Fotos", 46
                data: null,
                render: function (data, type, row) {
                    return `En Desarrollo`;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.proDt.init();
            this.rewDt.init();
            this.exeDt.init();
        },
        proDt: {
            init: function () {
                projectDatatable = $("#project_datatable").DataTable(proDtOption);
            }
        },
        rewDt: {
            init: function () {
                reviewDatatable = $("#review_datatable").DataTable(rewDtOption);
                this.events();
            },
            reload: function () {
                reviewDatatable.ajax.reload();
            },
            events: function () {
                reviewDatatable.on("click",
                    ".btn-req-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        select2.smLetters.reloadRequest(id);
                        $("#review_letter_pdf_modal").modal("show");
                    });

                reviewDatatable.on("click",
                    ".btn-app-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        select2.smLetters.reloadApproval(id);
                        $("#review_letter_pdf_modal").modal("show");
                    });
            }
        },
        exeDt: {
            init: function () {
                executionDatatable = $("#execution_datatable").DataTable(exeDtOption);
                this.events();
            },
            events: function () {
                executionDatatable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let forName = $btn.data("forname");
                        let smName = $btn.data("name");
                        let fileUrl = $btn.data("file");
                        forms.load.pdf(fileUrl, `${forName} - ${smName}`);

                    });
                executionDatatable.on("click",
                    ".btn-video",
                    function () {
                        let $btn = $(this);
                        let testName = $btn.data("test");
                        let smName = $btn.data("name");
                        let fileUrl = $btn.data("file");
                        forms.load.video(fileUrl, `${testName} - ${smName}`);
                    });
            }
        }
    };

    var forms = {
        load: {
            pdf: function (fileUrl, forName) {
                console.log(fileUrl);
                console.log(forName);
                loadPdf(forName, fileUrl, "for_pdf_views");
                $("#for_pdf_modal").modal("show");
            },
            video: function (fileUrl, testName) {
                $("#test_name").html(testName);
                $("#video_frame").prop("src", fileUrl);
                $("#video_modal").modal("show");
            }
        },
        reset: {
            video: function () {
                $("#video_frame").prop("src", "");
            }
        }
    };

    var modals = {
        init: function () {
            $("#video_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.video();
                });
        }
    };

    var select2 = {
        init: function () {
            this.smLetters.init();
        },
        smLetters: {
            init: function () {
                $(".select2-sm-letters").select2();
            },
            reloadRequest: function (id) {
                $("#letters_type").html("Cartas de Solicitud");
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-carta-solicitudes?smId=${id}`)
                }).done(function (result) {
                    $(".select2-sm-letters").empty();
                    $(".select2-sm-letters").select2({
                        data: result
                    });
                    $(".select2-sm-letters").trigger("change");
                });
            },
            reloadApproval: function (id) {
                $("#letters_type").html("Cartas de Aprobación");
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-carta-aprobaciones?smId=${id}`)
                }).done(function (result) {
                    $(".select2-sm-letters").empty();
                    $(".select2-sm-letters").select2({
                        data: result
                    });
                    $(".select2-sm-letters").trigger("change");
                });
            }
        }
    };

    var events = {
        init: function () {
            $("#review_letter_form [name='RequestLetterIds']").attr("id", "Add_RequestLetterIds");
            $("#review_letter_form [name='ApprovalLetterIds']").attr("id", "Add_ApprovalLetterIds");

            $("#review_letter_edit_form [name='RequestLetterIds']").attr("id", "Edit_RequestLetterIds");
            $("#review_letter_edit_form [name='ApprovalLetterIds']").attr("id", "Edit_ApprovalLetterIds");

            $(".select2-sm-letters").on("change", function () {
                _app.loader.show();
                let id = this.value;
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/${id}`)
                }).done(function (result) {
                    loadPdf(result.name, result.fileUrl, "letter_pdf_views");
                }).always(function () {
                    _app.loader.hide();
                });
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            modals.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    ManifoldNetworkSummary.init();
});