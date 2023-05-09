var BudgetInput = function () {

    var mainDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/insumos/listar"),
            data: function (d) {
                d.measurementUnitId = $("#measurement_unit_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código S10",
                data: "code"
            },
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Unidad",
                data: "measurementUnit.abbreviation"
            },
            {
                title: "Familia",
                data: "supplyFamily.fullName"
            },
            {
                title: "Grupo",
                data: "supplyGroup.fullName"
            },
            {
                title: "P.U (Venta)",
                data: "saleUnitPrice",
                render: function (data) {
                    return data ? _app.render.money(data) : "---";
                }
            },
            {
                title: "P.U (Meta)",
                data: "goalUnitPrice",
                render: function (data) {
                    return data ? _app.render.money(data) : "---";
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
        },
        reload: function () {
            mainDatatable.ajax.reload();
        }
    };

    var select2 = {
        init: function () {
            this.measurementUnits.init();
            this.supplyFamilies.init();
            this.supplyGroups.init();
        },
        measurementUnits: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    $(".select2-measurement-units").select2({
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

    var events = {
        init: function () {
            $("#measurement_unit_filter, #supply_family_filter, #supply_group_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
        }
    };
}();

$(function () {
    BudgetInput.init();
});