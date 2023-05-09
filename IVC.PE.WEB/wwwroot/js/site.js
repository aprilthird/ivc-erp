// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ----------------
// Adobe Viewer
// ----------------
var adobeId = "5808ad00ba8a467d8e7d5e030286738d";

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
        },
        employee: {
            workArea: {
                PRODUCTION: 1,
                TECHINAL_OFFICE: 2,
                QUALITY: 3,
                INTEGRATED_MANAGEMENT_SYSTEM: 4,
                ADMINISTRATION: 5,
                EQUIPMENT: 6,
                AGGREGATE_CONTROLLER: 7,
                WAREHOUSE: 8,
                SOCIAL_INTERVENTION: 9,
                ENVIRONMENT: 10,
                SECURITY_HEALTH: 11,
                DRIVER: 12,
                HUMAN_RESOURCES: 13,
                ARCHAEOLOGY: 14,
                LEGAL_AREA: 15,
                LOGISTIC: 16,
                VALUES: {
                    [1]: {
                        name: "Producción",
                        class: "success"
                    },
                    [2]: {
                        name: "Oficina Técnica",
                        class: "info"
                    },
                    [3]: {
                        name: "Calidad",
                        class: "warning"
                    },
                    [4]: {
                        name: "SIG",
                        class: "danger"
                    },
                    [5]: {
                        name: "Administración",
                        class: "primary"
                    },
                    [6]: {
                        name: "Equipos",
                        class: "success"
                    },
                    [7]: {
                        name: "Controlador de Agregados",
                        class: "info"
                    },
                    [8]: {
                        name: "Almacén",
                        class: "warning"
                    },
                    [9]: {
                        name: "Intervención Social",
                        class: "danger"
                    },
                    [10]: {
                        name: "Medio Ambiente",
                        class: "success"
                    },
                    [11]: {
                        name: "Seguridad y Salud en el Trabajo",
                        class: "primary"
                    },
                    [12]: {
                        name: "Choferes",
                        class: "info"
                    },
                    [13]: {
                        name: "RRHH",
                        class: "primary"
                    },
                    [14]: {
                        name: "Arqueología",
                        class: "warning"
                    },
                    [15]: {
                        name: "Área Legal",
                        class: "info"
                    },
                    [16]: {
                        name: "Logística",
                        class: "info"
                    },
                    [17]: {
                        name: "Licitaciones",
                        class: "info"
                    }
                }
            }
        },
        budget: {
            group: {
                MAIN_COMPONENT: 1,
                OTHER_COMPONENTS: 2,
                VALUES: {
                    [1]: {
                        name: "Componente Principal",
                        class: "primary"
                    },
                    [2]: {
                        name: "Otros Componentes",
                        class: "info"
                    }
                }
            },
            type: {
                CONTRACTUAL: 1,
                COTRACTUAL_HIGHER_ESTIMATE: 2,
                ADDITIONAL_HIGHER_ESTIMATE: 3,
                ADDITIONAL_NEW_RECORD: 4,
                DEDUCTIVE: 5,
                GOAL_REDUCTION: 6,
                VALUES: {
                    [1]: {
                        name: "Contractual",
                        class: "primary"
                    },
                    [2]: {
                        name: "Contractual - Mayor Metrado",
                        class: "success"
                    },
                    [3]: {
                        name: "Adicional - Mayor Metrado",
                        class: "success"
                    },
                    [4]: {
                        name: "Adicional - Nueva Partida",
                        class: "info"
                    },
                    [5]: {
                        name: "Deductivo",
                        class: "warning"
                    },
                    [6]: {
                        name: "Reducción de Metas",
                        class: "danger"
                    }
                }
            }
        },
        certificate: {
            fillingLaboratory: {
                materialType: {
                    OWN_FILLING: 1,
                    COMMON_FILLING: 2,
                    LOAN_FILLING: 3,
                    AFFIRMED: 4,
                    VALUES: {
                        [1]: {
                            name: "RELLENO PROPIO",
                            class: "primary"
                        },
                        [2]: {
                            name: "RELLENO COMÚN",
                            class: "info"
                        },
                        [3]: {
                            name: "RELLENO PRESTAMO",
                            class: "warning"
                        },
                        [4]: {
                            name: "AFIRMADO",
                            class: "success"
                        },
                        [5]: {
                            name: "BASE GRANULAR",
                            class: "dark"
                        }
                    }
                },
                originType: {
                    OWN_EXCAVATION: 1,
                    VALUES: {
                        [1]: {
                            name: "Material Propio de Excavación",
                            class: "primary"
                        }
                    }
                }
            },
            concreteQuality: {
                age: {
                    SEVEN: 7,
                    TWENTY_EIGHT: 28,
                    VALUES: {
                        [7]: {
                            name: "7",
                            class: "primary"
                        },
                        [28]: {
                            name: "28",
                            class: "success"
                        }
                    }
                },
                segment: {
                    SLAB: 1,
                    BODY: 2,
                    ROOF: 3,
                    VALUES: {
                        [1]: {
                            name: "Losa",
                            class: "primary"
                        },
                        [2]: {
                            name: "Cuerpo",
                            class: "info"
                        },
                        [3]: {
                            name: "Techo",
                            class: "success"
                        }
                    }
                }
            }
        },
        equipment: {
            status: {
                OPERATIONAL: 1,
                IN_REPAIR: 2,
                NOT_OPERATIONAL: 3,
                STOLEN: 4,
                DISCARDED: 5,
                VALUES: {
                    [1]: {
                        name: "Operativo",
                        class: "success"
                    },
                    [2]: {
                        name: "En Reparación",
                        class: "warning"
                    },
                    [3]: {
                        name: "No Operativo",
                        class: "danger"
                    },
                    [4]: {
                        name: "Robado",
                        class: "danger"
                    },
                    [5]: {
                        name: "Descartado",
                        class: "danger"
                    }
                }
            }
        },
        letter: {
            documentType: {
                LETTER: 1,
                REPORT: 2,
                MEETING_RECORD: 3,
                AGREEMENT_MINUTE: 4,
                WORKBOOK: 5,
                ANNOUNCEMENT: 6,
                MEMORANDUM: 7,
                DESCRIPTIVE_MEMORY: 8,
                VALUES: {
                    [1]: {
                        name: "Carta",
                        class: "info"
                    },
                    [2]: {
                        name: "Informe",
                        class: "primary"
                    },
                    [3]: {
                        name: "Acta de Reunión",
                        class: "info"
                    },
                    [4]: {
                        name: "Acta de Acuerdo",
                        class: "success"
                    },
                    [5]: {
                        name: "Cuaderno de Obra",
                        class: "info"
                    },
                    [6]: {
                        name: "Anuncio",
                        class: "info"
                    },
                    [7]: {
                        name: "Memorandum",
                        class: "danger"
                    },
                    [8]: {
                        name: "Memoria Descriptiva",
                        class: "info"
                    }
                }
            },
            status: {
                INFORMATIVE: 1,
                QUERY: 2,
                QUERY_US: 3,
                ANSWER: 4,
                APPROVAL: 5,
                OBSERVATION: 6,
                COMPLEMENT: 7,
                AUTHORIZATION_REQUEST: 8,
                VALORIZATION: 9,
                APPROVAL_REQUEST: 10,
                VALUES: {
                    [1]: {
                        name: "Informativo",
                        class: "info"
                    },
                    [2]: {
                        name: "Consultamos",
                        class: "primary"
                    },
                    [3]: {
                        name: "Nos consultan",
                        class: "warning"
                    },
                    [4]: {
                        name: "Respondemos",
                        class: "primary"
                    },
                    [5]: {
                        name: "Aprueban",
                        class: "success"
                    },
                    [6]: {
                        name: "Observan",
                        class: "danger"
                    },
                    [7]: {
                        name: "Complemento",
                        class: "warning"
                    },
                    [8]: {
                        name: "Solicitud de autorización",
                        class: "warning"
                    },
                    [9]: {
                        name: "Valorización",
                        class: "success"
                    },
                    [10]: {
                        name: "Solicitamos aprobación",
                        class: "warning"
                    }
                }
            }
        },
        pipeline: {
            type: {
                PVC: 1,
                VALUES: {
                    [1]: {
                        name: "PVC",
                        class: "success"
                    }
                }
            },
            class: {
                SN2: 1,
                SN4: 2,
                SN8: 3,
                VALUES: {
                    [1]: {
                        name: "SN-2",
                        class: "success"
                    },
                    [2]: {
                        name: "SN-4",
                        class: "info"
                    },
                    [3]: {
                        name: "SN-8",
                        class: "danger"
                    }
                }
            }
        },
        sewer: {
            group: {
                destination: {
                    LOCAL: 1,
                    COLLABORATOR: 2,
                    VALUES: {
                        [1]: {
                            name: "Casa",
                            class: "success"
                        },
                        [2]: {
                            name: "Colaborador",
                            class: "info"
                        }
                    }
                },
                workComponent: {
                    GENERAL: 1,
                    SECONDARY: 2,
                    OVERALL: 3,
                    OTHER: 4,
                    VALUES: {
                        [1]: {
                            name: "Obras Generales",
                            class: "success"
                        },
                        [2]: {
                            name: "Obras Secundarias",
                            class: "warning"
                        },
                        [3]: {
                            name: "Todo",
                            class: "primary"
                        },
                        [4]: {
                            name: "Otros Componentes",
                            class: "info"
                        }
                    }
                },
                workStructure: {
                    MASSIVE_EARTHWORKS: 1,
                    CIVIL_WORKS: 2,
                    CONDUCTION_LINE: 3,
                    DRIVE_LINES: 4,
                    STRATEGIC_TRUNKS: 5,
                    AIR_PURGE_LINE_CHAMBERS: 6,
                    REDUCING_CHAMBERS: 7,
                    COLLECTORS: 8,
                    OVERFLOWS: 9,
                    NETWORKS_CONNECTIONS: 10,
                    DISCHARGE_COLLECTOR: 11,
                    TOPOGRAPHY: 12,
                    PITS: 13,
                    ELECTRICAL_EQUIPMENT: 14,
                    HYDRAULIC_EQUIPMENT: 15,
                    MACHINERY_EQUIPMENT_MANAGEMENT: 16,
                    WAREHOUSE_MANAGEMENT: 17,
                    TRAFFIC_DIVERSION_MANAGEMENT: 18,
                    ENVIRONMENT_MANAGEMENT: 19,
                    UNION_MANAGEMENT: 20,
                    VALUES: {
                        [1]: {
                            name: "Movimiento de Tierras Masivas",
                            class: "primary"
                        },
                        [2]: {
                            name: "Obras Civiles",
                            class: "primary"
                        },
                        [3]: {
                            name: "Línea de Conducción",
                            class: "info"
                        },
                        [4]: {
                            name: "Líneas de Impulsión",
                            class: "info"
                        },
                        [5]: {
                            name: "Troncales Estratégicas",
                            class: "info"
                        },
                        [6]: {
                            name: "Cámaras de Aire, Purga y de Línea",
                            class: "info"
                        },
                        [7]: {
                            name: "Cámaras Reductoras",
                            class: "info"
                        },
                        [8]: {
                            name: "Colectores",
                            class: "info"
                        },
                        [9]: {
                            name: "Reboses",
                            class: "info"
                        },
                        [10]: {
                            name: "Redes y Conexiones",
                            class: "warning"
                        },
                        [11]: {
                            name: "Colector de Descarga",
                            class: "success"
                        },
                        [12]: {
                            name: "Topografía",
                            class: "info"
                        },
                        [13]: {
                            name: "Calicatas",
                            class: "success"
                        },
                        [14]: {
                            name: "Equipamiento Eléctrico",
                            class: "warning"
                        },
                        [15]: {
                            name: "Equipamiento Hidráulico",
                            class: "warning"
                        },
                        [16]: {
                            name: "Gestión de Maquinarias y Equipos",
                            class: "primary"
                        },
                        [17]: {
                            name: "Gestión de Almacén",
                            class: "primary"
                        },
                        [18]: {
                            name: "Gestión de Tránsito y Desvíos",
                            class: "primary"
                        },
                        [19]: {
                            name: "Gestión del Medio Ambiente",
                            class: "primary"
                        },
                        [20]: {
                            name: "Manejo Sindical",
                            class: "primary"
                        },
                    }
                }
            },
            box: {
                type: {
                    I: 1,
                    II: 2,
                    VALUES: {
                        [1]: {
                            name: "Tipo I",
                            class: "success"
                        },
                        [2]: {
                            name: "Tipo II",
                            class: "info"
                        },
                    }
                }
            }
        },
        finance: {
            bondType: {
                NORMAL: 1,
                SEMIROCOUS: 2,
                ROCKY: 3,
                VALUES: {
                    [1]: {
                        name: "Fiel Cumplimiento",
                        class: "success"
                    },
                    [2]: {
                        name: "Adelanto Directo N°01",
                        class: "info"
                    },
                    [3]: {
                        name: "Adelanto Directo N°02",
                        class: "danger"
                    }
                }
            }
        },
        terrain: {
            type: {
                NORMAL: 1,
                SEMIROCOUS: 2,
                ROCKY: 3,
                VALUES: {
                    [1]: {
                        name: "Normal",
                        code: "N",
                        class: "success"
                    },
                    [2]: {
                        name: "Semirocoso",
                        code: "SR",
                        class: "info"
                    },
                    [3]: {
                        name: "Rocoso",
                        code: "R",
                        class: "danger"
                    }
                }
            }
        },
        workbook: {
            status: {
                PENDING: 1,
                RESOLVED: 2,
                DELIVERED: 3,
                VALUES: {
                    [1]: {
                        name: "Pendiente",
                        class: "warning"
                    },
                    [2]: {
                        name: "Resuelto",
                        class: "success"
                    },
                    [3]: {
                        name: "Informción Entregada",
                        class: "primary"
                    }
                }
            },
            wroteBy: {
                CONTRACTOR: 1,
                SUPERVISION: 2,
                VALUES: {
                    [1]: {
                        name: "Contratista",
                        class: "primary"
                    },
                    [2]: {
                        name: "Supervisión",
                        class: "warning"
                    }
                }
            },
            type: {
                QUERY: 1,
                DELAY: 2,
                ADDITIONAL_METER: 3,
                ADDITIONAL_NEW_ITEM: 4,
                PERIOD_EXTENSION: 5,
                REPORT: 6,
                STAKEOUT: 7,
                PROGRESS: 8,
                GOAL_REDUCTION: 9,
                QUALITY: 10,
                KNOWLEDGE_TAKING: 11,
                COMMUNICATE_PROGRESS: 12,
                CHECK_PROGRESS: 13,
                AUTHORIZATION_REQUEST: 14,
                AUTHORIZATION_GRANTED: 15,
                ADDITIONAL_METER_GRANTED: 16,
                QUERY_RESOLUTION: 17,
                INFORMATION_REQUEST: 18,
                INFORMATION_DELIVERY: 19,
                VALUES: {
                    [1]: {
                        name: "Consulta",
                        class: "primary"
                    },
                    [2]: {
                        name: "Restricción o demora",
                        class: "danger"
                    },
                    [3]: {
                        name: "Adicional mayor metrado",
                        class: "info"
                    },
                    [4]: {
                        name: "Adicional nueva partida",
                        class: "info"
                    },
                    [5]: {
                        name: "Ampliación de plazo",
                        class: "success"
                    },
                    [6]: {
                        name: "Informativo",
                        class: "info"
                    },
                    [7]: {
                        name: "Replanteos",
                        class: "warning"
                    },
                    [8]: {
                        name: "Valorización y avances",
                        class: "success"
                    },
                    [9]: {
                        name: "Deductivos o reducción de metas",
                        class: "warning"
                    },
                    [10]: {
                        name: "Calidad",
                        class: "info"
                    },
                    [11]: {
                        name: "Toma de conocimiento",
                        class: "info"
                    },
                    [12]: {
                        name: "Comunicación de avances",
                        class: "info"
                    },
                    [13]: {
                        name: "Avance verificado",
                        class: "info"
                    },
                    [14]: {
                        name: "Solicitud de autorización",
                        class: "info"
                    },
                    [15]: {
                        name: "Autorización otorgada",
                        class: "info"
                    },
                    [16]: {
                        name: "Mayor metrado garantizado",
                        class: "info",
                    },
                    [17]: {
                        name: "Absolución de consulta",
                        class: "success",
                    },
                    [18]: {
                        name: "Solicitud de información",
                        class: "warning",
                    },
                    [19]: {
                        name: "Entrega de información",
                        class: "success",
                    }
                }
            }
        },
        worker: {
            category: {
                PAWN: 1,
                OPERATOR: 2,
                OFFICIAL: 3,
                SYNDICATE_PAWN: 4,
                POPULATION_PAWN: 5,
                VALUES: {
                    [1]: {
                        name: "Peón",
                        code: "PE",
                        class: "primary"
                    },
                    [2]: {
                        name: "Operador",
                        code: "OP",
                        class: "info"
                    },
                    [3]: {
                        name: "Oficial",
                        code: "OF",
                        class: "warning"
                    },
                    [4]: {
                        name: "Peón de Sindicato",
                        code: "PE-S",
                        class: "success"
                    },
                    [5]: {
                        name: "Peón de Población",
                        code: "PE-P",
                        class: "danger"
                    }
                }
            }
        },
        workFront: {
        }
    },
    render: {
        measure: function (number) {
            return number.toMeasure();
        },
        percent: function (number) {
            return number.toPercent();
        },
        money: function (number) {
            return number.toMoney();
        },
        badges: function (values, dictionary, shorter = false) {
            return values.map(x => _app.render.badge(x, dictionary, shorter)).join(" ");
        },
        badge: function (value, dictionary, shorter = false) {
            return value && dictionary[value]
                ? `<span class="kt-badge kt-badge--inline kt-badge--pill kt-badge--${dictionary[value].class}">${shorter ? dictionary[value].code : dictionary[value].name}</span>`
                : "";
        },
        labels: function (values, dictionary, shorter = false) {
            return values.map(x => _app.render.label(x, dictionary, shorter).join(" "));
        },
        label: function (value, dictionary, shorter = false) {
            return value && dictionary[value]
                ? `<span class="btn btn-bold btn-sm btn-font-sm btn-label-${dictionary[value].class}">${shorter ? dictionary[value].code : dictionary[value].name}</span>`
                : "";
        },
        ribbons: function (values, dictionary, shorter = false) {
            return values.map(x => _app.render.ribbon(x, dictionary, shorter)).join(" ");
        },
        ribbon: function (value, dictionary, shorter = false) {
            return value && dictionary[value]
                ? `<span class="kt-badge kt-badge--${dictionary[value].class} kt-badge--dot"></span>&nbsp;<span class="kt-font-bold kt-font-${dictionary[value].class}">${shorter ? dictionary[value].code : dictionary[value].name}</span>`
                : "";
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
            addRange: {
                success: function () {
                    toastr.success("Registros ingresados correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo completar los registros.", "Error");
                }
            }
        }
    }
};


