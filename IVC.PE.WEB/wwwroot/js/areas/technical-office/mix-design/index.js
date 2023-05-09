var IsoStandard = function () {

    var isoStandardDatatable = null;
    var addForm = null;
    var editForm = null;
    var sgOption = new Option('--Seleccione una Formula--', null, true, true);
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/diseños-de-mezcla/listar"),
            data: function (d) {
                d.designId = $("#design_filter").val();
                d.typeId = $("#type_filter").val();
                d.aggId = $("#agg_filter").val();
                d.concreteId = $("#concrete_filter").val();
                d.add = $("#add_filter").val();
                d.versionId = $("#version_filter").val();

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Tipo de Diseño",
                data: "designType.description"
            }, 

            {
                title: "Tipo de Cemento",
                data: "cementType.description"
            }, 
            {
                title: "Tipo de Agregado",
                data: "aggregateType.description"
            }, 

          
            {
                title: "Uso de Concreto",
                data: "concreteUse.description"
            },
            {
                title: "c/aditivo",
                data: "additive",
            render: function (data, type, row) {
                    var tmp = "";
                    if (data) {
                        tmp += "Si";

                    }
                    else
                        tmp += "No";
                    return tmp;
                }
            },

            {
                title: "Nombre",
                data: "name"
            },

            {
                title: "Fecha",
                data: "designDateStr"
            },
            {
                title: "Versión",
                data: "technicalVersion.description"
            },
            {
                title: "Diseños",
                data: "fileUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            isoStandardDatatable = $("#iso_standard_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            isoStandardDatatable.ajax.reload();
        },
        initEvents: function () {
            isoStandardDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            isoStandardDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
            isoStandardDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Diseño será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/diseños-de-mezcla/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El Diseño sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
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
                                            text: "Ocurrió un error al intentar eliminar el Diseño"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/diseños-de-mezcla/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='DesignTypeId']").val(result.designTypeId);
                        formElements.find("[name='select_design']").val(result.designTypeId).trigger("change");
                        formElements.find("[name='DesignDateStr']").datepicker("setDate", result.designDateStr);
                        formElements.find("[name='CementTypeId']").val(result.cementTypeId);
                        formElements.find("[name='select_cement']").val(result.cementTypeId).trigger("change");
                        
                        formElements.find("[name='AggregateTypeId']").val(result.aggregateTypeId);
                        formElements.find("[name='select_agg']").val(result.aggregateTypeId).trigger("change");


                        formElements.find("[name='ConcreteUseId']").val(result.concreteUseId);
                        formElements.find("[name='select_concrete']").val(result.concreteUseId).trigger("change");
                        
                        formElements.find("[name='Additive']").val(result.additive.toString());
                        formElements.find("[name='select_additive']").val(result.additive.toString()).trigger("change");

                        formElements.find("[name='TechnicalVersionId']").val(result.technicalVersionId);
                        formElements.find("[name='select_version']").val(result.technicalVersionId).trigger("change");

                        formElements.find("[name='Name']").val(result.name);
                        
                        
                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/diseños-de-mezcla/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.title}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                
                $(formElement).find("[name='TechnicalVersionId']").val($(formElement).find("[name='select_version']").val());
                $(formElement).find("[name='DesignTypeId']").val($(formElement).find("[name='select_design']").val());
                $(formElement).find("[name='CementTypeId']").val($(formElement).find("[name='select_cement']").val());
                $(formElement).find("[name='AggregateTypeId']").val($(formElement).find("[name='select_agg']").val());
                $(formElement).find("[name='ConcreteUseId']").val($(formElement).find("[name='select_concrete']").val());
                $(formElement).find("[name='Additive']").val($(formElement).find("[name='select_additive']").val());

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
                    url: _app.parseUrl("/oficina-tecnica/diseños-de-mezcla/crear"),
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
                        $(formElement).find("input").prop("disabled", false);
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
                $(formElement).find("[name='TechnicalVersionId']").val($(formElement).find("[name='select_version']").val());
                $(formElement).find("[name='DesignTypeId']").val($(formElement).find("[name='select_design']").val());
                $(formElement).find("[name='CementTypeId']").val($(formElement).find("[name='select_cement']").val());
                $(formElement).find("[name='AggregateTypeId']").val($(formElement).find("[name='select_agg']").val());
                $(formElement).find("[name='ConcreteUseId']").val($(formElement).find("[name='select_concrete']").val());
                $(formElement).find("[name='Additive']").val($(formElement).find("[name='select_additive']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/diseños-de-mezcla/editar/${id}`),
                    method: "put",
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
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
    var select2 = {
        init: function () {



            this.versions.init();
            this.aggs.init();
            this.cements.init();
            this.concretes.init();
            this.designs.init();
        },
        


        versions: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/versiones?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-versions").select2({
                        data: result
                    });
                });
            }
        },

        designs: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/diseños?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-designs").select2({
                        data: result
                    });
                });
            }
        },

        cements: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-cemento?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-cements").select2({
                        data: result
                    });
                });
            }
        },

        aggs: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-agregado?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-aggs").select2({
                        data: result
                    });
                });
            }
        },

        concretes: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/uso-de-concreto?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-concretes").select2({
                        data: result
                    });
                });
            }
        },

      
       
        
       
     
    };

    var events = {
        init: function () {

            $(".select2-formulas").on("change", function () {
                select2.fronts.reload(this.value);
                select2.specs.reload(this.value);
            });

            $("#design_filter,#type_filter,#agg_filter,#concrete_filter,#add_filter,#version_filter").on("change", function () {
                datatable.reload();

            });







        },



    };
    return {
        init: function () {
            select2.init();
            datatable.init();
            datepicker.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    IsoStandard.init();
});