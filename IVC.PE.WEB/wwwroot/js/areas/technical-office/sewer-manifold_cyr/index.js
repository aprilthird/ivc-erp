var SewerManifoldCP = function () {
    var selectWeekOption = new Option('--Seleccione una Semana--', null, true, true);

    var isBusy = false;

    var cpsDatatable = null;
    var sgsDatatable = null;

    var loadCPForm = null;
    var loadSGForm = null;

    var cpsOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/colector-descarga-cyr/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Tipo de Terreno",
                data: "terrainTypeStr"
            },
            {
                title: "Min",
                data: "heightMin",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Max",
                data: "heightMax",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Unid.",
                data: "unit"
            },
            {
                title: "Precio S/",
                data: "price",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Factor Seguridad",
                data: "securityFactor",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO",
                data: "workforce",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "EQ",
                data: "equipment",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "S/C",
                data: "services",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Mat.",
                data: "materials",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ",
                data: "workforceEquipment",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ+S/C",
                data: "workforceEquipmentServices",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ(FS)",
                data: "workforceEquipmentSf",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ+S/C(FS)",
                data: "workforceEquipmentServicesSf",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                width: "8%",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-file-excel'></i> Cargar Excel CyR Base",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    $("#load_modal").modal("show");
                }
            }
        ]
    };
    var sgsOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/cuadrillas-colector-descarga-cyr/listar"),
            data: function (d) {
                d.weekId = $("#week_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Descripción",
                data: "sewerManifoldCostPerformance.description"
            },
            {
                title: "Semana",
                data: "projectCalendarWeek.yearWeekNumber"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Factor Seguridad",
                data: "securityFactor",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ",
                data: "workforceEquipment",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Long. Mínima",
                data: "workforceEquipmentMinLength",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "MO+EQ+S/C",
                data: "workforceEquipmentService",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Long. Mínima",
                data: "workforceEquipmentServiceMinLength",
                render: function (data, type, row) {
                    return data.toFixed(2);
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                width: "8%",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-file-excel'></i> Cargar Excel CyR",
                className: "btn-success",
                action: function (e, dt, node, config) {
                    $("#load_sg_modal").modal("show");
                }
            }
        ],
        rowGroup: {
            dataSrc: "sewerGroup.code"
        }
    };

    var datatables = {
        init: function () {
            this.cpDt.init();
            this.sgDt.init();
        },
        cpDt: {
            init: function () {
                cpsDatatable = $("#cps_datatable").DataTable(cpsOpts);
            },
            reload: function () {
                cpsDatatable.ajax.reload();
            }
        },
        sgDt: {
            init: function () {
                sgsDatatable = $("#sgs_datatable").DataTable(sgsOpts);
            },
            reload: function () {
                sgsDatatable.ajax.reload();
                isBusy = false;
            }
        }
    };

    var forms = {
        submit: {
            loadCP: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/colector-descarga-cyr/carga-base"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyFile) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        return xhr;
                    }
                })
                .always(function () {
                    $btn.removeLoader();
                    $(formElement).find("input, select").prop("disabled", false);
                    $(".progress").empty().remove();
                    if (!emptyFile) {
                        $("#space-bar").remove();
                    }
                })
                .done(function (result) {
                    datatables.cpDt.reload();
                    $("#load_modal").modal("hide");
                    _app.show.notification.add.success();
                })
                .fail(function (error) {
                    if (error.responseText) {
                        $("#load_alert_text").html(error.responseText);
                        $("#load_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.add.error();
                });
            },
            loadSG: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/cuadrillas-colector-descarga-cyr/cargar-costos"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyFile) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        return xhr;
                    }
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.sgDt.reload();
                        $("#load_sg_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#load_alert_text").html(error.responseText);
                            $("#load_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            loadCP: function () {
                loadCPForm.reset();
                $("#load_form").trigger("reset");
                $("#load_alert").removeClass("show").addClass("d-none");
            },
            loadSG: function () {
                loadSGForm.reset();
                $("#load_sg_form").trigger("reset");
                $("#load_sg_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            loadCPForm = $("#load_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.loadCP(formElement);
                }
            });

            loadSGForm = $("#load_sg_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.loadSG(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#load_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.loadCP();
                });

            $("#load_sg_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.loadSG();
                });
        }
    };

    var select2 = {
        init: function () {
            this.sewergroups.init();
            this.weeks.init();
        },
        sewergroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas`)
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            }
        },
        weeks: {
            init: function () {
                projectId = $("#project_general_filter").val();
                year = $("#year_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").append(selectWeekOption);
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {
                $(".select2-weeks").empty();
                $(".select2-weeks").append(selectOption).trigger('change');
                projectId = $("#project_general_filter").val();
                year = $("#year_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/semanas?projectId=${projectId}&year=${year}`)
                }).done(function (result) {
                    $(".select2-weeks").empty();
                    $(".select2-weeks").append(selectWeekOption);
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("#year_filter").on("change", function () {
                year = $("#year_filter").val();
                select2.weeks.reload();
            });

            $("#week_filter").on("change", function () {
                if (!isBusy) {
                    isBusy = true;
                    datatables.sgDt.reload();
                }
            });

            $("#genCPSgExcel").on("click", function () {
                window.location = _app.parseUrl(`/oficina-tecnica/cuadrillas-colector-descarga-cyr/cargar-costos-modelo`);
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            validate.init();
            modals.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    SewerManifoldCP.init();
});