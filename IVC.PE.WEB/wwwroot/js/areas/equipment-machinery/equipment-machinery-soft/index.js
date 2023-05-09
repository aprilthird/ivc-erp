var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var detailForm2 = null;
    var detailForm3 = null;
    var addFoldingForm = null;
    var editFoldingForm = null;

    var addFoldingForm2 = null;
    var editFoldingForm2 = null;
    var addFoldingForm3 = null;
    var editFoldingForm3 = null;
    var selectSGOption = new Option('--Seleccione un Proveedor--', null, true, true);
    var nullable = null;
    var responsibles = null;

    var for05Datatable = null;

    var foldingDatatable = null;
    var foldingDatatable2 = null;
    var foldingDatatable3 = null;

    var importDataForm = null;

    var equipId = null;
    var for05DtOpt = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2,3, 7, 9, 12,13,14,15,16,17],
                hide: [4, 5, 6,8, 10, 11,18,19,20]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: ':hidden'
            },
            {
                text: "<i class='fa fa-briefcase'></i>Reporte Excel",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    window.location = `/equipos/equipo-liviano/reporte-equipos`;
                }
            }
        ],
        ajax: {
            url: _app.parseUrl("/equipos/equipo-liviano/listar"),
            data: function (d) {

                d.equipmentProviderId = $("#provider_filter").val();
                d.equipmentMachineryTypeTypeId = $("#soft_filter").val();
                delete d.columns;

            },
            dataSrc: ""
        },
        columns: [
            { //0

                data: "tradeName",
            },
            { //1

                data: "description"
            },

            {   //2

                data: "brand",
            },
            {//3

                data: "model",
                visible: false
            },
            {//4

                data: "potency",
                visible: false
            },
            {//5

                data: "year",
                visible: false
            },
            {//6

                data: "serieNumber",

            },
            {//7

                data: "plate"
            },
            {//8

                data: "startDateString",

            },
            {//9

                data: "statusDesc"
            },

            {//10

                data: "unitPrice"
            },
            {//11

                data: "freeText"
            },
            {//12

                data: "insuranceNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        if (row.lastInsuranceNameDesc != null)
                            tmp += row.lastInsuranceNameDesc;
                        else
                            tmp += '';

                    return tmp;
                },
                visible: false
            },

            {//13

                data: "insuranceNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastInsuranceNumber;

                    return tmp;
                },
                visible: false
            },
            {//14

                data: "insuranceNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastStartDateInsuranceString;

                    return tmp;
                }
            },

            {//15

                data: "insuranceNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastEndDateInsuranceString;

                    return tmp;
                }
            },

            {//16

                data: "insuranceNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0) {
                        tmp += "No hay Foldings"
                        return tmp;
                    }
                    else {
                        if (row.lastValidityInsurance > 30) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${row.lastValidityInsurance} días</span>
								</label>
							</span>`;
                        } else if (row.lastValidityInsurance > 15) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${row.lastValidityInsurance} días</span>
								</label>
							</span>`;
                        } else if (row.lastValidityInsurance > 0) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${row.lastValidityInsurance} días</span>
								</label>
							</span>`;
                        }
                        else if (!row.lastValidityInsurance) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">No Aplica</span>
								</label>
							</span>`;
                        } else {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${row.lastValidityInsurance} días</span>
								</label>
							</span>`;
                        }


                    }

                }

            },
            {//17

                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                }
            },

            {//18

                data: "id",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.serieNumber) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-qr">`;
                        tmp += `<i class="fa fa-qrcode"></i></button> `;
                    }
                    return tmp;
                }
            },
            {//19

                data: "id",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.serieNumber) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details2">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding2">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    return tmp;
                }
            },
            {//20

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

    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/equipos/equipo-liviano-seguro/listar`),
            data: function (d) {
                d.equipId = equipId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Aseguradora",
                data: "insuranceEntity.description"
            },
            {
                title: "N° de Poliza",
                data: "number"
            },
            {
                title: "Fecha Inicio",
                data: "startDateInsurance"
            },
            {
                title: "Fecha Fin",
                data: "endDateInsurance"
            },
            {
                title: "# de Folding",
                data: "orderInsurance"

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
                    tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]

    };

    var foldingDtOpt2 = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/equipos/equipo-liviano-folding/listar`),
            data: function (d) {
                d.equipId = equipId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Observación",
                data: "freeText"
            },
            {
                title: "Fecha",
                data: "freeDate"
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
            this.for05Dt.init();
            this.foldingDt.init();
            this.foldingDt2.init();
        },
        for05Dt: {
            init: function () {
                for05Datatable = $("#main_datatable").DataTable(for05DtOpt);
                this.initEvents();
            },
            reload: function () {
                for05Datatable.ajax.reload();
            },
            initEvents: function () {

                for05Datatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt.reload();
                        forms.load.detail(id);
                    });

                for05Datatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        forms.load.foldingFor05(id);
                    });
                for05Datatable.on("click",
                    ".btn-details2",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt2.reload();
                        forms.load.detail2(id);
                    });

                for05Datatable.on("click",
                    ".btn-folding2",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt2.reload();
                        forms.load.foldingFor052(id);
                    });

                

                for05Datatable.on("click",
                    ".btn-qr",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        window.location = `/equipos/equipo-liviano/qr/${id}`;

                    });

                for05Datatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });


                for05Datatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Equipo liviano será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/equipo-liviano/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Equipo liviano ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Proveedor de Equipo"
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
                        forms.load.editfolding(id);
                    });

                foldingDatatable.on
                    ("click", ".btn-pdf",
                        function () {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            forms.load.pdf(id);
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
                                        url: _app.parseUrl(`/equipos/equipo-liviano-seguro/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt.reload();
                                            datatables.for05Dt.reload();
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
        foldingDt2: {
            init: function () {
                foldingDatatable2 = $("#folding_datatable2").DataTable(foldingDtOpt2);
                this.events();
            },
            reload: function () {
                foldingDatatable2.ajax.reload();

            },
            events: function () {
                foldingDatatable2.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal2").modal("hide");
                        forms.load.editfolding2(id);
                    });


                foldingDatatable2.on("click",
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
                                        url: _app.parseUrl(`/equipos/equipo-liviano-folding/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt2.reload();
                                            datatables.for05Dt.reload();
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
    var forms = {
        load: {
            edit: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_equipprovider']").val(result.equipmentProviderId).trigger("change");

                        select2.softs.reload2(result.equipmentProviderId, result.equipmentProviderFoldingId);
                        //formElements.find("[name='EquipmentProviderFoldingId']").val(result.equipmentProviderFoldingId);
                        //formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='Model']").val(result.model);
                        formElements.find("[name='Brand']").val(result.brand);
                        formElements.find("[name='Year']").val(result.year);
                        formElements.find("[name='EquipmentPlate']").val(result.equipmentPlate);
                        formElements.find("[name='SerieNumber']").val(result.serieNumber);
                        formElements.find("[name='StartDate']").datepicker("setDate", result.startDate);

                        formElements.find("[name='MachineryName']").val(result.machineryName);
                        formElements.find("[name='Potency']").val(result.potency);
                        formElements.find("[name='Cubing']").val(result.cubing);



                        formElements.find("[name='Status']").val(result.status);
                        formElements.find("[name='select_status']").val(result.status).trigger("change");
                        formElements.find("[name='ServiceCondition']").val(result.serviceCondition);
                        formElements.find("[name='select_service']").val(result.serviceCondition).trigger("change");
                        formElements.find("[name='UnitPrice']").val(result.unitPrice);
                        formElements.find("[name='FreeText']").val(result.freeText);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_equipprovider']").val(result.equipmentProviderId).trigger("change");
                        formElements.find("[name='select_equipprovider']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentProviderFoldingId']").val(result.equipmentProviderFoldingId);
                        formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='select_machinery_type_soft']").attr("disabled", "disabled");


                        formElements.find("[name='Model']").val(result.model);
                        formElements.find("[name='Model']").attr("disabled", "disabled");



                        formElements.find("[name='Brand']").val(result.brand);
                        formElements.find("[name='Brand']").attr("disabled", "disabled");



                        formElements.find("[name='EquipmentYear']").val(result.equipmentYear);
                        formElements.find("[name='EquipmentYear']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentPlate']").val(result.equipmentPlate);
                        formElements.find("[name='EquipmentPlate']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentSerie']").val(result.equipmentSerie);
                        formElements.find("[name='EquipmentSerie']").attr("disabled", "disabled");

                        formElements.find("[name='StartDate']").datepicker("setDate", result.startDate);
                        formElements.find("[name='StartDate']").attr("disabled", "disabled");


                        formElements.find("[name='Status']").val(result.status);
                        formElements.find("[name='select_status']").val(result.status).trigger("change");
                        formElements.find("[name='select_status']").attr("disabled", "disabled");


                        formElements.find("[name='ServiceCondition']").val(result.serviceCondition);
                        formElements.find("[name='select_service']").val(result.serviceCondition).trigger("change");
                        formElements.find("[name='select_service']").attr("disabled", "disabled");

                        formElements.find("[name='UnitPrice']").val(result.unitPrice);
                        formElements.find("[name='UnitPrice']").attr("disabled", "disabled");


                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form2");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_equipprovider']").val(result.equipmentProviderId).trigger("change");
                        formElements.find("[name='select_equipprovider']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentProviderFoldingId']").val(result.equipmentProviderFoldingId);
                        formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='select_machinery_type_soft']").attr("disabled", "disabled");


                        formElements.find("[name='Model']").val(result.model);
                        formElements.find("[name='Model']").attr("disabled", "disabled");



                        formElements.find("[name='Brand']").val(result.brand);
                        formElements.find("[name='Brand']").attr("disabled", "disabled");



                        formElements.find("[name='EquipmentYear']").val(result.equipmentYear);
                        formElements.find("[name='EquipmentYear']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentPlate']").val(result.equipmentPlate);
                        formElements.find("[name='EquipmentPlate']").attr("disabled", "disabled");


                        formElements.find("[name='EquipmentSerie']").val(result.equipmentSerie);
                        formElements.find("[name='EquipmentSerie']").attr("disabled", "disabled");

                        formElements.find("[name='StartDate']").datepicker("setDate", result.startDate);
                        formElements.find("[name='StartDate']").attr("disabled", "disabled");



                        formElements.find("[name='Status']").val(result.status);
                        formElements.find("[name='select_status']").val(result.status).trigger("change");
                        formElements.find("[name='select_status']").attr("disabled", "disabled");


                        formElements.find("[name='ServiceCondition']").val(result.serviceCondition);
                        formElements.find("[name='select_service']").val(result.serviceCondition).trigger("change");
                        formElements.find("[name='select_service']").attr("disabled", "disabled");

                        formElements.find("[name='UnitPrice']").val(result.unitPrice);
                        formElements.find("[name='UnitPrice']").attr("disabled", "disabled");


                        $("#detail_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor052: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form2");
                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.id);
                        $("#add_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano-seguro/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.EquipmentMachinerySoftId);

                        formElements.find("[name='Responsibles']").val(result.responsibles.toString());

                        formElements.find("[name='InsuranceEntityId']").val(result.insuranceEntityId);
                        formElements.find("[name='select_insurance']").val(result.insuranceEntityId).trigger("change");

                        formElements.find("[name='Number']").val(result.number);


                        formElements.find("[name='StartDateInsurance']").datepicker('setDate', result.startDateInsurance);
                        formElements.find("[name='EndDateInsurance']").datepicker('setDate', result.endDateInsurance);
                        if (result.insuranceFileUrl) {
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
            editfolding2: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano-folding/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form2");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.equipmentMachinerySoftId);

                        formElements.find("[name='FreeText']").val(result.freeText);

                        formElements.find("[name='FreeDate']").datepicker('setDate', result.freeDate);
                        
                        $("#edit_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano-seguro/${id}`)
                }).done(function (result) {

                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.insuranceFileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='EquipmentProviderId']").val($(formElement).find("[name='select_equipprovider']").val());
                $(formElement).find("[name='EquipmentProviderFoldingId']").val($(formElement).find("[name='select_machinery_type_soft']").val());

                $(formElement).find("[name='Status']").val($(formElement).find("[name='select_status']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/equipo-liviano/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);

                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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
                $(formElement).find("[name='EquipmentProviderId']").val($(formElement).find("[name='select_equipprovider']").val());
                $(formElement).find("[name='EquipmentProviderFoldingId']").val($(formElement).find("[name='select_machinery_type_soft']").val());

                $(formElement).find("[name='Status']").val($(formElement).find("[name='select_status']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data,

                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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
            import: {
                data: function (formElement) {
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
                        url: "/equipos/equipo-liviano/importar-datos",
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
                        datatables.for05Dt.reload();
                        $("#import_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_data_alert_text").html(error.responseText);
                            $("#import_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },

            },
            addfolding: function (formElement) {
                $(formElement).find("[name='InsuranceEntityId']").val($(formElement).find("[name='select_insurance']").val());
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
                    url: _app.parseUrl(`/equipos/equipo-liviano-seguro/crear`),
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
                        datatables.for05Dt.reload();
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

                $(formElement).find("[name='InsuranceEntityId']").val($(formElement).find("[name='select_insurance']").val());
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
                    url: _app.parseUrl(`/equipos/equipo-liviano-seguro/editar/${id}`),
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
                        datatables.for05Dt.reload();
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
            addfolding2: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano-folding/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();

                        return xhr;
                    }
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();

                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt2.reload();
                        $("#add_folding_modal2").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding_alert_text2").html(error.responseText);
                            $("#add_folding_alert2").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding2: function (formElement) {


                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-liviano-folding/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();

                        return xhr;
                    }
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
     
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt2.reload();
                        $("#edit_folding_modal2").modal("hide");
                        _app.show.notification.add.success();

                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding_alert_text2").html(error.responseText);
                            $("#edit_folding_alert2").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },

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
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
            },
            addfolding: function () {
                addFoldingForm.reset();
                $("#add_folding_form").trigger("reset");
                $("#add_folding_alert").removeClass("show").addClass("d-none");
                select2.reses.reload();
            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
                select2.reses.reload();
                $("#detail_modal").modal("show");
            },
            addfolding2: function () {
                addFoldingForm2.reset();
                $("#add_folding_form2").trigger("reset");
                $("#add_folding_alert2").removeClass("show").addClass("d-none");
 
            },
            editfolding2: function () {
                editFoldingForm2.reset();
                $("#edit_folding_form2").trigger("reset");
                $("#edit_folding_alert2").removeClass("show").addClass("d-none");
                datatables.foldingDt2.reload();
                $("#detail_modal2").modal("show");
            },

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

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
                }
            });

            detailForm = $("#detail_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            detailForm2 = $("#detail_modal2").validate({
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


            addFoldingForm2 = $("#add_folding_form2").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addfolding2(formElement);
                }
            });

            editFoldingForm2 = $("#edit_folding_form2").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editfolding2(formElement);
                }
            });


            addFoldingForm3 = $("#add_folding_form3").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addfolding3(formElement);
                }
            });

            editFoldingForm3 = $("#edit_folding_form3").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editfolding3(formElement);
                }
            });


        }
    };

    var select2 = {
        init: function () {
  
            this.softs.init();
            this.machineries.init();
            this.machinerys.init();
            this.providers.init();
            this.providers2.init();
            this.softs2.init();
            this.users.init();
            this.reses.init();
            this.styles.init();
            this.insurances.init();
        },

        reses: {
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
                    url: _app.parseUrl("/equipos/responsables/proyecto")
                }).done(function (result) {
                    responsibles = result.responsibles.toString();

                    $("#Responsibles").val(result.responsibles.toString());

                });
            },
            reload: function () {

                $("#Responsibles").val(responsibles);
   
            }
        },
        processes: {
            init: function () {
                $(".select2-users").empty();
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            },

        },
        insurances: {
            init: function () {
                $(".select2-insurances").empty();
                $.ajax({
                    url: _app.parseUrl("/select/entidades-aseguradoras")
                }).done(function (result) {
                    $(".select2-insurances").select2({
                        data: result
                    });
                });
            },

        },

        users: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            }
        },
        providers2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-liviano")
                }).done(function (result) {

                    $(".select2-providers2").select2({
                        data: result
                    });

                });
            }
        },
        softs2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/equipos-liviano-filtro")
                }).done(function (result) {

                    $(".select2-softs2").select2({
                        data: result
                    });

                });
            }
        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-liviano")
                }).done(function (result) {
                    $(".select2-equipproviders").append(selectSGOption).trigger('change');
                    $(".select2-equipproviders").select2({
                        data: result
                    });

                });
            }
        },

        softs: {
            init: function () {
                $(".select2-softs").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-proveedor-seleccionado-liviano?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            },

            reload2: function (id, actId) {
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-proveedor-seleccionado-liviano?equipmentProviderId=${id}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").select2({
                        data: result
                    });
                    $(".select2-softs").val(actId).trigger("change");
                });
            },

        },
        machinerys: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-lista")
                }).done(function (result) {
                    $(".select2-machinerys").select2({
                        data: result
                    });
                });
            }
        },
        machineries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-liviano")
                }).done(function (result) {
                    $(".select2-machineries").select2({
                        data: result
                    });
                });
            }
        },
        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
        },


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

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.data();
                });

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding();
                });

            $("#add_folding_modal2").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding2();
                });

            $("#edit_folding_modal2").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding2();
                });

            $("#add_folding_modal3").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding3();
                });

            $("#edit_folding_modal3").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding3();
                });


        }
    };
    var events = {
        init: function () {
            $(".select2-equipproviders").on("change", function () {
                select2.softs.reload(this.value);
            });






            $("#soft_filter, #provider_filter").on("change", function () {
                for05Datatable.ajax.reload();
            });
        },
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/equipos/equipo-liviano/excel-carga-masiva`;
            });
        },
    };
    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    //var events = {


    //};

    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
            events.init();
            events.excel();

        }
    };
}();

$(function () {
    For05.init();
});