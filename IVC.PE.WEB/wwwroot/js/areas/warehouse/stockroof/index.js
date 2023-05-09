var StockRoof = function () {
    var stocksDt = null;

    var addForm = null;
    var editForm = null;

    var selectAllOption = new Option('--Cuadrillas--', null, true, true);

    var stocksDtOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/almacenes/techos/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "stock.code"
            },
            {
                title: "Descripción",
                data: "stock.description"
            },
            {
                title: "Unidades",
                data: "stock.unit"
            },
            {
                title: "Cantidad (Techo)",
                data: "roofQuantity"
            },
            {
                title: "Fase",
                data: "projectPhase.fullDescription"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroup.code"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatables = {
        init: function () {
            this.stocks.init();
        },
        stocks: {
            init: function () {
                stocksDt = $("#stockroofs_datatable").DataTable(stocksDtOptions);
                this.events();
            },
            reload: function () {
                stocksDt.ajax.reload();
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
                    url: _app.parseUrl(`/almacenes/techos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='StockId']").val(result.stockId)
                        formElements.find("[name='select_stock']").val(result.stockId).trigger("change");
                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId)
                        formElements.find("[name='select_sewergroup']").val(result.sewerGroupId).trigger("change");
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId)
                        formElements.find("[name='select_projectphase']").val(result.projectPhaseId).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='StockId']").val($(formElement).find("[name='select_stock']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_projectphase']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl("/almacenes/techos/crear"),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_modal").modal("hide");
                        datatables.stocks.reload();
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
                $(formElement).find("[name='StockId']").val($(formElement).find("[name='select_stock']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewergroup']").val());
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_projectphase']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                let id = formElement.find("[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/almacenes/techos/editar/${id}`),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#edit_modal").modal("hide");
                        datatables.stocks.reload();
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
            this.stocks.init();
            this.phases.init();
            this.workfrontheads.init();
            this.sewergroups.init();
        },
        stocks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/materiales")
                }).done(function (result) {
                    $(".select2-stocks").select2({
                        data: result
                    });
                })
            }
        },
        phases: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fases-proyecto")
                }).done(function (result) {
                    $(".select2-projectphases").select2({
                        data: result
                    });
                })
            }
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-workfrontheads").select2({
                        data: result
                    });
                    select2.sewergroups.reload();
                })
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
            },
            reload: function () {
                let wfh = $("[name ='select_workfronthead']").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectAllOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                })
            }
        }
    };

    var events = {
        init: function () {
            $("[name ='select_workfronthead']").on("change", function () {
                select2.sewergroups.reload();
            });
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
                    forms.reset.edit();
                });
        }
    };

    return {
        init: function () {
            datatables.init();
            select2.init();
            validate.init();
            modals.init();
            events.init();
        }
    };
}();

$(function () {
    StockRoof.init();
});