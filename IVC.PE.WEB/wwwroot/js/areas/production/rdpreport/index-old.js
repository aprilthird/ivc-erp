var RdpReport = function () {

    var selectSomeOption = new Option('--Seleccione una Cuadrilla--', null, true, true);

    var select2 = {
        init: function () {
            this.workfrontheads.init();
            this.sewergroups.init();
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-workfrontheads").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
            },
            reload: function () {
                let wfh = $("#workfronthead_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSomeOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                })
            }
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#workfronthead_filter").on("change", function () {
                select2.sewergroups.reload();
            });

            $("#sewergroup_filter").on("change", function () {
                let sgData = $("#sewergroup_filter").select2('data');
                let sg = sgData[0].text;
                if (sg.charAt(0) == "F") {
                    let hyphenPos = sg.indexOf("-");
                    let formulas = sg.substring(1, hyphenPos).split("/");
                    console.log(formulas);
                    let f1, f3, f4, f5, f6, f7 = true;
                    $.each(formulas, function (i, o) {
                        switch (o) {
                            case "1":
                                f1 = false; break;
                            case "3":
                                f3 = false; break;
                            case "4":
                                f4 = false; break;
                            case "5":
                                f5 = false; break;
                            case "6":
                                f6 = false; break;
                            case "7":
                                f7 = false; break;
                        };
                    });
                    $("#formula1").attr('hidden', f1);
                    $("#formula3").attr('hidden', f3);
                    $("#formula4").attr('hidden', f4);
                    $("#formula5").attr('hidden', f5);
                    $("#formula6").attr('hidden', f6);
                    $("#formula7").attr('hidden', f7);
                } else {
                    $("#formula1").attr('hidden', true);
                    $("#formula3").attr('hidden', true);
                    $("#formula4").attr('hidden', true);
                    $("#formula5").attr('hidden', true);
                    $("#formula6").attr('hidden', true);
                    $("#formula7").attr('hidden', true);
                }
            });
        }
    };

    return {
        init: function () {
            select2.init();
            events.init();
            datepickers.init();
        }
    };
}();

$(function () {
    RdpReport.init();
});