var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var detailForm2 = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var selectSGOption = new Option('--Seleccione un Proveedor--', null, true, true);
    var selectWOption = new Option('Seleccione una Semana', null, true, true);
    var for05Datatable = null;
    var foldingDatatable = null;
    var rateDatatable = null;
    var fId = null;

    var importDataForm = null;

    var softId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/parte-combustible-transporte/listar"),
            data: function (d) {
             d.month = $("#month_filter").val();
             d.year = $("#year_filter").val();
             d.week = $("#week_filter").val();

                d.equipmentProviderId = $("#provider_filter").val();
                d.transportId = $("#transport_filter").val();
                d.equipmentMachineryTransportPartId = $("#part_filter").val();
             delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Año de Parte Diario",
                data: "yearPart"
            },
            {
                title: "Mes de Parte Diario",
                data: "monthPart"
            },
            {
                title: "Equipo Transporte",
                data: "description"
            },
            {
                title: "Nombre del Equipo",
                data: "brand",
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += row.brand + "-" + row.model + "-" + row.plate;
                   
                    return tmp;
                },
            },

            {
                title: "Proveedor",
                data: "tradeName"
            },
            {
                title: "Año",
                data: "year"
            },
            {
                title: "Codigo/Placa",
                data: "plate"
            },
            {
                title: "Condicion de servicio",
                data: "serviceConditionDesc"
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
                title: "Kilometraje Acumulado (km)",
                data: "acumulatedMileage"
            },
            {
                title: "Consumo Acumulado (gln)",
                data:"acumulatedGallon"
            },
      //      {
      //          title: "Ratio de Consumo (gln/km)",
      //          data: "rateConsume",         
      //          render: function (data, type, row) {
      //              if (data > 0) {
      //                  return `
						//<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						//`;
      //              } else if (data == 0) {
      //                  return ` <span> 0 </span> `
      //              }
      //              else if (data == null) {
      //                  return ` <span> Sin Registros en su Folding de Equipos Transporte </span> `
      //              }
      //          }
      //      },

      //      {
      //          title: "Ratio de Consumo (km/gln)",
      //          data: "rateConsumeInv",
      //          render: function (data, type, row) {
      //              if (data > 0) {
      //                  return `
						//<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						//`;
      //              } else if (data == 0) {
      //                  return ` <span> 0 </span> `
      //              }
      //              else if (data == null) {
      //                  return ` <span> Sin Registros en su Folding de Equipos Transporte </span> `
      //              }
      //          }
      //      },
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
                title: "Ratio",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-rates">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
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
            url: _app.parseUrl(`/equipos/parte-combustible-transporte-folding/listar`),
            data: function (d) {
                d.softPartId = softId;
                d.month = $("#month_filter").val();
                d.year = $("#year_filter").val();
                d.week = $("#week_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "#Parte",
                data: "partNumber"
            },
            {
                title: "Fecha",
                data: "partDateString"
            },
            {
                title: "Operador",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1)
                        tmp += row.operatorName;
                    else if (data == 3)
                        tmp += row.fromOtherName;
                    else if (data == 2)
                        tmp += row.workerName + " " + row.workerMiddleName + " " + row.workerPaternalSurname + " " + row.workerMaternalSurname;

                    return tmp;
                },
            },
            {
                title: "Placa de Cisterna",
                data: "cisternPlate"
            },
            
            {
                title: "Precio (S/ x gln)",
                data: "price"
            },
            {
                title: "Hora de Carga",
                data: "partHour"
            },
            {
                title: "Consumo (gln)",
                data: "gallon"
            },
            {
                title: "Kilometraje de Carga",
                data: "mileage"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" data-father="${row.equipmentMachineryTransportPartId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]

    };

    var rateDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/equipos/parte-combustible-transporte-ratio/listar`),
            data: function (d) {
                d.fuelId =fId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Semana",
                data: "week"
            },
            
            {
                title: "Ratio (gln/km)",
                data: "rate",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 3)) / Math.pow(10, 3)).toFixed(3)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                    else if (data == null) {
                        return ` <span> Sin Registros en su Folding de Equipos Transporte </span> `
                    }
                }
            },

            {
                title: "Ratio (km/gln)",
                data: "invRate",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                    else if (data == null) {
                        return ` <span> Sin Registros en su Folding de Equipos Transporte </span> `
                    }
                }
            },

            
        ]

    };
    var datatables = {
        init: function () {
            this.for05Dt.init();
            this.foldingDt.init();
            this.rateDt.init();
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
                    ".btn-rates",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        fId = id;
                        datatables.rateDt.reload();
                        forms.load.detail2(id);
                    });

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
                                        url: _app.parseUrl(`/equipos/parte-combustible-transporte/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Parte Combustible ha sido eliminada con éxito",
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
                        let fatherid = $btn.data("father")
                        let providerid = $btn.data("provider")
                        $("#detail_modal").modal("hide");
                        forms.load.editfolding(id, fatherid, providerid);
                        console.log(fatherid)
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
                                        url: _app.parseUrl(`/equipos/parte-combustible-transporte-folding/eliminar/${id}`),
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
        },
        rateDt: {
            init: function () {
                rateDatatable = $("#rate_datatable").DataTable(rateDtOpt);
                this.events();
            },
            reload: function () {
                rateDatatable.ajax.reload();

            },
            events: function () {
             

                
            }
        }
    };
    var forms = {
        load: {
            edit: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='EquipmentProviderId']").val(result.equipmentProviderId);
                        formElements.find("[name='select_provider']").val(result.equipmentProviderId).trigger("change");

                        formElements.find("[name='EquipmentMachineryTransportPartId']").val(result.equipmentMachineryTransportPartId);
                        formElements.find("[name='select_eqtransport']").val(result.equipmentMachineryTransportPartId).trigger("change");

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
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form");

                        formElements.find("[name='Id']").val(result.id);


                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            detail2: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log("aqui");
                        let formElements = $("#detail_form2");

                        formElements.find("[name='Id']").val(result.id);


                        $("#detail_modal2").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor05: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='EquipmentMachineryFuelTransportPartId']").val(result.id);
                        select2.operators.reload(result.equipmentMachineryTransportPartId, "")
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id, fatherid,providerid) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte-folding/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        console.log(5);
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='EquipmentMachineryFuelTransportPartId']").val(result.equipmentMachineryFuelTransportPartId);
                        formElements.find("[name='PartNumber']").val(result.partNumber);
                        formElements.find("[name='PartDate']").datepicker('setDate', result.partDate);

                        formElements.find("[name='InitMileage']").val(result.initMileage);
                        formElements.find("[name='EndMileage']").val(result.endMileage);

                        formElements.find("[name='FuelProviderId']").val(result.fuelProviderId);
                        formElements.find("[name='select_fuel']").val(result.fuelProviderId).trigger("change");

                        //formElements.find("[name='FuelProviderFoldingId']").val(result.fuelProviderFoldingId);
                        //formElements.find("[name='select_plate']").val(result.fuelProviderFoldingId).trigger("change");

                        select2.plates.edit(result.fuelProviderId, result.fuelProviderFoldingId);


                        //formElements.find("[name='FuelProviderPriceFoldingId']").val(result.fuelProviderPriceFoldingId);
                        //formElements.find("[name='select_price']").val(result.fuelProviderPriceFoldingId).trigger("change");

                        select2.prices.edit(result.fuelProviderId, result.fuelProviderPriceFoldingId);

                        formElements.find("[name='EquipmentMachineryOperatorId']").val(result.equipmentMachineryOperatorId);

                        select2.operators.reload(fatherid, result.equipmentMachineryOperatorId)
                        formElements.find("[name='PartHour']").val(result.partHour);
                        formElements.find("[name='Gallon']").val(result.gallon);
                        formElements.find("[name='Mileage']").val(result.mileage);

                        console.log(fatherid);

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
                $(formElement).find("[name='EquipmentMachineryTransportPartId']").val($(formElement).find("[name='select_eqtransport']").val());
                
                
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/parte-combustible-transporte/crear"),
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
                $(formElement).find("[name='EquipmentMachineryTransportPartId']").val($(formElement).find("[name='select_eqtransport']").val());
                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte/editar/${id}`),
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
                        url: "/equipos/parte-combustible-transporte/importar-datos",
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
                $(formElement).find("[name='FuelProviderId']").val($(formElement).find("[name='select_fuel']").val());
                $(formElement).find("[name='FuelProviderFoldingId']").val($(formElement).find("[name='select_plate']").val());
                $(formElement).find("[name='FuelProviderPriceFoldingId']").val($(formElement).find("[name='select_price']").val());

                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte-folding/crear`),
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
                $(formElement).find("[name='FuelProviderId']").val($(formElement).find("[name='select_fuel']").val());
                $(formElement).find("[name='FuelProviderFoldingId']").val($(formElement).find("[name='select_plate']").val());
                $(formElement).find("[name='FuelProviderPriceFoldingId']").val($(formElement).find("[name='select_price']").val());


                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/equipos/parte-combustible-transporte-folding/editar/${id}`),
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

            detailForm2 = $("#detail_modal2").validate({
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
            this.parts.init();
            this.years.init();
            this.transports.init();
            this.providers.init();
            this.providers2.init();
            this.softs.init();
            this.operators.init();
            this.fuels.init();
            this.plates.init();
            this.prices.init();
            this.eqsofts.init();
            this.activities.init();
            this.eqtransports.init();
            this.weeks.init()
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
        weeks: {
            init: function () {

                $.ajax({
                    url: _app.parseUrl(`/select/semanas-equipos`),
                    data: {
                        year: $("#year_filter").val(),

                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-weeks").select2({
                        data: result
                    });
                })
            },
            reload: function () {

                $.ajax({
                    url: _app.parseUrl(`/select/semanas-equipos`),
                    data: {
                        year: $("#year_filter").val(),
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-weeks").append(selectWOption).trigger('change');
                    $(".select2-weeks").select2({
                        data: result
                    });
                })

                $(".select2-weeks").empty();
                this.init();
            }
        },
        parts: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/parte-transporte-filtro")
                }).done(function (result) {
                    $(".select2-parts").select2({
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

        eqtransports: {
            init: function () {
                $(".select2-eqtransports").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/parte-transporte-seleccionado?equipmentProviderId=${sg}`)
                }).done(function (result) {
                    $(".select2-eqtransports").empty();
                    $(".select2-eqtransports").select2({
                        data: result
                    });
                });
            },

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
                    url: _app.parseUrl(`/select/operadores-equipos`)
                }).done(function (result) {
                    $(".select2-operators").select2({
                        data: result
                    });
                });
            },
            reload: function (id, actId) {
                $.ajax({
                    url: _app.parseUrl(`/select/operador-transporte-parte-seleccionado?opId=${id}`)
                }).done(function (result) {
                    $(".select2-operators").empty();
                    $(".select2-operators").select2({
                        data: result
                    });
                    $(".select2-operators").val(actId).trigger("change");
                });
            },
        },

        fuels: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/proveedores-de-combustible`)
                }).done(function (result) {
                    $(".select2-fuels").append(selectSGOption).trigger('change');
                    $(".select2-fuels").select2({
                        data: result
                    });
                });
            },
        },

        plates: {
            init: function () {
                $(".select2-plates").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedores-de-combustible-placas?providerId=${sg}`)
                }).done(function (result) {
                    $(".select2-plates").empty();
                    $(".select2-plates").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedores-de-combustible-placas?providerId=${sg}`)
                }).done(function (result) {
                    $(".select2-plates").empty();
                    $(".select2-plates").select2({
                        data: result
                    });
                    $(".select2-plates").val(eqsid).trigger('change');
                });
            },
        },
        prices: {
            init: function () {
                $(".select2-prices").select2();
            },
            reload: function (id) {
                let sg = id;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedores-de-combustible-precios?providerId=${sg}`)
                }).done(function (result) {
                    $(".select2-prices").empty();
                    $(".select2-prices").select2({
                        data: result
                    });
                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/proveedores-de-combustible-precios?providerId=${sg}`)
                }).done(function (result) {
                    $(".select2-prices").empty();
                    $(".select2-prices").select2({
                        data: result
                    });
                    $(".select2-prices").val(eqsid).trigger('change');
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

        providers2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proveedores-de-equipos-transporte")
                }).done(function (result) {
                    $(".select2-providers2").select2({
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
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {

            $(".select2-providers").on("change", function () {
                select2.eqtransports.reload(this.value);
            });

            $(".select2-fuels").on("change", function () {
                select2.plates.reload(this.value);
                select2.prices.reload(this.value)

            });

            $("#month_filter,#year_filter,#week_filter,#provider_filter,#transport_filter,#part_filter").on("change", function () {
                datatables.foldingDt.reload();
                datatables.for05Dt.reload();
            });
            $("#year_filter").on("change", function () {
                select2.weeks.reload();
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