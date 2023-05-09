var ExpensesUtility = function () {

    var addForm = null;
    var editForm = null;

    var expensesUtilityDatatable = null;

    var expensesUtilityDtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/gastos-utilidades/listar"),
            dataSrc: ""
        },
        columns: [
            {
                //title: "Agrupación",
                data: "group",
                data: function (result) {
                    var res = result.group;
                    if (res == 1)
                        return "Componente Principal"
                    else if (res == 2)
                        return "Otros Componentes"
                    else
                        return "No Definido"
                }
            },
            {
                //title: "Tipo",
                data: "budgetType.name"
            },
            {
                //title: "Titulo",
                data: "budgetTitle.name"
            },
            {
                //title: "Gasto General Fijo",
                width: "5%",
                data: "fixedGeneralExpense"
            },
            {
                //title: "% Gasto General Fijo",
                width: "5%",
                data: "fixedGeneralPercentage",
                render: function (data, type, row) {
                    return data + " %";
                }
            },
            {
                //title: "Gasto General Variable",
                width: "5%",
                data: "variableGeneralExpense"
            },
            {
                //title: "% Gasto General Variable",
                width: "5%",
                data: "variableGeneralPercentage",
                render: function (data, type, row) {
                    return data + " %";
                }
            },
            {
                //title: "Gasto General Total",
                width: "5%",
                data: "totalGeneralExpense"
            },
            {
                //title: "% Gasto General Total",
                width: "5%",
                data: "totalGeneralPercentage",
                render: function (data, type, row) {
                    return data + " %";
                }
            },
            {
                //title: "% Utilidad",
                width: "5%",
                data: "utilityPercentage",
                render: function (data, type, row) {
                    return data + " %";
                }
            },
            {
                //title: "Opciones",
                width: "5%",
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
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3, 4, 5, 6, 7, 8, 9, 10] }
        ]
    };

    var datatable = {
        init: function () {
            this.expensesUtilityDt.init();
        },
        expensesUtilityDt: {
            init: function () {
                expensesUtilityDatatable = $("#expenses_utility_datatable").DataTable(expensesUtilityDtOpt);
                this.initEvents();
            },
            reload: function () {
                expensesUtilityDatatable.ajax.reload();
            },
            initEvents: function () {

                expensesUtilityDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                expensesUtilityDatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.excel(id);
                    });

                expensesUtilityDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El gasto/utilidad será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/gastos-utilidades/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatable.expensesUtilityDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El gasto/utilidad ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el gasto/utilidad"
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
                    url: _app.parseUrl(`/oficina-tecnica/gastos-utilidades/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_titles']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_formulas']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_types']").val(result.budgetTypeId).trigger("change");
                        formElements.find("[name='select_groups']").val(result.group).trigger("change");
                        formElements.find("[name='FixedGeneralExpense']").val(result.fixedGeneralExpense);
                        formElements.find("[name='FixedGeneralPercentage']").val(result.fixedGeneralPercentage);
                        formElements.find("[name='VariableGeneralExpense']").val(result.variableGeneralExpense);
                        formElements.find("[name='VariableGeneralPercentage']").val(result.variableGeneralPercentage);
                        formElements.find("[name='Utility']").val(result.utility);
                        formElements.find("[name='UtilityPercentage']").val(result.utilityPercentage);

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
            excel: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/gastos-utilidades/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.budgetTitle.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='BudgetTypeId'").val($(formElement).find("[name='select_types']").val());
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/gastos-utilidades/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
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
                        datatable.expensesUtilityDt.reload();
                        $("#add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formulas']").val());
                $(formElement).find("[name='BudgetTypeId'").val($(formElement).find("[name='select_types']").val());
                $(formElement).find("[name='Group']").val($(formElement).find("[name='select_groups']").val());
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
                    url: _app.parseUrl(`/oficina-tecnica/gastos-utilidades/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
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
                        datatable.expensesUtilityDt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $(".select2-titles").prop('selectedIndex', 0).trigger("change");
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $(".select2-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-types").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-formulas").prop("selectedIndex", 0).trigger("change");
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
                    forms.reset.edit()
                });
        }
    };

    var select2 = {
        init: function () {
            this.title();
            this.formula();
            this.type();
            this.group();
        },
        title: function () {
            $.ajax({
                url: _app.parseUrl("/select/titulos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-titles").select2({
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
                });
        },
        type: function () {
            _app.loader.show();
            $.ajax({
                url: _app.parseUrl("/select/tipos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-types").select2({
                        data: result
                    });
                    _app.loader.hide();
                });
        },
        group: function () {
            $(".select2-groups").select2({

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
    };
}();

$(function () {
    ExpensesUtility.init();
});