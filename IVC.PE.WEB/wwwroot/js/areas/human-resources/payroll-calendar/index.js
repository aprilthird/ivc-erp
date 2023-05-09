var Calendar = function () {

    var addForm = null;
    var addweekForm = null;
    var editweekForm = null;
    var searchForm = null;
    var projectCalendarWeeks = null;
    var projectCalendarDataTable = null;

    var calendarId = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/calendario-semanal/listar"),
            dataSrc: "",
            data: function (d) {
                d.projectId = $("#project_filter").val();
                d.year = $("#year_filter").val();
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Descripcion",
                data: "description"
            },
            {
                title: "Del",
                data: "weekStart"
            },
            {
                title: "Al",
                data: "weekEnd"
            },
            {
                title: "Estado",
                data: "isClosed",
                render: function (data, type, row) {
                    return (data == true) ? '<div class="kt-demo-icon"><div class="kt-demo-icon__preview" ><i class="la la-unlock"></i></div></div>' :
                                            '<div class="kt-demo-icon"><div class="kt-demo-icon__preview" ><i class="la la-unlock-alt"></i></div></div>';
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    if (meta.row == 0) {
                        calendarId = row.projectCalendarId;
                    }
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
            projectCalendarDataTable = $("#project_calendar_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            projectCalendarDataTable.clear().draw();
            projectCalendarDataTable.ajax.reload();
        },
        initEvents: function () {
            projectCalendarDataTable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.editweek(id);
                });

            projectCalendarDataTable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La semana será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/recursos-humanos/calendario-semanal/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "La semana ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar la semana"
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
            editweek: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/calendario-semanal/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#week_edit_form");
                        formElements.find("[name='Id'").val(result.id);
                        formElements.find("[name='ProjectCalendarId'").val(result.projectCalendarId);
                        formElements.find("[name='Description']").val(result.description);
                        formElements.find("[name='IsClosed']").val(result.isClosed.toString()).trigger("change");
                        formElements.find("[name='WeekNumber']").val(result.weekNumber);
                        formElements.find("[name='ProcessType']").val(result.processType).trigger("change");
                        formElements.find("[name='WeekStart']").datepicker("setDate", result.weekStart);
                        formElements.find("[name='WeekEnd']").datepicker("setDate", result.weekEnd);
                        formElements.find("[name='Month']").val(result.month).trigger("change");
                        $("#week_edit_modal").modal("show");
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
                    url: _app.parseUrl("/recursos-humanos/calendario-semanal/generar"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
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
            addweek: function (formElement) {
                $(formElement).find("[name='ProjectCalendarId']").val(calendarId);
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/recursos-humanos/calendario-semanal/crear"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
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
            editweek: function (formElement) {
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/calendario-semanal/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.reload();
                        $("#week_edit_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#week_edit_alert_text").html(error.responseText);
                            $("#week_edit_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            addweek: function () {
                addweekForm.resetForm();
                $("#addweek_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            editweek: function () {
                editweekForm.resetForm();
                $("#week_edit_form").trigger("reset");
                $("#week_edit_alert").removeClass("show").addClass("d-none");
            }
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
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });
            addWeekForm = $("#week_add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.addweek(formElement);
                }
            });
            editWeekForm = $("#week_edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.editweek(formElement);
                }
            })
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#addweek_modal").on("hidden.bs.modal",
                function () {
                    form.reset.addweek();
                });

            $("#editweek_modal").on("hidden.bs.modal",
                function () {
                    form.reset.editweek();
                });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#year_filter").focusout(function () {
                datatable.reload();
            });
            $("#project_filter").on("change", function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            events.init();
            modals.init();
            select2.init();
            validate.init();
            datepicker.init();
            datatable.init();
        }
    };
}();

$(function () {
    Calendar.init();
});