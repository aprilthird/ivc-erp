var For47 = function () {

    var addForm = null;
    var editForm = null;
    var for47Datatable = null;

    var for47DtOpt = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: "colvisGroup",
                text: "Nivel 1",
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8],
                hide: [9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 2",
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19],
                hide: [20, 21, 22]
            },
            {
                extend: "colvisGroup",
                text: "Nivel 3",
                show: ":hidden"
            }
        ],
        ajax: {
            url: _app.parseUrl("/calidad/for47-colector-descarga/listar"),
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
                //title: "Dirección",
                data: "sewerManifold.address"
            },
            {
                //title: "BZ (i) Nº",
                data: "sewerManifold.sewerBoxStart.code"
            },
            {
                //title: "BZ (i) TT Proyecto",
                data: function (result) {
                    var TT = parseInt(result.bZiRealTerrainType);
                    if (TT == 1) {
                        return "N";
                    } else if (TT == 2) {
                        return "SR";
                    } else {
                        return "R";
                    }
                }
            },
            {
                //title: "BZ (i) TT Real",
                data: function (result) {
                    var TT = parseInt(result.sewerManifold.sewerBoxStart.terrainType);
                    if (TT == 1) {
                        return "N";
                    } else if (TT == 2) {
                        return "SR";
                    } else {
                        return "R";
                    }
                }
            },
            {
                //title: "BZ (j) Nº",
                data: "sewerManifold.sewerBoxEnd.code"
            },
            {
                //title: "BZ (j) TT Proyecto",
                data: function (result) {
                    var TT = parseInt(result.bZjRealTerrainType);
                    if (TT == 1) {
                        return "N";
                    } else if (TT == 2) {
                        return "SR";
                    } else {
                        return "R";
                    }
                }
            },
            {
                //title: "BZ (j) TT Real",
                data: function (result) {
                    var TT = parseInt(result.sewerManifold.sewerBoxEnd.terrainType);
                    if (TT == 1) {
                        return "N";
                    } else if (TT == 2) {
                        return "SR";
                    } else {
                        return "R";
                    }
                }
            },
            {
                //title: "Tramo - Nombre",
                data: "sewerManifold.name",
                visible: false,
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //title: "TT Proyecto N",
                data: function (result) {
                    var TT = parseInt(result.sewerManifold.terrainType);
                    if (TT == 1) {
                        return result.sewerManifold.lengthOfDigging.toFixed(2);
                    } else {
                        return 0;
                    }
                },
                visible: false
            },
            {
                //title: "TT Proyecto SR",
                data: function (result) {
                    var TT = parseInt(result.sewerManifold.terrainType);
                    if (TT == 2) {
                        return result.sewerManifold.lengthOfDigging.toFixed(2);
                    } else {
                        return 0;
                    }
                },
                visible: false
            },
            {
                //title: "TT Proyecto R",
                data: function (result) {
                    var TT = parseInt(result.sewerManifold.terrainType);
                    if (TT == 3) {
                        return result.sewerManifold.lengthOfDigging.toFixed(2);
                    } else {
                        return 0;
                    }
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. N %",
                data: function (result) {
                    var lodN = parseFloat(result.lengthOfDiggingN).toFixed(2);
                    return lodN + "%";
                    },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. SR %",
                data: function (result) {
                    var lodSR = parseFloat(result.lengthOfDiggingSR).toFixed(2);
                    return lodSR + "%";
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. R %",
                data: function (result) {
                    var lodR = parseFloat(result.lengthOfDiggingR).toFixed(2);
                    return lodR + "%";
                },
                visible: false
            },
            {
                //title: "Tramo - Long. Excav. for01",
                data: function (result) {
                    return result.sewerManifold.lengthOfDigging.toFixed(2);
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. N",
                data: function (result) {
                    var lodN = parseFloat(result.lengthOfDiggingN);
                    var lod = parseFloat(result.sewerManifold.lengthOfDigging);
                    var resp = ((lod * lodN) / 100).toFixed(2);
                    return resp;
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. SR",
                data: function (result) {
                    var lodSR = parseFloat(result.lengthOfDiggingSR);
                    var lod = parseFloat(result.sewerManifold.lengthOfDigging);
                    var resp = ((lod * lodSR) / 100).toFixed(2);
                    return resp;
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Long. Excav. R",
                data: function (result) {
                    var lodR = parseFloat(result.lengthOfDiggingR);
                    var lod = parseFloat(result.sewerManifold.lengthOfDigging);
                    var resp = ((lod * lodR) / 100).toFixed(2);
                    return resp;
                },
                visible: false
            },
            {
                //title: "Tramo For47 - Fecha de Registro",
                data: "workBookRegistryDate",
                visible: false
            },
            {
                //title: "Tramo For47 - Cuaderno Obra Libro",
                data: "workBookNumber",
                visible: false
            },
            {
                //title: "Tramo For47 - Cuaderno Obra Asiento",
                data: "workBookSeat",
                visible: false
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
            { className: "dt-body-right", "targets": [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22] }
        ]
    };

    var datatables = {
        init: function () {
            this.for47Dt.init();
            this.initEvents();
        },

        for47Dt: {
            init: function () {
                for47Datatable = $("#for47s_datatable").DataTable(for47DtOpt);
            },
            reload: function () {
                for47Datatable.ajax.reload();
            }
        },
        initEvents: function () {

            for47Datatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.edit(id);
                });

            for47Datatable.on("click",
                ".btn-view",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    forms.load.pdf(id);
                });

            for47Datatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Estás Seguro?",
                        text: "El Sewer Manifold For47 será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/calidad/for47-colector-descarga/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatables.for47Dt.reload();
                                        select2.manifolds.init();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo técnico ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        })
                                    },
                                    error: function (errormessage) {
                                        swal.fire({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn-danger",
                                            animation: false,
                                            customClass: 'animated tada',
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el archivo ténico"
                                        });
                                    }
                                });
                            });
                        },
                        
                    })
                });
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                select2.manifolds.edit(id);
                $.ajax({
                    url: _app.parseUrl(`/calidad/for47-colector-descarga/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_manifold']").prop("disabled", true);
                        formElements.find("[name='select_starttt']").val(result.sewerManifold.sewerBoxStart.terrainType).trigger("change");
                        formElements.find("[name='select_endtt']").val(result.sewerManifold.sewerBoxEnd.terrainType).trigger("change");
                        formElements.find("[name='select_tt']").val(result.sewerManifold.terrainType).trigger("change");
                        $("#edit_form #bzi").html(result.sewerManifold.sewerBoxStart.code);
                        $("#edit_form #bzj").html(result.sewerManifold.sewerBoxEnd.code);
                        formElements.find("[name='SewerManifold.LengthOfDigging']").val(result.sewerManifold.lengthOfDigging).prop("disabled", true);
                        formElements.find("[name='LengthOfDiggingN']").val(result.lengthOfDiggingN);
                        formElements.find("[name='LengthOfDiggingSR']").val(result.lengthOfDiggingSR);
                        formElements.find("[name='LengthOfDiggingR']").val(result.lengthOfDiggingR);
                        formElements.find("[name='WorkBookNumber").val(result.workBookNumber);
                        formElements.find("[name='WorkBookSeat").val(result.workBookSeat);
                        formElements.find("[name='WorkBookRegistryDate']").datepicker("setDate", result.workBookRegistryDate);
                        $(".search-manifold").hide();
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
                    url: _app.parseUrl(`/calidad/for47-colector-descarga/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.sewerManifold.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='SewerManifold.SewerBoxStart.TerrainType']").val($(formElement).find("[name='select_starttt']").val());
                $(formElement).find("[name='SewerManifold.SewerBoxEnd.TerrainType']").val($(formElement).find("[name='select_endtt']").val());
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
                    url: _app.parseUrl("/calidad/for47-colector-descarga/crear"),
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
                        datatables.for47Dt.reload();
                        $("#add_modal").modal("hide");
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#add_alert_text").html(error.responseText);
                            $("#add_alert").removeClass("d-none").addClass("show");
                        }
                        $(formElement).find("[name='SewerManifold.LengthOfDigging']").prop("disabled", true);
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='SewerManifold.SewerBoxStart.TerrainType']").val($(formElement).find("[name='select_starttt']").val());
                $(formElement).find("[name='SewerManifold.SewerBoxEnd.TerrainType']").val($(formElement).find("[name='select_endtt']").val());
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
                    url: _app.parseUrl(`/calidad/for47-colector-descarga/editar/${id}`),
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
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function () {
                        datatables.for47Dt.reload();
                        $("#edit_modal").modal("hide");
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        if (error.reponseText) {
                            $("#edit_alert_text").html(error.responseText);
                            $("#edit_alert").removeClass("d-none").addClass("show");
                        }
                        $(formElement).find("[name='SewerManifold.LengthOfDigging']").prop("disabled", true);
                        _app.show.notification.edit.error();
                    })
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                select2.manifolds.init();
                $("#add_form #bzi").html("");
                $("#add_form #bzj").html("");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                select2.manifolds.init();
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
                    $("#SearchSelectManifold").show();
                });

        }
    };

    var select2 = {
        init: function () {
            this.manifolds.init();
            this.formulas.init();
            this.sewergroups.init();
            this.tts.init();
        },
        manifolds: {
            init: function () {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl("/select/colectores-descarga-ejecucion-for47")
                }).done(function (result) {
                    $(".select2-manifolds").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                });
            },
            edit: function (id) {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-ejecucion-for47/${id}`)
                }).done(function (result) {
                    $(".select2-manifolds").select2({
                        data: result
                    });
                });
            }
        },
        formulas: {
            init: function () {
                $(".select2-formulas").select2();
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
            }
        },
        tts: {
            init: function () {
                $(".select2-tts").select2();
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker({
                orientation: "top"
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
                    $("#add_form #bzi").html(result.sewerBoxStart.code);
                    formElements.find("[name='select_starttt']").val(result.sewerBoxStart.terrainType).trigger("change");
                    $("#add_form #bzj").html(result.sewerBoxEnd.code);
                    formElements.find("[name='select_endtt']").val(result.sewerBoxEnd.terrainType).trigger("change");
                    formElements.find("[name='select_tt']").val(result.terrainType).trigger("change");
                    formElements.find("[name='SewerManifold.LengthOfDigging']").val(result.lengthOfDigging).prop("disabled", true);
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
    For47.init();
});