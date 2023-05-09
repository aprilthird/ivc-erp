var IsoStandard = function () {

    var isoStandardDatatable = null;
    var addForm = null;
    var editForm = null;
    var sgOption = new Option('--Seleccione una Formula--', null, true, true);
    var allOption = new Option('Todos', null, true, true);
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/bim/listar"),
            data: function (d) {
                d.workFrontId = $("#work_filter").val();
                d.projectFormulaId = $("#formula_filter").val();
               
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Fórmula",
                data: "projectFormula.code",
            render: function (data, type, row) {
                var tmp = data + "-" + row.projectFormula.name;

                return tmp;
            }
            },
            {
                title: "Frente",
                data: "workFront.code"
            },
            {
                title: "BIM",
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
                        text: "El BIM será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/bim/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El BIM sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el BIM"
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
                    url: _app.parseUrl(`/oficina-tecnica/bim/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_formula']").val(result.projectFormulaId).trigger("change");

                        formElements.find("[name='WorkFrontId']").val(result.workFrontId);
                        formElements.find("[name='select_front']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='Name']").val(result.name);

                        select2.fronts.edit(result.projectFormulaId, result.workFrontId);
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
                    url: _app.parseUrl(`/oficina-tecnica/bim/${id}`)
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
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_front']").val());
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
                    url: _app.parseUrl("/oficina-tecnica/bim/crear"),
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
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_front']").val());
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
                    url: _app.parseUrl(`/oficina-tecnica/bim/editar/${id}`),
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

            this.formulas.init();
            this.fronts.init();
            this.formulas2.init();
            this.works2.init();

        },
        formulas: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({

                    url: _app.parseUrl(`/select/formulas-proyecto-filtro?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-formulas").append(sgOption).trigger('change');
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
            }
        },
        fronts: {
            init: function () {
                $(".select2-fronts").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-fronts").empty();
                    $(".select2-fronts").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}`)
                }).done(function (result) {
                    $(".select2-fronts").empty();
                    $(".select2-fronts").select2({
                        data: result
                    });
                    $(".select2-fronts").val(eqsid).trigger('change');
                });
            },

        },
        formulas2: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/formulas-proyecto-filtro?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-formulas2").select2({
                        data: result
                    });
                });
            }
        },
        works2: {
            init: function () {
                $(".select2-works2").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula-todos?projectFormulaId=${sg}&projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-works2").empty();
                    $(".select2-works2").append(allOption).trigger('change');
                    $(".select2-works2").select2({
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

            });

            $(".select2-formulas2").on("change", function () {
                select2.works2.reload(this.value);

            });

            $("#work_filter,#formula_filter").on("change", function () {
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