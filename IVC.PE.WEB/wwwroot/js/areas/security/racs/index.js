var Racs = function () {

    var racsDatatable = null;
    var confDatatable = null;

    var addConfigForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/racs/listar"),
            dataSrc: "",
            data: function (d) {
                d.year = $("#filter_year").val();
                d.month = $("#filter_month").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Proyecto",
                data: "projectAbbr"
            },
            {
                title: "Fecha",
                data: "reportDateStr"
            },
            {
                title: "Estado",
                data: "status",
                render: function (data, type, row) {
                    return (data == 0) ?
                        '<span class="kt-badge kt-badge--info kt-badge--inline">Pendiente</span>' :
                        '<span class="kt-badge kt-badge--success kt-badge--inline">Levantado</span>';
                }
            },
            {
                title: "Reporta",
                data: "reportUser"
            },
            {
                title: "Responsable de levantar Observaciones",
                data: "liftUser"
            },
            {
                title: "Frente de Trabajo",
                data: "workFrontCode"
            },
            {
                title: "Cuadrilla",
                data: "sewerGroupCode"
            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    tmp += `<button data-id="${row.id}" data-name="${row.code}" class="btn btn-info btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-file-pdf"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [],
        initComplete: function (settings, json) {
            events.racsNum();
        }
    };

    var confOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/seguridad/conf-racs/listar"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Proyecto",
                data: "project.abbreviation"
            },
            {
                title: "Código RACS",
                data: "racsCode"
            },
            {
                title: "Versión",
                data: "versionCode"
            },
            {
                title: "Opciones",
                data: null,
                width: "10%",
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus'></i> Nuevo",
                className: " btn-dark",
                action: function (e, dt, node, config) {
                    $("#config_modal").modal("hide");
                    $("#add_config_modal").modal("show");
                }
            }
        ]
    }; 

    var datatable = {
        init: function () {
            this.racsdt.init();
            this.confdt.init();
        },
        racsdt: {
            init: function () {
                racsDatatable = $("#racs_datatable").DataTable(options);
                this.events();
            },
            reload: function () {
                racsDatatable.ajax.reload(function () {
                    events.racsNum();
                });
            },
            events: function () {
                racsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El RACS será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/seguridad/racs/eliminar`),
                                        type: "delete",
                                        data: {
                                            id: id
                                        },
                                        success: function (result) {
                                            datatable.racsdt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El RACS ha sido eliminado con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el RACS"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                racsDatatable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        let name = $btn.data("name");

                        let bUrl = _app.parseUrl(`/seguridad/racs/generar-pdf/${id}`);
                        window.open(`/seguridad/racs/descargar-pdf?url=${bUrl}&filename=${name}`);
                        
                    });

                racsDatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        window.open(_app.parseUrl(`/seguridad/racs/generar-pdf/${id}`), '_blank');
                    });
            }
        },
        confdt: {
            init: function () {
                confDatatable = $("#configs_datatable").DataTable(confOpt);
            },
            reload: function () {
                confDatatable.ajax.reload();
            }
        }
    };

    var form = {
        submit: {
            addConfig: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='select_project']").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $(formElement).find("input").prop("disabled", true);
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/seguridad/conf-racs/crear`),
                    type: "post",
                    data: data
                }).always(function () {
                    $(formElement).find("input").prop("disabled", false);
                    $btn.removeLoader();
                })
                    .done(function (result) {
                        $("#add_config_modal").modal("hide");
                        $("#config_modal").modal("show");
                        datatable.confdt.reload();
                        _app.show.notification.add.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#add_config_alert_text").html(error.responseText);
                            $("#add_config_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            addConfig: function () {
                addConfigForm.resetForm();
                $("#add_config_form").trigger("reset");
                $("#add_config_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            addConfigForm = $("#add_config_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addConfig(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_config_modal").on("hidden.bs.modal",
                function () {
                    form.reset.addConfig();
                });
        }
    };

    var select2 = {
        init: function () {
            this.months.init();
            this.projects.init();
        },
        months: {
            init: function () {
                $("#filter_month").select2();
            }
        },
        projects: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/proyectos")
                }).done(function (result) {
                    $(".select2-projects").select2({
                        data: result
                    });
                });
            }
        }
    };

    var events = {
        init: function () {
            $("#filter_month, #filter_year").on("change", function () {
                datatable.racsdt.reload();
            });
        },
        racsNum: function () {
            $("#racs_total").text(racsDatatable.data().count());
        }
    }

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
            events.init();
        }
    }
}();

$(function () {
    Racs.init();
});