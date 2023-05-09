var Message = function () {

    var id = $("#submit_input").val();
    var observation = $("#obs_text").val();

    events = {
        init: function() {
            $("#prerequest_submit").on("click", function () {
                let data = new FormData();
                data.append("id", $("#submit_input").val());
                data.append("observation", $("#obs_text").val());
                $.ajax({
                    url: `/logistica/pre-requerimientos/enviar-observacion/${id}?observation=${observation}`,
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                }).done(function () {
                    window.location = ("https://erp-ivc.azurewebsites.net/mensaje/observacion-agregada");
                }).fail(function () {
                    window.location = ("https://erp-ivc.azurewebsites.net/mensaje/error");
                });
            });

            $("#order_submit").on("click", function () {
                let data = new FormData();
                data.append("id", $("#submit_input").val());
                data.append("observation", $("#obs_text").val());
                $.ajax({
                    url: `/logistica/ordenes/enviar-observacion/${id}?observation=${observation}`,
                    method: "put",
                    data: data,
                    contentType: false,
                    processData: false
                }).done(function () {
                    window.location = ("https://erp-ivc.azurewebsites.net/mensaje/observacion-agregada");
                }).fail(function () {
                    window.location = ("https://erp-ivc.azurewebsites.net/mensaje/error");
                });
            });
        }
    }

    return {
        init: function () {
            events.init();
        }
    };
}();

$(function () {
    Message.init();
});