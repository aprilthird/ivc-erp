var Formula = function () {

    var mainDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/cargafianza/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },


        columns: [

            {
                title: "Proyecto",
                data: "project.name"
            },
            {
                title: "Garante",
                data: "bondGuarantor.name"
            },
            {
                title: "Título",
                data: "budgetTitle.name"
            },
            {
                title: "Tipo de Fianza",
                data: "bondType.name"
            },
            {
                title: "Entidad Bancaria",
                data: "bank.name"
            },
            {
                title: "Numero de Fianza",
                data: "bondNumber"
            },
            {
                title: "Renovación",
                data: "bondRenovation.name"
            },
            {
                title: "Monto en S/.",
                data: "penAmmount"
            },
            {
                title: "Monto en US$",
                data: "usdAmmount"
            },
            {
                title: "Plazo",
                data: "daysLimitTerm"
            },
            {
                title: "Fecha de Creación",
                data: "createDate"
            },
            {
                title: "Contra Garantía",
                data: "guaranteeDesc"
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

    return {
        init: function () {
            datatable.init();
        }
    }
}();

$(function () {
    Formula.init();
});