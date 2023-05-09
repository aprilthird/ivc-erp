var Workbook = function () {

    var workbookDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/control-documentario/libros-de-obra/listar"),
            data: function (d) {
                delete d.columns;
            }
        },
        columns: [
            {
                title: "Número",
                data: "number"
            },
            {
                title: "Nombre",
                data: "name"
            },
            {
                title: "Rango",
                data: "range"
            },
            {
                title: "Período",
                data: "term"
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
                    url: _app.parseUrl(`/control-documentario/libros-de-obra/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.number);
                    $("#pdf_frame").prop("src", "https://view.officeapps.live.com/op/embed.aspx?src=" + result.fileUrl);
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Libro de Obra [${result.name}]: ` + "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.name).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
                    $("#pdf_modal").modal("show");
                }).always(function () {
                    _app.loader.hide();
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
            $("#add_modal").on("shown.bs.modal", function () {
                form.reset.add();
            });

            $(".btn-mailto").on("click", function () {
                let fileName = $(this).data("name");
                let fileUrl = $(this).data("url");
                window.open(`mailto:?subject=Documento recibido (${fileName})&body=Puede visualizar el documento en: ${fileUrl} %0D%0A%0D%0A`);
            });
        }
    };

    return {
        init: function () {
            events.init();
            datepicker.init();
            datatable.init();
        }
    };
}();

$(function () {
    Workbook.init();
});