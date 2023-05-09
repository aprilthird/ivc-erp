var Pdp = function () {

    var selectSMOption = new Option('--Seleccione un Tramo--', '', true, true);
    var selectWFHOption = new Option('--Seleccione un Jefe de Frente--', '', true, true);
    var selectWFOption = new Option('--Seleccione un Frente de Trabajo--', '', true, true);
    var selectSGOption = new Option('--Seleccione una Cuadrilla--', '', true, true);
    var selectPFOption = new Option('--Seleccione una Formula--', '', true, true);

    var pdpDatatable = null;

    var aux = null;

    var addForm = null;
    var editForm = null;
    var shedulerForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;

    var resultados = null;
    var value1s = [];
    var value2s = [];
    var value3s = [];
    var value4s = [];
    var sources = [];

    var sewergroup = null;

    var cant = null;

    var f7Id = null;

    var pdpDtOpts = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/f7-pdp/listar"),
            data: function (d) {
                d.sewerGroupId = $("#SewerGroup").val();
                d.workFrontHeadId = $("#WorkFrontHead").val();
                d.status = $("#Status").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 11, 12, 13, 14, 15, 16, 17, 18, 19],
                hide: [3, 4, 5, 6, 7, 8, 9, 10, 20, 21, 22]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            },
            {
                extend: 'copy',
                className: "btn-dark",
                text: "<i class='fa fa-copy'></i> Copiar"
            },
            {
                extend: 'excel',
                className: "btn-success",
                text: "<i class='fa fa-file-excel'></i> Excel"
            },
            {
                extend: 'csv',
                className: "btn-success",
                text: "<i class='fa fa-file-csv'></i> CSV"
            },
            {
                extend: 'pdf',
                className: "btn-danger",
                text: "<i class='fa fa-file-pdf'></i> PDF"
            },
            {
                extend: 'print',
                className: "btn-dark",
                text: "<i class='fa fa-print'></i> Imprimir"
            },
        ],
        columns: [
            {
                //title: "Jefe de Frente",
                data: "workFrontHead.user.auxFullName"
            },
            {
                //title: "Cuadrilla",
                data: "sewerGroup.code"
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
                //title: "Long. Excavación",
                data: "sewerManifold.lengthOfDigging",
                visible: false
            },
            {
                //title: "Long. Tuberia Instalada",
                data: "sewerManifold.lengthOfPipeInstalled",
                visible: false
            },
            {
                //title: "Altura del buzon aguas arriba",
                data: function (result) {
                    return result.sewerManifold.sewerBoxStart.height.toFixed(2);
                },
                visible: false
            },
            {
                //title: "Altura del buzon aguas abajo",
                data: function (result) {
                    return result.sewerManifold.sewerBoxEnd.height.toFixed(2);
                },
                visible: false
            },
            {
                //title: "Altura de la zanja",
                data: "sewerManifold.ditchHeight",
                visible: false
            },
            {
                //title: "H. Relleno (m)",
                data: "filling",
                visible: false
            },
            {
                //title: "# de Capa",
                data: "theoreticalLayer",
                visible: false
            },
            {
                //title: "Long. de relleno",
                data: "fillLength",
                visible: false
            },
            {
                //title: "Long. Excavada",
                data: function (result) {
                    return result.excavatedLength.toFixed(2);
                }
            },
            {
                //title: "Long. Instalada",
                data: function (result) {
                    return result.installedLength.toFixed(2);
                }
            },
            {
                //title: "Long. Rellenada",
                data: function (result) {
                    return result.refilledLength.toFixed(2);
                }
            },
            {
                //title: "Long. Base Granular",
                data: function (result) {
                    return result.granularBaseLength.toFixed(2);
                }
            },
            {
                //title: "Estado",
                data: "status"
            },
            {
                //title: "Long. Excavada",
                data: function (result) {
                    return result.excavatedLengthToExecute;
                }
            },
            {
                //title: "Long. Instalada",
                data: function (result) {
                    return result.installedLengthToExecute;
                }
            },
            {
                //title: "Long. Rellenada",
                data: function (result) {
                    return result.refilledLengthToExecute;
                }
            },
            {
                //title: "Long. Base Granular",
                data: function (result) {
                    return result.granularBaseLengthToExecute;
                }
            },
            {
                //title: "Excavacion",
                data: function (result) {
                    return result.excavation.toFixed(2);
                },
                visible: false
            },
            {
                //title: "Instalacion",
                data: function (result) {
                    return result.installation.toFixed(2);
                },
                visible: false
            },
            {
                //title: "Relleno",
                data: function (result) {
                    return result.filled.toFixed(2);
                },
                visible: false
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    if (row.granularBaseLength < row.sewerManifold.lengthOfDigging || row.excavatedLength < row.sewerManifold.lengthOfDigging || row.installedLength < row.sewerManifold.lengthOfPipeInstalled || row.refilledLength < row.fillLength) {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18, 19, 20, 21, 22] }
        ]
    };

    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/listar`),
            data: function (d) {
                d.ProductionDailyPartId = f7Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fecha",
                data: "date",
                orderable: true,
                "sType": "date-uk"
            },
            {
                title: "Long. Excavada",
                data: function (result) {
                    var res = parseFloat(result.excavatedLength).toFixed(2);
                    if (res == 0)
                        return "";
                    else
                        return res;
                }
            },
            {
                title: "Long. Instalada",
                data: function (result) {
                    var res = parseFloat(result.installedLength).toFixed(2);
                    if (res == 0)
                        return "";
                    else
                        return res;
                }
            },
            {
                title: "Longitud Rellenada",
                data: function (result) {
                    var res = parseFloat(result.refilledLength).toFixed(2);
                    if (res == 0)
                        return "";
                    else
                        return res;
                }
            },
            {
                title: "Long. Base Granular",
                data: function (result) {
                    var res = parseFloat(result.granularBaseLength).toFixed(2);
                    if (res == 0)
                        return "";
                    else
                        return res;
                }
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

    var datatables = {
        init: function () {
            this.pdpDt.init();
            this.foldingDt.init();
        },
        pdpDt: {
            init: function () {
                pdpDatatable = $("#pdps_datatable").DataTable(pdpDtOpts);
                this.events();
            },
            reload: function () {
                pdpDatatable.ajax.reload();
            },
            events: function () {

                pdpDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        f7Id = id;
                        datatables.foldingDt.reload();
                        forms.load.detail(id);
                    });

                pdpDatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        f7Id = id;
                        datatables.foldingDt.reload();
                        forms.load.foldingF7(id);
                    });

                pdpDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                pdpDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Parte Diario de Producción será eliminado permanentemente.",
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
                                        url: _app.parseUrl(`/oficina-tecnica/f7-pdp/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.pdpDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Parte Diario de Producción ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Parte Diario de Producción."
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
            },
            events: function () {
                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        aux = $btn.data("id");
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
                            text: "El Folding F7 será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            $.ajax({
                                                url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/actualizar/${f7Id}`),
                                                method: "put",
                                                contentType: false,
                                                processData: false
                                            })
                                                .done(function (result) {
                                                    datatables.pdpDt.reload();
                                                });
                                            datatables.foldingDt.reload();
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
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/f7-pdp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_formula']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_formula']").attr("disabled", "disabled");
                        formElements.find("[name='ReportDate']").val(result.reportDate);
                        formElements.find("[name='WorkFrontHeadId']").val(result.workFrontHeadId);
                        formElements.find("[name='select_workfronthead']").val(result.workFrontHeadId).trigger("change");
                        formElements.find("[name='WorkFrontId']").val(result.workFrontId);
                        formElements.find("[name='select_workfront']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        select2.sewergroups.edit(result.workFrontHeadId, result.sewerGroupId);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_sewermanifold']").val(result.sewerManifoldId).trigger("change");
                        formElements.find("[name='select_sewermanifold']").attr("disabled", "disabled");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/f7-pdp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='WorkFrontHead.User.AuxFullName']").val(result.workFrontHead.user.auxFullName);
                        formElements.find("[name='SewerGroup.Code']").val(result.sewerGroup.code);
                        formElements.find("[name='SewerManifold.Name']").val(result.sewerManifold.name);
                        formElements.find("[name='ExcavatedLength']").val(result.excavatedLength.toFixed(2));
                        formElements.find("[name='InstalledLength']").val(result.installedLength.toFixed(2));
                        formElements.find("[name='RefilledLength']").val(result.refilledLength.toFixed(2));
                        formElements.find("[name='GranularBaseLength']").val(result.granularBaseLength.toFixed(2));

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingF7: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/f7-pdp/${id}`)
                })
                    .done(function (result) {
                        console.log("Aquí");
                        console.log(result);
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='ProductionDailyPartId']").val(result.id);
                        formElements.find("#Tramo").val(result.sewerManifold.name);
                        formElements.find("#Excavated").val((parseFloat(result.sewerManifold.lengthOfDigging) - parseFloat(result.excavatedLength)).toFixed(2));
                        formElements.find("#Installed").val((parseFloat(result.sewerManifold.lengthOfPipeInstalled) - parseFloat(result.installedLength)).toFixed(2));
                        formElements.find("#Refilled").val((parseFloat(result.fillLength) - parseFloat(result.refilledLength)).toFixed(2));
                        formElements.find("#GranularBase").val((parseFloat(result.sewerManifold.lengthOfDigging) - parseFloat(result.granularBaseLength)).toFixed(2));

                        formElements.find("#ExcavatedTotal").val(parseFloat(result.sewerManifold.lengthOfDigging).toFixed(2));
                        formElements.find("#InstalledTotal").val(parseFloat(result.sewerManifold.lengthOfPipeInstalled).toFixed(2));
                        formElements.find("#RefilledTotal").val(parseFloat(result.fillLength).toFixed(2));
                        formElements.find("#GranularBaseTotal").val(parseFloat(result.sewerManifold.lengthOfDigging).toFixed(2));

                        if (formElements.find("#Excavated").val() == 0) {
                            formElements.find("[name='ExcavatedLength']").val(0).prop("disabled", true);
                        }
                        else
                            formElements.find("[name='ExcavatedLength']").prop("disabled", false);

                        if (formElements.find("#Installed").val() == 0) {
                            formElements.find("[name='InstalledLength']").val(0).prop("disabled", true);
                        }
                        else
                            formElements.find("[name='InstalledLength']").prop("disabled", false);

                        if (formElements.find("#Refilled").val() == 0) {
                            formElements.find("[name='RefilledLength']").val(0).prop("disabled", true);
                        }
                        else
                            formElements.find("[name='RefilledLength']").prop("disabled", false);

                        if (formElements.find("#GranularBase").val() == 0) {
                            formElements.find("[name='GranularBaseLength']").val(0).prop("disabled", true);
                        }
                        else
                            formElements.find("[name='GranularBaseLength']").prop("disabled", false);


                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProductionDailyPartId']").val(result.productionDailyPartId);
                        formElements.find("[name='GranularBaseLength']").val(parseFloat(result.granularBaseLength).toFixed(2));
                        formElements.find("[name='ExcavatedLength']").val(parseFloat(result.excavatedLength).toFixed(2));
                        formElements.find("[name='InstalledLength']").val(parseFloat(result.installedLength).toFixed(2));
                        formElements.find("[name='RefilledLength']").val(parseFloat(result.refilledLength).toFixed(2));
                        formElements.find("[name='Date']").datepicker("setDate", result.date);

                        $.ajax({
                            url: _app.parseUrl(`/oficina-tecnica/f7-pdp/${result.productionDailyPartId}`),
                            dataSrc: ""
                        })
                            .done(function (result) {
                                console.log("aqui");
                                console.log(result);
                                formElements.find("#Tramo").val(result.sewerManifold.name);
                                formElements.find("#Excavated").val((parseFloat(result.sewerManifold.lengthOfDigging) - parseFloat(result.excavatedLength)).toFixed(2));
                                formElements.find("#Installed").val((parseFloat(result.sewerManifold.lengthOfPipeInstalled) - parseFloat(result.installedLength)).toFixed(2));
                                formElements.find("#Refilled").val((parseFloat(result.fillLength) - parseFloat(result.refilledLength)).toFixed(2));
                                formElements.find("#GranularBase").val((parseFloat(result.sewerManifold.lengthOfDigging) - parseFloat(result.granularBaseLength)).toFixed(2));

                                formElements.find("#ExcavatedTotal").val(parseFloat(result.sewerManifold.lengthOfDigging).toFixed(2));
                                formElements.find("#InstalledTotal").val(parseFloat(result.sewerManifold.lengthOfPipeInstalled).toFixed(2));
                                formElements.find("#RefilledTotal").val(parseFloat(result.fillLength).toFixed(2));
                                formElements.find("#GranularBaseTotal").val(parseFloat(result.sewerManifold.lengthOfDigging).toFixed(2));
                            });

                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_workfronthead']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_workfront']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_sewermanifold']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/f7-pdp/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.pdpDt.reload();
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
                $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_workfronthead']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_workfront']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/f7-pdp/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.pdpDt.reload();
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
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $("#Excavated").prop("disabled", true);
                        $("#Installed").prop("disabled", true);
                        $("#Refilled").prop("disabled", true);
                        $("#GranularBase").prop("disabled", true);
                        $("#ExcavatedTotal").prop("disabled", true);
                        $("#InstalledTotal").prop("disabled", true);
                        $("#RefilledTotal").prop("disabled", true);
                        $("#GranularBaseTotal").prop("disabled", true);
                    })
                    .done(function (result) {
                        $.ajax({
                            url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/actualizar/${f7Id}`),
                            method: "put",
                            contentType: false,
                            processData: false
                        })
                            .done(function (result) {
                                datatables.pdpDt.reload();
                            });
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
                    url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(formElement).find("#Excavated").prop("disabled", true);
                        $(formElement).find("#Installed").prop("disabled", true);
                        $(formElement).find("#Refilled").prop("disabled", true);
                        $(formElement).find("#GranularBase").prop("disabled", true);
                        $(formElement).find("#ExcavatedTotal").prop("disabled", true);
                        $(formElement).find("#InstalledTotal").prop("disabled", true);
                        $(formElement).find("#RefilledTotal").prop("disabled", true);
                        $(formElement).find("#GranularBaseTotal").prop("disabled", true);
                    })
                    .done(function (result) {
                        $.ajax({
                            url: _app.parseUrl(`/oficina-tecnica/folding-f7-pdp/actualizar/${f7Id}`),
                            method: "put",
                            contentType: false,
                            processData: false
                        })
                            .done(function (result) {
                                datatables.pdpDt.reload();
                            });
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
            sheduler: function (formElement) {
                $(formElement).find("[name='SewerGroupShedulers']").append($(formElement).find("[name='select_sheduler_sewergroups']").val());
                let formElements = $("#sheduler_form");
                console.log($(formElement).find("[name='SewerGroupShedulers']").val());
                console.log($(formElement).find("[name='StartDate']").val());
                console.log($(formElement).find("[name='EndDate']").val());
                let data = new FormData($(formElement).get(0));
                console.log(data);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/f7-pdp/listar-scheduler"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                }).done(function (result) {
                    sources = [];
                    result.forEach(function (f7) {
                        f7.folding.forEach(function (folding) {
                            var fecha = new Date(folding.calendarDate);
                            if (folding.excavatedLength != "0")
                                value1s.push({
                                    from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    label: folding.excavatedLength,
                                    desc: "Something",
                                    customClass: "ganttRed",
                                    dataObj: folding.excavatedLength
                                });
                            if (folding.installedLength != "0")
                                value2s.push({
                                    from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    label: folding.installedLength,
                                    desc: "Something",
                                    customClass: "ganttGreen",
                                    dataObj: folding.installedLength
                                });
                            if (folding.refilledLength != "0")
                                value3s.push({
                                    from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    label: folding.refilledLength,
                                    desc: "Something",
                                    customClass: "ganttBlue",
                                    dataObj: folding.refilledLength
                                });
                            if (folding.granularBaseLength != "0")
                                value4s.push({
                                    from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
                                    label: folding.granularBaseLength,
                                    desc: "Something",
                                    customClass: "ganttOrange",
                                    dataObj: folding.granularBaseLength
                                });
                        });
                        sources.push(
                            {
                                name: f7.sewerManifold.name,
                                desc: "# Capas: " + f7.theoreticalLayer,
                                values: []
                            },
                            {
                                name: "L. Exca. : " + f7.sewerManifold.lengthOfDigging,
                                desc: "Excavada",
                                values: value1s
                            },
                            {
                                name: "L. Instal. : " + f7.sewerManifold.lengthOfPipeInstalled,
                                desc: "Instalada",
                                values: value2s
                            },
                            {
                                name: "H. Zanja: " + f7.sewerManifold.ditchHeight,
                                desc: "Rellenada",
                                values: value3s
                            },
                            {
                                name: "H. Relleno: " + f7.filling,
                                desc: "Base Granular",
                                values: value4s
                            },
                            {
                                name: "",
                                desc: "",
                                values: []
                            }
                        );
                        value1s = [];
                        value2s = [];
                        value3s = [];
                        value4s = [];
                    });
                    console.log(sources);
                    //Número de tramos * Número de filas por tramo
                    cant = 6 * 6;
                    console.log(cant);
                    $("#gant").gantt({
                        source: sources,
                        months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Augosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        dow: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        navigate: "scroll",
                        maxScale: "days",
                        minScale: "days",
                        itemsPerPage: cant,
                        onItemClick: function (data) {
                            alert(data);
                            console.log($(".dataPanel").width());
                        },
                        onAddClick: function (dt, rowId) {
                            alert("Empty space clicked - add an item!");
                        },
                        onRender: function () {
                            console.log("aki");
                            $(".bar").width("26px");
                            $(".spacer").text("TRAMOS");
                            $(".fn-label").css("fontSize", 13);
                        }
                    });
                });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
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
            sheduler: function () {
                shedulerForm.reset();
                $(".select2-sheduler-sewergroups").val('').trigger("change");
                $("#sheduler_form").trigger("reset");
                $("#sheduler_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
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

            $("#sheduler_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.sheduler();
                });
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

            shedulerForm = $("#sheduler_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.sheduler(formElement);
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.workfronts.init();
            this.workfrontheads.init();
            this.sewergroups.init();
            this.sewermanifolds.init();
        },
        formulas: {
            init: function () {
                $(".select2-formulas").select2();
            }
        },
        workfronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/frentes`)
                }).done(function (result) {
                    $(".select2-workfronts").append(selectWFOption).trigger('change');
                    $(".select2-workfronts").select2({
                        data: result
                    });
                })
            }
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-workfrontheads").append(selectWFHOption);
                    $(".select2-workfrontheads").select2({
                        data: result
                    });
                });
                $.ajax({
                    url: _app.parseUrl("/select/pdpf7-jefes-de-frente")
                }).done(function (result) {
                    $(".select2-filter-workfrontheads").select2({
                        data: result
                    });
                });
                $("#WorkFrontHead").on("change", function () {
                    datatables.pdpDt.reload();
                });
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
                $(".select2-sewergroups").append(selectSGOption);
                $.ajax({
                    url: _app.parseUrl("/select/pdpf7-cuadrillas")
                }).done(function (result) {
                    $(".select2-filter-sewergroups").select2({
                        data: result
                    });
                    $(".select2-sheduler-sewergroups").select2({
                        data: result
                    });
                });
                $("#SewerGroup").on("change", function () {
                    datatables.pdpDt.reload();
                });
                $("#Status").on("change", function () {
                    datatables.pdpDt.reload();
                });
            },
            reload: function (id) {
                let wfh = id;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            },
            edit: function (wfhId, sgId) {
                let wfh = wfhId;
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption);
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                    $(".select2-sewergroups").val(sgId).trigger('change');
                });
            }
        },
        sewermanifolds: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga`)
                }).done(function (result) {
                    $(".select2-sewermanifolds").append(selectSMOption).trigger('change');
                    $(".select2-sewermanifolds").select2({
                        data: result
                    });
                });
            }
        },
        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/formulas-proyecto`)
                }).done(function (result) {
                    $(".select2-formulas").append(selectPFOption).trigger('change');
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            this.workfront();
            this.update();
            this.sheduler();
            this.headers();
        },
        workfront: function () {
            $(".select2-workfrontheads").on("change", function () {
                select2.sewergroups.reload(this.value);
            });

            $(".select2-formulas").on("change", function () {
                var txt = $(".select2-formulas option:selected").text();
                console.log(txt);
                if (txt.indexOf("F5/6") >= 0) {
                    console.log("entre");
                    $(".workfront_group").attr("hidden", false);
                } else {
                    $(".workfront_group").attr("hidden", true);
                }
            });
        },
        update: function () {
            $("#btn-massive-update").on("click", function () {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/f7-pdp/actualizar-todo"),
                    method: "put",
                    contentType: false,
                    processData: false
                })
                    .done(function () {
                        datatables.pdpDt.reload();
                        console.log("ga");
                        _app.loader.hide();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.loader.hide();
                        _app.show.notification.add.error();
                    });
            });
        },
        sheduler: function () {
            $("#btn-sheduler").on("click", function () {
                $("#sheduler_modal").modal("show");
            });
        },
        headers: function () {
            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/f7-pdp/instalado"),
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        }
    };

    return {
        init: function () {
            validate.init();
            modals.init();
            select2.init();
            events.init();
            events.update();
            datepickers.init();
            datatables.init();
        }
    };
}();

$(function () {
    Pdp.init();
});