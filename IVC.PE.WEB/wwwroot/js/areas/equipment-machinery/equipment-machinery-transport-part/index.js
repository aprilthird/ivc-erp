var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var selectSGOption = new Option('--Seleccione un Proveedor--', null, true, true);
    var selectSGOption2 = new Option('--Seleccione un Transporte--', null, true, true);
    var for05Datatable = null;
    var foldingDatatable = null;
    var a = null;
    var importDataForm = null;

    var softId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/parte-equipo-transporte/listar"),
            data: function (d) {
                d.month = $("#month_filter").val();
                d.year = $("#year_filter").val();
                d.providerId = $("#provider_filter").val();
                
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Proveedor",
                data: "tradeName"
            },
            {
                title: "Equipo Transporte",
                data: "description"
            },
            {
                title: "Marca",
                data: "brand"
            },
            {
                title: "Modelo",
                data: "model"
            },
            {
                title: "Placa de Equipo Menor",
                data: "equipmentPlate"
            },
            {
                title: "Mes",
                data: "monthDesc"

            },
            {
                title: "Año",
                data: "year"
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
            url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/listar`),
            data: function (d) {
                d.softPartId = softId;
                d.month = $("#month_filter").val();
                d.year = $("#year_filter").val();
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
                title: "Asignado",
                data: "userName"
            },
            {
                title: "Área de Trabajo",
                data: "workArea",
                render: function (data) {
                    return _app.render.label(data, _app.constants.employee.workArea.VALUES);
                }
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
                data: "equipmentMachineryOperator.hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1)
                        tmp += row.equipmentMachineryOperator.operatorName;
                    else if (data == 3)
                        tmp += row.equipmentMachineryOperator.fromOtherName;
                    else if (data == 2)
                        tmp += row.equipmentMachineryOperator.worker.name + " " + row.equipmentMachineryOperator.worker.paternalSurname + " " + row.equipmentMachineryOperator.worker.maternalSurname;

                    return tmp;
                },
            },
            {
                title: "Actividades",
                data: "activities",
                render: function (data, type, row) {
                    if (data == "Domingo")
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">${data}</span>
								</label>
							</span>`;
                    else
                        return data;
                },
            },
            {
                title: "Fases",
                data: "transportPhase.projectPhase.code"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" data-father="${row.equipmentMachineryTransportPart.equipmentMachineryTypeTransportId}" data-provider="${row.equipmentMachineryTransportPart.equipmentProvider.providerId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
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
                                        url: _app.parseUrl(`/equipos/parte-equipo-transporte/eliminar/${id}`),
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
                                        url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/eliminar/${id}`),
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
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_provider']").val(result.equipmentProviderId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTypeTransportId']").val(result.equipmentMachineryTypeTransportId);
                        formElements.find("[name='select_soft']").val(result.equipmentMachineryTypeTransportId).trigger("change");

                        formElements.find("[name='Month']").val(result.month);
                        formElements.find("[name='select_month']").val(result.month).trigger("change");

                        formElements.find("[name='Year']").val(result.year);
                        formElements.find("[name='select_year']").val(result.year).trigger("change");
                        //formElements.find("[name='EquipmentMachineryTransportId']").val(result.equipmentMachineryTransportId);
                        //formElements.find("[name='select_eqsoft']").val(result.equipmentMachineryTransportId).trigger("change");

                        

                        select2.eqsofts.edit(result.equipmentProviderId, result.equipmentMachineryTransportId);
                        select2.softs.edit(result.equipmentProviderId, result.equipmentMachineryTypeTransportId);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_provider']").val(result.equipmentProviderId).trigger("change");
                        formElements.find("[name='select_provider']").attr("disabled", "disabled");

                        select2.softs.edit(result.equipmentProviderId, result.equipmentMachineryTypeTransportId);

                        formElements.find("[name='select_soft']").attr("disabled", "disabled");

                        select2.eqsofts.edit(result.equipmentProviderId, result.equipmentMachineryTransportId);
                        
                        formElements.find("[name='select_eqsoft']").attr("disabled", "disabled");


                        



                        

                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentMachineryTransportPartId']").val(result.id);
                        select2.activities.reload(result.equipmentMachineryTypeTransportId, "")
                        select2.operators.reload(result.equipmentMachineryTypeTransportId, "")
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id,fatherid,providerid) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentMachineryTransportPartId']").val(result.equipmentMachineryTransportPartId);
                        formElements.find("[name='PartNumber']").val(result.partNumber);
                        formElements.find("[name='PartDate']").datepicker('setDate', result.partDate);
                        formElements.find("[name='InitMileage']").val(result.initMileage);
                        formElements.find("[name='EndMileage']").val(result.endMileage);

                        formElements.find("[name='EquipmentMachineryOperatorId']").val(result.equipmentMachineryOperatorId);

                        formElements.find("[name='EquipmentMachineryTypeTransportActivities']").val(result.equipmentMachineryTypeTransportActivities);

                        formElements.find("[name='UserId']").val(result.userId);
                        formElements.find("[name='select_user']").val(result.userId).trigger("change");

                        formElements.find("[name='TransportPhaseId']").val(result.transportPhaseId);
                        formElements.find("[name='select_phase']").val(result.transportPhaseId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTypeTransportActivities']").val(result.equipmentMachineryTypeTransportActivities);
                        select2.activities.reload(fatherid, result.equipmentMachineryTypeTransportActivities)
                        select2.operators.reload(fatherid, result.equipmentMachineryOperatorId)
                        //formElements.find("[name='select_activities']").val(result.equipmentMachineryTypeTransportActivities);
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
                $(formElement).find("[name='EquipmentMachineryTransportId']").val($(formElement).find("[name='select_eqsoft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_soft']").val());
                $(formElement).find("[name='Month']").val($(formElement).find("[name='select_month']").val());
                $(formElement).find("[name='Year']").val($(formElement).find("[name='select_year']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/parte-equipo-transporte/crear"),
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
                $(formElement).find("[name='EquipmentMachineryTransportId']").val($(formElement).find("[name='select_eqsoft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_soft']").val());
                $(formElement).find("[name='Month']").val($(formElement).find("[name='select_month']").val());
                $(formElement).find("[name='Year']").val($(formElement).find("[name='select_year']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte/editar/${id}`),
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
                        url: "/equipos/parte-equipo-transporte/importar-datos",
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
                $(formElement).find("[name='TransportPhaseId']").val($(formElement).find("[name='select_phase']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportActivities']").append($(formElement).find("[name='select_activities']").val());
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/crear`),
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
                $(formElement).find("[name='EquipmentMachineryTypeTransportActivities']").append($(formElement).find("[name='select_activities']").val());
                $(formElement).find("[name='UserId']").val($(formElement).find("[name='select_user']").val());
                $(formElement).find("[name='TransportPhaseId']").val($(formElement).find("[name='select_phase']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/editar/${id}`),
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
                $("#Add_EquipmentMachineryTypeTransportActivities").prop("selectedIndex", 0).trigger("change");
            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
                $("#detail_modal").modal("show");
                $("#Edit_EquipmentMachineryTypeTransportActivities").prop("selectedIndex", 0).trigger("change");
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
            this.providers2.init();
            this.years.init();
            this.providers.init();
            this.users.init();
            this.softs.init();
            this.phases.init();
            this.operators.init();
            this.eqsofts.init();
            this.activities.init();
        },

        years: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/años-equipos")
                }).done(function (result) {

                    $(".select2-years").select2({
                        data: result
                    });

                });
            }
        },
        providers2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-transporte")
                }).done(function (result) {

                    $(".select2-equipproviders2").select2({
                        data: result
                    });

                });
            }
        },
        phases: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-transporte")
                }).done(function (result) {
                    $(".select2-phases").select2({
                        data: result
                    });

                });
            }
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
                    url: _app.parseUrl(`/select/actividad-transporte-seleccionado-folding?tsId=${id}`)
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
                    url: _app.parseUrl(`/select/proveedor-seleccionado-eqtransport?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-eqsofts").empty();
                    $(".select2-eqsofts").select2({
                        data: result
                    });
                });
            },
            reload2: function (id,tid) {
                let sg = id;
                let tsg = tid;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedor-seleccionado-eqtransport2?equipmentProviderId=${sg}&equipmentTypeId=${tsg}`)
                }).done(function (result) {
                    $(".select2-eqsofts").empty();
                    $(".select2-eqsofts").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid,eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedor-seleccionado-eqtransport?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-eqsofts").empty();
                    $(".select2-eqsofts").select2({
                        data: result
                    });
                    $(".select2-eqsofts").val(eqsid).trigger('change');
                });
            },

        },
        operators: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/actividades-equipos-menores`)
                }).done(function (result) {
                    $(".select2-activities").select2({
                        data: result
                    });
                });
            },

            reload: function (id, actId) {
                $.ajax({
                    url: _app.parseUrl(`/select/operador-transporte-seleccionado?opId=${id}`)
                }).done(function (result) {
                    $(".select2-operators").empty();
                    $(".select2-operators").select2({
                        data: result
                    });
                    $(".select2-operators").val(actId).trigger("change");
                });
            },


        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-transporte")
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
                    url: _app.parseUrl("/select/empleados-equipos")
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
                    url: _app.parseUrl(`/select/proveedor-seleccionado-transporte?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").append(selectSGOption2).trigger('change');
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedor-seleccionado-transporte?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-softs").empty();
                    $(".select2-softs").append(selectSGOption2).trigger('change');
                    $(".select2-softs").select2({
                        data: result
                    });
                    $(".select2-softs").val(eqsid).trigger('change');
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

            $(".search-select").on("click", function () {
                let formId = $(this).closest(`.form`).attr(`id`)
                let id = $(`#${formId} .fatherid`).val();
                console.log(formId);
                console.log(id);
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-equipo-transporte-folding/domingo/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {


                        let formElements = $(`#${formId}`);

                        
                        //.val(
                        formElements.find("[name='InitMileage']").val(parseFloat(Math.round(result.endMileage * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='EndMileage']").val(parseFloat(Math.round(result.endMileage * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='EquipmentMachineryOperatorId']").val(result.equipmentMachineryOperatorId);
                        //select2.operators.reload(result.equipmentMachineryTransportPart.equipmentMachineryTypeTransportId, result.equipmentMachineryOperatorId)
                        
                        formElements.find("[name='select_phase']").val(result.transportPhaseId).trigger("change");

                        formElements.find("[name='UserId']").val(result.userId);
                        formElements.find("[name='select_user']").val(result.userId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTypeTransportActivities']").val(result.equipmentMachineryTypeTransportActivities);
                        select2.activities.reload(result.equipmentMachineryTransportPart.equipmentMachineryTypeTransportId, result.equipmentMachineryTypeTransportActivities)

                        console.log(result.equipmentMachineryTransportPart);
                        console.log(result.equipmentMachineryTypeTransportActivities);
                        //formElements.find("[name='EquipmentMachineryOperatorId']").val(result.equipmentMachineryOperatorId);
                        //select2.operators.reload(result.equipmentMachineryTransportPart.equipmentMachineryTypeTransportId, result.equipmentMachineryOperatorId)


                        //formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").attr("disabled", "disabled");
                    })
            });


            console.log(2);
            $("#add_folding_form [name='EquipmentMachineryTypeTransportActivities']").attr("id", "Add_EquipmentMachineryTypeTransportActivities");
            $("#edit_folding_form [name='EquipmentMachineryTypeTransportActivities']").attr("id", "Edit_EquipmentMachineryTypeTransportActivities");

            $(".select2-providers").on("change", function () {
                select2.softs.reload(this.value);
                select2.eqsofts.reload(this.value);
                a = this.value;
            });

            $(".select2-softs").on("change", function () {
                select2.eqsofts.reload2(a,this.value);
            });



            $("#month_filter,#year_filter,#provider_filter").on("change", function () {
                datatables.foldingDt.reload();
                datatables.for05Dt.reload();
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