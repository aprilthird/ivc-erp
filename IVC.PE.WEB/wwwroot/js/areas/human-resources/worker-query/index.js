var WorkerQuery = function () {
    var searchForm = null;

    var form = {
        submit: {
            search: function (updateUrl) {
                $("#search_field").prop("disabled", true);
                let searchTerm = $("#search_field").val();
                _app.loader.showOnElement("#query_container");
                $.ajax({
                    url: _app.parseUrl(`/recursos-humanos/obreros/consulta/buscar?document=${searchTerm}`),
                })
                    .always(function () {
                        _app.loader.hideOnElement("#result_container");
                        $("#search_field").prop("disabled", false);
                    })
                    .done(function (result) {
                        $("#query_container").html(result);
                        if (updateUrl) {
                            window.history.pushState({ q: searchTerm }, "", _app.parseUrl(`/recursos-humanos/obreros/consulta?document=${searchTerm}`));
                        }
                    })
                    .fail(function (error) {
                        _app.show.notification.add.error();
                    });
            }
        }
    };

    var validate = {
        init: function () {
            searchForm = $("#search_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.search(true);
                }
            });
        }
    };

    var events = {
        init: function () {
            $(window).on("popstate", function (e) {
                events.updateResults();
            }).trigger("popstate");
        },
        updateResults: function () {
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            if (urlParams.has("document")) {
                const searchTerm = urlParams.get("document");
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
        }
    };
}();

$(function () {
    WorkerQuery.init();
});