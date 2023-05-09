var Supply = function () {

    var supplyDatatable = null;
    var detailDatatable = null;
    var addForm = null;
    var editForm = null;
    var importForm = null;
    var detailForm = null;

    var Id = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/logistica/insumos/listar"),
            data: function (d) {
                d.measurementUnitId = $("#measurement_unit_filter").val();
                d.supplyFamilyId = $("#supply_family_filter").val();
                d.supplyGroupId = $("#supply_group_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código de Artículo IVC",
                data: "fullCode"
            },
            {
                title: "Descripción de Artículo",
                data: "description"
            },
            {
                title: "Unidad",
                data: "measurementUnit.abbreviation"
            },
            {
                title: "Familia",
                data: "supplyFamily.fullName"
            },
            {
                title: "Grupo",
                data: "supplyGroup.fullName"
            },
            {
                title: "Correlativo",
                data: "correlativeCodeString"
            },
            {
                title: "Estado",
                data: "status",
                render: function (data) {
                    if (data == 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--light">SIN ORDEN</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">EN USO EN ORDENES</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.status == 1) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-detail">`;
                        tmp += `<i class="fa fa-th-list"></i></button> `;
                    }
                    return tmp;
                }
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


    var detailOptions = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/logistica/insumos/ordenes/listar"),
            data: function (d) {
                d.id = Id;
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Órden",
                data: "correlativeCodeStr"
            },
            {
                title: "Estado",
                data: "status",
                render: function (data) {
                    if (data == 1) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">PRE-EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 2) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">EMITIDO</span>
								</label>
							</span>`;
                    } else if (data == 3) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">APROBADA</span>
								</label>
							</span>`;
                    } else if (data == 8) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--success">PARCIALMENTE APROBADA</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">ANULADA</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Proveedor",
                data: "provider.tradename"
            },
            {
                title: "Moneda",
                data: function (result) {
                    if (result.currency == 1)
                        return "Soles";
                    else
                        return "Dólares";
                }
            },
            {
                title: "Tipo de Cambio",
                data: "exchangeRate"
            },
            {
                title: "Parcial Soles",
                data: "parcial"
            },
            {
                title: "Parcial Dólares",
                data: "dolarParcial"
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [4, 5, 6] }
        ]
    };

    var datatable = {
        init: function () {
            supplyDatatable = $("#supply_datatable").DataTable(options);
            detailDatatable = $("#detail_datatable").DataTable(detailOptions);
            this.initEvents();
        },
        reload: function () {
            supplyDatatable.ajax.reload();
        },
        detailReload: function () {
            detailDatatable.ajax.reload();
        },
        initEvents: function () {
            supplyDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            supplyDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El insumo será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/logistica/insumos/eliminar`),
                                    type: "delete",
                                    data: {
                                        id: id
                                    },
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El insumo ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar al insumo"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });



            supplyDatatable.on("click",
                ".btn-detail",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Id = id;
                    datatable.detailReload();
                    $("#detail_modal").modal("show");
                });

        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/logistica/insumos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='MeasurementUnitId']").val(result.measurementUnitId).trigger("change");
                        formElements.find("[name='SupplyFamilyId']").val(result.supplyFamilyId).trigger("change");
                        formElements.find("[name='SupplyGroupId']").val(result.supplyGroupId).trigger("change");
                        formElements.find("[name='CorrelativeCode']").val(result.correlativeCode);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/logistica/insumos/crear"),
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
                    url: _app.parseUrl(`/logistica/insumos/editar/${id}`),
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
                    url: "/logistica/insumos/importar",
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
                    datatable.reload();
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
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $(".select2-supply-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-supply-families").prop("selectedIndex", 0).trigger("change");
                $(".select2-measurement-units").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-supply-groups").prop("selectedIndex", 0).trigger("change");
                $(".select2-supply-families").prop("selectedIndex", 0).trigger("change");
                $(".select2-measurement-units").prop("selectedIndex", 0).trigger("change");
            },
            import: function () {
                importForm.resetForm();
                $("#import_form").trigger("reset");
                $("#import_alert").removeClass("show").addClass("d-none");
            },
            detail: function () {
                detailForm.resetForm();
                $("#detail_form").trigger("reset");
                $("#detail_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.measurementUnits.init();
            this.supplyFamilies.init();
            this.supplyGroups.add();
            this.supplyGroups.edit();
            this.supplyGroups.filter();
        },
        measurementUnits: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/unidades-de-medida")
                }).done(function (result) {
                    $(".select2-measurement-units").select2({
                        data: result
                    });
                    $(".select2-measurement-unit-filter").select2({
                        data: result
                    });
                });
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
                    $(".select2-supply-familiy-filter").select2({
                        data: result
                    });
                });
            }
        },
        supplyGroups: {
            filter: function () {
                $(".select2-supply-group-filter").empty();
                $(".select2-supply-group-filter").append(`<option>Todos</option>`);
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#supply_family_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $(".select2-supply-group-filter").select2({
                        data: result
                    });
                });
            },
            add: function () {
                $("#add_form").find(".select2-supply-groups").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#add_form").find("[name='SupplyFamilyId']").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                     $("#add_form").find(".select2-supply-groups").select2({
                        data: result
                    });
                });
            },
            edit: function () {
                $("#edit_form").find(".select2-supply-groups").empty();
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-insumos-filtro"),
                    data: {
                        supplyFamilyId: $("#edit_form").find("[name='SupplyFamilyId']").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#edit_form").find(".select2-supply-groups").select2({
                        data: result
                    });
                });
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

            importForm = $("#import_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.import(formElement);
                }
            });

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });
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

            $("#import_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import();
                });

            $("#detail_modal").on("hidden.bs.modal",
                function () {
                    form.reset.detail();
                });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='MeasurementUnitId']").attr("id", "Add_MeasurementUnitId");
            $("#edit_form [name='MeasurementUnitId']").attr("id", "Edit_MeasurementUnitId");
            $("#add_form [name='SupplyFamilyId']").attr("id", "Add_SupplyFamilyId");
            $("#edit_form [name='SupplyFamilyId']").attr("id", "Edit_SupplyFamilyId");
            $("#add_form [name='SupplyGroupId']").attr("id", "Add_SupplyGroupId");
            $("#edit_form [name='SupplyGroupId']").attr("id", "Edit_SupplyGroupId");

            $("#measurement_unit_filter, #supply_family_filter").on("change", function () {
                datatable.reload();
                select2.supplyGroups.filter();
            });

            $("#add_form").find(".select2-supply-families").on("change", function () {
                console.log($(".select2-supply-families").val());
                select2.supplyGroups.add();
            });

            $("#edit_form").find(".select2-supply-families").on("change", function () {
                console.log($(".select2-supply-families").val());
                select2.supplyGroups.edit();
            });

            $("#supply_group_filter").on("change", function () {
                datatable.reload();
            });

            $("#genEsxelSample").on("click", function () {
                window.location = _app.parseUrl(`/logistica/insumos/excel-modelo`);
            });
            /*
            $.ajax({
                url: _app.parseUrl(`/logistica/insumos/actulizar`),
                method: "put"
            }).done(function () {
                console.log("listo");
            });
            */
            this.deleteByFilters();
        },
        deleteByFilters: function () {
            $("#btn-delete-by-filters").on("click", function () {
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Los insumos filtrados serán eliminados permanentemente",
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
                                url: _app.parseUrl(`/logistica/insumos/eliminar-filtro`),
                                data: {
                                    supplyFamilyId: $("#supply_family_filter").val(),
                                    supplyGroupId: $("#supply_group_filter").val()
                                },
                                type: "delete",
                                success: function (result) {
                                    datatable.reload();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "Los insumos han sido eliminados con éxito",
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
            /*
            $.ajax({
                url: _app.parseUrl("/logistica/insumos/actulizar"),
                method: "put"
            })
                .done(function (result) {
                    datatable.reload();
                    _app.show.notification.add.success();
                    console.log(result);
                });
                */
        },
        export: function () {

        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datatable.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Supply.init();
});