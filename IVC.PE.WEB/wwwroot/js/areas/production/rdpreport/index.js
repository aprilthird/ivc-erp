var RdpReport = function () {

    var selectSomeOption = new Option('--Seleccione una Cuadrilla--', null, true, true);

    var select2 = {
        init: function () {
            this.workfrontheads.init();
            this.sewergroups.init();
            this.phases.init();
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
                });
            }
        },
        phases: {
            init: function () {
                $(".select2-projectphases").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/fases-proyecto-cuadrilla?sgId=${id}`)
                }).done(function (result) {
                    $(".select2-projectphases").empty();
                    $(".select2-projectphases").select2({
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
                let sgId = $("#sewergroup_filter").val();
                select2.phases.reload(sgId);
            });

            $("#search_button").on("click", function () {
                let sgId = $("#sewergroup_filter").val();
                let ppId = $("#projectphase_filter").val();
                let rptDate = $("#day_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/produccion/rdp/listar?sgId=${sgId}&ppId=${ppId}&rptDate=${rptDate}`),
                    dataScr: ""
                }).done(function (result) {
                    $("#rdp_items").empty();
                    $("#rdp_items").html(result);
                });
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