var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var selectSGOption = new Option('--Seleccione un Proveedor--', null, true, true);
    var for05Datatable = null;
    var foldingDatatable = null;

    var importDataForm = null;

    var softId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/parte-equipo-liviano/listar"),
            data: function (d) {
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proveedor",
                data: "equipmentProvider.provider.tradename"
            },
            {
                title: "Equipo Menor",
                data: "equipmentMachineryTypeSoft.description"
            },
            {
                title: "Marca",
                data: "equipmentMachinerySoft.brand"
            },
            {
                title: "Marca",
                data: "equipmentMachinerySoft.model"
            },
            {
                title: "Placa de Equipo Menor",
                data: "equipmentMachinerySoft.equipmentPlate"
            },
            {
                title: "Asignado",
                data: "userName"
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
            url: _app.parseUrl(`/equipos/parte-equipo-liviano-folding/listar`),
            data: function (d) {
                d.softPartId = softId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Dia",
                data: "partDate"
            },
            {
                title: "# Parte",
                data: "partNumber"
            },
            {
                title: "Kilometraje Inicial",
                data: "initMileage"
            },
            {
                title: "Kilometraje Final",
                data: "endMileage"
            },
            {
                title: "Operador",
                data:"equipmentMachineryOperator.operatorName"
            },
            {
                title: "Actividades",
                data: "activities"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" data-father="${row.equipmentMachinerySoftPart.equipmentMachineryTypeSoftId}" data-provider="${row.equipmentMachinerySoftPart.equipmentProvider.providerId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
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
                        softId = id;
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
                            text: "El Parte Diario será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/parte-equipo-liviano/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Parte Diario ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar Parte Diario"
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
                        let fatherid = $btn.data("father");
                        let providerid = $btn.data("provider")
                        $("#detail_modal").modal("hide");
                        forms.load.editfolding(id,fatherid,providerid);
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
                                        url: _app.parseUrl(`/equipos/parte-equipo-liviano-folding/eliminar/${id}`),
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
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_provider']").val(result.equipmentProviderId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTypeSoftId']").val(result.equipmentMachineryTypeSoftId);
                        formElements.find("[name='select_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");

                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.equipmentMachinerySoftId);
                        formElements.find("[name='select_eqsoft']").val(result.equipmentMachinerySoftId).trigger("change");

                        formElements.find("[name='UserId']").val(result.userId);
                        formElements.find("[name='select_user']").val(result.userId).trigger("change");

                       

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_provider']").val(result.equipmentProviderId).trigger("change");
                        formElements.find("[name='select_provider']").attr("disabled", "disabled");
                        formElements.find("[name='EquipmentMachineryTypeSoftId']").val(result.equipmentMachineryTypeSoftId);
                        formElements.find("[name='select_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='select_soft']").attr("disabled", "disabled");

                        formElements.find("[name='EquipmentMachinerySoftId']").val(result.equipmentMachinerySoftId);
                        formElements.find("[name='select_eqsoft']").val(result.equipmentMachinerySoftId).trigger("change");
                        formElements.find("[name='select_eqsoft']").attr("disabled", "disabled");


                        formElements.find("[name='UserId']").val(result.userId);
                        formElements.find("[name='select_user']").val(result.userId).trigger("change");
                        formElements.find("[name='select_user']").attr("disabled", "disabled");



                        

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentMachinerySoftPartId']").val(result.id);
                        select2.activities.reload(result.equipmentMachineryTypeSoftId, "")
                        select2.operators.reload(result.equipmentProvider.providerId,"")
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id,fatherid,providerid) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano-folding/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentMachinerySoftPartId']").val(result.equipmentMachinerySoftPartId);
                        formElements.find("[name='PartNumber']").val(result.partNumber);
                        formElements.find("[name='PartDate']").datepicker('setDate', result.partDate);
                        formElements.find("[name='InitMileage']").val(result.initMileage);
                        formElements.find("[name='EndMileage']").val(result.endMileage);

                        formElements.find("[name='EquipmentMachineryOperatorId']").val(result.equipmentMachineryOperatorId);
                        formElements.find("[name='select_operator']").val(result.equipmentMachineryOperatorId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTypeSoftActivities']").val(result.equipmentMachineryTypeSoftActivities);

                        select2.activities.reload(fatherid, result.equipmentMachineryTypeSoftActivities)
                        select2.operators.reload(providerid, result.equipmentMachineryOperatorId)
                        //formElements.find("[name='select_activities']").val(result.equipmentMachineryTypeSoftActivities);
                        //$(".select2-activities").trigger('change');

                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='EquipmentProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='EquipmentMachinerySoftId']").val($(formElement).find("[name='select_eqsoft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_soft']").val());
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/parte-equipo-liviano/crear"),
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
                $(formElement).find("[name='EquipmentProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='EquipmentMachinerySoftId']").val($(formElement).find("[name='select_eqsoft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_soft']").val());
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano/editar/${id}`),
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
                        url: "/equipos/parte-equipo-liviano/importar-datos",
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
                $(formElement).find("[name='EquipmentMachineryOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftActivities']").append($(formElement).find("[name='select_activities']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano-folding/crear`),
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
                $(formElement).find("[name='EquipmentMachineryOperatorId']").val($(formElement).find("[name='select_operator']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftActivities']").append($(formElement).find("[name='select_activities']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-liviano-folding/editar/${id}`),
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
                $("#Add_EquipmentMachineryTypeSoftActivities").prop("selectedIndex", 0).trigger("change");
            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
                $("#detail_modal").modal("show");
                $("#Edit_EquipmentMachineryTypeSoftActivities").prop("selectedIndex", 0).trigger("change");
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
            this.providers.init();
            this.users.init();
            this.softs.init();
      
            this.operators.init();
            this.eqsofts.init();
            this.activities.init();
        },
        activities: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/actividades-equipos-menores`)
                }).done(function (result) {
                    $(".select2-activities").select2({
                        data: result
                    });
                });
            },
            
            reload: function (id,actId) {
                $.ajax({
                    url: _app.parseUrl(`/select/actividad-menor-seleccionado-folding?tsId=${id}`)
                }).done(function (result) {
                    $(".select2-activities").empty();
                    $(".select2-activities").select2({
                        data: result
                    });
                    $(".select2-activities").val(actId).trigger("change");
                });
            },


        },
        eqsofts: {
            init: function () {
                $(".select2-eqsofts").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedor-seleccionado-eqsoft?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-eqsofts").empty();
                    $(".select2-eqsofts").select2({
                        data: result
                    });
                });
            },

        },
        operators: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/operadores-equipos")
                }).done(function (result) {
                    $(".select2-operators").select2({
                        data: result
                    });
                });
            },
            reload: function (id, opId) {
                $.ajax({
                    url: _app.parseUrl(`/select/operador-padre?oId=${id}`)
                }).done(function (result) {
                    $(".select2-operators").empty();
                    $(".select2-operators").select2({
                        data: result
                    });
                    $(".select2-operators").val(opId).trigger("change");
                });
            },

        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-liviano")
                }).done(function (result) {
                    $(".select2-providers").append(selectSGOption).trigger('change');
                    $(".select2-providers").select2({
                        data: result
                    });

                });
            }
        },
        users: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-users").select2({
                        data: result
                    });
                });
            }
        },
        softs: {
            init: function () {
                $(".select2-softs").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedor-seleccionado?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            },

        },
        guarantors: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/garantes")
                }).done(function (result) {
                    $(".select2-guarantors").select2({
                        data: result
                    });
                });
            }
        },
        budgetType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/titulos-de-presupuesto")
                }).done(function (result) {
                    $(".select2-budgetType").select2({
                        data: result
                    });
                });
            }
        },
        bondType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-fianza")
                }).done(function (result) {
                    $(".select2-bondType").select2({
                        data: result
                    });
                });
            }
        },
        banks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/bancos")
                }).done(function (result) {
                    $(".select2-banks").select2({
                        data: result
                    });
                });
            }
        },
        currencies: {
            init: function () {
                $(".select2-currencies").select2({
                    data: [{ "id": "PEN", "text": "SOLES" }, { "id": "USD", "text": "DOLARES" }]
                });
            }
        },
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
    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {

            console.log(2);
            $("#add_folding_form [name='EquipmentMachineryTypeSoftActivities']").attr("id", "Add_EquipmentMachineryTypeSoftActivities");
            $("#edit_folding_form [name='EquipmentMachineryTypeSoftActivities']").attr("id", "Edit_EquipmentMachineryTypeSoftActivities");
            $(".select2-providers").on("change", function () {
                select2.softs.reload(this.value);
                select2.eqsofts.reload(this.value);
            });

        }
    };

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