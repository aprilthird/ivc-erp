var CertifiedSewerLines = function () {

	var chart1 = null;
	var chart2 = null;
	var chart3 = null;
	var chart4 = null;

    var chart = {
        config: {
            init: function () {
                moment.locale("es-ES");
                Chart.pluginService.register({
                    beforeDraw: function (chart) {
                        if (chart.config.options.elements.center) {
                            //Get ctx from string
                            var ctx = chart.chart.ctx;

                            //Get options from the center object in options
                            var centerConfig = chart.config.options.elements.center;
                            var fontStyle = centerConfig.fontStyle || 'Arial';
                            var txt = centerConfig.text;
                            var color = centerConfig.color || '#000';
                            var sidePadding = centerConfig.sidePadding || 20;
                            var sidePaddingCalculated = (sidePadding / 100) * (chart.innerRadius * 2)
                            //Start with a base font of 30px
                            ctx.font = "30px " + fontStyle;

                            //Get the width of the string and also the width of the element minus 10 to give it 5px side padding
                            var stringWidth = ctx.measureText(txt).width;
                            var elementWidth = (chart.innerRadius * 2) - sidePaddingCalculated;

                            // Find out how much the font can grow in width.
                            var widthRatio = elementWidth / stringWidth;
                            var newFontSize = Math.floor(30 * widthRatio);
                            var elementHeight = (chart.innerRadius * 2);

                            // Pick a new font size so it will not be larger than the height of label.
                            var fontSizeToUse = Math.min(newFontSize, elementHeight);

                            //Set font settings to draw it correctly.
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'middle';
                            var centerX = ((chart.chartArea.left + chart.chartArea.right) / 2);
                            var centerY = ((chart.chartArea.top + chart.chartArea.bottom) / 2);
                            ctx.font = fontSizeToUse + "px " + fontStyle;
                            ctx.fillStyle = color;

                            //Draw text in center
                            ctx.fillText(txt, centerX, centerY);
                        }
                    }
                });
            }
        },
        executionDate: {
            init: function (workFrontId, sewerGroupId) {
                _app.loader.showOnElement("#portlet1");
                $.ajax({
                    url: _app.parseUrl(`/reportes/calidad/tramos-certificados/fecha-de-ejecucion?workFrontId=${workFrontId}&sewerGroupId=${sewerGroupId}`)
                }).done(function (result) {
                    var data = result.map(function(e, i) {
                        return {
                            t: new Date(e.year, e.month, e.day),
                            y: i + 1
                        };
                    });

                    var labels = data.map((x) => x.t.toLocaleString());

                    if (chart1) {
                        chart1.data.labels = labels;
                        chart1.data.datasets[0].data = data;
                        chart1.options.scales.xAxes[0].time.min = data.length ? data[0].t : 0;
                        chart1.update();
                    }
                    else {
                        var ctx = document.getElementById("myChart1").getContext("2d");
                        chart1 = new Chart(ctx, {
                            type: "line",
                            data: {
                                labels: labels,
                                datasets: [{
                                    label: "Todos los Frentes",
                                    data: data,
                                    backgroundColor: "rgba(255, 99, 132, 0.2)",
                                    borderColor: "rgba(255, 99, 132, 0.2)"
                                }]
                            },
                            options: {
                                scales: {
                                    xAxes: [{
                                        type: "time",
                                        time: {
                                            min: data[0].t,
                                            unit: "month",
                                            displayFormats: {
                                                "month": 'MMM YYYY'
                                            }
                                        }
                                    }]
                                }
                            }
                        });
                    }
                    _app.loader.hideOnElement("#portlet1");
                });
            }
        },
        certified: {
            init: function (workFrontId, sewerGroupId) {
                _app.loader.showOnElement("#portlet2");
                $.ajax({
                    url: _app.parseUrl(`/reportes/calidad/tramos-certificados/certificados?workFrontId=${workFrontId}&sewerGroupId=${sewerGroupId}`)
                }).done(function (result) {
                    var data = {
                        datasets: [{
                            data: [result.hasCertificate, result.noCertificate],
                            backgroundColor: [
                                "rgb(255, 99, 132)",
                                "rgb(54, 162, 235)"]
                        }],
                        labels: [
                            "Tramos Certificados",
                            "Tramos No Certificados"
                        ]
                    };

                    let total = result.hasCertificate + result.noCertificate;
                    let certificateProp = total === 0 ? 0 : result.hasCertificate / total;
                    certificateProp *= 100;

                    if (chart2) {
                        chart2.data = data;
                        chart2.options.elements.center.text = certificateProp.toPercent();
                        chart2.update();
                    } else {
                        var ctx = document.getElementById("myChart2").getContext("2d");
                        chart2 = new Chart(ctx, {
                            type: "doughnut",
                            data: data,
                            options: {
                                elements: {
                                    center: {
                                        text: certificateProp.toPercent(),
                                        color: "#FF6384"
                                    }
                                }
                            }
                        });
                    }
                    _app.loader.hideOnElement("#portlet2");
                });
            }
        },
        files: {
            init: function (workFrontId, sewerGroupId) {
                _app.loader.showOnElement("#portlet3");
                $.ajax({
                    url: _app.parseUrl(`/reportes/calidad/tramos-certificados/archivos?workFrontId=${workFrontId}&sewerGroupId=${sewerGroupId}`)
                }).done(function (result) {
                    var data = {
                        datasets: [{
                            data: [result.hasCertificateAndFile, result.noCertificateAndFile],
                            backgroundColor: [
                                "rgb(255, 99, 132)",
                                "rgb(54, 162, 235)"]
                        }],
                        labels: [
                            "Tramos Certificados con Archivo subido",
                            "Tramos No Certificados"
                        ]
                    };

                    let total = result.hasCertificateAndFile + result.noCertificateAndFile;
                    let certificateProp = total === 0 ? 0 : result.hasCertificateAndFile / total;
                    certificateProp *= 100;

                    if (chart3) {
                        chart3.data = data;
                        chart3.options.elements.center.text = certificateProp.toPercent();
                        chart3.update();
                    }
                    else {
                        var ctx = document.getElementById("myChart3").getContext("2d");
                        chart3 = new Chart(ctx, {
                            type: "doughnut",
                            data: data,
                            options: {
                                elements: {
                                    center: {
                                        text: certificateProp.toPercent(),
                                        color: '#FF6384'
                                    }
                                }
                            }
                        });
                    }
                    _app.loader.hideOnElement("#portlet3");
                });
            }
        }
	};

	var select2 = {
		init: function () {
            this.workFronts.init();
            this.fillingLaboratoryTests.init();
		},
        workFronts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/frentes")
                }).done(function (result) {
                    $(".select2-work-fronts")
                        .on("change", function () {
                            select2.sewerGroups.init(`#${$(this).attr("id").replace("work_front", "sewer_group")}`, $(this).val());
                        })
                        .select2({
                            data: result,
                            placeholder: "Frente"
                        }).trigger("change");
                });
            }
        },
        sewerGroups: {
            init: function (selector, workFrontId) {
                if (selector)
                    $(selector).empty();
                else {
                    selector = ".select2-sewer-groups";
                }
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas?type=1&workFrontId=${workFrontId}`)
                }).done(function (result) {
                    result.unshift({
                        text: "Todas",
                        id: "Todas"
                    });
                    $(selector).select2({
                        data: result,
                        placeholder: "Cuadrilla"
                    }).trigger("change");
                });
            }
        },
        fillingLaboratoryTests: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/pruebas-de-laboratorio-de-rellenos")
                }).done(function (result) {
                    $(".select2-filling-laboratory-tests").select2({
                        data: result,
                        placeholder: "Proctor"
                    });
                });
            }
        }
	};

	var events = {
        init: function () {
            $("#work_front_filter1, #sewer_group_filter1").on("change", function () {
                let workFrontId = $("#work_front_filter1").val();
                let sewerGroupId = $("#sewer_group_filter1").val();
                chart.executionDate.init(workFrontId, sewerGroupId);
            });

            $("#work_front_filter2, #sewer_group_filter2").on("change", function () {
                let workFrontId = $("#work_front_filter2").val();
                let sewerGroupId = $("#sewer_group_filter2").val();
                chart.certified.init(workFrontId, sewerGroupId);
            });

            $("#work_front_filter3, #sewer_group_filter3").on("change", function () {
                let workFrontId = $("#work_front_filter3").val();
                let sewerGroupId = $("#sewer_group_filter3").val();
                chart.files.init(workFrontId, sewerGroupId);
            });
		}
	};

	return {
        init: function () {
            events.init();
            chart.config.init();
            select2.init();
            chart.executionDate.init();
            chart.certified.init();
            chart.files.init();
		}
	};
}();

$(function () {
	CertifiedSewerLines.init();
});
