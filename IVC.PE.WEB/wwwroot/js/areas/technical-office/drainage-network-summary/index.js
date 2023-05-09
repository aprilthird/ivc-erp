var DrainageNetworkSummary = function () {

    var contractualDatatable = null;
    var stakingDatatable = null;
    var realDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var addSewerBoxForm = null;
    var for47Form = null;

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
            },
            {
                title: "Revisado",
                data: "isReviewed",
                render: function (data, type, row) {
                    return `<span class="kt-switch kt-switch--icon kt-switch--outline kt-switch--primary"><label><input data-id="${row.id}" class="ch-review" type="checkbox" ${data ? "checked" : ""}><span></span></label></span>`;
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
            },
            {
                title: "Revisado",
                data: "isReviewed",
                render: function (data, type, row) {
                    return `<span class="kt-switch kt-switch--icon kt-switch--outline kt-switch--primary"><label><input data-id="${row.id}" class="ch-review" type="checkbox" ${data ? "checked" : ""}><span></span></label></span>`;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "<div class='row'>";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-add-sewer-box">`;
                    tmp += `<i class="fa fa-project-diagram"></i></button>`;
                    if (row.addedLately) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete-sewer-box">`;
                        tmp += `<i class="fa fa-times"></i></button>`;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-brand btn-sm btn-icon btn-for-47">`;
                    tmp += `<i class="fa fa-flask"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-warning btn-sm btn-icon btn-change-direction">`;
                    tmp += `<i class="fa fa-exchange-alt"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button></div>`;
                    return tmp;
                }
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
                this.initEvents();
            },
            reload: function () {
                contractualDatatable.ajax.reload();
            },
            adjust: function () {
                contractualDatatable.columns.adjust().draw();
            },
            initEvents: function () {
                contractualDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                contractualDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tramo y sus consolidados serán eliminados permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.contractual.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tramo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tramo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }   
        },
        staking: {
            init: function () {
                options2.ajax.url = _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/listar?stage=2");
                stakingDatatable = $("#staking_datatable").DataTable(options2);
                this.initEvents();
            },
            reload: function () {
                stakingDatatable.ajax.reload();
            },
            adjust: function () {
                stakingDatatable.columns.adjust().draw();
            },
            initEvents: function () {
                stakingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                stakingDatatable.on("change",
                    ".ch-review",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let checked = $(this).is(":checked");
                        $.ajax({
                            url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/editar-estado/${id}`),
                            type: "PUT",
                            data: {
                                IsReviewed: checked
                            }
                        }).done(function () {
                            _app.show.notification.add.success("El tramo ha sido actualizado con éxito.");
                            datatable.staking.reload();
                        }).fail(function () {
                            _app.show.notification.add.error();
                        });
                    });

                stakingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tramo y sus consolidados será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.contractual.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tramo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tramo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }   
        },
        real: {
            init: function () {
                options3.ajax.url = _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/listar?stage=3");
                realDatatable = $("#real_datatable").DataTable(options3);
                this.initEvents();
            },
            reload: function () {
                realDatatable.ajax.reload();
            },
            adjust: function () {
                realDatatable.columns.adjust().draw();
            },
            initEvents: function () {
                realDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                realDatatable.on("click",
                    ".btn-add-sewer-box",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.sewerBox.add(id);
                    });

                realDatatable.on("change",
                    ".ch-review",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let checked = $(this).is(":checked");
                        $.ajax({
                            url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/editar-estado/${id}`),
                            type: "PUT",
                            data: {
                                IsReviewed: checked
                            }
                        }).done(function () {
                            _app.show.notification.add.success("El tramo ha sido actualizado con éxito.");
                            datatable.real.reload();
                        }).fail(function () {
                            _app.show.notification.add.error();
                        });
                    });

                realDatatable.on("click",
                    ".btn-delete-sewer-box",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El buzón será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/anular-buzon/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El buzón ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el buzón"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                realDatatable.on("click",
                    ".btn-for-47",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.for47(id);
                    });

                realDatatable.on("click",
                    ".btn-change-direction",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "Se invertirá la dirección y la información de los buzones de aguas abajo y arriba.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, invertirlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/cambiar-direccion/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La dirección ha sido cambiada",
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
                                                text: "Ocurrió un error al intentar cambiar de dirección"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                realDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El tramo y sus consolidados será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El tramo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el tramo"
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
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerLineId']").val(result.sewerLineId);
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='QualificationZoneId']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='InitialSewerBoxCode']").val(result.initialSewerBoxCode);
                        formElements.find("[name='InitialSewerHalfCane']").val(result.initialSewerHalfCane);
                        formElements.find("[name='Address']").val(result.address);
                        formElements.find("[name='InitialSewerDrainageArea']").val(result.initialSewerDrainageArea);
                        formElements.find("[name='FinalSewerBoxCode']").val(result.finalSewerBoxCode);
                        formElements.find("[name='FinalSewerHalfCane']").val(result.finalSewerHalfCane);
                        formElements.find("[name='FinalSewerDrainageArea']").val(result.finalSewerDrainageArea);
                        formElements.find("[name='WaterUpCover']").val(result.waterUpCover);
                        formElements.find("[name='WaterUpOutput']").val(result.waterUpOutput);
                        formElements.find("[name='WaterUpBottom']").val(result.waterUpBottom);
                        formElements.find("[name='DownstreamCover']").val(result.downstreamCover);
                        formElements.find("[name='DownstreamInput']").val(result.downstreamInput);
                        formElements.find("[name='DownstreamBottom']").val(result.downstreamBottom);
                        formElements.find("[name='HorizontalDistanceOnAxis']").val(result.horizontalDistanceOnAxis);
                        formElements.find("[name='NominalDiameter']").val(result.nominalDiameter);
                        formElements.find("[name='PipelineType']").val(result.pipelineType).trigger("change");
                        formElements.find("[name='TerrainType']").val(result.terrainType).trigger("change");

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            sewerBox: {
                add: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#add_sewer_box_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='Code']").val(result.finalSewerBoxCode + "A");
                            $("#add_sewer_box_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            },
            for47: function (id) {
                let formElements = $("#for_47_form");
                formElements.find("[name='Id']").val(id);
                $("#for_47_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/consolidado-alcantarillado/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                    url: "/oficina-tecnica/consolidado-alcantarillado/importar",
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
                    $("#import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            },
            sewerBox: {
                add: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/agregar-buzon/${id}`),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatable.reload();
                            $("#add_sewer_box_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_sewer_box_alert_text").html(error.responseText);
                                $("#add_sewer_box_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            },
            for47: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/consolidado-alcantarillado/for-47/${id}`),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#for_47_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#for_47_alert_text").html(error.responseText);
                            $("#for_47_alert").removeClass("d-none").addClass("show");
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
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            sewerBox: {
                add: function () {
                    addSewerBoxForm.resetForm();
                    $("#add_sewer_box_form").trigger("reset");
                    $("#add_sewer_box_alert").removeClass("show").addClass("d-none");
                }
            },
            for47: function () {
                for47Form.resetForm();
                $("#for_47_form").trigger("reset");
                $("#for_47_alert").removeClass("show").addClass("d-none");
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

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            addSewerBoxForm = $("#add_sewer_box_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.sewerBox.add(formElement);
                }
            });

            for47Form = $("#for_47_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.for47(formElement);
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

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });

            $("#add_sewer_box_modal").on("hidden.bs.modal",
                function () {
                    form.reset.sewerBox.add();
                });

            $("#for_47_modal").on("hidden.bs.modal",
                function () {
                    form.reset.for47();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='SewerGroupId']").attr("id", "Add_SewerGroupId");
            $("#edit_form [name='SewerGroupId']").attr("id", "Edit_SewerGroupId");
            $("#add_form [name='QualificationZoneId']").attr("id", "Add_QualificationZoneId");
            $("#edit_form [name='QualificationZoneId']").attr("id", "Edit_QualificationZoneId");
            $("#add_form [name='TerrainType']").attr("id", "Add_TerrainType");
            $("#edit_form [name='TerrainType']").attr("id", "Edit_TerrainType");
            $("#add_form [name='PipelineType']").attr("id", "Add_PipelineType");
            $("#edit_form [name='PipelineType']").attr("id", "Edit_PipelineType");
            $("#add_form [name='InitialSewerBoxTerrainType']").attr("id", "Add_InitialSewerBoxTerrainType");
            $("#edit_form [name='InitialSewerBoxTerrainType']").attr("id", "Edit_InitialSewerBoxTerrainType");
            $("#add_form [name='InitialSewerBoxType']").attr("id", "Add_InitialSewerBoxType");
            $("#edit_form [name='InitialSewerBoxType']").attr("id", "Edit_InitialSewerBoxType");
            $("#add_form [name='FinalSewerBoxType']").attr("id", "Add_FinalSewerBoxType");
            $("#edit_form [name='FinalSewerBoxType']").attr("id", "Edit_FinalSewerBoxType");
            $("#add_form [name='FinalSewerBoxTerrainType']").attr("id", "Add_FinalSewerBoxTerrainType");
            $("#edit_form [name='FinalSewerBoxTerrainType']").attr("id", "Edit_FinalSewerBoxTerrainType");

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
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    DrainageNetworkSummary.init();
});