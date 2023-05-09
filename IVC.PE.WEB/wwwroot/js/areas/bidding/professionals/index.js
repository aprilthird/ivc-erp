var Worker = function () {

    var workerDatatable = null;
    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var foldingDatatable = null;
    var importForm = null;
    var ceaseForm = null;
    var newEntryForm = null;

    var shedulerForm = null;

    var value1s = [];
    var value2s = [];

    var selectSGOption = new Option('--Seleccione un Proveedor--', null, true, true);

    var equipId = null;


    var options = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17],
                hide: [4,5,8,13,14,15,16,17,18,19,20]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                hide: [13, 14, 15, 16, 17,18,19,20]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 3',
                show: ':hidden'
            }
        ],
        //serverSide: true,
        //processing: true,
        ajax: {
            url: _app.parseUrl("/licitaciones/profesionales/listar"),
            data: function (d) {
                d.profe = $("#profe_filter").val();
                d.positionId = $("#position_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "DNI",
                data: "document",
            },
            {
                title: "Ap.Paterno",
                data: "paternalSurname"
            },
            {
                title: "Ap.Materno",
                data: "maternalSurname"
            },
            {
                title: "Nombres",
                data: "name"
            },
            {
                title: "Correo",
                data: "email",
                visible: false
            },
            {
                title: "Numero de Telefono",
                data: "phoneNumber",
                visible: false
            },

            {
                title: "Profesion",
                data: "professionName",
            },

            {
                title: "Fecha de Nacimiento",
                data: "birthDateString",
                visible: false
            },

            {
                title: "Fecha de Expedición del titulo",
                data: "startTitleString",
            },
            {
                title: "Colegiatura",
                data: "cipNumber"
            },
            {
                title: "Fecha de Colegiatura",
                data: "cipDateString",
            },

            {
                title: "¿Tiene Validacion Sunedu?",
                data: "validationSunedu", render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1) {
                        tmp += "Si"
                    }
                    else
                        tmp += "No"
                    return tmp;
                }
            },

            {
                title: "¿Tiene CertiAdulto?",
                data: "certiAdult", render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1) {
                        tmp += "Si"
                    }
                    else
                        tmp += "No"
                    return tmp;
                }
            },

            {
                title: "Domicilio",
                data: "address",
                visible: false,
            },

            {
                title: "Universidad o Centro de Estudios",
                data: "university",
                visible: false,
            },

            {
                title: "Nacionalidad",
                data: "nacionality",
                visible: false,
            },

            {
                title: "DNI Adjunto",
                data: "dniUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="DNI" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {
                title: "Título Adjunto",
                data: "titleUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Título" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {
                title: "Colegiatura Adjunto",
                data: "cipUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Colegiatura" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {
                title: "CertiAdulto Adjunto",
                data: "certiAdultUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="CertiAdulto" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },

            {
                title: "Capacitaciones Adjunto",
                data: "capacitationUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Capacitación" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
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

            {//15
                title:"Experiencia",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                },
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
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel">`;
                    tmp += `<i class="fa fa-file-excel"></i></button>`;
                    return tmp;
                }
            }
        ]
    };
    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/licitaciones/profesionales-experiencias/listar`),
            data: function (d) {
                d.equipId = equipId;
                d.positionId = $("#position_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "N°",
                data:"order"
            },
            {
                title: "Cliente o Empleador",
                data: "business.tradename"
            },
            {
                title: "Objeto de la Contratación",
                data: "biddingWork.name"
            },
            {
                title: "Cargo Desempeñado",
                data: "position.name"
            },
            {
                title: "Fecha de inicio",
                data: "startDate"
            },

            {
                title: "Fecha de Culminación",
                data: "endDate"
            },

            {
                title: "Tiempo Acumulado",
                data: "dif"

            },
            {
                title: "Observaciones",
                data: "observations"

            },

            {
                title: "Certificado",
                data: "fileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Certificado" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
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
                    //tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-excel">`;
                    //tmp += `<i class="fa fa-file-excel"></i></button>`;
                    //tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    //tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]

    };


    var datatables = {
        init: function () {
            this.foldingDt.init();
            this.datatable.init();
        },
        datatable: {
            init: function () {
                workerDatatable = $("#professional_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                workerDatatable.ajax.reload();
            },
            initEvents: function () {

                workerDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt.reload();
                        form.load.detail(id);
                    });

                workerDatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        form.load.foldingFor05(id);
                    });

                workerDatatable.on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");

                        let param1 = id;


                        window.location = `/licitaciones/profesionales/detalle-folding-excel?proId=${param1}`;

                    });

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
                    form.load.pdf(id, url, sname);
                });

                workerDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El profesional será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/licitaciones/profesionales/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El profesional ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar al profesional"
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
                        $("#detail_modal").modal("hide");
                        form.load.editfolding(id);
                    });



                foldingDatatable.on
                    ("click", ".btn-view",
                        function () {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            let url = $btn.data("url");
                            let sname = $btn.data("name");
                            form.load.pdf2(id, url, sname);
                        });


                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El detalle será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/licitaciones/profesionales-experiencias/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt.reload();
                                            datatables.datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El detalle ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el detalle"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
    };

    var form = {
        load: {
            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales-experiencias/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='ProfessionalId']").val(result.professionalId);

                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='select_business']").val(result.businessId).trigger("change");

                        formElements.find("[name='BiddingWorkId']").val(result.biddingWorkId);
                        formElements.find("[name='select_work']").val(result.biddingWorkId).trigger("change");

                        formElements.find("[name='PositionId']").val(result.positionId);
                        formElements.find("[name='select_position']").val(result.positionId).trigger("change");


                        formElements.find("[name='StartDate']").datepicker('setDate', result.startDate);
                        formElements.find("[name='EndDate']").datepicker('setDate', result.endDate);
                        if (result.fileUrl) {
                            $("#edit_folding_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_folding_form [for='customFile']").text("Selecciona un archivo");
                        }

                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='DocumentType']").val(result.documentType).trigger("change");
                        formElements.find("[name='Document']").val(result.document);

                        formElements.find("[name='Address']").val(result.address);

                        formElements.find("[name='University']").val(result.university);

                        formElements.find("[name='Nacionality']").val(result.nacionality);


                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='CIPNumber']").val(result.cipNumber);
                        formElements.find("[name='ProfessionId']").val(result.professionId);
                        formElements.find("[name='select_profession']").val(result.professionId).trigger("change");

                        formElements.find("[name='CertiAdult']").val(result.certiAdult.toString());
                        formElements.find("[name='select_certiadult']").val(result.certiAdult.toString()).trigger("change");

                        formElements.find("[name='ValidationSunedu']").val(result.validationSunedu.toString());
                        formElements.find("[name='select_validation']").val(result.validationSunedu.toString()).trigger("change");

                        formElements.find("[name='BirthDateStr']").datepicker('setDate', result.birthDateStr);
                        formElements.find("[name='CipDateStr']").datepicker('setDate', result.cipDateStr);
                        formElements.find("[name='StartTitleStr']").datepicker('setDate', result.startTitleStr);

                        if (result.dniUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        if (result.titleUrl) {
                            $("#edit_form [for='customFile2']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile2']").text("Selecciona un archivo");
                        }
                        if (result.cipUrl) {
                            $("#edit_form [for='customFile3']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile3']").text("Selecciona un archivo");
                        }
                        if (result.certiAdultUrl) {
                            $("#edit_form [for='customFile4']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile4']").text("Selecciona un archivo");
                        }
                        if (result.capacitationUrl) {
                            $("#edit_form [for='customFile5']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile5']").text("Selecciona un archivo");
                        }

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales/usp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");

                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Name']").attr("disabled", "disabled");

                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='PaternalSurname']").attr("disabled", "disabled");

                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='MaternalSurname']").attr("disabled", "disabled");
                        
                        formElements.find("[name='Dif']").val(result.dif);
                        formElements.find("[name='Dif']").attr("disabled", "disabled");

                        formElements.find("[name='Years']").val(result.years);
                        formElements.find("[name='Years']").attr("disabled", "disabled");

                        formElements.find("[name='Months']").val(result.months);
                        formElements.find("[name='Months']").attr("disabled", "disabled");

                        formElements.find("[name='Days']").val(result.days);
                        formElements.find("[name='Days']").attr("disabled", "disabled");

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='ProfessionalId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id,url,sname) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(sname);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url+ "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdf2: function (id, url, sname) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales-experiencias/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(sname);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },

        },
        submit: {
            sheduler: function (formElement) {
                $(formElement).find("[name='PositionId']").val($(formElement).find("[name='select_position']").val());
                $(formElement).find("[name='ProfessionalId']").val($(formElement).find("[name='select_profe']").val());
                let formElements = $("#sheduler_form");

                let data = new FormData($(formElement).get(0));
                console.log(data);
                $.ajax({
                    url: _app.parseUrl("/licitaciones/profesionales-experiencias/listar-scheduler"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                }).done(function (result) {
                    sources = [];
                    result.forEach(function (pro) {
                        var startDateWork = new Date(pro.biddingWork.startDateSche);
                        var endDateWork = new Date(pro.biddingWork.endDateSche);
                        var startDate = new Date(pro.startDateSche);
                        var endDate = new Date(pro.endDateSche);
                        console.log(startDateWork + "Fecha de inicio de Obra");
                        console.log(endDateWork + "Fecha de fin de Obra");
                        console.log(startDate + "Fecha de inicio de experiencia");
                        console.log(endDate + "Fecha de fin de expereincia");
                        value1s.push({
                            from: startDateWork.getMonth() + 1 + "/" + startDateWork.getDate() + "/" + startDateWork.getFullYear(),

                            to: endDateWork.getMonth() + 1 + "/" + endDateWork.getDate() + "/" + endDateWork.getFullYear(),
                            //from: '2012-02-10',
                            //to: '2012-04-03',
                            
                            //label: "Example Value",
                            desc: "Something",
                            customClass: "ganttBlue",
                            dataObj: pro.biddingWork.name + " " + pro.biddingWork.startDate + "-" + pro.biddingWork.endDate
                        });
                        console.log(startDateWork.getMonth());
                        console.log(startDateWork.getDate());
                        console.log(startDateWork.getFullYear());

                        console.log(endDateWork.getMonth());
                        console.log(endDateWork.getDate());
                        console.log(endDateWork.getFullYear());

                        value2s.push({
                            from: startDate.getMonth() + 1 + "/" + startDate.getDate() + "/" + startDate.getFullYear(),
                            to: endDate.getMonth() + 1 + "/" + endDate.getDate() + "/" + endDate.getFullYear(),
                            //label: "Example Value",
                            desc: "Something",
                            customClass: "ganttGreen",
                            dataObj: pro.position.name + " " + pro.startDate + "-" + pro.endDate
                        });

                        sources.push(
                            {
                                name: "Obra" +" "+ pro.biddingWork.number,
                                //desc: pro.biddingWork.startDateSche + "-" + pro.biddingWork.endDateSche,
                                values: value1s
                            },
                            {
                                name: pro.position.name,
                                //desc: pro.startDateSche + "-" + pro.endDateSche,
                                values: value2s
                            },


  
                            {
                                name: "",
                                desc: "",
                                values: []
                            }
                        );
                        value1s = [];
                        value2s = [];

                    });
                    //console.log(sources);
                    //Número de tramos * Número de filas por tramo
                    cant = 6 * 6;
                   // console.log(cant);
                    $("#gant").gantt({
                        source: sources,
                        months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Augosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        dow: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        navigate: "scroll",
                        scale: "months",
                        maxScale: "months",
                        minScale: "months",
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
/*                            $(".bar").width("26px");*/
                            $(".spacer").text("Experiencia");
                            $(".fn-label").css("fontSize", 13);
                        }
                    });
                });
            },
            editfolding: function (formElement) {

                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='BiddingWorkId']").val($(formElement).find("[name='select_work']").val());
                $(formElement).find("[name='PositionId']").val($(formElement).find("[name='select_position']").val());

                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales-experiencias/editar/${id}`),
                    method: "put",
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
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.datatable.reload();
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
            addfolding: function (formElement) {
                $(formElement).find("[name='BusinessId']").val($(formElement).find("[name='select_business']").val());
                $(formElement).find("[name='BiddingWorkId']").val($(formElement).find("[name='select_work']").val());

                $(formElement).find("[name='PositionId']").val($(formElement).find("[name='select_position']").val());



                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales-experiencias/crear`),
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
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.datatable.reload();
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
            add: function (formElement) {
                $(formElement).find("[name='ProfessionId']").val($(formElement).find("[name='select_profession']").val());
                $(formElement).find("[name='CertiAdult']").val($(formElement).find("[name='select_certiadult']").val());
                $(formElement).find("[name='ValidationSunedu']").val($(formElement).find("[name='select_validation']").val());
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

                var emptyCapacitation = $(formElement).find(".customFile5").get(0).files.length === 0;
                if (!emptyCapacitation) {
                    $(formElement).find(".custom-file-capacitation").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/licitaciones/profesionales/crear"),
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
                        if (!emptyCapacitation) {
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
                        if (!emptyCapacitation) {
                            $("#space-bar").remove();
                        }
                        if (!emptyCerti) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.datatable.reload();
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

                $(formElement).find("[name='ProfessionId']").val($(formElement).find("[name='select_profession']").val());
                $(formElement).find("[name='CertiAdult']").val($(formElement).find("[name='select_certiadult']").val());
                $(formElement).find("[name='ValidationSunedu']").val($(formElement).find("[name='select_validation']").val());

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

                var emptyCapacitation = $(formElement).find(".customFile5").get(0).files.length === 0;
                if (!emptyCapacitation) {
                    $(formElement).find(".custom-file-capacitation").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }


                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/licitaciones/profesionales/editar/${id}`),
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
                        if (!emptyCapacitation) {
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
                        if (!emptyCapacitation) {
                            $("#space-bar").remove();
                        }
                        if (!emptyCerti) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.datatable.reload();
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
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            cease: function () {
                ceaseForm.resetForm();
                $("#cease_form").trigger("reset");
                $("#cease_alert").removeClass("show").addClass("d-none");
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
                
                $("#sheduler_form").trigger("reset");
                $("#sheduler_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
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
            this.positions.init();
            this.works.init();
            this.profe_filters.init();
            this.position_filters.init();
            this.profes.init();
        },
        businesses: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empresas")
                }).done(function (result) {
                    $(".select2-businesses").append(selectSGOption).trigger('change');
                    $(".select2-businesses").select2({
                        data: result
                    });
                });
            }
        },

        works: {
            init: function () {
                $(".select2-works").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-empresa-seleccionada?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-works").empty();
                    $(".select2-works").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-empresa-seleccionada?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-works").empty();
                    $(".select2-works").select2({
                        data: result
                    });
                    $(".select2-works").val(eqsid).trigger('change');
                });
            },
        },

        profes: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/profesionales-lista")
                }).done(function (result) {
                    $(".select2-profes").select2({
                        data: result
                    });
                });
            }
        },

        positions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cargos")
                }).done(function (result) {
                    $(".select2-positions").select2({
                        data: result
                    });
                });
            }
        },

        position_filters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cargos")
                }).done(function (result) {
                    $(".select2-position_filters").select2({
                        data: result
                    });
                });
            }
        },

        professions: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/profesiones-lista")
                }).done(function (result) {
                    $(".select2-professions").select2({
                        data: result
                    });
                });
            }
        },

        profe_filters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/profesiones-lista")
                }).done(function (result) {
                    $(".select2-profe_filters").select2({
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

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            addFoldingForm = $("#add_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addfolding(formElement);
                }
            });

            editFoldingForm = $("#edit_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.editfolding(formElement);
                }
            });

            shedulerForm = $("#sheduler_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.sheduler(formElement);
                }
            });

            detailForm = $("#detail_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
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

            $("#sheduler_modal").on("hidden.bs.modal",
                function () {
                    form.reset.sheduler();
                });

            $("#cease_modal").on("hidden.bs.modal",
                function () {
                    form.reset.cease();
                });
        }
    };

    var events = {
        init: function () {
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
            $("#profe_filter,#position_filter").on("change", function () {
                datatables.datatable.reload();
                datatables.foldingDt.reload();
            });
            
            $(".btn-export-workers").on("click", function () {
                window.location = `/recursos-humanos/obreros/exportar`;
            });

            $("#btn-massive-load").on("click", function () {
                window.location = `/recursos-humanos/obreros/excel-carga-masiva`;
            });

            $(".select2-businesses").on("change", function () {
                select2.works.reload(this.value);

            });

                $("#btn-sheduler").on("click", function () {
                    $("#sheduler_modal").modal("show");
                });
            
        }
    };

    return {
        init: function () {
            events.init();
            datatables.init();
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