var for11 = function () {

    var addFirstForm = null;
    var addSecondForm = null;
    var addActionForm = null;
    var detailActionForm = null;
    var addEquipmentForm = null;
    var detailEquipmentForm = null;
    var addThirdForm = null;

    var for11Datatable = null;
    var actionDatatable = null;
    var equipmentDatatable = null;

    var for24PartOneId = null;
    var for24PartTwoId = null;

    var for11DtOpt = {
        responsive: false,
        scrollX: true,
        ajax: {
            url: _app.parseUrl("/sistema-de-manejo-integrado/for11-no-conformidad/listar"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "Nombre de Proceso",
                data: "sewerManifoldFor24FirstPart.newSIGProcess.processName"
            },
            {
                //tile: "Codigo Proceso",
                data: "sewerManifoldFor24FirstPart.newSIGProcess.code"
            },
            {
                //title: "Fecha",
                data: "sewerManifoldFor24FirstPart.date"
            },
            {
                //title: "Reportado por",
                data: "sewerManifoldFor24FirstPart.reportUserName"
            },
            {
                //title: "Origen NC",
                data: function (result) {
                    var opt = result.sewerManifoldFor24FirstPart.ncOrigin;
                    if (opt == 1)
                        return "Cliente";
                    else if (opt == 2)
                        return "Proveedor";
                    else 
                        return "Procesos Internos"
                }
            },
            {
                //title: "Opcion P c I",
                data: function (result) {
                    var SG = result.sewerManifoldFor24FirstPart.sewerGroup.code;
                    var P = result.sewerManifoldFor24FirstPart.provider.businessName;
                    var C = result.sewerManifoldFor24FirstPart.client;

                    if (SG != null)
                        return SG;
                    else if (P != null)
                        return P;
                    else
                        return C;
                }
            },
            {
                //title: "Descripción",
                data: "sewerManifoldFor24FirstPart.description",
            },
            {
                //title: "Responsable",
                data: "sewerManifoldFor24FirstPart.responsableUserName"
            },
            {
                data: "sewerManifoldFor24FirstPart.videoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Video" data-name="${row.sewerManifoldFor24FirstPart.newSIGProcess.processName}"  class="btn btn-secondary btn-sm btn-icon btn-play-first">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sewerManifoldFor24SecondPart.decision == 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24FirstPartId}" class="btn btn-success btn-sm btn-icon btn-second">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    if (row.sewerManifoldFor24FirstPart.gallery.length != 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24FirstPartId}" class="btn btn-info btn-sm btn-icon btn-gallery-first">`;
                        tmp += `<i class="fa fa-folder"></i></button>`;
                    }
                    if (row.sewerManifoldFor24FirstPart.fileUrl != null) {
                        tmp += `<button data-id="${row.sewerManifoldFor24FirstPartId}" class="btn btn-secondary btn-sm btn-icon btn-view-first">`;
                        tmp += `<i class="fa fa-eye"></i></button>`;
                    }
                    if (row.sewerManifoldFor24SecondPart.decision == 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24FirstPartId}" class="btn btn-danger btn-sm btn-icon btn-delete-first">`;
                        tmp += `<i class="fa fa-trash"></i></button>`;
                    }
                    return tmp;
                }
            },
            {
                //title:"Decision",
                data: function (result) {
                    var opt = result.sewerManifoldFor24SecondPart.decision;
                    if (opt == 1)
                        return "Rechazo";
                    else if (opt == 2)
                        return "Aceptación al Proveedor";
                    else if (opt == 3)
                        return "Retrabajo";
                    else if (opt == 4)
                        return "Corrección";
                    else if (opt == 5)
                        return "Cambio";
                    else if (opt == 6)
                        return "Reparación";
                    else
                        return result.sewerManifoldFor24SecondPart.other;

                }
            },
            {
                //title: "Acciones Tomadas",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sewerManifoldFor24SecondPart.decision != 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-info btn-sm btn-icon btn-action-folding">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                        tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-success btn-sm btn-icon btn-action">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Equipos",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sewerManifoldFor24SecondPart.decision != 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-info btn-sm btn-icon btn-equipment-folding">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                        tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-success btn-sm btn-icon btn-equipment">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title:"Fecha",ProposedDate
                data: "sewerManifoldFor24SecondPart.proposedDate"
            },
            {
                data: "sewerManifoldFor24SecondPart.videoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Video" data-name="${row.sewerManifoldFor24FirstPart.newSIGProcess.processName}"  class="btn btn-secondary btn-sm btn-icon btn-play-second">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sewerManifoldFor24SecondPart.decision != 0) {
                        if (row.sewerManifoldFor24ThirdPart.actionTaken == 0) {
                            tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-success btn-sm btn-icon btn-third">`;
                            tmp += `<i class="fa fa-history"></i></button> `;
                        }
                        if (row.sewerManifoldFor24SecondPart.gallery.length != 0) {
                            tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-info btn-sm btn-icon btn-gallery-second">`;
                            tmp += `<i class="fa fa-folder"></i></button>`;
                        }
                        if (row.sewerManifoldFor24SecondPart.fileUrl != null) {
                            tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-secondary btn-sm btn-icon btn-view-second">`;
                            tmp += `<i class="fa fa-eye"></i></button>`;
                        }
                        if (row.sewerManifoldFor24ThirdPart.actionTaken == 0) {
                            tmp += `<button data-id="${row.sewerManifoldFor24SecondPartId}" class="btn btn-danger btn-sm btn-icon btn-delete-second">`;
                            tmp += `<i class="fa fa-trash"></i></button>`;
                        }
                    }
                    return tmp;
                }
            },
            {
                //title: "Acciones Toamdas",
                data: function (result) {
                    var opt = result.sewerManifoldFor24ThirdPart.actionTaken;
                    if (opt == 1)
                        return "Observado";
                    else if (opt == 2)
                        return "Pendiente";
                    else if (opt == 3)
                        return "Levantada";
                    else
                        return "";
                }
            },
            {
                //title: "Deriva acción correctiva",
                data: function (result) {
                    var opt = result.sewerManifoldFor24ThirdPart.preventiveCorrectiveAction;
                    if (result.sewerManifoldFor24ThirdPart.actionTaken == 0)
                        return "";
                    else if (opt == true)
                        return "Sí";
                    else if (opt == false)
                        return "No";
                }
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sewerManifoldFor24ThirdPart.actionTaken != 0) {
                        tmp += `<button data-id="${row.sewerManifoldFor24ThirdpartId}" class="btn btn-secondary btn-sm btn-icon btn-view-third">`;
                        tmp += `<i class="fa fa-eye"></i></button>`;
                        tmp += `<button data-id="${row.sewerManifoldFor24ThirdpartId}" class="btn btn-danger btn-sm btn-icon btn-delete-third">`;
                        tmp += `<i class="fa fa-trash"></i></button>`;
                    }
                    return tmp;
                }
            }
        ]
    };

    var actionDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/sistema-de-manejo-integrado/accion-for24/listar`),
            data: function (d) {
                d.SewerManifoldFor24SecondPartId = for24PartTwoId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Acción Tomada",
                data: "actionName"
            },
            {
                title: "Fecha",
                data: "date"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var equipmentDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/sistema-de-manejo-integrado/equipo-for24/listar`),
            data: function (d) {
                d.SewerManifoldFor24SecondPartId = for24PartTwoId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Nombre del Equipo",
                data: "equipmentName"
            },
            {
                title: "Cantidad",
                data: "equipmentQuantity"
            },
            {
                title: "Horas Máquina",
                data: "equipmentHours"
            },
            {
                title: "Total Horas",
                data: "equipmentTotalHours"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.for11Dt.init();
            this.actionDt.init();
            this.equipmentDt.init();
        },
        for11Dt: {
            init: function () {
                for11Datatable = $("#for24s_datatable").DataTable(for11DtOpt);
                this.events();
            },
            reload: function () {
                for11Datatable.ajax.reload();
            },
            events: function () {
                /*SecondCreation*/
                for11Datatable.on("click",
                    ".btn-second",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.second(id);
                    });

                for11Datatable.on("click",
                    ".btn-third",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.third(id);
                    });
                /*Action*/
                for11Datatable.on("click",
                    ".btn-action",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.action(id);
                    });

                for11Datatable.on("click",
                    ".btn-action-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        for24PartTwoId = id;
                        forms.load.actiontable(id);
                    });
                /*Equipment*/
                for11Datatable.on("click",
                    ".btn-equipment",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.equipment(id);
                    });

                for11Datatable.on("click",
                    ".btn-equipment-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        for24PartTwoId = id;
                        forms.load.equipmenttable(id);
                    });
                /*Gallery*/
                for11Datatable.on("click",
                    ".btn-gallery-first",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.gallery.first(id);
                    });

                for11Datatable.on("click",
                    ".btn-gallery-second",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.gallery.second(id);
                    });

                /*PDF*/
                for11Datatable.on("click",
                    ".btn-view-first",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.pdf.first(id);
                    });

                for11Datatable.on("click",
                    ".btn-view-second",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.pdf.second(id);
                    });

                for11Datatable.on("click",
                    ".btn-view-third",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.pdf.third(id);
                    });

                /*Video*/
                for11Datatable.on("click",
                    ".btn-play-first",
                    function () {
                        let $btn = $(this);
                        let testName = $btn.data("test");
                        let smName = $btn.data("name");
                        let videourl = $btn.data("videourl");
                        forms.load.vid(videourl, `${testName} - ${smName}`);
                    });

                for11Datatable.on("click",
                    ".btn-play-second",
                    function () {
                        let $btn = $(this);
                        let testName = $btn.data("test");
                        let smName = $btn.data("name");
                        let videourl = $btn.data("videourl");
                        forms.load.vid(videourl, `${testName} - ${smName}`);
                    });

                for11Datatable.on("click",
                    ".btn-play-third",
                    function () {
                        let $btn = $(this);
                        let testName = $btn.data("test");
                        let smName = $btn.data("name");
                        let videourl = $btn.data("videourl");
                        forms.load.vid(videourl, `${testName} - ${smName}`);
                    });

                /*Delete*/
                for11Datatable.on("click",
                    ".btn-delete-first",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Estás Seguro?",
                            text: "La Parte I será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for11Dt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            })
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar la Parte I"
                                            });
                                        }
                                    });
                                });
                            },

                        })
                    });

                for11Datatable.on("click",
                    ".btn-delete-second",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Estás Seguro?",
                            text: "La Parte II será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/sistema-de-manejo-integrado/second-part-for24/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for11Dt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            })
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar la Parte I"
                                            });
                                        }
                                    });
                                });
                            },

                        })
                    });

                for11Datatable.on("click",
                    ".btn-delete-third",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Estás Seguro?",
                            text: "La Parte III será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/sistema-de-manejo-integrado/third-part-for24/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for11Dt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            })
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar la Parte III"
                                            });
                                        }
                                    });
                                });
                            },

                        })
                    });
            }
        },
        actionDt: {
            init: function () {
                actionDatatable = $("#actions_datatable").DataTable(actionDtOpt);
                this.events();
            },
            reload: function () {
                actionDatatable.ajax.reload();
            },
            events: function () {
                actionDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La Acción será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/sistema-de-manejo-integrado/accion-for24/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for11Dt.reload();
                                            datatables.actionDt.reload();
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
        },
        equipmentDt: {
            init: function () {
                equipmentDatatable = $("#equipments_datatable").DataTable(equipmentDtOpt);
                this.events();
            },
            reload: function () {
                equipmentDatatable.ajax.reload();
            },
            events: function () {
                equipmentDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El equipo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/sistema-de-manejo-integrado/equipo-for24/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for11Dt.reload();
                                            datatables.equipmentDt.reload();
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
            second: function (id) {
                let formElements = $("#add_second_form");

                formElements.find("[name='LaborerQuantity']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#LaTotalH").val(parseInt(data) * parseFloat(formElements.find("[name='LaborerHoursMan']").val()));                   
                });

                formElements.find("[name='LaborerHoursMan']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#LaTotalH").val(parseInt(formElements.find("[name='LaborerQuantity']").val()) * parseFloat(data));
                });

                formElements.find("[name='OfficialQuantity']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#OfTotalH").val(parseInt(data) * parseFloat(formElements.find("[name='OfficialHoursMan']").val()));
                });

                formElements.find("[name='OfficialHoursMan']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#OfTotalH").val(parseInt(formElements.find("[name='OfficialQuantity']").val()) * parseFloat(data));
                });

                formElements.find("[name='OperatorQuantity']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#OpTotalH").val(parseInt(data) * parseFloat(formElements.find("[name='OperatorHoursMan']").val()));
                });

                formElements.find("[name='OperatorHoursMan']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#OpTotalH").val(parseInt(formElements.find("[name='OperatorQuantity']").val()) * parseFloat(data));
                });

                $(".otro").hide();
                formElements.find("[name='select_decision']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    if (data == 7) {
                        $(".otro").show();
                    } else {
                        $(".otro").hide();
                    }

                });

                formElements.find("[name='SewerManifoldFor24FirstPartId']").val(id);
               
                $("#add_second_modal").modal("show");
            },
            action: function (id) {
                let formElements = $("#add_action_form");
                formElements.find("[name='SewerManifoldFor24SecondPartId']").val(id);
                $("#add_action_modal").modal("show");
            },
            actiontable: function (id) {
                datatables.actionDt.reload();
                $("#detail_action_modal").modal("show");
            },
            equipment: function (id) {
                let formElements = $("#add_equipment_form");
                formElements.find("[name='SewerManifoldFor24SecondPartId']").val(id);

                formElements.find("[name='EquipmentQuantity']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#EqTotalH").val(parseInt(data) * parseFloat(formElements.find("[name='EquipmentHours']").val()));
                });

                formElements.find("[name='EquipmentHours']").change(function () {
                    let data = $(this).val();
                    console.log(data);
                    formElements.find("#EqTotalH").val(parseInt(formElements.find("[name='EquipmentQuantity']").val()) * parseFloat(data));
                });

                $("#add_equipment_modal").modal("show");
            },
            equipmenttable: function (id) {
                datatables.equipmentDt.reload();
                $("#detail_equipment_modal").modal("show");
            },
            third: function (id) {
                let formElements = $("#add_third_form");
                formElements.find("[name='SewerManifoldFor24SecondPartId']").val(id);
                $("#add_third_modal").modal("show");
            },
            gallery: {
                first: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/${id}`)
                    }).done(function (result) {
                        var count = 1;
                        $('#container').empty();
                        result.gallery.forEach(function (image) {
                            console.log(image.url);
                            console.log(count);
                            if (count == 1)
                                $('#container').append('<div class="carousel-item active"><h5>' + image.name + '</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                            else
                                $('#container').append('<div class="carousel-item"><h5>' + image.name + '</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                            count++;
                        });
                        $("#gallery_modal").modal("show");
                    }).always(function () {
                        _app.loader.hide();
                    });
                },
                second: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/sistema-de-manejo-integrado/second-part-for24/${id}`)
                    }).done(function (result) {
                        var count = 1;
                        $('#container').empty();
                        result.gallery.forEach(function (image) {
                            console.log(image.url);
                            console.log(count);
                            if (count == 1)
                                $('#container').append('<div class="carousel-item active"><h5>' + image.name + '</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                            else
                                $('#container').append('<div class="carousel-item"><h5>' + image.name + '</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                            count++;
                        });
                        $("#gallery_modal").modal("show");
                    }).always(function () {
                        _app.loader.hide();
                    });
                }
            },
            pdf: {
                first: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/${id}`)
                    }).done(function (result) {
                        $("#pdf_name").text(" I");
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                        $("#pdf_modal").modal("show");
                    }).always(function () {
                        _app.loader.hide();
                    });
                },
                second: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/sistema-de-manejo-integrado/second-part-for24/${id}`)
                    }).done(function (result) {
                        $("#pdf_name").text(" II");
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                        console.log(result.fileUrl);

                        $("#pdf_modal").modal("show");
                    }).always(function () {
                        _app.loader.hide();
                    });
                },
                third: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/sistema-de-manejo-integrado/third-part-for24/${id}`)
                    }).done(function (result) {
                        $("#pdf_name").text(" III");
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                        console.log(result.fileUrl);

                        $("#pdf_modal").modal("show");
                    }).always(function () {
                        _app.loader.hide();
                    });
                }
            },
            vid: function (videourl, testName) {
                $("#test_name").html(testName);
                $("#video_frame").prop("src", videourl);
                $("#video_modal").modal("show");
            }
        },
        submit: {
            addFirst: function (formElement) {
                $(formElement).find("[name='NewSIGProcessId']").val($(formElement).find("[name='select_processes']").val());
                $(formElement).find("[name='ReportUserId']").val($(formElement).find("[name='select_report_user']").val());
                $(formElement).find("[name='OriginType']").val($(formElement).find("[name='select_origin_Type']").val());
                $(formElement).find("[name='NCOrigin']").val($(formElement).find("[name='select_nc_origin']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_group']").val());
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='ResponsableUserId']").val($(formElement).find("[name='select_responsable_user']").val());
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
                    url: _app.parseUrl("/sistema-de-manejo-integrado/first-part-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
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
                        datatables.for11Dt.reload();
                        $("#add_first_modal").modal("hide");
                        $(".select2-report-users").val('').trigger("change");
                        $(".select2-reponsable-users").val('').trigger("change");
                        $(".select2-processes").val('').trigger("change");
                        $(".select2-sewer-groups").val('').trigger("change");
                        $(".select2-providers").val('').trigger("change");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_first_alert_text").html(error.responseText);
                            $("#add_first_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addSecond: function (formElement) {
                $(formElement).find("[name='Decision']").val($(formElement).find("[name='select_decision']").val());
                $(formElement).find("[name='LaborerTotalHoursMan']").val($(formElement).find("#LaTotalH").val());
                $(formElement).find("[name='OfficialTotalHoursMan']").val($(formElement).find("#OfTotalH").val());
                $(formElement).find("[name='OperatorTotalHoursMan']").val($(formElement).find("#OpTotalH").val());
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
                console.log(for24PartOneId);
                $.ajax({
                    url: _app.parseUrl("/sistema-de-manejo-integrado/second-part-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
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
                        datatables.for11Dt.reload();
                        $("#add_second_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_second_alert_text").html(error.responseText);
                            $("#add_second_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addAction: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/sistema-de-manejo-integrado/accion-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for11Dt.reload();
                        $("#add_action_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_action_alert_text").html(error.responseText);
                            $("#add_action_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addEquipment: function (formElement) {
                $(formElement).find("[name='EquipmentTotalHours']").val($(formElement).find("#EqTotalH").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/sistema-de-manejo-integrado/equipo-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for11Dt.reload();
                        $("#add_equipment_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_equipment_alert_text").html(error.responseText);
                            $("#add_equipment_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addThird: function (formElement) {
                $(formElement).find("[name='ActionTaken']").val($(formElement).find("[name='select_taken']").val());
                $(formElement).find("[name='PreventiveCorrectiveAction']").val($(formElement).find("[name='select_action']").val());
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
                    url: _app.parseUrl("/sistema-de-manejo-integrado/third-part-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
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
                        datatables.for11Dt.reload();
                        $("#add_third_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_third_alert_text").html(error.responseText);
                            $("#add_third_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            addFirst: function () {
                addFirstForm.resetForm();
                $("#add_first_form").trigger("reset");
                $("#add_first_alert").removeClass("show").addClass("d-none");
            },
            addSecond: function () {
                addSecondForm.resetForm();
                $("#add_second_form").trigger("reset");
                $("#add_second_alert").removeClass("show").addClass("d-none");
            },
            addAction: function () {
                addActionForm.resetForm();
                $("#add_action_form").trigger("reset");
                $("#add_action_alert").removeClass("show").addClass("d-none");
            },
            addEquipment: function () {
                addEquipmentForm.resetForm();
                $("#add_equipment_form").trigger("reset");
                $("#add_equipment_alert").removeClass("show").addClass("d-none");
            },
            addThird: function () {
                addThirdForm.resetForm();
                $("#add_third_form").trigger("reset");
                $("#add_third_alert").removeClass("show").addClass("d-none");
            },
            vid: function () {
                $("#video_frame").prop("src", "");
            }
        }
    };

    var validate = {
        init: function () {
            addFirstForm = $("#add_first_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addFirst(formElement);
                }
            });

            addSecondForm = $("#add_second_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addSecond(formElement);
                }
            });

            addActionForm = $("#add_action_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addAction(formElement);
                }
            });

            detailActionForm = $("#detail_action_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addEquipmentForm = $("#add_equipment_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addEquipment(formElement);
                }
            });

            detailEquipmentForm = $("#detail_equipment_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addThirdForm = $("#add_third_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addThird(formElement);
                }
            });
            
        }
    };

    var modals = {
        init: function () {
            $("#add_first_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addFirst();
                });
            $("#add_second_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addSecond();
                });
            $("#add_action_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addAction();
                });
            $("#add_equipment_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addEquipment();
                });
            $("#add_third_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addThird();
                });

            $("#video_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.vid();
                });
        }
    };

    var select2 = {
        init: function () {
            this.users.init();
            this.sigProcess.init();
            this.sewerGroups.init();
            this.providers.init();
        },
        users: {
            init: function () {
                $(".select2-report-users").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-report-users").select2({
                        placeholder: "--Seleccionar--",
                        allowClear: true,
                        data: result
                    });
                });

                $(".select2-reponsable-users").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-reponsable-users").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        sigProcess: {
            init: function () {
                $(".select2-processes").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/sig-process")
                }).done(function (result) {
                    $(".select2-processes").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        sewerGroups: {
            init: function () {
                $(".select2-sewer-groups").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-jefe-frente")
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        providers: {
            init: function () {
                $(".select2-providers").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/proveedores")
                }).done(function (result) {
                    $(".select2-providers").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    var ncOriginOpts = {
        init: function () {
            $("#add_first_modal").find("[name='select_nc_origin']").trigger("change");
            $(".provider").hide();
            $(".internal-process").hide();
            $("#add_first_modal").find("[name='select_nc_origin']").change(function () {
                var data = $(this).val();
                console.log(data);
                if (data == 1) {
                    $(".client").show();
                    $(".select2-sewer-groups").val('').trigger("change");
                    $(".select2-providers").val('').trigger("change");
                    $(".provider").hide();
                    $(".internal-process").hide();
                } else if (data == 2) {
                    $(".provider").show();
                    $(".select2-sewer-groups").val('').trigger("change");
                    $("#add_modal").find("[name='Client']").val('');
                    $(".internal-process").hide();
                    $(".client").hide();
                } else if (data == 3) {
                    $(".internal-process").show();
                    $(".select2-providers").val('').trigger("change");
                    $("#add_modal").find("[name='Client']").val('');
                    $(".provider").hide();
                    $(".client").hide();
                }
            })
        }
    };

    var originTypeOpt = {
        init: function () {
            $("#add_first_modal").find("[name='select_origin_Type']").trigger("change");
            $("#add_first_modal").find("[name='select_origin_Type']").change(function () {
                var data = $(this).val();
                console.log(data);
                if (data == 1)
                    $(".product").show();
                else if (data == 2)
                    $(".product").hide();
            })
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
            ncOriginOpts.init();
            originTypeOpt.init();
        }
    };
}();

$(function () {
    for11.init();
});