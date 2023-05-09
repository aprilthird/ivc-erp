
var LettersSent = function () {

    var lettersSentDatatable = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/cartas-enviadas/listar"),
            data: function (d) {
                d.issuerTargetId = $("#letter_menu_nav li.kt-nav__item--active").data("id");
                d.interestGroupId = $("#interest_group_filter").val();
                d.status = $("#status_filter").val();
                d.hasFile = $("#has_file").is(":checked");
                d.hasAnswer = $("#has_answer").val();
                d.other = $("#letter_menu_nav li.kt-nav__item--active").data("other");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Asunto",
                data: "subject",
                orderable: false
            },
            {
                title: "Fecha",
                data: "date"
            },
            {
                title: "Receptor",
                data: "issuerTargets",
                orderable: false,
                render: function (data) {
                    return data.map((x) => x.name).join(", ");
                }
            },
            {
                title: "Referencia",
                data: "references",
                orderable: false,
                render: function (data) {
                    return data.map((x) => x.name).join(", ");
                }
            },
            {
                title: "Características del Documento",
                data: "status",
                orderable: false,
                render: function (data) {
                    return _app.render.badges(data, _app.constants.letter.status.VALUES);
                }
            },
            {
                title: "Plazo de Respuesta",
                data: "responseTermDays",
                orderable: false,
                render: function (data) {
                    return data ? data + " días" : "---";
                }
            },
            {
                title: "Áreas de Interés",
                data: "interestGroups",
                orderable: false,
                render: function (data) {
                    return data.map((x) => x.name).join(", ");
                }
            },
            {
                title: "Reponsable Directo",
                data: "employee.fullName",
                orderable: false,
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Opciones",
                width: "10%",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.fileUrl) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-view">`;
                        tmp += `<i class="fa fa-eye"></i></button> `;
                    }
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            lettersSentDatatable = $("#letter_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            lettersSentDatatable.ajax.reload();
        },
        initEvents: function () {
            lettersSentDatatable.on("click", ".btn-view", function () {
                let $btn = $(this);
                let id = $btn.data("id");
                form.load.pdf(id);
            });
        }
    };

    var form = {
        load: {
            pdf: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/control-documentario/cartas-enviadas/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.name);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.name}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
                });
            }
        }
    };

    var select2 = {
        init: function () {
            this.answers.init();
            this.status.init();
            this.letters.init();
            this.issuerTargets.init();
            this.interestGroups.init();
            this.employees.init();
        },
        answers: {
            init: function () {
                $(".select2-answers").select2();
            }
        },
        status: {
            init: function () {
                $(".select2-status").select2();
            }
        },
        letters: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cartas")
                }).done(function (result) {
                    $(".select2-letters").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        },
        issuerTargets: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/entidades-emisoras-receptoras-de-cartas")
                }).done(function (result) {
                    $(".select2-issuer-targets").select2({
                        data: result
                    });
                });
            }
        },
        interestGroups: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/grupos-de-interes")
                }).done(function (result) {
                    $(".select2-interest-groups").select2({
                        data: result
                    });
                });
            }
        },
        employees: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/empleados")
                }).done(function (result) {
                    $(".select2-employees").select2({
                        data: result,
                        allowClear: true
                    });
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var events = {
        init: function () {
            $("#status_filter, #interest_group_filter, #has_file, #has_answer").on("change", function () {
                datatable.reload();
            });

            this.loadTargets();

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });
            $("#letter_menu_nav").on("scroll", function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    alert("end reached");
                }
            });
        },
        loadTargets: function () {
            _app.loader.showOnElement("#letter_menu_nav");
            $.ajax({
                url: _app.parseUrl("/control-documentario/cartas-enviadas/receptores")
            }).done(function (result) {
                $.each(result, function (i, item) {
                    let element = $(`<li class="kt-nav__item col-2" data-id="${item.issuerTargetId}" data-name="${item.name}" data-acronym="${item.acronym}" data-other="${item.other}">
                        <a href="javascript:;" class="kt-nav__link" style="height: 100%;">
                            <i class="kt-nav__link-icon flaticon2-user"></i>
                            <span class="kt-nav__link-text">${(item.acronym || item.name)}</span>
                            <span class="kt-nav__link-badge">
                                <span class="kt-badge kt-badge--danger kt-badge--inline kt-badge--pill kt-badge--rounded">${item.totalCount}</span>
                            </span>
                        </a>
                        </li>`);
                    $("#letter_menu_nav").append(element);
                });
                $("#total_letters").text(result.length === 0 ? 0 : result.map(x => x.totalCount).reduce((a, b) => a + b, 0));
                $("#letter_menu_nav li").on("click", function () {
                    $('#letter_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                    $(this).addClass('kt-nav__item--active');
                    $("#datatable_portlet_title").text(`Listado de Cartas para ${$(this).data("name") || $(this).data("acronym")}`);
                    datatable.reload();
                });
                _app.loader.hideOnElement("#letter_menu_nav");
            });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            datepicker.init();
            datatable.init();
        }
    };
}();

$(function () {
    LettersSent.init();
});