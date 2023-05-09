var Provider = function () {

    var mainDatatable = null;
    var foldingDatatable = null;
    var foldingDatatable2 = null;
    var detailDatatables = {};
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var addFileForm = null;
    var providerId = null;

    var detailForm = null;
    var detailForm2 = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var addFoldingForm2 = null;
    var editFoldingForm2 = null;
    var equipId = null;
    var fId = null;


    var options = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 4, 12],
                hide: [5,6,7,8,9,10,11]
            },

            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: ':hidden'
            }
        ],
        ajax: {
            url: _app.parseUrl("/logistica/empresas/listar"),
            dataSrc: "",
            data: function (d) {
                //d.supplyFamilyId = $("#supply_family_filter").val();
               // d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            }
        },
        columns: [
            //{
            //    title: "Detalle",
            //    width: "5%",
            //    className: "details-control",
            //    orderable: false,
            //    data: null,
            //    defaultContent: "<i class='flaticon2-next'></i>"
            //},
            //0
            {
                title: "Código",
                data: "code"
            },
            //1
            {
                title: "Razón Social",
                data: "businessName"
            },
            //2
            {
                title: "RUC",
                data: "ruc"
            },
            //3
            {
                title: "Dirección",
                data: "address"
            },
            //4
            {
                title: "Fecha de Creación",
                data: "createDateString"
            },
            //5
            {
                title: "Teléfono",
                data: "phoneNumber"
                , visible: false
            },
            //6
            {
                title: "Persona de Contacto",
                data: "collectionAreaContactName"
                , visible: false
            },
            //7
            //{
            //    title: "Representante Legal",
            //    data: "isActive",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (row.isActive == 1) {
            //            tmp += row.legalAgent
            //        }
            //        else if (row.isActive2 == 1) {
            //            tmp += row.legalAgent2
            //        }
            //        else if (row.isActive3 == 1) {
            //            tmp += row.legalAgent3
            //        } else if (row.isActive4 == 1) {
            //            tmp += row.legalAgent4
            //        } else if (row.isActive5 == 1) {
            //            tmp += row.legalAgent5
            //        } 
            //        return tmp;
            //    }
            //},
            //{
            //    title: "Desde",
            //    data: "isActive",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (row.isActive == 1) {
            //            tmp += row.fromDate
            //        }
            //        else if (row.isActive2 == 1) {
            //            tmp += row.fromDate2
            //        }
            //        else if (row.isActive3 == 1) {
            //            tmp += row.fromDate3
            //        } else if (row.isActive4 == 1) {
            //            tmp += row.fromDate4
            //        } else if (row.isActive5 == 1) {
            //            tmp += row.fromDate5
            //        }
            //        return tmp;
            //    }
            //},
            //{
            //    title: "Hasta",
            //    data: "isActive",
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        if (row.isActive == 1) {
            //            tmp += row.toDate
            //        }
            //        else if (row.isActive2 == 1) {
            //            tmp += row.toDate2
            //        }
            //        else if (row.isActive3 == 1) {
            //            tmp += row.toDate3
            //        } else if (row.isActive4 == 1) {
            //            tmp += row.toDate4
            //        } else if (row.isActive5 == 1) {
            //            tmp += row.toDate5
            //        }
            //        return tmp;
            //    }
            //},
            {
                title: "Documento RUC",
                data: "rucUrl",
                visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="RUC" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
            //8
            {
                title: "Testimonio Adjunto",
                data: "testimonyUrl", visible: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Testimonio" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button>  `;
                    }
                    return tmp;
                }
            },
            //9
            {//15
                title: "Folding RL",
                visible: false,
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
            //10
            {
                title: "Representante Legal",
                data: "legalAgent",
                visible: false
            },
            //11
            {//15
                title: "Folding Participación",
                visible: false,
                data: "type",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 2) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-detail-prt">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding-prt">`;
                        tmp += `<i class="fa fa-history"></i></button> `;
                    }
                    return tmp;
                },
            },
            //12
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

    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/logistica/empresas-representante/listar`),
            data: function (d) {
                d.equipId = equipId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "N°",
                data: "order"
            },
            {
                title: "Representante Legal",
                data: "legalAgent"
            },
            {
                title: "Desde",
                data: "fromDateString"
            },
            {
                title: "Hasta",
                data:"toDateString"
            },
            {
                title: "¿Actual?",
                data: "isActive",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1) {
                        tmp += "SI";
                    }
                    else {
                        tmp += "NO";
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
                    //tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    //tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]

    };
    var foldingDtOpt2 = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/logistica/empresas-participacion/listar`),
            data: function (d) {
                d.fId = fId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "N°",
                data: "order"
            },
            {
                title: "Participación de IVC (%)",
                data: "ivcParticipation"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Testimonio Adjunto",
                data: "testimonyUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-name="Testimonio" data-url="${data}" data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
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
            this.foldingDt2.init();
            this.datatable.init();
        },
        datatable: {
            init: function () {
                mainDatatable = $("#main_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                mainDatatable.ajax.reload();
            },
            initEvents: function () {


                mainDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt.reload();
                        form.load.detail(id);
                    });

                mainDatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        form.load.foldingFor05(id);
                    });

                mainDatatable.on("click",
                    ".btn-detail-prt",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        fId = id;
                        datatables.foldingDt2.reload();
                        form.load.detail2(id);
                    });

                mainDatatable.on("click",
                    ".btn-folding-prt",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt2.reload();
                        form.load.foldingFor052(id);
                    });

                mainDatatable.on("click", "td.details-control", function () {
                    var tr = $(this).closest("tr");
                    var row = mainDatatable.row(tr);
                    if (row.child.isShown()) {
                        row.child.hide();
                        $(this).html("<i class='flaticon2-next'></i>");
                        tr.removeClass('shown');
                    }
                    else {
                        row.child(datatable.child.init(row.data())).show();
                        $(this).html("<i class='flaticon2-down'></i>");
                        tr.addClass('shown');
                    }
                });

                mainDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                mainDatatable.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let url = $btn.data("url");
                    let sname = $btn.data("name");
                    form.load.pdf(id, url, sname);
                });

                mainDatatable.on("click",
                    ".btn-add-file",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        providerId = id;
                        $("#add_file_modal").modal("show");
                    });

                mainDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La empresa será eliminada permanentemente",
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
                                        url: _app.parseUrl(`/logistica/empresas/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El proveedor ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar al proveedor"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

            },
            //child: {
            //    init: function (data) {
            //        var optionsDetail = genOptionsDetails(data.id);
            //        var $table = $(`<table id="items_${data.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
            //        detailDatatables[data.id] = $table.DataTable(optionsDetail);
            //        this.initEvents($table);
            //        return $table;
            //    },
            //    reload: function (id) {
            //        var dt = detailDatatables[id];
            //        if (dt) {
            //            dt.ajax.reload();
            //        }
            //    },
            //    initEvents: function (table) {
            //        table.on("click", ".btn-view", function () {
            //            let $btn = $(this);
            //            let url = $btn.data("url");
            //            form.load.file.pdf(url);
            //        });

            //        table.on("click", ".btn-file-delete", function () {
            //            let $btn = $(this);
            //            let id = $btn.data("id");
            //            let providerId = $btn.data("providerId");
            //            Swal.fire({
            //                title: "¿Está seguro?",
            //                text: "El archivo será eliminado permanentemente",
            //                type: "warning",
            //                showCancelButton: true,
            //                confirmButtonText: "Sí, eliminarla",
            //                confirmButtonClass: "btn-danger",
            //                cancelButtonText: "Cancelar",
            //                showLoaderOnConfirm: true,
            //                allowOutsideClick: () => !swal.isLoading(),
            //                preConfirm: () => {
            //                    return new Promise((resolve) => {
            //                        $.ajax({
            //                            url: _app.parseUrl(`/logistica/empresas/${providerId}/archivos/eliminar/${id}`),
            //                            type: "delete",
            //                            success: function (result) {
            //                                datatable.child.reload(providerId);
            //                                swal.fire({
            //                                    type: "success",
            //                                    title: "Completado",
            //                                    text: "El archivo ha sido eliminado con éxito",
            //                                    confirmButtonText: "Excelente"
            //                                });
            //                            },
            //                            error: function (errormessage) {
            //                                swal.fire({
            //                                    type: "error",
            //                                    title: "Error",
            //                                    confirmButtonClass: "btn-danger",
            //                                    animation: false,
            //                                    customClass: 'animated tada',
            //                                    confirmButtonText: "Entendido",
            //                                    text: "Ocurrió un error al intentar eliminar al archivo"
            //                                });
            //                            }
            //                        });
            //                    });
            //                }
            //            });
            //        });
            //    }
            //}
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
                                        url: _app.parseUrl(`/logistica/empresas-representante/eliminar/${id}`),
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
                        form.load.editfolding2(id);
                    });

                foldingDatatable2.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let url = $btn.data("url");
                    let sname = $btn.data("name");
                    form.load.pdf(id, url, sname);
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
                                        url: _app.parseUrl(`/logistica/empresas-participacion/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt2.reload();
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
        }
    };
    var form = {
        load: {
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        
                        formElements.find("[name='Tradename']").val(result.tradename);
                        formElements.find("[name='Tradename']").attr("disabled", "disabled");



                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form2");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='Tradename']").val(result.tradename);
                        formElements.find("[name='Tradename']").attr("disabled", "disabled");



                        $("#detail_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='BusinessId']").val(result.id);

                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor052: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form2");
                        formElements.find("[name='BusinessId']").val(result.id);
                        $("#add_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas-representante/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='LegalAgent']").val(result.legalAgent);
                        formElements.find("[name='IsActive']").val(result.isActive.toString());
                        formElements.find("[name='FromDate']").datepicker('setDate', result.fromDate);
                        formElements.find("[name='ToDate']").datepicker('setDate', result.toDate);
                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding2: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas-participacion/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form2");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='BusinessId']").val(result.businessId);
                        formElements.find("[name='IvcParticipation']").val(result.ivcParticipation);
                        formElements.find("[name='Name']").val(result.name);
                        if (result.testimonyUrl) {
                            $("#edit_form [for='customFile2']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile2']").text("Selecciona un archivo");
                        }

                        $("#edit_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BusinessName']").val(result.businessName);
                        formElements.find("[name='Tradename']").val(result.tradename);
                        formElements.find("[name='RUC']").val(result.ruc);
                        formElements.find("[name='Address']").val(result.address);

                        formElements.find("[name='BusinessConsortium1']").val(result.businessConsortium1);
                        formElements.find("[name='select_business1']").val(result.businessConsortium1).trigger("change");

                        formElements.find("[name='Type']").val(result.type);
                        formElements.find("[name='select_type']").val(result.type).trigger("change");

                        formElements.find("[name='BusinessConsortium2']").val(result.businessConsortium2);
                        formElements.find("[name='select_business2']").val(result.businessConsortium2).trigger("change");

                        formElements.find("[name='BusinessConsortium3']").val(result.businessConsortium3);
                        formElements.find("[name='select_business3']").val(result.businessConsortium3).trigger("change");

                        formElements.find("[name='BusinessConsortium4']").val(result.businessConsortium4);
                        formElements.find("[name='select_business4']").val(result.businessConsortium4).trigger("change");

                        formElements.find("[name='BusinessConsortium5']").val(result.businessConsortium5);
                        formElements.find("[name='select_business5']").val(result.businessConsortium5).trigger("change");

                        

                        formElements.find("[name='CollectionAreaContactName']").val(result.collectionAreaContactName);
                        formElements.find("[name='CollectionAreaEmail']").val(result.collectionAreaEmail);
                        formElements.find("[name='CollectionAreaPhoneNumber']").val(result.collectionAreaPhoneNumber);

                        formElements.find("[name='CreateDate']").datepicker('setDate', result.createDate);

                        
                        if (result.rucUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        if (result.testimonyUrl) {
                            $("#edit_form [for='customFile2']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile2']").text("Selecciona un archivo");
                        }
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },

            pdf: function (id, url, sname) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(sname);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            file: {
                pdf: function (url) {
                    $("#preview_name").text("Empresa");
                    $("#preview_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                    $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${url}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(url)));
                    $(".btn-mailto").data("name", "Empresa").data("url", "https://docs.google.com/gview?url=" + encodeURI(url));
                    $("#preview_modal").modal("show");
                }
            }
        },
        submit: {
            addfolding: function (formElement) {
                $(formElement).find("[name='IsActive']").val($(formElement).find("[name='select_isactive']").val());
                
                let data = new FormData($(formElement).get(0));
                $(formElement).find("input, select").prop("disabled", true);
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
             
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas-representante/crear`),
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
            editfolding: function (formElement) {
                $(formElement).find("[name='IsActive']").val($(formElement).find("[name='select_isactive']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input, select").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas-representante/editar/${id}`),
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
            addfolding2: function (formElement) {

                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                var emptyDni = $(formElement).find(".customFile2").get(0).files.length === 0;
                if (!emptyDni) {
                    $(formElement).find(".custom-file-dni").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/logistica/empresas-participacion/crear"),
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

                    })
                    .done(function (result) {
                        datatables.datatable.reload();
                        $("#add_folding_modal2").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding2_alert_text").html(error.responseText);
                            $("#add_folding2_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding2: function (formElement) {
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                var emptyDni = $(formElement).find(".customFile2").get(0).files.length === 0;
                if (!emptyDni) {
                    $(formElement).find(".custom-file-dni").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas-participacion/editar/${id}`),
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

                    })
                    .done(function (result) {
                        datatables.datatable.reload();
                        $("#edit_folding_modal2").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding2_alert_text").html(error.responseText);
                            $("#edit_folding2_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            add: function (formElement) {
                $(formElement).find("[name='BusinessConsortium1']").val($(formElement).find("[name='select_business1']").val());
                $(formElement).find("[name='BusinessConsortium2']").val($(formElement).find("[name='select_business2']").val());
                $(formElement).find("[name='BusinessConsortium3']").val($(formElement).find("[name='select_business3']").val());
                $(formElement).find("[name='BusinessConsortium4']").val($(formElement).find("[name='select_business4']").val());
                $(formElement).find("[name='BusinessConsortium5']").val($(formElement).find("[name='select_business5']").val());
                $(formElement).find("[name='IsActive']").val($(formElement).find("[name='select_isactive']").val());
                $(formElement).find("[name='Type']").val($(formElement).find("[name='select_type']").val());
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

                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/logistica/empresas/crear"),
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
                        if (!emptyTitle) {
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
                $(formElement).find("[name='BusinessConsortium1']").val($(formElement).find("[name='select_business1']").val());
                $(formElement).find("[name='BusinessConsortium2']").val($(formElement).find("[name='select_business2']").val());
                $(formElement).find("[name='BusinessConsortium3']").val($(formElement).find("[name='select_business3']").val());
                $(formElement).find("[name='BusinessConsortium4']").val($(formElement).find("[name='select_business4']").val());
                $(formElement).find("[name='BusinessConsortium5']").val($(formElement).find("[name='select_business5']").val());
                $(formElement).find("[name='IsActive']").val($(formElement).find("[name='select_isactive']").val());
                $(formElement).find("[name='Type']").val($(formElement).find("[name='select_type']").val());
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

                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/empresas/editar/${id}`),
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
                        if (!emptyTitle) {
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
                    url: "/logistica/empresas/importar",
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
                    datatables.datatable.reload();
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
            file: {
                add: function (formElement) {
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
                        url: _app.parseUrl(`/logistica/empresas/${providerId}/archivos/crear`),
                        method: "post",
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
                            datatable.child.reload(providerId);
                            $("#add_file_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_file_alert_text").html(error.responseText);
                                $("#add_file_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#Add_BankId").prop("selectedIndex", 0).trigger("change");
                $("#Add_TaxBankId").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyGroupId").prop("selectedIndex", 0).trigger("change");
                $("#Add_BankAccountType").prop("selectedIndex", 0).trigger("change");
                $("#Add_BankAccountCurrency").prop("selectedIndex", 0).trigger("change");
                $("#Add_PropertyServiceType").prop("selectedIndex", 0).trigger("change");
            },  
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_BankId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_TaxBankId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyGroupId").prop("selectedIndex", 0).trigger("change");
                $("#Edit_BankAccountType").prop("selectedIndex", 0).trigger("change");
                $("#Edit_BankAccountCurrency").prop("selectedIndex", 0).trigger("change");
                $("#Edit_PropertyServiceType").prop("selectedIndex", 0).trigger("change");
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

            addfolding2: function () {
                addFoldingForm.reset();
                $("#add_folding_form2").trigger("reset");
                $("#add_folding2_alert").removeClass("show").addClass("d-none");

            },

            editfolding2: function () {
                editFoldingForm.reset();
                $("#edit_folding_form2").trigger("reset");
                $("#edit_folding2_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt2.reload();
                $("#detail_modal2").modal("show");
            },

            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            file: {
                add: function() {
                    addFileForm.resetForm();
                    $("#add_file_form").trigger("reset");
                    $("#add_file_form [name='Type']").prop("selectedIndex", 0).trigger("change");
                }
            }

        }
    };

    var select2 = {
        init: function () {
            this.businesses.init();
            this.fileTypes.init();
            this.accountTypes.init();
            this.currencies.init();
            this.propertyServices.init();
            this.banks.init();
            this.supplyFamilies.init();
            this.supplyGroups.init();
            this.isactive.init();
            this.isactive2.init();
            this.isactive3.init();
            this.isactive4.init();
            this.isactive5.init();
        },

        businesses: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empresas-comercial")
                }).done(function (result) {
                    $(".select2-businesses").select2({
                        data: result
                    });
                });
            }
        },

        isactive: {
            init: function () {
                $(".select2-isactive").select2();
            }
        },
        isactive2: {
            init: function () {
                $(".select2-isactive2").select2();
            }
        },
        isactive3: {
            init: function () {
                $(".select2-isactive3").select2();
            }
        },
        isactive4: {
            init: function () {
                $(".select2-isactive4").select2();
            }
        },
        isactive5: {
            init: function () {
                $(".select2-isactive5").select2();
            }
        },
        fileTypes: {
            init: function () {
                $(".select2-file-types").select2();
            }
        },
        accountTypes: {
            init: function () {
                $(".select2-account-types").select2();
            }
        },
        currencies: {
            init: function () {
                $(".select2-currencies").select2();
            }
        },
        propertyServices: {
            init: function () {
                $(".select2-property-services").select2();
            }
        },
        banks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/bancos")
                }).done(function (result) {
                    $(".select2-banks").select2({
                        data: result
                    });
                });
            }
        },
        supplyFamilies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
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

            addFoldingForm2 = $("#add_folding_form2").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addfolding2(formElement);
                }
            });

            editFoldingForm2 = $("#edit_folding_form2").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.editfolding2(formElement);
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

            addFileForm = $("#add_file_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.file.add(formElement);
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

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    form.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    form.reset.editfolding();
                });

            $("#add_folding_modal2").on("hidden.bs.modal",
                function () {
                    form.reset.addfolding2();
                });

            $("#edit_folding_modal2").on("hidden.bs.modal",
                function () {
                    form.reset.editfolding2();
                });

            $("#add_file_modal").on("hidden.bs.modal",
                function () {
                    form.reset.file.add();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
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
            $("#add_form [name='PropertyServiceType']").attr("id", "Add_PropertyServiceType");
            $("#edit_form [name='PropertyServiceType']").attr("id", "Edit_PropertyServiceType");
            $("#add_form [name='SupplyFamilyId']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='SupplyFamilyId']").attr("id", "Edit_SupplyFamilyId");
            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");
            $("#add_form [name='BankId']").attr("id", "Add_BankId");
            $("#edit_form [name='BankId']").attr("id", "Edit_BankId");
            $("#add_form [name='BankAccountType']").attr("id", "Add_BankAccountType");
            $("#edit_form [name='BankAccountType']").attr("id", "Edit_BankAccountType");
            $("#add_form [name='ForeignBankId']").attr("id", "Add_ForeignBankId");
            $("#edit_form [name='ForeignBankId']").attr("id", "Edit_ForeignBankId");
            $("#add_form [name='ForeignBankAccountType']").attr("id", "Add_ForeignBankAccountType");
            $("#edit_form [name='ForeignBankAccountType']").attr("id", "Edit_ForeignBankAccountType");
            $("#add_form [name='ForeignBankAccountCurrency']").attr("id", "Add_ForeignBankAccountCurrency");
            $("#edit_form [name='ForeignBankAccountCurrency']").attr("id", "Edit_ForeignBankAccountCurrency");
            $("#add_form [name='TaxBankId']").attr("id", "Add_TaxBankId");
            $("#edit_form [name='TaxBankId']").attr("id", "Edit_TaxBankId");

            /*$("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatable.reload();
            });*/

            $(".select2-types").on("change", function () {
                var txt = $(".select2-types option:selected").text();
                if (txt.indexOf("Consorcio") <= 0) {

                    
                    $(".con-group").attr("hidden", true);

                } if (txt.indexOf("Consorcio") >= 0) {

                    $(".con-group").attr("hidden", false);


                }

            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatables.init();
            validate.init();
            modals.init();

            datepickers.init();
        }
    };
}();

$(function () {
    Provider.init();
});