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
            this.plameFilesBtn.init();
        },
        plameFilesBtn: {
            init: function () {
                $("#plameFiles").on("click", function () {
                    let yid = $("#year_filter").val();
                    let mid = $("#month_filter").val();
                    window.location = `/recursos-humanos/obreros/reportes/plame/archivos?year=${yid}&month=${mid}`;
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