var PayrollConcept = function () {

    var addForm = null;
    var editForm = null;
    var formulaAddForm = null;
    var formulaEditForm = null;

    var conceptId = null;

    var conceptDt = null;
    var formulaDt = null;

    var conceptDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/conceptos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Descripción",
                data: "description"
            },
            {
                title: "Descripción Corta",
                data: "shortDescription"
            },
            {
                title: "Categoría",
                data: "categoryName"
            },
            {
                title: "Opciones",
                width: "15%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-formula">`;
                    tmp += `<i class="fa fa-flask"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };
    var formulaDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/conceptos/formulas/listar`),
            data: function (d) {
                d.conceptId = conceptId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Régimen Laboral",
                data: "laborRegimeName"
            },
            {
                title: "Fórmula",
                data: "formula"
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
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#formula_modal").modal("hide");
                    $("#formula_add_modal").modal("show");
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.concept.init();
            this.formula.init();
        },
        concept: {
            init: function() {
                conceptDt = $("#payroll_concepts_datatable").DataTable(conceptDtOpts);
                this.events();
            },
            reload: function() {
                conceptDt.ajax.reload();
            },
            events: function() {
                conceptDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });

                conceptDt.on("click",
                    ".btn-formula",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        conceptId = id;
                        datatables.formula.reload();
                        $("#formula_modal").modal("show");
                    });

                conceptDt.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El concepto será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/recursos-humanos/conceptos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El concepto ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el concepto"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        formula: {
            init: function () {
                formulaDt = $("#formulas_datatable").DataTable(formulaDtOpts);
                this.events();
            },
            reload: function () {
                formulaDt.clear().draw();
                formulaDt.ajax.reload();
            },
            events: function () {
                formulaDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.formula.edit(id);
                    });
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/conceptos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Code']").val(result.code);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='ShortDescription']").val(result.shortDescription);
                        formElements.find("[name='CategoryId']").val(result.categoryId).trigger("change");
                        formElements.find("[name='SunatCode']").val(result.sunatCode);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            formula: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/conceptos/formulas/${id}`)
                    })
                        .done(function (result) {
                            let formElements = $("#formula_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='PayrollConceptId']").val(result.payrollConceptId);
                            formElements.find("[name='LaborRegimeId']").val(result.laborRegimeId).trigger("change");
                            formElements.find("[name='Active']").val(result.active.toString()).trigger("change");
                            formElements.find("[name='Formula']").val(result.formula);
                            formElements.find("[name='PayrollVariableId']").val(result.payrollVariableId).trigger("change");
                            formElements.find("[name='IsAffectedToAfp']").val(result.isAffectedToAfp.toString()).trigger("change");
                            formElements.find("[name='IsAffectedToEsSalud']").val(result.isAffectedToEsSalud.toString()).trigger("change");
                            formElements.find("[name='IsAffectedToOnp']").val(result.isAffectedToOnp.toString()).trigger("change");
                            formElements.find("[name='IsAffectedToQta']").val(result.isAffectedToQta.toString()).trigger("change");
                            formElements.find("[name='IsAffectedToRetJud']").val(result.isAffectedToRetJud.toString()).trigger("change");
                            formElements.find("[name='IsComputableToCTS']").val(result.isComputableToCTS.toString()).trigger("change");
                            formElements.find("[name='IsComputableToGrati']").val(result.isComputableToGrati.toString()).trigger("change");
                            formElements.find("[name='IsComputableToVacac']").val(result.isComputableToVacac.toString()).trigger("change");
                            $("#formula_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/recursos-humanos/conceptos/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.concept.reload();
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
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/conceptos/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.concept.reload();
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
            formula: {
                add: function (formElement) {
                    $(formElement).find("[name='PayrollConceptId']").val(conceptId);
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/conceptos/formulas/crear`),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.formula.reload();
                            $("#formula_add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#formula_add_alert_text").html(error.responseText);
                                $("#formula_add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                },
                edit: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/conceptos/formulas/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatables.formula.reload();
                            $("#formula_edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#formula_edit_alert_text").html(error.responseText);
                                $("#formula_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
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
            },
            formula: {
                add: function () {
                    formulaAddForm.resetForm();
                    $("#formula_add_form").trigger("reset");
                    $("#formula_add_alert").removeClass("show").addClass("d-none");
                    $("#formula_modal").modal("show");
                },
                edit: function () {
                    formulaEditForm.resetForm();
                    $("#formula_edit_form").trigger("reset");
                    $("#formula_edit_alert").removeClass("show").addClass("d-none");
                    $("#formula_modal").modal("show");
                }
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

            formulaAddForm = $("#formula_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.formula.add(formElement);
                }
            });

            formulaEditForm = $("#formula_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.formula.edit(formElement);
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.categories.init();
        },
        categories: {
            init: function () {
                $(".select2-concept-categories").select2();
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

            $("#formula_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.formula.add();
                });

            $("#formula_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.formula.edit();
                });
        }
    };

    var events = {
        init: function () {
            this.fields();
        },
        fields: function () {
            $("#add_form [name='Id']").attr("id", "Add_Id");
            $("#edit_form [name='Id']").attr("id", "Edit_Id");

            $("#add_form [name='Code']").attr("id", "Add_Code");
            $("#edit_form [name='Code']").attr("id", "Edit_Code");

            $("#add_form [name='Description']").attr("id", "Add_Description");
            $("#edit_form [name='Description']").attr("id", "Edit_Description");

            $("#add_form [name='ShortDescription']").attr("id", "Add_ShortDescription");
            $("#edit_form [name='ShortDescription']").attr("id", "Edit_ShortDescription");

            $("#add_form [name='CategoryId']").attr("id", "Add_CategoryId");
            $("#edit_form [name='CategoryId']").attr("id", "Edit_CategoryId");

            $("#add_form [name='SunatCode']").attr("id", "Add_SunatCode");
            $("#edit_form [name='SunatCode']").attr("id", "Edit_SunatCode");
        }
    };

    return {
        init: function () {
            validate.init();
            select2.init();
            modals.init();
            datatables.init();
            events.init();
        }
    };
}();

$(function () {
    PayrollConcept.init();
});