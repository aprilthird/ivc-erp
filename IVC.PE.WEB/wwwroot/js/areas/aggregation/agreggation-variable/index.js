var Variables = function () {

    var providerTypesDt = null;
    var providersDt = null;
    var stockTypesDt = null;
    var stocksDt = null;
    var entriesDt = null;
    var pricesDt = null;

    var importModal = null;

    var providerTypesOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/tipos-proveedor/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo",
                data: "description"
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
    var providersOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/proveedor/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Placa",
                data: "licensePlate"
            },
            {
                title: "Volúmen(m3)",
                data: "volume"
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
    var stockTypesOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/tipos-material/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo",
                data: "description"
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
    var stocksOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/material/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo de Metarial",
                data: "aggregationStockType.description"
            },
            {
                title: "Material",
                data: "description"
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
    var entriesOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/partida/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Partida",
                data: "name"
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
    var pricesOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/agregados/variables/preciario/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo de Material",
                data: "aggregationStockType.description"
            },
            {
                title: "Producto",
                data: "aggregationStock.description"
            },
            {
                title: "Partida",
                data: "aggregationEntry.name"
            },
            {
                title: "Preciario (S/)",
                data: "price"
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

    var datatables = {
        init: function () {
            this.providerTypes.init();
            this.providers.init();
            this.stockTypes.init();
            this.stocks.init();
            this.entries.init();
            this.prices.init();
        },
        providerTypes: {
            init: function () {
                providerTypesDt = $("#providerTypes_datatable").DataTable(providerTypesOpts);
            },
            reload: function () {
                providerTypesDt.ajax.reload();
            }
        },
        providers: {
            init: function () {
                providersDt = $("#providers_datatable").DataTable(providersOpts);
            },
            reload: function () {
                providersDt.ajax.reload();
            }
        },
        stockTypes: {
            init: function () {
                stockTypesDt = $("#stockTypes_datatable").DataTable(stockTypesOpts);
            },
            reload: function () {
                stockTypesDt.ajax.reload();
            }
        },
        stocks: {
            init: function () {
                stocksDt = $("#stocks_datatable").DataTable(stocksOpts);
            },
            reload: function () {
                stocksDt.ajax.reload();
            }
        },
        entries: {
            init: function () {
                entriesDt = $("#entries_datatable").DataTable(entriesOpts);
            },
            reload: function () {
                entriesDt.ajax.reload();
            }
        },
        prices: {
            init: function () {
                pricesDt = $("#prices_datatable").DataTable(pricesOpts);
            },
            reload: function () {
                pricesDt.ajax.reload();
            }
        }
    };

    var forms = {
        submit: {
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
                    url: "/agregados/variables/importar",
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
                    datatables.providerTypes.reload();
                    datatables.providers.reload();
                    datatables.stockTypes.reload();
                    datatables.stocks.reload();
                    datatables.entries.reload();
                    datatables.prices.reload();
                    $("#variables_import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#variables_import_alert_text").html(error.responseText);
                        $("#variables_import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            }
        },
        reset: {
            import: function () {
                importModal.resetForm();
                $("#variables_import_form").trigger("reset");
                $("#variables_import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            importModal = $("#variables_import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#variables_import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });
        }
    };

    var events = {
        init: function () {
            $("#importExcelFormat").on("click", function () {
                window.location = _app.parseUrl(`/agregados/variables/formato-carga`);
            });
        }
    };

    return {
        init: function() {
            datatables.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    Variables.init();
});