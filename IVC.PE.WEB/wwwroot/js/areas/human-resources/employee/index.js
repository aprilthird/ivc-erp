var Employee = function () {

    var employeesDatatable = null;

    var importNewEntryModal = null;

    var employeesDtOpts = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/recursos-humanos/empleados/listar"),
            data: function (d) {
                d.workArea = $("#work_area_filter").val();
                d.entryPositionId = $("#entry_position_filter").val();
                d.currentPositionId = $("#current_position_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Apellido Paterno",
                data: "paternalSurname"
            },
            {
                title: "Apellido Materno",
                data: "maternalSurname"
            },
            {
                title: "Primer Nombre",
                data: "name"
            },
            {
                title: "Documento",
                data: "document"
            },
            {
                title: "Fecha Nacimiento",
                data: "birthDate"
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
            this.employeesDt.init();
        },
        employeesDt: {
            init: function () {
                employeesDatatable = $("#employees_datatable").DataTable(employeesDtOpts);
            },
            reload: function () {
                employeesDatatable.ajax.reload();
            }
        }
    };

    var forms = {
        submit: {
            import: {
                newEntry: function (formElement) {
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
                        url: "/recursos-humanos/empleados/importar/nuevos",
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
                        $("#import_new_entry_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_new_entry_modal_alert_text").html(error.responseText);
                            $("#import_new_entry_modal_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            import: {
                newEntry: function () {
                    importNewEntryModal.resetForm();
                    $("#import_new_entry_modal").trigger("reset");
                    $("#import_new_entry_modal_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var validate = {
        init: function () {
            importNewEntryModal = $("#import_new_entry_modal").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.newEntry(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#import_new_entry_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.newEntry();
                });
        }
    };

    var events = {
        init: function () {

            $("#work_area_filter, #entry_position_filter, #current_position_filter").on("change", function () {
                datatable.reload();
            });

            $("#newEntrySample").on("click", function () {
                window.location = `/recursos-humanos/empleados/importar/nuevos`;
            });
        }
    }

    return {
        init: function () {
            datatables.init();
            validate.init();
            modals.init();
            events.init();
        }
    };

}();

$(function () {
    Employee.init();
});