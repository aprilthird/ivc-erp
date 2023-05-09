"use strict";

google.charts.load('current', { 'packages': ['controls'] });
var today = new Date();
var todaydate = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
// Class definition
var KTDashboard = function () {

    var hoursByWeekChart = null;
    var costsByWeekChart = null;
    var workersWeekCountChart = null;

    var TransportChart = {
        init: function () {
            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(this.drawScq);
        },
        drawScq: function () {
            $.ajax({
                url: _app.parseUrl(`/equipos/prueba-grafico-transporte/barras-transporte`),
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
                var rowHeight = data.getNumberOfRows() * 40;
                var chartHeight = rowHeight + paddingHeight + 100;

                // Chart Options
                var options = {

                    
                    title: "Equipos Transporte hasta la fecha "+todaydate ,
                    height: chartHeight,
                    chartArea: {
                        left: 150,
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

                // Draw Dashboard
                dashboard.draw(data);
                chart.draw();
            });
        }
    };

    var MachChart = {
        init: function () {
            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(this.drawScq2);
        },
        drawScq2: function () {
            $.ajax({
                url: _app.parseUrl(`/equipos/prueba-grafico-maquinaria/barras-transporte`),
                dataType: "json",
                async: false
            }).done(function (result) {

                console.log(result);

                // Create dashboard
                var dashboard = new google.visualization.Dashboard(
                    document.getElementById('scq2Dashboard')
                );

                // Create our data table out of JSON data loaded from server.
                var data = new google.visualization.DataTable(result);

                var paddingHeight = 40;
                var rowHeight = data.getNumberOfRows() * 40;
                var chartHeight = rowHeight + paddingHeight + 100;

                // Chart Options
                var options = {


                    title: "Equipos Maquinaria hasta la fecha " + todaydate,
                    height: chartHeight - 50 ,
                    chartArea: {
                        left: 150,
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
                    containerId: 'scq2Chart',
                    options: options,
                    dataTable: data
                });

                // Draw Dashboard
                dashboard.draw(data);
                chart.draw();
            });
        }
    };

    // Based on Chartjs plugin - http://www.chartjs.org/
    //Cartas Enviadas Recibidas
    var lettersChart = function () {
        if($('#kt_chart_letters').length == 0) {
            return;
        }

        var ctx = document.getElementById("kt_chart_letters").getContext("2d");

        var config = {
            type: 'doughnut',
            data: {
                labels: [],
                datasets: [{
                    label: "Cartas",
                    data: [],
                    backgroundColor: ['red','blue'],
                }]
            },
            options: {
                title: {
                    display: false,
                },
                responsive: true,
            }
        };

        var chart = new Chart(ctx, config);

        $.ajax({
            url: _app.parseUrl("/cartas-enviadas-recibidas")
        }).done(function (result) {
            chart.data.labels = result.labels;
            chart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
            chart.update(); // finally update our chart
        });
    };

    //Horas por semana
    var hoursByWeekChartDraw = {
        init: function () {
            if ($('#kt_chart_hours').length == 0) {
                return;
            }

            var ctx = document.getElementById("kt_chart_hours").getContext("2d");

            var gradient = ctx.createLinearGradient(0, 0, 0, 240);
            gradient.addColorStop(0, Chart.helpers.color('#d1f1ec').alpha(1).rgbString());
            gradient.addColorStop(1, Chart.helpers.color('#d1f1ec').alpha(0.3).rgbString());

            var config = {
                type: 'line',
                data: {
                    labels: [],
                    datasets: [{
                        label: "Horas",
                        backgroundColor: gradient,
                        borderColor: KTApp.getStateColor('success'),
                        pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointHoverBackgroundColor: KTApp.getStateColor('danger'),
                        pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),
                        data: []
                    }]
                },
                options: {
                    title: {
                        display: false,
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10
                    },
                    legend: {
                        display: false
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        xAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Month'
                            }
                        }],
                        yAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            },
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    elements: {
                        line: {
                            tension: 0.0000001
                        },
                        point: {
                            radius: 4,
                            borderWidth: 12
                        }
                    },
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 10,
                            bottom: 0
                        }
                    }
                }
            };

            hoursByWeekChart = new Chart(ctx, config);

            $.ajax({
                url: _app.parseUrl("/horas-semana")
            }).done(function (result) {
                $("#kt_chart_hours_title").html(result.data.quantity[9].toLocaleString('en-US') + " h trabajadas");
                $("#kt_chart_hours_subtitle").html("Al " + result.labels[9]);
                hoursByWeekChart.data.labels = result.labels;
                hoursByWeekChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                hoursByWeekChart.update(); // finally update our chart
            });
        },
        reload: function (id, category, text) {
            $.ajax({
                url: _app.parseUrl(`/horas-semana?sgId=${id}&category=${category}`)
            }).done(function (result) {
                $("#kt_chart_hours_filter").html(text);
                $("#kt_chart_hours_title").html(result.data.quantity[9].toLocaleString('en-US') + " h trabajadas");
                $("#kt_chart_hours_subtitle").html("Al " + result.labels[9]);
                hoursByWeekChart.data.labels = result.labels;
                hoursByWeekChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                hoursByWeekChart.update(); // finally update our chart
            });
        }
    };
    //Costos por semana
    var costsByWeekChartDraw = {
        init: function () {
            if ($('#kt_chart_costs').length == 0) {
                return;
            }

            var ctx = document.getElementById("kt_chart_costs").getContext("2d");

            var gradient = ctx.createLinearGradient(0, 0, 0, 240);
            gradient.addColorStop(0, Chart.helpers.color('#d1f1ec').alpha(1).rgbString());
            gradient.addColorStop(1, Chart.helpers.color('#d1f1ec').alpha(0.3).rgbString());

            var config = {
                type: 'line',
                data: {
                    labels: [],
                    datasets: [{
                        label: "Costos de Mano de Obra",
                        backgroundColor: gradient,
                        borderColor: KTApp.getStateColor('success'),
                        pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointHoverBackgroundColor: KTApp.getStateColor('danger'),
                        pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),
                        data: []
                    }]
                },
                options: {
                    title: {
                        display: false,
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10
                    },
                    legend: {
                        display: false
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        xAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Month'
                            }
                        }],
                        yAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            },
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    elements: {
                        line: {
                            tension: 0.0000001
                        },
                        point: {
                            radius: 4,
                            borderWidth: 12
                        }
                    },
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 10,
                            bottom: 0
                        }
                    }
                }
            };

            costsByWeekChart = new Chart(ctx, config);

            $.ajax({
                url: _app.parseUrl("/costos-semana")
            }).done(function (result) {
                $("#kt_chart_costs_title").html("S/ " + result.data.quantity[9].toLocaleString('en-US'));
                $("#kt_chart_costs_subtitle").html("Al " + result.labels[9]);
                costsByWeekChart.data.labels = result.labels;
                costsByWeekChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                costsByWeekChart.update(); // finally update our chart
            });
        },
        reload: function (id, category, text) {
            $.ajax({
                url: _app.parseUrl(`/costos-semana?sgId=${id}&category=${category}`)
            }).done(function (result) {
                $("#kt_chart_costs_filter").html(text);
                $("#kt_chart_costs_title").html("S/ " + result.data.quantity[9].toLocaleString('en-US'));
                $("#kt_chart_costs_subtitle").html("Al " + result.labels[9]);
                costsByWeekChart.data.labels = result.labels;
                costsByWeekChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                costsByWeekChart.update(); // finally update our chart
            });
        }
    };
    //Obreros por semana
    var workersWeekCountChartDraw = {
        init: function () {
            if ($('#kt_chart_workers_week_count').length == 0) {
                return;
            }

            var ctx = document.getElementById("kt_chart_workers_week_count").getContext("2d");

            var gradient = ctx.createLinearGradient(0, 0, 0, 240);
            gradient.addColorStop(0, Chart.helpers.color('#d1f1ec').alpha(1).rgbString());
            gradient.addColorStop(1, Chart.helpers.color('#d1f1ec').alpha(0.3).rgbString());

            var config = {
                type: 'line',
                data: {
                    labels: [],
                    datasets: [{
                        label: "Obreros",
                        backgroundColor: gradient,
                        borderColor: KTApp.getStateColor('success'),
                        pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                        pointHoverBackgroundColor: KTApp.getStateColor('danger'),
                        pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),
                        data: []
                    }]
                },
                options: {
                    title: {
                        display: false,
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10
                    },
                    legend: {
                        display: false
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        xAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Month'
                            }
                        }],
                        yAxes: [{
                            display: false,
                            gridLines: false,
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            },
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    elements: {
                        line: {
                            tension: 0.0000001
                        },
                        point: {
                            radius: 4,
                            borderWidth: 12
                        }
                    },
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 10,
                            bottom: 0
                        }
                    }
                }
            };

            workersWeekCountChart = new Chart(ctx, config);

            $.ajax({
                url: _app.parseUrl("/obreros-semana")
            }).done(function (result) {
                $("#kt_chart_workers_week_count_count").html(result.data.quantity[9] + " obreros");
                $("#kt_chart_workers_week_count_week").html("Al " + result.labels[9]);
                workersWeekCountChart.data.labels = result.labels;
                workersWeekCountChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                workersWeekCountChart.update(); // finally update our chart
            });
        },
        reload: function (id, category, text) {
            $.ajax({
                url: _app.parseUrl(`/obreros-semana?sgId=${id}&category=${category}`)
            }).done(function (result) {
                $("#kt_chart_workers_week_count_filter").html(text);
                $("#kt_chart_workers_week_count_count").html(result.data.quantity[9] + " obreros");
                $("#kt_chart_workers_week_count_week").html("Al " + result.labels[9]);
                workersWeekCountChart.data.labels = result.labels;
                workersWeekCountChart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
                workersWeekCountChart.update(); // finally update our chart
            });
        }
    };

    //Carta fianza activas
    var bondsChart = function () {
        $.ajax({
            url: _app.parseUrl("/cartas-fianza")
        }).done(function (result) {
            $("#kt_chart_bonds_title").html("S/ " + result.totalPen.toLocaleString('en-US'));
            $("#kt_chart_bonds_subtitle").html("En " + result.total + " cartas fianza");
        });
    };
        
    // Daterangepicker Init
    var daterangepickerInit = function() {
        if ($('#kt_dashboard_daterangepicker').length == 0) {
            return;
        }

        var picker = $('#kt_dashboard_daterangepicker');
        var start = moment();
        var end = moment();

        function cb(start, end, label) {
            var title = '';
            var range = '';

            if ((end - start) < 100 || label == 'Today') {
                title = 'Today:';
                range = start.format('MMM D');
            } else if (label == 'Yesterday') {
                title = 'Yesterday:';
                range = start.format('MMM D');
            } else {
                range = start.format('MMM D') + ' - ' + end.format('MMM D');
            }

            $('#kt_dashboard_daterangepicker_date').html(range);
            $('#kt_dashboard_daterangepicker_title').html(title);
        }

        picker.daterangepicker({
            direction: KTUtil.isRTL(),
            startDate: start,
            endDate: end,
            opens: 'left',
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }
        }, cb);

        cb(start, end, '');
    }
    
    // Calendar Init
    var calendarInit = function() {
        if ($('#kt_calendar').length === 0) {
            return;
        }
        
        var todayDate = moment().startOf('day');
        var YM = todayDate.format('YYYY-MM');
        var YESTERDAY = todayDate.clone().subtract(1, 'day').format('YYYY-MM-DD');
        var TODAY = todayDate.format('YYYY-MM-DD');
        var TOMORROW = todayDate.clone().add(1, 'day').format('YYYY-MM-DD');

        $('#kt_calendar').fullCalendar({
            isRTL: KTUtil.isRTL(),
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listWeek'
            },
            editable: true,
            eventLimit: true, // allow "more" link when too many events
            navLinks: true,
            defaultDate: moment('2017-09-15'),
            events: [
                {
                    title: 'Meeting',
                    start: moment('2017-08-28'),
                    description: 'Lorem ipsum dolor sit incid idunt ut',
                    className: "fc-event-light fc-event-solid-warning"
                },
                {
                    title: 'Conference',                    
                    description: 'Lorem ipsum dolor incid idunt ut labore',
                    start: moment('2017-08-29T13:30:00'),
                    end: moment('2017-08-29T17:30:00'),
                    className: "fc-event-success"
                },
                {
                    title: 'Dinner',
                    start: moment('2017-08-30'),
                    description: 'Lorem ipsum dolor sit tempor incid',
                    className: "fc-event-light  fc-event-solid-danger"
                },
                {
                    title: 'All Day Event',
                    start: moment('2017-09-01'),
                    description: 'Lorem ipsum dolor sit incid idunt ut',
                    className: "fc-event-danger fc-event-solid-focus"
                },
                {
                    title: 'Reporting',                    
                    description: 'Lorem ipsum dolor incid idunt ut labore',
                    start: moment('2017-09-03T13:30:00'),
                    end: moment('2017-09-04T17:30:00'),
                    className: "fc-event-success"
                },
                {
                    title: 'Company Trip',
                    start: moment('2017-09-05'),
                    end: moment('2017-09-07'),
                    description: 'Lorem ipsum dolor sit tempor incid',
                    className: "fc-event-primary"
                },
                {
                    title: 'ICT Expo 2017 - Product Release',
                    start: moment('2017-09-09'),
                    description: 'Lorem ipsum dolor sit tempor inci',
                    className: "fc-event-light fc-event-solid-primary"
                },
                {
                    title: 'Dinner',
                    start: moment('2017-09-12'),
                    description: 'Lorem ipsum dolor sit amet, conse ctetur'
                },
                {
                    id: 999,
                    title: 'Repeating Event',
                    start: moment('2017-09-15T16:00:00'),
                    description: 'Lorem ipsum dolor sit ncididunt ut labore',
                    className: "fc-event-danger"
                },
                {
                    id: 1000,
                    title: 'Repeating Event',
                    description: 'Lorem ipsum dolor sit amet, labore',
                    start: moment('2017-09-18T19:00:00'),
                },
                {
                    title: 'Conference',
                    start: moment('2017-09-20T13:00:00'),
                    end: moment('2017-09-21T19:00:00'),
                    description: 'Lorem ipsum dolor eius mod tempor labore',
                    className: "fc-event-success"
                },
                {
                    title: 'Meeting',
                    start: moment('2017-09-11'),
                    description: 'Lorem ipsum dolor eiu idunt ut labore'
                },
                {
                    title: 'Lunch',
                    start: moment('2017-09-18'),
                    className: "fc-event-info fc-event-solid-success",
                    description: 'Lorem ipsum dolor sit amet, ut labore'
                },
                {
                    title: 'Meeting',
                    start: moment('2017-09-24'),
                    className: "fc-event-warning",
                    description: 'Lorem ipsum conse ctetur adipi scing'
                },
                {
                    title: 'Happy Hour',
                    start: moment('2017-09-24'),
                    className: "fc-event-light fc-event-solid-focus",
                    description: 'Lorem ipsum dolor sit amet, conse ctetur'
                },
                {
                    title: 'Dinner',
                    start: moment('2017-09-24'),
                    className: "fc-event-solid-focus fc-event-light",
                    description: 'Lorem ipsum dolor sit ctetur adipi scing'
                },
                {
                    title: 'Birthday Party',
                    start: moment('2017-09-24'),
                    className: "fc-event-primary",
                    description: 'Lorem ipsum dolor sit amet, scing'
                },
                {
                    title: 'Company Event',
                    start: moment('2017-09-24'),
                    className: "fc-event-danger",
                    description: 'Lorem ipsum dolor sit amet, scing'
                },
                {
                    title: 'Click for Google',
                    url: 'http://google.com/',
                    start: moment('2017-09-26'),
                    className: "fc-event-solid-info fc-event-light",
                    description: 'Lorem ipsum dolor sit amet, labore'
                }
            ],

            eventRender: function(event, element) {
                if (element.hasClass('fc-day-grid-event')) {
                    element.data('content', event.description);
                    element.data('placement', 'top');
                    KTApp.initPopover(element);
                } else if (element.hasClass('fc-time-grid-event')) {
                    element.find('.fc-title').append('<div class="fc-description">' + event.description + '</div>');
                } else if (element.find('.fc-list-item-title').lenght !== 0) {
                    element.find('.fc-list-item-title').append('<div class="fc-description">' + event.description + '</div>');
                }
            }
        });
    }

    

    var chartRacsDailyCount = function () {
        if ($('#kt_chart_racs_daily_count').length == 0) {
            return;
        }

        var ctx = document.getElementById("kt_chart_racs_daily_count").getContext("2d");

        var gradient = ctx.createLinearGradient(0, 0, 0, 240);
        gradient.addColorStop(0, Chart.helpers.color('#ffefce').alpha(1).rgbString());
        gradient.addColorStop(1, Chart.helpers.color('#ffefce').alpha(0.3).rgbString());

        var config = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: "RACS",
                    backgroundColor: gradient,
                    borderColor: KTApp.getStateColor('warning'),
                    pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                    pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                    pointHoverBackgroundColor: KTApp.getStateColor('danger'),
                    pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),
                    data: []
                }]
            },
            options: {
                title: {
                    display: false,
                },
                tooltips: {
                    mode: 'nearest',
                    intersect: false,
                    position: 'nearest',
                    xPadding: 10,
                    yPadding: 10,
                    caretPadding: 10
                },
                legend: {
                    display: false
                },
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    xAxes: [{
                        display: false,
                        gridLines: false,
                        scaleLabel: {
                            display: true,
                            labelString: 'Month'
                        }
                    }],
                    yAxes: [{
                        display: false,
                        gridLines: false,
                        scaleLabel: {
                            display: true,
                            labelString: 'Value'
                        },
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                elements: {
                    line: {
                        tension: 0.0000001
                    },
                    point: {
                        radius: 4,
                        borderWidth: 12
                    }
                },
                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 10,
                        bottom: 0
                    }
                }
            }
        };

        var chart = new Chart(ctx, config);

        $.ajax({
            url: _app.parseUrl("/racs-diario")
        }).done(function (result) {
            $("#kt_chart_racs_daily_count_count").html(result.data.quantity[14] + " RACS nuevos");
            $("#kt_chart_racs_daily_count_day").html("Al " + result.labels[14]);
            chart.data.labels = result.labels;
            chart.data.datasets[0].data = result.data.quantity; // or you can iterate for multiple datasets
            chart.update(); // finally update our chart
        });
    }

    var select2 = {
        init: function () {
            this.sewergroups.init();
            this.categories.ini();
        },
        sewergroups: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-dashboard`)
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result,
                        allowClear: true
                    });
                })
            }
        },
        categories: {
            ini: function () {
                $(".select2-categories").select2({
                    allowClear: true
                });
            }
        }
    };

    var requests = {
        init: function () {
            this.opt();
        },
        opt: function () {
            $.ajax({
                url: _app.parseUrl('/requerimientos-pendientes'),
                dataSrc: ""
            }).done(function (result) {
                $("#pending_requests").empty();
                $.each(result, function (i, item) {
                    let element = requests.get(item.correlativeCodeStr, item.reviewDate, item.id);
                    $("#pending_requests").append(element);
                });

                $("#pending_requests").on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        window.location = `/logistica/requerimientos/excel/${id}`;
                    });

                $("#pending_requests").on("click",
                    ".btn-send",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        _app.loader.show();
                        $.ajax({
                            url: _app.parseUrl(`/requerimiento/${id}`)
                        })
                            .done(function (result) {
                                let formElements = $("#send_form");
                                formElements.find("[name='Id']").val(result.id);
                                formElements.find("[name='Email']").val(result.email);
                                formElements.find("[name='Name']").val(result.name);
                                formElements.find("[name='Email']").disabled = true;
                                formElements.find("[name='Name']").disabled = true;

                                $("#send_modal").modal("show");
                            })
                            .always(function () {
                                _app.loader.hide();
                            });
                    });

                $("#pending_requests").on("click",
                    ".btn-approved",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        Swal.fire({
                            title: "\u00BFEst\u00E1 seguro?",
                            text: `El Requerimiento ${name} ser\u00E1 aprobado`,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "S\u00ED, aprobarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/aprobar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: `El Requerimiento ${name} ha sido aprobado con \u00E9xito`,
                                                confirmButtonText: "Excelente"
                                            });
                                            $.ajax({
                                                url: _app.parseUrl('/requerimientos-pendientes'),
                                                dataSrc: ""
                                            }).done(function (result) {
                                                $("#pending_requests").empty();
                                                $.each(result, function (i, item) {
                                                    let element = requests.get(item.correlativeCodeStr, item.reviewDate, item.id);
                                                    $("#pending_requests").append(element);
                                                });
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurri\u00F3 un error al intentar aprobar el Requerimiento ${name}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                $("#pending_requests").on("click",
                    ".btn-cancel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        Swal.fire({
                            title: "\u00BFEst\u00E1 seguro?",
                            text: `El Requerimiento ${name} ser\u00E1 anulado`,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "S\u00ED, anular",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/requerimientos/cancelar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: `El Requerimiento ${name} ha sido anulado con \u00E9xito`,
                                                confirmButtonText: "Excelente"
                                            });
                                            $.ajax({
                                                url: _app.parseUrl('/requerimientos-pendientes'),
                                                dataSrc: ""
                                            }).done(function (result) {
                                                $("#pending_requests").empty();
                                                $.each(result, function (i, item) {
                                                    let element = requests.get(item.correlativeCodeStr, item.reviewDate, item.id);
                                                    $("#pending_requests").append(element);
                                                });
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurri\u00F3 un error al intentar anular el Requerimiento ${name}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            });
        },
        get: function (name, datetime, id) {
            return $(`<div class="kt-widget2__item kt-widget2__item--primary">
                                                        <div class="kt-widget2__checkbox">
                                                        </div>
                                                        <div class="kt-widget2__info">
                                                            <a href="#" class="kt-widget2__title">
                                                                ${name}
                                                            </a>
                                                            <a href="#" class="kt-widget2__username">
                                                                Registrada el ${datetime}
                                                            </a>
                                                        </div>
                                                        <div class="kt-widget2__actions">
                                                            <a data-id="${id}" class="btn btn-clean btn-sm btn-icon btn-excel" title="Descargar Excel">
                                                                <i class="la la-file-excel-o"></i>
                                                            </a>
                                                            <a data-id="${id}" data-name="${name}" class="btn btn-clean btn-sm btn-icon btn-cancel" title="Anular">
                                                                <i class="la la-times-circle"></i>
                                                            </a>
                                                            <a data-id="${id}" class="btn btn-clean btn-sm btn-icon btn-send" title="Enviar Observación" >
                                                                <i class="la la-comment"></i>
                                                            </a>
                                                            <a data-id="${id}" data-name="${name}" class="btn btn-clean btn-sm btn-icon btn-approved" title="Aprobar">
                                                                <i class="la la-check-circle"></i>
                                                            </a>
                                                        </div>
                                                    </div>`);
        }
    }

    var orders = {
        init: function () {
            this.opt();
        },
        opt: function () {
            $.ajax({
                url: _app.parseUrl('/ordenes-pendientes'),
                dataSrc: ""
            }).done(function (result) {
                $("#pending_orders").empty();
                $.each(result, function (i, item) {
                    let element = orders.get(item.correlativeCodeStr, item.reviewDate, item.id);
                    $("#pending_orders").append(element);
                });

                $("#pending_orders").on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        window.location = `/logistica/ordenes/excel/${id}`;
                    });

                $("#pending_orders").on("click",
                    ".btn-send",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        _app.loader.show();
                        $.ajax({
                            url: _app.parseUrl(`/orden/${id}`)
                        })
                            .done(function (result) {
                                let formElements = $("#send_order_form");
                                formElements.find("[name='Id']").val(result.id);
                                formElements.find("[name='Email']").val(result.email);
                                formElements.find("[name='Name']").val(result.name);
                                formElements.find("[name='Email']").disabled = true;
                                formElements.find("[name='Name']").disabled = true;

                                $("#send_order_modal").modal("show");
                            })
                            .always(function () {
                                _app.loader.hide();
                            });
                    });

                $("#pending_orders").on("click",
                    ".btn-approved",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        Swal.fire({
                            title: "\u00BFEst\u00E1 seguro?",
                            text: `La Orden ${name} ser\u00E1 aprobado`,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "S\u00ED, aprobarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/ordenes/aprobar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: `La Orden ${name} ha sido aprobada con \u00E9xito`,
                                                confirmButtonText: "Excelente"
                                            });
                                            $.ajax({
                                                url: _app.parseUrl('/ordenes-pendientes'),
                                                dataSrc: ""
                                            }).done(function (result) {
                                                $("#pending_Orders").empty();
                                                $.each(result, function (i, item) {
                                                    let element = orders.get(item.correlativeCodeStr, item.reviewDate, item.id);
                                                    $("#pending_Orders").append(element);
                                                });
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurri\u00F3 un error al intentar aprobar la Orden ${name}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                $("#pending_orders").on("click",
                    ".btn-cancel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");
                        Swal.fire({
                            title: "\u00BFEst\u00E1 seguro?",
                            text: `La Orden ${name} ser\u00E1 anulada`,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "S\u00ED, anular",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/ordenes/cancelar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: `La Orden ${name} ha sido anulada con \u00E9xito`,
                                                confirmButtonText: "Excelente"
                                            });
                                            $.ajax({
                                                url: _app.parseUrl('/ordenes-pendientes'),
                                                dataSrc: ""
                                            }).done(function (result) {
                                                $("#pending_orders").empty();
                                                $.each(result, function (i, item) {
                                                    let element = orders.get(item.correlativeCodeStr, item.reviewDate, item.id);
                                                    $("#pending_orders").append(element);
                                                });
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurri\u00F3 un error al intentar anular la Order ${name}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            });
        },
        get: function (name, datetime, id) {
            return $(`<div class="kt-widget2__item kt-widget2__item--primary">
                                                        <div class="kt-widget2__checkbox">
                                                        </div>
                                                        <div class="kt-widget2__info">
                                                            <a href="#" class="kt-widget2__title">
                                                                ${name}
                                                            </a>
                                                            <a href="#" class="kt-widget2__username">
                                                                Registrada el ${datetime}
                                                            </a>
                                                        </div>
                                                        <div class="kt-widget2__actions">
                                                            <a data-id="${id}" class="btn btn-clean btn-sm btn-icon btn-excel" title="Descargar Excel">
                                                                <i class="la la-file-excel-o"></i>
                                                            </a>
                                                            <a data-id="${id}" data-name="${name}" class="btn btn-clean btn-sm btn-icon btn-cancel" title="Anular">
                                                                <i class="la la-times-circle"></i>
                                                            </a>
                                                            <a data-id="${id}" class="btn btn-clean btn-sm btn-icon btn-send" title="Enviar Observación" >
                                                                <i class="la la-comment"></i>
                                                            </a>
                                                            <a data-id="${id}" data-name="${name}" class="btn btn-clean btn-sm btn-icon btn-approved" title="Aprobar">
                                                                <i class="la la-check-circle"></i>
                                                            </a>
                                                        </div>
                                                    </div>`);
        }
    }

    var events = {
        init: function () {
            $("#btnWorkerFilters").on("click", function () {
                var data = $('#worker_filter_sewergroup').select2('data');
                let category = $("#category_filter").val();
                console.log(category);
                //let sgId = $("#worker_filter_sewergroup").val();
                //let sgText = $("#worker_filter_sewergroup").val();
                console.log(data);
                if (data.length === 0) {
                    console.log("terrible");
                    hoursByWeekChartDraw.reload(null, category, "");
                    costsByWeekChartDraw.reload(null, category, "");
                    workersWeekCountChartDraw.reload(null, category, "");
                } else {
                    let sgId = data[0].id;
                    let sgText = data[0].text + " ";
                    hoursByWeekChartDraw.reload(sgId, category, sgText);
                    costsByWeekChartDraw.reload(sgId, category, sgText);
                    workersWeekCountChartDraw.reload(sgId, category, sgText);
                }
            });
        }
    };

    return {
        // Init demos
        init: function() {
            // init charts
            TransportChart.init();
            MachChart.init();
            workersWeekCountChartDraw.init();
            chartRacsDailyCount();
            hoursByWeekChartDraw.init();
            lettersChart();
            costsByWeekChartDraw.init();
            bondsChart();

            // init daterangepicker
            daterangepickerInit();

            // calendar
            calendarInit();

            select2.init();
            events.init();
            
            // demo loading
            var loading = new KTDialog({'type': 'loader', 'placement': 'top center', 'message': 'Loading ...'});
            loading.show();

            setTimeout(function() {
                loading.hide();
            }, 3000);

            requests.init();
            orders.init();
        }
    };
}();

// Class initialization on page load
jQuery(document).ready(function() {
    KTDashboard.init();
});