var ValuedSupplyEntry = function () {

    var mainDatatable = null;
    var createDatatable = null;
    var detailDatatable = null;
    var itemsDatatable = null;
    var partialDatatable = null;
    var createForm = null;
    var editForm = null;
    var detailForm = null;
    var Id = null;
    var supplyEntryId = null;
    var inCreate = false;

    var list = [];

    var totalCost = 0;
    var totalDolarCost = 0;

    var formatter = new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: 'PEN'
    });

    var formatter2 = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    });

    var options = {
        processing: true,
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/guias-valorizadas/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                d.providerId = $("#provider_filter").val();
                d.year = $("#year_filter").val();
                d.month = $("#month_filter").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                totalCost = 0;
                totalDolarCost = 0;
                console.log(result);
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        totalCost += item.parcial
                        totalDolarCost += item.dolarParcial;
                    });
                }

                return [$("#total_parcial").val(formatter.format(totalCost)),
                    $("#total_dolar_parcial").val(formatter2.format(totalDolarCost))];
            }
        },
        columns: [
            {
                title: "Grupo",
                data: "groups"
            },
            {
                title: "Proveedor",
                data: "provider"
            },
            {
                title: "Año Valorizado",
                data: "year"
            },
            {
                title: "Mes Valorizado",
                data: "month"
            }, 
            {
                title: "Monto (S/)",
                data: "parcialString"
            },
            {
                title: "Monto (USD)",
                data: "dolarParcialString"
            },
            {
                title: "Monto (S/) Aux",
                data: "parcial",
                visible: false
            },
            {
                title: "Monto (USD) Aux",
                data: "dolarParcial",
                visible: false
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.providerId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    */
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [4, 5] }
        ]
    };

    var createOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/guias-valorizadas/crear/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_add").val();
                d.supplyGroupId = $("#supply_group_add").val();
                d.providerId = $("#provider_add").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                list = [];
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        list.push(item.id)
                    });
                }
                _app.loader.hide();
            }
        },
        columns: [
            {
                title: "Selección",
                data: "isValued",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}_bool" data-id="${row.id}" class="form-control selections" type="checkbox" value="${data}">`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Guía de Remisión",
                data: "remissionGuideName"
            },
            {
                title: "Proveedor",
                data: "order.provider.tradename"
            },
            {
                title: "Fecha",
                data: "deliveryDate"
            },
            {
                title: "Monto (S/)",
                data: "parcialString"
            },
            {
                title: "Monto (USD)",
                data: "dolarParcialString"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.remissionGuideUrl}" data-name="${row.remissionGuideName}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5] }
        ]
    };

    var detailOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/guias-valorizadas/detalles/listar"),
            data: function (d) {
                d.providerId = Id;
                d.year = $("#year_filter").val();
                d.month = $("#month_filter").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                list = [];
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        list.push(item.id)
                    });
                }
            }
        },
        columns: [
            {
                title: "Selección",
                data: "isValued",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}_bool" data-id="${row.id}" class="form-control selections" type="checkbox" value="${data}" checked>`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Guía de Remisión",
                data: "remissionGuideName"
            },
            {
                title: "Año Valorizado",
                data: "valuedYear"
            },
            {
                title: "Mes Valorizado",
                data: "valuedMonth"
            },
            {
                title: "Fecha",
                data: "deliveryDate"
            },
            {
                title: "Monto (S/)",
                data: "parcialString"
            },
            {
                title: "Monto (USD)",
                data: "dolarParcialString"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.remissionGuideUrl}" data-name="${row.remissionGuideName}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-report" title="Vale de Guia">`;
                    tmp += `<i class="la la-file-text"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [5, 6] }
        ]
    };

    var itemsOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/guias-valorizadas/items-ingreso/listar"),
            data: function (d) {
                d.id = supplyEntryId;

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Código IVC",
                data: "orderItem.supply.fullCode"
            },
            {
                title: "Insumo",
                data: "orderItem.supply.description"
            },
            {
                title: "Und",
                data: "orderItem.supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Despachado",
                data: "measure"
            },
            {
                title: "Estado",
                data: function (result) {
                    let valued = result.isValued;

                    if (valued == true)
                        return `<b style="color:rgb(44, 182, 0);" >Valorizado</b>`;
                    else
                        return `<b style="color:rgb(182, 0, 0);" >Sin Valorizar</b>`;
                }
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var partialOpts = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/almacenes/guias-valorizadas/crear-items/listar"),
            data: function (d) {
                d.id = $("#supply_entries").val();

                delete d.columns;
            },
            dataSrc: "",
            complete: function (result) {
                list = [];
                if (result.statusText != "abort") {
                    result.responseJSON.forEach(function (item) {
                        list.push(item.id)
                    });
                }
            }
        },
        columns: [
            {
                title: "Selección",
                data: "isValued",
                render: function (data, type, row) {
                    var tmp = `<input id="${row.id}_bool" data-id="${row.id}" class="form-control selections" type="checkbox" value="${data}">`;
                    tmp += `</input>`;
                    return tmp;
                }
            },
            {
                title: "Código IVC",
                data: "orderItem.supply.fullCode"
            },
            {
                title: "Insumo",
                data: "orderItem.supply.description"
            },
            {
                title: "Und",
                data: "orderItem.supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado Despachado",
                data: "measure"
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var datatables = {
        init: function () {
            this.mainDt.init();
            this.createDt.init();
            this.detailDt.init();
            this.itemDt.init();
            this.partialDt.init();
        },
        mainDt: {
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
                        inCreate = false;
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.detailDt.reload();
                        $("#detail_modal").modal("show");
                    });
            }
        },
        createDt: {
            init: function () {
                createDatatable = $("#add_datatable").DataTable(createOpts);
                this.initEvents();
            },
            reload: function () {
                createDatatable.ajax.reload();
            },
            initEvents: function () {
                createDatatable.on("change",
                    ".selections",
                    function () {
                        if ($(this)[0].checked == true) {
                            $(this).val(true);
                        } else {
                            $(this).val(false);
                        }
                    });

                createDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        supplyEntryId = id;
                        datatables.itemDt.reload();
                        $("#add_modal").modal("hide");
                        $("#item_modal").modal("show");
                    });

                createDatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let url = $btn.data("id");
                        let name = $btn.data("name");
                        $("#add_modal").modal("hide");
                        form.load.pdf(url, name);
                    });
            }
        },
        detailDt: {
            init: function () {
                detailDatatable = $("#detail_datatable").DataTable(detailOpts);
                this.initEvents();
            },
            reload: function () {
                detailDatatable.ajax.reload();
            },
            initEvents: function () {
                detailDatatable.on("change",
                    ".selections",
                    function () {
                        if ($(this)[0].checked == true) {
                            $(this).val(true);
                        } else {
                            $(this).val(false);
                        }
                    });

                detailDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        supplyEntryId = id;
                        datatables.itemDt.reload();
                        $("#detail_modal").modal("hide");
                        $("#item_modal").modal("show");
                    });

                detailDatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let url = $btn.data("id");
                        let name = $btn.data("name");
                        $("#detail_modal").modal("hide");
                        form.load.pdf(url, name);
                    });

                detailDatatable.on("click",
                    ".btn-report",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");

                        window.location = `/almacenes/guias-valorizadas/vale/${id}`;
                    });
            }
        },
        itemDt: {
            init: function () {
                itemsDatatable = $("#item_datatable").DataTable(itemsOpts);
            },
            reload: function () {
                itemsDatatable.ajax.reload();
            }
        },
        partialDt: {
            init: function () {
                partialDatatable = $("#partial_datatable").DataTable(partialOpts);
                this.initEvents();
            },
            reload: function () {
                partialDatatable.ajax.reload();
            },
            initEvents: function () {
                partialDatatable.on("change",
                    ".selections",
                    function () {
                        if ($(this)[0].checked == true) {
                            $(this).val(true);
                        } else {
                            $(this).val(false);
                        }
                    });
            }
        }
    };

    var form = {
        load: {
            pdf: function (url, name) {
                _app.loader.show();
                $("#pdf_name").text(name);
                $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");

                $("#pdf_modal").modal("show");
                _app.loader.hide();
            }
        },
        submit: {
            add: function (formElement) {
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let obs = $(`#${item}_bool`).val();
                    data.append('Items', item + "|" + obs);
                });
                data.append('Month', $("#month_add").val());
                data.append('Year', $("#year_add").val());
                let $btn = $("#save");
                $btn.addLoader();

                $.ajax({
                    url: _app.parseUrl("/almacenes/guias-valorizadas/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                    })
                    .done(function (result) {
                        datatables.mainDt.reload();
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
            partial: function (formElement) {
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let obs = $(`#${item}_bool`).val();
                    data.append('Items', item + "|" + obs);
                });
                data.append('Month', $("#month_partial").val());
                data.append('Year', $("#year_partial").val());
                let $btn = $("#partial");
                $btn.addLoader();

                $.ajax({
                    url: _app.parseUrl("/almacenes/guias-valorizadas/crear-items"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                    })
                    .done(function (result) {
                        datatables.mainDt.reload();
                        partialDatatable.clear().draw();
                        $("#partial_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#partial_alert_text").html(error.responseText);
                            $("#partial_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            detail: function (formElement) {
                let data = new FormData($(formElement).get(0));
                list.forEach(function (item) {
                    let obs = $(`#${item}_bool`).val();
                    data.append('Items', item + "|" + obs);
                });
                let $btn = $("#edit");
                $btn.addLoader();

                $.ajax({
                    url: _app.parseUrl("/almacenes/guias-valorizadas/editar"),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                    })
                    .done(function (result) {
                        datatables.mainDt.reload();
                        datatables.detailDt.reload();
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
            }
        },
        reset: {
            add: function () {
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#supply_group_add").prop("selectedIndex", 0).trigger("change");
                datatables.createDt.reload();
            },
            edit: function () {
                $("#edit_alert").removeClass("show").addClass("d-none");
                datatables.detailDt.reload();
            },
            detail: function () {
                $("#detail_alert").removeClass("show").addClass("d-none");
            },
            items: function () {
                if (inCreate == true) {
                    $("#add_modal").modal("show");
                } else {
                   $("#detail_modal").modal("show");
                }
                
            },
            pdf: function () {
                if (inCreate == true) {
                    $("#add_modal").modal("show");
                } else {
                   $("#detail_modal").modal("show");
                }
            },
            partial: function () {
                partialDatatable.clear().draw();
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

    var select2 = {
        init: function () {
            this.providers.init();
            this.providers.add();
            this.supplyGroup.init();
            this.supplyGroup.add();
            this.supplyFamily.init();
            this.year.init();
            this.month.init();
            this.supplyEntries.init();
        },
        supplyFamily: {
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
        supplyGroup: {
            init: function () {
                $("#supply_group_filter").empty();
                $("#supply_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $("#supply_group_filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#supply_group_add").empty();
                $("#supply_group_add").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_add").val()
                    }
                }).done(function (result) {
                    $("#supply_group_add").select2({
                        data: result
                    });
                });
            }
        },
        providers: {
            init: function () {
                $("#provider_filter").empty();
                $("#provider_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores-grupos"),
                    data: {
                        supplyGroupId: $("#supply_group_filter").val()
                    }
                }).done(function (result) {
                    $("#provider_filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#provider_add").empty();
                $("#provider_add").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores-grupos"),
                    data: {
                        supplyGroupId: $("#supply_group_add").val()
                    }
                }).done(function (result) {
                    $("#provider_add").select2({
                        data: result
                    });
                });
            }
        },
        year: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/anios-proyecto")
                }).done(function (result) {
                    $(".select2-years").select2({
                        data: result
                    });
                });
            }
        },
        month: {
            init: function () {
                $(".select2-months").select2({
                });
            }
        },
        supplyEntries: {
            init: function () {
                $(".select2-supply-entries").empty();
                $.ajax({
                    url: _app.parseUrl("/select/ingreso-materiales")
                }).done(function (result) {
                    $(".select2-supply-entries").select2({
                        data: result
                    });
                });
            }
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

            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });

            $("#item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    form.reset.pdf();
                });

            $("#partial_modal").on("hidden.bs.modal",
                function () {
                    form.reset.partial();
                });
        }
    };

    var events = {
        init: function () {

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroup.init();
                datatables.mainDt.reload();
            });

            $("#supply_group_filter").on("change", function () {
                select2.providers.init();
                datatables.mainDt.reload();
            });

            $("#provider_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#supply_family_add").on("change", function () {
                select2.supplyGroup.add();
            });

            $("#supply_group_add").on("change", function () {
                select2.providers.add();
            });

            $("#year_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#month_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#search").on("click", function () {
                _app.loader.show();
                datatables.createDt.reload();
            });

            $("#save").on("click", function () {
                form.submit.add();
            });

            $("#partial").on("click", function () {
                form.submit.partial();
            });

            $("#edit").on("click", function () {
                form.submit.detail();
            });

            $('#main_datatable').on('search.dt', function () {
                console.log($('.dataTables_filter input').val());
                /*
                let res = mainDatatable.api().column(7)
                    .data()
                    .reduce(function (a, b) {
                        return a + b;
                    }, 0);
                */
                //return $("#total_parcial").val(formatter.format(res));
                //datatable.aux();
            });

            $("#add_button").on("click", function () {
                inCreate = true;
            });

            $("#guia_parcial").on("click", function () {
                select2.supplyEntries.init();
                $("#option_add_modal").modal("hide");
                $("#partial_modal").modal("show");
            });

            $("#guia_nueva").on("click", function () {
                $("#option_add_modal").modal("hide");
                $("#add_modal").modal("show");
            });

            $("#listItems").on("click", function () {
                datatables.partialDt.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            datatables.init();
            validate.init();
            modals.init();
            select2.init();
        }
    };
}();

$(function () {
    ValuedSupplyEntry.init();
});