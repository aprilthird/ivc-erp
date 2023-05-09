var For05 = function () {

    var addForm = null;
    var editForm = null;
    var detailForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;
    var selectSGOption = new Option('--Seleccione un Tipo de Equipo--', null, true, true);
    var for05Datatable = null;
    var foldingDatatable = null;

    var importDataForm = null;

    var equipId = null;

    var for05DtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/equipos/fase-transporte/listar"),
            data: function (d) {

                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Código",
                data: "projectPhase.code"
            },
            {
                title: "Descripción",
                data: "projectPhase.description"
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

    var datatables = {
        init: function () {
            this.for05Dt.init();

        },
        for05Dt: {
            init: function () {
                for05Datatable = $("#main_datatable").DataTable(for05DtOpt);
                this.initEvents();
            },
            reload: function () {
                for05Datatable.ajax.reload();
            },
            initEvents: function () {

                for05Datatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });


                for05Datatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La Fase de equipo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/equipos/fase-transporte/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for05Dt.reload();

                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La Fase de Equipo ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar la Fase de Equipo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },

    };
    var forms = {
        load: {
            edit: function (id) {

                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/equipos/fase-transporte/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");

                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectPhaseId']").val(result.projectPhaseId);
                        formElements.find("[name='select_phase']").val(result.projectPhaseId).trigger("change");

                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },



        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phase']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/equipos/fase-transporte/crear"),
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
                        datatables.for05Dt.reload();
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
                $(formElement).find("[name='ProjectPhaseId']").val($(formElement).find("[name='select_phase']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();

                $(formElement).find("input").prop("disabled", true);

                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/equipos/fase-transporte/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data,

                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for05Dt.reload();
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


        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");

                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");

            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
            },
            addfolding: function () {
                addFoldingForm.reset();
                $("#add_folding_form").trigger("reset");
                $("#add_folding_alert").removeClass("show").addClass("d-none");
            },
            editfolding: function () {
                editFoldingForm.reset();
                $("#edit_folding_form").trigger("reset");
                $("#edit_folding_alert").removeClass("show").addClass("d-none");
                datatables.foldingDt.reload();
                $("#detail_modal").modal("show");
            },

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

            detailForm = $("#detail_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });


            addFoldingForm = $("#add_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addfolding(formElement);
                }
            });

            editFoldingForm = $("#edit_folding_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editfolding(formElement);
                }
            });




        }
    };

    var select2 = {
        init: function () {
            this.styles.init();
            this.phases.init();
            this.providers.init();
            this.equipproviders2.init();
            this.softs.init();
            this.machineries.init();
            this.transports.init();
            this.machinerys.init();
            this.machinerys2.init();
            this.types.init();
        },
        phases: {
            init: function () {
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/fases-proyecto?projectId=${pId}`)
                }).done(function (result) {
                    $(".select2-phases").select2({
                        data: result
                    });

                });
            }
        },
        types: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-transporte-mixto`)
                }).done(function (result) {
                    $(".select2-types").select2({
                        data: result
                    });
                });
            },

            reload: function (id) {
                $.ajax({
                    url: _app.parseUrl(`/select/tipos-de-transporte-mixto-por-clase-folding?equipmentMachineryTypeId=${id}`)
                }).done(function (result) {

                    $(".select2-types").empty();
                    $(".select2-types").append(selectSGOption).trigger('change');
                    $(".select2-types").select2({
                        data: result
                    });
                });
            },


        },
        softs: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-transporte-liviano")
                }).done(function (result) {
                    $(".select2-softs").select2({
                        data: result
                    });
                });
            }
        },
        transports: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-transporte-transporte")
                }).done(function (result) {
                    $(".select2-transports").select2({
                        data: result
                    });
                });
            }
        },

        machinerys2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-transporte-lista")
                }).done(function (result) {
                    $(".select2-machinerys2").select2({
                        data: result
                    });
                });
            }
        },

        machineries: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-transporte-transporte")
                }).done(function (result) {
                    $(".select2-machineries").select2({
                        data: result
                    });
                });
            }
        },
        machinerys: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-transporte-lista")
                }).done(function (result) {
                    $(".select2-machinerys").select2({
                        data: result
                    });
                });
            }
        },
        providers: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/lista-proveedores")
                }).done(function (result) {
                    $(".select2-providers").select2({
                        data: result
                    });
                });
            }
        },

        equipproviders2: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/fase-transporte")
                }).done(function (result) {
                    $(".select2-equipproviders2").select2({
                        data: result
                    });
                });
            }
        },

        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
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

            $("#add_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addfolding();
                });

            $("#edit_folding_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editfolding();
                });



        }
    };
    var events = {
        init: function () {


        },

    };
    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
            }).datepicker('setDate', 'today')
        }
    };

    //var events = {


    //};

    return {
        init: function () {
            select2.init();
            datepicker.init();
            datatables.init();
            validate.init();
            modals.init();
            events.init();

        }
    };
}();

$(function () {
    For05.init();
});