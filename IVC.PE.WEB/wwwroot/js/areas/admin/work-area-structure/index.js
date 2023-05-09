var WorkAreaStructure = function () {

    var itemsDatatable = null;
    
    var options = {
        "core": {
            "data": [
                { "id": "a1", "parent": "#", "text": "Seguridad" },
                { "id": "m1", "parent": "a1", "text": "Capacitaciones" },
                { "id": "s1", "parent": "m1", "text": "Variables" },
                { "id": "s11", "parent": "s1", "text": "Estados" },
                { "id": "s12", "parent": "s1", "text": "Categorías" },
                { "id": "s2", "parent": "m1", "text": "Temas" },
                { "id": "s3", "parent": "m1", "text": "Sesiones" },
                { "id": "s4", "parent": "m1", "text": "Control" }
            ]
        }
    };

    var tree = {
        init: function () {
            $("#menu_container").jstree(options);
        }
    };

    var events = {
        init: function () {

        }
    };

    return {
        init: function () {
            events.init();
            tree.init();
        }
    };
}();

$(function () {
    WorkAreaStructure.init();
});