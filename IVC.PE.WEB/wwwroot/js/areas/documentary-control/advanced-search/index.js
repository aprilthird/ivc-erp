var AdvancedSearch = function () {
    var searchForm = null;

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Number']").val(result.number);
                        formElements.find("[name='Subject']").val(result.subject);
                        formElements.find("[name='Date']").datepicker("setDate", result.date);
                        formElements.find("[name='Type']").val(result.type).trigger("change");
                        formElements.find("[name='WroteBy']").val(result.wroteBy).trigger("change");
                        formElements.find("[name='WorkbookId']").val(result.workbookId).trigger("change");
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
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.number);
                    $("#pdf_frame").prop("src", "https://view.officeapps.live.com/op/embed.aspx?src=" + result.fileUrl);
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Cuaderno de Obra [${result.name}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        },
        submit: {
            search: function (updateUrl) {
                $("#search_field").prop("disabled", true);
                let searchTerm = $("#search_field").val();
                _app.loader.showOnElement("#result_container");
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/busqueda/buscar?searchTerm=${searchTerm}`),
                })
                    .always(function () {
                        _app.loader.hideOnElement("#result_container");
                        $("#search_field").prop("disabled", false);
                    })
                    .done(function (result) {
                        $("#result_container").html(result);
                        $(".btn-view, .timeline-card").on("click", function () {
                            let title = $(this).data("title");
                            let type = $(this).data("type");
                            let fileUrl = $(this).data("url");
                            $("#preview_name").text(title);
                            let documentUrl = "";
                            if (fileUrl.includes("pdf")) {
                                documentUrl = "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(fileUrl));
                                //$("#preview_frame").prop("src", documentUrl + "&embedded=true");
                                loadPdf(title, fileUrl, "pdf_views");
                                $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${title}]: ` + documentUrl);
                                $(".btn-mailto").data("name", title).data("url", documentUrl);
                                $("#adobe_modal").modal("show");
                            }
                            else if (fileUrl.includes("docx")) {
                                documentUrl = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(fileUrl));
                                $("#preview_frame").prop("src", documentUrl + "&embedded=true");
                                $("#share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${title}]: ` + documentUrl);
                                $(".btn-mailto").data("name", title).data("url", documentUrl);
                                $("#preview_modal").modal("show");
                            }
                            
                        });
                        $(".btn-print").on("click", function () {
                            window.scrollTo(0, 0);
                            html2canvas(document.querySelector(".timeline-container"), {
                                logging: true,
                                backgroundColor: "#fff",
                                scrollY: $(document).scrollTop()
                                //y: 2000
                            }).then(canvas => {
                                let newCanvas = document.createElement("canvas");
                                newCanvas.width = canvas.height;
                                newCanvas.height = canvas.width;
                                let rCtx = newCanvas.getContext("2d");
                                rCtx.translate(newCanvas.width * 0.5, newCanvas.height * 0.5);
                                rCtx.rotate(Math.PI * 0.5);
                                rCtx.drawImage(canvas, -canvas.width * 0.5, -canvas.height * 0.5);

                                var canvasImg = newCanvas.toDataURL("image/jpg");
                                //$("#img_container").html(`<img src='${canvasImg}' alt='img'/>`);
                                var doc = new jsPDF();
                                if (canvas.height > doc.internal.pageSize.getHeight()) {
                                    doc.addImage(canvasImg, "JPEG", 20, 20, 120, doc.internal.pageSize.getHeight());
                                }
                                else {
                                    doc.addImage(canvasImg, "JPEG", 20, 20, 120, canvas.height);
                                }
                                doc.save("Linea de Tiempo.pdf");
                            });
                            window.scrollTo(0, document.body.scrollHeight || document.documentElement.scrollHeight);
                        });
                        if (updateUrl) {
                            window.history.pushState({ q: searchTerm }, "", _app.parseUrl(`/control-documentario/busqueda?q=${searchTerm}`));
                        }
                    })
                    .fail(function (error) {
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#Add_Type").prop("selectedIndex", 0).trigger("change");
                $("#Add_WroteBy").prop("selectedIndex", 0).trigger("change");
                $("#Add_WorkbookId").val(null).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#Edit_Type").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WroteBy").prop("selectedIndex", 0).trigger("change");
                $("#Edit_WorkbookId").val(null).trigger("change");
            },
            import: {
                data: function () {
                    importDataForm.resetForm();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                },
                files: function () {
                    importFilesForm.resetForm();
                    $("#import_files_form").trigger("reset");
                    $("#import_files_alert").removeClass("show").addClass("d-none");
                }
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#search_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.search(true);
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

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.data();
                });

            $("#import_files_modal").on("hidden.bs.modal",
                function () {
                    form.reset.import.files();
                });
        }
    };

    var events = {
        init: function () {
            //html2canvas(document.querySelector("#result_container")).then(canvas => {
            //    // Export the canvas to  its data URI representation
            //    var base64image = canvas.toDataURL("image/png");

            //    // Open the image in a new window
            //    window.open(base64image, "_blank");
            //});

            $(window).on("popstate", function (e) {
                events.updateResults();
            }).trigger("popstate");

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });
        },
        updateResults: function () {
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            if (urlParams.has("q")) {
                const searchTerm = urlParams.get("q");
                if (searchTerm) {
                    $("#search_field").val(searchTerm);
                    form.submit.search(false);
                }
            }
        }
    };

    return {
        init: function () {
            validate.init();
            events.init();
            modals.init();
        }
    };
}();

$(function () {
    AdvancedSearch.init();
});