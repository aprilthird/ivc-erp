var PayrollMovementWeek = function () {

    var variableDt = null;

    var variableDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/variables/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Tipo",
                data: "typeName"
            },
            {
                title: "Fórmula",
                data: "formula"
            }
        ]
    };

    var datatable = {
        init: function () {
            variableDt = $("#variables_datatable").DataTable(variableDtOpts);
        },
        reload: function () {
            variableDt.ajax.reload();
        }
    };

    var select2 = {
        init: function () {
            this.categories.init();
            this.types.init();
        },
        categories: {
            init: function () {
                $(".select2-payroll-variable-categories").select2();
            }
        },
        types: {
            init: function () {
                $(".select2-payroll-variable-types").select2();
            }
        }
    };

    return {
        init: function () {
            select2.init();
            datatable.init();
        }
    };
}();

$(function () {
    PayrollMovementWeek.init();
});