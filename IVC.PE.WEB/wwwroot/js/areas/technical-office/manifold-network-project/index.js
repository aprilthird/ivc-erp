var ManifoldNetworkSummary = function () {

    var projectEditForm = null;

    var projectDatatable = null;

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
            },
            {
                //title: "Opciones",
                data: null,
                render: function (data, type, row) {
                    var tmp = ``;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: 'dt-body-right', 'targets': [2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 17, 18, 20, 21, 22, 25, 26, 27, 29, 30, 31, 32, 33] }
        ]
    };

    var datatables = {
        init: function () {
            this.proDt.init();
        },
        proDt: {
            init: function () {
                projectDatatable = $("#project_datatable").DataTable(proDtOption);
                this.events();
            },
            reload: function () {
                projectDatatable.ajax.reload();
            },
            events: function () {
                projectDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.project.edit(id);
                    });
            }
        }
    };

    var forms = {
        load: {
            project: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/${id}`),
                        dataSrc: ""
                    })
                        .done(function (result) {
                            let formElements = $("#project_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            $("#sm_name").html(result.name);

                            $("#i-name").html(result.sewerBoxStart.code);
                            formElements.find("[name='SewerBoxStart.Code']").val(result.sewerBoxStart.code);
                            formElements.find("[name='SewerBoxStart.CoverLevel']").val(result.sewerBoxStart.coverLevel);
                            formElements.find("[name='SewerBoxStart.ArrivalLevel']").val(result.sewerBoxStart.arrivalLevel);
                            formElements.find("[name='SewerBoxStart.BottomLevel']").val(result.sewerBoxStart.bottomLevel);
                            formElements.find("[name='SewerBoxStart.TerrainType']").val(result.sewerBoxStart.terrainType).trigger("change");
                            formElements.find("[name='SewerBoxStart.SewerBoxType']").val(result.sewerBoxStart.sewerBoxType).trigger("change");
                            formElements.find("[name='SewerBoxStart.Thickness']").val(result.sewerBoxStart.thickness);

                            $("#j-name").html(result.sewerBoxEnd.code);
                            formElements.find("[name='SewerBoxEnd.Code']").val(result.sewerBoxEnd.code);
                            formElements.find("[name='SewerBoxEnd.CoverLevel']").val(result.sewerBoxEnd.coverLevel);
                            formElements.find("[name='SewerBoxEnd.ArrivalLevel']").val(result.sewerBoxEnd.arrivalLevel);
                            formElements.find("[name='SewerBoxEnd.BottomLevel']").val(result.sewerBoxEnd.bottomLevel);
                            formElements.find("[name='SewerBoxEnd.TerrainType']").val(result.sewerBoxEnd.terrainType).trigger("change");
                            formElements.find("[name='SewerBoxEnd.SewerBoxType']").val(result.sewerBoxEnd.sewerBoxType).trigger("change");
                            formElements.find("[name='SewerBoxEnd.Thickness']").val(result.sewerBoxEnd.thickness);

                            $("#manifold_name").html(result.name);
                            formElements.find("[name='Name']").val(result.name);
                            formElements.find("[name='Address']").val(result.address);
                            formElements.find("[name='PipeDiameter']").val(result.pipeDiameter);
                            formElements.find("[name='PipeType']").val(result.pipeType).trigger("change");
                            formElements.find("[name='LengthBetweenHAxles']").val(result.lengthBetweenHAxles);
                            formElements.find("[name='TerrainType']").val(result.terrainType).trigger("change");

                            formElements.find("[name='Pavement2In']").val(result.pavement2In);
                            formElements.find("[name='Pavement3In']").val(result.pavement3In);
                            formElements.find("[name='Pavement3InMixed']").val(result.pavement3InMixed);
                            formElements.find("[name='PavementWidth']").val(result.pavementWidth);

                            $("#project_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            project: {
                edit: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='SewerManifoldId']").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/editar/${id}`),
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
                            datatables.proDt.reload();
                            $("#project_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#project_edit_alert_text").html(error.responseText);
                                $("#project_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            }
        },
        reset: {
            project: {
                edit: function () {
                    projectEditForm.reset();
                    $("#project_edit_form").trigger("reset");
                }
            }
        }
    };

    var validate = {
        init: function () {
            projectEditForm = $("#project_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.project.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#project_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.project.edit();
                });
        }
    };

    var events = {
        init: function () {
            $("#genExcelSample").on("click", function () {
                window.location = _app.parseUrl(`/oficina-tecnica/colectores-descarga/formato-excel`);
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
    ManifoldNetworkSummary.init();
});