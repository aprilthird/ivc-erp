var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var selectSGOption = new Option('--Seleccione un Tipo de Equipo--', null, true, true);
    var for05Datatable = null;
    var foldingDatatable = null;

    var importDataForm = null;

    var equipId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/proveedores-de-equipos/listar"),
            data: function (d) {
                d.equipmentProviderId = $("#provider_filter").val();
                d.classEquip = $("#equipment_filter").val();
                d.equipmentType = $("#type_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre Comercial",
                data: "tradename"
            },
            {
                title: "RUC",
                data: "ruc"
            },
            {
                title: "Dirección",
                data: "address"
            },
            {
                title: "Número de Telefono",
                data: "phoneNumber"
            },
            {
                title: "Nombre de persona de contacto",
                data: "collectionAreaContactName"
            },
            {
                title: "Número de telefono de persona de contacto",
                data: "collectionAreaPhoneNumber"
            },
            {
                title: "Correo de persona de contacto",
                data: "collectionAreaEmail"
            },
            {
                title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Clase de Equipos",
                data: "equips"
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

    var foldingDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/equipos/proveedores-de-equipos-folding/listar`),
            data: function (d) {
                d.equipmentProviderId = equipId;
                d.providerId = $("#provider_filter").val();
                d.classEquip = $("#equipment_filter").val();
                d.equipmentType = $("#type_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Clase de Equipo",
                data: "equipmentMachineryType.description"
            },
            {
                title: "Tipo de Equipo",
                data: "equipmentMachineryType.description",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == "Maquinaria")
                        tmp += row.equipmentMachineryTypeType.description
                    else if (data == "Equipos Menores")
                        tmp += row.equipmentMachineryTypeSoft.description;
                    else if (data == "Transporte")
                        tmp += row.equipmentMachineryTypeTransport.description;
                    return tmp;
                }
            },
            {
                title: "Opciones",
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

    var datatables = {
        init: function () {
            this.for05Dt.init();
            this.foldingDt.init();
        },
        for05Dt: {
            init: function () {
                for05Datatable = $("#main_datatable").DataTable(for05DtOpt);
                this.initEvents();
            },
            reload: function () {
                for05Datatable.ajax.reload();
            },
            initEvents: function () {

                for05Datatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        equipId = id;
                        datatables.foldingDt.reload();
                        forms.load.detail(id);
                    });

                for05Datatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        forms.load.foldingFor05(id);
                    });

                for05Datatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });


                for05Datatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Proveedor de equipo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/proveedores-de-equipos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Proveedor de Equipo ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Proveedor de Equipo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        foldingDt: {
            init: function () {
                foldingDatatable = $("#folding_datatable").DataTable(foldingDtOpt);
                this.events();
            },
            reload: function () {
                foldingDatatable.ajax.reload();
 
            },
            events: function () {
                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editfolding(id);
                    });

                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El detalle será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/proveedores-de-equipos-folding/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt.reload();
                                            datatables.for05Dt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El detalle ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el detalle"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        }
    };
    var forms = {
        load: {
            edit: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProviderId']").val(result.providerId);
                        formElements.find("[name='select_provider']").val(result.providerId).trigger("change");
                        
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProviderId']").val(result.providerId);
                        formElements.find("[name='select_provider']").val(result.providerId).trigger("change");
                        formElements.find("[name='select_provider']").attr("disabled", "disabled");
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentProviderId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos-folding/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SerieNumber']").val(result.serieNumber);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='EquipmentMachineryTypeSoftId']").val(result.equipmentMachineryTypeSoftId);

                        formElements.find("[name='EquipmentMachineryTypeTransportId']").val(result.equipmentMachineryTypeTransportId);
                        formElements.find("[name='select_machinery_type_transport']").val(result.equipmentMachineryTypeTransportId).trigger("change");

                        formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeTypeId']").val(result.equipmentMachineryTypeTypeId);
                        formElements.find("[name='select_machinery_type_type']").val(result.equipmentMachineryTypeTypeId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeId']").val(result.equipmentMachineryTypeId);
                        formElements.find("[name='select_machinery']").val(result.equipmentMachineryTypeId).trigger("change");
                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/proveedores-de-equipos/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                     
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
               
                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data,

                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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
                    url: "/equipos/proveedores-de-equipos/importar-datos",
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
                    datatables.for05Dt.reload();
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

        },
            addfolding: function (formElement) {
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_machinery_type_transport']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos-folding/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt.reload();
                        $("#add_folding_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding_alert_text").html(error.responseText);
                            $("#add_folding_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding: function (formElement) {
                
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_machinery_type_transport']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/proveedores-de-equipos-folding/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
                        datatables.foldingDt.reload();
                        $("#edit_folding_modal").modal("hide");
                        _app.show.notification.add.success();

                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding_alert_text").html(error.responseText);
                            $("#edit_folding_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");

                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");

            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
            },
            addfolding: function () {
                addFoldingForm.reset();
                $("#add_folding_form").trigger("reset");
                $("#add_folding_alert").removeClass("show").addClass("d-none");
            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
                $("#detail_modal").modal("show");
            },
          
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

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
                }
            });

            detailForm = $("#detail_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });


            addFoldingForm = $("#add_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addfolding(formElement);
                }
            });

            editFoldingForm = $("#edit_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editfolding(formElement);
                }
            });




        }
    };

    var select2 = {
        init: function () {
            this.styles.init();
            this.providers.init();
            this.equipproviders2.init();
            this.softs.init();
            this.machineries.init();
            this.transports.init();
            this.machinerys.init();
            this.machinerys2.init();
            this.types.init();
        },

        types: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-maquinaria-mixto`)
                }).done(function (result) {
                    $(".select2-types").select2({
                        data: result
                    });
                });
            },

            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-maquinaria-mixto-por-clase-folding?equipmentMachineryTypeId=${id}`)
                }).done(function (result) {

                    $(".select2-types").empty();
                    $(".select2-types").append(selectSGOption).trigger('change');
                    $(".select2-types").select2({
                        data: result
                    });
                });
            },


        },
        softs: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-liviano")
                }).done(function (result) {
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            }
        },
        transports: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-transporte")
                }).done(function (result) {
                    $(".select2-transports").select2({
                        data: result
                    });
                });
            }
        },

        machinerys2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-lista")
                }).done(function (result) {
                    $(".select2-machinerys2").select2({
                        data: result
                    });
                });
            }
        },

        machineries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-maquinaria")
                }).done(function (result) {
                    $(".select2-machineries").select2({
                        data: result
                    });
                });
            }
        },
        machinerys: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-maquinaria-lista")
                }).done(function (result) {
                    $(".select2-machinerys").select2({
                        data: result
                    });
                });
            }
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

        equipproviders2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos")
                }).done(function (result) {
                    $(".select2-equipproviders2").select2({
                        data: result
                    });
                });
            }
        },

        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.data();
                });

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding();
                });



        }
    };
    var events = {
        init: function () {

            $(".select2-machinerys2").on("change", function () {
                select2.types.reload(this.value);

            });

            $("#provider_filter,#equipment_filter , #type_filter").on("change", function () {
                for05Datatable.ajax.reload();
                foldingDatatable.ajax.reload();
            });

            $(".select2-machinerys").on("change", function () {
                var txt = $(".select2-machinerys option:selected").text();
                if (txt.indexOf("Equipos Menores") >= 0) {
                    $(".soft_group").attr("hidden", false);
                    $(".type_group").attr("hidden", true);
                    $(".transport_group").attr("hidden", true);

                } else if (txt.indexOf("Equipos Menores") <= 0) {
                    $(".soft_group").attr("hidden", true);

                }
                if (txt.indexOf("Maquinaria") >= 0) {
                    $(".soft_group").attr("hidden", true);
                    $(".type_group").attr("hidden", false);
                    $(".transport_group").attr("hidden", true);

                } else if (txt.indexOf("Maquinaria") <= 0) {
                    $(".type_group").attr("hidden", true);

                }
                if (txt.indexOf("Transporte") >= 0) {
                    $(".soft_group").attr("hidden", true);
                    $(".type_group").attr("hidden", true);
                    $(".transport_group").attr("hidden", false);

                } else if (txt.indexOf("Transporte") <= 0) {
                    $(".transport_group").attr("hidden", true);

                }
            });
        },

    };
    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    //var events = {


    //};

    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
            events.init();

        }
    };
}();

$(function () {
    For05.init();
});