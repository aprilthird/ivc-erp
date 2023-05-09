var Request = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        initComplete: function () {
            select2.attentionStatus.init();
        },
        ajax: {
            url: _app.parseUrl("/logistica/ordenes/compra/listar"),
            dataSrc: "",
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Centro de Costo",
                data: "request.project.costCenter"
            },
            {
                title: "Proyecto",
                data: "request.project.abbreviation"
            },
            {
                title: "Código",
                data: "correlativeCode"
            },
            {
                title: "N° de Requerimiento",
                data: "request.correlativeCodeStr"
            },
            {
                title: "Atención",
                data: "attentionStatus",
                width: "15%",
                render: function (data, type, row) {
                    var tmp = `<select id="${row.id}_status" data-id="${row.id}" class="form-control kt-select select2-attention-status">`;
                    
                    tmp += `<option value='1' ${data == 1 ? "selected" : ""}> POR ATENDER</option>`;
                    tmp += `<option value='2' ${data == 2 ? "selected" : ""}> PARCIAL</option>`;
                    tmp += `<option value='3' ${data == 3 ? "selected" : ""}> TOTAL</option>`;
                    tmp += `<option value='4' ${data == 4 ? "selected" : ""}> ANULADA</option>`;
                    tmp += `<option value='5' ${data == 5 ? "selected" : ""}> MODIFICADA</option>`;
                    tmp += `</select>`;
                    return tmp;
                }
            },
            {
                title: "Fecha",
                data: "date",
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
                title: "Tiempo Transcurrido",
                data: null,
                render: function (data) {
                    return "0 días";
                }
            },
            {
                title: "Observaciones",
                data: "observations"
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
            mainDatatable.on("change", ".select2-attention-status", function () {
                let id = $(this).data("id");
                let value = $(this).val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/ordenes/compra/${id}/actualizar-atencion`),
                    method: "post",
                    data: {
                        status: value
                    }
                }).done(function () {
                    datatable.reload();
                    _app.show.notification.edit.success();
                }).fail(function () {
                    _app.show.notification.edit.error();
                });
            });
        }
    };

    var form = {
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/logistica/requerimientos/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/logistica/requerimientos/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
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
                $("#Add_BudgetType").prop("selectedIndex", 0).trigger("change");
                $("#Add_Type").prop("selectedIndex", 0).trigger("change");
                $("#Add_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_BudgetType").prop("selectedIndex", 0).trigger("change");
                $("#Edit_Type").prop("selectedIndex", 0).trigger("change");
                $("#Edit_SupplyFamilyId").prop("selectedIndex", 0).trigger("change");
            }
        }
    };

    var select2 = {
        init: function () {
            this.budgetTypes.init();
            this.types.init();
            this.supplyFamilies.init();
        },
        attentionStatus: {
            init: function () {
                $(".select2-attention-status").select2();
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
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#supply_family_filter, #supply_group_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Request.init();
});