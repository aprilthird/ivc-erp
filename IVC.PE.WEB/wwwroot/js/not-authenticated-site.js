// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var _app = {
    parseUrl: function (urlString) {
        var url = window.location.protocol;
        url += "//";
        url += window.location.host;
        url += this.constants.url.root;
        url += urlString;
        return url;
    },
    constants: {
        url: {
            root: ""
        },
        emptyGuid: "00000000-0000-0000-0000-000000000000",
        currency: "S/",
        formats: {
            datepicker: "dd/mm/yyyy",
            datetimepicker: "dd/mm/yyyy H:ii P",
            datepickerJsMoment: "DD/MM/YYYY",
            datetimepickerJsMoment: "DD/MM/YYYY h:mm A",
            timepickerJsMoment: "h:mm A",
            momentJsDate: "DD-MM-YYYY",
            momentJsDateTime: "DD-MM-YYYY h:mm A"
        },
        calendar: {
            dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
            dayNamesShort: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"],
            monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
            dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"]
        }
    },
    loader: {
        show: function (message) {
            message = message || "Cargando...";
            KTApp.blockPage({
                overlayColor: '#000000',
                type: 'v2',
                state: 'primary',
                message: message
            });
        },
        hide: function () {
            KTApp.unblockPage();
        },
        showOnElement: function (selector, message) {
            message = message || "Cargando...";
            KTApp.block(selector, {
                overlayColor: '#000000',
                type: 'v2',
                state: 'primary',
                message: message
            });
        },
        hideOnElement: function (selector) {
            $(selector).unblock();
        }
    },
    alert: {
        show: {
            question: function (question, promise, title) {
                title = title || "¿Está seguro?";
                question = question || "El registro será eliminado";
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "El proyecto será eliminado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn-danger",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            promise
                        });
                    }
                });
            }
        }
    },
    show: {
        notification: {
            success: function (message, title) {
                title = title || "Exito";
                toastr.success(message, title);
            },
            warning: function (message, title) {
                title = title || "Alerta";
                toastr.warning(message, title);
            },
            info: function (message, title) {
                title = title || "Info:";
                toastr.info(message, title);
            },
            error: function (message, title) {
                title = title || "Error";
                toastr.error(message, title);
            },
            add: {
                success: function () {
                    toastr.success("Registro ingresado correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo completar el registro.", "Error");
                }
            },
            edit: {
                success: function () {
                    toastr.success("Cambios guardados correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo guardar los cambios.", "Error");
                }
            },
            delete: {
                success: function () {
                    toastr.success("Registro eliminado correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo eliminar el registro.", "Error");
                }
            },
            download: {
                success: function () {
                    toastr.success("Archivo descargado correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo descargar el archivo.", "Error");
                }
            }
        }
    }
};

// ----------
// jQuery
// ----------
(function ($) {
    $.fn.addLoader = function () {
        this.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
        return this;
    };

    $.fn.removeLoader = function () {
        this.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
        return this;
    };
})(jQuery);

// ----------
// jQuery Validation
// ----------
$.extend($.validator.messages, {
    accept: "Por favor, ingrese un archivo con un formato válido.",
    cifES: "Por favor, escriba un CIF válido.",
    creditcard: "Por favor, escriba un número de tarjeta válido.",
    date: "Por favor, escriba una fecha válida.",
    dateISO: "Por favor, escriba una fecha (ISO) válida.",
    digits: "Por favor, escriba sólo dígitos.",
    email: "Por favor, escriba un correo electrónico válido.",
    equalTo: "Por favor, escriba el mismo valor de nuevo.",
    extension: "Por favor, escriba un valor con una extensión permitida.",
    max: $.validator.format("Por favor, escriba un valor menor o igual a {0}."),
    maxlength: $.validator.format("Por favor, no escriba más de {0} caracteres."),
    min: $.validator.format("Por favor, escriba un valor mayor o igual a {0}."),
    minlength: $.validator.format("Por favor, no escriba menos de {0} caracteres."),
    nieES: "Por favor, escriba un NIE válido.",
    nifES: "Por favor, escriba un NIF válido.",
    number: "Por favor, escriba un número válido.",
    pattern: "Por favor, escriba un formato válido.",
    range: $.validator.format("Por favor, escriba un valor entre {0} y {1}."),
    rangelength: $.validator.format("Por favor, escriba un valor entre {0} y {1} caracteres."),
    remote: "Por favor, llene este campo.",
    required: "Este campo es obligatorio.",
    url: "Por favor, escriba una URL válida.",
    step: "Por favor, ingrese un número entero."
});

