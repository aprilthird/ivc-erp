var ReEntryForReturn = function () {

    var mainDatatable = null;
    var detailDatatable = null;
    var addForm = null;
    var editForm = null;
    var addItemForm = null;
    var editItemForm = null;
    var Id = null;
    var supplyFamilyId = null;
    var workFrontId = null;
    var detailForm = null;
    var projectFormulaId = null;

    var vals = [];

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/reingreso-por-devolucion/listar"),
            data: function (d) {
                d.projectFormulaId = $("#project_formula_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "N° Documento",
                data: "documentNumber"
            },
            {
                title: "Fecha de Devolución",
                data: "returnDate"
            },
            {
                title: "Fórmula",
                data: "projectFormula.name"
            },
            {
                title: "Frente",
                data: "workFront.code"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Familia",
                data: "supplyFamily.name"
            },
            {
                title: "Persona que Devuelve",
                data: "userName"
            },
            {
                title: "Estado",
                data: "status",
                render: function (data) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">EN PROCESO</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">INGRESADO</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Carpeta",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.status == 1) {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-confirm" title="Ingresar">`;
                        tmp += `<i class="la la-check-circle"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    if (row.fileUrl != null) {
                        tmp += `<button data-url="${row.fileUrl}" data-name="${row.documentNumber}" class="btn btn-danger btn-sm btn-icon btn-view" title="PDF">`;
                        tmp += `<i class="la la-file-pdf-o"></i></button> `;
                    }
                    /*
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    */
                    return tmp;
                }
            },
            {
                title: "Opciones",
                width: "8%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    //tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-issue" title="Emitir">`;
                    //tmp += `<i class="fas fa-arrow-right"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;

                    return tmp;
                }
            }
        ]
    };

    var detailOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/reingreso-por-devolucion/items/listar"),
            data: function (d) {
                d.id = Id;

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Grupo",
                data: "goalBudgetInput.supply.supplyGroup.name"
            },
            {
                title: "Fase",
                data: "projectPhase.code"
            },
            {
                title: "Código IVC",
                data: "goalBudgetInput.supply.fullCode"
            },
            {
                title: "Insumo",
                data: "goalBudgetInput.supply.description"
            },
            {
                title: "Und",
                data: "goalBudgetInput.supply.measurementUnit.abbreviation"
            },
            {
                title: "Metrado",
                data: "quantity"
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

                mainDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                mainDatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Id = id;
                        _app.loader.show();

                        //$("#detail_modal").modal("show");

                        $.ajax({
                            url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/${Id}`)
                        })
                            .done(function (result) {
                                supplyFamilyId = result.supplyFamilyId;
                                workFrontId = result.workFrontId;
                                projectFormulaId = result.projectFormulaId;
                                select2.supply.init();

                                select2.projectPhase.init();
                                datatables.itemDt.reload();
                            })
                            .always(function () {
                                _app.loader.hide();

                                $("#detail_modal").modal("show");
                            });

                    });

                mainDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Pedido de Campo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.mainDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Pedido de Campo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Pedido de Campo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
                    ".btn-confirm",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Reingreso por devolución será confirmado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, confirmar",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/confirmar/${id}`),
                                        type: "put",
                                        success: function (result) {
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Reingreso por devolución ha sido confirmado con éxito",
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
                                                text: errormessage.responseText
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                mainDatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let url = $btn.data("url");
                        let name = $btn.data("name");
                        form.load.pdf(name, url);
                    });
            }
        },
        itemDt: {
            init: function () {
                detailDatatable = $("#detail_datatable").DataTable(detailOpts);
                this.initEvents();
            },
            reload: function () {
                detailDatatable.ajax.reload();
            },
            initEvents: function () {
                detailDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.editItem(id);
                    });

                detailDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Item será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/items/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.itemDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Item ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el Item"
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

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_project_formula']").val(result.projectFormulaId).trigger("change");

                        setTimeout(() => {
                            formElements.find("[name='select_work_front']").val(result.workFrontId).trigger("change");

                            setTimeout(() => {
                                //select2.sewerGroup.edit();

                                setTimeout(() => {
                                    formElements.find("[name='select_sewer_group']").val(result.sewerGroupId).trigger("change");
                                }, 1500);
                            }, 100);
                        }, 1500);

                        formElements.find("[name='select_supply_family']").val(result.supplyFamilyId).trigger("change");
                        formElements.find("[name='ReturnDate']").datepicker('setDate', result.returnDate);
                    })
                    .always(function () {

                        setTimeout(() => {

                            _app.loader.hide();
                            $("#edit_modal").modal("show");
                        }, 2500);
                    });
            },
            editItem: function (id) {
                _app.loader.show();
                $("#detail_modal").modal("hide");
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/items/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_item_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ReEntryForReturnId']").val(result.reEntryForReturnId);
                        formElements.find("[name='select_goal_budget_input']").val(result.goalBudgetInputId).trigger("change");
                        /*
                        $.ajax({
                            url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/insumo-meta/${result.goalBudgetInputId}`)
                        }).done(function (result) {
                            vals = result;
                            $("#edit_folding_form #limite").text("(límite: " + vals[0] + ")");
                            $("#edit_folding_form #stock").text("(stock: " + vals[1] + ")");
                            $("#edit_folding_form #techo").text("(techo: " + vals[2] + ")");
                            formElements.find("[name='GoalBudgetInput.MeasurementUnit.Abbreviation']").val(vals[3]);
                            vals = [];
                        });*/
                        formElements.find("[name='select_project_phase']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='Quantity']").val(result.quantity);

                    })
                    .always(function () {
                        setTimeout(() => {
                            _app.loader.hide();

                            $("#edit_item_modal").modal("show");
                        }, 1000);
                    });
            },
            pdf: function (name, url) {
                $("#pdf_name").text(name);
                $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + url + "&embedded=true");
                $("#pdf_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_budget_title']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_front']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_group']").val());
                $(formElement).find("[name='WarehouseId']").val($(formElement).find("[name='select_warehouse']").val());
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_supply_family']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/almacenes/reingreso-por-devolucion/crear"),
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
                        datatables.mainDt.reload();
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
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formula']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_front']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_group']").val());
                $(formElement).find("[name='WarehouseId']").val($(formElement).find("[name='select_warehouse']").val());
                $(formElement).find("[name='SupplyFamilyId']").val($(formElement).find("[name='select_supply_family']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/editar/${id}`),
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
                        datatables.mainDt.reload();
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
            addItem: function (formElement) {
                $(formElement).find("[name='ReEntryForReturnId']").val(Id);
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_project_phase']").val());
                $(formElement).find("[name='GoalBudgetInputId']").val($(formElement).find("[name='select_goal_budget_input']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                console.log(data);
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/items/crear`),
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
                        $("#add_item_modal").modal("hide");
                        datatables.itemDt.reload();
                        $("#detail_modal").modal("show");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_item_alert_text").html(error.responseText);
                            $("#add_item_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            },
            editItem: function (formElement) {
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_project_phase']").val());
                $(formElement).find("[name='GoalBudgetInputId']").val($(formElement).find("[name='select_goal_budget_input']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                console.log(data);
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/items/editar/${id}`),
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
                        $("#edit_item_modal").modal("hide");
                        datatables.itemDt.reload();
                        $("#detail_modal").modal("show");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_item_alert_text").html(error.responseText);
                            $("#edit_item_alert").removeClass("d-none").addClass("show");
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
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
            },
            addItem: function () {
                addItemForm.resetForm();
                $("#add_item_form").trigger("reset");
                $("#add_item_alert").removeClass("show").addClass("d-none");
                $("#detail_modal").modal("show");
            },
            editItem: function () {
                editItemForm.resetForm();
                $("#edit_item_alert").removeClass("show").addClass("d-none");
                $("#edit_item_form").trigger("reset");
                $("#detail_modal").modal("show");
            },
            detail: function () {
                detailForm.resetForm();
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            addItemForm = $("#add_item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addItem(formElement);
                }
            });

            editItemForm = $("#edit_item_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.editItem(formElement);
                }
            });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var select2 = {
        init: function () {
            this.formula.init();
            this.workFront.init();
            this.workFront.edit();
            this.sewerGroup.init();
            this.sewerGroup.edit();
            this.supplyFamily.init();
            this.supplyGroup.init();
            this.supply.init();
            //this.supply.edit();
            this.projectPhase.init();
        },
        formula: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                }).done(function (result) {
                    $(".select2-project-formulas").select2({
                        data: result
                    });
                });
            }
        },
        workFront: {
            init: function () {
                $("#work_front_filter").empty();
                $("#work_front_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formula"),
                    data: {
                        projectFormulaId: $("#project_formula_filter").val()
                    }
                }).done(function (result) {
                    $("#work_front_filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#Add_WorkFrontId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formulas"),
                    method: "post",
                    data: {
                        projectFormulas: $("#Add_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Add_WorkFrontId").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#Edit_WorkFrontId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/frentes-formulas"),
                    method: "post",
                    data: {
                        projectFormulas: $("#Edit_ProjectFormulaId").val()
                    }
                }).done(function (result) {
                    $("#Edit_WorkFrontId").select2({
                        data: result
                    });
                });
            }
        },
        sewerGroup: {
            init: function () {
                $("#sewer_group_filter").empty();
                $("#sewer_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-frente"),
                    data: {
                        workFrontId: $("#work_front_filter").val()
                    }
                }).done(function (result) {
                    $("#sewer_group_filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                _app.loader.show();
                $("#Add_SewerGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-frente"),
                    data: {
                        workFrontId: $("#Add_WorkFrontId").val()
                    }
                }).done(function (result) {
                    $("#Add_SewerGroupId").select2({
                        data: result
                    });
                    _app.loader.hide();
                });
            },
            edit: function () {
                $("#Edit_SewerGroupId").empty();
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas-frente"),
                    data: {
                        workFrontId: $("#Edit_WorkFrontId").val()
                    }
                }).done(function (result) {
                    $("#Edit_SewerGroupId").select2({
                        data: result
                    });
                });
            }
        },
        supplyFamily: {
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
        supplyGroup: {
            init: function () {
                $("#supply_group_filter").empty();
                $("#supply_group_filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos"),
                    data: {
                        familyId: $("#supply_family_filter").val()
                    }
                }).done(function (result) {
                    $(".select2-supply-groups").select2({
                        data: result
                    });
                });
            }
        },
        supply: {
            init: function () {
                $(".select2-goal-budget-inputs").empty();
                $.ajax({
                    url: _app.parseUrl("/select/insumos-de-meta"),
                    data: {
                        supplyFamilyId: supplyFamilyId,
                        workFrontId: workFrontId,
                        projectFormulaId: projectFormulaId
                    }
                }).done(function (result) {
                    $(".select2-goal-budget-inputs").select2({
                        data: result
                    });
                });
            }
        },
        projectPhase: {
            init: function () {
                $(".select2-project-phases").empty();
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto-frente-trabajo"),
                    data: {
                        workFrontId: workFrontId,
                        projectFormulaId: projectFormulaId
                    }
                }).done(function (result) {
                    $(".select2-project-phases").select2({
                        data: result
                    });
                });
            }
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
            
            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });
                
            $("#add_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.addItem();
                });

            $("#edit_item_modal").on("hidden.bs.modal",
                function () {
                    form.reset.editItem();
                });
        }
    };


    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='select_project_formula']").attr("id", "Add_ProjectFormulaId");
            $("#edit_form [name='select_project_formula']").attr("id", "Edit_ProjectFormulaId");

            $("#add_form [name='select_work_front']").attr("id", "Add_WorkFrontId");
            $("#edit_form [name='select_work_front']").attr("id", "Edit_WorkFrontId");

            $("#add_form [name='select_sewer_group']").attr("id", "Add_SewerGroupId");
            $("#edit_form [name='select_sewer_group']").attr("id", "Edit_SewerGroupId");

            $("#add_form [name='select_supply_family']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='select_supply_family']").attr("id", "Edit_SupplyFamilyId");

            $("#add_item_form [name='select_goal_budget_input']").attr("id", "Add_GoalBudgetInputId");
            $("#edit_item_form [name='select_goal_budget_input']").attr("id", "Edit_GoalBudgetInputId");

            $("#add_item_form [name='select_project_phase']").attr("id", "Add_ProjectPhaseId");
            $("#edit_item_form [name='select_project_phase']").attr("id", "Edit_ProjectPhaseId");

            $("#Add_ProjectFormulaId").on("change", function () {
                select2.workFront.add();

                setTimeout(() => {
                    select2.sewerGroup.add();
                }, 1000);
            });

            $("#Add_WorkFrontId").on("change", function () {
                select2.sewerGroup.add();
            });

            $("#Edit_ProjectFormulaId").on("change", function () {
                select2.workFront.edit();
            });

            $("#Edit_WorkFrontId").on("change", function () {
                select2.sewerGroup.edit();
            });

            $("#addNewItem").on("click",
                function () {
                    _app.loader.show
                    //events.metaAdd();
                    $("#detail_modal").modal("hide");/*
                    setTimeout(() => {
                        let id = $("#Add_GoalBudgetInputId").val();
                        $.ajax({
                            url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/insumo-meta/${id}`)
                        }).done(function (result) {
                            vals = result;
                            $("#add_folding_form #limite").text("(límite: " + vals[0] + ")");
                            $("#add_folding_form #stock").text("(stock: " + vals[1] + ")");
                            $("#add_folding_form #techo").text("(techo: " + vals[2] + ")");
                            vals = [];
                        });

                        _app.loader.hide();

                        $("#add_folding_modal").modal("show");
                    }, 1500);*/
                    _app.loader.hide();

                    $("#add_item_modal").modal("show");
                });

            $("#add_button").on("click",
                function () {
                    _app.loader.show();
                    select2.workFront.add();

                    setTimeout(() => {
                        _app.loader.hide();

                        select2.sewerGroup.add();

                        $("#add_modal").modal("show");
                    }, 2000);
                });

            this.filtros();
        },
        filtros: function () {
            $("#project_formula_filter").on("change", function () {
                select2.workFront.init();
                datatables.mainDt.reload();
            });

            $("#supply_family_filter").on("change", function () {
                select2.supplyGroup.init();
                datatables.mainDt.reload();
            });

            $("#project_formula_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#work_front_filter").on("change", function () {
                datatables.mainDt.reload();
                select2.sewerGroup.init();
            });

            $("#sewer_group_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#supply_family_filter").on("change", function () {
                datatables.mainDt.reload();
            });

            $("#supply_group_filter").on("change", function () {
                datatables.mainDt.reload();
            });
        },
        metaAdd: function () {
            _app.loader.show();
            setTimeout(() => {
                let id = $("#Add_GoalBudgetInputId").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/insumo-meta/${id}`)
                }).done(function (result) {
                    vals = result;
                    $("#add_folding_form #limite").text("(límite: " + vals[0] + ")");
                    $("#add_folding_form #stock").text("(stock: " + vals[1] + ")");
                    $("#add_folding_form #techo").text("(techo: " + vals[2] + ")");


                    $("#add_folding_form [name='GoalBudgetInput.MeasurementUnit.Abbreviation']").val(vals[3]);
                    vals = [];
                });
            }, 1000);
            _app.loader.hide();
        },
        metaEdit: function () {
            _app.loader.show();
            setTimeout(() => {
                let id = $("#Edit_GoalBudgetInputId").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/reingreso-por-devolucion/insumo-meta/${id}`)
                }).done(function (result) {
                    vals = result;
                    $("#edit_folding_form #limite").text("(límite: " + vals[0] + ")");
                    $("#edit_folding_form #stock").text("(stock: " + vals[1] + ")");
                    $("#edit_folding_form #techo").text("(techo: " + vals[2] + ")");

                    $("#edit_folding_form [name='GoalBudgetInput.MeasurementUnit.Abbreviation']").val(vals[3]);
                    vals = [];
                });
            }, 1000);
            _app.loader.hide();
        }
    };

    return {
        init: function () {
            events.init();
            datatables.init();
            validate.init();
            select2.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    ReEntryForReturn.init();
});