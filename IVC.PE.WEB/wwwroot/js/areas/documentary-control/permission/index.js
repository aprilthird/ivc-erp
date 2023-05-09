var Provider = function () {

    var bondAddsDataTable = null;
    var bondRensDataTable = null;
    var addForm = null;
    var editForm = null;    
    var addRenForm = null;
    var editRenForm = null;

    var responsibles = null;

    var isFromDetail = false;

    var addId = null;
    var renId = null;

    var bondAddsDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/permisos/listar"),
            data: function (d) {
                d.projectId = $("#project_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                title: "Formula",
                data: "projectFormula"
            },
            {
                title: "Tramo",
                data: "length"
            },
            {
                title: "Via Principal",
                data: "principalWay"
            },
            {
                title: "Desde",
                data: "from"
            },
            {
                title: "Hasta",
                data: "to"
            },
            {
                title: "Entidad que Autoriza",
                data: "authorizingEntity"
            },
            {
                title: "# de Autorización   ",
                data: "authorizationNumber"
            },
            {
                title: "Tipo de Renovación",
                data: "renovationType"
            },
            {
                title: "Días para Vencimiento",
                data: "validity",
                render: function (data, type, row) {
                    if (row.isTheLast) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--info">Cerrado</span>
								</label>
							</span>`;
                    }

                    if (data > 30) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--primary">OK ${data} días</span>
								</label>
							</span>`;
                    } else if (data > 15) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--warning">ADVERTENCIA ${data} días</span>
								</label>
							</span>`;
                    } else if (data > 0) {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--danger">URGENTE ${data} días</span>
								</label>
							</span>`;
                    } else {
                        return `<span class="kt-switch kt-switch--icon">
								<label>
									<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--dark">VENCIDO ${data} días</span>
								</label>
							</span>`;
                    }
                }
            },
            {
                title: "Inicio de Vigencia",
                data: "startDateString"
            },
            {
                title: "Fin de Vigencia",
                data: "endDateString"
            },
            {
                title: "Carpeta",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.bondRenovationId}" class="btn btn-success btn-sm btn-icon btn-renovation">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    tmp += `<button data-id="${row.bondRenovationId}" data-bondid="${row.bondAddId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button>`;
                    tmp += `<button data-id="${row.bondAddId}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
    };

    var bondRensDt_options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/permisos/renovaciones/listar"),
            data: function (d) {
                d.bondId = addId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "# de Autorización   ",
                data: "authorizationNumber"
            },
            {
                title: "Tipo de Renovación",
                data: "renovationType.description"
            },
            {
                title: "Inicio de Vigencia",
                data: "startDate"
            },
            {
                title: "Fin de Vigencia",
                data: "endDate"
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
                    tmp += `<button data-id="${row.id}" data-bondid="${row.permissionId}" class="btn btn-secondary btn-sm btn-icon btn-pdf">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    return tmp;
                }
            }
        ]
    };



    var dataTables = {
        init: function () {
            this.bondAddsDt.init();
            this.bondRensDt.init();
        },
        bondAddsDt: {
            init: function () {
                bondAddsDataTable = $("#bondadds_datatable").DataTable(bondAddsDt_options);
                this.events();
            },
            reload: function () {
                bondAddsDataTable.ajax.reload();
            },
            events: function () {
                bondAddsDataTable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        addId = id;
                        dataTables.bondRensDt.reload();
                        forms.load.detail(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-renovation",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        renId = id;
                        forms.load.addren(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = false;
                        select2.bondRenovations.reload(bondid, renid);
                    });

                bondAddsDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                bondAddsDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Permiso y sus renovaciones serán eliminadas permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/control-documentario/permisos/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El Permiso y sus renovaciones han sido eliminadas con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el permiso"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        bondRensDt: {
            init: function () {
                bondRensDataTable = $("#bondrens_datatable").DataTable(bondRensDt_options);
                this.events();
            },
            reload: function () {
                bondRensDataTable.clear().draw();
                bondRensDataTable.ajax.reload();
            },
            events: function () {
                bondRensDataTable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editren(id);
                    });

                bondRensDataTable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "La renovación será eliminada permanentemente.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlas",
                            confirmButtonClass: "btn-danger",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: _app.parseUrl(`/control-documentario/permisos/renovaciones/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            dataTables.bondAddsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "La crenovación ha sido eliminada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            console.log(errormessage);
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: 'animated tada',
                                                confirmButtonText: "Entendido",
                                                text: `Ocurrió un error al intentar eliminar la renovación. Motivo: ${errormessage.responseText}`
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });

                bondRensDataTable.on("click",
                    ".btn-pdf",
                    function () {
                        let $btn = $(this);
                        let bondid = $btn.data("bondid");
                        let renid = $btn.data("id");
                        isFromDetail = true;
                        select2.bondRenovations.reload(bondid, renid);
                        $("#detail_modal").modal("hide");
                    });
            }
        },
    };

    var forms = {
        load: {
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });                
            },
            editren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_ren_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='PermissionId']").val(result.permissionId.toString());
                        formElements.find("[name='Responsibles']").val(result.responsibles.toString());
                        formElements.find("[name='RenovationTypeId']").val(result.renovationTypeId);
                        formElements.find("[name='select_renovationtype']").val(result.renovationTypeId).trigger("change");
                        
                        formElements.find("[name='AuthorizationNumber']").val(result.authorizationNumber);
                        formElements.find("[name='StartDate']").datepicker('setDate', result.startDate);
                        formElements.find("[name='EndDate']").datepicker('setDate', result.endDate);
                        formElements.find("[name='IsTheLast']").val(result.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.isTheLast.toString()).trigger("change");

                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        $("#edit_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                        $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                        $("#pdf_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });   
            },
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/${id}`),

                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProjectId']").val(result.projectId);
                        formElements.find("[name='select_project']").val(result.projectId).trigger("change");
                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_formula']").val(result.projectFormulaId).trigger("change");
                        formElements.find("[name='AuthorizingEntityId']").val(result.authorizingEntityId);
                        formElements.find("[name='select_authentity']").val(result.authorizingEntityId).trigger("change");
                        formElements.find("[name='AuthorizationTypeId']").val(result.authorizationTypeId);
                        formElements.find("[name='select_authtype']").val(result.authorizationTypeId).trigger("change");
                        formElements.find("[name='PrincipalWay']").val(result.principalWay);
                        formElements.find("[name='From']").val(result.principalWay);
                        formElements.find("[name='To']").val(result.principalWay);
                        formElements.find("[name='Length']").val(result.principalWay);
                        formElements.find("[name='PermissionRenovation.Id']").val(result.permissionRenovation.id);
                        formElements.find("[name='PermissionRenovation.Responsibles']").val(result.permissionRenovation.responsibles.toString());
                        formElements.find("[name='PermissionRenovation.RenovationTypeId']").val(result.permissionRenovation.renovationTypeId);
                        formElements.find("[name='select_renovationtype']").val(result.permissionRenovation.renovationTypeId).trigger("change");
                        
                        formElements.find("[name='PermissionRenovation.AuthorizationNumber']").val(result.permissionRenovation.authorizationNumber);


                        formElements.find("[name='PermissionRenovation.StartDate']").datepicker('setDate', result.permissionRenovation.startDate);
                        formElements.find("[name='PermissionRenovation.EndDate']").datepicker('setDate', result.permissionRenovation.endDate);
                        formElements.find("[name='PermissionRenovation.IsTheLast']").val(result.permissionRenovation.isTheLast.toString());
                        formElements.find("[name='select_isthelast']").val(result.permissionRenovation.isTheLast.toString()).trigger("change");
                       
                            if (result.permissionRenovation.fileUrl) {
                                $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                            }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    }); 
            },
            addren: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/renovaciones/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#add_ren_form");
                        formElements.find("[name='PermissionId']").val(result.permissionId);
                        formElements.find("[name='StartDate']").datepicker('setDate', result.endDate);
                        $("#add_ren_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='select_project']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='AuthorizingEntityId']").val($(formElement).find("[name='select_authentity']").val());
                $(formElement).find("[name='AuthorizationTypeId']").val($(formElement).find("[name='select_authtype']").val());
                $(formElement).find("[name='PermissionRenovation.RenovationTypeId']").val($(formElement).find("[name='select_renovationtype']").val());
                $(formElement).find("[name='PermissionRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/control-documentario/permisos/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        dataTables.bondAddsDt.reload();
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
            editren: function (formElement) {
                
                
                $(formElement).find("[name='RenovationTypeId']").val($(formElement).find("[name='select_renovationtype']").val());
                $(formElement).find("[name='IsTheLAst']").val($(formElement).find("[name='select_isthelast']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/renovaciones/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        dataTables.bondRensDt.reload();
                        $("#edit_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_ren_alert_text").html(error.responseText);
                            $("#edit_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            addren: function (formElement) {
                console.log(renId);
                $(formElement).find("[name='PermissionRenovationId']").val(renId);
                $(formElement).find("[name='RenovationTypeId']").val($(formElement).find("[name='select_renovationtype']").val());
                $(formElement).find("[name='IsTheLAst']").val($(formElement).find("[name='select_isthelast']").val());


                
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/renovaciones/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        dataTables.bondAddsDt.reload();
                        $("#add_ren_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_ren_alert_text").html(error.responseText);
                            $("#add_ren_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='ProjectId']").val($(formElement).find("[name='select_project']").val());
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_formula']").val());
                $(formElement).find("[name='AuthorizingEntityId']").val($(formElement).find("[name='select_authentity']").val());
                $(formElement).find("[name='AuthorizationTypeId']").val($(formElement).find("[name='select_authtype']").val());
                $(formElement).find("[name='PermissionRenovation.RenovationTypeId']").val($(formElement).find("[name='select_renovationtype']").val());
                $(formElement).find("[name='PermissionRenovation.IsTheLast']").val($(formElement).find("[name='select_isthelast']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input, select").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/permisos/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false,
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        dataTables.bondAddsDt.reload();
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
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                select2.users.reload();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            editren: function () {
                editRenForm.reset();
                $("#edit_ren_form").trigger("reset");
                $("#edit_ren_alert").removeClass("show").addClass("d-none");
                dataTables.bondAddsDt.reload();
                dataTables.bondRensDt.reload();
                select2.users.reload();
                $("#detail_modal").modal("show");
            },
            addren: function () {
                addRenForm.reset();
                $("#add_ren_form").trigger("reset");
                select2.users.reload();
                $("#add_ren_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                select2.users.reload();
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            pdf: function () {
                if (isFromDetail)
                    $("#detail_modal").modal("show");
            }
        }
    };

    var select2 = {
        init: function () {
            this.formulas.init();
            this.authentities.init();
            this.authtypes.init();
            this.renovationtypes.init();
            this.users.init();
            this.bondRenovations.init();
            this.isTheLast.init();
        },


        formulas: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/formulas-proyecto")
                }).done(function (result) {
                    $(".select2-formulas").select2({
                        data: result
                    });
                });
            }
        },

        authentities: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/entidades-que-autorizan")
                }).done(function (result) {
                    $(".select2-authentities").select2({
                        data: result
                    });
                });
            }
        },

        authtypes: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-autorizacion")
                }).done(function (result) {
                    $(".select2-authtypes").select2({
                        data: result
                    });
                });
            }
        },

        renovationtypes: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/tipos-de-renovacion")
                }).done(function (result) {
                    $(".select2-renovationtypes").select2({
                        data: result
                    });
                });
            }
        },

        users: {
            init: function () {
                
                $.ajax({
                    url: _app.parseUrl("/control-documentario/responsables-de-permisos/proyecto")
                }).done(function (result) {
                    responsibles = result.responsibles.toString();
                    $("#PermissionRenovation_Responsibles").val(result.responsibles.toString());
                    $("#Responsibles").val(result.responsibles.toString());
                });
            },
            reload: function () {
                $("#PermissionRenovation_Responsibles").val(responsibles);
                $("#Responsibles").val(responsibles);
            }
        },
        bondRenovations: {
            init: function () {
                $(".select2-bond-renovations").select2();
            },
            reload: function (bondid, renid) {
                console.log(bondid);
                console.log(renid);
                $(".select2-bond-renovations").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/permisos/renovaciones/${bondid}`)
                }).done(function (result) {
                    $(".select2-bond-renovations").select2({
                        data: result
                    });
                    if (renid != null)
                        $("#select_bondrenovations").val(renid).trigger("change");
                    $("#btn_LoadPdf").trigger("click");
                });
            }
        },
        isTheLast: {
            init: function () {
                $(".select2-isthelast").select2();
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

            detailForm = $("#detail_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                }
            });

            editRenForm = $("#edit_ren_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.editren(formElement);
                }
            });

            addRenForm = $("#add_ren_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.addren(formElement);
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

            $("#edit_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.editren();
                });

            $("#add_ren_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.addren();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#pdf_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.pdf();
                });
        }
    };

    var datepickers = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#btn_LoadPdf").on("click", function () {
                let id = $("#select_bondrenovations").val();
                console.log(id);
                forms.load.pdf(id);
            });

            $("#project_filter,#bank_filter,#supply_family_filter, #supply_group_filter").on("change", function () {
                dataTables.bondAddsDt.reload();
            });
        }
    };

    return {
        init: function () {
            select2.init();
            dataTables.init();
            validate.init();
            modals.init();
            datepickers.init();
            events.init();
        }
    };
}();

$(function () {
    Provider.init();
});