// ----------
// DataTables
// ----------
$.extend($.fn.dataTable.defaults, {
    dom: '<"top row"<"col-md-6"B><"col-md-6"f>>rt<"bottom row"<"col-md-4"i><"col-md-4"l><"col-md-4"p>>',
    buttons: [
        { extend: "copyHtml5", text: "<i class='fa fa-copy'></i> Copiar", className: " btn-dark" },
        { extend: "excelHtml5", text: "<i class='fa fa-file-excel'></i> Excel", className: "btn-success" },
        { extend: "csvHtml5", text: "<i class='fa fa-file-csv'></i> CSV", className: "btn-success" },
        { extend: "pdfHtml5", text: "<i class='fa fa-file-pdf'></i> PDF", className: "btn-danger" },
        { extend: "print", text: "<i class='fa fa-print'></i> Imprimir", className: "btn-dark" }
    ],
    language: {
        "sProcessing": "<div class='m-blockui' style='display: inline; background: none; box-shadow: none;'><span>Cargando...</span><span><div class='m-loader  m-loader--brand m-loader--lg'></div></span></div>",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando _START_ - _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 - 0 de 0 registros",
        "sInfoFiltered": "(filtrado de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        //"oPaginate": {
        //    "sFirst": "<i class='la la-angle-double-left'></i>",
        //    "sLast": "<i class='la la-angle-double-right'></i>",
        //    "sNext": "<i class='la la-angle-right'></i>",
        //    "sPrevious": "<i class='la la-angle-left'></i>"
        //},
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    lengthMenu: [10, 25, 50],
    orderMulti: false,
    pagingType: "full_numbers",
    responsive: true,
    info: true,
    order: []
});

jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "date-uk-pre": function (a) {
        var ukDatea = a.split('/');
        return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
    },

    "date-uk-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "date-uk-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});

