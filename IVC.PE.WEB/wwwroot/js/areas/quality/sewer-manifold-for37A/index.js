var For37A = function () {

    var addForm = null;
    var editForm = null;
    var addFoldingForm = null;
    var editFoldingForm = null;

    var for37ADatatable = null;
    var foldingDatatable = null;

    var for37AId = null;

    var for37ADtOpt = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/calidad/for37A-colector-descarga/listar"),
            data: function (d) {
                d.sewerGroupId = $("#sewergroup_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        columns: [
            {
                //title: "# Protocolo",
                data: "for01ProtocolNumber"
            },
            {
                //title: "Cuadrilla",
                data: "sewerManifold.productionDailyPart.sewerGroup.code"
            },
            {
                //title: "Direccion",
                data: "sewerManifold.address"
            },
            {
                //title: "Tramo",
                data: "sewerManifold.name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "Long. entre ejes H",
                data: function (result) {
                    resp = parseFloat(result.sewerManifold.lengthOfPipeInstalled);
                    return resp.toFixed(2);
                }
            },
            {
                //title: "# Termofusiones",
                data: "hotMeltsNumber"
            },
            {
                //title: "# Electrofusiones",
                data: "electrofusionsNumber"
            },
            {
                //title: "# Electrofusiones",
                data: "electrofusionsPasNumber"
            },
            {
                //title: "Inicio Electrofusion",
                data: "startElectrofusionDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                //title: "Fin Electrofusion",
                data: "endElectrofusionDate",
                orderable: true,
                "sType": "date-uk"
            },
            {
                //title: "Lote de Tuberia 1",
                data: "firstPipeBatch"
            },
            {
                //title: "Lote de Tuberia 2",
                data: "secondPipeBatch"
            },
            {
                //title: "Lote de Tuberia 3",
                data: "thridPipeBatch"
            },
            {
                //title: "Lote de Tuberia 4",
                data: "forthPipeBatch"
            },
            {
                //title: "Folding",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-details">`;
                    tmp += `<i class="fa fa-th-list"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-icon btn-folding">`;
                    tmp += `<i class="fa fa-history"></i></button> `;
                    return tmp;
                }
            },
            {
                //title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [4, 5, 6, 7] }
        ]
    };

    var foldingDtOpt = {
        reponsive: true,
        ajax: {
            url: _app.parseUrl(`/calidad/foldingfor37A-for37A-colector-descarga/listar`),
            data: function (d) {
                d.SewerManifoldFor37AId = for37AId;
                delete d.columns;
            },
            dataSrc: ""
        },
        buttons: [],
        columns: [
            {
                title: "Tipo de Soldadura",
                data: function (result) {
                    var resp = parseInt(result.weldingType);
                    if (resp == 1)
                        return "Termofusión";
                    else if (resp == 2)
                        return "Electrofusión Tub.";
                    else
                        return "Electrofusión Pas.";
                }
            },
            {
                title: "N° de Junta",
                data: "meetingNumber"
            },
            {
                title: "Fecha",
                data: "date"
            },
            {
                title: "Opciones",
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
    }

    var datatables = {
        init: function () {
            this.for37ADt.init();
            this.foldingDt.init();
        },
        for37ADt: {
            init: function () {
                for37ADatatable = $("#for37As_datatable").DataTable(for37ADtOpt);
                this.initEvents();
            },
            reload: function () {
                for37ADatatable.ajax.reload();
                select2.index.electrofusionesTub();
                select2.index.electrofusionesPas();
                select2.index.termofusiones();
                select2.sewergroups.filter();
            },
            initEvents: function () {

                for37ADatatable.on("click",
                    ".btn-details",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        for37AId = id;
                        datatables.foldingDt.reload();
                        forms.load.detail(id);
                    });

                for37ADatatable.on("click",
                    ".btn-folding",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        datatables.foldingDt.reload();
                        forms.load.foldingFor37A(id);
                    });

                for37ADatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                for37ADatatable.on("click",
                    ".btn-view",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        forms.load.pdf(id);
                    });

                for37ADatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Sewer Manifold For37A será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/calidad/for37A-colector-descarga/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.for37ADt.reload();
                                            select2.manifolds.init();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el archivo técnico"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        },
        foldingDt: {
            init: function () {
                foldingDatatable = $("#folding_datatable").DataTable(foldingDtOpt);
                this.events();
            },
            reload: function () {
                foldingDatatable.ajax.reload();
                select2.index.electrofusionesTub();
                select2.index.electrofusionesPas();
                select2.index.termofusiones();
            },
            events: function () {

                foldingDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        $("#detail_modal").modal("hide");
                        forms.load.editfolding(id);
                    });

                foldingDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El Folding For37A será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/calidad/foldingfor37A-for37A-colector-descarga/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.foldingDt.reload();
                                            datatables.for37ADt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El archivo técnico ha sido eliminada con éxito",
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
                                                text: "Ocurrió un error al intentar eliminar el archivo técnico"
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
                select2.manifolds.edit(id);
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for37A-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='select_manifold']").prop("disabled", true);
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        /*
                        formElements.find("[name='FirstPipeBatch']").val(result.firstPipeBatch);
                        formElements.find("[name='SecondPipeBatch']").val(result.secondPipeBatch);
                        formElements.find("[name='ThridPipeBatch']").val(result.thridPipeBatch);
                        formElements.find("[name='ForthPipeBatch']").val(result.forthPipeBatch);
                        */
                        $("#edit_form #SewerGroup_Code").val(result.sewerManifold.productionDailyPart.sewerGroup.code).prop("disabled", true);
                        $("#edit_form .search-manifold").hide();
                        if (result.fileUrl) {
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
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for37A-colector-descarga/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.sewerManifold.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            detail: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for37A-colector-descarga/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#detail_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='For01ProtocolNumber']").val(result.for01ProtocolNumber);
                        formElements.find("[name='SewerManifold.ProductionDailyPart.SewerGroup.Code']").val(result.sewerManifold.productionDailyPart.sewerGroup.code);
                        formElements.find("[name='SewerManifold.Name']").val(result.sewerManifold.name);
                        formElements.find("[name='SewerManifold.Address']").val(result.sewerManifold.address);
                        formElements.find("[name='SewerManifold.LengthOfPipeInstalled']").val(result.sewerManifold.lengthOfPipeInstalled.toFixed(2));
                        $("#detail_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            foldingFor37A: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for37A-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#add_folding_form");
                        formElements.find("[name='SewerManifoldFor37AId']").val(result.id);
                        $("#add_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            editfolding: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor37A-for37A-colector-descarga/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {
                        let formElements = $("#edit_folding_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldFor37AId']").val(result.sewerManifoldFor37AId);
                        formElements.find("[name='Date']").datepicker("setDate", result.date);
                        formElements.find("[name='select_welding']").val(result.weldingType).trigger("change");
                        formElements.find("[name='MeetingNumber']").val(result.meetingNumber);
                        $("#edit_folding_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
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
                    url: _app.parseUrl("/calidad/for37A-colector-descarga/crear"),
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function (result) {
                        datatables.for37ADt.reload();
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
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
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
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/for37A-colector-descarga/editar/${id}`),
                    method: "put",
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
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for37ADt.reload();
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
            addfolding: function (formElement) {
                $(formElement).find("[name='WeldingType']").val($(formElement).find("[name='select_welding']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor37A-for37A-colector-descarga/crear`),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for37ADt.reload();
                        datatables.foldingDt.reload();
                        $("#add_folding_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_folding_alert_text").html(error.responseText);
                            $("#add_folding_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            },
            editfolding: function (formElement) {
                $(formElement).find("[name='WeldingType']").val($(formElement).find("[name='select_welding']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                let id = $(formElement).find("input[name='Id']").val();
                $btn.addLoader();
                $.ajax({
                    url: _app.parseUrl(`/calidad/foldingfor37A-for37A-colector-descarga/editar/${id}`),
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.for37ADt.reload();
                        datatables.foldingDt.reload();
                        $("#edit_folding_modal").modal("hide");
                        _app.show.notification.add.success();

                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#edit_folding_alert_text").html(error.responseText);
                            $("#edit_folding_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                select2.manifolds.init();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.reset();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                select2.manifolds.init();
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
            }
        }
    }

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

            detailForm = $("#detail_form").validate({
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
            })
        }
    };

    var select2 = {
        init: function () {
            this.manifolds.init();
            this.index.termofusiones();
            this.index.electrofusionesTub();
            this.index.electrofusionesPas();
            this.sewergroups.filter();
            this.filters();
        },
        manifolds: {
            init: function () {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl("/select/colectores-descarga-ejecucion-for37A-hasfor01")
                }).done(function (result) {
                    $("#add_form .select2-manifolds").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            },
            edit: function (id) {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-ejecucion-for37A/${id}`)
                }).done(function (result) {
                    $("#edit_form .select2-manifolds").select2({
                        data: result
                    });
                });
            }
        },
        sewergroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cuadrillas")
                }).done(function (result) {
                    $(".select2-sewergroups").select2({
                        data: result
                    });
                });
            },
            filter: function () {
                $.ajax({
                    url: _app.parseUrl("/select/for37A-cuadrillas")
                }).done(function (result) {
                    $(".select2-sewergroup-filter").select2({
                        data: result
                    });
                });
            }
        },
        index: {
            termofusiones: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/for37A-colector-descarga/total-termofusiones"),
                    data: {
                        sewerGroupId: $("#sewergroup_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#total_termofusiones").val(result);
                });
            },
            electrofusionesTub: function () {
                $.ajax({
                    url: _app.parseUrl("/calidad/for37A-colector-descarga/total-electrofusiones-tub"),
                    data: {
                        sewerGroupId: $("#sewergroup_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#total_electrofusiones_tub").val(result);
                });
            },
            electrofusionesPas: function () { 
                $.ajax({
                    url: _app.parseUrl("/calidad/for37A-colector-descarga/total-electrofusiones-pas"),
                    data: {
                        sewerGroupId: $("#sewergroup_filter").val()
                    },
                    dataSrc: ""
                }).done(function (result) {
                    $("#total_electrofusiones_pas").val(result);
                });
            }
        },
        filters: function(){
            $("#sewergroup_filter").on("change", function () {
                filter = $("#sewergroup_filter").val();
                datatables.for37ADt.reload();
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "bottom"
            }).datepicker('setDate', 'today')
        }
    };

    var events = {
        init: function () {
            $("#SearchSelectManifold").on("click", function () {
                let formElements = $("#add_form");
                let id = formElements.find("[name='select_manifold']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/colectores-descarga/${id}`)
                }).done(function (result) {
                    formElements.find("[name='SewerManifoldId']").val(result.id);
                    $("#add_form #SewerGroup_Code").val(result.productionDailyPart.sewerGroup.code);
                });
            });
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            events.init();
            datatables.init();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    For37A.init();
});