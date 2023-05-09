var Operator = function () {

    var mainDataTable = null;
    var addForm = null;
    var editForm = null;
    var selectSGOption = new Option('--Seleccione un Tipo de Equipo--', null, true, true);
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/operadores/listar"),

            data: function (d) {

                d.equipmentTypeId = $("#equipment_filter").val();
                d.hiringType = $("#hiring_filter").val();
                d.machineryId = $("#type_filter").val();
                delete d.columns;

            },

            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre del Operador",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    
                        if (data == 1)
                            tmp += row.operatorName;
                        else if (data == 2)
                            tmp += row.worker.name + " " + row.worker.paternalSurname + " " + row.worker.maternalSurname;
 
                    else if (data == 3)
                        tmp += row.fromOtherName;
                    return tmp;
                }
            },
            {
                title: "Número de celular",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1)
                        tmp += row.phoneOperator;
                    else if (data == 2)
                        tmp += row.worker.phoneNumber;

                    else if (data == 3)
                        tmp += row.fromOtherPhone;
                    return tmp;
                }
            },
            {
                title: "DNI",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 1)
                        tmp += row.dniOperator;
                    else if (data == 2)
                        tmp += row.worker.document;

                    else if (data == 3)
                        tmp += row.fromOtherDNI;
                    return tmp;
                }
            },
            {
                title: "Clase de Equipos",
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
                    else
                        tmp += row.equipmentMachineryTypeTransport.description;
                    return tmp;
                }
            },
            {
                title: "Tipo de Contratación",
                data: "hiringTypeDesc"
            },
            {
                title: "Fecha de Inicio",
                data: "startDate"
            },

            {
                title: "Fecha de Fín",
                data: "hiringType",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data == 2) {
                        if (row.worker.isActive == 0)
                            tmp += row.worker.ceaseDateStr;
                }
                    return tmp;
                }
            },


            {
                title: "Observaciones",
                data: "hiringType",
                render: function (data, type, row) {

                    if (data == 2) {
                        if (row.worker.isActive == 0) {
                           
                           return     `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">Cesado</span>
								</label>
							</span>`;
                        }
                        else {
                  
                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">Ninguna</span>
								</label>
							</span>`;

                        }
                    }
                    else {
              
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">Ninguna</span>
								</label>
							</span>`;
                    }

                }
            },
            {
                title: "Qr",
                data: "id",
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.actualDni) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-qr">`;
                        tmp += `<i class="fa fa-qrcode"></i></button> `;
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
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            mainDataTable = $("#main_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            mainDataTable.ajax.reload();
        },
        initEvents: function () {
            mainDataTable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            mainDataTable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });

            mainDataTable.on("click",
                ".btn-qr",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    window.location = `/equipos/operadores/qr/${id}`;

                });
            mainDataTable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El operador será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/equipos/operadores/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo técnico ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el archivo técnico"
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
                    url: _app.parseUrl(`/equipos/operadores/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='IsFrom']").val(result.isFrom);
                        formElements.find("[name='select_isFrom']").val(result.isFrom).trigger("change");
                        formElements.find("[name='HiringType']").val(result.hiringType);
                        formElements.find("[name='select_hiring']").val(result.hiringType).trigger("change");
                        formElements.find("[name='WorkerId']").val(result.workerId);
                        formElements.find("[name='select_worker']").val(result.workerId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeSoftId']").val(result.equipmentMachineryTypeSoftId);
                        formElements.find("[name='select_machinery_type_soft']").val(result.equipmentMachineryTypeSoftId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeTransportId']").val(result.equipmentMachineryTypeTransportId);
                        formElements.find("[name='select_machinery_type_transport']").val(result.equipmentMachineryTypeTransportId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeTypeId']").val(result.equipmentMachineryTypeTypeId);
                        formElements.find("[name='select_machinery_type_type']").val(result.equipmentMachineryTypeTypeId).trigger("change");
                        formElements.find("[name='EquipmentMachineryTypeId']").val(result.equipmentMachineryTypeId);
                        formElements.find("[name='select_machinery']").val(result.equipmentMachineryTypeId).trigger("change");
                        formElements.find("[name='OperatorName']").val(result.operatorName);
                        formElements.find("[name='PhoneOperator']").val(result.phoneOperator);
                        formElements.find("[name='DNIOperator']").val(result.dniOperator);
                        formElements.find("[name='FromOtherName']").val(result.fromOtherName);
                        formElements.find("[name='FromOtherPhone']").val(result.fromOtherPhone);
                        formElements.find("[name='FromOtherDNI']").val(result.fromOtherDNI);
                        formElements.find("[name='StartDate']").datepicker("setDate", result.startDate);
                        formElements.find("[name='EndDate']").datepicker("setDate", result.endDate);

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
                    url: _app.parseUrl(`/equipos/operadores/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.title);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.title}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.title).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='IsFrom']").val($(formElement).find("[name='select_isfrom']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='HiringType']").val($(formElement).find("[name='select_hiring']").val());
                $(formElement).find("[name='WorkerId']").val($(formElement).find("[name='select_worker']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_machinery_type_transport']").val());
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
                    url: _app.parseUrl("/equipos/operadores/crear"),
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
                
                $(formElement).find("[name='ProviderId']").val($(formElement).find("[name='select_provider']").val());
                $(formElement).find("[name='IsFrom']").val($(formElement).find("[name='select_isfrom']").val());
                $(formElement).find("[name='EquipmentMachineryTypeId']").val($(formElement).find("[name='select_machinery']").val());
                $(formElement).find("[name='HiringType']").val($(formElement).find("[name='select_hiring']").val());
                $(formElement).find("[name='WorkerId']").val($(formElement).find("[name='select_worker']").val());
                $(formElement).find("[name='EquipmentMachineryTypeSoftId']").val($(formElement).find("[name='select_machinery_type_soft']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTypeId']").val($(formElement).find("[name='select_machinery_type_type']").val());
                $(formElement).find("[name='EquipmentMachineryTypeTransportId']").val($(formElement).find("[name='select_machinery_type_transport']").val());
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
                    url: _app.parseUrl(`/equipos/operadores/editar/${id}`),
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
 

            this.workers.init();
            this.softs.init();
            this.machineries.init();
            this.machinerys.init();
            this.machinerys2.init();
            this.providers.init();
            this.transports.init();
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
                    url: _app.parseUrl(`/select/tipos-de-maquinaria-mixto-por-clase?equipmentMachineryTypeId=${id}`)
                }).done(function (result) {

                    $(".select2-types").empty();
                    $(".select2-types").append(selectSGOption).trigger('change'); 
                    $(".select2-types").select2({
                        data: result
                    });
                });
            },


        },

        //types: function () {
        //    $.ajax({
        //        url: _app.parseUrl("/select/tipos-de-maquinaria-mixto-por-clase"),
        //        data: {
        //            equipmentMachineryTypeId: $("#equipment_filter").val(),
        //        },
        //        dataSrc: ""
        //    })
        //        .done(function (result) {

        //            $(".select2-types").empty();
        //            $(".select2-types").select2({
        //                data: result
        //            });

        //            //$(".select2-types").select2({
        //            //    data: result
        //            //});
        //        });
        //},


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

        workers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/obreros-operadores")
                }).done(function (result) {
                    $(".select2-workers").select2({
                        data: result
                    });
                });
            }
        },
 
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

    var events = {
        init: function () {

            $(".select2-hirings").on("change", function () {
                var txt = $(".select2-hirings option:selected").text();
                if (txt.indexOf("Empleado") >= 0) {
                    $(".is-from-ivc").attr("hidden", false);
                    $(".is-from-another").attr("hidden", true);
                    $(".is-from-worker").attr("hidden", true);
                } else if (txt.indexOf("Empleado") <= 0){
                    
                    $(".is-from-ivc").attr("hidden", true);
                }
               if (txt.indexOf("Obrero") >= 0) {

                    $(".is-from-worker").attr("hidden", false);
                    $(".is-from-ivc").attr("hidden", true);
                    $(".is-from-another").attr("hidden", true);
                } else if (txt.indexOf("Obrero") <= 0) {

                    $(".is-from-worker").attr("hidden", true);
                }

                if (txt.indexOf("Otro") >= 0) {

                    $(".is-from-another").attr("hidden", false);
                    $(".is-from-ivc").attr("hidden", true);
                    $(".is-from-worker").attr("hidden", true);
                } else if (txt.indexOf("Obrero") <= 0) {

                    $(".is-from-another").attr("hidden", true);
                }

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


            $(".select2-machinerys2").on("change", function () {
                select2.types.reload(this.value);

            });


            $("#hiring_filter, #equipment_filter , #type_filter").on("change", function () {
                datatable.reload();
            });

        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            datepicker.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Operator.init();
});