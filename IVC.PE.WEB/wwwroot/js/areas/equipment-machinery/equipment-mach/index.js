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
                show: [0, 1, 2, 6, 7, 8, 15, 16, 17, 18, 19,20,21],
                hide: [3, 4, 5, 9, 10, 11, 12, 13, 14]
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
                    window.location = `/equipos/equipo-maquinaria/reporte-equipos`;
                }
            }
        ],
        ajax: {
            url: _app.parseUrl("/equipos/equipo-maquinaria/listar"),
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

                data: "cubing",
                visible: false
            },


            {//10

                data: "statusDesc"
            },
            {//11
                data:"initHorometer"
            },
            {//12
                data: "endHorometer"
            },
            {//13
                data: "dif"
            },
            {//14

                data: "serviceConditionDesc",
                visible: false
            },
            {//15

                data: "unitPrice"
            },
            {//16

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

            {//17

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
            {//18

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

            {//19

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

            {//20

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
                        }else {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${row.lastValidityInsurance} días</span>
								</label>
							</span>`;
                        }


                    }

                }

            },
            {//21

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
            {//22

                data: "soatNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastStartDateSoatString;

                    return tmp;
                },
                visible: false
            },

            {//23

                data: "soatNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastEndDateSoatString;

                    return tmp;
                },
                visible: false
            },

            {//24

                data: "soatNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0) {
                        tmp += "No hay Foldings"
                        return tmp;
                    }
                    else {
                        if (row.lastValiditySoat > 30) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${row.lastValiditySoat} días</span>
								</label>
							</span>`;
                        } else if (row.lastValiditySoat > 15) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${row.lastValiditySoat} días</span>
								</label>
							</span>`;
                        } else if (row.lastValiditySoat > 0) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${row.lastValiditySoat} días</span>
								</label>
							</span>`;
                        } else if (!row.lastValiditySoat) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">No Aplica</span>
								</label>
							</span>`;
                        }else {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${row.lastValiditySoat} días</span>
								</label>
							</span>`;
                        }


                    }

                },
                visible: false

            },


            {//25

                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details2">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding2">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                },
                visible: false
            },

            {//26

                data: "tecNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastStartDateTechnicalString;

                    return tmp;
                },
                visible: false
            },

            {//27

                data: "tecNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0)
                        tmp += "No hay Foldings"
                    else
                        tmp += row.lastEndDateTechnicalString;

                    return tmp;
                },
                visible: false
            },

            {//28

                data: "tecNumber",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 0) {
                        tmp += "No hay Foldings"
                        return tmp;
                    }
                    else {
                        if (row.lastValidityTec > 30) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${row.lastValidityTec} días</span>
								</label>
							</span>`;
                        } else if (row.lastValidityTec > 15) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${row.lastValidityTec} días</span>
								</label>
							</span>`;
                        } else if (row.lastValidityTec > 0) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${row.lastValidityTec} días</span>
								</label>
							</span>`;
                        } else if (!row.lastValidityTec) {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">No Aplica</span>
								</label>
							</span>`;
                        }
                        else {
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${row.lastValidityTec} días</span>
								</label>
							</span>`;
                        }


                    }

                },
                visible: false

            },


            {//29

                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details3">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding3">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                },
                visible: false
            },



            {//30

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
            {//31

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
            url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/listar`),
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
            url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/listar`),
            data: function (d) {
                d.equipId = equipId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fecha Inicio",
                data: "startDateSoat"
            },
            {
                title: "Fecha Fin",
                data: "endDateSoat"
            },
            {
                title: "# de Folding",
                data: "orderSoat"

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

    var foldingDtOpt3 = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/listar`),
            data: function (d) {
                d.equipId = equipId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fecha Inicio",
                data: "startDateTechnical"
            },
            {
                title: "Fecha Fin",
                data: "endDateTechnical"
            },

            {
                title: "# de Folding",
                data: "orderTechnical"

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

    var datatables = {
        init: function () {
            this.for05Dt.init();
            this.foldingDt.init();
            this.foldingDt2.init();
            this.foldingDt3.init();
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
                    ".btn-details3",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt3.reload();
                        forms.load.detail3(id);
                    });

                for05Datatable.on("click",
                    ".btn-folding3",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt3.reload();
                        forms.load.foldingFor053(id);
                    });

                for05Datatable.on("click",
                    ".btn-qr",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        window.location = `/equipos/equipo-maquinaria/qr/${id}`;

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
                            text: "El Equipo Maquinaria será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/equipo-maquinaria/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Equipo Maquinaria ha sido eliminada con éxito",
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
                                        url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/eliminar/${id}`),
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


                foldingDatatable2.on
                    ("click", ".btn-pdf",
                        function () {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            forms.load.pdf2(id);
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
                                        url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/eliminar/${id}`),
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
        foldingDt3: {
            init: function () {
                foldingDatatable3 = $("#folding_datatable3").DataTable(foldingDtOpt3);
                this.events();
            },
            reload: function () {
                foldingDatatable3.ajax.reload();

            },
            events: function () {
                foldingDatatable3.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal3").modal("hide");
                        forms.load.editfolding3(id);
                    });

                foldingDatatable3.on
                    ("click", ".btn-pdf",
                        function () {
                            let $btn = $(this);
                            let id = $btn.data("id");
                            forms.load.pdf3(id);
                        });

                foldingDatatable3.on("click",
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
                                        url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt3.reload();
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
        }
    };
    var forms = {
        load: {
            edit: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`)
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
                        formElements.find("[name='Plate']").val(result.plate);
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

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`),
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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`),
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
            detail3: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form3");
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


                        $("#detail_modal3").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentMachId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor052: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form2");
                        formElements.find("[name='EquipmentMachId']").val(result.id);
                        $("#add_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor053: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form3");
                        formElements.find("[name='EquipmentMachId']").val(result.id);
                        $("#add_folding_modal3").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentMachId']").val(result.equipmentMachId);

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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form2");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentMachId']").val(result.equipmentMachId);

                        formElements.find("[name='ResponsiblesSoat']").val(result.responsiblesSoat.toString());

                        formElements.find("[name='StartDateSoat']").datepicker('setDate', result.startDateSoat);
                        formElements.find("[name='EndDateSoat']").datepicker('setDate', result.endDateSoat);
                        if (result.insuranceFileUrl) {
                            $("#edit_folding_form2 [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_folding_form2 [for='customFile']").text("Selecciona un archivo");
                        }

                        $("#edit_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding3: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form3");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentMachId']").val(result.equipmentMachId);

                        formElements.find("[name='ResponsiblesTec']").val(result.responsiblesTec.toString());

                        formElements.find("[name='StartDateTechnical']").datepicker('setDate', result.startDateTechnical);
                        formElements.find("[name='EndDateTechnical']").datepicker('setDate', result.endDateTechnical);
                        if (result.insuranceFileUrl) {
                            $("#edit_folding_form3 [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_folding_form3 [for='customFile']").text("Selecciona un archivo");
                        }

                        $("#edit_folding_modal3").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/${id}`)
                }).done(function (result) {

                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.insuranceFileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdf2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/${id}`)
                }).done(function (result) {

                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.soatFileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdf3: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/${id}`)
                }).done(function (result) {

                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.technicalFileUrl + "&embedded=true");

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
                $(formElement).find("[name='ServiceCondition']").val($(formElement).find("[name='select_service']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/equipo-maquinaria/crear"),
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
                $(formElement).find("[name='ServiceCondition']").val($(formElement).find("[name='select_service']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria/editar/${id}`),
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
                        url: "/equipos/equipo-maquinaria/importar-datos",
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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/crear`),
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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-seguro/editar/${id}`),
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
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/crear`),
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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-soat/editar/${id}`),
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
            addfolding3: function (formElement) {
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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/crear`),
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
                        datatables.foldingDt3.reload();
                        $("#add_folding_modal3").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding_alert_text3").html(error.responseText);
                            $("#add_folding_alert3").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding3: function (formElement) {


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
                    url: _app.parseUrl(`/equipos/equipo-maquinaria-tecnica/editar/${id}`),
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
                        datatables.foldingDt3.reload();
                        $("#edit_folding_modal3").modal("hide");
                        _app.show.notification.add.success();

                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding_alert_text3").html(error.responseText);
                            $("#edit_folding_alert3").removeClass("d-none").addClass("show");
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
                select2.reses.reload();
            },
            editfolding2: function () {
                editFoldingForm2.reset();
                $("#edit_folding_form2").trigger("reset");
                $("#edit_folding_alert2").removeClass("show").addClass("d-none");
                datatables.foldingDt2.reload();
                select2.reses.reload();
                $("#detail_modal2").modal("show");
            },
            addfolding3: function () {
                addFoldingForm3.reset();
                $("#add_folding_form3").trigger("reset");
                $("#add_folding_alert3").removeClass("show").addClass("d-none");
                select2.reses.reload();
            },
            editfolding3: function () {
                editFoldingForm3.reset();
                $("#edit_folding_form3").trigger("reset");
                $("#edit_folding_alert3").removeClass("show").addClass("d-none");
                datatables.foldingDt3.reload();
                select2.reses.reload();
                $("#detail_modal3").modal("show");
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
            detailForm3 = $("#detail_modal3").validate({
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
            this.workers.init();
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
                    $("#ResponsiblesSoat").val(result.responsibles.toString());
                    $("#ResponsiblesTec").val(result.responsibles.toString());
                });
            },
            reload: function () {

                $("#Responsibles").val(responsibles);
                $("#ResponsiblesSoat").val(responsibles);
                $("#ResponsiblesTec").val(responsibles);
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
                    url: _app.parseUrl("/select/proveedores-de-equipos-maquinaria")
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
                    url: _app.parseUrl("/select/equipos-maquinaria-filtro")
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
                    url: _app.parseUrl("/select/proveedores-de-equipos-maquinaria")
                }).done(function (result) {
                    $(".select2-equipproviders").append(selectSGOption).trigger('change');
                    $(".select2-equipproviders").select2({
                        data: result
                    });

                });
            }
        },
        //sos: {
        //    init: function () {
        //        $.ajax({
        //            url: _app.parseUrl("/select/foldings-proveedor-seleccionado")
        //        }).done(function (result) {
        //            $(".select2-softs").select2({
        //                data: result
        //            });
        //        });
        //    }
        //},
        softs: {
            init: function () {
                $(".select2-softs").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-proveedor-seleccionado-maquinaria?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            },

            reload2: function (id, actId) {
                $.ajax({
                    url: _app.parseUrl(`/select/foldings-proveedor-seleccionado-maquinaria?equipmentProviderId=${id}`)
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
                    url: _app.parseUrl("/select/tipos-de-maquinaria-maquinaria")
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
        workers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/obreros")
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result
                    });
                });
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

            $(".select2-hirings").on("change", function () {
                var txt = $(".select2-hirings option:selected").text();
                if (txt.indexOf("Obrero") >= 0) {
                    $(".worker_group").attr("hidden", false);
                    $(".operator_group").attr("hidden", true);
                    $(".operator_group_2").attr("hidden", true);
                    $(".operator_group_3").attr("hidden", true);
                } else if (txt.indexOf("Obrero") <= 0) {
                    $(".worker_group").attr("hidden", true);
                    $(".operator_group").attr("hidden", false);
                    $(".operator_group_2").attr("hidden", false);
                    $(".operator_group_3").attr("hidden", false);
                }
            });

            $(".select2-machinerys").on("change", function () {
                var txt = $(".select2-machinerys option:selected").text();
                if (txt.indexOf("Maquinaria") >= 0) {
                    $(".soft_group").attr("hidden", true);
                    $(".type_group").attr("hidden", false);
                } else if (txt.indexOf("Maquinaria") <= 0) {
                    $(".soft_group").attr("hidden", false);
                    $(".type_group").attr("hidden", true);

                }
            });

            $(".select2-isfroms").on("change", function () {
                var txt = $(".select2-isfroms option:selected").text();
                if (txt.indexOf("Tercero") >= 0) {
                    $(".is-from-ivc").attr("hidden", true);
                    $(".is-from-another").attr("hidden", false);

                } else if (txt.indexOf("Tercero") <= 0) {
                    $(".is-from-ivc").attr("hidden", false);
                    $(".is-from-another").attr("hidden", true);
                }
            });

            $("#soft_filter, #provider_filter").on("change", function () {
                for05Datatable.ajax.reload();
            });
        },
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/equipos/equipo-maquinaria/excel-carga-masiva`;
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