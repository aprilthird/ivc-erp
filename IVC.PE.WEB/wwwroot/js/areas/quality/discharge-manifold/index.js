var Main = function () {

    var mainDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: false,
        scrollX: true,
        buttons: [
            {
                extend: 'colvisGroup',
                text: 'Nivel 1',
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,32,33,34],
                hide: [21, 22, 23, 24, 25, 26, 27, 28, 29, 30,31]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 2',
                show: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,27,32,33,34],
                hide: [28, 29, 30, 31]
            },
            {
                extend: 'colvisGroup',
                text: 'Nivel 3',
                show: ':hidden'
            }
        ],
        ajax: {
            url: _app.parseUrl("/calidad/colector-descarga/listar"),
            dataSrc: ""
        },
        columns: [
            {
                //0
                //title: "# Protocolo",
                data: "protocolNumber"
            },
            {   //1
                data: "sewerManifold.sewerBoxStart.code"
            },
            {
                //2
                //title: "Cota Tapa Bz(I)",
                data: "sewerManifold.sewerBoxStart.coverLevel",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //3
                //title: "Cota Fondo Bz(I)",
                data: "sewerManifold.sewerBoxStart.bottomLevel",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //4
                //title: "Altura (I)",
                data: "sewerManifold.sewerBoxStart.height",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {   //5
                data: "sewerManifold.sewerBoxEnd.code"
            },
            {
                //6
                //title: "Cota Tapa Bz(J)",
                data: "sewerManifold.sewerBoxEnd.coverLevel",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //7
                //title: "Cota Fondo Bz(J)",
                data: "sewerManifold.sewerBoxEnd.bottomLevel",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //8
                //title: "Cota Llegada Bz(J)",
                data: "sewerManifold.sewerBoxEnd.arrivalLevel",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //9
                //title: "Altura (J)",
                data: "sewerManifold.sewerBoxEnd.height",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //10
                data: "sewerManifold.name",
                render: function (data, type, row) {
                    var tmp = data;
                    var b = tmp.bold();
                    return b;
                }
            },
            {
                //11
                data: "sewerManifold.ditchHeight"
            },
            {   //12
                data: "sewerManifold.ditchLevelPercent"
            },
            {   //13
                //title: "Longitud Entre Ejes H",
                data: "sewerManifold.lengthBetweenHAxles",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //14
                //title: "Longitud Entre Ejes I",
                data: "sewerManifold.lengthBetweenIAxles",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //15
                //title: "Longitud Entre Ejes I",
                data: "sewerManifold.lengthOfPipelineInstalled",
                render: function (data, type, row) {
                    if (data > 0) {
                        return `
						<span>${parseFloat(Math.round(data * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)}</span>
						`;
                    } else if (data == 0) {
                        return ` <span> 0 </span> `
                    }
                }
            },
            {
                //16
                //title: "Nivelación",
                data: "leveling",

            },
            {
                //17
                //title: "Prueba Z. Abierta",
                data: "openZTest",
                
            },
            {
                //18
                //title: "Prueba Z. Tapada",
                data: "closedZTest",
                
            },
        {
                //19
                //title: "Prueba Espejo",
                data: "mirrorTest",
                
            },
        {
                //20
                //title: "Prueba Bola/Mandril",
                data: "ballTest",
                
            },
            {
                //21
                //title: "Fabricante",
                data: "producer",
                visible: false
            },
            {   //22
                //title: "Lote Tubería",
                data: "pipeBatch",
                visible: false
            },
            {
                //23
                //title: "Lote Tubería",
                data: "secondPipeBatch",
                visible: false
            },
            {   //24
                //title: "Lote Tubería",
                data: "thirdPipeBatch",
                visible: false
            },
            {
                //25
                //title: "Lote Tubería",
                data: "fourthPipeBatch",
                visible: false
            },
            {
                //26
                //title: "# Serie Equipo Topográfico",
                data: "equipmentCertificate.serial",
                visible: false
            },
            {
                //27
                //title: "# Serie Equipo Topográfico",
                data: "equipmentCertificate2.serial",
                visible: false
            },


            {
                //title: "Libro PZA",
                data: "bookPZO",
                visible: false
            },
            {
                //title: "Asiento PZA",
                data: "seatPZC",
                visible: false
            },
            {
                //title: "Libro PZT",
                data: "bookPZF",
                visible: false
            },
            {
                //title: "Asiento PZT",
                data: "seatPZF",
                visible: false
            },
            {
                data: "mirrorTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Espejo" data-name="${row.sewerManifold.name}"  class="btn btn-secondary btn-sm btn-icon btn-play">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
                    return tmp;
                }
            },
            {
                data: "monkeyBallTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Bola/Mandril" data-name="${row.sewerManifold.name}" class="btn btn-secondary btn-sm btn-icon btn-play">`;
                        tmp += `<i class="fa fa-video"></i></button>`;
                    }
                    return tmp;
                }
            },
            {
                data: "zoomTestVideoUrl",
                render: function (data, type, row) {
                    var tmp = "";
                    if (data != null) {
                        tmp += `<button data-videourl="${data}" data-test="Cámara Zoom" data-name="${row.sewerManifold.name}" class="btn btn-secondary btn-sm btn-icon btn-play">`;
                        tmp += `<i class="fa fa-video"></i></button> `;
                    }
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
                    tmp += `<button data-id="${row.id}" data-name="${row.sewerManifold.name}" class="btn btn-info btn-sm btn-icon btn-view">`;
                    tmp += `<i class="fa fa-eye"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: 'text-right', 'targets': [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20] },
            { className: 'text-center', 'targets': [31,32,33] }
        ]
    };

    var datatable = {
        init: function () {
            mainDatatable = $("#main_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            mainDatatable.ajax.reload();
        },
        initEvents: function () {
            mainDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            mainDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let smName = $btn.data("name");
                let id = $btn.data("id");
                form.load.pdf(id, smName);
            });
            mainDatatable.on("click", ".btn-play", function () {
                let $btn = $(this);
                let testName = $btn.data("test");
                let smName = $btn.data("name");
                let videourl = $btn.data("videourl");
                form.load.vid(videourl, `${testName} - ${smName}`);
            });
            mainDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El Colector de Descarga será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/calidad/colector-descarga/eliminar/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        select2.manifolds.init();
                                        swal.fire({
                                            type: "success",
                                            title: "Completado",
                                            text: "El colector de descarga ha sido eliminado con éxito",
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
                                            text: "Ocurrió un error al intentar eliminar el colector de descarga"
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
            edit: function (id) {
                _app.loader.show();
                select2.manifolds.edit(id);
                console.log(id);
                $.ajax({
                    url: _app.parseUrl(`/calidad/colector-descarga/${id}`)
                })
                    .done(function (result) {

                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='ProtocolNumber']").val(result.protocolNumber);
                        formElements.find("[name='SewerManifoldId']").val(result.sewerManifoldId);
                        formElements.find("[name='select_manifold']").prop("disabled", true);
                        formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxStart.coverLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxStart.bottomLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxStart.ArrivalLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxStart.arrivalLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxEnd.CoverLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxEnd.coverLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxEnd.bottomLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxEnd.ArrivalLevel']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxEnd.arrivalLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxStart.Height']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxStart.height * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxEnd.Height']").val(parseFloat(Math.round(result.sewerManifold.sewerBoxEnd.height * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.LengthBetweenHAxles']").val(parseFloat(Math.round(result.sewerManifold.lengthBetweenHAxles * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.LengthBetweenIAxles']").val(parseFloat(Math.round(result.sewerManifold.lengthBetweenIAxles * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='EquipmentCertificateId']").val(result.equipmentCertificateId);
                        formElements.find("[name='select_equipment']").val(result.equipmentCertificateId).trigger("change");
                        formElements.find("[name='EquipmentCertificate2Id']").val(result.equipmentCertificate2Id);
                        formElements.find("[name='select_equipment2']").val(result.equipmentCertificate2Id).trigger("change");
                        formElements.find("[name='Producer']").val(result.producer);
                        formElements.find("[name='PipeBatch']").val(result.pipeBatch);
                        formElements.find("[name='SecondPipeBatch']").val(result.secondPipeBatch);
                        formElements.find("[name='ThirdPipeBatch']").val(result.thirdPipeBatch);
                        formElements.find("[name='FourthPipeBatch']").val(result.fourthPipeBatch);
                        formElements.find("[name='Leveling']").datepicker("setDate", result.leveling);
                        formElements.find("[name='OpenZTest']").datepicker("setDate", result.openZTest);
                        formElements.find("[name='ClosedZTest']").datepicker("setDate", result.closedZTest);
                        formElements.find("[name='MirrorTest']").datepicker("setDate", result.mirrorTest);
                        formElements.find("[name='BallTest']").datepicker("setDate", result.ballTest);
                        formElements.find("[name='BookPZO']").val(result.bookPZO);
                        formElements.find("[name='SeatPZC']").val(result.seatPZC);
                        formElements.find("[name='BookPZF']").val(result.bookPZF);
                        formElements.find("[name='SeatPZF']").val(result.seatPZF);
                        $("#edit_form .search-manifold").hide();
                        if (result.fileUrl) {
                            $("#edit_form [for='customFile']").text("Reemplazar archivo subido");
                        }
                        else {
                            $("#edit_form [for='customFile']").text("Selecciona un archivo");
                        }

                        if (result.mirrorTestVideoUrl) {
                            $("#edit_form [for='videoFileMirror']").text("Reemplazar video subido");
                        }
                        else {
                            $("#edit_form [for='videoFileMirror']").text("Selecciona un video");
                        }
                        if (result.monkeyBallTestVideoUrl) {
                            $("#edit_form [for='videoFileMonkey']").text("Reemplazar video subido");
                        }
                        else {
                            $("#edit_form [for='videoFileMonkey']").text("Selecciona un video");
                        }
                        if (result.ZoomTestVideoUrl) {
                            $("#edit_form [for='videoFileZoom']").text("Reemplazar video subido");
                        }
                        else {
                            $("#edit_form [for='videoFileZoom']").text("Selecciona un video");
                        }
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            },
            pdf: function (id,smName) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/calidad/colector-descarga/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text("For 01 del tramo " + smName);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");

                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            },
            vid: function (videourl,testName) {
                $("#test_name").html(testName);
                $("#video_frame").prop("src", videourl);
                $("#video_modal").modal("show");
            }
        },
        submit: {
            add: function (formElement) {
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='EquipmentCertificateId']").val($(formElement).find("[name='select_equipment']").val());
                $(formElement).find("[name='EquipmentCertificate2Id']").val($(formElement).find("[name='select_equipment2']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file-pdf").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                var emptyVideoMirror = $(formElement).find(".videoFileMirror").get(0).files.length === 0;
                if (!emptyVideoMirror) {
                    $(formElement).find(".custom-video-mirror").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                var emptyVideoMonkey = $(formElement).find(".videoFileMonkey").get(0).files.length === 0;
                if (!emptyVideoMonkey) {
                    $(formElement).find(".custom-video-monkey").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                var emptyVideoZoom = $(formElement).find(".videoFileZoom").get(0).files.length === 0;
                if (!emptyVideoZoom) {
                    $(formElement).find(".custom-video-zoom").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: _app.parseUrl("/calidad/colector-descarga/crear"),
                    method: "post",
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
                        if (!emptyVideoMirror) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyVideoMonkey) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyVideoZoom) {
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
                        if (!emptyVideoMirror) {
                            $("#space-bar").remove();
                        }
                        if (!emptyVideoMonkey) {
                            $("#space-bar").remove();
                        }
                        if (!emptyVideoZoom) {
                            $("#space-bar").remove();
                        }
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
                $(formElement).find("[name='SewerManifoldId']").val($(formElement).find("[name='select_manifold']").val());
                $(formElement).find("[name='EquipmentCertificateId']").val($(formElement).find("[name='select_equipment']").val());
                $(formElement).find("[name='EquipmentCertificate2Id']").val($(formElement).find("[name='select_equipment2']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                if (!emptyFile) {
                    $(formElement).find(".custom-file-pdf").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }

                var emptyVideoMirror = $(formElement).find(".videoFileMirror").get(0).files.length === 0;
                if (!emptyVideoMirror) {
                    $(formElement).find(".custom-video-mirror").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                var emptyVideoMonkey = $(formElement).find(".videoFileMonkey").get(0).files.length === 0;
                if (!emptyVideoMonkey) {
                    $(formElement).find(".custom-video-monkey").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                var emptyVideoZoom = $(formElement).find(".videoFileZoom").get(0).files.length === 0;
                if (!emptyVideoZoom) {
                    $(formElement).find(".custom-video-zoom").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/calidad/colector-descarga/editar/${id}`),
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

                        if (!emptyVideoMirror) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyVideoMonkey) {
                            xhr.upload.onprogress = function (evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            };
                        }
                        if (!emptyVideoZoom) {
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
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                select2.manifolds.init();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                select2.manifolds.init();
            },
            vid: function () {
                $("#video_frame").prop("src", "");
            }
        }
    };
    var select2 = {
        init: function () {
            this.styles.init();
            this.manifolds.init();
            this.equipments.init();
        },
        styles: {
            init: function () {
                $(".select2-styles").select2();
            }
        },
        manifolds: {
            init: function () {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl("/select/colectores-descarga-ejecucion")
                }).done(function (result) {
                    $(".select2-manifolds").select2({

                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
                    });
                    mainDatatable.ajax.reload();
                });
            },
            edit: function (id) {
                $(".select2-manifolds").empty();
                $.ajax({
                    url: _app.parseUrl(`/select/colectores-descarga-ejecucion/${id}`)
                }).done(function (result) {
                    $(".select2-manifolds").select2({
                        data: result
                    });
                    mainDatatable.ajax.reload();
                });
            }
        },
        equipments: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/series-certificado-equipo")
                }).done(function (result) {
                    $(".select2-equipments").select2({
                        data: result,
                        allowClear: true,
                        placeholder: "--Seleccionar--"
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
            //$(".iaxlesformula").on("change", function () {
            //    let formElements = $("#add_form");
            //    var swstart = formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val();
            //    var swend = $("#SewerManifold_SewerBoxEnd_BottomLevel").val();
            //    var haxles = $("#SewerManifold_LengthBetweenHAxles").val();

            //    if (!swstart.isnum) {
            //        $("#SewerManifold_LengthBetweenIAxles").val(0);
            //        return;
            //    }
            //    if (!swend.isnum) {
            //        $("#SewerManifold_LengthBetweenIAxles").val(0);
            //        return;
            //    }

            //    var iaxles = ((swstart - swend) ^ 2 + haxles ^ 2) ^ (0.5);
            //    $("#SewerManifold_LengthBetweenIAxles").val(iaxles);
            //});

            $(".search-manifold").on("click", function () {
                let formId = $(this).closest(`.form`).attr(`id`)
                let id = $(`#${formId} .select2-manifolds`).val();
                console.log(formId);
                console.log(id);
                $.ajax({
                    url: _app.parseUrl(`/calidad/colector-descarga/colector/${id}`),
                    dataSrc: ""
                })
                    .done(function (result) {


                        let formElements = $(`#${formId}`);

                        console.log(formElements);
                        //.val(
                        formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").val(parseFloat(Math.round(result.sewerBoxStart.coverLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").attr("disabled", "disabled");
                        formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val(parseFloat(Math.round(result.sewerBoxStart.bottomLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").attr("disabled", "disabled");
                        //formElements.find("[name='SewerManifold.SewerBoxStart.ArrivalLevel']").val(parseFloat(Math.round(result.sewerBoxStart.arrivalLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxStart.ArrivalLevel']").attr("disabled", "disabled");
                        formElements.find("[name='SewerManifold.SewerBoxEnd.CoverLevel']").val(parseFloat(Math.round(result.sewerBoxEnd.coverLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxEnd.CoverLevel']").attr("disabled", "disabled");
                        formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").val(parseFloat(Math.round(result.sewerBoxEnd.bottomLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").attr("disabled", "disabled");
                        formElements.find("[name='SewerManifold.SewerBoxEnd.ArrivalLevel']").val(parseFloat(Math.round(result.sewerBoxEnd.arrivalLevel * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.SewerBoxEnd.ArrivalLevel']").attr("disabled", "disabled");
                        formElements.find("[name='SewerManifold.LengthBetweenHAxles']").val(parseFloat(Math.round(result.lengthBetweenHAxles * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.LengthBetweenHAxles']").attr("disabled", "disabled");
                        //formElements.find("[name='SewerManifold.LengthBetweenIAxles']").val(parseFloat(Math.round(result.lengthBetweenIAxles * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        //formElements.find("[name='SewerManifold.LengthBetweenIAxles']").attr("disabled", "disabled");

                        let cover_value = formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").val();
                        let bottom_value = formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val();

                        let cover_value_e = formElements.find("[name='SewerManifold.SewerBoxEnd.CoverLevel']").val();
                        let bottom_value_e = formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").val();
                        let h_formula = cover_value - bottom_value;
                        let h_formula_e = cover_value_e - bottom_value_e;
                        formElements.find("[name='SewerManifold.SewerBoxStart.Height']").val(parseFloat(Math.round(h_formula * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                        formElements.find("[name='SewerManifold.SewerBoxEnd.Height']").val(parseFloat(Math.round(h_formula_e * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));

                        let len_h = formElements.find("[name='SewerManifold.LengthBetweenHAxles']").val();

                        let formula = Math.pow(bottom_value - bottom_value_e, 2);
                        let formula2 = Math.pow(len_h, 2);
                        console.log(formula);
                        console.log(formula2);
                        let len_i = Math.pow((formula + formula2), 0.5);

                        formElements.find("[name='SewerManifold.LengthBetweenIAxles']").val(parseFloat(Math.round(len_i * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                    })
            });

            $(".start_h_formula").on("change", function () {
                let formId = $(this).closest(`.form`).attr(`id`)
                let formElements = $(`#${formId}`);



                let cover_value = formElements.find("[name='SewerManifold.SewerBoxStart.CoverLevel']").val();
                let bottom_value = formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val();

                let cover_value_e = formElements.find("[name='SewerManifold.SewerBoxEnd.CoverLevel']").val();
                let bottom_value_e = formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").val();
                let h_formula = cover_value - bottom_value;
                let h_formula_e = cover_value_e - bottom_value_e;
                formElements.find("[name='SewerManifold.SewerBoxStart.Height']").val(parseFloat(Math.round(h_formula * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
                formElements.find("[name='SewerManifold.SewerBoxEnd.Height']").val(parseFloat(Math.round(h_formula_e * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));


            });

            $(".length_formula").on("change", function () {
                let formId = $(this).closest(`.form`).attr(`id`)
                let formElements = $(`#${formId}`);



                let bottom_value = formElements.find("[name='SewerManifold.SewerBoxStart.BottomLevel']").val();
                let bottom_value_e = formElements.find("[name='SewerManifold.SewerBoxEnd.BottomLevel']").val();
                let len_h = formElements.find("[name='SewerManifold.LengthBetweenHAxles']").val();

                let formula = Math.pow(bottom_value - bottom_value_e, 2);
                let formula2 = Math.pow(len_h, 2);
                console.log(formula);
                console.log(formula2);
                let len_i = Math.pow((formula + formula2), 0.5);

                formElements.find("[name='SewerManifold.LengthBetweenIAxles']").val(parseFloat(Math.round(len_i * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2));
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

            $("#video_modal").on("hidden.bs.modal",
                function () {
                    form.reset.vid();
                });
        }
    };

    return {
        init: function () {
            datatable.init();
            datepicker.init();
            validate.init();
            modals.init();
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    Main.init();
});