jQuery.validator.setDefaults({
    errorElement: "em",
    errorPlacement: function (error, element) {
        if (element.parent(".input-group").length) {
            error.insertAfter(element.parent()); // radio/checkbox?      
        }
        else if (element.parent(".m-input-icon").length) {
            error.insertAfter(element.parent());
        }
        else if (element.parent().parent(".m-radio-inline").length) {
            error.insertAfter(element.parent().parent());
        }
        else if (element.hasClass("select2-single")) {
            error.addClass("text-danger").insertAfter(element.next("span")); // select2
        } else {
            error.addClass('invalid-feedback');
            if (element.prop('type') === 'checkbox') {
                error.insertAfter(element.parent('label'));
            } else {
                error.insertAfter(element);
            }
            error.insertAfter(element); // default
        }
    },
    highlight: function highlight(element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function unhighlight(element) {
        $(element).removeClass('is-invalid');//.addClass('is-valid')
    },
    success: function (label, element) {
        $(element).removeClass('is-invalid');//.addClass('is-valid')
    }
});

// ----------
// jQuery Validation Images
// ----------
$.validator.addMethod("fileSizeBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param;
}, "El archivo debe pesar menos de {0} B.");

$.validator.addMethod("fileSizeKBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024;
}, "El archivo debe pesar menos de {0} KB.");

$.validator.addMethod("fileSizeMBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024 * 1024;
}, "El archivo debe pesar menos de {0} MB.");

// ----------
// Datepicker
// ----------
var dayNames = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
var monthNames = [
    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre",
    "Diciembre"
];
var dayNamesShort = ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"];
var dayNamesMin = ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"];
var monthNamesShort = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];
var todayString = "Hoy";

$.fn.datepicker.dates.es = {
    clear: "Borrar",
    days: dayNames,
    daysMin: dayNamesMin,
    daysShort: dayNamesShort,
    format: _app.constants.formats.datepicker,
    months: monthNames,
    monthsShort: monthNamesShort,
    monthsTitle: "Meses",
    today: todayString,
    weekStart: 1
};

$.fn.datepicker.defaults.autoclose = true;
$.fn.datepicker.defaults.clearBtn = true;
$.fn.datepicker.defaults.language = "es";
//$.fn.datepicker.defaults.templates = {
//    leftArrow: "<i class=\"la la-angle-left\"></i>",
//    rightArrow: "<i class=\"la la-angle-right\"></i>"
//};
$.fn.datepicker.defaults.todayHighlight = true;

// ----------
// Select2
// ----------
(function () {
    if (jQuery && jQuery.fn && jQuery.fn.select2 && jQuery.fn.select2.amd) {
        var e = jQuery.fn.select2.amd;

        return e.define("select2/i18n/es", [], function () {
            return {
                errorLoading: function () {
                    return "No se pudieron cargar los resultados";
                },
                inputTooLong: function (e) {
                    var t = e.input.length - e.maximum,
                        n = "Por favor, elimine " + t + " car";

                    return t === 1 ? n += "ácter" : n += "acteres", n;
                },
                inputTooShort: function (e) {
                    var t = e.minimum - e.input.length,
                        n = "Por favor, introduzca " + t + " car";

                    return t === 1 ? n += "ácter" : n += "acteres", n;
                },
                loadingMore: function () {
                    return "Cargando más resultados…";
                },
                maximumSelected: function (e) {
                    var t = "Sólo puede seleccionar " + e.maximum + " elemento";

                    return e.maximum !== 1 && (t += "s"), t;
                },
                noResults: function () {
                    return "No se encontraron resultados";
                },
                searching: function () {
                    return "Buscando…";
                }
            };
        }), { define: e.define, require: e.require };
    }
})();

//$.fn.select2.defaults.set("ajax--delay", 1000);
$.fn.select2.defaults.set("language", "es");
$.fn.select2.defaults.set("placeholder", "---");
$.fn.select2.defaults.set("width", "100%");

//--------------
// Toastr
//--------------
toastr.options = {
    closeButton: true,
    newestOnTop: false,
    positionClass: "toast-top-right",
    preventDuplicates: false,
    progressBar: true,
    onclick: null,
    showDuration: "300",
    hideDuration: "1000",
    timeOut: "5000",
    extendedTimeOut: "1000",
    showEasing: "easeOutBounce",
    hideEasing: "easeInBack",
    closeEasing: "easeInBack",
    showMethod: "slideDown",
    hideMethod: "slideUp",
    closeMethod: "slideUp"
};

// ----------------
// Bootstrap Alerts
// ----------------
$(".alert").on("close.bs.alert", function () {
    $(this).addClass("d-none");
    return false;
});

// ----------
// Number
// ----------
Object.defineProperty(Number.prototype, "toRound", {
    value: function toRound(isPercent = false, decimals, mode = 1, char = "%") {
        var num = this;
        if (isPercent) {
            num *= 100;
        }
        var round = num.toFixed(decimals);
        if (mode === "append")
            round += " " + char;
        else if (mode === "prepend")
            round = char + round;
        return round;
    },
    writable: true,
    configurable: true
});

Object.defineProperty(Number.prototype, "toPercent", {
    value: function toPercent(decimals = 7) {
        var num = this;
        return num.toRound(true, decimals, "append");
    },
    writable: true,
    configurable: true
});

Object.defineProperty(Number.prototype, "toMoney", {
    value: function toMoney(decimals = 2) {
        var num = this;
        return num.toRound(false, decimals, "prepend", _app.constants.currency);
    },
    writable: true,
    configurable: true
});