var Cement = function () {

    var addForm = null;
    var editForm = null;

    var cementDatatable = null;
    var importDataForm = null;

    var cementDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/listar"),
            data: function (d) {
                d.projectFormulaId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                d.projectPhaseId = $("#project_phase_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        ordering: false,
        columns: [
            {
                //title: "Frente",
                width: "5%",
                data: "workFront.code"
            },
            {
                //tile: "Item",
                width: "15%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = tmp;
                    if (und == "") {
                        if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }
                    else
                        return b;

                }
            },
            {
                //tile: "Descripción",
                width: "40%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = result.description;
                    if (und == "") {
                        if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }
                    else
                        return b;

                }
            },
            {
                //tile: "Unidad",
                width: "5%",
                data: "unit"
            },
            {
                //title: "Metrado",
                data: function (result) {
                    var b = result.metered;

                    if (b == "0.00")
                        return "";
                    else
                        return b;
                }
            },
            {
                //tile: "Rod6mm",
                width: "5%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = result.contractualMeteredTypeOne;
                    if (und == "") {
                        if (b == "0.00")
                            return "";
                        else if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }

                    if (b == "0.00")
                        return "";
                    else
                        return b;
                }
            },
            {
                //tile: "Rod6mm",
                width: "5%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = result.contractualMeteredTypeFive;
                    if (und == "") {
                        if (b == "0.00")
                            return "";
                        else if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }

                    if (b == "0.00")
                        return "";
                    else
                        return b;
                }
            },
            {
                //tile: "Rod3x8",
                width: "5%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = result.contractualRestatedTypeOne;
                    if (und == "") {
                        if (b == "0.00")
                            return "";
                        else if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }

                    if (b == "0.00")
                        return "";
                    else
                        return b;
                },
            },
            {
                //tile: "Rod3x8",
                width: "5%",
                data: function (result) {
                    var tmp = result.itemNumber;
                    var und = result.unit;
                    var b = result.contractualRestatedTypeFive;
                    if (und == "") {
                        if (b == "0.00")
                            return "";
                        else if (tmp.length <= 2)
                            return `<b style="color:rgb(255, 25, 25);" >${b}</b>`;
                        else if (tmp.length == 5)
                            return `<b style="color:rgb(50, 40, 210);" >${b}</b>`;
                        else if (tmp.length == 8)
                            return `<b style="color:rgb(85, 185, 65);" >${b}</b>`;
                        else if (tmp.length == 11)
                            return `<b style="color:rgb(185, 45, 45);" >${b}</b>`;
                        else if (tmp.length == 14)
                            return `<b style="color:rgb(250, 40, 250);" >${b}</b>`;
                        else if (tmp.length == 17)
                            return `<b style="color:rgb(40, 190, 250);" >${b}</b>`;
                        else
                            return b;
                    }

                    if (b == "0.00")
                        return "";
                    else
                        return b;
                }
            },
            {
                //title: "Opciones",
                width: "5%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 7] }
        ]
    };

    var datatables = {
        init: function () {
            this.cementDt.init();
        },
        cementDt: {
            init: function () {
                cementDatatable = $("#cements_datatable").DataTable(cementDtOpt);
                this.initEvents();
            },
            reload: function () {
                cementDatatable.ajax.reload();
                select2.totals();
            },
            initEvents: function () {

                cementDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                cementDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El cemento será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.cementDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El cemento ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: "animated tada",
                                                confirmButtonClass: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar el cemento"
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
            affectation: function () {
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f1`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f1']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f2`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f2']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f3`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f3']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f4`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f4']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f5`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f5']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f6`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f6']").val(result);
                    });
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/f7`)
                })
                    .done(function (result) {
                        let formElements = $("#load_data_form");
                        formElements.find("[name='select_affection_f7']").val(result);
                    });
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_titles']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_formulas']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_phases']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='select_work_fronts']").val(result.workFrontId).trigger("change");
                        formElements.find("[name='ItemNumber']").val(result.itemNumber);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='select_units']").val(result.unit);
                        formElements.find("[name='ContractualMeteredTypeOne']").val(result.contractualMeteredTypeOne);
                        formElements.find("[name='ContractualMeteredTypeFive']").val(result.contractualMeteredTypeFive);
                        formElements.find("[name='ContractualRestatedTypeOne']").val(result.contractualRestatedTypeOne);
                        formElements.find("[name='ContractualRestatedTypeFive']").val(result.contractualRestatedTypeFive);

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/crear"),
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
                        datatables.cementDt.reload();
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
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='WorkFrontId']").val($(formElement).find("[name='select_work_fronts']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phases']").val());
                $(formElement).find("[name='Unit']").val($(formElement).find("[name='select_units']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.cementDt.reload();
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
            load: {
                data: function (formElement) {
                    $(formElement).find("[name='AffectationF1']").val($(formElement).find("[name='select_affection_f1']").val());
                    $(formElement).find("[name='AffectationF2']").val($(formElement).find("[name='select_affection_f2']").val());
                    $(formElement).find("[name='AffectationF3']").val($(formElement).find("[name='select_affection_f3']").val());
                    $(formElement).find("[name='AffectationF4']").val($(formElement).find("[name='select_affection_f4']").val());
                    $(formElement).find("[name='AffectationF5']").val($(formElement).find("[name='select_affection_f5']").val());
                    $(formElement).find("[name='AffectationF6']").val($(formElement).find("[name='select_affection_f6']").val());
                    $(formElement).find("[name='AffectationF7']").val($(formElement).find("[name='select_affection_f7']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $.ajax({
                        url: "/oficina-tecnica/monto-consolidado-cementos/afectacion",
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: data
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                    }).done(function (result) {
                        datatables.cementDt.reload();
                        $("#load_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#load_data_alert_text").html(error.responseText);
                            $("#load_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                    $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                    $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
                }
            }
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

            loadDataForm = $("#load_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.load.data(formElement);
                }
            });
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
        }
    };

    var select2 = {
        init: function () {
            this.title();
            this.formula();
            this.filters();
            this.phase();
            this.workfront();
            this.totals();
        },
        title: function () {
            $.ajax({
                url: _app.parseUrl("/select/titulos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-titles").select2({
                        data: result
                    });
                    $(".select2-budgettitle-filter").select2({
                        data: result
                    });
                });
        },
        formula: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                    $(".select2-projectformula-filter").select2({
                        data: result
                    });
                });
        },
        phase: function () {
            $(".select2-projectphase-filter").empty();
            $(".select2-projectphase-filter").append(`<option>Todos</option>`);
            $.ajax({
                url: _app.parseUrl("/select/fases-proyecto-frente-trabajo"),
                data: {
                    workFrontId: $("#work_front_filter").val()
                }
            })
                .done(function (result) {
                    $(".select2-phases").select2({
                        data: result
                    });
                    $(".select2-projectphase-filter").select2({
                        data: result
                    });
                });


        },
        workfront: function () {
            $(".select2-workfront-filter").empty();
            $(".select2-workfront-filter").append(`<option>Todos</option>`);
            $.ajax({
                url: _app.parseUrl("/select/frentes-formula"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val()
                }
            })
                .done(function (result) {
                    $(".select2-work-fronts").select2({
                        data: result
                    });
                    $(".select2-workfront-filter").select2({
                        data: result
                    });
                });
        },
        filters: function () {
            $("#project_formula_filter").on("change", function () {
                select2.workfront();
                datatables.cementDt.reload();
            });
            $("#budget_title_filter").on("change", function () {
                datatables.cementDt.reload();
            });
            $("#project_phase_filter").on("change", function () {
                datatables.cementDt.reload();
            });
            $("#work_front_filter").on("change", function () {
                select2.phase();
                datatables.cementDt.reload();
            });
        },
        totals: function () {
            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/metrado-contractual-tipo-uno"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val(),
                    budgetTitleId: $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    projectPhaseId: $("#project_phase_filter").val()
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_metered_type_one").val(result);
                });
            //---------------------------
            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/metrado-contractual-tipo-cinco"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val(),
                    budgetTitleId: $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    projectPhaseId: $("#project_phase_filter").val()
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_metered_type_five").val(result);
                });
            //---------------------------
            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/metrado-replanteado-tipo-uno"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val(),
                    budgetTitleId: $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    projectPhaseId: $("#project_phase_filter").val()
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_restated_type_one").val(result);
                });
            //---------------------------
            $.ajax({
                url: _app.parseUrl("/oficina-tecnica/monto-consolidado-cementos/metrado-replanteado-tipo-cinco"),
                data: {
                    projectFormulaId: $("#project_formula_filter").val(),
                    budgetTitleId: $("#budget_title_filter").val(),
                    workFrontId: $("#work_front_filter").val(),
                    projectPhaseId: $("#project_phase_filter").val()
                },
                dataSrc: ""
            })
                .done(function (result) {
                    $("#total_restated_type_five").val(result);
                });
        }
    };

    var events = {
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/monto-consolidado-cementos/excel-carga-masiva`;
            });
        },
        deleteByFilters: function () {
            $("#btn-delete-by-filters").on("click", function () {
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Los monto-consolidado-cementos filtrados serán eliminados permanentemente",
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
                                url: _app.parseUrl(`/oficina-tecnica/monto-consolidado-cementos/eliminar-filtro`),
                                data: {
                                    projectFormulaId: $("#project_formula_filter").val(),
                                    budgetTitleId: $("#budget_title_filter").val()
                                },
                                type: "delete",
                                success: function (result) {
                                    datatables.cementDt.reload();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "Los monto-consolidado-cementos han sido eliminados con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (result) {
                                    console.log(result.responseText);
                                    swal.fire({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn-danger",
                                        animation: false,
                                        customClass: "animated tada",
                                        confirmButtonClass: "Entendido",
                                        text: result.responseText
                                    });
                                }
                            });
                        });
                    }
                });
            });
        },
        affectation: function () {
            $("#btn-update").on("click", function () {
                _app.loader.show();
                $.ajax({
                    url: "/oficina-tecnica/monto-consolidado-cementos/cargar",
                    type: "POST",
                    contentType: false,
                    processData: false
                }).always(function () {
                    _app.loader.hide();
                }).done(function (result) {
                    datatables.cementDt.reload();
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#load_data_alert_text").html(error.responseText);
                        $("#load_data_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            })
        }
    };

    return {
        init: function () {
            select2.init();
            events.excel();
            events.deleteByFilters();
            events.affectation();
            datatables.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Cement.init();
});