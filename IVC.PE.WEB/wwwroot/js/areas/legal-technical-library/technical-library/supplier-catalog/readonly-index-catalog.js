var SupplierCatalog = function () {

    var technicalLibraryDatatable = null;

    var guid = $("#supplierGuid").val();

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl(`/libreria-tecnica/catalogo-proveedores/listar?supplierGuid=${guid}`),
            dataSrc: ""
        },
        columns: [
            {
                title: "Título",
                data: "title"
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
            technicalLibraryDatatable = $("#technical_library_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            technicalLibraryDatatable.ajax.reload();
        },
        initEvents: function () {
            technicalLibraryDatatable.on("click", ".btn-view", function () {
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
                    url: _app.parseUrl(`/libreria-tecnica/catalogo-proveedores/catalogo/${id}`)
                }).done(function (result) {
                    $("#pdf_name").text(result.title);
                    $("#pdf_frame").prop("src", "https://docs.google.com/gview?url=" + result.fileUrl + "&embedded=true");
                    $("#pdf_share_whatsapp").prop("href", "https://wa.me/?text=" + `Documento [${result.title}]: ` + "https://docs.google.com/gview?url=" + encodeURIComponent(encodeURI(result.fileUrl)));
                    $(".btn-mailto").data("name", result.title).data("url", "https://docs.google.com/gview?url=" + encodeURI(result.fileUrl));
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
        },
        types: {
            init: function () {
                $(".select2-types").select2();
            }
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    return {
        init: function () {
            datatable.init();
            select2.init();
            datepicker.init();
        }
    };
}();

$(function () {
    SupplierCatalog.init();
});