var CompactionDensityCertificate = function () {

    var sewerLineDatatable = null;
    var addForm = null;
    var editForm = null;
    var formRepeater = null;
    var importDataForm = null;
    var importFilesForm = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/calidad/certificado-densidad-compactacion/listar"),
            data: function (d) {
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.hasCertificate = $("#has_certificate").is(":checked");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Detalle",
                width: "5%",
                className: "details-control",
                orderable: false,
                data: null,
                defaultContent: "<i class='flaticon2-next'></i>"
            },
            {
                title: "Frente",
                data: "sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Ubicación",
                data: "address"
            },
            {
                title: "Tramo",
                data: "name"
            },
            {
                title: "Registro N°",
                data: "compactionDensityCertificate.serialNumber"
            },
            {
                title: "Fecha de Término",
                data: "compactionDensityCertificate.executionDate"
            },
            {
                title: "Clase de Material",
                data: "compactionDensityCertificate.materialType",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.fillingLaboratory.materialType.VALUES);
                }
            },
            {
                title: "Cantera",
                data: "compactionDensityCertificate.quarry.name"
            },
            {
                title: "Profunfidad Prom. Tramo",
                data: "averageDepthSewerLine",
                render: _app.render.measure
            },
            {
                title: "DN (mm)",
                data: "nominalDiameter",
                render: _app.render.measure
            },
            {
                title: "Capas",
                data: "layers"
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.compactionDensityCertificate.serialNumber !== "---") {
                        tmp += `<button data-id="${row.id}" data-layers="${row.layers}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                        tmp += `<i class="la la-edit"></i></button> `;
                    }
                    else {
                        tmp += `<button data-id="${row.id}" data-layers="${row.layers}" class="btn btn-info btn-sm btn-icon btn-add">`;
                        tmp += `<i class="la la-plus"></i></button> `;
                    }
                    if (row.compactionDensityCertificate.fileUrl) {
                        tmp += `<button data-id="${row.compactionDensityCertificate.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="la la-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var optionsDetail = {
        responsive: true,
        ajax: {
            url: "",
            dataSrc: ""
        },
        rowCallback: function (row, data) {
            //if (data.id) {
            //    if (data.firstResult > 210 && data.secondResult > 210) {
            //        $(row).css("background-color", "#f2d3cb");
            //    }
            //    else {
            //        $(row).css("background-color", "#ccffcc");
            //    }
            //}
        },
        columns: [
            {
                title: "Capa",
                data: "layer",
                render: function (data, type, row) {
                    return row.latest ? "Rasante" : data;
                }
            },
            {
                title: "Fecha de Ensayo",
                data: "testDate"
            },
            {
                title: "Muestra N°",
                data: "fillingLaboratoryTest.recordNumber"
            },
            {
                title: "Densidad Húmeda",
                data: "wetDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "% Humedad",
                data: "moisture",
                render: function (data) {
                    return data.toMeasure(1);
                }
            },
            {
                title: "Densidad Seca",
                data: "dryDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "Max. Densidad Seca",
                data: "fillingLaboratoryTest.maxDensity",
                render: function (data) {
                    return data.toMeasure(3);
                }
            },
            {
                title: "Opt. Contenido Humedad",
                data: "fillingLaboratoryTest.optimumMoisture",
                render: function (data) {
                    return data.toMeasure(1);
                }
            },
            {
                title: "% Densidad",
                data: "densityPercentage",
                className: "font-weight-bold",
                render: function (data) {
                    return data.toMeasure(1);
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            sewerLineDatatable = $("#sewer_line_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            sewerLineDatatable.ajax.reload();
        },
        initEvents: function () {
            sewerLineDatatable.on("click", "td.details-control", function () {
                var tr = $(this).closest("tr");
                var row = sewerLineDatatable.row(tr);
                console.log(row);
                if (row.child.isShown()) {
                    row.child.hide();
                    $(this).html("<i class='flaticon2-next'></i>");
                    tr.removeClass('shown');
                }
                else {
                    row.child(datatable.child.init(row.data())).show();
                    $(this).html("<i class='flaticon2-down'></i>");
                    tr.addClass('shown');
                }
            });

            sewerLineDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });

            sewerLineDatatable.on("click", ".btn-add", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                let layers = $btn.data("layers");
                form.load.add(id, layers);
            });
        },
        child: {
            init: function (data) {
                optionsDetail.ajax.url = _app.parseUrl(`/calidad/certificado-densidad-compactacion/detalle/${data.id}/listar`);
                var $table = $(`<table id="certificate_${data.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
                $table.DataTable(optionsDetail);
                this.initEvents($table);
                return $table;
            },
            initEvents: function (table) {
                table.on("click", ".btn-view", function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.pdf(id);
                });
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/certificado-densidad-compactacion/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Description']").val(result.description);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/certificado-densidad-compactacion/${id}`)
                }).done(function (result) {
                    $("#pdf_serial_number").text(result.serialNumber);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            add: function (id, layers) {
                _app.loader.show();
                let formElements = $("#add_form");
                formElements.find("[name='SewerLineId']").val(id);
                let list = [];
                for (i = 0; i < layers; ++i) {
                    list.push({
                        Layer: i === layers -1 ? "Rasante" : i + 1
                    });
                }
                formRepeater.setList(list);
                $("#add_modal").modal("show");
                _app.loader.hide();
            }
        },
        submit: {
            add: function (formElement) {
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
                    url: _app.parseUrl("/calidad/certificado-densidad-compactacion/crear"),
                    method: "post",
                    contentType: false,
                    processData: false,
                    data: data,
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
                        datatable.reload();
                        $("#add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, textarea").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/certificado-calidad-concreto/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, textarea").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },

            import: {
                data: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: "/calidad/certificado-densidad-compactacion/importar-datos",
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
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
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        datatable.reload();
                        $("#import_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_data_alert_text").html(error.responseText);
                            $("#import_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                },
                files: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: "/calidad/certificado-densidad-compactacion/importar-archivos",
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data,
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
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        datatable.reload();
                        $("#import_files_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_files_alert_text").html(error.responseText);
                            $("#import_files_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            import: {
                data: function () {
                    importDataForm.resetForm();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
                files: function () {
                    importFilesForm.resetForm();
                    $("#import_files_form").trigger("reset");
                    $("#import_files_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var select2 = {
        init: function () {
            this.workFronts.init();
            this.sewerGroups.init();
            this.materialTypes.init();
            this.quarries.init();
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
        materialTypes: {
            init: function () {
                $(".select2-material-types").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        quarries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/canteras")
                }).done(function (result) {
                    $(".select2-quarries").select2({
                        data: result
                    });
                });
            }
        },
        fillingLaboratoryTests: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/pruebas-de-laboratorio-de-rellenos")
                }).done(function (result) {
                    $(".select2-filling-laboratory-tests").select2({
                        data: result
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.data(formElement);
                }
            });

            importFilesForm = $("#import_files_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import.files(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.data();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.files();
                });
        }
    };

    var events = {
        init: function () {
            formRepeater = $('#kt_repeater_1').repeater({
                initEmpty: false,
                isFirstItemUndeletable: true,
                defaultValues: {
                    //Segment: "1",
                    //SegmentNumber: "100"
                },
                show: function () {
                    $(this).slideDown();
                    $("#add_form .select2-filling-laboratory-tests").each((i, e) => {
                        $(e).prop("id", `select_filling_laboratory_test_${i}`);
                    });
                    select2.fillingLaboratoryTests.init();
                    datepicker.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
            });

            $("#sewer_group_filter, #has_certificate").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datepicker.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    CompactionDensityCertificate.init();
});