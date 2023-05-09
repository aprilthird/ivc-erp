var PayrollReportWorkforceCost = function () {

    var select2 = {
        init: function () {
            this.months.init();
        },
        months: {
            init: function () {
                $(".select2-months").select2();
            }
        }
    };

    var excelButtons = {
        init: function () {
            this.afpFormatBtn.init();
            this.phaseBtn.init();
        },
        afpFormatBtn: {
            init: function () {
                $("#afpFormat").on("click", function () {
                    let yid = $("#year_filter").val();
                    let mid = $("#month_filter").val();
                    window.location = `/recursos-humanos/obreros/reportes/afp/formato?year=${yid}&month=${mid}`;
                });
            }
        },
        phaseBtn: {
            init: function () {
                $("#payrollPhaseCostBtn").on("click", function () {
                    _app.loader.show();
                    week = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/reportes/fases-cuadrillas-costos/${week}`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/recursos-humanos/obreros/reportes/descargar-excel?excelName=${result}`;
                    }).fail(function (error) {
                        swal.fire({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn-danger",
                            animation: false,
                            customClass: 'animated tada',
                            confirmButtonText: "Entendido",
                            text: error.responseText
                        });
                    })
                });
            }
        }
    };

    return {
        init: function () {
            select2.init();
            excelButtons.init();
        }
    };
}();

$(function () {
    PayrollReportWorkforceCost.init();
});