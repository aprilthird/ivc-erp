var Formula = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/finanzas/cargafianza/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Proyecto",
                data: "project.name"
            },
            {
                title: "Garante",
                data: "bondGuarantor.name"
            },
            {
                title: "Título",
                data: "budgetTitle.name"
            },
            {
                title: "Tipo de Fianza",
                data: "bondType.name"
            },
            {
                title: "Entidad Bancaria",
                data: "bank.name"
            },
            {
                title: "Numero de Fianza",
                data: "bondNumber"
            },
            {
                title: "Renovación",
                data: "bondRenovation.name"
            },
            {
                title: "Monto en S/.",
                data: "penAmmount"
            },
            {
                title: "Monto en US$",
                data: "usdAmmount"
            },
            {
                title: "Plazo",
                data: "daysLimitTerm"
            },
            {
                title: "Fecha de Creación",
                data: "createDate"
            },
            {
                title: "Contra Garantía",
                data: "guaranteeDesc"
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

    var datatable = {
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
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La fianza será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/finanzas/cargafianza/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La fianza ha sido eliminada con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el banco"
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
                    url: _app.parseUrl(`/finanzas/cargafianza/${id}`)
                }).done(function (result) {
                    let formElements = $("#edit_form");
                    formElements.find("[name='Id']").val(result.id);
                    formElements.find("[name='ProjectId']").val(result.projectId).trigger("change");
                    formElements.find("[name='BondGuarantorId']").val(result.bondGuarantorId).trigger("change");
                    formElements.find("[name='BudgetTitleId']").val(result.budgetTitleId).trigger("change");
                    
                    formElements.find("[name='BondTypeId']").val(result.bondTypeId).trigger("change");
                    formElements.find("[name='BankId']").val(result.bankId).trigger("change");
                    formElements.find("[name='BondNumber']").val(result.bondNumber);
                    formElements.find("[name='BondRenovationId']").val(result.bondRenovationId).trigger("change");
                    formElements.find("[name='EmployeeId']").val(result.employeeId).trigger("change");

                    formElements.find("[name='PenAmmount']").val(result.penAmmount);
                    formElements.find("[name='UsdAmmount']").val(result.usdAmmount);
                    formElements.find("[name='daysLimitTerm']").val(result.daysLimitTerm);
                    formElements.find("[name='CreateDate']").val(result.createDate);
                    formElements.find("[name='guaranteeDesc']").val(result.guaranteeDesc);

                    
                    $("#edit_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/finanzas/cargafianza/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.add.success();
                    }).fail(function (error) {
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
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/finanzas/cargafianza/editar/${id}`),
                    type: "put",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatable.reload();
                        _app.show.notification.edit.success();
                    }).fail(function (error) {
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
                $("#add_form [name='IsDirectCost']").prop("checked", true);
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='IsDirectCost']").prop("checked", true);
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
        }
    };

    var select2 = {
        init: function () {
            this.answers.init();
            this.status.init();
            this.letters.init();
            this.issuerTargets.init();
            this.interestGroups.init();
            this.employees.init();
            this.budgetType.init();
            this.bondType.init();
            this.projects.init();
            this.banks.init();
            this.renovations.init();
            this.guarantors.init();
        },
        answers: {
            init: function () {
                $(".select2-answers").select2();
            }
        },
        status: {
            init: function () {
                $(".select2-status").select2();
            }
        },
        letters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas")
                }).done(function (result) {
                    $(".select2-letters").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        issuerTargets: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/entidades-emisoras-receptoras-de-cartas")
                }).done(function (result) {
                    $(".select2-issuer-targets").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        interestGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-interes")
                }).done(function (result) {
                    $(".select2-interest-groups").select2({
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
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        employees: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-employees").select2({
                        data: result,
                        allowClear: true
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
                        data: result,
                        allowClear: true
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
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        renovations: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/renovaciones")
                }).done(function (result) {
                    $(".select2-renovations").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        guarantors: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/garantes")
                }).done(function (result) {
                    $(".select2-guarantors").select2({
                        data: result,
                        allowClear: true
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
        }
    };

    return {
        init: function () {
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    }
}();

$(function () {
    Formula.init();
});