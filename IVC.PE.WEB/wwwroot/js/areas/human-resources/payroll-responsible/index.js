var PayrollResponsibles = function () {

    var addForm = null;
    var editForm = null;
    var authDataTable = null;

    var options = {
        resposive: true,
        ajax: {
            url: _app.parseUrl(`/recursos-humanos/resposanbles/listar`),
            dataSrc: ""
        },
        columns: [
            {
                title: "Proyecto",
                data: "projectFullName"
            },
            {
                title: "Autoriza Tareo Semanal #1",
                data: "responsible1FullName"
            },
            {
                title: "Autoriza Tareo Semanal #2",
                data: "responsible2FullName"
            },
            {
                title: "Autoriza Planilla",
                data: "responsible3FullName"
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            this.auth.init();
        },
        auth: {
            init: function () {
                authDataTable = $("#auths_datatable").DataTable(options);
                this.events();
            },
            reload: function () {
                authDataTable.clear().draw();
                authDataTable.ajax.reload();
            },
            events: function () {
                authDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        form.load.edit(id);
                    });
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/resposanbles/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId).trigger('change');
                        formElements.find("[name='Responsible1Id']").val(result.responsible1Id).trigger('change');
                        formElements.find("[name='Responsible2Id']").val(result.responsible2Id).trigger('change');
                        formElements.find("[name='Responsible3Id']").val(result.responsible3Id).trigger('change');
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                //$(formElement).find("[name='ProjectId']").val($("#search_project").val());
                //$(formElement).find("[name='Responsible1Id']").val($("#search_resp_1").val());
                //$(formElement).find("[name='Responsible2Id']").val($("#search_resp_2").val());
                //$(formElement).find("[name='Responsible3Id']").val($("#search_resp_3").val());
                let data = $(formElement).serialize();
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/resposanbles/crear`),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.auth.reload();
                        $("#add_modal").modal('hide');
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
                    url: _app.parseUrl(`/recursos-humanos/resposanbles/editar/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.auth.reload();
                        $("#edit_modal").modal('hide');
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
    }

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
        }
    };

    var select2 = {
        init: function () {
            this.projects.init();
            this.users.init();
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
        },
        users: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/usuarios")
                }).done(function (result) {
                    $(".select2-users").select2({
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

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ProjectId']").attr("id", "Add_ProjectId");
            $("#edit_form [name='ProjectId']").attr("id", "Edit_ProjectId");
            $("#add_form [name='Responsible1Id']").attr("id", "Add_Responsible1Id");
            $("#edit_form [name='Responsible1Id']").attr("id", "Edit_Responsible1Id");
            $("#add_form [name='Responsible2Id']").attr("id", "Add_Responsible2Id");
            $("#edit_form [name='Responsible2Id']").attr("id", "Edit_Responsible2Id");
            $("#add_form [name='Responsible3Id']").attr("id", "Add_Responsible3Id");
            $("#edit_form [name='Responsible3Id']").attr("id", "Edit_Responsible3Id");
        }
    };

    return {
        init: function () {
            select2.init();
            validate.init();
            modals.init();
            datatable.init();
        }
    };
}();

$(function () {
    PayrollResponsibles.init();
});