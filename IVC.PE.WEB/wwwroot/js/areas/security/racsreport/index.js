// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['controls'] });

var RacsReport = function () {

    var ScqChart = {
        init: function () {
            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(this.drawScq);
        },
        drawScq: function () {
            $.ajax({
                url: _app.parseUrl(`/seguridad/reportes-racs/condicion-subestandar-grafico`),
                dataType: "json",
                async: false
            }).done(function (result) {

                console.log(result);

                // Create dashboard
                var dashboard = new google.visualization.Dashboard(
                    document.getElementById('scqDashboard')
                );

                // Create our data table out of JSON data loaded from server.
                var data = new google.visualization.DataTable(result);

                var paddingHeight = 40;
                var rowHeight = data.getNumberOfRows() * 50;
                var chartHeight = rowHeight + paddingHeight + 100;

                // Chart Options
                var options = {
                    title: "Condiciones Subestandar",
                    height: chartHeight,
                    chartArea: {
                        left: 600,
                        height: rowHeight
                    },
                    //bar: { groupWidth: "95%" },
                    legend: { position: "top" },
                    hAxis: {
                        viewWindow: {
                            interval: 5
                        }
                    }
                };

                // Create Chart
                var chart = new google.visualization.ChartWrapper({
                    chartType: 'BarChart',
                    containerId: 'scqChart',
                    options: options,
                    dataTable: data
                });

                // Create Control
                //var control = new google.visualization.ControlWrapper({
                //    controlType: 'DateRangeFilter',
                //    containerId: 'filters',
                //    options: {
                //        filterColumnIndex: 1,
                //        ui: {
                //            label: 'Semanas',
                //            showRangeValues: false
                //        }
                //    }
                //});

                // Bind Control
                //dashboard.bind(control, chart);

                // Draw Dashboard
                dashboard.draw(data);
                chart.draw();
            });
        }
    };

    var SaqChart = {
        init: function () {
            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(this.drawSaq);
        },
        drawSaq: function () {
            $.ajax({
                url: _app.parseUrl(`/seguridad/reportes-racs/acto-subestandar-grafico`),
                dataType: "json",
                async: false
            }).done(function (result) {

                console.log(result);

                // Create dashboard
                var dashboard = new google.visualization.Dashboard(
                    document.getElementById('saqDashboard')
                );

                // Create our data table out of JSON data loaded from server.
                var data = new google.visualization.DataTable(result);

                var paddingHeight = 40;
                var rowHeight = data.getNumberOfRows() * 50;
                var chartHeight = rowHeight + paddingHeight + 100;

                // Chart Options
                var options = {
                    title: "Actos Subestandar",
                    height: chartHeight,
                    chartArea: {
                        left: 600,
                        height: rowHeight
                    },
                    //bar: { groupWidth: "95%" },
                    legend: { position: "top" },
                };


                // Create Chart
                var chart = new google.visualization.ChartWrapper({
                    chartType: 'BarChart',
                    containerId: 'saqChart',
                    options: options,
                    dataTable: data
                });

                // Create Control
                //var control = new google.visualization.ControlWrapper({
                //    controlType: 'DateRangeFilter',
                //    containerId: 'filters',
                //    options: {
                //        filterColumnIndex: 1,
                //        ui: {
                //            label: 'Semanas',
                //            showRangeValues: false
                //        }
                //    }
                //});

                // Bind Control
                //dashboard.bind(control, chart);

                // Draw Dashboard
                dashboard.draw(data);
                chart.draw();
            });
        }
    };

    var events = {
        init: function () {
            $("#racs_sg_btn").on("click", function () {
                _app.loader.show();
                var year = $("#filter_year").val();
                var month = $("#filter_month").val();
                $.ajax({
                    url: _app.parseUrl(`/seguridad/reportes-racs/por-cudarillas?year=${year}&month=${month}`)
                }).always(function () {
                    _app.loader.hide();
                }).done(function (result) {
                    window.location = `/seguridad/reportes-racs/descargar-excel?excelName=${result}`;
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
    };

    var select2 = {
        init: function () {
            this.months.init();
        },
        months: {
            init: function () {
                $("#filter_month").select2();
            }
        }
    };

    return {
        init: function () {
            ScqChart.init();
            SaqChart.init();
            events.init();
            select2.init();
        }
    };
}();

$(function () {
    RacsReport.init();
});