var IsoStandard = function () {

    var isoStandardDatatable = null;
    var foldingDatatable = null;
    let a = true;
    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;

    var detailForm2 = null;
    var addFoldingForm2 = null;
    var editFoldingForm2 = null;

    var sgOption = new Option('--Seleccione una Formula--', null, true, true);
    var sgOption2 = new Option('--Seleccione un Frente--', null, true, true);
    var allOption = new Option('Todos', null, true, true);
    var allOption2 = new Option('Todos', null, true, true);
    var allOption3 = new Option('Todos', null, true, true);
    var softId = null;
    var fdetail = null;
    var importDataForm = null;
    var importFilesForm = null;
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/planos/listar"),
            data: function (d) {
                d.budgetId = $("#budget_filter").val();
                d.workFrontId = $("#work_filter").val();
                d.projectFormulaId = $("#formula_filter").val();
                d.specId = $("#spec_filter").val();
                d.versionId = $("#version_filter").val();
                d.phaseId = $("#phase_filter").val();
                d.typeId = $("#type_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-briefcase'></i>Reporte Excel",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    window.location = `/oficina-tecnica/planos/reporte-planos`;
                }
            }
        ],
        columns: [
            {
                title: "Título de presupuesto",
                data: "budgetTitleName"
            },
            {
                title: "Fase",
                data: "projectPhaseCode",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null)
                    tmp = row.projectPhaseCode + "-" + row.projectPhaseDescription
                    return tmp;
                }

            },
            {
                title: "Fórmula",
                data: "projectFormulaCode",
                render: function (data, type, row) {
                    var tmp = data + "-" + row.projectFormulaName;

                return tmp;
            }
            },
            {
                title: "Frente",
                data: "workFrontCode"
            },
            {
                title: "Especialidad",
                data: "specialityDescription"
            },
            {
                title: "Tipo de Plano",
                data: "blueprintTypeDescription"
            },
            {
                title: "Nombre",
                data: "name"
            },

            {
                title: "Código",
                data: "description"
            },

            {
                title: "Lámina",
                data :"sheet"
            },
            {
                title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    return tmp;
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
                    return tmp;
                }
            }
        ]
    };
    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/oficina-tecnica/folding-planos/listar`),
            data: function (d) {
                d.bpId = softId;
                d.versionId = $("#version_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Versión",
                data: "technicalVersion.description"
            },
            {
                title: "Archivo CAD",
                data: "cadUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.cadUrl != null) {
                        tmp += `<button data-id="${row.id}"  data-cad="${row.cadUrl}" class="btn btn-info btn-sm btn-icon btn-cad">`;
                        tmp += `<i class="fa fa-download"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Plano",
                data: "fileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.fileUrl != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Carta de Aprobación",
                data: "letter.fileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.letterId != null) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-letter">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Fecha de Aprobación",
                data: "blueprintDateStr"
            },
            {
                title: "QR",
                data: "id",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.code) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-qr">`;
                        tmp += `<i class="fa fa-qrcode"></i></button> `;
                    }
                    return tmp;
                }
            },

            {
                title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.blueprintId}" data-fid="${row.id}"class="btn btn-success btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
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
                    return tmp;
                }
            }
        ]

    };

    var foldingDetailDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/oficina-tecnica/folding-planos-detalle/listar`),
            data: function (d) {
                d.bpfId = fdetail;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fecha de entrega",
                data: "dateType"
            },

            {
                title: "Entregado a",
                data: "userName"
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
    var datatable = {
        init: function () {
            this.bprintDt.init();
            this.foldingDt.init();
            this.foldingDetailDt.init();
        },
        bprintDt: {
            init: function () {
                isoStandardDatatable = $("#iso_standard_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                isoStandardDatatable.ajax.reload();
            },
            initEvents: function () {


                isoStandardDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        softId = id;
                        datatable.foldingDt.reload();
                        form.load.detail(id);
                    });

                isoStandardDatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        softId = id;
                        datatable.foldingDt.reload();
                        form.load.foldingFor05(id);
                    });

                isoStandardDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });


                isoStandardDatatable.on("click",
                    ".btn-qr",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");




                        window.location = `/oficina-tecnica/planos/qr/${id}`;

                    });

                isoStandardDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Plano será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/planos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.bprintDt.reload();
                                            datatable.foldingDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Plano sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Plano"
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
                this.initEvents();
            },
            reload: function () {
                foldingDatatable.ajax.reload();
            },
            initEvents: function () {

                foldingDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let foldingid = $btn.data("fid");
                        fdetail = foldingid;
                        $("#detail_modal").modal("hide");
                        datatable.foldingDetailDt.reload();
                        form.load.foldingDetail(id);
                    });

                foldingDatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        fdetail = id;
                        $("#detail_modal").modal("hide");
                        datatable.foldingDetailDt.reload();
                        form.load.foldingDetailC(id);
                    });

                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        form.load.editfolding(id);
                    });


                foldingDatatable.on("click",
                    ".btn-qr",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");

                        window.location = `/oficina-tecnica/planos/qr/${id}`;

                    });

                foldingDatatable.on("click", ".btn-cad", function () {
                    let $btn = $(this);
                    let u = $btn.data("cad");
                    window.location = u;

                });

                foldingDatatable.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.pdf(id);
                    $("#detail_modal").modal("hide");
                });

                foldingDatatable.on("click", ".btn-letter", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.pdfletter(id);
                    $("#detail_modal").modal("hide");
                });

                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Plano será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/folding-planos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.bprintDt.reload();
                                            datatable.foldingDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Plano sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Plano"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        foldingDetailDt: {
            init: function () {
                foldingDetailDatatable = $("#foldingdetail_datatable").DataTable(foldingDetailDtOpt);
                this.initEvents();
            },
            reload: function () {
                foldingDetailDatatable.ajax.reload();
            },
            initEvents: function () {
                foldingDetailDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        a = false;
                        $("#detail_modal2").modal("hide");
                        $("#detail_modal").modal("hide");
                        form.load.editfoldingdetail(id);
                    });

                

                foldingDetailDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Plano será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/folding-planos-detalle/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.bprintDt.reload();
                                            datatable.foldingDt.reload();
                                            datatable.foldingDetailDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Plano sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Plano"
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
                    url: _app.parseUrl(`/oficina-tecnica/planos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='Description']").attr("disabled", "disabled");
                        formElements.find("[name='Sheet']").val(result.sheet);
                        formElements.find("[name='Sheet']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingDetail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/planos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form2");
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='Description']").attr("disabled", "disabled");
                        formElements.find("[name='Sheet']").val(result.sheet);
                        formElements.find("[name='Sheet']").attr("disabled", "disabled");
                        $("#detail_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/planos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='BlueprintId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingDetailC: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form2");
                        formElements.find("[name='BlueprintFoldingId']").val(result.id);
                        $("#add_folding_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/planos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_formula']").val(result.projectFormulaId).trigger("change");
                        
                        formElements.find("[name='WorkFrontId']").val(result.workFrontId);
                        formElements.find("[name='select_front']").val(result.workFrontId).trigger("change");

                        formElements.find("[name='BlueprintTypeId']").val(result.blueprintTypeId);
                        formElements.find("[name='select_type']").val(result.blueprintTypeId).trigger("change");

                        select2.fronts.edit(result.projectFormulaId, result.workFrontId);
                        select2.fronts.edit(result.workFrontId, result.projectPhaseId);
                        formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId);
                        formElements.find("[name='select_budget']").val(result.budgetTitleId).trigger("change");

                        

                        formElements.find("[name='SpecialityId']").val(result.specialityId);
                        formElements.find("[name='select_spec']").val(result.specialityId).trigger("change");
                        select2.specs.edit(result.projectFormulaId, result.specialityId);

                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='Sheet']").val(result.sheet);
                        
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BlueprintId']").val(result.blueprintId);
                        formElements.find("[name='LetterId']").val(result.letterId);
                        formElements.find("[name='select_letter']").val(result.letterId).trigger("change");

                        formElements.find("[name='TechnicalVersionId']").val(result.technicalVersionId);
                        formElements.find("[name='select_version']").val(result.technicalVersionId).trigger("change");
                        formElements.find("[name='BlueprintDateStr']").datepicker("setDate", result.blueprintDateStr);

                        if (result.fileUrl) {
                            $("#edit_folding_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_folding_form [for='customFile']").text("Selecciona un archivo");
                        }


                        if (result.cadUrl) {
                            $("#edit_folding_form [for='customCadFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_folding_form [for='customCadFile']").text("Selecciona un archivo");
                        }

                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfoldingdetail: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos-detalle/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form2");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BlueprintFoldingId']").val(result.blueprintFoldingId);
                        formElements.find("[name='DateType']").datepicker("setDate", result.dateType);

                        formElements.find("[name='UserId']").val(result.userId);
                        formElements.find("[name='select_user']").val(result.userId).trigger("change");

                        $("#edit_folding_modal2").modal("show");

                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.code);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                   
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            pdfletter: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.letter.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.letter.fileUrl + "&embedded=true");
                    
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
        },
        submit: {
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
                        url: "/oficina-tecnica/planos/importar-datos",
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
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
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
                files: function (formElement) {
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
                        url: "/oficina-tecnica/planos/importar-archivos",
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
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
                        $("#import_files_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_files_alert_text").html(error.responseText);
                            $("#import_files_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }

            },
            add: function (formElement) {
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_front']").val());
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budget']").val());
                $(formElement).find("[name='SpecialityId']").val($(formElement).find("[name='select_spec']").val());


                $(formElement).find("[name='BlueprintTypeId']").val($(formElement).find("[name='select_type']").val());

                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phase']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/planos/crear"),
                    method: "post",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();

                        return xhr;
                    }
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
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
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_front']").val());
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budget']").val());
                $(formElement).find("[name='SpecialityId']").val($(formElement).find("[name='select_spec']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phase']").val());
                $(formElement).find("[name='BlueprintTypeId']").val($(formElement).find("[name='select_type']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");

                
                $(formElement).find("input").prop("disabled", true);
                
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/planos/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
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
                $(formElement).find("[name='TechnicalVersionId']").val($(formElement).find("[name='select_version']").val());
                $(formElement).find("[name='LetterId']").val($(formElement).find("[name='select_letter']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;

                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                //var emptyCad = $(formElement).find(".customCadFile").get(0).files.length === 0;
                //if (!emptyCad) {
                //    $(formElement).find(".custom-cad").append(
                //        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                //    $(".progress-bar").width("0%");
                //}

                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/crear`),
                    method: "post",
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
                    },
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                        //if (!emptyCad) {
                        //    $("#space-bar").remove();
                        //}
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
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
                $(formElement).find("[name='TechnicalVersionId']").val($(formElement).find("[name='select_version']").val());
                $(formElement).find("[name='LetterId']").val($(formElement).find("[name='select_letter']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                //var emptyCad = $(formElement).find(".customCadFile").get(0).files.length === 0;
                //if (!emptyCad) {
                //    $(formElement).find(".custom-cat").append(
                //        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                //    $(".progress-bar").width("0%");
                //}

                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos/editar/${id}`),
                    method: "put",
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
                    },
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                        //if (!emptyCad) {
                        //    $("#space-bar").remove();
                        //}
                    })
                    .done(function (result) {
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
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
                
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos-detalle/crear`),
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
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
                        datatable.foldingDetailDt.reload();
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
            
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/folding-planos-detalle/editar/${id}`),
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
                        datatable.bprintDt.reload();
                        datatable.foldingDt.reload();
                        datatable.foldingDetailDt.reload();
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
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
                files: function () {
                    importFilesForm.reset();
                    $("#import_files_form").trigger("reset");
                    $("#import_files_alert").removeClass("show").addClass("d-none");
                }
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
                datatable.foldingDt.reload();
                $("#detail_modal").modal("show");
            },


            addfolding2: function () {
                addFoldingForm2.reset();
                $("#add_folding_form2").trigger("reset");
                $("#add_folding_alert2").removeClass("show").addClass("d-none");
                $("#detail_modal").modal("hide");
                $("#detail_modal2").modal("show");

            },
            editfolding2: function () {
                editFoldingForm2.reset();
                $("#edit_folding_form2").trigger("reset");
                $("#edit_folding_alert2").removeClass("show").addClass("d-none");
                datatable.foldingDetailDt.reload();
                $("#detail_modal").modal("hide");
                $("#detail_modal2").modal("show");

            },

            detail2: function () {
                detailForm.reset();
                detailForm2.reset();
                datatable.foldingDt.reload();
                if (a == false)
                {
                    $("#detail_modal").modal("hide");
                    a = true;
                }
                else
                    $("#detail_modal").modal("show");
            },


            pdf: function () {
                    $("#detail_modal").modal("show");
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

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.data(formElement);
                }
            });
            importFilesForm = $("#import_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.files(formElement);
                }
            });

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            detailForm2 = $("#detail_form2").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
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
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#detail_modal2").on("hidden.bs.modal",
                function () {
                    form.reset.detail2();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                    
                });


            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.data();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.files();
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



            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    form.reset.pdf();
                });
        }
    };
    var select2 = {
        init: function () {
            this.users.init();
            this.formulas.init();
            this.types.init();
            this.fronts.init();
            this.budgets.init();
            this.versions.init();
            this.specs.init();
            this.phases.init();
            this.specs2.init();
            this.formulas2.init();
            this.works2.init();
            this.letters.init();
            this.phases2.init();
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
        formulas: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({

                    url: _app.parseUrl(`/select/formulas-proyecto-filtro?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-formulas").append(sgOption).trigger('change');
                    $(".select2-formulas").select2({
                        data: result
                        
                    });

                });
            }
        },

        types: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({

                    url: _app.parseUrl(`/select/tipos-de-planos?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-types").select2({
                        data: result

                    });

                });
            }
        },

        phases: {
            init: function () {
                $(".select2-phases").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/fases-por-proyecto-frente-trabajo?workFrontId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-phases").empty();
                    $(".select2-phases").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/fases-por-proyecto-frente-trabajo?workFrontId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-phases").empty();
                    $(".select2-phases").select2({
                        data: result
                    });
                    $(".select2-phases").val(eqsid).trigger('change');
                });
            },

        },
        fronts: {
            init: function () {
                $(".select2-fronts").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-fronts").empty();
                    $(".select2-fronts").append(sgOption).trigger('change');
                    $(".select2-fronts").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-fronts").empty();
                    $(".select2-fronts").select2({
                        data: result
                    });
                    $(".select2-fronts").val(eqsid).trigger('change');
                });
            },

        },
        formulas2: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/formulas-proyecto-filtro?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-formulas2").select2({
                        data: result
                    });
                });
            }
        },
        works2: {
            init: function () {
                $(".select2-works2").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-works2").empty();
                    $(".select2-works2").append(allOption).trigger('change');
                    $(".select2-works2").select2({
                        data: result
                    });
                });
            }
        },
        phases2: {
            init: function () {
                $(".select2-phases_filter").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/fases-por-proyecto-frente-trabajo?workFrontId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-phases_filter").empty();
                    $(".select2-phases_filter").append(allOption3).trigger('change');
                    $(".select2-phases_filter").select2({
                        data: result
                    });
                });
            }
        },
        specs2: {
            init: function () {
                $(".select2-specs2").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/especialidades-formula?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-specs2").empty();
                    $(".select2-specs2").append(allOption2).trigger('change');
                    $(".select2-specs2").select2({
                        data: result
                    });
                });
            }
        },
        specs: {
            init: function () {
                $(".select2-specs").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/especialidades-formula?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-specs").empty();
                    $(".select2-specs").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/especialidades-formula?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-specs").empty();
                    $(".select2-specs").select2({
                        data: result
                    });
                    $(".select2-specs").val(eqsid).trigger('change');
                });
            },

        },
        budgets: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/titulos-de-presupuesto-area-tecnica?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-budgets").select2({
                        data: result
                    });
                });
            }
        },
        versions: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    
                    url: _app.parseUrl(`/select/versiones-planos?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-versions").select2({
                        data: result
                    });
                });
            }
        },
        letters: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({

                    url: _app.parseUrl(`/select/cartas-recibidas?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-letters").select2({
                        data: result
                    });
                });
            }
        },
 
        
       
     
    };

    var events = {
        init: function () {

            $(".select2-formulas").on("change", function () {
                let formElements = $("#add_folding_form");
                select2.fronts.reload(this.value);

                select2.specs.reload(this.value);
            });

            $(".select2-formulas2").on("change", function () {
                
                select2.specs2.reload(this.value);
                select2.works2.reload(this.value);

            });

            $(".select2-fronts").on("change", function () {
                
                select2.phases.reload(this.value);

            });

            $(".select2-works2").on("change", function () {

            select2.phases2.reload(this.value);
            });
            $("#budget_filter,#work_filter,#formula_filter,#spec_filter,#version_filter,#phase_filter,#type_filter").on("change", function () {
                datatable.bprintDt.reload();
                datatable.foldingDt.reload();

            });


        },

        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/planos/excel-carga-masiva`;
            });
        },

    };
    return {
        init: function () {
            select2.init();
            datatable.init();
            datepicker.init();
            validate.init();
            modals.init();
            events.init();
            events.excel();
        }
    };
}();

$(function () {
    IsoStandard.init();
});