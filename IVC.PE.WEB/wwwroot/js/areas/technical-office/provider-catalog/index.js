var IsoStandard = function () {

    var isoStandardDatatable = null;
    var addForm = null;
    var editForm = null;
    var sgOption = new Option('--Seleccione una Familia--', null, true, true);
    var allOption = new Option('Todos', null, true, true);
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/catalogo-de-proveedores/listar"),
            data: function (d) {
                d.groupId = $("#group_filter").val();
                d.familyId = $("#family_filter").val();
                d.specId = $("#spec_filter").val();

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Familia",
                data: "supplyFamily.code",
                render: function (data, type, row) {
                    var tmp = data + "-" + row.supplyFamily.name;
    
                    return tmp;
                }
            },
            {
                title: "Grupo",
                data: "supplyGroup.code",
                render: function (data, type, row) {
                    var tmp = data + "-" + row.supplyGroup.name;

                    return tmp;
                }
            },

            {
                title: "Especialidad",
                data: "speciality.description"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Proveedor",
                data: "provider.tradename"
            },
            {
                title: "Catálogo",
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
                        text: "El catálogo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/oficina-tecnica/catalogo-de-proveedores/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El catálogo sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el catálogo"
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
                    url: _app.parseUrl(`/oficina-tecnica/catalogo-de-proveedores/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId);
                        formElements.find("[name='select_family']").val(result.supplyFamilyId).trigger("change");
                        
                        formElements.find("[name='SupplyGroupId']").val(result.supplyGroupId);
                        formElements.find("[name='select_group']").val(result.supplyGroupId).trigger("change");

                        select2.groups.edit(result.supplyFamilyId, result.supplyGroupId);


                        formElements.find("[name='SpecialityId']").val(result.specialityId);
                        formElements.find("[name='select_spec']").val(result.specialityId).trigger("change");
                        formElements.find("[name='ProviderId']").val(result.providerId);
                        formElements.find("[name='select_provider']").val(result.providerId).trigger("change");

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
                    url: _app.parseUrl(`/oficina-tecnica/catalogo-de-proveedores/${id}`)
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
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_family']").val());
                $(formElement).find("[name='SupplyGroupId']").val($(formElement).find("[name='select_group']").val());
                
                $(formElement).find("[name='SpecialityId']").val($(formElement).find("[name='select_spec']").val());
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());

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
                    url: _app.parseUrl("/oficina-tecnica/catalogo-de-proveedores/crear"),
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
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_family']").val());
                $(formElement).find("[name='SupplyGroupId']").val($(formElement).find("[name='select_group']").val());
                $(formElement).find("[name='SpecialityId']").val($(formElement).find("[name='select_spec']").val());
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
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
                    url: _app.parseUrl(`/oficina-tecnica/catalogo-de-proveedores/editar/${id}`),
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

            this.groups.init();
            this.familys.init();
            this.specs.init();
            this.familys2.init();
            this.groups2.init();
            this.specs2.init();

            this.providers.init();

        },

        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores")
                }).done(function (result) {
                    $(".select2-providers").select2({
                        data: result
                    });
                });
            }
        },
        groups: {
            init: function () {
                $(".select2-groups").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/grupos-de-insumos-familia?familyId=${sg}`)
                }).done(function (result) {
                    $(".select2-groups").empty();
                    $(".select2-groups").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/grupos-de-insumos-familia?familyId=${sg}`)
                }).done(function (result) {
                    $(".select2-groups").empty();
                    $(".select2-groups").select2({
                        data: result
                    });
                    $(".select2-groups").val(eqsid).trigger('change');
                });
            },

        },
        specs: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/especialidades?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-specs").select2({
                        data: result
                    });
                });
            }
        },
        specs2: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/especialidades?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-specs2").select2({
                        data: result
                    });
                });
            }
        },

        groups2: {
            init: function () {
                $(".select2-groups").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/grupos-de-insumos-familia?familyId=${sg}`)
                }).done(function (result) {
                    $(".select2-groups2").empty();
                    $(".select2-groups2").append(allOption).trigger('change');
                    $(".select2-groups2").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/grupos-de-insumos-familia?familyId=${sg}`)
                }).done(function (result) {
                    $(".select2-groups2").empty();
                    $(".select2-groups2").append(allOption).trigger('change');
                    $(".select2-groups2").select2({
                        data: result
                    });
                    $(".select2-groups").val(eqsid).trigger('change');
                });
            },

        },

        familys2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-familys2").append(allOption).trigger('change');
                    $(".select2-familys2").select2({
                        data: result
                    });
                });
            }
        },
        familys: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-familys").append(sgOption).trigger('change');
                    $(".select2-familys").select2({
                        data: result
                    });
                });
            }
        },
  

       
        
       
     
    };
    var events = {
        init: function () {

            $(".select2-familys").on("change", function () {
                select2.groups.reload(this.value);

            });

            $(".select2-familys2").on("change", function () {
                select2.groups2.reload(this.value);

            });
            $("#group_filter,#family_filter,#spec_filter").on("change", function () {
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