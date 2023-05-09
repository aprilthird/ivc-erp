var Supply = function () {

    var supplyDatatable = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/logistica/insumos/listar"),
            data: function (d) {
                d.measurementUnitId = $("#measurement_unit_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.
                    Id = $("#supply_group_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código de Artículo",
                data: "fullCode"
            },
            {
                title: "Descripción de Artículo",
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
                title: "Correlativo",
                data: "correlativeCodeString"
            }
        ]
    };

    var datatable = {
        init: function () {
            supplyDatatable = $("#supply_datatable").DataTable(options);
        },
        reload: function () {
            supplyDatatable.ajax.reload();
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
    Supply.init();
});