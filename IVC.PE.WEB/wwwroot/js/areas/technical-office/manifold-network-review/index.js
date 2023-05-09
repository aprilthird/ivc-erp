var ManifoldNetworkSummary = function () {
    var smToPartitionId = null;

    var reviewLetterForm = null;
    var reviewEditLetterForm = null;
    var reviewEditForm = null;
    var partitionAddForm = null;

    var reviewDatatable = null;

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
                text: "<i class='fa fa-mail-bulk'></i> Cartas de Solicitud y Aprobación",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    $("#review_letter_modal").modal("show");
                }
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
            },
            {
                //title: "Opciones",
                data: null,
                render: function (data, type, row) {
                    var tmp = ``;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-letter" data-toggle="tooltip" title="Cartas">`;
                    tmp += `<i class="fa fa-mail-bulk"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-edit" data-toggle="tooltip" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: 'dt-body-right', 'targets': [2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 17, 18, 20, 21, 22, 25, 26, 27, 29, 30, 31, 32, 33] }
        ],
        initComplete: function (settings, json) {
            events.pipelineTotal();
            events.asfaltTotal();
        }
    };

    var datatables = {
        init: function () {
            this.rewDt.init();
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
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.review.edit(id);
                    });

                reviewDatatable.on("click",
                    ".btn-letter",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.review.letters(id);
                    });

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
        }
    };

    var forms = {
        load: {
            review: {
                letters: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/cartas-colectores-descarga/listar/${id}`),
                        dataSrc: ""
                    })
                        .done(function (result) {
                            let formElements = $("#review_letter_edit_form");
                            formElements.find("[name='SewerManifoldId']").val(id);
                            formElements.find("[name='RequestLetterIds']").val(result.requestLetterIds);
                            formElements.find("[name='select_req_letters']").val(result.requestLetterIds);
                            formElements.find("[name='ApprovalLetterIds']").val(result.approvalLetterIds);
                            formElements.find("[name='select_app_letters']").val(result.approvalLetterIds);
                            $(".select2-letters-sent").trigger('change');
                            $(".select2-letters-received").trigger('change');
                            $("#review_letter_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                },
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/${id}`),
                        dataSrc: ""
                    })
                        .done(function (result) {
                            let formElements = $("#review_edit_form");
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

                            $("#review_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            review: {
                letters: function (formElement) {
                    $(formElement).find("[name='SewerManifoldIds']").append($(formElement).find("[name='select_sm_reviews']").val());
                    $(formElement).find("[name='RequestLetterIds']").append($(formElement).find("[name='select_req_letters']").val());
                    $(formElement).find("[name='ApprovalLetterIds']").append($(formElement).find("[name='select_app_letters']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/oficina-tecnica/cartas-colectores-descarga/crear"),
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
                            datatables.rewDt.reload();
                            $("#review_letter_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#review_letter_alert_text").html(error.responseText);
                                $("#review_letter_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                editLetters: function (formElement) {
                    $(formElement).find("[name='RequestLetterIds']").append($(formElement).find("[name='select_req_letters']").val());
                    $(formElement).find("[name='ApprovalLetterIds']").append($(formElement).find("[name='select_app_letters']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='SewerManifoldId']").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/cartas-colectores-descarga/editar/${id}`),
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
                            datatables.rewDt.reload();
                            $("#review_letter_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#review_letter_edit_alert_text").html(error.responseText);
                                $("#review_letter_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                },
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
                            datatables.rewDt.reload();
                            $("#review_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#review_edit_alert_text").html(error.responseText);
                                $("#review_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                },
                partition: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/replanteo-colectores-descarga/partir-tramo`),
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
                            datatables.rewDt.reload();
                            $("#partition_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#partition_add_alert_text").html(error.responseText);
                                $("#partition_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            }
        },
        reset: {
            review: {
                letters: function () {
                    reviewLetterForm.reset();
                    $(".select2-sm-reviews").val('').trigger("change");
                    $(".select2-letters-sent").val('').trigger("change");
                    $(".select2-letters-received").val('').trigger("change");
                    $("#review_letter_form").trigger("reset");
                },
                editLetters: function () {
                    reviewEditLetterForm.reset();
                    $(".select2-letters-sent").val('').trigger("change");
                    $(".select2-letters-received").val('').trigger("change");
                    $("#review_letter_edit_form").trigger("reset");
                },
                edit: function () {
                    reviewEditForm.reset();
                    $("#review_edit_form").trigger("reset");
                },
                partition: function () {
                    partitionAddForm.reset();
                    $("#partition_add_form").trigger("reset");
                }
            }
        }
    };

    var validate = {
        init: function () {
            reviewLetterForm = $("#review_letter_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.review.letters(formElement);
                }
            });

            reviewEditLetterForm = $("#review_letter_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.review.editLetters(formElement);
                }
            });

            reviewEditForm = $("#review_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.review.edit(formElement);
                }
            });

            partitionAddForm = $("#partition_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.review.partition(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#review_letter_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.review.letters();
                });

            $("#review_letter_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.review.editLetters();
                });

            $("#review_edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.review.edit();
                });

            $("#partition_add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.review.partition();
                });
        }
    };

    var select2 = {
        init: function () {
            this.lettersSent.init();
            this.lettersReceived.init();
            this.smLetters.init();
            this.smReview.init();
        },
        lettersSent: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas?type=1")
                }).done(function (result) {
                    $(".select2-letters-sent").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        lettersReceived: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas?type=2")
                }).done(function (result) {
                    $(".select2-letters-received").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
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
        },
        smReview: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga`)
                }).done(function (result) {
                    $(".select2-sm-reviews").select2({
                        data: result
                    });
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

            $("#genExcelSample").on("click", function () {
                window.location = _app.parseUrl(`/oficina-tecnica/colectores-descarga/formato-excel`);
            });
        },
        pipelineTotal: function () {
            var pipeline = reviewDatatable.column(27).data().reduce(function (a, b) {
                return a + parseFloat(b);
            }, 0);

            $("#pipeline_total").text(
                pipeline.toFixed(2)
            );
        },
        asfaltTotal: function () {
            var asfalt2 = reviewDatatable.column(30).data().reduce(function (a, b) {
                return a + parseFloat(b);
            }, 0);
            var asfalt3 = reviewDatatable.column(31).data().reduce(function (a, b) {
                return a + parseFloat(b);
            }, 0);
            var asfalt3m = reviewDatatable.column(32).data().reduce(function (a, b) {
                return a + parseFloat(b);
            }, 0);

            var total = asfalt2 + asfalt3 + asfalt3m;
            $("#asfalt_total").text(
                total.toFixed(2)
            );
        }
    };

    return {
        init: function () {
            datatables.init();
            modals.init();
            validate.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    ManifoldNetworkSummary.init();
});