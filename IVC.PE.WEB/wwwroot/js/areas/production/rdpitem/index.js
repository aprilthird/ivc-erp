var RdpItems = function () {

    var selectSGOption = new Option('--Seleccione una Cuadrilla--', null, true, true);
    var selectPPOption = new Option('--Seleccione una Fase--', null, true, true);

    var ppId = null;

    var editForm = null;
    var importForm = null;

    var itemsDt = null;

    var itemsOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/produccion/items-rdp/listar"),
            data: function (d) {
                d.ppId = ppId;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Fase Principal",
                data: "projectPhase.fullDescription"
            },
            {
                title: "Grupo",
                data: "itemGroupStr"
            },
            {
                title: "Descripción",
                data: "itemDescription"
            },
            {
                title: "Unidad",
                data: "itemUnit"
            },
            {
                title: "Metrado Contractual",
                data: "itemContractualAmmount"
            },
            {
                title: "Metrado Replanteo",
                data: "itemStakeOutAmmount"
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
        ],
        rowGroup: {
            dataSrc: "itemGroupStr"
        }
    };

    var datatables = {
        init: function () {
            this.items.init();
        },
        items: {
            init: function () {
                itemsDt = $("#items_datatable").DataTable(itemsOpt);
                this.events();
            },
            reload: function () {
                itemsDt.clear().draw();
                itemsDt.ajax.reload();
            },
            events: function () {
                itemsDt.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });
            }
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/produccion/items-rdp/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId);
                        formElements.find("[name='select_projectphase']").val(result.projectPhaseId).trigger("change");
                        formElements.find("[name='ItemGroup']").val(result.itemGroup);
                        formElements.find("[name='select_itemgroup']").val(result.itemGroup).trigger("change");
                        formElements.find("[name='ItemPhaseCode']").val(result.itemPhaseCode);
                        formElements.find("[name='ItemDescription']").val(result.itemDescription);
                        formElements.find("[name='ItemUnit']").val(result.itemUnit);
                        formElements.find("[name='ItemContractualAmmount']").val(result.itemContractualAmmount);
                        formElements.find("[name='ItemStakeOutAmmount']").val(result.itemStakeOutAmmount);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                {
                    $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_projectphase']").val());
                    $(formElement).find("[name='ItemGroup']").val($(formElement).find("[name='select_itemgroup']").val());
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/produccion/items-rdp/editar/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            $btn.removeLoader();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            dataTables.items.reload();
                            $("#edit_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#edit_alert_text").html(error.responseText);
                                $("#edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                }
            },
            import: function (formElement) {
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
                    url: "/produccion/items-rdp/importar",
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
                    datatables.items.reload();
                    $("#import_modal").modal("hide");
                    _app.show.notification.addRange.success();
                }).fail(function (error) {
                    if (error.responseText) {
                        $("#import_alert_text").html(error.responseText);
                        $("#import_alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.addRange.error();
                });
            }
        },
        reset: {
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            import: function () {
                importForm.reset();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var modals = {
        init: function () {
            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import();
                });
        }
    };

    var validate = {
        init: function () {
            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import(formElement);
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.workfrontheads.init();
            this.sewergroups.init();
            this.phases.init();
            this.itemgroups.init();
        },
        workfrontheads: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/jefes-de-frente`)
                }).done(function (result) {
                    $(".select2-workfrontheads").select2({
                        data: result
                    });
                })
            }
        },
        sewergroups: {
            init: function () {
                $(".select2-sewergroups").select2();
            },
            reload: function () {
                let wfh = $("#workfronthead_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/cuadrillas-jefe-frente?workFrontHeadId=${wfh}`)
                }).done(function (result) {
                    $(".select2-sewergroups").empty();
                    $(".select2-sewergroups").append(selectSGOption).trigger('change');
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            }
        },
        phases: {
            init: function () {
                $(".select2-projectphases").select2();
            },
            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/fases-proyecto-cuadrilla?sgId=${id}`)
                }).done(function (result) {
                    $(".select2-projectphases").empty();
                    $(".select2-projectphases").append(selectPPOption).trigger('change');
                    $(".select2-projectphases").select2({
                        data: result
                    });
                })
            }
        },
        itemgroups: {
            init: function () {
                $(".select2-itemgroups").select2();
            }
        }
    };

    var events = {
        init: function () {
            $("#workfronthead_filter").on("change", function () {
                select2.sewergroups.reload();
            });

            $("#sewergroup_filter").on("change", function () {
                let sgId = $("#sewergroup_filter").val();
                select2.phases.reload(sgId);
            });

            $("#search_button").on("click", function () {
                ppId = $("#projectphase_filter").val();
                datatables.items.reload();
            });

            $("#download_excel").on("click", function () {
                window.location = `/produccion/items-rdp/excel-carga`;
            });
        }
    };

    return {
        init: function () {
            datatables.init();
            modals.init();
            validate.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    RdpItems.init();
});