// ----------
// jQuery
// ----------
(function ($) {
    $.fn.addLoader = function () {
        this.addClass("kt-spinner kt-spinner--right kt-spinner--light").attr("disabled", true);
        return this;
    };

    $.fn.removeLoader = function () {
        this.removeClass("kt-spinner kt-spinner--right kt-spinner--light").attr("disabled", false);
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
    value: function toRound(isPercent = false, decimals, mode = "none", char = "") {
        var num = this;
        if (isPercent) {
            //num *= 100;*
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
    value: function toPercent(decimals = 2) {
        var num = this;
        return num.toRound(true, decimals, "append", "%");
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

Object.defineProperty(Number.prototype, "toMeasure", {
    value: function toMoney(decimals = 2) {
        var num = this;
        return num.toRound(false, decimals);
    },
    writable: true,
    configurable: true
});

// ----------------
// General Events
// ----------------
$(".btn-export").on("click",
    function () {
        let $btn = $(this);
        $btn.addLoader();
        let url = $(this).prop("href");
        console.log(url);
        //if (url !== "#" || url !== undefined) {
        //    $.fileDownload(url)
        //        .always(function () {
        //            $btn.removeLoader();
        //        }).done(function () {
        //            _app.show.notification.download.success();
        //        }).fail(function () {
        //            _app.show.notification.download.error();
        //        });
        /*} else {*/
            setTimeout(function () {
                $btn.removeLoader();
                //_app.show.notification.download.success();
            }, 2000);
        //}
    });