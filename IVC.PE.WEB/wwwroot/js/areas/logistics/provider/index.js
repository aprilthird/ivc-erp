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
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [1, 2, 3, 4, 5],
                hide: [0, 6, 7, 8]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: ":hidden"
            }
        ],
        ajax: {
            url: _app.parseUrl("/logistica/proveedores/listar"),
            dataSrc: "",
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Detalle",
                width: "5%",
                className: "details-control",
                orderable: false,
                data: null,
                defaultContent: "<i class='flaticon2-next'></i>",
                visible: false
            },
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Razón Social",
                data: "businessName"
            },
            {
                title: "RUC",
                data: "ruc"
            },
            {
                title: "Familias",
                data: "supplyFamilyNames"
            },
            {
                title: "Grupos",
                data: "supplyGroupNames"
            },
            {
                title: "Dirección",
                data: "address",
                visible: false
            },
            {
                title: "Teléfono",
                data: "phoneNumber",
                visible: false
            },
            {
                title: "Persona de Contacto",
                data: "collectionAreaContactName",
                visible: false
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
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-add-file">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var genOptionsDetails = function (id) {
        var result = {
            responsive: true,
            ajax: {
                url: _app.parseUrl(`/logistica/proveedores/${id}/archivos/listar`),
                dataSrc: ""
            },
            rowCallback: function (row, data) {
            },
            columns: [
                {
                    title: "Tipo",
                    data: "type",
                    render: function (data) {
                        if (data == 1)
                            return "Tributario";
                        if (data == 2)
                            return "Legal";
                        if (data == 3)
                            return "Comercial";
                        return "---";
                    }
                },
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
                        text: "El proveedor será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/logistica/proveedores/eliminar/${id}`),
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
                                    url: _app.parseUrl(`/logistica/proveedores/${providerId}/archivos/eliminar/${id}`),
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
                    url: _app.parseUrl(`/logistica/proveedores/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='BusinessName']").val(result.businessName);
                        formElements.find("[name='Tradename']").val(result.tradename);
                        formElements.find("[name='RUC']").val(result.ruc);
                        formElements.find("[name='LegalAgent']").val(result.legalAgent);
                        formElements.find("[name='Address']").val(result.address);
                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        formElements.find("[name='CollectionAreaContactName']").val(result.collectionAreaContactName);
                        formElements.find("[name='CollectionAreaEmail']").val(result.collectionAreaEmail);
                        formElements.find("[name='CollectionAreaPhoneNumber']").val(result.collectionAreaPhoneNumber);
                        formElements.find("[name='BankAccountNumber']").val(result.bankAccountNumber);
                        formElements.find("[name='BankAccountCCI']").val(result.bankAccountCCI);
                        formElements.find("[name='ForeignBankAccountNumber']").val(result.foreignBankAccountNumber);
                        formElements.find("[name='ForeignBankAccountCCI']").val(result.foreignBankAccountCCI);
                        formElements.find("[name='TaxBankAccountNumber']").val(result.taxBankAccountNumber);
                        formElements.find("[name='PropertyServiceCode']").val(result.propertyServiceCode);

                        formElements.find("[name='BankId']").val(result.bankId).trigger("change");
                        formElements.find("[name='BankAccountType']").val(result.bankAccountType).trigger("change");
                        formElements.find("[name='ForeignBankId']").val(result.foreignBankId).trigger("change");
                        formElements.find("[name='ForeignBankAccountType']").val(result.foreignBankAccountType).trigger("change");
                        formElements.find("[name='ForeignBankAccountCurrency']").val(result.foreignBankAccountCurrency).trigger("change");
                        formElements.find("[name='PropertyServiceType']").val(result.propertyServiceType).trigger("change");
                        //formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                        //formElements.find("[name='SupplyGroupId']").val(result.supplyGroupId).trigger("change");

                        formElements.find("[name='SupplyFamilyIds']").val(result.supplyFamilyIds);
                        formElements.find("[name='select_families']").val(result.supplyFamilyIds);
                        $(".select2-supply-families").trigger('change');
                        formElements.find("[name='SupplyGroupIds']").val(result.supplyGroupIds);
                        formElements.find("[name='select_groups']").val(result.supplyGroupIds);
                        $(".select2-supply-groups").trigger('change');

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            file: {
                pdf: function (url) {
                    $("#preview_name").text("Proveedor");
                    $("#preview_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                    $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${url}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(url)));
                    $(".btn-mailto").data("name", "Proveedor").data("url", "https://docs.google.com/gview?url=" + encodeURI(url));
                    $("#preview_modal").    modal("show");
                }
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SupplyFamilyIds']").append($(formElement).find("[name='select_families']").val());
                $(formElement).find("[name='SupplyGroupIds']").append($(formElement).find("[name='select_groups']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/logistica/proveedores/crear"),
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
                $(formElement).find("[name='SupplyFamilyIds']").append($(formElement).find("[name='select_families']").val());
                $(formElement).find("[name='SupplyGroupIds']").append($(formElement).find("[name='select_groups']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/proveedores/editar/${id}`),
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
                    url: "/logistica/proveedores/importar",
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
                        url: _app.parseUrl(`/logistica/proveedores/${providerId}/archivos/crear`),
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
                $("#Add_SupplyFamilyIds").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyGroupIds").prop("selectedIndex", 0).trigger("change");
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
                $("#Edit_SupplyFamilyIds").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyGroupIds").prop("selectedIndex", 0).trigger("change");
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
            this.fileTypes.init();
            this.accountTypes.init();
            this.currencies.init();
            this.propertyServices.init();
            this.banks.init();
            this.supplyFamilies.init();
            this.supplyGroups.init();
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
            $("#add_form [name='SupplyFamilyIds']").attr("id", "Add_SupplyFamilyIds");
            $("#edit_form [name='SupplyFamilyIds']").attr("id", "Edit_SupplyFamilyIds");
            $("#add_form [name='SupplyGroupIds']").attr("id", "Add_SupplyGroupIds");
            $("#edit_form [name='SupplyGroupIds']").attr("id", "Edit_SupplyGroupIds");
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

            $("#supply_family_filter, #supply_group_filter").on("change", function () {
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
        },
        resetCode: function () {
            $.ajax({
                url: _app.parseUrl(`/logistica/proveedores/codigo`),
                type: "PUT"
            }).done(function (result) {
                console.log("ya ta papi");
            });
        }
    };
}();

$(function () {
    Provider.init();
});