var for24First = function () {

    var addForm = null;
    var editForm = null;
    var for24FirstDatatable = null;

    var for24FirstDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/sistema-de-manejo-integrado/first-part-for24/listar"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "for24First",
                data: "newSIGProcess.processName"
            },
            {
                //title: "Reportado",
                data: "reportUserName"
            },
            {
                //title: "Fecha",
                data: "date"
            },
            {
                //title: "Tipo Origen",
                data: "originType"
            },
            {
                //title: "Origen NC",
                data: "ncOrigin"
            },
            {
                //title: "Nombre Report",
                data: "sewerGroup.code"
            },
            {
                //title: "Descripcion",
                data: "description"
            },
            {
                //title: "Nombre del Producto",
                data: "productName"
            },
            {
                //title: "Cantidad",
                data: "quantity"
            },
            {
                //title: "Marca/Proveedor",
                data: "brandProvider"
            },
            {
                //title: "Codigo/Referencia",
                data: "codeReference"
            },
            {
                //title: "Fecha de Vencimiento",
                data: "expirationDate"
            },
            {
                //title: "Responsable",
                data: "responsableUserName"
            },
            {
                data: "videoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Video" data-name="${row.newSIGProcess.processName}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-gallery">`;
                    tmp += `<i class="fa fa-folder"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-secondary btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.for24FirstDt.init();
            this.initEvents();
        },
        for24FirstDt: {
            init: function () {
                for24FirstDatatable = $("#for24s_datatable").DataTable(for24FirstDtOpt);
            },
            reload: function () {
                for24FirstDatatable.ajax.reload();
            }
        },
        initEvents: function () {
            for24FirstDatatable.on("click",
                ".btn-gallery",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.gallery(id);
                });

            for24FirstDatatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.pdf(id);
                });

            for24FirstDatatable.on("click", ".btn-play", function () {
                let $btn = $(this);
                let testName = $btn.data("test");
                let smName = $btn.data("name");
                let videourl = $btn.data("videourl");
                forms.load.vid(videourl, `${testName} - ${smName}`);
            });

            for24FirstDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Estás Seguro?",
                        text: "El for24First será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn-danger",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatables.for24FirstDt.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo técnico ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        })
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el archivo ténico"
                                        });
                                    }
                                });
                            });
                        },

                    })
                });
        }
    };

    var forms = {
        load: {
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.newSIGProcess.processName);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    console.log(result.fileUrl);

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            gallery: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/sistema-de-manejo-integrado/first-part-for24/${id}`)
                }).done(function (result) {
                    var count = 1;
                    $('#container').empty();
                    result.gallery.forEach(function (image) {
                        console.log(image.url);
                        console.log(count);
                        if(count == 1)
                            $('#container').append('<div class="carousel-item active"><h5>'+image.name+'</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                        else
                            $('#container').append('<div class="carousel-item"><h5>' + image.name +'</h5><img src="' + image.url + '" style="width:100%;" /></div>');
                        count++;
                    });
                    $("#gallery_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            vid: function (videourl, testName) {
                $("#test_name").html(testName);
                $("#video_frame").prop("src", videourl);
                $("#video_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='NewSIGProcessId']").val($(formElement).find("[name='select_processes']").val());
                $(formElement).find("[name='ReportUserId']").val($(formElement).find("[name='select_report_user']").val());
                $(formElement).find("[name='OriginType']").val($(formElement).find("[name='select_origin_Type']").val());
                $(formElement).find("[name='NCOrigin']").val($(formElement).find("[name='select_nc_origin']").val());
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='ResponsableUserId']").val($(formElement).find("[name='select_responsable_user']").val());
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
                    url: _app.parseUrl("/sistema-de-manejo-integrado/first-part-for24/crear"),
                    method: "post",
                    processData: false,
                    contentType: false,
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
                        datatables.for24FirstDt.reload();
                        $("#add_modal").modal("hide");
                        $(".select2-report-users").val('').trigger("change");
                        $(".select2-reponsable-users").val('').trigger("change");
                        $(".select2-processes").val('').trigger("change");
                        $(".select2-sewer-groups").val('').trigger("change");
                        $(".select2-providers").val('').trigger("change");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
             add: function () {
                    addForm.resetForm();
                    $("#add_form").trigger("reset");
                    $("#add_alert").removeClass("show").addClass("d-none");
                }
            }   
            
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.add(formElement);
                }
            });

        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });
        }
    };

    var select2 = {
        init: function () {
            this.users.init();
            this.sigProcess.init();
            this.sewerGroups.init();
            this.providers.init();
        },
        users: {
            init: function () {
                $(".select2-report-users").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-report-users").select2({
                        placeholder: "--Seleccionar--",
                        allowClear: true,
                        data: result
                    });
                });

                $(".select2-reponsable-users").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-reponsable-users").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        sigProcess: {
            init: function () {
                $(".select2-processes").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/sig-process")
                }).done(function (result) {
                    $(".select2-processes").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        sewerGroups: {
            init: function () {
                $(".select2-sewer-groups").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-jefe-frente")
                }).done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        },
        providers: {
            init: function () {
                $(".select2-providers").trigger("change");
                $.ajax({
                    url: _app.parseUrl("/select/proveedores")
                }).done(function (result) {
                    $(".select2-providers").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    var ncOriginOpts = {
        init: function () {
            $("#add_modal").find("[name='select_nc_origin']").trigger("change");
            $(".provider").hide();
            $(".internal-process").hide();
            $("#add_modal").find("[name='select_nc_origin']").change(function () {
                var data = $(this).val();
                console.log(data);
                if (data == 1) {
                    $(".client").show();
                    $(".select2-sewer-groups").val('').trigger("change");
                    $(".select2-providers").val('').trigger("change");
                    $(".provider").hide();
                    $(".internal-process").hide();
                } else if (data == 2) {
                    $(".provider").show();
                    $(".select2-sewer-groups").val('').trigger("change");
                    $("#add_modal").find("[name='Client']").val('');
                    $(".internal-process").hide();
                    $(".client").hide();
                } else if (data == 3) {
                    $(".internal-process").show();
                    $(".select2-providers").val('').trigger("change");
                    $("#add_modal").find("[name='Client']").val('');
                    $(".provider").hide();
                    $(".client").hide();
                }
            })
        }
    };

    var originTypeOpt = {
        init: function () {
            $("#add_modal").find("[name='select_origin_Type']").trigger("change");
            $("#add_modal").find("[name='select_origin_Type']").change(function () {
                var data = $(this).val();
                console.log(data);
                if (data == 1)
                    $(".product").show();
                else if (data == 2)
                    $(".product").hide();
            })
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
            ncOriginOpts.init();
            originTypeOpt.init();
        }
    };
}();

$(function () {
    for24First.init();

    
});