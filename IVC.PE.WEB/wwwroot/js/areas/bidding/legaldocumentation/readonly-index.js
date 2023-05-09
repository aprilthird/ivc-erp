var Formula = function () {

    var bondAddsDataTable = null;
    var bondRensDataTable = null;

    var addId = null;

    var bondAddsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/cartas-fianza/listar"),
            data: function (d) {
                d.projectId = $("#project_filter").val();
                d.bankId = $("#bank_filter").val();
                d.bondTypeId = $("#supply_family_filter").val();
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
                }
            },
            {
                title: "Entidad Bancaria",
                data: "bank"
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
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-file-excel'></i> Reportes en Excel",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    $("#excel_opt_modal").modal("show");
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
                    tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
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
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        isFromDetail = false;
                        select2.bondRenovations.reload(id);
                        $("#btn_LoadPdf").trigger("click");
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
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        isFromDetail = true;
                        $("#detail_modal").modal("hide");
                        $("#select_bondrenovations").val(id).trigger("change");
                        $("#btn_LoadPdf").trigger("click");
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
                        formElements.find("[name='select_users']").val(result.responsibles.toString().split(',')).trigger("change");
                        formElements.find("[name='PenAmmount']").val(result.penAmmount);
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
            }
        }
    };

    var select2 = {
        init: function () {
            this.projects.init();
            this.guarantors.init();
            this.budgetType.init();
            this.bondType.init();
            this.banks.init();
            this.users.init();
            this.currencies.init();
            this.bondRenovations.init();
            this.isTheLast.init();
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                });
            }
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
                $.ajax({
                    url: _app.parseUrl("/select/usuarios")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
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
            reload: function (id) {
                $(".select2-bond-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/cartas-fianza/renovaciones/${id}`)
                }).done(function (result) {
                    $(".select2-bond-renovations").select2({
                        data: result
                    });
                    $("#btn_LoadPdf").trigger("click");
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
            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
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

            $("#project_filter,#bank_filter,#supply_family_filter, #supply_group_filter").on("change", function () {
                dataTables.bondAddsDt.reload();
            });
        }
    };

    return {
        init: function () {
            dataTables.init();
            select2.init();
            validate.init();
            datepickers.init();
            events.init();
        }
    }
}();

$(function () {
    Formula.init();
});