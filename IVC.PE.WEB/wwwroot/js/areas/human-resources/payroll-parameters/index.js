var PayrollMovementWeek = function () {

    var parameterAddForm = null;
    var parameterEditForm = null;

    var workerEditForm = null;
    var fundPensionEditForm = null;

    var parametersDataTable = null;
    var categoriesDataTable = null;
    var fundPensionsDataTable = null;

    var parametersOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/parametros/planilla/listar`),
            dataSrc: ""
        },
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevos Parámetros",
                className: "btn-primary",
                action: function (e, dt, node, config) {
                    $("#parameter_add_modal").modal("show");
                }
            }
        ],
        columns: [
            {
                title: "Proyecto",
                data: "project.abbreviation",
            },
            {
                title: "UIT",
                data: "uit"
            },
            {
                title: "Sueldo Mínimo",
                data: "minimumWage"
            },
            {
                title: "Rem. Máxima Asegurable",
                data: "maximumInsurableRemuneration"
            },
            {
                title: "EsSalud +Vida",
                data: "esSaludMasVidaCost"
            },
            {
                title: "Tasa SCTR",
                data: "sctrRate"
            },
            {
                title: "SCTR Salud Fijo",
                data: "sctrHealthFixed"
            },
            {
                title: "SCTR Pensión Fijo",
                data: "sctrPensionFixed"
            },
            {
                title: "Cuota Sindical",
                data: "unionFee"
            },
            {
                title: "T/C Dolar",
                data: "dollarExchangeRate"
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
                    return tmp;
                }
            }
        ]
    };
    var categoriesOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/parametros/categorias/listar`),
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Categoría",
                data: "workerCategoryName"
            },
            {
                title: "Jornal Diario",
                data: "dayWage"
            },
            {
                title: "B.U.C.",
                data: "bucRate"
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
                    return tmp;
                }
            }
        ]
    };
    var fundPensionsOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/parametros/pensiones/listar`),
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "AFP",
                data: "name"
            },
            {
                title: "Fondo",
                data: "fundRate"
            },
            {
                title: "Comisión Flujo",
                data: "flowComissionRate"
            },
            {
                title: "Comisión Mixta",
                data: "mixedComissionRate"
            },
            {
                title: "Prima de Seguro",
                data: "disabilityInsuranceRate"
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
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            this.parameter.init();
            this.categories.init();
            this.fundPensions.init();
        },
        parameter: {
            init: function () {
                parametersDataTable = $("#parameters_datatable").DataTable(parametersOptions);
                this.events();
            },
            events: function () {
                parametersDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.parameter.edit(id);
                    });
            }
        },
        categories: {
            init: function () {
                categoriesDataTable = $("#categories_datatable").DataTable(categoriesOptions);
                this.events();
            },
            events: function () {
                categoriesDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.worker.edit(id);
                    });
            }
        },
        fundPensions: {
            init: function () {
                fundPensionsDataTable = $("#fund_pensions_datatable").DataTable(fundPensionsOptions);
                this.events();
            },
            events: function () {
                fundPensionsDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.fundPension.edit(id);
                    });
            }
        }
    };

    var form = {
        load: {
            parameter: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/parametros/planilla/${id}`)
                    })
                        .done(function (result) {
                            console.log(result);
                            let formElements = $("#parameter_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='ProjectId']").val(result.projectId);
                            formElements.find("[name='parameter_field_projectid']").val(result.projectId).trigger('change');
                            formElements.find("[name='UIT']").val(result.uit);
                            formElements.find("[name='MinimumWage']").val(result.minimumWage);
                            formElements.find("[name='DollarExchangeRate']").val(result.dollarExchangeRate);
                            formElements.find("[name='MaximumInsurableRemuneration']").val(result.maximumInsurableRemuneration);
                            formElements.find("[name='SCTRRate']").val(result.sctrRate);
                            formElements.find("[name='EsSaludMasVidaCost']").val(result.esSaludMasVidaCost);
                            formElements.find("[name='UnionFee']").val(result.unionFee);
                            $("#parameter_edit_modal").modal("show");
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            },
            worker: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/parametros/categorias/${id}`),
                    })
                        .done(function (result) {
                            let formElements = $("#category_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='WorkerCategoryId']").val(result.workerCategoryId);
                            formElements.find("[name='WorkerCategoryName']").val(result.workerCategoryName);
                            formElements.find("[name='DayWage']").val(result.dayWage);
                            formElements.find("[name='BUCRate']").val(result.bucRate);
                            $("#category_edit_modal").modal('show');
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            },
            fundPension: {
                edit: function (id) {
                    _app.loader.show();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/parametros/pensiones/${id}`),
                    })
                        .done(function (result) {
                            let formElements = $("#fund_pension_edit_form");
                            formElements.find("[name='Id']").val(result.id);
                            formElements.find("[name='Code']").val(result.code);
                            formElements.find("[name='Name']").val(result.name);
                            formElements.find("[name='FundRate']").val(result.fundRate);
                            formElements.find("[name='FlowComissionRate']").val(result.flowComissionRate);
                            formElements.find("[name='MixedComissionRate']").val(result.mixedComissionRate);
                            formElements.find("[name='DisabilityInsuranceRate']").val(result.disabilityInsuranceRate);
                            $("#fund_pension_edit_modal").modal('show');
                        })
                        .always(function () {
                            _app.loader.hide();
                        });
                }
            }
        },
        submit: {
            parameter: {
                add: function (formElement) {
                    $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='parameter_field_projectid']").val());
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/recursos-humanos/parametros/planilla/crear"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#parameter_add_alert_text").html(error.responseText);
                                $("#parameter_add_alert").removeClass("d-none").addClass("show");
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
                        url: _app.parseUrl(`/recursos-humanos/parametros/planilla/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                            $("#parameter_edit_modal").modal("hide");
                            parametersDataTable.ajax.reload();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#parameter_edit_alert_text").html(error.responseText);
                                $("#parameter_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            },
            worker: {
                edit: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/parametros/categorias/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                            $("#category_edit_modal").modal("hide");
                            categoriesDataTable.ajax.reload();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#category_edit_alert_text").html(error.responseText);
                                $("#category_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                }
            },
            fundPension: {
                edit: function (formElement) {
                    let data = $(formElement).serialize();
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/recursos-humanos/parametros/pensiones/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.add.success();
                            $("#fund_pension_edit_modal").modal('hide');
                            fundPensionsDataTable.ajax.reload();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#fund_pension_edit_alert_text").html(error.responseText);
                                $("#fund_pension_edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            }
        },
        reset: {
            parameter: {
                add: function () {
                    parameterAddForm.resetForm();
                    $("#parameter_add_form").trigger("reset");
                    $("#parameter_add_alert").removeClass("show").addClass("d-none");
                },
                edit: function () {
                    parameterEditForm.resetForm();
                    $("#parameter_edit_form").trigger("reset");
                    $("#parameter_edit_alert").removeClass("show").addClass("d-none");
                }
            },
            worker: {
                edit: function () {
                    workerEditForm.resetForm();
                    $("#worker_edit_form").trigger("reset");
                    $("#worker_edit_alert").removeClass("show").addClass("d-none");
                }
            },
            fundPension: {
                edit: function () {
                    fundPensionEditForm.resetForm();
                    $("#fund_pension_edit_form").trigger("reset");
                    $("#fund_pension_edit_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var modal = {
        init: function () {
            $("#parameter_add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.parameter.add();
                });

            $("#parameter_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.parameter.edit();
                });

            $("#category_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.worker.edit();
                });

            $("#fund_pension_edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.fundPension.edit();
                });
        }
    };

    var select2 = {
        init: function () {
            this.projects.init();
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                })
            }
        }
    };

    var validate = {
        init: function () {
            parameterAddForm = $("#parameter_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.parameter.add(formElement);
                }
            });
            parameterEditForm = $("#parameter_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.parameter.edit(formElement);
                }
            });
            workerEditForm = $("#category_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.worker.edit(formElement);
                }
            });
            fundPensionEditForm = $("#fund_pension_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.fundPension.edit(formElement);
                }
            });
        }
    };

    return {
        init: function () {
            validate.init();
            modal.init();
            select2.init();
            datatable.init();
        }
    };
}();

$(function () {
    PayrollMovementWeek.init();
});