var Request = function () {

    var mainDatatable = null;
    var itemsDatatable = null;
    var addForm = null;
    var editForm = null;
    var editItemForm = null;
    var projectFormulaId = null;
    var requestId = null;

    var options = {
        responsive: true,
        processing: true,
        initComplete: function () {
            select2.status.init();
        },
        ajax: {
            url: _app.parseUrl("/logistica/pre-requerimientos/listado/listar"),
            dataSrc: "",
            data: function (d) {
                d.status = $("#status_filter").val();
                d.attentionStatus = $("#attention_status_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "project.costCenter"
            },
            {
                title: "Proyecto",
                data: "project.abbreviation"
            },
            {
                title: "Código",
                data: "correlativeCodeStr"
            },
            {
                title: "Estado",
                data: "orderStatus",
                render: function (data, type, row) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">PRE-EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">EMITIDO</span>
								</label>
							</span>`;
                    } else if(data == 7) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">PENDIENTE DE RESPUESTA</span>
								</label>
							</span>`;
                    } else if (data == 8) {

                        if (row.attentionStatus == 1) {

                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO</span>
								</label>
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">PENDIENTE ATENCIÓN</span>
								</label>
							</span>`;
                        } else if (row.attentionStatus == 2) {

                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO</span>
								</label>
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">PARCIALMENTE ATENDIDO</span>
								</label>
							</span>`;
                        } else if (row.attentionStatus == 3) {

                            return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO</span>
								</label>
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">ATENDIDO</span>
								</label>
							</span>`;
                        }
                    } else if (data == 9) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADO PARCIALMENTE</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Solicita",
                data: "requestUsernames"
            },
            {
                title: "Presupuesto",
                data: "budgetTitle.name"
            },
            {
                title: "Tipo de Solicitud",
                data: "requestTypeStr"
            },
            {
                title: "Fecha de Emisión",
                data: "issueDate",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Fecha de Entrega",
                data: "deliveryDate",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Fecha de Aprobación",
                data: "approveDate",
                render: function (data, type, row) {
                    if (row.orderStatus == 7)
                        return "---";
                    else
                        return data || "---";
                }
            },
            {
                title: "Carpeta",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-items" title="Ver Items">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-excel" title="Descargar Excel">`;
                    tmp += `<i class="la la-file-excel-o"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-files" title="Ver Archivos">`;
                    tmp += `<i class="fa fa-paperclip"></i></button> `;
                    if (row.orderStatus != 8) {
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-degrade" title="Degradar">`;
                        tmp += `<i class="la la-arrow-left"></i></button> `;
                    }
                    if (row.orderStatus == 7 || row.orderStatus == 9) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-send" title="Enviar recordatorio">`;
                        tmp += `<i class="la la-send"></i></button> `;
                    }
                    return tmp;
                }
            },
            /*
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.orderStatus == 2) {
                        tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-approved">`;
                        tmp += `<i class="fas fa-arrow-right"></i></button> `;
                        tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-cancel">`;
                        tmp += `<i class="la la-ban"></i></button>`;
                    }
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
             */
        ]
    };

    var itemOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/pre-requerimientos/detalles/listar"),
            data: function (d) {
                d.reqId = requestId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Grupo",
                data: function (result) {
                    var group = result.supplyGroupStr;

                    if (group == "")
                        return `<b style="color:rgb(255, 25, 25);" >Ingreso Manual</b>`;
                    else
                        return group;
                }
            },
            {
                title: "Material",
                data: function (result) {
                    var supply = result.supply.description;

                    if (supply == "")
                        return result.supplyName
                    else
                        return supply;
                }
            },
            {
                title: "Unidad",
                data: function (result) {
                    var unit = result.supply.measurementUnit.abbreviation;

                    if (unit == "")
                        return result.measurementUnitName;
                    else
                        return unit;
                }
            },
            {
                title: "Metrado",
                data: "measure"
            },
            /*
            {
                title: "Techo Max",
                data: "goalBudgetInput.metered"
            },
            {
                title: "Precio aprox.:",
                data: "goalBudgetInput.unitPrice"
            },
            */
            {
                title: "Para se usado en:",
                data: "workFront.code"
            },
            {
                title: "Observaciones",
                data: "observations"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit" title="Editar">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ],
        buttons: []
    };

    var datatables = {
        init: function () {
            this.mainDt.init();
            this.itemDt.init();
        },
        mainDt: {
            init: function () {
                mainDatatable = $("#main_datatable").DataTable(options);
                this.initEvents();
            },
            reload: function () {
                mainDatatable.ajax.reload();
            },
            initEvents: function () {
               /* mainDatatable.on("change", ".select2-status", function () {
                    let id = $(this).data("id");
                    let value = $(this).val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/${id}/actualizar-estado`),
                        method: "post",
                        data: {
                            status: value
                        }
                    }).done(function () {
                        this.reload();
                        _app.show.notification.edit.success();
                    }).fail(function () {
                        _app.show.notification.edit.error();
                    });
                });*/

                mainDatatable.on("click",
                    ".btn-items",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.items.detail(id);
                    });

                mainDatatable.on("click",
                    ".btn-files",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.file(id);
                    });

                mainDatatable.on("click",
                    ".btn-excel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        window.location = `/logistica/pre-requerimientos/excel/${id}`;
                    });

                mainDatatable.on("click",
                    ".btn-files",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        form.load.file(id);
                    });

                mainDatatable.on("click",
                    ".btn-send",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "Se enviará recordatorio a los autorizantes que no hayan respondido aún",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, enviar recordatorio",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/enviar-recordatorio/${id}`),
                                        type: "get",
                                        success: function (result) {
                                            datatables.itemDt.reload();
                                            datatables.mainDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "Se ha enviado el recordatorio con éxito",
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
                                                text: "Ocurrió un error al enviar el recordatorio"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
                    ".btn-degrade",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        requestId = id;
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El siguiente Pre-Requerimiento pasará a estado Pre-Emitido",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, degradar",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/degradar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Pre-Requerimiento ha sido degradado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                            datatables.mainDt.reload();
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar degradar el Pre-Requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
                    ".btn-approved",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Pre-equerimiento será aprobado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, aprobarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/aprobar/${id}`),
                                        type: "get",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido aprobado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                            datatables.mainDt.reload();
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar aprobar el requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
                    ".btn-cancel",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Requerimiento será cancelado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, cancelarlo",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/logistica/pre-requerimientos/cancelar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Requerimiento ha sido cancelado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                            datatables.mainDt.reload();
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar cancelar el requerimiento"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        itemDt: {
            init: function () {
                itemsDatatable = $("#items_datatable").DataTable(itemOpts);
                this.initEvents();
            },
            reload: function () {
                itemsDatatable.ajax.reload();
            },
            initEvents: function () {
                itemsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.items.edit(id);
                    });
            }
        }
    };

    var form = {
        load: {
            file: function (id) {
                select2.files.reload(id);
                $("#file_modal").modal("show");
            },
            items: {
                detail: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/select/pre-requerimientos/thecnical-spec/${id}`)
                    }).done(function (result) {
                        tecDiv = document.querySelector("#thecnicalSpec");

                        tecDiv.innerHTML = "";
                        result.forEach(function (item) {
                            tecDiv.innerHTML += item.text + "<br/>";
                        })
                    }).always(function () {
                        _app.loader.hide();
                    });
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/${id}`)
                    })
                        .done(function (result) {
                            $("#type").text(result.requestTypeStr);
                            $("#code").text(result.correlativeCodeStr);
                            $("#user").text(result.issuedUserName);
                            projectFormulaId = result.projectFormulaId;
                            select2.workfronts();
                        })
                    //select2.files.existing(id);

                    datatables.itemDt.reload();
                    $("#detail_modal").modal("show");
                },
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#edit_item_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='PreRequestId']").val(result.preRequestId);
                            formElements.find("[name='WorkFrontId']").val(result.workFrontId).trigger("change");
                            formElements.find("[name='Measure']").val(result.measure);
                            formElements.find("[name='Observations']").val(result.observations);
                            formElements.find("[name='UsedFor']").val(result.usedFor);
                            formElements.find("[name='SupplyManual']").val(result.supplyManual);

                            if (result.supplyManual == false) {
                                $.ajax({
                                    url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                                }).done(function (result) {
                                    formElements.find("[name='MeasurementUnitName']").val(result);
                                });

                                setTimeout(() => {
                                    formElements.find("[name='SupplyId']").val(result.supplyId).trigger("change");
                                }, 1000);

                            } else {
                                $("#Edit_SupplyManual").prop('checked', true);
                                $(".insumo").hide();
                                $(".manual").show();
                                formElements.find("[name='MeasurementUnitName']").val(result.measurementUnitName);
                                formElements.find("[name='SupplyName']").val(result.supplyName);
                            }

                            $("#detail_modal").modal("hide");
                            setTimeout(() => {
                                $("#edit_item_modal").modal("show");
                            }, 1000);
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            items: {
                edit: function (formElement) {
                    let data = new FormData($(formElement).get(0));
                    data.set('SupplyManual', $("#Edit_SupplyManual").val());
                    console.log(data);
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/detalles/editar/${id}`),
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
                            datatables.itemDt.reload();
                            _app.show.notification.add.success();
                            $("#Edit_SupplyManual").prop('checked', false);
                            $(".manual").hide();
                            $(".insumo").show();

                            $("#edit_item_modal").modal("hide");
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#edit_item_alert_text").html(error.responseText);
                                $("#edit_item_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            }
        },
        reset: {
            items: {
                edit: function () {
                    editItemForm.resetForm();
                    $("#edit_item_form").trigger("reset");
                    $("#edit_item_alert").removeClass("show").addClass("d-none");
                    $("#detail_modal").modal("show");
                },
            },
            file: function () {
                $("#gdocs_frame").attr("hidden", true);
                $("#files_pdf").attr("hidden", true);
            }
        }
    };

    var select2 = {
        init: function () {
            this.budgetTypes.init();
            this.types.init();
            this.supplyFamilies.init();
            this.workfronts();
            this.supplies.init();
            this.attentionStatus.init();
            this.status.init();
            this.filters();
        },
        status: {
            init: function () {
                $(".select2-status").select2();
            }
        },
        budgetTypes: {
            init: function () {
                $(".select2-budget-types").select2();
            }
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        },
        supplyFamilies: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/familias-de-insumos")
                }).done(function (result) {
                    $(".select2-supply-families").select2({
                        data: result
                    });
                });
            }
        },
        files: {
            init: function () {
                $(".select2-files").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/pre-requerimientos/archivos-tspec/${id}`)
                }).done(function (result) {
                    $(".select2-files").empty();
                    $(".select2-files").select2({
                        data: result
                    });
                });
            },
            existing: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/pre-requerimientos/archivos/${id}`)
                }).done(function (result) {
                    selDiv = document.querySelector("#existingFiles");
                    selDiv.innerHTML = "";
                    result.forEach(function (item) {
                        selDiv.innerHTML += item.text + "<br/>";
                    })
                });
            }
        },
        workfronts: function () {
            $.ajax({
                url: _app.parseUrl("/select/frentes-formula"),
                data: {
                    projectFormulaId: projectFormulaId
                }
            })
                .done(function (result) {
                    $('#add_item_form').find(".select2-work-fronts").empty();
                    $('#add_item_form').find(".select2-work-fronts").select2({
                        data: result
                    });
                    $('#edit_item_form').find(".select2-work-fronts").empty();
                    $('#edit_item_form').find(".select2-work-fronts").select2({
                        data: result
                    });
                });
        },
        supplies: {
            init: function () {
                $(".select2-supplies").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero`)
                }).done(function (result) {
                    $(".select2-supplies").select2({
                        data: result
                    });
                    let id = $(".select2-supplies").val();
                    $.ajax({
                        url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                    }).done(function (result) {
                        $("[name='MeasurementUnitName']").val(result);
                    });
                    /*
                    $.ajax({
                        url: _app.parseUrl(`/logistica/pre-requerimientos/insumo-meta/${id}`)
                    }).done(function (result) {
                        limit = result;
                        $("#add_item_form #limite").text("(límite: " + limit + ")");
                        $("#edit_item_form #limite").text("(límite: " + limit + ")");
                    });
                    */
                });
            },
            reloadEdit: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/insumos-acero`),
                }).done(function (result) {
                    $("#Edit_SupplyId").empty();
                    $("#Edit_SupplyId").select2({
                        data: result
                    });
                });
            }
        },
        attentionStatus: {
            init: function () {
                $(".select2-attention-status-filter").select2();
            }
        },
        filters: function () {
            $("#status_filter").on("change", function () {
                datatables.mainDt.reload();
            });
            $("#attention_status_filter").on("change", function () {
                datatables.mainDt.reload();
            });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var validate = {
        init: function () {
            editItemForm = $("#edit_item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.items.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#edit_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.items.edit();
                });

            $("#file_modal").on("hidden.bs.modal",
                function () {
                    form.reset.file();
                });
        }
    };

    var events = {
        init: function () {

            $("#edit_item_form [name='SupplyId']").attr("id", "Edit_SupplyId");
            $("#edit_item_form [name='WorkFrontId']").attr("id", "Edit_WorkFrontId");
            $("#edit_item_form [name='SupplyManual']").attr("id", "Edit_SupplyManual");

            $("#Edit_SupplyId").on("change", function () {
                let id = this.value;
                $.ajax({
                    url: _app.parseUrl(`/logistica/insumos/obtener-unidad/${id}`)
                }).done(function (result) {
                    $("#edit_item_form [name='MeasurementUnitName']").val(result);
                });
            });
            $("#btn_deleteFile").hide();
            $(".manual").hide();

            $(".supply_manual").on("change", function () {
                var data = $(this);

                if ($(this).is(':checked')) {
                    data.val(true);
                    $(".insumo").hide();
                    $(".manual").show();
                }
                else {
                    data.val(false);
                    $(".manual").hide();
                    $(".insumo").show();
                }
            });

            $("#btn_LoadFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri.includes("pdf")) {
                    $("#gdocs_frame").attr("hidden", true);
                    $("#files_pdf").removeAttr("hidden");
                    loadPdf("Archivo", uri, "files_pdf");
                } else {
                    $("#files_pdf").attr("hidden", true);
                    $("#gdocs_frame").removeAttr("hidden");
                    let documentUrl = "";
                    documentUrl = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(uri));
                    $("#gdocs_frame").prop("src", documentUrl + "&embedded=true");
                }
            });

            $("#btn_downloadFile").on("click", function () {
                var uri = $("#select_files").val();
                if (uri != null) {
                    window.location = `${uri}`;
                }
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatables.init();
            validate.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Request.init();
});