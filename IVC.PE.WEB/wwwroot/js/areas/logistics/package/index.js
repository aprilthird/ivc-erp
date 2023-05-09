var Package = function () {

    var packageDatatable = null;
    var orderSupplyDatatable = null;
    var addedSupplyDatatable = null;
    var virtualOutputDatatable = null;
    var usedSupplyDatatable = null;
    var Id = null;

    var options = {
        responsive: false,
        processing: true,
        scrollX: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/paqueteo/listar"),
            data: function (d) {
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Item",
                data: "item"
            },
            {
                title: "Insumos",
                data: "insumos"
            },
            {
                title: "Insumos Venta",
                data: "saleSupply"
            },
            {
                title: "Insumos Ordenes Generadas",
                data: "generatedOrderSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-orderSupply" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Insumos Ingresados",
                data: "addedSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-addedSupply" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Insumos c/ Salida Virtual",
                data: "virtualOutputSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-virtualOutput" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Insumos Consumidos",
                data: "usedSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-usedSupply" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Insumos Pagados",
                data: "paidSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-paidSupply" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Insumos Valorizados",
                data: "valuedSupply"
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isFamily == false && row.isGroup == false) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-valuedSupply" title="Ver Items">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2, 3, 5, 7, 9, 11, 13] },
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.isFamily) {
                $(row).css("background-color", "#FBF3F2");
            } else {
                if (data.isGroup) {
                    $(row).css("background-color", "#F4EDEC");
                }
            }

        }
    };
    
    var orderSupplyOpt = {
        responsive: true,
        paging: false,
        ajax: {
            url: _app.parseUrl("/logistica/paqueteo/detalles/ordenes-generadas"),
            data: function (d) {
                d.supplyGroupId = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proveedor",
                data: "provider.businessName"
            },
            {
                title: "Tipo",
                data: "type"
            },
            {
                title: "Número",
                data: "number"
            },
            {
                title: "Monto",
                data: "parcial"
            }
        ],
        buttons: [],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var addedSupplyOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/paqueteo/detalles/insumos-ingresados"),
            data: function (d) {
                d.supplyGroupId = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proveedor",
                data: "provider.businessName"
            },
            {
                title: "Tipo",
                data: "type"
            },
            {
                title: "Número",
                data: "number"
            },
            {
                title: "Monto",
                data: "parcial"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var virtualOutputOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/paqueteo/detalles/insumos-virtual"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proveedor",
                data: ""
            },
            {
                title: "Tipo",
                data: ""
            },
            {
                title: "Número",
                data: ""
            },
            {
                title: "Monto",
                data: ""
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var usedSupplyOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/paqueteo/detalles/insumos-consumidos"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Código",
                data: "supply.fullcode"
            },
            {
                title: "Monto",
                data: "parcial"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [2] }
        ]
    };

    var datatables = {
        init: function () {
            this.packageDt.init();
            this.orderSupplyDt.init();
            this.addedSupplyDt.init();
            this.virtualOutputDt.init();
            this.usedSupplyDt.init();
        },
        packageDt: {
            init: function () {
                packageDatatable = $("#package_datatable").DataTable(options);
                this.events();
            },
            reload: function () {
                packageDatatable.ajax.reload();
            },
            events: function () {
                packageDatatable.on("click",
                    ".btn-orderSupply",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.orderSupplyDt.reload();
                        $("#orderSupply_detail_modal").modal("show");
                    });
                packageDatatable.on("click",
                    ".btn-addedSupply",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.addedSupplyDt.reload();
                        $("#addedSupply_detail_modal").modal("show");
                    });
                packageDatatable.on("click",
                    ".btn-virtualOutput",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.virtualOutputDt.reload();
                        $("#virtualOutput_detail_modal").modal("show");
                    });
                packageDatatable.on("click",
                    ".btn-usedSupply",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        datatables.usedSupplyDt.reload();
                        $("#usedSupply_detail_modal").modal("show");
                    });

                //form.load.items.detail(id);
            }
        },
        orderSupplyDt: {
            init: function () {
                orderSupplyDatatable = $("#orderSupply_datatable").DataTable(orderSupplyOpt);
                this.events();
            },
            reload: function () {
                orderSupplyDatatable.ajax.reload();
            },
            events: function () {

            }
        },
        addedSupplyDt: {
            init: function () {
                addedSupplyDatatable = $("#addedSupply_datatable").DataTable(addedSupplyOpt);
                this.events();
            },
            reload: function () {
                addedSupplyDatatable.ajax.reload();
            },
            events: function () {

            }
        },
        virtualOutputDt: {
            init: function () {
                virtualOutputDatatable = $("#virtualOutput_datatable").DataTable(virtualOutputOpt);
                this.events();
            },
            reload: function () {
                virtualOutputDatatable.ajax.reload();
            },
            events: function () {

            }
        },
        usedSupplyDt: {
            init: function () {
                usedSupplyDatatable = $("#usedSupply_datatable").DataTable(usedSupplyOpt);
                this.events();
            },
            reload: function () {
                usedSupplyDatatable.ajax.reload();
            },
            events: function () {

            }
        }
        
    };
    
    var select2 = {
        init: function () {
            this.supplyFamilies.init();
            this.supplyGroups.filter();
        },
        supplyFamilies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                    $(".select2-supply-familiy-filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            filter: function () {
                $(".select2-supply-group-filter").empty();
                $(".select2-supply-group-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#supply_family_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-supply-group-filter").select2({
                        data: result
                    });
                });
            }
        }
    };

    var validate = {

    };

    var modals = {

    };

    var events = {
        init: function () {
            $("#supply_family_filter").on("change", function () {
                select2.supplyGroups.filter();
            });

            $("#btn-refresh").on("click", function () {
                datatables.packageDt.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatables.init();
        }
    };
}();

$(function () {
    Package.init();
});