var PayrollReportWorkforceCost = function () {

    var select2 = {
        init: function () {
            this.projects.init();
            this.weeks.init();
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                })
            }
        },
        weeks: {
            init: function () {
                projectId = $("#project_filter").val();
                year = $("#year_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-weeks").empty();
                this.init();
            }
        },
    };

    var events = {
        init: function () {
            $("#project_filter").on("change", function () {
                select2.weeks.reload();
            });

            $("#year_filter").change(function () {
                select2.weeks.reload();
            });
        }
    };

    var excelButtons = {
        init: function () {
            this.payrollBtn.init();
            this.phaseBtn.init();
            this.hoursBtn.init();
            this.hoursCostsBtn.init();
            this.phasesWorkersBtn.init();
        },
        payrollBtn: {
            init: function () {
                $("#payrollWorkforceCostBtn").on("click", function () {
                    _app.loader.show();
                    week = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/reportes/planilla-costos/${week}`)
                    }).always(function () {
                        _app.loader.hide();
                    }).done(function (result) {
                        window.location = `/recursos-humanos/obreros/reportes/descargar-excel?excelName=costosPlanilla`;
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
        },
        hoursBtn: {
            init: function () {
                $("#payrollWorkforceHoursBtn").on("click", function () {
                    _app.loader.show();
                    week = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/reportes/tareo-horas/${week}`)
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
        },
        hoursCostsBtn: {
            init: function () {
                $("#payrollWorkforceHoursCostsBtn").on("click", function () {
                    _app.loader.show();
                    week = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/reportes/fases-horas-costos/${week}`)
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
        },
        phasesWorkersBtn: {
            init: function () {
                $("#payrollWorkforcePhasesWorkersBtn").on("click", function () {
                    _app.loader.show();
                    week = $("#week_filter").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/obreros/reportes/fases-obreros/${week}`)
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
            events.init();
            excelButtons.init();
        }
    };
}();

$(function () {
    PayrollReportWorkforceCost.init();
});