var Provider = function () {

    var bondAddsDataTable = null;
    var bondRensDataTable = null;
    var addForm = null;
    var editForm = null;    
    var addRenForm = null;
    var editRenForm = null;

    var responsibles = null;

    var isFromDetail = false;

    var addId = null;
    var renId = null;

    var bondAddsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/cartas-fianza/listar"),
            data: function (d) {
                d.last = $("#last_filter").val();
                d.projectId = $("#project_filter").val();
                d.bankId = $("#bank_filter").val();
                d.bondTypeId = $("#supply_family_filter").val();
                d.bondGuarantorId = $("#bondguarantor_filter").val();
                d.budgetTitleId = $("#supply_group_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proyecto",
                data: "projectAbbreviation"
            },
            {
                title: "Garante",
                data: "bondGuarantor"
            },
            {
                title: "Fianza Nº",
                data: "bondName"
            },
            {
                title: "Título de Presupuesto",
                data: "budgetTitle"
            },
            {
                title: "Tipo de Fianza",
                data: "bondType",
                render: function (data, type, row) {
                    if (data == "Fiel Cumplimiento") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">${data}</span>
								</label>
							</span>`;
                    } else if (data.includes("Adelanto Directo") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">${data}</span>
								</label>
							</span>`;
                    } else if (data.includes("Adelanto de Materiales") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${data}</span>
								</label>
							</span>`;
                    }
                    else if (data.includes("Seriedad de Oferta") == true) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">${data}</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Entidad Bancaria",
                data: "bank"
            },
            {
                title: "# de Fianzas",
                data: "bondOrder"
            },

            {
                title: "Días para Vencimiento",
                data: "validity",
                render: function (data, type, row) {
                    if (row.isTheLast) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">Amortizada</span>
								</label>
							</span>`;
                    }

                    if (data > 30) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${data} días</span>
								</label>
							</span>`;
                    } else if (data > 15) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${data} días</span>
								</label>
							</span>`;
                    } else if (data > 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${data} días</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${data} días</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Inicio Vigencia",
                data: "createDateString"
            },
            {
                title: "Fin Vigencia",
                data: "endDateString"
            },
            {
                title: "Monto",
                data: "penAmmountFormatted"
            },
            {
                title: "Costo por Emisión",
                data: "issueCostSumFormatted"
            },
            {
                title: "Moneda",
                data: "currencyType"
            },
            {
                title: "Contra Garantía",
                data: "guaranteeDesc"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.bondRenovationId}" class="btn btn-success btn-sm btn-icon btn-renovation">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.bondRenovationId}" data-bondid="${row.bondAddId}" class="btn btn-secondary btn-sm btn-icon btn-pdf" data-toggle="tooltip" title="Ver Carta Fianza">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    tmp += `<button data-id="${row.bondRenovationId}" data-bondid="${row.bondAddId}" class="btn btn-primary btn-sm btn-icon btn-issue-pdf" data-toggle="tooltip" title="Ver Costo de Emisión">`;
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
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-briefcase'></i> Reporte Excel Por Centro de Costos",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-proyecto`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
                    });
                }
            },
            {
                text: "<i class='fa fa-piggy-bank'></i> Reporte Excel Por Bancos",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-banco`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
                    });
                }
            },
            {
                text: "<i class='fa fa-book'></i> Reporte Excel Histórico",
                className: "btn-info",
                action: function (e, dt, node, config) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/finanzas/cartas-fianza/reporte-excel-historico`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/finanzas/cartas-fianza/descargar-excel?excelName=${result}`;
                    });
                }
            }
        ]
    };

    var bondRensDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/cartas-fianza/renovaciones/listar"),
            data: function (d) {
                d.bondId = addId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Fianza Nº",
                data: "bondName"
            },
            {
                title: "Monto",
                data: "penAmmountFormatted"
            },
            {
                title: "Costo por Emisión",
                data: "issueCostFormatted"
            },
            {
                title: "Moneda",
                data: "currencyType"
            },
            {
                title: "Inicio Vigencia",
                data: "createDate"
            },
            {
                title: "Fin Vigencia",
                data: "endDate"
            },
            {
                title: "Contra Garantía",
                data: "guaranteeDesc"
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
                    tmp += `<button data-id="${row.id}" data-bondid="${row.bondAddId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    tmp += `<button data-id="${row.id}" data-bondid="${row.bondAddId}" class="btn btn-secondary btn-sm btn-icon btn-issue-pdf">`;
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
                bondAddsDataTable = $("#bondadds_datatable").DataTable(bondAddsDt_options);
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
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = false;
                        select2.bondRenovations.reload(bondid, renid);
                    });

                bondAddsDataTable.on("click",
                    ".btn-issue-pdf",
                    function () {
                        let $btn = $(this);
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = false;
                        select2.issues.reload(bondid, renid);
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
                            text: "La carta fianza y sus renovaciones serán eliminadas permanentemente.",
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
                                        url: _app.parseUrl(`/finanzas/cartas-fianza/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La carta fianza y sus renovaciones han sido eliminadas con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la carta fianza."
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
                bondRensDataTable = $("#bondrens_datatable").DataTable(bondRensDt_options);
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
                                        url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La crenovación ha sido eliminada con éxito",
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
                                                text: `Ocurrió un error al intentar eliminar la renovación. Motivo: ${errormessage.responseText}`
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
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = true;
                        select2.bondRenovations.reload(bondid, renid);
                        $("#detail_modal").modal("hide");
                    });

                bondRensDataTable.on("click",
                    ".btn-issue-pdf",
                    function () {
                        let $btn = $(this);
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = true;
                        select2.issues.reload(bondid, renid);
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='select_project']").val(result.projectId).trigger("change");
                        formElements.find("[name='select_project']").attr("disabled", "disabled");
                        formElements.find("[name='BondGuarantorId']").val(result.bondGuarantorId);
                        formElements.find("[name='select_bondguarantor']").val(result.bondGuarantorId).trigger("change");
                        formElements.find("[name='select_bondguarantor']").attr("disabled", "disabled");
                        formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId);
                        formElements.find("[name='select_budgettype']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_budgettype']").attr("disabled", "disabled");
                        formElements.find("[name='BondTypeId']").val(result.bondTypeId);
                        formElements.find("[name='select_bondtype']").val(result.bondTypeId).trigger("change");
                        formElements.find("[name='select_bondtype']").attr("disabled", "disabled");
                        formElements.find("[name='BankId']").val(result.bankId);
                        formElements.find("[name='select_bank']").val(result.bankId).trigger("change");
                        formElements.find("[name='select_bank']").attr("disabled", "disabled");
                        formElements.find("[name='BondNumber']").val(result.bondNumber);
                        formElements.find("[name='BondNumber']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });                
            },
            editren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_ren_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BondAddId']").val(result.bondAddId);
                        formElements.find("[name='Responsibles']").val(result.responsibles.toString());
                        formElements.find("[name='BondName']").val(result.bondName);
                        //formElements.find("[name='select_users']").val(result.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='PenAmmount']").val(result.penAmmount);
                        formElements.find("[name='IssueCost']").val(result.issueCost);
                        formElements.find("[name='currencyType']").val(result.currencyType);
                        formElements.find("[name='select_currencytype']").val(result.currencyType).trigger("change");
                        formElements.find("[name='CreateDate']").datepicker('setDate', result.createDate);
                        formElements.find("[name='EndDate']").datepicker('setDate', result.endDate);
                        formElements.find("[name='guaranteeDesc']").val(result.guaranteeDesc);
                        formElements.find("[name='IsTheLast']").val(result.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.isTheLast.toString()).trigger("change");
                        $("#edit_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                        $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                        $("#pdf_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });   
            },
            pdf2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.issueFileUrl + "&embedded=true");
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/${id}`),

                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='select_project']").val(result.projectId).trigger("change");
                        formElements.find("[name='BondGuarantorId']").val(result.bondGuarantorId);
                        formElements.find("[name='select_bondguarantor']").val(result.bondGuarantorId).trigger("change");
                        formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId);
                        formElements.find("[name='select_budgettype']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='BondTypeId']").val(result.bondTypeId);
                        formElements.find("[name='select_bondtype']").val(result.bondTypeId).trigger("change");
                        formElements.find("[name='BankId']").val(result.bankId);
                        formElements.find("[name='select_bank']").val(result.bankId).trigger("change");
                        formElements.find("[name='BondNumber']").val(result.bondNumber);
                        formElements.find("[name='BondRenovation.Id']").val(result.bondRenovation.id);
                        formElements.find("[name='BondRenovation.Responsibles']").val(result.bondRenovation.responsibles.toString());
                        formElements.find("[name='BondRenovation.BondName']").val(result.bondRenovation.bondName);
                        //formElements.find("[name='select_users']").val(result.bondRenovation.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='BondRenovation.PenAmmount']").val(result.bondRenovation.penAmmount);
                        formElements.find("[name='BondRenovation.IssueCost']").val(result.bondRenovation.issueCost);
                        formElements.find("[name='BondRenovation.currencyType']").val(result.bondRenovation.currencyType);
                        formElements.find("[name='select_currencytype']").val(result.bondRenovation.currencyType).trigger("change");
                        formElements.find("[name='BondRenovation.CreateDate']").datepicker('setDate', result.bondRenovation.createDate);
                        formElements.find("[name='BondRenovation.EndDate']").datepicker('setDate', result.bondRenovation.endDate);
                        formElements.find("[name='BondRenovation.guaranteeDesc']").val(result.bondRenovation.guaranteeDesc);
                        formElements.find("[name='BondRenovation.IsTheLast']").val(result.bondRenovation.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.bondRenovation.isTheLast.toString()).trigger("change");
                        if (result.bondRenovation.fileUrl) {
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#add_ren_form");
                        formElements.find("[name='BondAddId']").val(result.bondAddId);
                        formElements.find("[name='CreateDate']").datepicker('setDate', result.endDate);
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
                $(formElement).find("[name='BondGuarantorId']").val($(formElement).find("[name='select_bondguarantor']").val());
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budgettype']").val());
                $(formElement).find("[name='BondTypeId']").val($(formElement).find("[name='select_bondtype']").val());
                $(formElement).find("[name='BankId']").val($(formElement).find("[name='select_bank']").val());
                //$(formElement).find("[name='BondRenovation.Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='BondRenovation.currencyType']").val($(formElement).find("[name='select_currencytype']").val());
                $(formElement).find("[name='BondRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl("/finanzas/cartas-fianza/crear"),
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
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editren: function (formElement) {
                //$(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='currencyType']").val($(formElement).find("[name='select_currencytype']").val());
                $(formElement).find("[name='IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/editar/${id}`),
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
                $(formElement).find("[name='BondRenovationId']").val(renId);
                $(formElement).find("[name='IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());

                //$(formElement).find("[name='Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='currencyType']").val($(formElement).find("[name='select_currencytype']").val());
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/renovaciones/crear`),
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
                $(formElement).find("[name='BondGuarantorId']").val($(formElement).find("[name='select_bondguarantor']").val());
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budgettype']").val());
                $(formElement).find("[name='BondTypeId']").val($(formElement).find("[name='select_bondtype']").val());
                $(formElement).find("[name='BankId']").val($(formElement).find("[name='select_bank']").val());
                //$(formElement).find("[name='BondRenovation.Responsibles']").val($(formElement).find("[name='select_users']").val());
                $(formElement).find("[name='BondRenovation.currencyType']").val($(formElement).find("[name='select_currencytype']").val());
                $(formElement).find("[name='BondRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
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
                    url: _app.parseUrl(`/finanzas/cartas-fianza/editar/${id}`),
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
                $("#add_alert").removeClass("show").addClass("d-none");
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
                $("#add_ren_form").trigger("reset");
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
            },
            pdf2: function () {
                if (isFromDetail)
                    $("#detail_modal").modal("show");
            }
        }
    };

    var select2 = {
        init: function () {
            this.guarantors.init();
            this.budgetType.init();
            this.bondType.init();
            this.banks.init();
            this.users.init();
            this.currencies.init();
            this.bondRenovations.init();
            this.isTheLast.init();
        },
        guarantors: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/garantes")
                }).done(function (result) {
                    $(".select2-guarantors").select2({
                        data: result
                    });
                });
            }
        },
        budgetType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budgetType").select2({
                        data: result
                    });
                });
            }
        },
        bondType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-fianza")
                }).done(function (result) {
                    $(".select2-bondType").select2({
                        data: result
                    });
                });
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
                    url: _app.parseUrl("/finanzas/responsables/proyecto")
                }).done(function (result) {
                    responsibles = result.responsibles.toString();
                    $("#BondRenovation_Responsibles").val(result.responsibles.toString());
                    $("#Responsibles").val(result.responsibles.toString());
                });
            },
            reload: function () {
                $("#BondRenovation_Responsibles").val(responsibles);
                $("#Responsibles").val(responsibles);
            }
        },
        currencies: {
            init: function () {
                $(".select2-currencies").select2({
                    data: [{ "id": "PEN", "text": "SOLES" }, { "id": "USD", "text": "DOLARES" }]
                });
            }
        },
        bondRenovations: {
            init: function () {
                $(".select2-bond-renovations").select2();
            },
            reload: function (bondid, renid) {
                console.log(bondid);
                console.log(renid);
                $(".select2-bond-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/cartas-fianza/renovaciones/${bondid}`)
                }).done(function (result) {
                    $(".select2-bond-renovations").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_bondrenovations").val(renid).trigger("change");
                    $("#btn_LoadPdf").trigger("click");
                });
            }
        },
        issues: {
            init: function () {
                $(".select2-bond-renovations").select2();
            },
            reload: function (bondid, renid) {

                $(".select2-bond-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/cartas-fianza/renovaciones/${bondid}`)
                }).done(function (result) {
                    $(".select2-bond-renovations").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_bondrenovations").val(renid).trigger("change");
                    $("#btn_LoadPdf2").trigger("click");
                });
            }
        },
        isTheLast: {
            init: function () {
                $(".select2-isthelast").select2();
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

            addRenForm = $("#add_ren_form").validate({
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
                    forms.reset.pdf2();
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
                let id = $("#select_bondrenovations").val();
                console.log(id);
                forms.load.pdf(id);
               
            });

            $("#btn_LoadPdf2").on("click", function () {
                let id = $("#select_bondrenovations").val();
                console.log(id);
                forms.load.pdf2(id);

            });

            $("#project_filter,#bank_filter,#supply_family_filter, #supply_group_filter, #last_filter, #bondguarantor_filter").on("change", function () {
                dataTables.bondAddsDt.reload();
            });
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