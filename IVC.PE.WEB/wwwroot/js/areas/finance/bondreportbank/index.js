var Provider = function () {

    var mainDatatable = null;
    var detailDatatables = {};
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var addFileForm = null;
    var providerId = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/reportebancofianza/listar"),
            dataSrc: "",

            data: function (d) {
                d.bankId = $("#supply_family_filter").val();
                d.isEnabled = $("#inlineCheckbox1").is(":checked");
                d.isExpired = $("#inlineCheckbox2").is(":checked");
                d.isClosed = $("#inlineCheckbox3").is(":checked");

                delete d.columns;
            }
        },

        rowGroup: {
            dataSrc: "bank.name",
            title: "Centro de costo",

        },
        
        columns: [
            {
                "visible":false,
                data: "bank.name",
                title: "Entidad Financiera",

            },
            {
                title: "Centro de Costos",
                data: "project.name"
            },
            {
                title: "Concepto",
                data: "bondType.name"
            },
            {
                title: "Numero de Fianza",
                data: "bondNumber"
            },
            {
                title: "Inicio",
                data: "dateInitial"
            },
            {
                title: "Vencimiento",
                data: "dateEnd"
            },
            {
                title: "Vence (días)",
                data: "daysToEnd",

                render: function (data, type, row) {

                    if (data >= "31") {
                        //        $(td).css('color', '#1E90FF').css('font-weight', 'bold')
                        //$(td).css('color', '#FFFFFF').addClass('badge').css('background-color', '#1E90FF')


                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">${data}</span>
								</label>
							</span>`;
                    }
                    if (data <= "30" && data >= "15") {
                        //     $(td).css('color', '#ff8000').css('font-weight', 'bold')
                        // $(td).css('color', '#FFFFFF').addClass('badge').css('background-color', '#ff8000')
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${data}</span>
								</label>
							</span>`;
                    }
                    if (data <= "14") {
                        //   $(td).css('color', '#FFFFFF').addClass('badge').css('background-color', '#FF0000')
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">${data}</span>
								</label>
							</span>`;
                    }
                },
               
            },
            {
                title: "Estado",
                data: "daysToEnd",
                render: function (data, type, row) {

                    if (data >= "0") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">ACTIVO</span>
								</label>
							</span>`;
                    }
                    if (data < "0") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">VENCIDO</span>
								</label>
							</span>`;
                    }

                },
            },
            
            {
                title: "Por vencer",
                data: "daysToEnd",

                render: function (data, type, row) {
                    data = parseInt(data)

                    if (data <= 30 && data >= 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">POR VENCER</span>
								</label>
							</span>`;
                    }
                    else if (data < 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">VENCIDA</span>
								</label>
							</span>`;
                    }
                    else if (data > 30) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">NO</span>
								</label>
							</span>`;
                    }

                },


            /*    "createdCell": function (td, cellData, rowData, row, col) {
                    if (cellData <= "30" && cellData >= "0") {
                        $(td).text("POR VENCER");
                    }
                    if (cellData < "0") {
                        $(td).text("VENCIDA");
                    }
                    if (cellData > "30") {
                        $(td).text("NO");
                    }

                }*/
            },
            {
                title: "Cerrada",

                data: "daysLimitTerm",
                render: function (data, type, row) {

                    if (data == "1") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">CERRADA</span>
								</label>
							</span>`;
                    }
                    if (data == "0") {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">ABIERTA</span>
								</label>
							</span>`;
                    }

                },
            },
            {
                title: "Monto",
                data: "penAmmount2",
            },
            {
                title: "Total",
                data: "penAmmount",

                "createdCell": function (td, row, data, start, end, display) {
                    var api = this.api(), data;

                    // converting to interger to find total
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };

                    //data = api.column(10).cache('search');
                    //var friTotal = data.length ?
                    //    data.reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    }) :
                    //    0;

                    var friTotal = api
                        .column(11)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                   //var friTotal = api
                   //     .column(11, { page: "current"})
                   //     .data()
                   //     .reduce(function (a, b) {
                   //         return intVal(a) + intVal(b);
                   //     }, 0);

                     //Update footer by showing the total with the reference of the column index 
                    //$(api.column(11).header()).html(friTotal);
                    //$(td).text(friTotal);

                    $("#main_datatable_footer").html(friTotal);
                    //console.log(api.column(11, { page: 'current' }).data())
                    //console.log(friTotal)
                    //console.log(api.column(11).header())
                },
              
            },
            {
                "visible": false,
                width: "0%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                  
                    return tmp;
                }
            }
        ],



    };

    var genOptionsDetails = function (id) {
        var result = {
            responsive: true,
            ajax: {
                url: _app.parseUrl(`/finanzas/reportebancofianza/${id}/archivos/listar`),
                dataSrc: ""
            },
            rowCallback: function (row, data) {
            },
            columns: [
                {
                    title: "Archivo",
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var tmp = "";
                        tmp += `<button data-url="${row.fileUrl}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
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
                        tmp += `<button data-provider-id="${id}" data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-file-delete">`;
                        tmp += `<i class="fa fa-trash"></i></button>`;
                        return tmp;
                    }
                }
            ]
        };

        return result;
    };

    var datatable = {
        init: function () {
            
            mainDatatable = $("#main_datatable").DataTable(options);
            $("#main_datatable").append(
                "<tfoot><tr><th>Total</th><th id='main_datatable_footer'></th></tr></tfoot>"
            ); 
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        initEvents: function () {



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
                                    url: _app.parseUrl(`/finanzas/reportebancofianza/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
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
        child: {
            init: function (data) {
                var optionsDetail = genOptionsDetails(data.id);
                var $table = $(`<table id="items_${data.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
                detailDatatables[data.id] = $table.DataTable(optionsDetail);
                this.initEvents($table);
                return $table;
            },
            reload: function (id) {
                var dt = detailDatatables[id];
                if (dt) {
                    dt.ajax.reload();
                }
            },
            initEvents: function (table) {
                table.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let url = $btn.data("url");
                    form.load.file.pdf(url);
                });

                table.on("click", ".btn-file-delete", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    let providerId = $btn.data("providerId");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El archivo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/finanzas/reportebancofianza/${providerId}/archivos/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.child.reload(providerId);
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al archivo"
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
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/reportebancofianza/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                        formElements.find("[name='BondGuarantorId']").val(result.bondGuarantorId).trigger("change");
                        formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId).trigger("change");

                        formElements.find("[name='BondTypeId']").val(result.bondTypeId).trigger("change");
                        formElements.find("[name='BankId']").val(result.bankId).trigger("change");
                        formElements.find("[name='BondNumber']").val(result.bondNumber);
                        formElements.find("[name='BondRenovationId']").val(result.bondRenovationId).trigger("change");
                        formElements.find("[name='EmployeeId']").val(result.employeeId).trigger("change");
                        formElements.find("[name='currenyType']").val(result.currencyType);

                        formElements.find("[name='PenAmmount']").val(result.penAmmount);
                        //formElements.find("[name='UsdAmmount']").val(result.usdAmmount);
                        formElements.find("[name='daysLimitTerm']").val(result.daysLimitTerm);
                        formElements.find("[name='CreateDate']").val(result.createDate);
                        formElements.find("[name='EndDate']").val(result.endDate);

                        formElements.find("[name='guaranteeDesc']").val(result.guaranteeDesc);

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
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
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/finanzas/reportebancofianza/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/reportebancofianza/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                    url: "/finanzas/reportebancofianzaimportar",
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
                    datatable.reload();
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
                        url: _app.parseUrl(`/finanzas/reportebancofianza/${providerId}/archivos/crear`),
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
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            file: {
                add: function () {
                    addFileForm.resetForm();
                    $("#add_file_form").trigger("reset");
                    $("#add_file_form [name='Type']").prop("selectedIndex", 0).trigger("change");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.fileTypes.init();
            this.accountTypes.init();
            this.currencies.init();
            this.propertyServices.init();
            this.banks.init();
            this.supplyFamilies.init();
            this.supplyGroups.init();

            this.employees.init();
            this.budgetType.init();
            this.bondType.init();
            this.projects.init();

          
            this.renovations.init();
            this.guarantors.init();
            this.currencies.init();
            this.creditlines.init();

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
                    url: _app.parseUrl("/select/bancos?existRegister=true")
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
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        employees: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados2")
                }).done(function (result) {
                    $(".select2-employees").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },

        currencies: {
            init: function () {
                $.ajax({
                    //  url: _app.parseUrl("/select/empleados2")
                }).done(function () {
                    $(".select2-currencies").select2({
                        data: [{ "id": "PEN", "text": "SOLES" }, { "id": "USD", "text": "DOLARES" }],
                        allowClear: true
                    });
                });
            }
        },

        creditlines: {
            init: function () {
                $.ajax({
                    //  url: _app.parseUrl("/select/empleados2")
                }).done(function () {
                    $(".select2-creditlines").select2({
                        data: [{ "id": "LINEACREDITO", "text": "LINEA DE CREDITO" }, { "id": "MONTORETENIDO", "text": "MONTO RETENIDO" }],
                        allowClear: true
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
                        data: result,
                        allowClear: true
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
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        renovations: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/renovaciones")
                }).done(function (result) {
                    $(".select2-renovations").select2({
                        data: result,
                        allowClear: true
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
                        data: result,
                        allowClear: true
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

            $("#supply_family_filter, #inlineCheckbox1, #inlineCheckbox2, #inlineCheckbox3").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Provider.init();

});


