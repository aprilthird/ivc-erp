var Worker = function () {

    var workerDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var ceaseForm = null;
    var newEntryForm = null;
    var sgOption = new Option('--Seleccione una Empresa--', null, true, true);
    var options = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9,11,12,14,15],
                hide: [10,13,16,17,18,19,20,21]
            },

            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: ':hidden'
            }
        ],
        //serverSide: true,
        //processing: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/obras/listar"),
            data: function (d) {
                d.period = $("#period_filter").val();
                d.ivc = $("#participation_filter").val();
                d.biddingWorkType = $("#work_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                //0
                title: "N°",
                data: "number"
            },
            {
                //1
                title: "Nombres",
                data: "name"
            },

            {//2
                title: "Tipo de Obra",
                data: "biddingWorkType.name",
                

                render: function (data, type, row) {

                    tmp = "";

                    if (row.biddingWorkType.pillColor == '0') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${data}</span></label></span>`;
                    }
                    if (row.biddingWorkType.pillColor == '1') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${data}</span></label></span>`;
                    }
                    if (row.biddingWorkType.pillColor == '2') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${data}</span></label></span>`;
                    }
                    if (row.biddingWorkType.pillColor == '3') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${data}</span></label></span>`;
                    }
                    if (row.biddingWorkType.pillColor == '4') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${data}</span></label></span>`;
                    }
                    if (row.biddingWorkType.pillColor == '5') {
                        tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${data}</span></label></span>`;
                    }

                    return tmp;
                }
            },
            {//3
                title: "Componentes de Obra",
                data: "components"
            },
            {//4
                title: "Empresa",
                data: "business.tradename"
            },

            {//5
                title: "Fecha de Inicio",
                data: "startDate",
                "sType": "date-uk"
            },
            {//6
                title: "Fecha de Fín",
                data: "endDate",
                "sType": "date-uk"
            },
            {//7
                title: "Plazo (días)",
                data: "difDate"
            },
            {//8
                title: "Fecha de Recepción",
                data: "receivedDate",
                "sType": "date-uk"
            },
            {//9
                title: "Monto de Contrato (S/)",
                data: "contractAmmountFormated"
            },
            {//10
                title: "Monto de Contrato ($)",
                data: "contractDollarAmmountFormated",
                visible: false,
            },
            {//11
                title: "Fecha de Liquidación",
                data: "liquidationDate",
                "sType": "date-uk"
            },
            {//12
                title: "Monto de Liquidacion (S/)",
                data: "liquidationAmmountFormated"
            },
            {//13
                title: "Monto de Liquidacion ($)",
                data: "liquidationDollarAmmountFormated",
                visible: false,
            },


            {//14
                title: "% de Participacion de IVC",
                data: "businessParticipationFolding.ivcParticipation",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {//16
                title: "Monto de Participacion de IVC (S/)",
                data: "participationAmmountFormated",

            },
            {//17
                title: "Monto de Participacion de IVC ($)",
                data: "participationDollarAmmountFormated",
                visible: false,
            },

            {//18
                title: "Contrato",
                data: "contractUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Contrato" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
            {//19
                title: "Acta de Recepción",
                data: "receivedActUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Acta" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
            {//20
                title: "Liquidación",
                data: "liquidationUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Liquidación" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {//21
                title: "Conformidad de Obra",
                data: "confirmedWork",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Conformidad de Obra" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {//22
                title: "Facturas",
                data: "inVoiceUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Factura" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            //{
            //    title: "Título Adjunto",
            //    data: "titleUrl",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-name="Título" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
            //            tmp += `<i class="fa fa-eye"></i></button>  `;
            //        }
            //        return tmp;
            //    }
            //},

            //{
            //    title: "Colegiatura Adjunto",
            //    data: "cipUrl",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-name="Colegiatura" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
            //            tmp += `<i class="fa fa-eye"></i></button>  `;
            //        }
            //        return tmp;
            //    }
            //},

            //{
            //    title: "CertiAdulto Adjunto",
            //    data: "certiAdultUrl",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-name="CertiAdulto" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
            //            tmp += `<i class="fa fa-eye"></i></button>  `;
            //        }
            //        return tmp;
            //    }
            //},

            //{
            //    title: "Capacitaciones Adjunto",
            //    data: "capacitationUrl",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-name="Capacitación" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
            //            tmp += `<i class="fa fa-eye"></i></button>  `;
            //        }
            //        return tmp;
            //    }
            //},
            //{
            //    title: "Título Adjunto",
            //    data: "profession.name",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-videourl="${data}" data-test="Espejo" data-name="${row.sewerManifold.name}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
            //            tmp += `<i class="fa fa-video"></i></button> `;
            //        }
            //        return tmp;
            //    }
            //},
            //{
            //    title: "CIP Adjunto",
            //    data: "profession.name",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-videourl="${data}" data-test="Espejo" data-name="${row.sewerManifold.name}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
            //            tmp += `<i class="fa fa-video"></i></button> `;
            //        }
            //        return tmp;
            //    }
            //},
            //{
            //    title: "Profesion",
            //    data: "profession.name",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-videourl="${data}" data-test="Espejo" data-name="${row.sewerManifold.name}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
            //            tmp += `<i class="fa fa-video"></i></button> `;
            //        }
            //        return tmp;
            //    }
            //},
            //{
            //    title: "Capacitación Adjunto",
            //    data: "profession.name",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (data != null) {
            //            tmp += `<button data-videourl="${data}" data-test="Espejo" data-name="${row.sewerManifold.name}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
            //            tmp += `<i class="fa fa-video"></i></button> `;
            //        }
            //        return tmp;
            //    }
            //},
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

    var datatable = {
        init: function () {
            workerDatatable = $("#professional_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            workerDatatable.ajax.reload();
            select2.headers();
        },
        initEvents: function () {
            workerDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            workerDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                let url = $btn.data("url");
                let sname = $btn.data("name");
                form.load.pdf(id,url,sname);
            });

            workerDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "la obra será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/licitaciones/obras/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "la obra ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la obra"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            workerDatatable.on("click",
                ".btn-cease",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.cease(id);
                });

            workerDatatable.on("click",
                ".btn-new-entry",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.newentry(id);
                });
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/obras/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Number']").val(result.number);

                        formElements.find("[name='BiddingWorkTypeId']").val(result.biddingWorkTypeId);
                        formElements.find("[name='select_profession']").val(result.biddingWorkTypeId).trigger("change");


                        formElements.find("[name='StartDate']").datepicker("setDate", result.startDate);
                        formElements.find("[name='EndDate']").datepicker("setDate", result.endDate);
                        formElements.find("[name='ReceivedDate']").datepicker("setDate", result.receivedDate);
                        formElements.find("[name='LiquidationDate']").datepicker("setDate", result.liquidationDate);

                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='select_business']").val(result.businessId).trigger("change");

                        formElements.find("[name='CurrencyType']").val(result.currencyType);
                        formElements.find("[name='select_currency']").val(result.currencyType).trigger("change");


                        formElements.find("[name='BiddingCurrencyTypeId']").val(result.biddingCurrencyTypeId);
                        formElements.find("[name='select_biddingcurrency']").val(result.biddingCurrencyTypeId).trigger("change");
                       


                        formElements.find("[name='BusinessParticipationFoldingId']").val(result.businessParticipationFoldingId);
                        formElements.find("[name='select_businessfolding']").val(result.businessParticipationFoldingId).trigger("change");

                        select2.businessfoldings.edit(result.businessId, result.businessParticipationFoldingId);



                        formElements.find("[name='ContractAmmount']").val(parseFloat(Math.round(result.contractAmmount * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        
                        formElements.find("[name='LiquidationAmmount']").val(parseFloat(Math.round(result.liquidationAmmount* Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='ParticipationAmmount']").val(parseFloat(Math.round(result.participationAmmount * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));

                        formElements.find("[name='ContractDollarAmmount']").val(parseFloat(Math.round(result.contractDollarAmmount * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        
                        formElements.find("[name='LiquidationDollarAmmount']").val(parseFloat(Math.round(result.liquidationDollarAmmount* Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='ParticipationDollarAmmount']").val(parseFloat(Math.round(result.participationDollarAmmount * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));


                        formElements.find("[name='BiddingWorkComponents']").val(result.biddingWorkComponents);
                        formElements.find("[name='select_components']").val(result.biddingWorkComponents);
                        $(".select2-components").trigger('change');
                        if (result.contractUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        if (result.inVoiceUrl) {
                            $("#edit_form [for='customFile2']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile2']").text("Selecciona un archivo");
                        }
                        if (result.liquidationUrl) {
                            $("#edit_form [for='customFile3']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile3']").text("Selecciona un archivo");
                        }
                        if (result.receivedActUrl) {
                            $("#edit_form [for='customFile4']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile4']").text("Selecciona un archivo");
                        }
                       

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id,url,sname) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/obras/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(sname);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url+ "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },


        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BiddingWorkTypeId']").val($(formElement).find("[name='select_profession']").val());
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='BusinessParticipationFoldingId']").val($(formElement).find("[name='select_businessfolding']").val());
                $(formElement).find("[name='BiddingWorkComponents']").append($(formElement).find("[name='select_components']").val());
                $(formElement).find("[name='CurrencyType']").val($(formElement).find("[name='select_currency']").val());
                $(formElement).find("[name='BiddingCurrencyTypeId']").val($(formElement).find("[name='select_biddingcurrency']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyTitle = $(formElement).find(".customFile").get(0).files.length === 0;
                if (!emptyTitle) {
                    $(formElement).find(".custom-file-title").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyDni = $(formElement).find(".customFile2").get(0).files.length === 0;
                if (!emptyDni) {
                    $(formElement).find(".custom-file-dni").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyCip = $(formElement).find(".customFile3").get(0).files.length === 0;
                if (!emptyCip) {
                    $(formElement).find(".custom-file-cip").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyCerti = $(formElement).find(".customFile4").get(0).files.length === 0;
                if (!emptyCerti) {
                    $(formElement).find(".custom-file-certi").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                $.ajax({
                    url: _app.parseUrl("/licitaciones/obras/crear"),
                    method: "post",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyDni) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyTitle) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyCerti) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyCip) {
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
                        if (!emptyDni) {
                            $("#space-bar").remove();
                        }
                        if (!emptyCip) {
                            $("#space-bar").remove();
                        }
                        if (!emptyTitle) {
                            $("#space-bar").remove();
                        }

                        if (!emptyCerti) {
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
                $(formElement).find("[name='BusinessParticipationFoldingId']").val($(formElement).find("[name='select_businessfolding']").val());
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='BiddingWorkComponents']").append($(formElement).find("[name='select_components']").val());    
                $(formElement).find("[name='BiddingWorkTypeId']").val($(formElement).find("[name='select_profession']").val());
                $(formElement).find("[name='CurrencyType']").val($(formElement).find("[name='select_currency']").val());
                $(formElement).find("[name='BiddingCurrencyTypeId']").val($(formElement).find("[name='select_biddingcurrency']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                var emptyTitle = $(formElement).find(".customFile").get(0).files.length === 0;
                if (!emptyTitle) {
                    $(formElement).find(".custom-file-title").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyDni = $(formElement).find(".customFile2").get(0).files.length === 0;
                if (!emptyDni) {
                    $(formElement).find(".custom-file-dni").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyCip = $(formElement).find(".customFile3").get(0).files.length === 0;
                if (!emptyCip) {
                    $(formElement).find(".custom-file-cip").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyCerti = $(formElement).find(".customFile4").get(0).files.length === 0;
                if (!emptyCerti) {
                    $(formElement).find(".custom-file-certi").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }




                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/obras/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyDni) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyTitle) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyCerti) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyCip) {
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
                        if (!emptyDni) {
                            $("#space-bar").remove();
                        }
                        if (!emptyCip) {
                            $("#space-bar").remove();
                        }
                        if (!emptyTitle) {
                            $("#space-bar").remove();
                        }

                        if (!emptyCerti) {
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

        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#Add_BiddingWorkComponents").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_BiddingWorkComponents").prop("selectedIndex", 0).trigger("change");
            },
            cease: function () {
                ceaseForm.resetForm();
                $("#cease_form").trigger("reset");
                $("#cease_alert").removeClass("show").addClass("d-none");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            newEntry: function () {
                newEntryForm.resetForm();
                $("#new_entry_form").trigger("reset");
                $("#new_entry_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.professions.init();
            this.businesses.init();
            this.work_filters.init();
            this.components.init();
            this.headers;
            this.biddingcurrencies.init();
            this.businessfoldings.init();
        },
        biddingcurrencies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipo-de-cambio-licitaciones")
                }).done(function (result) {
                    $(".select2-biddingcurrencies").select2({
                        data: result
                    });
                });
            }
        },
        headers: function () {
            $.ajax({
                url: _app.parseUrl("/licitaciones/obras/monto"),
                data: {
                    period: $("#period_filter").val(),
                    ivc: $("#participation_filter").val(),
                    biddingWorkType: $("#work_filter").val(),
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        },
        professions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-obra-lista")
                }).done(function (result) {
                    $(".select2-professions").select2({
                        data: result
                    });
                });
            }
        },

        components: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/componentes-de-obra-lista")
                }).done(function (result) {
                    $(".select2-components").select2({
                        data: result,
                        allowClear: true
                    }).val(null).trigger("change");

                });
            }
        },


        work_filters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-obra-lista")
                }).done(function (result) {
                    $(".select2-work_filters").select2({
                        data: result
                    });
                });
            }
        },
        businesses: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empresas")
                }).done(function (result) {
                    $(".select2-businesses").append(sgOption).trigger('change');
                    $(".select2-businesses").select2({
                        data: result
                    });
                });
            }
        },
        businessfoldings: {
            init: function () {
                $(".select2-businessfoldings").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/empresa-seleccionado?businessId=${sg}`)
                }).done(function (result) {
                    $(".select2-businessfoldings").empty();
                    $(".select2-businessfoldings").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/empresa-seleccionado?businessId=${sg}`)
                }).done(function (result) {
                    $(".select2-businessfoldings").empty();
                    $(".select2-businessfoldings").select2({
                        data: result
                    });
                    $(".select2-businessfoldings").val(eqsid).trigger('change');
                });
            },

        },
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
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

            ceaseForm = $("#cease_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.cease(formElement);
                }
            });

            newEntryForm = $("#new_entry_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.newEntry(formElement);
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

            $("#cease_modal").on("hidden.bs.modal",
                function () {
                    form.reset.cease();
                });
        }
    };

    var events = {
        init: function () {

            $(".select2-businesses").on("change", function () {
                select2.businessfoldings.reload(this.value);
                
            });

            $(".select2-currencies").on("change", function () {
                var txt = $(".select2-currencies option:selected").text();
                if (txt.indexOf("Dolares") <= 0) {
                   
                    $(".dollar-group").attr("hidden", true);
                    $(".pen-group").attr("hidden", false);

                } if (txt.indexOf("Dolares") >= 0) {
                   
                    $(".dollar-group").attr("hidden", false);
                    $(".pen-group").attr("hidden", true);
                    

                } 
  
            });

            $("#add_form [name='DocumentType']").attr("id", "Add_DocumentType");
            $("#edit_form [name='DocumentType']").attr("id", "Edit_DocumentType");
            $("#add_form [name='WorkerPositionId']").attr("id", "Add_WorkerPositionId");
            $("#edit_form [name='WorkerPositionId']").attr("id", "Edit_WorkerPositionId");
            $("#add_form [name='Category']").attr("id", "Add_Category");
            $("#edit_form [name='Category']").attr("id", "Edit_Category");
            //$("#add_modal").on("shown.bs.modal", function () {
            //    $("#Add_RoleIds").select2();
            //});
            //$("#edit_modal").on("shown.bs.modal", function () {
            //    $("#Edit_RoleIds").select2();
            //});

            $("#add_form [name='BiddingWorkComponents']").attr("id", "Add_BiddingWorkComponents");
            $("#edit_form [name='BiddingWorkComponents']").attr("id", "Edit_BiddingWorkComponents");

            $("#period_filter,#participation_filter,#work_filter").on("change", function () {
                datatable.reload();
            });
            
            $(".btn-export-workers").on("click", function () {
                window.location = `/recursos-humanos/obreros/exportar`;
            });

            $("#btn-massive-load").on("click", function () {
                window.location = `/recursos-humanos/obreros/excel-carga-masiva`;
            });

            this.headers();
        },

        headers: function () {
            $.ajax({
                url: _app.parseUrl("/licitaciones/obras/monto"),
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_installed_header").val(result);
                });
        }

    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Worker.init();
});