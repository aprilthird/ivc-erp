var Metered = function () {

    var editForm = null;

    var meteredsDatatable = null;
    var importDataForm = null;
    var allOption = new Option('Todos', null, true, true);
    var meteredsDtOpt = {
        responsive: false,
        sScrollX: true,
        ajax: {
            url: _app.parseUrl("/oficina-tecnica/metrados-replanteo-por-tramo/listar"),
            data: function (d) {
                d.budgetTitleId = $("#budget_title_filter").val();
                d.projectFormulaId = $("#project_formula_filter").val();
                d.workFrontId = $("#work_front_filter").val();
                d.sewerGroupId = $("#sewer_group_filter").val();
                delete d.columns;
            },
            dataSrc: ""
        },
        ordering: false,
        columns: [
            {
                title: "Item",
                data: "itemNumber"              

            },
            {
                title: "Descripción",
                //data: "description"
                data: function (result) {
                    var item=result.itemNumber
                    var titulo1 = result.description;
                    
                    if (item == "06.04" || item=="06.03") {//azul
                        return `<b style="color:rgb(0, 0, 255);" >${titulo1}</b>`;
                    } else
                        if (item == "06.04.03" || item == "06.04.04" || item=="06.03.07") {//verde
                            return `<b style="color:rgb(0, 143, 57);" >${titulo1}</b>`;
                        }
                        else
                            return titulo1;
                }
            },
            {
                title: "Unidad",
                data: "unit"
                
            },
            {
                title: "Metrado",
                //data: "metered",
                data: function (x) {
                   
                    
                    
                    if (x.metered == "0") {
                        return "";
                    } 
                     else
                        return parseFloat(x.metered).toFixed(2);

                    
              
                        
                }

                
            },
            {
                title: "Frente de Trabajo",
                data: "workFrontCode"
                
               
            },
            {
                title: "Cuadrilla",
                data: "sewerCode"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-icon btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-icon btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ],
        "columnDefs": [
            { className: "dt-body-right", "targets": [3] }
        ]
    };

    var datatables = {
        init: function () {
            this.meteredsDt.init();
        },
        meteredsDt: {
            init: function () {
                meteredsDatatable = $("#metereds_datatable").DataTable(meteredsDtOpt);
                this.initEvents();
            },
            reload: function () {
                meteredsDatatable.ajax.reload();
            },
            initEvents: function () {

                meteredsDatatable.on("click",
                    ".btn-edit",
                    function () {
                        let $btn = $(this);
                        //no usado?
                        let id = $btn.data("id");
                        forms.load.edit(id);
                    });

                meteredsDatatable.on("click",
                    ".btn-delete",
                    function () {
                        let $btn = $(this);
                        let id = $btn.data("id");
                        Swal.fire({
                            title: "¿Está seguro?",
                            text: "El metrado replanteo será eliminado permanentemente",
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
                                        url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-tramo/eliminar/${id}`),
                                        type: "delete",
                                        success: function (result) {
                                            datatables.meteredsDt.reload();
                                            swal.fire({
                                                type: "success",
                                                title: "Completado",
                                                text: "El metrado replanteo ha sido eliminado con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (errormessage) {
                                            swal.fire({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn-danger",
                                                animation: false,
                                                customClass: "animated tada",
                                                confirmButtonClass: "Entendido",
                                                text: "Ocurrió un error al intentar eliminar el metrado replanteo"
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
            }
        }
    };

    var forms = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-tramo/${id}`)
                })
                    .done(function (result) {
                        console.log(result);
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);

                        formElements.find("[name='BudgetTittleId']").val(result.budgetTittleId);
                        formElements.find("[name='select_titles']").val(result.budgetTittleId).trigger("change");



                        formElements.find("[name='ItemNumber']").val(result.itemNumber);
                        formElements.find("[name='Description']").val(result.description);
                        //
                        //formElements.find("[name='WrokFrontId']").val(result.workFrontId);
                        //formElements.find("[name='select_work_fronts']").val(result.workFrontId);

                        //

                        formElements.find("[name='SewerGroupId']").val(result.sewerGroupId);
                        formElements.find("[name='select_sewer_groups']").val(result.sewerGroupId).trigger("change");

                        //

                        formElements.find("[name='ProjectFormulaId']").val(result.projectFormulaId);
                        formElements.find("[name='select_project_formulas']").val(result.projectFormulaId).trigger("change");

                        select2.workfront.edit(result.projectFormulaId, result.workFrontId);
                        //
                        formElements.find("[name='Metered']").val(result.metered);

                        formElements.find("[name='Unit']").val(result.unit);
                        formElements.find("[name='select_units']").val(result.unit);
                     
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/oficina-tecnica/metrados-replanteo-por-tramo/crear"),
                    method: "post",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.meteredsDt.reload();
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
                //let data = $(formElement).serialize();
                $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formulas']").val());
                $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                $(formElement).find("[name='WorkFrontId'").val($(formElement).find("[name='select_work_fronts']").val());
                $(formElement).find("[name='SewerGroupId']").val($(formElement).find("[name='select_sewer_groups']").val());
                $(formElement).find("[name='Unit']").val($(formElement).find("[name='select_units']").val());
                let data = new FormData($(formElement).get(0));
                let $btn = $(formElement).find("button[type='submit']");
                
                
                
                $btn.addLoader();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-tramo/editar/${id}`),
                    method: "put",
                    contentType: false,
                    processData: false,
                    data: data
                })
                    .always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatables.meteredsDt.reload();
                        
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
            },
            import: {
                data: function (formElement) {
                    $(formElement).find("[name='ProjectFormulaId']").val($(formElement).find("[name='select_project_formulas']").val());
                    $(formElement).find("[name='BudgetTitleId']").val($(formElement).find("[name='select_titles']").val());
                    var p1 = $(".select2-project-formulas").val();
                    var p2 = $(".select2-titles").val();
                    //console.log(p1); //parametro 1 del import
                    //console.log(p2); //parametro 2 del import
                    let data = new FormData($(formElement).get(0));
                    let $btn = $(formElement).find("button[type='submit']");
                    $btn.addLoader();
                    var emptyFile = $(formElement).find("input[type='file']").get(0).files.length === 0;
                    $(formElement).find("input").prop("disabled", true);
                    if (!emptyFile) {
                        $(formElement).find(".custom-file").append(
                            "<div id='space-bar' class='m--space-10'></div><div class='progress kt-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated kt--bg-primary' role='progressbar'></div></div>");
                        $(".progress-bar").width("0%");
                    }
                    $.ajax({
                        url: `/oficina-tecnica/metrados-replanteo-por-tramo/importar?formulaId=${p1}&titleId=${p2}`, //import(formulaId,titleid)
                        type: "POST",
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
                            return xhr;
                        }
                    }).always(function () {
                        $btn.removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    }).done(function (result) {
                        datatables.meteredsDt.reload();
                        $("#import_data_modal").modal("hide");
                        _app.show.notification.addRange.success();
                    }).fail(function (error) {
                        if (error.responseText) {
                            $("#import_data_alert_text").html(error.responseText);
                            $("#import_data_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.addRange.error();
                    });
                }
            }
        },
        reset: {
            add: function () {
                addForm.reset();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $(".select2-project-formulas").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
            },
            edit: function () {
                
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $(".select2-project-formulas").prop("selectedIndex", 0).trigger("change");
                $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
            },
            import: {
                data: function () {
                    importDataForm.reset();
                    $("#import_data_form").trigger("reset");
                    $("#import_data_alert").removeClass("show").addClass("d-none");
                    $(".select2-project-formulas").prop("selectedIndex", 0).trigger("change");
                    $(".select2-titles").prop("selectedIndex", 0).trigger("change");
                    $(".select2-work-fronts").prop("selectedIndex", 0).trigger("change");
                    $(".select2-sewer-groups").prop("selectedIndex", 0).trigger("change");
                }
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.edit(formElement);
                }
            });

            importDataForm = $("#import_data_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    forms.submit.import.data(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.edit();
                });

            $("#import_data_modal").on("hidden.bs.modal",
                function () {
                    forms.reset.import.data();
                });
        }
    };

    var select2 = {
 
        init: function () {
            this.projectformula();
            this.title();
            this.workfront.init();
            this.sewergroup();
            

        },
        title: function () {
            $.ajax({
                url: _app.parseUrl("/select/titulos-de-presupuesto")
            })
                .done(function (result) {
                    $(".select2-titles").select2({
                        data: result
                    });
                    $(".select2-budgettitle-filter").select2({
                        data: result
                    });
                });
        },
        projectformula: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-project-formulas").select2({
                        data: result
                    });
                    $(".select2-projectformula-filter").select2({
                        data: result
                    });

 


                });

          
        },
        bugetTitle: function () {
            $.ajax({
                url: _app.parseUrl("/select/formulas-proyecto")
            })
                .done(function (result) {
                    $(".select2-titles").select2({
                        data: result
                    });
                    $(".select2-budgettitle-filter").select2({
                        data: result
                    });
                });
        },
        
        workfront: {
            init: function () {
                $(".select2-workfront-filter").select2();
                $(".select2-work-fronts").select2();
            },
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula?projectFormulaId=${sg}`)
                }).done(function (result) {
                    $(".select2-work-fronts").empty();
                    $(".select2-work-fronts").append(allOption).trigger('change');
                    $(".select2-work-fronts").select2({
                        data: result
                    });


                });
            },
            edit: function (eqid, eqsid) {
                let sg = eqid;
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula?projectFormulaId=${sg}`)
                }).done(function (result) {
                    $(".select2-work-fronts").empty();
                    $(".select2-work-fronts").select2({
                        data: result
                    });
                    $(".select2-work-fronts").val(eqsid).trigger('change');
                    console.log(eqsid);
                });
            },

        },
        workfrontfilter: {
 
            reload: function (id) {
                let sg = id;
                let pId = $("#project_general_filter").val();
                $.ajax({
                    url: _app.parseUrl(`/select/frentes-formula?projectFormulaId=${sg}`)
                }).done(function (result) {


                    $(".select2-workfront-filter").empty();
                    $(".select2-workfront-filter").append(allOption).trigger('change');
                    $(".select2-workfront-filter").select2({
                        data: result
                    });
                });
            }

        },

        sewergroup: function () {
            $.ajax({
                url: _app.parseUrl("/select/cuadrillas-proyecto")
            })
                .done(function (result) {
                    $(".select2-sewer-groups").select2({
                        data: result
                    });
                    $(".select2-sewergroup-filter").select2({
                        data: result
                    });
                });
        },
       
    };

    var events = {
        excel: function () {
            $("#btn-massive-load").on("click", function () {
                window.location = `/oficina-tecnica/metrados-replanteo-por-tramo/excel-carga-masiva`;
            });

        },
        init: function () {
            $("#budget_title_filter,#work_front_filter,#project_formula_filter,#sewer_group_filter").on("change", function () {
                datatables.meteredsDt.reload();
            });

            $(".select2-project-formulas").on("change", function () {
                select2.workfront.reload(this.value);
            });

            $(".select2-projectformula-filter").on("change", function () {
                select2.workfrontfilter.reload(this.value);
            });
            

            
        },
        deleteByFilters: function () {
            $("#btn-delete-by-filters").on("click", function () {
                Swal.fire({
                    title: "¿Está seguro?",
                    text: "Los aceros filtrados serán eliminados permanentemente",
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
                                url: _app.parseUrl(`/oficina-tecnica/metrados-replanteo-por-tramo/eliminar-filtro`),
                                data: {
                                    projectFormulaId: $("#project_formula_filter").val(),
                                    budgetTitleId: $("#budget_title_filter").val()
                                },
                                type: "delete",
                                success: function (result) {
                                    datatables.steelDt.reload();
                                    swal.fire({
                                        type: "success",
                                        title: "Completado",
                                        text: "Los metrados han sido eliminados con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (result) {
                                    console.log(result.responseText);
                                    swal.fire({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn-danger",
                                        animation: false,
                                        customClass: "animated tada",
                                        confirmButtonClass: "Entendido",
                                        text: result.responseText
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
        

        
    };

    return {
        init: function () {
            events.excel();
            events.init();
            select2.init();
            datatables.init();
            events.deleteByFilters();
            validate.init();
            modals.init();
        }
    };
}();

$(function () {
    Metered.init();
});