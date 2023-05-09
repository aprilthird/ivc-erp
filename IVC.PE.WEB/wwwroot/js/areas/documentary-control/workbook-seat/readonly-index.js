var Workbook = function () {

    var workbookDatatable = null;

    var options = {
        responsive: true,
        serverSide: true,
        processing: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/cuadernos-de-obra/listar"),
            data: function (d) {
                d.wroteBy = $("#workbook_menu_nav li.kt-nav__item--active").data("type");
                d.workbookId = $("#workbook_filter").val();
                d.type = $("#type_filter").val();
                d.status = $("#status_filter").val();
                d.hasFile = $("#has_file").is(":checked");
                d.hasAnswer = $("#has_answer").val();
                d.other = $("#letter_menu_nav li.kt-nav__item--active").data("other");
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Número",
                data: "workbook.number"
            },
            {
                title: "Asiento",
                data: "number"
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
                title: "Tipo",
                data: "type",
                orderable: false,
                render: function (data) {
                    return _app.render.badge(data, _app.constants.workbook.type.VALUES);
                }
            },
            {
                title: "Escribe",
                data: "wroteBy",
                orderable: false,
                render: function (data) {
                    return _app.render.badge(data, _app.constants.workbook.wroteBy.VALUES);
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            workbookDatatable = $("#workbook_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            workbookDatatable.ajax.reload();
        },
        initEvents: function () {
            workbookDatatable.on("click", ".btn-view", function () {
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
                    url: _app.parseUrl(`/control-documentario/cuadernos-de-obra/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.number);
                    $("#pdf_frame").prop("src", "https://view.officeapps.live.com/op/embed.aspx?src=" + result.fileUrl);
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Cuaderno de Obra [${result.name}]: ` + "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(result.fileUrl)));
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
            this.types.init();
            this.wroteBys.init();
            this.workbooks.init();
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        },
        wroteBys: {
            init: function () {
                $(".select2-wrotebys").select2();
            }
        },
        workbooks: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/select/cuadernos-de-obra")
                }).done(function (result) {
                    $(".select2-workbooks").select2({
                        data: result
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
            $("#type_filter, #workbook_filter, #has_file, #has_answer").on("change", function () {
                datatable.reload();
            });

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });

            $("#workbook_menu_nav li").on("click", function () {
                $('#workbook_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                $(this).addClass('kt-nav__item--active');
                $("#datatable_portlet_title").text(`Listado de Cartas para ${$(this).data("name") || $(this).data("acronym")}`);
                datatable.reload();
            });

            $("#workbook_menu_nav").on("scroll", function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    alert("end reached");
                }
            });

            this.loadWriters();
        },
        loadWriters: function () {
            _app.loader.showOnElement("#workbook_menu_nav");
            $.ajax({
                url: _app.parseUrl("/control-documentario/cuadernos-de-obra/autores")
            }).done(function (result) {
                $.each(result, function (i, item) {
                    let element = $(`<li class="kt-nav__item col-2" data-id="${item.id}" data-name="${item.name}" data-file-url="${item.fileUrl}">
                        <a href="javascript:;" class="kt-nav__link" style="height: 100%;">
                            <i class="kt-nav__link-icon flaticon2-user"></i>
                            <span class="kt-nav__link-text">${item.name}</span>
                            <span class="kt-nav__link-badge">
                                <span class="kt-badge kt-badge--danger kt-badge--inline kt-badge--pill kt-badge--rounded">${item.totalCount}</span>
                            </span>
                        </a>
                        </li>`);
                    $("#workbook_menu_nav").append(element);
                });
                $("#total_workbooks").text(result.length === 0 ? 0 : result.map(x => x.totalCount).reduce((a, b) => a + b, 0));
                $("#workbook_menu_nav li").on("click", function () {
                    $('#workbook_menu_nav li.kt-nav__item--active').removeClass('kt-nav__item--active');
                    $(this).addClass('kt-nav__item--active');
                    $("#datatable_portlet_title").text(`Listado de Asientos de Cuaderno de Obra para ${$(this).data("name")}`);
                    datatable.reload();
                });
                _app.loader.hideOnElement("#workbook_menu_nav");
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
    Workbook.init();
});