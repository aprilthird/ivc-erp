var Metered = function () {

    var editForm = null;

    var meteredsDatatable = null;
    var importDataForm = null;

    var meteredsDtOpt = {
        responsive: false,
        sScrollX: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/metrados-replanteo-por-partida/listar"),
            data: function (d) {
                d.workFrontHeadId = $("#project_formula_filter").val();
                d.budgetTitleId = $("#budget_title_filter").val();
                d.workFrontId  = $("#budget_type_filter").val();
                d.sewerGroupId = $("#budget_group_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        ordering: false,
        columns: [
            {
                //tile: "Item",
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
                //tile: "Metrado Neto",
                width: "5%",
                data: function (result) {
                    if (result.metered == "0.00")
                        return "";
                    else
                        return result.metered;
                }
            },
            {
                //tile: "P. U.",
                width: "10%",
                data: function (result) {
                    if (result.unitPrice == "0.00")
                        return "";
                    else
                        return result.unitPrice;
                }
            },
            {
                //tile: "Parcial",
                width: "5%",
                data: function (result) {
                    if (result.parcial == "0.00")
                        return "";
                    else
                        return result.parcial;
                }
            },
            {
                //tile: "F5/6-C01Metered",
                data: function (result) {
                    return result.f5_C01Metered;
                }
            },
            {
                //tile: "F5/6-C01Amount",
                data: function (result) {
                    return result.f5_C01Amount;
                }
            },
            {
                //tile: "F5/6-C01AMetered",
                data: function (result) {
                    return result.f5_C01AMetered;
                }
            },
            {
                //tile: "F5/6-C01AAmount",
                data: function (result) {
                    return result.f5_C01AAmount;
                }
            },
            {
                //tile: "F5/6-C02Metered",
                data: function (result) {
                    return result.f5_C02Metered;
                }
            },
            {
                //tile: "F5/6-C02Amount",
                data: function (result) {
                    return result.f5_C02Amount;
                }
            },
            {
                //tile: "F5/6-C02AMetered",
                data: function (result) {
                    return result.f5_C02AMetered;
                }
            },
            {
                //tile: "F5/6-C02AAmount",
                data: function (result) {
                    return result.f5_C02AAmount;
                }
            },
            {
                //tile: "F5/6-C03Metered",
                data: function (result) {
                    return result.f5_C03Metered;
                }
            },
            {
                //tile: "F5/6-C03Amount",
                data: function (result) {
                    return result.f5_C03Amount;
                }
            },
            {
                //tile: "F5/6-C03AMetered",
                data: function (result) {
                    return result.f5_C03AMetered;
                }
            },
            {
                //tile: "F5/6-C03AAmount",
                data: function (result) {
                    return result.f5_C03AAmount;
                }
            },
            {
                //tile: "F5/6-C04Metered",
                data: function (result) {
                    return result.f5_C04Metered;
                }
            },
            {
                //tile: "F5/6-C04Amount",
                data: function (result) {
                    return result.f5_C04Amount;
                }
            },
            {
                //tile: "F5/6-C04AMetered",
                data: function (result) {
                    return result.f5_C04AMetered;
                }
            },
            {
                //tile: "F5/6-C04AAmount",
                data: function (result) {
                    return result.f5_C04AAmount;
                }
            },
            {
                //tile: "F5/6-C05Metered",
                data: function (result) {
                    return result.f5_C05Metered;
                }
            },
            {
                //tile: "F5/6-C05Amount",
                data: function (result) {
                    return result.f5_C05Amount;
                }
            },
            {
                //tile: "F5/6-C05AMetered",
                data: function (result) {
                    return result.f5_C05AMetered;
                }
            },
            {
                //tile: "F5/6-C05AAmount",
                data: function (result) {
                    return result.f5_C05AAmount;
                }
            },
            {
                //tile: "F5/6-C06Metered",
                data: function (result) {
                    return result.f5_C06Metered;
                }
            },
            {
                //tile: "F5/6-C06Amount",
                data: function (result) {
                    return result.f5_C06Amount;
                }
            },
            {
                //tile: "F5/6-C06AMetered",
                data: function (result) {
                    return result.f5_C06AMetered;
                }
            },
            {
                //tile: "F5/6-C06AAmount",
                data: function (result) {
                    return result.f5_C06AAmount;
                }
            },
            {
                //tile: "F5/6-C07Metered",
                data: function (result) {
                    return result.f5_C07Metered;
                }
            },
            {
                //tile: "F5/6-C07Amount",
                data: function (result) {
                    return result.f5_C07Amount;
                }
            },
            {
                //tile: "F5/6-C07AMetered",
                data: function (result) {
                    return result.f5_C07AMetered;
                }
            },
            {
                //tile: "F5/6-C07AAmount",
                data: function (result) {
                    return result.f5_C07AAmount;
                }
            },
            {
                //tile: "F5/6-C08Metered",
                data: function (result) {
                    return result.f5_C08Metered;
                }
            },
            {
                //tile: "F5/6-C08Amount",
                data: function (result) {
                    return result.f5_C08Amount;
                }
            },
            {
                //tile: "F5/6-C08AMetered",
                data: function (result) {
                    return result.f5_C08AMetered;
                }
            },
            {
                //tile: "F5/6-C08AAmount",
                data: function (result) {
                    return result.f5_C08AAmount;
                }
            },
            {
                //tile: "F5/6-C09Metered",
                data: function (result) {
                    return result.f5_C09Metered;
                }
            },
            {
                //tile: "F5/6-C09Amount",
                data: function (result) {
                    return result.f5_C09Amount;
                }
            },
            {
                //tile: "F5/6-C09AMetered",
                data: function (result) {
                    return result.f5_C09AMetered;
                }
            },
            {
                //tile: "F5/6-C09AAmount",
                data: function (result) {
                    return result.f5_C09AAmount;
                }
            },
            {
                //tile: "F5/6-C10Metered",
                data: function (result) {
                    return result.f5_C10Metered;
                }
            },
            {
                //tile: "F5/6-C10Amount",
                data: function (result) {
                    return result.f5_C10Amount;
                }
            },
            {
                //tile: "F5/6-C10AMetered",
                data: function (result) {
                    return result.f5_C10AMetered;
                }
            },
            {
                //tile: "F5/6-C10AAmount",
                data: function (result) {
                    return result.f5_C10AAmount;
                }
            },
            {
                //tile: "F5/6-C11Metered",
                data: function (result) {
                    return result.f5_C11Metered;
                }
            },
            {
                //tile: "F5/6-C11Amount",
                data: function (result) {
                    return result.f5_C11Amount;
                }
            },
            {
                //tile: "F5/6-C11AMetered",
                data: function (result) {
                    return result.f5_C11AMetered;
                }
            },
            {
                //tile: "F5/6-C11AAmount",
                data: function (result) {
                    return result.f5_C11AAmount;
                }
            },
            {
                //tile: "F5/6-C12Metered",
                data: function (result) {
                    return result.f5_C12Metered;
                }
            },
            {
                //tile: "F5/6-C12Amount",
                data: function (result) {
                    return result.f5_C12Amount;
                }
            },
            {
                //tile: "F5/6-C12AMetered",
                data: function (result) {
                    return result.f5_C12AMetered;
                }
            },
            {
                //tile: "F5/6-C12AAmount",
                data: function (result) {
                    return result.f5_C12AAmount;
                }
            },
            {
                //tile: "F5/6-C13Metered",
                data: function (result) {
                    return result.f5_C13Metered;
                }
            },
            {
                //tile: "F5/6-C13Amount",
                data: function (result) {
                    return result.f5_C13Amount;
                }
            },
            {
                //tile: "F5/6-C13AMetered",
                data: function (result) {
                    return result.f5_C13AMetered;
                }
            },
            {
                //tile: "F5/6-C13AAmount",
                data: function (result) {
                    return result.f5_C13AAmount;
                }
            },
            {
                //tile: "F5/6-C14Metered",
                data: function (result) {
                    return result.f5_C14Metered;
                }
            },
            {
                //tile: "F5/6-C14Amount",
                data: function (result) {
                    return result.f5_C14Amount;
                }
            },
            {
                //tile: "F5/6-C14AMetered",
                data: function (result) {
                    return result.f5_C14AMetered;
                }
            },
            {
                //tile: "F5/6-C14AAmount",
                data: function (result) {
                    return result.f5_C14AAmount;
                }
            },
            {
                //tile: "F5/6-C15Metered",
                data: function (result) {
                    return result.f5_C15Metered;
                }
            },
            {
                //tile: "F5/6-C15Amount",
                data: function (result) {
                    return result.f5_C15Amount;
                }
            },
            {
                //tile: "F5/6-C15AMetered",
                data: function (result) {
                    return result.f5_C15AMetered;
                }
            },
            {
                //tile: "F5/6-C15AAmount",
                data: function (result) {
                    return result.f5_C15AAmount;
                }
            },
            {
                //tile: "F5/6-C16Metered",
                data: function (result) {
                    return result.f5_C16Metered;
                }
            },
            {
                //tile: "F5/6-C16Amount",
                data: function (result) {
                    return result.f5_C16Amount;
                }
            },
            {
                //tile: "F5/6-C16AMetered",
                data: function (result) {
                    return result.f5_C16AMetered;
                }
            },
            {
                //tile: "F5/6-C16AAmount",
                data: function (result) {
                    return result.f5_C16AAmount;
                }
            },
            {
                //tile: "F5/6-C17Metered",
                data: function (result) {
                    return result.f5_C17Metered;
                }
            },
            {
                //tile: "F5/6-C17Amount",
                data: function (result) {
                    return result.f5_C17Amount;
                }
            },
            {
                //tile: "F5/6-C17AMetered",
                data: function (result) {
                    return result.f5_C17AMetered;
                }
            },
            {
                //tile: "F5/6-C17AAmount",
                data: function (result) {
                    return result.f5_C17AAmount;
                }
            },
            {
                //tile: "F5/6-C18Metered",
                data: function (result) {
                    return result.f5_C18Metered;
                }
            },
            {
                //tile: "F5/6-C18Amount",
                data: function (result) {
                    return result.f5_C18Amount;
                }
            },
            {
                //tile: "F5/6-C18AMetered",
                data: function (result) {
                    return result.f5_C18AMetered;
                }
            },
            {
                //tile: "F5/6-C18AAmount",
                data: function (result) {
                    return result.f5_C18AAmount;
                }
            },
            {
                //tile: "F5/6-C19Metered",
                data: function (result) {
                    return result.f5_C19Metered;
                }
            },
            {
                //tile: "F5/6-C19Amount",
                data: function (result) {
                    return result.f5_C19Amount;
                }
            },
            {
                //tile: "F5/6-C19AMetered",
                data: function (result) {
                    return result.f5_C19AMetered;
                }
            },
            {
                //tile: "F5/6-C19AAmount",
                data: function (result) {
                    return result.f5_C19AAmount;
                }
            },
            {
                //tile: "F5/6-C20Metered",
                data: function (result) {
                    return result.f5_C20Metered;
                }
            },
            {
                //tile: "F5/6-C20Amount",
                data: function (result) {
                    return result.f5_C20Amount;
                }
            },
            {
                //tile: "F5/6-C20AMetered",
                data: function (result) {
                    return result.f5_C20AMetered;
                }
            },
            {
                //tile: "F5/6-C20AAmount",
                data: function (result) {
                    return result.f5_C20AAmount;
                }
            },
            {
                //tile: "F5/6-C21Metered",
                data: function (result) {
                    return result.f5_C21Metered;
                }
            },
            {
                //tile: "F5/6-C21Amount",
                data: function (result) {
                    return result.f5_C21Amount;
                }
            },
            {
                //tile: "F5/6-C21AMetered",
                data: function (result) {
                    return result.f5_C21AMetered;
                }
            },
            {
                //tile: "F5/6-C21AAmount",
                data: function (result) {
                    return result.f5_C21AAmount;
                }
            },
            {
                //tile: "Metrado Acumulado",
                data: function (result) {
                    return result.accumulatedMetered;
                }
            },
            {
                //tile: "Monto Acumulado",
                data: function (result) {
                    return result.accumulatedAmount;
                }
            },
            {
                //title: "Opciones",
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
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var datatables = {
        init: function () {
            this.meteredsDt.init();
        },
        meteredsDt: {
            init: function () {
                meteredsDatatable = $("#metereds_datatable").DataTable(meteredsDtOpt);
                this.initEvents();
            },
            reload: function () {
                meteredsDatatable.ajax.reload();
            },
            initEvents: function () {

                meteredsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                    });

                meteredsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El metrado replanteo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-partida/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.meteredsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El metrado replanteo ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el metrado replanteo"
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
                    url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-partida/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='select_titles']").val(result.budgetTitleId).trigger("change");
                        formElements.find("[name='select_formulas']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='select_types']").val(result.budgetTypeId).trigger("change");
                        formElements.find("[name='select_groups']").val(result.group).trigger("change");
                        formElements.find("[name='NumberItem']").val(result.numberItem);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='select_units']").val(result.unit);
                        formElements.find("[name='Metered']").val(result.metered);
                        formElements.find("[name='UnitPrice']").val(result.unitPrice);
                        formElements.find("[name='TotalPrice']").val(result.totalPrice);
                        formElements.find("[name='ContractualMO']").val(result.contractualMO);
                        formElements.find("[name='ContractualEQ']").val(result.contractualEQ);
                        formElements.find("[name='ContractualSubcontract']").val(result.contractualSubcontract);
                        formElements.find("[name='ContractualMaterials']").val(result.contractualMaterials);
                        formElements.find("[name='CollaboratorMO']").val(result.collaboratorMO);
                        formElements.find("[name='CollaboratorEQ']").val(result.collaboratorEQ);

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
                        url: _app.parseUrl("/oficina-tecnica/metrados-replanteo-por-partida/crear"),
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
                            datatables.meteredsDt.reload();
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
                    $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_work_front-heads']").val());
                    $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                    $(formElement).find("[name='WorkFrontId'").val($(formElement).find("[name='select_work_fronts']").val());
                    $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_group']").val());
                    $(formElement).find("[name='Unit']").val($(formElement).find("[name='select_units']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-partida/editar/${id}`),
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
                            datatables.meteredsDt.reload();
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
                import: {
                    data: function (formElement) {
                        $(formElement).find("[name='WorkFrontHeadId']").val($(formElement).find("[name='select_work_front_heads']").val());
                        $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                        $(formElement).find("[name='WorkFrontId'").val($(formElement).find("[name='select_work_fronts']").val());
                        $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_groups']").val());
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
                            url: "/oficina-tecnica/metrados-replanteo-por-partida/importar",
                            type: "POST",
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
                        }).always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input").prop("disabled", false);
                            $(".progress").empty().remove();
                            if (!emptyFile) {
                                $("#space-bar").remove();
                            }
                        }).done(function (result) {
                            datatables.meteredsDt.reload();
                            $("#import_data_modal").modal("hide");
                            _app.show.notification.addRange.success();
                        }).fail(function (error) {
                            if (error.responseText) {
                                $("#import_data_alert_text").html(error.responseText);
                                $("#import_data_alert").removeClass("d-none").addClass("show");
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
                $(".select2-work-front-heads").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-work-front-heads").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                    $(".select2-work-front-heads").prop("selectedIndex", 0).trigger("change");
                    $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                    $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                    $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
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

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
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
            this.workfronthead();
            this.title();
            this.workfront();
            this.sewergroup();
            this.filters();
        },
        title: function () {
            $.ajax({
                url: _app.parseUrl("/select/general-expenses-titulos-de-presupuesto")
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
        workfronthead: function () {
            $.ajax({
                url: _app.parseUrl("/select/jefes-de-frente")
            })
                .done(function (result) {
                    $(".select2-work-front-heads").select2({
                        data: result
                    });
                    $(".select2-workfronthead-filter").select2({
                        data: result
                    });
                });
        },
        workfront: function () {
            $.ajax({
                url: _app.parseUrl("/select/frentes")
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
        sewergroup: function () {
            $.ajax({
                url: _app.parseUrl("/select/cuadrillas-f5")
            })
                .done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                    $(".select2-sewergroup-filter").select2({
                        data: result
                    });
                });
        },
        filters: function () {
            $("#work_front_head_filter").on("change", function () {
                datatables.meteredsDt.reload();
            });
            $("#budget_title_filter").on("change", function () {
                datatables.meteredsDt.reload();
            });
            $("#work_front_filter").on("change", function () {
                datatables.meteredsDt.reload();
            });
            $("#sewer_group_filter").on("change", function () {
                datatables.meteredsDt.reload();
            });
        }
    };

    var events = {
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/metrados-replanteo-por-partida/excel-carga-masiva`;
            });
        },
    };

    return {
        init: function () {
            events.excel();
            select2.init();
            datatables.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Metered.init();
});