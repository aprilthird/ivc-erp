var Provider = function () {

    var bondAddsDataTable = null;
    var bondRensDataTable = null;
    var addForm = null;
    var editForm = null;    
    var addRenForm = null;
    var editRenForm = null;

    var isFromDetail = false;



    var addId = null;
    var renId = null;
    var responsibles = null;

    var bondAddsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/calidad/equipos-certificados/listar"),
            data: function (d) {
                d.equipmentCertificateTypeId = $("#equtype_filter").val();
                d.validity = $("#validity_filter").val();
                d.hasavoid = $("#hasavoid_filter").val();
                d.situation = $("#situation_filter").val();
                d.operation = $("#operation_filter").val();

                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-briefcase'></i>Reporte Excel",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    window.location = `/calidad/equipos-certificados/reporte-calidad`;
                }
            }
        ],
        columns: [
            {
                title: "Correlativo",
                data: "correlative"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Marca",
                data: "brand"
            },
            {
                title: "Modelo",
                data: "model"
            },
            {
                title: "Serie/Cod. ID",
                data: "serial"
            },
            {
                title: "# de certficado",
                data: "equipmentCertificateNumber"
            },
            {
                title: "Propietario",
                data: "equipmentOwnerName"
            },
            {
                title: "Tipo de Equipo",
                data: "equipmentCertificateTypeName",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        if (data.length != 0) {
                            $.each(data.split('/'), function (index, value) {

                                var color = value.split('-');
                                if (color[1] === '0') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${color[0]}</span></label></span>`;
                                }
                                if (color[1] === '1') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${color[0]}</span></label></span>`;
                                }
                                if (color[1] === '2') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${color[0]}</span></label></span>`;
                                }
                                if (color[1] === '3') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${color[0]}</span></label></span>`;
                                }
                                if (color[1] === '4') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${color[0]}</span></label></span>`;
                                }
                                if (color[1] === '5') {
                                    tmp += `<span class="kt-switch kt-switch--icon"><label><span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${color[0]}</span></label></span>`;
                                }
                            });
                        }
                    }
                    return tmp;
                }
            },
            {
                title: "Estado",
                data: "hasAVoid",
                render: function (data, type, row) {
                    if (data === false) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK</span>
								</label>
							</span>`;
                    }
                    if (data === true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">REVISAR</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Vigencia",
                data: "validity",
                render: function (data, type, row) {
                    if (row.situationStatus == 2 || row.operationalStatus == 0)
                        return ``;


                    if (data > 30) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">VIGENTE ${data}</span>
								</label>
							</span>`;
                    } else if (data > 15) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${data}</span>
								</label>
							</span>`;
                    } else if (data > 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${data}</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${data}</span>
								</label>
							</span>`;
                    }
                }
            },


            {
                title: "Fecha de Ingreso",
                data: "entryDateStr"
            },

            {
                title: "Inicio de Vigencia",
                data: "startDateStr"
            },
            {
                title: "Fin de Vigencia",
                data: "endDateStr"
            },
            {
                title: "Condición",
                data: "operationStatusDesc",
                                render: function (data, type, row) {
                    if (data == "Operativo") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${data}</span>
								</label>
							</span>`;
                    } else if (data.includes("Inoperativo") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${data}</span>
								</label>
							</span>`;
                    } 
                }
            },
            {
                title: "Situación",
                data: "situationStatusDesc",
                render: function (data, type, row) {
                    if (data == "Stand by") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${data}</span>
								</label>
							</span>`;
                    } else if (data.includes("En Uso") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${data}</span>
								</label>
							</span>`;
                    } else if (data.includes("Retirado de Obra") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${data}</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Operador",
                data: "operator"
            },

            {
                title: "Frente",
                data: "front"
            },

            {
                title: "Entidad Certificadora",
                data: "equipmentCertifyingEntityName"
            },
            {
                title: "Patron Referencia",
                data: "patternCalibrationRenewalReference"
            },
            {
                title: "Observación",
                data: "observation"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.equipmentCertificateId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.renewalId}" class="btn btn-success btn-sm btn-icon btn-renovation">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.renewalId}" data-equid="${row.equipmentCertificateId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.equipmentCertificateId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.equipmentCertificateId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
 
    };

    var bondRensDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/calidad/equipos-certificados/renovaciones/listar"),
            data: function (d) {
                d.id = addId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Entidad Certificadora",
                data: "equipmentCertifyingEntity.certifyingEntityName"
            },
            {
                title: "# Renovación",
                data: "equipmentCertificateNumber"
            },
            {
                title: "Referencia Patron",
                data: "patternCalibrationRenewal.referenceNumber"
            },
            {
                title: "Inicio de Vigencia",
                data: "startDate"
            },
            {
                title: "Fin de Vigencia",
                data: "endDate"
            },
            {
                title: "Condición",
                data: "operationalStatusDesc"
            },
            {
                title: "Situación",
                data: "situationStatusDesc"
            },
            {
                title: "Operador",
                data: "equipmentCertificateUserOperator.operator"
            },

            {
                title: "Frente",
                data: "qualityFront.description"
            },

            {
                title: "Observación",
                data: "patternCalibrationRenewal.referenceNumber",
                render: function (data, type, row) {
                    if (data !== null) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">FALTA PATRÓN</span>
								</label>
							</span>`;
                    }
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
                    tmp += `<button data-id="${row.id}" data-equid="${row.equipmentCertificateId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var dataTables = {
        init: function () {
            this.bondAddsDt.init();
            this.bondRensDt.init();
        },
        bondAddsDt: {
            init: function () {
                bondAddsDataTable = $("#equipments_datatable").DataTable(bondAddsDt_options);
                this.events();
            },
            reload: function () {
                bondAddsDataTable.ajax.reload();
            },
            events: function () {
                bondAddsDataTable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        addId = id;
                        dataTables.bondRensDt.reload();
                        forms.load.detail(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-renovation",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        renId = id;
                        forms.load.addren(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let eid = $btn.data("equid");
                        let renid = $btn.data("id");
                        isFromDetail = false;

                        select2.renewals.reload(eid, renid);
                        console.log(renid)
                    });

                bondAddsDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El equipo y sus certificaciones serán eliminadas permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/calidad/equipos-certificados/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El equipo y sus certificaciones han sido eliminadas con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el equipo y sus renovaciones."
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        bondRensDt: {
            init: function () {
                bondRensDataTable = $("#renewals_datatable").DataTable(bondRensDt_options);
                this.events();
            },
            reload: function () {
                bondRensDataTable.clear().draw();
                bondRensDataTable.ajax.reload();
            },
            events: function () {
                bondRensDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editren(id);
                    });

                bondRensDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La renovación será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            datatables.bondRensDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La certificación ha sido eliminada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            console.log(errormessage);
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurrió un error al intentar eliminar la certificación. Motivo: ${errormessage.responseText}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                bondRensDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let eid = $btn.data("equid");
                        let renid = $btn.data("id");
                        isFromDetail = true;

                        select2.renewals.reload(eid, renid);
                        $("#detail_modal").modal("hide");
                    });
            }
        },
    };

    var forms = {
        load: {
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='select_project']").val(result.projectId).trigger("change");
                        formElements.find("[name='select_project']").attr("disabled", "disabled");
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Name']").attr("disabled", "disabled");
                        formElements.find("[name='Brand']").val(result.brand);
                        formElements.find("[name='Brand']").attr("disabled", "disabled");
                        formElements.find("[name='Model']").val(result.model);
                        formElements.find("[name='Model']").attr("disabled", "disabled");
                        formElements.find("[name='Serial']").val(result.serial);
                        formElements.find("[name='Serial']").attr("disabled", "disabled");
                        //formElements.find("[name='EquipmentCertificateNumber']").val(result.equipmentCertificateNumber);
                        //formElements.find("[name='EquipmentCertificateNumber']").attr("disabled", "disabled");
                        formElements.find("[name='EquipmentCertificateOwnerId']").val(result.equipmentCertificateOwnerId);
                        formElements.find("[name='select_owner']").val(result.equipmentCertificateOwnerId).trigger("change");
                        formElements.find("[name='select_owner']").attr("disabled", "disabled");
                        formElements.find("[name='EquipmentCertificateTypeId']").val(result.equipmentCertificateTypeId);
                        formElements.find("[name='select_certificate']").val(result.equipmentCertificateTypeId).trigger("change");
                        formElements.find("[name='select_certificate']").attr("disabled", "disabled");

                        

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });                
            },
            editren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_ren_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Responsibles']").val(result.responsibles.toString());
                        formElements.find("[name='EquipmentCertificateNumber']").val(result.equipmentCertificateNumber);
                        formElements.find("[name='EquipmentCertificateId']").val(result.equipmentCertificateId);
                        formElements.find("[name='Oservation']").val(result.observation);
                        formElements.find("[name='StartDate']").datepicker('setDate', result.startDate);
                        formElements.find("[name='EndDate']").datepicker('setDate', result.endDate);
                        formElements.find("[name='OperationalStatus']").val(result.operationalStatus);
                        formElements.find("[name='select_operational']").val(result.operationalStatus).trigger("change");
                        formElements.find("[name='SituationStatus']").val(result.situationStatus);
                        formElements.find("[name='select_situational']").val(result.situationStatus).trigger("change");
                        formElements.find("[name='EquipmentCertificateUserOperatorId']").val(result.equipmentCertificateUserOperatorId);
                        formElements.find("[name='select_operator']").val(result.equipmentCertificateUserOperatorId).trigger("change");
                        formElements.find("[name='EquipmentCertifyingEntityId']").val(result.equipmentCertifyingEntityId);
                        formElements.find("[name='select_entity']").val(result.equipmentCertifyingEntityId).trigger("change");
                        formElements.find("[name='PatternCalibrationRenewalId']").val(result.patternCalibrationRenewalId);
                        formElements.find("[name='select_pattern']").val(result.patternCalibrationRenewalId).trigger("change");

                        formElements.find("[name='InspectionType']").val(result.inspectionType);
                        formElements.find("[name='select_inspection']").val(result.inspectionType).trigger("change");

                        formElements.find("[name='CalibrationMethod']").val(result.calibrationMethod);
                        formElements.find("[name='select_method']").val(result.calibrationMethod).trigger("change");

                        formElements.find("[name='CalibrationFrecuency']").val(result.calibrationFrecuency);
                        formElements.find("[name='select_frecuency']").val(result.calibrationFrecuency).trigger("change");

                        formElements.find("[name='QualityFrontId']").val(result.qualityFrontId);
                        formElements.find("[name='select_front']").val(result.qualityFrontId).trigger("change");

                        $("#edit_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        select2.references.reload(id);
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                       
                        $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));

                        $("#pdf_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });   
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/${id}`),

                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='Correlative']").val(result.correlative);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Brand']").val(result.brand);
                        formElements.find("[name='Model']").val(result.model);
                        formElements.find("[name='Serial']").val(result.serial);
                        formElements.find("[name='EntryDate']").datepicker('setDate', result.entryDate.startDate);


                        formElements.find("[name='EquipmentCertificateRenewal.EquipmentCertificateNumber']").val(result.equipmentCertificateRenewal.equipmentCertificateNumber);
                        formElements.find("[name='EquipmentCertificateRenewal.Responsibles']").val(result.equipmentCertificateRenewal.responsibles.toString());
                        formElements.find("[name='EquipmentCertificateOwnerId']").val(result.equipmentCertificateOwnerId);
                        formElements.find("[name='select_owner']").val(result.equipmentCertificateOwnerId).trigger("change");
                        formElements.find("[name='EquipmentCertificateTypeId']").val(result.equipmentCertificateTypeId);
                        formElements.find("[name='select_certificate']").val(result.equipmentCertificateTypeId).trigger("change");
                        formElements.find("[name='EquipmentCertificateRenewal.Id']").val(result.equipmentCertificateRenewal.id);
                        formElements.find("[name='EquipmentCertificateRenewal.Observation']").val(result.equipmentCertificateRenewal.observation);
                        formElements.find("[name='EquipmentCertificateRenewal.StartDate']").datepicker('setDate', result.equipmentCertificateRenewal.startDate);
                        formElements.find("[name='EquipmentCertificateRenewal.EndDate']").datepicker('setDate', result.equipmentCertificateRenewal.endDate);
                        formElements.find("[name='EquipmentCertificateRenewal.EquipmentCertifyingEntityId']").val(result.equipmentCertificateRenewal.equipmentcertifyingEntityId);
                        formElements.find("[name='select_entity']").val(result.equipmentCertificateRenewal.equipmentCertifyingEntityId).trigger("change");
                        formElements.find("[name='EquipmentCertificateRenewal.PatternCalibrationRenewalId']").val(result.equipmentCertificateRenewal.patternCalibrationRenewalId);
                        formElements.find("[name='select_pattern']").val(result.equipmentCertificateRenewal.patternCalibrationRenewalId).trigger("change");

                        formElements.find("[name='EquipmentCertificateRenewal.PatternCalibrationRenewalId']").val(result.equipmentCertificateRenewal.qualityFrontId);
                        formElements.find("[name='select_front']").val(result.equipmentCertificateRenewal.qualityFrontId).trigger("change");


                        formElements.find("[name='EquipmentCertificateRenewal.InspectionType']").val(result.equipmentCertificateRenewal.inspectionType);
                        formElements.find("[name='select_inspection']").val(result.equipmentCertificateRenewal.inspectionType).trigger("change");

                        formElements.find("[name='EquipmentCertificateRenewal.CalibrationMethod']").val(result.equipmentCertificateRenewal.calibrationMethod);
                        formElements.find("[name='select_method']").val(result.equipmentCertificateRenewal.calibrationMethod).trigger("change");

                        formElements.find("[name='EquipmentCertificateRenewal.CalibrationFrecuency']").val(result.equipmentCertificateRenewal.calibrationFrecuency);
                        formElements.find("[name='select_frecuency']").val(result.equipmentCertificateRenewal.calibrationFrecuency).trigger("change");

                        formElements.find("[name='EquipmentCertificateRenewal.OperationalStatus']").val(result.equipmentCertificateRenewal.operationalStatus);
                        formElements.find("[name='select_operational']").val(result.equipmentCertificateRenewal.operationalStatus).trigger("change");
                        formElements.find("[name='EquipmentCertificateRenewal.SituationStatus']").val(result.equipmentCertificateRenewal.situationStatus);
                        formElements.find("[name='select_situational']").val(result.equipmentCertificateRenewal.situationStatus).trigger("change");
                        formElements.find("[name='EquipmentCertificateRenewal.EquipmentCertificateUserOperatorId']").val(result.equipmentCertificateRenewal.equipmentCertificateUserOperatorId);
                        formElements.find("[name='select_operator']").val(result.equipmentCertificateRenewal.equipmentCertificateUserOperatorId).trigger("change");
                        if (result.equipmentCertificateRenewal.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            addren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#renewal_form");
                        formElements.find("[name='EquipmentCertificateId']").val(result.equipmentCertificateId);
                        formElements.find("[name='StartDate']").datepicker('setDate', result.endDate);
                        $("#add_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='select_project']").val());
                $(formElement).find("[name='EquipmentCertificateOwnerId']").val($(formElement).find("[name='select_owner']").val());
                $(formElement).find("[name='EquipmentCertificateTypeId']").val($(formElement).find("[name='select_certificate']").val());

                $(formElement).find("[name='EquipmentCertificateRenewal.InspectionType']").val($(formElement).find("[name='select_inspection']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.CalibrationMethod']").val($(formElement).find("[name='select_method']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.CalibrationFrecuency']").val($(formElement).find("[name='select_frecuency']").val());

                $(formElement).find("[name='EquipmentCertificateRenewal.OperationalStatus']").val($(formElement).find("[name='select_operational']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.SituationStatus']").val($(formElement).find("[name='select_situational']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.EquipmentCertificateUserOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.EquipmentCertifyingEntityId']").val($(formElement).find("[name='select_entity']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.PatternCalibrationRenewalId']").val($(formElement).find("[name='select_pattern']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.QualityFrontId']").val($(formElement).find("[name='select_front']").val());
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
                    url: _app.parseUrl("/calidad/equipos-certificados/crear"),
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
                        dataTables.bondAddsDt.reload();
                        $("#add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#detail_alert_text").html(error.responseText);
                            $("#detail_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editren: function (formElement) {
                $(formElement).find("[name='InspectionType']").val($(formElement).find("[name='select_inspection']").val());
                $(formElement).find("[name='CalibrationMethod']").val($(formElement).find("[name='select_method']").val());
                $(formElement).find("[name='CalibrationFrecuency']").val($(formElement).find("[name='select_frecuency']").val());

                $(formElement).find("[name='OperationalStatus']").val($(formElement).find("[name='select_operational']").val());
                $(formElement).find("[name='SituationStatus']").val($(formElement).find("[name='select_situational']").val());
                $(formElement).find("[name='EquipmentCertificateUserOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentCertifyingEntityId']").val($(formElement).find("[name='select_entity']").val());
                $(formElement).find("[name='PatternCalibrationRenewalId']").val($(formElement).find("[name='select_pattern']").val());
                $(formElement).find("[name='QualityFrontId']").val($(formElement).find("[name='select_front']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/editar/${id}`),
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
                        dataTables.bondRensDt.reload();
                        $("#edit_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_ren_alert_text").html(error.responseText);
                            $("#edit_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addren: function (formElement) {
                console.log(renId);
                $(formElement).find("[name='RenewalId']").val(renId);
                $(formElement).find("[name='InspectionType']").val($(formElement).find("[name='select_inspection']").val());
                $(formElement).find("[name='CalibrationMethod']").val($(formElement).find("[name='select_method']").val());
                $(formElement).find("[name='CalibrationFrecuency']").val($(formElement).find("[name='select_frecuency']").val());

                $(formElement).find("[name='OperationalStatus']").val($(formElement).find("[name='select_operational']").val());
                $(formElement).find("[name='SituationStatus']").val($(formElement).find("[name='select_situational']").val());
                $(formElement).find("[name='EquipmentCertificateUserOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentCertifyingEntityId']").val($(formElement).find("[name='select_entity']").val());
                $(formElement).find("[name='PatternCalibrationRenewalId']").val($(formElement).find("[name='select_pattern']").val());
                $(formElement).find("[name='QualityFrontId']").val($(formElement).find("[name='select_front']").val());
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
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/crear`),
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
                        dataTables.bondAddsDt.reload();
                        $("#add_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_ren_alert_text").html(error.responseText);
                            $("#add_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='select_project']").val());
                $(formElement).find("[name='EquipmentCertificateOwnerId']").val($(formElement).find("[name='select_owner']").val());
                $(formElement).find("[name='EquipmentCertificateTypeId']").val($(formElement).find("[name='select_certificate']").val());
                $(formElement).find("[name='CalibrationVerificationMethod']").val($(formElement).find("[name='select_methods']").val());
                $(formElement).find("[name='CalibrationVerificationFrecuency']").val($(formElement).find("[name='select_frecuencies']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.InspectionType']").val($(formElement).find("[name='select_inspection']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.CalibrationMethod']").val($(formElement).find("[name='select_method']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.CalibrationFrecuency']").val($(formElement).find("[name='select_frecuency']").val());

                $(formElement).find("[name='EquipmentCertificateRenewal.OperationalStatus']").val($(formElement).find("[name='select_operational']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.SituationStatus']").val($(formElement).find("[name='select_situational']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.EquipmentCertificateUserOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.EquipmentCertifyingEntityId']").val($(formElement).find("[name='select_entity']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.PatternCalibrationRenewalId']").val($(formElement).find("[name='select_pattern']").val());
                $(formElement).find("[name='EquipmentCertificateRenewal.QualityFrontId']").val($(formElement).find("[name='select_front']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/editar/${id}`),
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
                    })
                    .done(function (result) {
                        dataTables.bondAddsDt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                select2.users.reload();
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
            editren: function () {
                editRenForm.reset();
                $("#edit_ren_form").trigger("reset");
                $("#edit_ren_alert").removeClass("show").addClass("d-none");
                dataTables.bondRensDt.reload();
                dataTables.bondAddsDt.reload();


                select2.users.reload();
                $("#detail_modal").modal("show");
            },
            addren: function () {
                addRenForm.reset();
                $("#renewal_form").trigger("reset");
                select2.users.reload();
                $("#add_ren_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                select2.users.reload();
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            pdf: function () {
                if (isFromDetail)
                    $("#detail_modal").modal("show");
            }
        }
    };

    var select2 = {
        init: function () {
            this.patterns.init();
            this.users.init();
            this.renewals.init();
            this.sewerGroups.init();
            this.operators.init();
            this.owners.init();
            this.certificates.init();
            this.entities.init();
            this.references.init();
            this.fronts.init();
        },
        patterns: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/patrones-por-fecha")
                }).done(function (result) {
                    $(".select2-patterns").select2({
                        data: result
                    });
                });
            }
        },
        //patterns: {
        //    init: function () {
        //        $(".select2-patterns").select2();
        //    },
        //    reload: function () {
        //        var initDate = $("#EquipmentCertificateRenewal_StartDate").val();
        //        var endDate = $("#EquipmentCertificateRenewal_EndDate").val();
        //        $.ajax({
        //            url: _app.parseUrl(`/select/patrones-por-fecha?initDate=${initDate}&endDate=${endDate}`)
        //        }).done(function (result) {
        //            $(".select2-patterns").select2({
        //                data: result
        //            });
        //        });
        //    }
        //},
        users: {
            init: function () {
                //$.ajax({
                //    url: _app.parseUrl("/select/usuarios")
                //}).done(function (result) {
                //    $(".select2-users").select2({
                //        data: result
                //    });
                //    $.ajax({
                //        url: _app.parseUrl("/finanzas/responsables/proyecto")
                //    }).done(function (result) {
                //        $(".select2-users").val(result.responsibles.toString().split(',')).trigger("change");
                //    });
                //});
                $.ajax({
                    url: _app.parseUrl("/calidad/responsables/proyecto")
                }).done(function (result) {
                    responsibles = result.responsibles.toString();
                    $("#EquipmentCertificateRenewal_Responsibles").val(result.responsibles.toString());
                    $("#Responsibles").val(result.responsibles.toString());
                });
            },
            reload: function () {
                $("#EquipmentCertificateRenewal_Responsibles").val(responsibles);
                $("#Responsibles").val(responsibles);
            }
        },
        renewals: {
            init: function () {
                $(".select2-renewals").select2();
            },
            reload: function (equid, renid) {
                console.log(equid);
                console.log(renid);
                $(".select2-renewals").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/equipos-certificados/renovaciones/${equid}`)
                }).done(function (result) {
                    $(".select2-renewals").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_renewal").val(renid).trigger("change");
                    $("#btn_LoadPdf").trigger("click");
                });
            }
        },
        sewerGroups: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result
                    });

                });
            }
        },
        operators: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/operadores")
                }).done(function (result) {
                    $(".select2-operators").select2({
                        data: result
                    });
                });
            }
        },
        owners: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/propietarios-lista")
                }).done(function (result) {
                    $(".select2-owners").select2({
                        data: result
                    });
                });
            }
        },
       entities: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/entidad-certificadora-lista")
                }).done(function (result) {
                    $(".select2-entities").select2({
                        data: result
                    });
                });
            }
        }, references: {
            init: function () {
                $(".select2-references").select2();
            },
            reload: function (id) {
                $(".select2-references").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/patrones-referencias?id=${id}`)
                }).done(function (result) {
                    $(".select2-references").select2({
                        data: result
                    });
                });
            }
        },
        certificates: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipo-de-certificado-lista")
                }).done(function (result) {
                    $(".select2-certificates").select2({
                        data: result
                    });
                });
            }
        },
        fronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes-de-calidad")
                }).done(function (result) {
                    $(".select2-fronts").select2({
                        data: result
                    });
                });
            }
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            editRenForm = $("#edit_ren_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editren(formElement);
                }
            });

            addRenForm = $("#renewal_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addren(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editren();
                });

            $("#add_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addren();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.pdf();
                });
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
            $("#btn_LoadPdf").on("click", function () {
                let id = $("#select_renewal").val();
                forms.load.pdf(id);
                console.log(id);
            });
            $("#btn_LoadRefPdf").on("click", function () {
                let id = $("#select_reference").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/equipos-certificados/renovaciones/patron/${id}`)
                }).done(function (result) {
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                })
            });
            $("#equtype_filter,#validity_filter,#hasavoid_filter,#situation_filter,#operation_filter").on("change", function () {
                dataTables.bondAddsDt.reload();
            });

            //$("#EquipmentCertificateRenewal_StartDate, #EquipmentCertificateRenewal_EndDate").on("change", function () {
            //    //select2.patterns.reload();
            //});
        }
    };

    return {
        init: function () {
            select2.init();
            dataTables.init();
            validate.init();
            modals.init();
            datepickers.init();
            events.init();
        }
    };
}();

$(function () {
    Provider.init();
});