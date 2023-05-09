var ConcreteQualityCertificate = function () {

    var sewerBoxDatatable = null;
    var addForm = null;
    var editForm = null;
    var sewerBoxes = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/calidad/certificado-calidad-concreto/listar"),
        },
        rowCallback: function (row, data) {
            if (data.maxSlabAge === 28) {
                $(row).find("td:eq(4)").css("background-color", "#ccffcc");
            }
            else if (data.maxSlabAge >= 7) {
                $(row).find("td:eq(4)").css("background-color", "#fcf4a3");
            }
            else {
                $(row).find("td:eq(4)").css("background-color", "#f2d3cb");
            }
            if (data.maxBodyAge === 28) {
                $(row).find("td:eq(5)").css("background-color", "#ccffcc");
            }
            else if (data.maxBodyAge >= 7) {
                $(row).find("td:eq(5)").css("background-color", "#fcf4a3");
            }
            else {
                $(row).find("td:eq(5)").css("background-color", "#f2d3cb");
            }
            if (data.maxRoofAge === 28) {
                $(row).find("td:eq(6)").css("background-color", "#ccffcc");
            }
            else if (data.maxRoofAge >= 7) {
                $(row).find("td:eq(6)").css("background-color", "#fcf4a3");
            }
            else {
                $(row).find("td:eq(6)").css("background-color", "#f2d3cb");
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
                data: "sewerBox.sewerGroup.workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerBox.sewerGroup.code"
            },
            {
                title: "Buzón",
                data: "sewerBox.code"
            },
            {
                title: "Losa",
                data: "maxSlabAge"
            },
            {
                title: "Cuerpo",
                data: "maxBodyAge"
            },
            {
                title: "Techo",
                data: "maxRoofAge"
            } 
            //{
            //    title: "Opciones",
            //    width: "10%",
            //    data: null,
            //    orderable: false,
            //    render: function (data, type, row) {
            //        var tmp = "";
            //        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
            //        tmp += `<i class="fa fa-edit"></i></button> `;
            //        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
            //        tmp += `<i class="fa fa-trash"></i></button>`;
            //        return tmp;
            //    }
            //}
        ]
    };

    var optionsDetail = {
        responsive: true,
        ajax: {
            url: "",
            dataSrc: ""
        },
        rowCallback: function (row, data) {
            if (data.id) {
                if (data.firstResult > 210 && data.secondResult > 210) {
                    $(row).css("background-color", "#f2d3cb");
                }
                else {
                    $(row).css("background-color", "#ccffcc");
                }
            }
        },
        columns: [
            {
                title: "Edad",
                data: "concreteQualityCertificate.age"
            },
            {
                title: "Estructura",
                data: "segment",
                render: function (data) {
                    return _app.render.badge(data, _app.constants.certificate.concreteQuality.segment.VALUES);
                }
            },
            {
                title: "N° Estructura",
                data: "segmentNumber"
            },
            {
                title: "Fecha de Muestreo",
                data: "concreteQualityCertificate.samplingDate"
            },
            {
                title: "Fecha de Ensayo",
                data: "concreteQualityCertificate.testDate"
            },
            {
                title: "N° de Registro",
                data: "concreteQualityCertificate.certificateSerialNumber"
            },
            {
                title: "N° de Registro de Vaciado",
                data: "concreteQualityCertificate.for07SerialNumber"
            },
            {
                title: "Resistencia (Probeta 1)",
                data: "concreteQualityCertificate.firstResult",
                render: _app.render.measure
            },
            {
                title: "Resistencia (Probeta 2)",
                data: "concreteQualityCertificate.secondResult",
                render: _app.render.measure
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.id) {
                        tmp += `<button data-id="${row.concreteQualityCertificateId}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            sewerBoxDatatable = $("#sewer_box_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            sewerBoxDatatable.ajax.reload();
        },
        initEvents: function () {
            sewerBoxDatatable.on("click", "td.details-control", function () {
                var tr = $(this).closest("tr");
                var row = sewerBoxDatatable.row(tr);
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
        },
        child: {
            init: function (data) {
                optionsDetail.ajax.url = _app.parseUrl(`/calidad/certificado-calidad-concreto/detalle/${data.sewerBox.id}/listar`);
                var $table = $(`<table id="certificate_${data.sewerBox.id}" class="table table-striped table-bordered table-hover table-checkable datatable"></table>`);
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
                    url: _app.parseUrl(`/calidad/certificado-calidad-concreto/${id}`)
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
                    url: _app.parseUrl(`/calidad/certificado-calidad-concreto/${id}`)
                }).done(function (result) {
                    $("#pdf_serial_number").text(result.certificateSerialNumber);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.certificateFileUrl + "&embedded=true");
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
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
                    url: _app.parseUrl("/calidad/certificado-calidad-concreto/crear"),
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
            }
        }
    };

    var select2 = {
        init: function () {
            this.ages.init();
            this.segments.init();
            this.sewerBoxes.init();
        },
        ages: {
            init: function () {
                $(".select2-ages").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        segments: {
            init: function () {
                $(".select2-segments").select2({
                    minimumResultsForSearch: -1
                }).trigger("change");
            }
        },
        sewerBoxes: {
            init: function () {
                if (sewerBoxes) {
                    $(".select2-sewer-boxes").select2({
                        data: sewerBoxes,
                        placeholder: "Buzón"
                    });
                    return;
                }
                $.ajax({
                    url: _app.parseUrl("/select/buzones")
                }).done(function (result) {
                    sewerBoxes = result.slice(1, 75);
                    $(".select2-sewer-boxes").select2({
                        data: sewerBoxes,
                        placeholder: "Buzón",
                        //minimumInputLength: 3,
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
        }
    };

    var events = {
        init: function () {
            $('#kt_repeater_1').repeater({
                initEmpty: false,
                isFirstItemUndeletable: true,
                defaultValues: {
                    Segment: "1",
                    //SegmentNumber: "100"
                },
                show: function () {
                    $(this).slideDown();
                    $("#add_form .select2-sewer-boxes").each((i, e) => {
                        $(e).prop("id", `select_sewer_box_${i}`);
                    });
                    $("#add_form .select2-segments").each((i, e) => {
                        $(e).prop("id", `select_segment_${i}`);
                    });
                    $("#add_form .input-numbers").each((i, e) => {
                        $(e).prop("id", `input_number_${i}`);
                        if (!$(e).val()) {
                            $(e).val(1);
                        }
                    });
                    select2.sewerBoxes.init();
                    select2.segments.init();
                },
                hide: function (deleteElement) {
                    $(this).slideUp(deleteElement);
                }
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
    ConcreteQualityCertificate.init();
});