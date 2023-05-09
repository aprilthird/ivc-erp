using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace IVC.PE.CORE.Helpers
{
    public static class ConstantHelpers
    {
        public static class Seed
        {
            public const bool ENABLED = false;
            public const bool SEWER_LINES = false;
        }

        public static class Project
        {
            public const string TITLE = "Sistema de Gestión IVC";
        }

        public static class DashboardUtils
        {
            public static class Colors
            {
                public const string RED = "red";
                public const string BLUE = "blue";
            }

        }

        public static class EntityStatus
        {
            public const bool ENABLED = true;
            public const bool DISABLED = false;

            public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
            {
                { ENABLED, "Activo" },
                { DISABLED, "Inactivo" },
            };
        }

        public static class DocumentType
        {
            public const int ID_CARD = 1;
            public const int INMIGRATION_CARD = 2;
            public const int TAX_IDENTIFICATION_NUMBER = 3;
            public const int PASSPORT = 4;
            public const int BIRTH_CERTIFICATE = 5;
            public const int OTHER = 6;

            public const string ID_CARD_CODE = "DNI";
            public const string INMIGRATION_CARD_CODE = "CARNET EXT.";
            public const string TAX_IDENTIFICATION_NUMBER_CODE = "RUC";
            public const string PASSPORT_CODE = "PASAPORTE";
            public const string BIRTH_CERTIFICATE_CODE = "P. NAC.";
            public const string OTHER_CODE = "OTROS";

            public const string ID_CARD_SCODE = "DNI";
            public const string INMIGRATION_CARD_SCODE = "CEX";
            public const string TAX_IDENTIFICATION_NUMBER_SCODE = "RUC";
            public const string PASSPORT_SCODE = "PAS";
            public const string BIRTH_CERTIFICATE_SCODE = "PNA";
            public const string OTHER_SCODE = "OTR";

            public const string ID_CARD_NAME = "Documento Nacional de Identidad";
            public const string INMIGRATION_CARD_NAME = "Carnet de Extranjería";
            public const string TAX_IDENTIFICATION_NUMBER_NAME = "Registro Único de Contribuyentes";
            public const string PASSPORT_NAME = "Pasaporte";
            public const string BIRTH_CERTIFICATE_NAME = "Partida de Nacimiento";
            public const string OTHER_NAME = "Otros";

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ID_CARD, ID_CARD_CODE },
                { INMIGRATION_CARD, INMIGRATION_CARD_CODE },
                { TAX_IDENTIFICATION_NUMBER, TAX_IDENTIFICATION_NUMBER_CODE },
                { PASSPORT, PASSPORT_CODE },
                { BIRTH_CERTIFICATE, BIRTH_CERTIFICATE_CODE },
                { OTHER, OTHER_CODE }
            };

            public static Dictionary<int, string> SVALUES = new Dictionary<int, string>()
            {
                { ID_CARD, ID_CARD_SCODE },
                { INMIGRATION_CARD, INMIGRATION_CARD_SCODE },
                { TAX_IDENTIFICATION_NUMBER, TAX_IDENTIFICATION_NUMBER_SCODE },
                { PASSPORT, PASSPORT_SCODE },
                { BIRTH_CERTIFICATE, BIRTH_CERTIFICATE_SCODE },
                { OTHER, OTHER_SCODE }
            };

            public static Dictionary<int, string> NAMES = new Dictionary<int, string>()
            {
                { ID_CARD, ID_CARD_NAME },
                { INMIGRATION_CARD, INMIGRATION_CARD_NAME },
                { TAX_IDENTIFICATION_NUMBER, TAX_IDENTIFICATION_NUMBER_NAME },
                { PASSPORT, PASSPORT_NAME },
                { BIRTH_CERTIFICATE, BIRTH_CERTIFICATE_NAME },
                { OTHER, OTHER_NAME }
            };

            public static Dictionary<int, (string Code, string Name, int Length, bool ExactLength, bool IsNumeric)> OBJECTS = new Dictionary<int, (string ShortName, string Name, int Length, bool ExactLength, bool IsNumeric)>
            {
                { ID_CARD, (ID_CARD_CODE, ID_CARD_NAME, 8, true, true) },
                { INMIGRATION_CARD, (INMIGRATION_CARD_CODE, INMIGRATION_CARD_NAME, 12, false, false) },
                { TAX_IDENTIFICATION_NUMBER, (TAX_IDENTIFICATION_NUMBER_CODE, TAX_IDENTIFICATION_NUMBER_NAME, 11, true, true) },
                { PASSPORT, (PASSPORT_CODE, PASSPORT_NAME, 12, false, false) },
                { BIRTH_CERTIFICATE, (BIRTH_CERTIFICATE_CODE, BIRTH_CERTIFICATE_NAME, 15, false, false) },
                { OTHER, (OTHER_CODE, OTHER_NAME, 15, false, false) }
            };
        }

        public static class TimeZone
        {
            public const string DEFAULT_WINDOWS_ID = "SA Pacific Standard Time";
            public const string DEFAULT_LINUX_ID = "America/New_York";
            public static string DEFAULT_ID => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DEFAULT_WINDOWS_ID : DEFAULT_LINUX_ID;
        }

        public static class Format
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
        }

        public static class Permissions
        {
            public static class Level
            {
                public const int VIEW = 0;
                public const int MANAGE = 1;


            }
        }
        public static class Roles
        {
            public const string SUPERADMIN = "Superadmin";
            public const string CENTRAL_OFFICE = "Oficina Central";
            public const string PRODUCTION = "Producción";
            public const string TECHNICAL_OFFICE = "Oficina Técnica";
            public const string TECHNICAL_SUPPORT = "Soporte Técnico";
            public const string BASIC_SUPPORT = "Soporte Básico";
            public const string QUALITY = "Calidad";
            public const string INTEGRATED_MANAGEMENT_SYSTEM = "SIG";
            public const string HUMAN_RESOURCES = "RRHH";
            public const string ADMINISTRATION = "Administración";
            public const string SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY = "IS-MA-SST-ARQ";
            public const string WAREHOUSE = "Almacenes";
            public const string BIDDING = "Licitaciones";
        }

        public static class Areas
        {
            public const string ADMIN = "Admin";
            public const string ARCHEOLOGY = "Archeology";
            public const string AGGREGATION = "Aggregation";
            public const string BIDDING = "Bidding";
            public const string DOCUMENTARY_CONTROL = "DocumentaryControl";
            public const string ENVIRONMENT = "Environment";
            public const string EQUIPMENT_MACHINERY = "EquipmentMachinery";
            public const string FINANCE = "Finance";
            public const string HUMAN_RESOURCES = "HumanResources";
            public const string INTEGRATED_MANAGEMENT_SYSTEM = "IntegratedManagementSystem";
            public const string LEGAL_TECHNICAL_LIBRARY = "LegalTechnicalLibrary";
            public const string LOGISTICS = "Logistics";
            public const string QUALITY = "Quality";
            public const string REPORTS = "Reports";
            public const string SECURITY = "Security";
            public const string SOCIAL_INTERVENTION = "SocialIntervention";
            public const string TECHNICAL_OFFICE = "TechnicalOffice";
            public const string PRODUCTION = "Production";
            public const string WAREHOUSE = "Warehouse";
            public const string ACCOUNTING = "Accounting";
        }

        public static class Storage
        {
            public static class Containers

            {
                public const string BIDDING = "licitaciones";
                public const string FINANCE = "finanzas";
                public const string FUEL_PROVIDERS = "proveedores-combustible";
                public const string BUSINESS = "empresas";
                public const string LOGISTICS = "logistica";
                public const string TECHNICAL_OFFICE = "oficina-tecnica";
                public const string EQUIPMENT_MACHINERY = "equipos";
                public const string QUALITY = "calidad";
                public const string DOCUMENTARY_CONTROL = "control-documentario";
                public const string INTEGRATED_MANAGEMENT_SYSTEM = "sistema-de-manejo-integrado";
                //Container para libreria-tecnica
                public const string TECHNICAL_LIBRARY = "biblioteca-tecnica";
                public const string LEGAL_LIBRARY = "biblioteca-legal";
                public const string MEDICAL_REST = "descansos-medicos";
                public const string PRODUCTION = "produccion";
                //Container para Seguridad
                public const string SECURITY = "seguridad";
                //Container para Admin
                public const string PROJECTS = "proyectos";
                public const string USERS = "usuarios";
                //Container para HR
                public const string WORKERS = "obreros";

                public const string WAREHOUSE = "almacen";
                public const string ACCOUNTING = "contabilidad";
            }

            public static class Blobs
            {

                //Blobs para licitaciones
                public const string CONTRACT = "contrato";
                public const string INVOICE = "factura";
                public const string LIQUIDATION = "liquidacion";
                public const string ACT = "acta";
                public const string CONFIRMED = "conformidad";
                public const string EXPERIENCE = "experiencia";
                public const string TITLE_PROFESSIONAL = "titulo-profesional";
                public const string CAPACITATION_PROFESSIONAL = "capacitacion";
                public const string DNI_PROFESSIONAL = "dni";
                public const string CIP_PROFESSIONAL = "cip";
                public const string CERTI_PROFESSIONAL = "cip";
                public const string LEGAL_DOCUMENTATION = "documentaciones-legal";
                public const string SKILL = "habilidades";
                public const string BUSINESS = "empresas";
                //Blobs para calidad
                public const string CONCRETE_QUALITY_CERTIFICATE = "certificados-concreto";
                public const string COMPACTION_DENSITY_CERTIFICATE = "certificados-densidad-compactacion";
                public const string FILLING_LABORATORY_TEST = "pruebas-laboratorio-relleno";
                public const string FOR_07 = "for-07";
                public const string FOR_47 = "for-47";
                public const string FOR_05 = "for-05";
                public const string FOR_29 = "for-29";
                public const string FOR_37A = "for-37a";
                public const string FOR_24_FIRST_PART = "for-24-first-part";
                public const string FOR_24_FIRST_PART_VIDEO = "for-24-first-part-video";
                public const string FOR_24_FIRST_PART_GALLERY = "for-24-first-part-gallery";
                public const string FOR_24_SECOND_PART = "for-24-second-part";
                public const string FOR_24_SECOND_PART_VIDEO = "for-24-second-part-video";
                public const string FOR_24_SECOND_PART_GALLERY = "for-24-second-part-gallery";
                public const string FOR_24_THIRD_PART = "for-24-third-part";
                public const string DISCHARGE_MANIFOLD = "colector-descarga-for01";
                public const string EQUIPMENT_CERTIFICATES = "certificados-equipos";
                public const string INSTRUCTIONAL_PROCEDURES = "procedimientos-instructivos";
                public const string SIG_PROCESSES = "procesos-sig";
                public const string CALIBRATION_PATTERNS = "patron-calibracion";
                //Blobs para control documentario
                public const string LETTERS_SENT = "cartas-enviadas";
                public const string LETTERS_RECEIVED = "cartas-recibidas";
                public const string WORKBOOKS_PDF = "libros-de-obra-pdf";
                public const string WORKBOOKS_WORD = "libros-de-obra-word";
                public const string WORKBOOK_SEATS = "cuadernos-de-obra";
                public const string PERMS = "permisos";
                //Blobs para equipos
                public const string EQUIPMENT_MACHINERY_OPERATORS = "operadores";
                public const string EQUIPMENT_MACHINERY_INSURANCES = "polizas-seguro";
                public const string EQUIPMENT_MACH_INSURANCES = "polizas-seguro-maquinaria";
                public const string EQUIPMENT_MACH_SOAT = "seguro-soat-maquinaria";
                public const string EQUIPMENT_MACH_TECHNICAL_REVISION = "revision-tecnica-maquinaria";
                public const string EQUIPMENT_MACHINERY_SOAT = "seguro-soat";
                public const string EQUIPMENT_MACHINERY_TECHNICAL_REVISION = "revision-tecnica";
                //Blobs para oficina tecnica
                public const string EXPENSES_UTILITY = "gastos-utilidades";
                //Blobs para libreria-tecnica
                public const string ISO_STANDARD = "normas-iso";
                public const string GENERAL_CONSTRUCTION_PROCEDURES = "procedimientos-constructivos-generales";
                public const string PERUVIAN_TECHNICAL_STANDAR = "normas-tecnicas-peruanas";
                public const string SEDAPAL_TECHNICAL_SPECIFICATION = "especificaciones-tecnicas-sedapal";
                public const string SUPPLIER_CATALOG = "catalogo-proveedores";
                public const string GENERAL_POLICY = "politicas-generales";
                public const string MANUAL_TECHNICAL_BOOK = "manuales-libros-tecnicos-diversos";
                public const string BIM = "bim";
                public const string PROCEDURE = "procedimiento";
                public const string BLUEPRINT = "plano";
                public const string MANUAL = "manual";
                public const string DESIGN = "diseño-de-mezcla";
                public const string TECHNICAL_SPEC = "especificaciones-tecnicas";

                public const string CATALOG_PROVIDER = "catalogo-de-proveedores";
                public const string TECHNICAL_LIBRARY = "biblioteca-tecnica";
                //Blobs para logística
                public const string PROVIDER_TAX_FILES = "proveedores-archivos-tributarios";
                public const string PROVIDER_LEGAL_FILES = "proveedores-archivos-legales";
                public const string PROVIDER_COMMERCIAL_FILES = "proveedores-archivos-comerciales";
                public const string ORDER_PRICE = "ordenes-cotizaciones";
                public const string ORDER_SUPPORT = "ordenes-sustento";
                public const string ORDER_PDF = "ordenes-pdf";
                public const string REQUEST_FILES = "requerimientos-adjuntos";
                public const string PREREQUEST_FILES = "pre-requerimientos-adjuntos";
                //Blobs para descansos médicos
                public const string MEDICAL_REST = "descansos-medicos";
                public const string MEDICAL_APPOINTMENTS = "citas-medicas";
                //Blbos para finanzas
                public const string BOND_LETTERS = "cartas-fianza";
                //Blobs para produccion
                public const string RDP = "rdp";
                //Blobs para seguridad
                public const string RACS = "racs";
                public const string TRAINING_TOPIC = "temas-de-capacitacion";
                //Blobs para proveedores de Combustible
                public const string FUEL_PRICES = "precios-de-combustible";
                //Blobs para administracion
                public const string LOGOS = "logos";
                public const string SIGNATURE = "firmas";
                //Blobs para recursos humanos
                public const string WORKERS_PHOTOS = "fotos-obreros";

                //Blobs para almacen
                public const string SUPPLY_ENTRY = "ingreso-material";
                public const string RE_ENTRY_FOR_RETURN = "reingreso-por-devolucion";
                public const string FIELD_REQUEST = "pedido-de-campo";

                //Blobs para Contabilidad
                public const string ACCOUNTING_INVOICE = "contabilidad-factura";
            }
        }

        public static class Datatable
        {
            public static class ServerSide
            {
                public static class Default
                {
                    public const string ORDER_DIRECTION = "DESC";
                }

                public static class SentParameters
                {
                    public const string DRAW_COUNTER = "draw";
                    public const string PAGING_FIRST_RECORD = "start";
                    public const string RECORDS_PER_DRAW = "length";
                    public const string SEARCH_VALUE = "search[value]";
                    public const string SEARCH_REGEX = "search[regex]";
                    public const string ORDER_COLUMN = "order[0][column]";
                    public const string ORDER_DIRECTION = "order[0][dir]";
                }
            }
        }

        public static class PillStyle
        {
            public const int PRIMARY = 0;
            public const int INFO = 1;
            public const int SUCCESS = 2;
            public const int DANGER = 3;
            public const int WARNING = 4;
            public const int DARK = 5;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {PRIMARY, "Azul" },
                    {INFO, "Morado" },
                    {SUCCESS, "Verde" },
                    {DANGER, "Rojo" },
                    {WARNING, "Naranja" },
                    {DARK, "Negro" }
                };
        }

        public static class Bank
        {
            public static class AccountType
            {
                public const int SALARY = 1;
                public const int SAVINGS = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { SALARY, "Corriente" },
                    { SAVINGS, "Ahorros" }
                };
            }
        }

        public static class Currency
        {
            public const int NUEVOS_SOLES = 1;
            public const int AMERICAN_DOLLARS = 2;
            public const int EUROS = 3;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { NUEVOS_SOLES, "Nuevos Soles" },
                    { AMERICAN_DOLLARS, "Dólares Americanos" },
                    { EUROS, "Euros"}
                };

            public static Dictionary<int, string> SIGN_VALUES = new Dictionary<int, string>
                {
                    { NUEVOS_SOLES, "S/" },
                    { AMERICAN_DOLLARS, "US$" },
                    { EUROS, "€"}
                };

            public static Dictionary<int, string> FOREIGN_VALUES = new Dictionary<int, string>
                {
                    { AMERICAN_DOLLARS, "Dólares Americanos" },
                    { EUROS, "Euros"}
                };
        }

        public static class Budget
        {
            public static class Group
            {
                public const int MAIN_COMPONENT = 1;
                public const int OTHER_COMPONENTS = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { MAIN_COMPONENT, "Componente Principal" },
                    { OTHER_COMPONENTS, "Otros Componentes" }
                };
            }

            public static class Type
            {
                public const int CONTRACTUAL = 1;
                public const int COTRACTUAL_HIGHER_ESTIMATE = 2;
                public const int ADDITIONAL_HIGHER_ESTIMATE = 3;
                public const int ADDITIONAL_NEW_RECORD = 4;
                public const int DEDUCTIVE = 5;
                public const int GOAL_REDUCTION = 6;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { CONTRACTUAL, "Contractual" },
                    { COTRACTUAL_HIGHER_ESTIMATE, "Contractual - Mayor Metrado" },
                    { ADDITIONAL_HIGHER_ESTIMATE, "Adicional - Mayor Metrado" },
                    { ADDITIONAL_NEW_RECORD, "Adicional - Partida Nueva" },
                    { DEDUCTIVE, "Deductivo" },
                    { GOAL_REDUCTION, "Reducción de Metas" }
                };
            }

        }

        public static class Certificate
        {
            public static class FillingLaboratory
            {
                public static class OriginType
                {
                    public const int OWN_EXCAVATION = 1;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { OWN_EXCAVATION, "MATERIAL PROPIO DE EXCAVACION" },
                    };
                }

                public static class MaterialType
                {
                    public const int OWN_FILLING = 1;
                    public const int COMMON_FILLING = 2;
                    public const int LOAN_FILLING = 3;
                    public const int AFFIRMED = 4;
                    public const int GRANULARBASE = 5;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { OWN_FILLING, "RELLENO PROPIO" },
                        { COMMON_FILLING, "RELLENO COMUN" },
                        { LOAN_FILLING, "RELLENO PRESTAMO" },
                        { AFFIRMED, "AFIRMADO" },
                        { GRANULARBASE, "BASE GRANULAR" }

                    };
                }
            }

            public static class ConcreteQuality
            {
                public static class Age
                {
                    public const int SEVEN = 7;
                    public const int TWENTY_EIGHT = 28;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { SEVEN, "7" },
                        { TWENTY_EIGHT, "28" },
                    };
                }

                public static class Segment
                {
                    public const int SLAB = 1;
                    public const int BODY = 2;
                    public const int ROOF = 3;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { SLAB, "Losa" },
                        { BODY, "Cuerpo" },
                        { ROOF, "Techo" },
                    };
                }
            }
        }

        public static class SewerManifoldFor24
        {
            public static class OriginType
            {
                public const int PRODUCT = 1;
                public const int SERVICE = 2;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                    {
                        { PRODUCT, "Producto" },
                        { SERVICE, "Servicio" }

                    };
            }
            public static class NCOrigin
            {
                public const int CLIENT = 1;
                public const int SUPPLIER = 2;
                public const int INTERNAL_PROCESS = 3;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                    {
                        { CLIENT, "Cliente" },
                        { SUPPLIER, "Proveedor" },
                        { INTERNAL_PROCESS, "Procesos Internos" }

                    };
            }

            public static class Decision
            {
                public const int REJECTION = 1;
                public const int SUPPLIER_ACCEPTANCE = 2;
                public const int REWORK = 3;
                public const int CORRECTION = 4;
                public const int CHANGE = 5;
                public const int REPAIR = 6;
                public const int OTHER = 7;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { REJECTION, "Rechazo" },
                    { SUPPLIER_ACCEPTANCE, "Aceptación al Proveedor" },
                    { REWORK, "Retrabajo" },
                    { CORRECTION, "Correción" },
                    { CHANGE, "Cambio" },
                    { REPAIR, "Reparación" },
                    { OTHER, "Otro" }
                };
            }

            public static class ActionTaken
            {
                public const int OBSERVED = 1;
                public const int PENDING = 2;
                public const int RAISED = 3;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { OBSERVED, "Observado" },
                    { PENDING, "Pendiente" },
                    { RAISED, "Levantada" }
                };
            }
        }

        public static class Employee
        {
            public static class WorkArea
            {
                public const int PRODUCTION = 1;
                public const int TECHINAL_OFFICE = 2;
                public const int QUALITY = 3;
                public const int INTEGRATED_MANAGEMENT_SYSTEM = 4;
                public const int ADMINISTRATION = 5;
                public const int EQUIPMENT = 6;
                public const int AGGREGATE_CONTROLLER = 7;
                public const int WAREHOUSE = 8;
                public const int SOCIAL_INTERVENTION = 9;
                public const int ENVIRONMENT = 10;
                public const int SECURITY_HEALTH = 11;
                public const int DRIVER = 12;
                public const int HUMAN_RESOURCES = 13;
                public const int ARCHAEOLOGY = 14;
                public const int LEGAL_AREA = 15;
                public const int LOGISTIC = 16;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PRODUCTION, "Producción" },
                    { TECHINAL_OFFICE, "Oficina Técnica" },
                    { QUALITY, "Calidad" },
                    { INTEGRATED_MANAGEMENT_SYSTEM, "SIG" },
                    { ADMINISTRATION, "Administración" },
                    { EQUIPMENT, "Equipos" },
                    { AGGREGATE_CONTROLLER, "Controlador de Agregados" },
                    { WAREHOUSE, "Almacén" },
                    { SOCIAL_INTERVENTION, "Intervención Social" },
                    { ENVIRONMENT, "Medio Ambiente" },
                    { SECURITY_HEALTH, "Seguridad y Salud en el Trabajo" },
                    { DRIVER, "Choferes" },
                    { HUMAN_RESOURCES, "RRHH" },
                    { ARCHAEOLOGY, "Arqueología" },
                    { LEGAL_AREA, "Área Legal" },
                    { LOGISTIC, "Logística" }
                };

                public static Dictionary<int, string> NORMALIZED_VALUES = new Dictionary<int, string>
                {
                    { PRODUCTION, "Produccion" },
                    { TECHINAL_OFFICE, "Oficina Tecnica" },
                    { QUALITY, "Calidad" },
                    { INTEGRATED_MANAGEMENT_SYSTEM, "SIG" },
                    { ADMINISTRATION, "Administracion" },
                    { EQUIPMENT, "Equipos" },
                    { AGGREGATE_CONTROLLER, "Controlador de Agregados" },
                    { WAREHOUSE, "Almacen" },
                    { SOCIAL_INTERVENTION, "Intervencion Social" },
                    { ENVIRONMENT, "Medio Ambiente" },
                    { SECURITY_HEALTH, "Seguridad y Salud en el Trabajo" },
                    { DRIVER, "Choferes" },
                    { HUMAN_RESOURCES, "RRHH" },
                    { ARCHAEOLOGY, "Arqueologia" },
                    { LEGAL_AREA, "Area Legal" },
                    { LOGISTIC, "Logística" }
                };

                public static Dictionary<int, string> CODE_VALUES = new Dictionary<int, string>
                {
                    { PRODUCTION, Areas.PRODUCTION },
                    { TECHINAL_OFFICE, Areas.TECHNICAL_OFFICE },
                    { QUALITY, Areas.QUALITY },
                    { INTEGRATED_MANAGEMENT_SYSTEM, Areas.INTEGRATED_MANAGEMENT_SYSTEM },
                    { ADMINISTRATION, Areas.ADMIN },
                    { EQUIPMENT, Areas.EQUIPMENT_MACHINERY },
                    { AGGREGATE_CONTROLLER, Areas.AGGREGATION },
                    { WAREHOUSE, Areas.WAREHOUSE },
                    { SOCIAL_INTERVENTION, Areas.SOCIAL_INTERVENTION },
                    { ENVIRONMENT, Areas.ENVIRONMENT },
                    { SECURITY_HEALTH, Areas.SECURITY },
                    { DRIVER, "Choferes" },
                    { HUMAN_RESOURCES, Areas.HUMAN_RESOURCES },
                    { ARCHAEOLOGY, Areas.ARCHEOLOGY },
                    { LEGAL_AREA, Areas.LEGAL_TECHNICAL_LIBRARY },
                    { LOGISTIC, Areas.LOGISTICS }
                };
            }
        }

        public static class Equipment
        {
            public static class Status
            {
                public const int OPERATIONAL = 1;
                public const int IN_REPAIR = 2;
                public const int NOT_OPERATIONAL = 3;
                public const int STOLEN = 4;
                public const int DISCARDED = 5;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { OPERATIONAL, "Operativo" },
                    { IN_REPAIR, "En Reparación" },
                    { NOT_OPERATIONAL, "No Operativo" },
                    { STOLEN, "Robado" },
                    { DISCARDED, "Descartado" },
                };

                public static Dictionary<int, string> NORMALIZED_VALUES = new Dictionary<int, string>
                {
                    { OPERATIONAL, "Operativo" },
                    { IN_REPAIR, "En Reparacion" },
                    { NOT_OPERATIONAL, "Inoperativo" },
                    { STOLEN, "Robado" },
                    { DISCARDED, "Descartado" }
                };
            }
        }

        public static class Letter
        {
            public const int DEFAULT_RESPONSE_TERM_DAYS = 3;

            public static class Type
            {
                public const int SENT = 1;
                public const int RECEIVED = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { SENT, "Enviadas"  },
                    { RECEIVED, "Recibidas"  }
                };
            }

            public static class DocumentType
            {
                public const int LETTER = 1;
                public const int REPORT = 2;
                public const int MEETING_RECORD = 3;
                public const int AGREEMENT_MINUTE = 4;
                public const int WORKBOOK = 5;
                public const int ANNOUNCEMENT = 6;
                public const int MEMORANDUM = 7;
                public const int DESCRIPTIVE_MEMORY = 8;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { LETTER, "Carta"  },
                    { REPORT, "Informe"  },
                    { MEETING_RECORD, "Acta de Reunión"  },
                    { AGREEMENT_MINUTE, "Acta de Acuerdos"  },
                    { WORKBOOK, "Cuaderno de Obra"  },
                    { ANNOUNCEMENT, "Comunicado"  },
                    { MEMORANDUM, "Memorandum"  },
                    { DESCRIPTIVE_MEMORY, "Memoria Descriptiva" }
                };
            }


            //Pasar a DB LetterDocumentCaracteristic
            public static class Status
            {
                public const int INFORMATIVE = 1;
                public const int QUERY = 2;
                public const int QUERY_US = 3;
                public const int ANSWER = 4;
                public const int APPROVAL = 5;
                public const int OBSERVATION = 6;
                public const int COMPLEMENT = 7;
                public const int AUTHORIZATION_REQUEST = 8;
                public const int VALORIZATION = 9;
                public const int APPROVAL_REQUEST = 10;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { INFORMATIVE, "Informativo"  },
                    { QUERY, "Consultamos"  },
                    { QUERY_US, "Nos Consultan"  },
                    { ANSWER, "Respondemos" },
                    { APPROVAL, "Aprueban" },
                    { OBSERVATION, "Observan" },
                    { COMPLEMENT, "Complemento" },
                    { AUTHORIZATION_REQUEST, "Solicitud de Autorización" },
                    { VALORIZATION, "Valorización" },
                    { APPROVAL_REQUEST, "Solicitamos Aprobación" }
                };
            }
        }

        public static class Logistics
        {
            public static class RequestOrder
            {
                public static class Type
                {
                    public const int PURCHASE = 1;
                    public const int SERVICE = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PURCHASE, "Compra" },
                        { SERVICE, "Servicio" }
                    };
                }

                public static class Status
                {
                    public const int PRE_ISSUED = 1;
                    public const int ISSUED = 2;
                    public const int APPROVED = 3;
                    public const int CANCELED = 4;
                    public const int ORDER_C = 5;
                    public const int ORDER_S = 6;
                    public const int OBSERVED = 7;
                    public const int APPROVED_PARTIALLY = 8;
                    public const int ANSWER_PENDING = 9;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PRE_ISSUED, "PRE-EMITIDO" },
                        { ISSUED, "EMITIDO" },
                        { APPROVED, "APROBADO" },
                        { CANCELED, "ANULADO" },
                        { ORDER_C, "O/C GENERADA" },
                        { ORDER_S, "O/S GENERADA" },
                        { OBSERVED, "OBSERVADO" },
                        { APPROVED_PARTIALLY, "APROBADO PARCIALMENTE" },
                        { ANSWER_PENDING, "PENDIENTE DE RESPUESTA" }
                    };
                }

                public static class AttentionStatus
                {
                    public const int PENDING = 1;
                    public const int PARTIAL = 2;
                    public const int TOTAL = 3;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PENDING, "POR ATENDER" },
                        { PARTIAL, "PARCIAL" },
                        { TOTAL, "TOTAL" }
                    };
                }

                public static class UserTypes
                {
                    public const int AuthRequest = 1;
                    public const int OkRequest = 2;
                    public const int FailRequest = 3;
                    public const int ReviewRequest = 4;
                    public const int AuthOrder = 5;
                    public const int OkOrder = 6;
                    public const int FailOrder = 7;
                    public const int ReviewOrder = 8;
                    public const int AuthPreRequest = 9;
                    public const int OkPreRequest = 10;
                    public const int FailPreRequest = 11;
                    public const int SecAuthPreRequest = 12;
                }
            }
            public static class PreRequest
            {
                public static class Type
                {
                    public const int PURCHASE = 1;
                    public const int SERVICE = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PURCHASE, "Compra" },
                        { SERVICE, "Servicio" }
                    };
                }

                public static class Status
                {
                    public const int PRE_ISSUED = 1;
                    public const int ISSUED = 2;
                    //public const int PROCESSED = 3;
                    //public const int PROCESSED_PARTIALLY = 4;
                    public const int CANCELED = 5;
                    public const int OBSERVED = 6;
                    public const int ANSWER_PENDING = 7;
                    public const int APPROVED = 8;
                    public const int APPROVED_PARTIALLY = 9;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PRE_ISSUED, "PRE-EMITIDO" },
                        { ISSUED, "EMITIDO" },
                        //{ PROCESSED, "PROCESADO" },
                        //{ PROCESSED_PARTIALLY, "PROCESADO PARCIALMENTE" },
                        { CANCELED, "RECHAZADO" },
                        { OBSERVED, "OBSERVADO" },
                        { ANSWER_PENDING, "PENDIENTE DE RESPUESTA" },
                        { APPROVED, "APROBADO" },
                        { APPROVED_PARTIALLY, "APROBADO PARCIALMENTE" }
                    };
                }
            }
        }

        public static class Supply
        {
            public static class Category
            {
                public const int MAIN_CATEGORY = 1;
                public const int SECONDARY_CATEGORY = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { MAIN_CATEGORY, "Principal" },
                    { SECONDARY_CATEGORY, "Secundaria" }
                };
            }
            public static class Status
            {
                public const int NO_ORDER = 0;
                public const int IN_ORDER = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { NO_ORDER, "Sin Orden" },
                    { IN_ORDER, "En Uso en Órdenes" }
                };
            }
        }

        public static class Terrain
        {
            public static class Type
            {
                public const int NORMAL = 1;
                public const int SEMIROCOUS = 2;
                public const int ROCKY = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { NORMAL, "N" },
                    { SEMIROCOUS, "SR" },
                    { ROCKY, "R" }
                };

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { NORMAL, "Normal" },
                    { SEMIROCOUS, "Semirocoso" },
                    { ROCKY, "Rocoso" }
                };
            }
        }

        public static class Welding
        {
            public static class Type
            {
                public const int TERMOFUSION = 1;
                public const int ELECTROFUSION_TUB = 2;
                public const int ELECTROFUSION_PAS = 3;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { TERMOFUSION, "Termofusión" },
                    { ELECTROFUSION_TUB, "Electrofusión Tub." },
                    { ELECTROFUSION_PAS, "Electrofusión Pas." }
                };
            }
        }

        public static class Asphalt
        {
            public static class Type
            {
                public const int HOTASPHALT = 1;
                public const int MIXED = 2;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { HOTASPHALT, "Asfalto en Caliente" },
                    { MIXED, "Mixto" }
                };
            }
        }

        public static class Thickness
        {
            public static class Type
            {
                public const int TWOS = 1;
                public const int THREES = 2;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { TWOS, "2\"" },
                    { THREES, "3\"" }
                };
            }
        }

        public static class Steel
        {
            public const int LENGTH = 9;

            public static class Affectation
            {
                public const int FIRSTFACTOR = 1;
                public const int METEREDFACTOR = 2;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>
                {
                    { FIRSTFACTOR, "Factor 1" },
                    { METEREDFACTOR, "Factor metrado" }
                };
            }
        }

        public static class Stage
        {
            public const int CONTRACTUAL = 1;
            public const int STAKING = 2;
            public const int REAL = 3;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { CONTRACTUAL, "Contractual" },
                        { STAKING, "Replanteo" },
                        { REAL, "Real" }
                    };
        }

        public static class Sewer
        {
            public static class Box
            {
                public static class Type
                {
                    public const int I = 1;
                    public const int II = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { I, "Tipo I" },
                        { II, "Tipo II" }
                    };
                }

                public static class Footage
                {
                    public static class Ranges
                    {
                        public const int R0 = 0;
                        public const int R1 = 1;
                        public const int R2 = 2;
                        public const int R3 = 3;
                        public const int R4 = 4;
                        public const int R5 = 5;
                        public const int R6 = 6;
                        public const int R7 = 7;
                        public const int R8 = 8;
                        public const int R9 = 9;
                        public const int R10 = 10;
                        public const int R11 = 11;
                        public const int R12 = 12;
                        public const int R13 = 13;
                        public const int R14 = 14;
                        public const int R15 = 15;
                        public const int R16 = 16;

                        public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                        {
                            {R0, "1.01-1.25" },
                            {R1, "1.26-1.50" },
                            {R2, "1.51-1.75" },
                            {R3, "1.76-2.00" },
                            {R4, "2.01-2.50" },
                            {R5, "2.51-3.00" },
                            {R6, "3.01-3.50" },
                            {R7, "3.51-4.00" },
                            {R8, "4.01-5.00" },
                            {R9, "5.01-6.00" },
                            {R10, "6.01-7.00" },
                            {R11, "7.01-8.00" },
                            {R12, "8.01-9.00" },
                            {R13, "9.01-10.00" },
                            {R14, "10.01-11.00" },
                            {R15, "11.01-12.00" },
                            {R16, "12.01-13.00" }
                        };
                    }

                    public static class Groups
                    {
                        public const int TOTAL = 0;
                        public const int ROOF = 1;
                        public const int WALL = 2;
                        public const int FLOOR = 3;
                        public const int MCDICE = 4;

                        public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                        {
                            {TOTAL, "Total" },
                            {ROOF, "Techo" },
                            {WALL, "Muro" },
                            {FLOOR, "Losa" },
                            {MCDICE, "M.C.+DADO" }
                        };
                    }

                    public static class Types
                    {
                        public const int VOL_CONCRETE = 0;
                        public const int TYPE_CONCRETE = 1;
                        public const int STEEL = 2;
                        public const int RODS38 = 3;
                        public const int RODS12 = 4;
                        public const int RODS58 = 5;
                        public const int WIRE8 = 6;
                        public const int WIRE16 = 7;
                        public const int SAND_FINE = 8;
                        public const int SAND_GROSS = 9;
                        public const int ROCK = 10;
                        public const int WATER = 11;
                        public const int ADDITIVE = 12;
                        public const int NAILS = 13;
                        public const int AMMOUNT = 14;

                        public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                        {
                            {VOL_CONCRETE, "Vol. Concreto (m3)" },
                            {TYPE_CONCRETE, "Cemento Tipo V (bls)" },
                            {STEEL, "Acero (kg)" },
                            {RODS38, "Varillas 3/8 (und)" },
                            {RODS12, "Varillas 1/2 (und)" },
                            {RODS58, "Varillas 5/8 (und)" },
                            {WIRE8, "Alambre # 8 (kg)" },
                            {WIRE16, "Alambre # 16 (kg)" },
                            {SAND_FINE, "Arena fina (m3)" },
                            {SAND_GROSS, "Arena Gruesa (m3)" },
                            {ROCK, "Piedra (m3)" },
                            {WATER, "Agua (m3)" },
                            {ADDITIVE, "Aditivo (lts)" },
                            {NAILS, "Clavos (kg)" },
                            {AMMOUNT, "Monto (S/)" }
                        };
                    }
                }
            }

            public static class Group
            {
                public static class Destination
                {
                    public const int LOCAL = 1;
                    public const int COLLABORATOR = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { LOCAL, "Casa" },
                        { COLLABORATOR, "Colaborador" },
                    };
                }

                public static class Type
                {
                    public const int DRAINAGE = 1;
                    public const int MANIFOLD = 2;
                    public const int DRINKING_WATER = 3;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { DRAINAGE, "Redes Secundarias de Alcantarillado" },
                        { MANIFOLD, "Colector" },
                        { DRINKING_WATER, "Agua Potable" }
                    };
                }


                public static class WorkComponent
                {
                    public const int GENERAL = 1;
                    public const int SECONDARY = 2;
                    public const int OVERALL = 3;
                    public const int OTHER = 4;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { GENERAL, "Obras Generales" },
                        { SECONDARY, "Obras Secundarias" },
                        { OVERALL, "Total" },
                        { OTHER, "Otros Componentes" }
                    };
                }

                public static class WorkStructure
                {
                    public const int MASSIVE_EARTHWORKS = 1;
                    public const int CIVIL_WORKS = 2;
                    public const int CONDUCTION_LINE = 3;
                    public const int DRIVE_LINES = 4;
                    public const int STRATEGIC_TRUNKS = 5;
                    public const int AIR_PURGE_LINE_CHAMBERS = 6;
                    public const int REDUCING_CHAMBERS = 7;
                    public const int COLLECTORS = 8;
                    public const int OVERFLOWS = 9;
                    public const int NETWORKS_CONNECTIONS = 10;
                    public const int DISCHARGE_COLLECTOR = 11;
                    public const int TOPOGRAPHY = 12;
                    public const int PITS = 13;
                    public const int ELECTRICAL_EQUIPMENT = 14;
                    public const int HYDRAULIC_EQUIPMENT = 15;
                    public const int MACHINERY_EQUIPMENT_MANAGEMENT = 16;
                    public const int WAREHOUSE_MANAGEMENT = 17;
                    public const int TRAFFIC_DIVERSION_MANAGEMENT = 18;
                    public const int ENVIRONMENT_MANAGEMENT = 19;
                    public const int UNION_MANAGEMENT = 20;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { MASSIVE_EARTHWORKS, "Movimiento de Tierras Masivas" },
                        { CIVIL_WORKS, "Obras Civiles" },
                        { CONDUCTION_LINE, "Línea de Conducción" },
                        { DRIVE_LINES, "Líneas de Impulsión" },
                        { STRATEGIC_TRUNKS, "Troncales Estratégicas" },
                        { AIR_PURGE_LINE_CHAMBERS, "Cámaras de Aire, Purga y de Línea" },
                        { REDUCING_CHAMBERS, "Cámaras Reductoras" },
                        { COLLECTORS, "Colectores" },
                        { OVERFLOWS, "Reboses" },
                        { NETWORKS_CONNECTIONS, "Redes y Conexiones" },
                        { DISCHARGE_COLLECTOR, "Colector de Descarga" },
                        { TOPOGRAPHY, "Topografía" },
                        { PITS, "Calicatas" },
                        { ELECTRICAL_EQUIPMENT, "Equipamiento Eléctrico" },
                        { HYDRAULIC_EQUIPMENT, "Equipamiento Hidráulico" },
                        { MACHINERY_EQUIPMENT_MANAGEMENT, "Gestión de Maquinarias y Equipos" },
                        { WAREHOUSE_MANAGEMENT, "Gestión de Almacén" },
                        { TRAFFIC_DIVERSION_MANAGEMENT, "Gestión de Tránsito y Desvíos" },
                        { ENVIRONMENT_MANAGEMENT, "Gestión del Medio Ambiente" },
                        { UNION_MANAGEMENT, "Manejo Sindical" },
                    };
                }
            }

            public static class Manifolds
            {
                public static class Process
                {
                    public const int PROJECT = 0;
                    public const int REVIEW = 1;
                    public const int EXECUTION = 2;

                    public static Dictionary<int, string> TYPES = new Dictionary<int, string>
                    {
                        {PROJECT, "Proyecto" },
                        {REVIEW, "Replanteo" },
                        {EXECUTION, "Ejecución" }
                    };
                }

                public static class Letters
                {
                    public const int REQUEST = 0;
                    public const int APPROVAL = 1;

                    public static Dictionary<int, string> TYPES = new Dictionary<int, string>
                    {
                        {REQUEST, "Solicitud" },
                        {APPROVAL, "Aprobación" }
                    };
                }
            }
        }

        public static class Pipeline
        {
            public static class Type
            {
                public const int PVC = 1;
                public const int HDPE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PVC, "PVC" },
                    { HDPE, "HDPE" },
                };
            }

            public static class Class
            {
                public const int SN2 = 1;
                public const int SN4 = 2;
                public const int SN8 = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { SN2, "SN-2" },
                    { SN4, "SN-4" },
                    { SN8, "SN-8" },
                };
            }
        }

        public static class Business
        {
            public static class FileType
            {
                public const int TAX = 1;
                public const int LEGAL = 2;
                public const int COMMERCIAL = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { TAX, "Tributarios" },
                    { LEGAL, "Legales" },
                    { COMMERCIAL, "Comerciales" }
                };

                public static Dictionary<int, string> BLOBS = new Dictionary<int, string>
                {
                    { TAX, Storage.Blobs.PROVIDER_TAX_FILES },
                    { LEGAL, Storage.Blobs.PROVIDER_LEGAL_FILES },
                    { COMMERCIAL, Storage.Blobs.PROVIDER_COMMERCIAL_FILES }
                };
            }

            public static class PropertyServiceType
            {
                public const int PROPERTY = 1;
                public const int SERVICE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PROPERTY, "Bien" },
                    { SERVICE, "Servicio" }
                };
            }
        }
        public static class Provider
        {
            public static class FileType
            {
                public const int TAX = 1;
                public const int LEGAL = 2;
                public const int COMMERCIAL = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { TAX, "Tributarios" },
                    { LEGAL, "Legales" },
                    { COMMERCIAL, "Comerciales" }
                };

                public static Dictionary<int, string> BLOBS = new Dictionary<int, string>
                {
                    { TAX, Storage.Blobs.PROVIDER_TAX_FILES },
                    { LEGAL, Storage.Blobs.PROVIDER_LEGAL_FILES },
                    { COMMERCIAL, Storage.Blobs.PROVIDER_COMMERCIAL_FILES }
                };
            }

            public static class PropertyServiceType
            {
                public const int PROPERTY = 1;
                public const int SERVICE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PROPERTY, "Bien" },
                    { SERVICE, "Servicio" }
                };
            }
        }

        public static class Workbook
        {
            public static class Status
            {
                public const int PENDING = 1;
                public const int RESOLVED = 2;
                public const int DELIVERED = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PENDING, "Pendiente" },
                    { RESOLVED, "Absuelto" },
                    { DELIVERED, "Información Entregada" }
                };
            }

            public static class Type
            {
                public const int QUERY = 1;
                public const int DELAY = 2;
                public const int ADDITIONAL_METER = 3;
                public const int ADDITIONAL_NEW_ITEM = 4;
                public const int PERIOD_EXTENSION = 5;
                public const int REPORT = 6;
                public const int STAKEOUT = 7;
                public const int PROGRESS = 8;
                public const int GOAL_REDUCTION = 9;
                public const int QUALITY = 10;
                //20-02-2020 Ricardo: Solicitaron Nuevos Status para el Cuaderno de Trabajo, confirmar con el Ing. Francisco
                public const int KNOWLEDGE_TAKING = 11; //Toma de conocimiento
                public const int COMMUNICATE_PROGRESS = 12; //Comunica Avance
                public const int CHECK_PROGRESS = 13; //Verifica Avance
                public const int AUTHORIZATION_REQUEST = 14; //Solicita Autorizacón
                public const int AUTHORIZATION_GRANTED = 15; //Autorización Otorgada
                public const int ADDITIONAL_METER_GRANTED = 16; //Autoriza Mayor Metrado
                public const int QUERY_RESOLUTION = 17;
                public const int INFORMATION_REQUEST = 18;
                public const int INFORMATION_DELIVERY = 19;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { QUERY, "Consulta" },
                        { DELAY, "Restricción o demora" },
                        { ADDITIONAL_METER, "Solicita Mayor Metrado" },
                        { ADDITIONAL_NEW_ITEM, "Adicional nueva partida" },
                        { PERIOD_EXTENSION, "Ampliación de plazo" },
                        { REPORT, "Informativo" },
                        { STAKEOUT, "Replanteos" },
                        { PROGRESS, "Valorización y avances" },
                        { GOAL_REDUCTION, "Deductivos o reducción de metas" },
                        { QUALITY, "Calidad" },
                        { KNOWLEDGE_TAKING, "Toma de Conocimiento" },
                        { COMMUNICATE_PROGRESS, "Comunicación de Avances" },
                        { CHECK_PROGRESS, "Avance Verificado" },
                        { AUTHORIZATION_REQUEST, "Solicitud de Autorización" },
                        { AUTHORIZATION_GRANTED, "Autorización Otorgada" },
                        { ADDITIONAL_METER_GRANTED, "Mayor Metrado Autorizado" },
                        { QUERY_RESOLUTION, "Absolución de Consulta" },
                        { INFORMATION_REQUEST, "Solicitud de Información" },
                        { INFORMATION_DELIVERY, "Entrega de Información" },
                    };
            }

            public static class WroteBy
            {
                public const int CONTRACTOR = 1;
                public const int SUPERVISION = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { CONTRACTOR, "Contratista" },
                        { SUPERVISION, "Supervisión" }
                    };
            }
        }

        public static class Worker
        {
            public static class Category
            {
                public const int PAWN = 1;
                public const int OPERATOR = 2;
                public const int OFFICIAL = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PAWN, "Peón" },
                    { OPERATOR, "Operario" },
                    { OFFICIAL, "Oficial" }
                };

                public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>
                {
                    { PAWN, "PE" },
                    { OPERATOR, "OP"},
                    { OFFICIAL, "OF" }
                };

                public static Dictionary<int, string> BUC_VALUES = new Dictionary<int, string>
                {
                    { PAWN, "30%" },
                    { OPERATOR, "32%"},
                    { OFFICIAL, "30%" }
                };
            }

            public static class Origin
            {
                public const int POPULATION = 1;
                public const int SYNDICATE = 2;
                public const int IVC = 3;
                public const int HELPER = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { POPULATION, "Población" },
                    { SYNDICATE, "Sindicato" },
                    { IVC, "IVC" },
                    { HELPER, "Colaborador" },
                };

                public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>
                {
                    { POPULATION, "P" },
                    { SYNDICATE, "S" },
                    { IVC, "IVC" },
                    { HELPER, "C" },
                };
            }

            public static class Workgroup
            {
                public const int HOME = 1;
                public const int COLLABORATORS = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { HOME, "Casa" },
                    { COLLABORATORS, "Colaboradores" }
                };
            }

            public static class Gender
            {
                public const int MALE = 0;
                public const int FEMALE = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { MALE, "Masculino" },
                    { FEMALE, "Femenino" }
                };

                public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>
                {
                    { MALE, "M" },
                    { FEMALE, "F" }
                };
            }
        }

        public static class WorkPositions
        {
            public static class Type
            {
                public const int BOTH = 0;
                public const int EMPLOYEE = 1;
                public const int WORKER = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {BOTH, "Ambos" },
                    {EMPLOYEE, "Empleado" },
                    {WORKER, "Obrero" }
                };
            }

        }

        public static class WorkFront
        {
        }

        public static class Messages
        {
            public static class Error
            {
                public const string MESSAGE = "Ocurrió un problema al procesar su consulta";
                public const string TITLE = "Error";
            }

            public static class Info
            {
                public const string MESSAGE = "Mensaje informativo";
                public const string TITLE = "Info";
            }

            public static class Success
            {
                public const string MESSAGE = "Tarea realizada satisfactoriamente";
                public const string TITLE = "Éxito";
            }

            public static class Validation
            {
                public const string COMPARE = "El campo '{0}' no coincide con '{1}'";
                public const string EMAIL_ADDRESS = "El campo '{0}' no es un correo electrónico válido";
                public const string RANGE = "El campo '{0}' debe tener un valor entre {1}-{2}";
                public const string REGULAR_EXPRESSION = "El campo '{0}' no es válido";
                public const string REQUIRED = "El campo '{0}' es obligatorio";
                public const string STRING_LENGTH = "El campo '{0}' debe tener {1}-{2} caracteres";
                public const string NOT_VALID = "El campo '{0}' no es válido'";
                public const string FILE_EXTENSIONS = "El campo '{0}' solo acepta archivos con los formatos: {1}";
            }
        }

        public static class Permission
        {
            public static class Administrator
            {
                public const string PARTIAL_ACCESS = Roles.SUPERADMIN;

                public const string FULL_ACCESS = Roles.SUPERADMIN;
            }

            public static class LegalTechnicalLibrary
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.BASIC_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.HUMAN_RESOURCES + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT;
            }

            public static class Finance
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.ADMINISTRATION;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE;
            }

            public static class DocumentaryControl
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.BASIC_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.HUMAN_RESOURCES + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT;
            }

            public static class IntegratedManagementSystem
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.BASIC_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.HUMAN_RESOURCES + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM;
            }

            public static class TechnicalOffice
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.BASIC_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.BASIC_SUPPORT;
            }

            public static class Quality
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.BASIC_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.QUALITY;
            }

            public static class Logistics
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.BASIC_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.HUMAN_RESOURCES + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string RESTRICTED_ACCESS = Roles.CENTRAL_OFFICE;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.ADMINISTRATION;
            }

            public static class EquipmentMachinery
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.HUMAN_RESOURCES + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.ADMINISTRATION;
            }

            public static class HumanResources
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.HUMAN_RESOURCES + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.HUMAN_RESOURCES;
            }

            public static class Production
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.HUMAN_RESOURCES + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.PRODUCTION;
            }

            public static class Warehouse
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.WAREHOUSE;
            }

            public static class Accounting
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.WAREHOUSE;
            }

            public static class Aggregation
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                    + Roles.PRODUCTION + ","
                    + Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.QUALITY + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.ADMINISTRATION + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT + ","
                    + Roles.ADMINISTRATION;
            }

            public static class Security
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.HUMAN_RESOURCES + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;
            }

            public static class Environment
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;
            }

            public static class SocialIntervention
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;
            }

            public static class Archeology
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;
            }

            public static class Bidding
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.BIDDING + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string RESTRICTED_ACCESS = Roles.BIDDING;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                    + Roles.CENTRAL_OFFICE + ","
                    + Roles.BIDDING + ","
                    + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;
            }

            public static class Reports
            {
                public const string PARTIAL_ACCESS = Roles.CENTRAL_OFFICE + ","
                   + Roles.PRODUCTION + ","
                   + Roles.SUPERADMIN + ","
                   + Roles.TECHNICAL_OFFICE + ","
                   + Roles.TECHNICAL_SUPPORT + ","
                   + Roles.BASIC_SUPPORT + ","
                   + Roles.QUALITY + ","
                   + Roles.INTEGRATED_MANAGEMENT_SYSTEM + ","
                   + Roles.HUMAN_RESOURCES + ","
                   + Roles.ADMINISTRATION + ","
                   + Roles.SOCIAL_SECURITY_ENVIRONMENT_ARCHEOLOGY;

                public const string FULL_ACCESS = Roles.SUPERADMIN + ","
                    + Roles.TECHNICAL_OFFICE + ","
                    + Roles.TECHNICAL_SUPPORT;
            }
        }

        public static class TechnicalLibrary
        {
            public static class FileType
            {
                public const int SEDAPAL_TECHNICAL_SPECIFICATIONS = 1;
                public const int SUPPLIER_CATALOG = 2;
                public const int GENERAL_CONSTRUCTION_PROCEDURES = 3;
                public const int PERUVIAN_TECHNICAL_STANDAR = 4;
                public const int GENERAL_POLICY = 5;
                public const int MANUAL_TECHNICAL_BOOK = 6;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {SEDAPAL_TECHNICAL_SPECIFICATIONS, "Especificaciones Técnicas de SEDAPAL" },
                    {SUPPLIER_CATALOG, "Catálago de Proveedores" },
                    {GENERAL_CONSTRUCTION_PROCEDURES, "Procedimientos Constructivos Generales" },
                    {PERUVIAN_TECHNICAL_STANDAR, "Normas Técnicas Peruanas" },
                    {GENERAL_POLICY, "Políticas Generales" },
                    {MANUAL_TECHNICAL_BOOK, "Manuales y Libros Técnicos Diversos" },
                };
            }
        }

        public static class Payroll
        {
            public static class ProcessStatus
            {
                public const int NON = 0;
                public const int IN_PROCESS = 1;
                public const int PROCESSED = 2;

                public static Dictionary<int, string> Status = new Dictionary<int, string>
                {
                    {NON, "Sin Procesar"},
                    {IN_PROCESS, "Procesando" },
                    {PROCESSED, "Calculado" }
                };
            }
        }

        public static class PayrollVariable
        {
            public static class Type
            {
                public const int NON = 0;
                public const int FACT = 1;
                public const int FORMULA = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {NON, "Ninguno" },
                    {FACT, "Dato" },
                    {FORMULA, "Fórmula" }
                };
            }
        }

        public static class PayrollConcept
        {
            public static class Category
            {
                public const int INCOMES = 1;
                public const int DISCOUNTS = 2;
                public const int CONTRIBUTIONS = 3;
                public const int TOTALS = 4;
                public const int DAILY = 5;
                public const int CONTROL = 6;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {INCOMES, "Ingresos" },
                    {DISCOUNTS, "Descuentos" },
                    {CONTRIBUTIONS, "Aportes" },
                    {TOTALS, "Totales" },
                    {DAILY, "Tareo" },
                    {CONTROL, "Control" }
                };

                public static Dictionary<int, string> PREFIX = new Dictionary<int, string>
                {
                    {INCOMES, "R" },
                    {DISCOUNTS, "D" },
                    {CONTRIBUTIONS, "A" },
                    {TOTALS, "T" },
                    {DAILY, "E" },
                    {CONTROL, "C" }
                };
            }
        }

        public static class PayrollConceptFormula
        {
            public static class LaborRegime
            {
                public const int COMMON = 1;
                public const int CIVILCONSTRUCTION = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {COMMON, "Régimen Común" },
                    {CIVILCONSTRUCTION, "Construcción Civil" }
                };
            }
        }

        public static class SystemUrl
        {
            public const string Url = "https://erp-ivc.azurewebsites.net/";
            public const string PdfGeneratorUrl = "https://erp-ivc-pdf.azurewebsites.net/api/functionapp";
        }

        public static class CovidTest
        {
            public static class Type
            {
                public const int FAST = 0;
                public const int MOLECULAR = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {FAST, "Rápida" },
                    {MOLECULAR, "Molecular" }
                };
            }

            public static class Outcome
            {
                public const int NEGATIVE = 0;
                public const int POSITIVE = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {NEGATIVE, "Negativo" },
                    {POSITIVE, "Positivo" }
                };
            };
        }

        public static class WorkerDailyTask
        {
            public static class HourConcept
            {
                public const string HOURS_PAID_LEAVE = "PG";
                public const string MEDICAL_LEAVE = "SM";
                public const string NOT_ATTENDANCE = "F";
                public const string LABOR_SUSPENSION = "S";
                public const string HOURS_MEDICAL_REST = "DM";
                public const string UNPAID_LEAVE = "PS";
                public const string HOURS_HOLIDAY = "FE";

                public static Dictionary<string, string> VALUES = new Dictionary<string, string>
                {
                    { HOURS_PAID_LEAVE, "Permiso de goce de haber" },
                    { MEDICAL_LEAVE, "Subsidio médico" },
                    { NOT_ATTENDANCE, "Inasistencia" },
                    { LABOR_SUSPENSION, "Suspensión de labores" },
                    { HOURS_MEDICAL_REST, "Descanso médico" },
                    { UNPAID_LEAVE, "Permiso sin goce de haber" },
                    { HOURS_HOLIDAY, "Feriado" }
                };
            }
        }

        public static class WorkerMedicalRest
        {
            public static class FileType
            {
                public const int MEDICAL_REST = 0;
                public const int MEDICAL_APPOINTMENT = 1;
                public const int MEDICAL_LEAVE = 2;
                public const int MEDICAL_LEAVE_COVID = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    {MEDICAL_REST, "Descanso Médico" },
                    {MEDICAL_APPOINTMENT, "Cita Médica" },
                    {MEDICAL_LEAVE, "Subsidio Médico" },
                    {MEDICAL_LEAVE_COVID, "Subsidio Covid" }
                };
            }
        }

        public static class Months
        {
            public const int JANUARY = 1;
            public const int FEBRUARY = 2;
            public const int MARCH = 3;
            public const int APRIL = 4;
            public const int MAY = 5;
            public const int JUNE = 6;
            public const int JULY = 7;
            public const int AUGUST = 8;
            public const int SEPTEMBER = 9;
            public const int OCTOBER = 10;
            public const int NOVEMBER = 11;
            public const int DECEMBER = 12;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                {JANUARY, "Enero" },
                {FEBRUARY, "Febrero" },
                {MARCH, "Marzo" },
                {APRIL, "Abril" },
                {MAY, "Mayo" },
                {JUNE, "Junio" },
                {JULY, "Julio" },
                {AUGUST, "Agosto" },
                {SEPTEMBER, "Setiembre" },
                {OCTOBER, "Octubre" },
                {NOVEMBER, "Noviembre" },
                {DECEMBER, "Diciembre" }
            };

            public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>
            {
                {JANUARY, "Ene" },
                {FEBRUARY, "Feb" },
                {MARCH, "Mar" },
                {APRIL, "Abr" },
                {MAY, "May" },
                {JUNE, "Jun" },
                {JULY, "Jul" },
                {AUGUST, "Ago" },
                {SEPTEMBER, "Set" },
                {OCTOBER, "Oct" },
                {NOVEMBER, "Nov" },
                {DECEMBER, "Dic" }
            };
        }

        public static class EquipmentMachinery
        {
            public static class EquipmentMachineryOperator
            {
                public const int WEEK_VALUE_ALL = 9999;
                public const int WEEK_VALUE_1 = 1;
                public const int WEEK_VALUE_2 = 2;
                public const int WEEK_VALUE_3 = 3;
                public const int WEEK_VALUE_4 = 4;
                public const int WEEK_VALUE_5 = 5;
                public const int WEEK_VALUE_6 = 6;
                public const int WEEK_VALUE_7 = 7;
                public const int WEEK_VALUE_8 = 8;
                public const int WEEK_VALUE_9 = 9;
                public const int WEEK_VALUE_10 = 10;
                public const int WEEK_VALUE_11 = 11;
                public const int WEEK_VALUE_12 = 12;
                public const int WEEK_VALUE_13 = 13;
                public const int WEEK_VALUE_14 = 14;
                public const int WEEK_VALUE_15 = 15;
                public const int WEEK_VALUE_16 = 16;
                public const int WEEK_VALUE_17 = 17;
                public const int WEEK_VALUE_18 = 18;
                public const int WEEK_VALUE_19 = 19;
                public const int WEEK_VALUE_20 = 20;
                public const int WEEK_VALUE_21 = 21;
                public const int WEEK_VALUE_22 = 22;
                public const int WEEK_VALUE_23 = 23;
                public const int WEEK_VALUE_24 = 24;
                public const int WEEK_VALUE_25 = 25;
                public const int WEEK_VALUE_26 = 26;
                public const int WEEK_VALUE_27 = 27;
                public const int WEEK_VALUE_28 = 28;
                public const int WEEK_VALUE_29 = 29;
                public const int WEEK_VALUE_30 = 30;
                public const int WEEK_VALUE_31 = 31;
                public const int WEEK_VALUE_32 = 32;
                public const int WEEK_VALUE_33 = 33;
                public const int WEEK_VALUE_34 = 34;
                public const int WEEK_VALUE_35 = 35;
                public const int WEEK_VALUE_36 = 36;
                public const int WEEK_VALUE_37 = 37;
                public const int WEEK_VALUE_38 = 38;
                public const int WEEK_VALUE_39 = 39;
                public const int WEEK_VALUE_40 = 40;
                public const int WEEK_VALUE_41 = 41;
                public const int WEEK_VALUE_42 = 42;
                public const int WEEK_VALUE_43 = 43;
                public const int WEEK_VALUE_44 = 44;
                public const int WEEK_VALUE_45 = 45;
                public const int WEEK_VALUE_46 = 46;
                public const int WEEK_VALUE_47 = 47;
                public const int WEEK_VALUE_48 = 48;
                public const int WEEK_VALUE_49 = 49;
                public const int WEEK_VALUE_50 = 50;
                public const int WEEK_VALUE_51 = 51;
                public const int WEEK_VALUE_52 = 52;
                public const int WEEK_VALUE_53 = 53;
                public static Dictionary<int, string> WEEK_VALUE = new Dictionary<int, string>
                    {
                { WEEK_VALUE_ALL,"Seleccione una Semana" },
                { WEEK_VALUE_1 ,"  1"},
                { WEEK_VALUE_2 ," 2"},
                { WEEK_VALUE_3 ," 3"},
                { WEEK_VALUE_4 ," 4"},
                { WEEK_VALUE_5 ," 5"},
                { WEEK_VALUE_6 ," 6"},
                { WEEK_VALUE_7 ," 7"},
                { WEEK_VALUE_8 ," 8"},
                { WEEK_VALUE_9 ," 9"},
                { WEEK_VALUE_10 ," 10"},
                { WEEK_VALUE_11 ," 11"},
                { WEEK_VALUE_12 ," 12"},
                { WEEK_VALUE_13 ," 13"},
                { WEEK_VALUE_14 ," 14"},
                { WEEK_VALUE_15 ," 15"},
                { WEEK_VALUE_16 ," 16"},
                { WEEK_VALUE_17 ," 17"},
                { WEEK_VALUE_18 ," 18"},
                { WEEK_VALUE_19 ," 19"},
                { WEEK_VALUE_20 ," 20"},
                { WEEK_VALUE_21 ," 21"},
                { WEEK_VALUE_22 ," 22"},
                { WEEK_VALUE_23 ," 23"},
                { WEEK_VALUE_24 ," 24"},
                { WEEK_VALUE_25 ," 25"},
                { WEEK_VALUE_26 ," 26"},
                { WEEK_VALUE_27 ," 27"},
                { WEEK_VALUE_28 ," 28"},
                { WEEK_VALUE_29 ," 29"},
                { WEEK_VALUE_30 ," 30"},
                { WEEK_VALUE_31 ," 31"},
                { WEEK_VALUE_32 ," 32"},
                { WEEK_VALUE_33 ," 33"},
                { WEEK_VALUE_34 ," 34"},
                { WEEK_VALUE_35 ," 35"},
                { WEEK_VALUE_36 ," 36"},
                { WEEK_VALUE_37 ," 37"},
                { WEEK_VALUE_38 ," 38"},
                { WEEK_VALUE_39 ," 39"},
                { WEEK_VALUE_40 ," 40"},
                { WEEK_VALUE_41 ," 41"},
                { WEEK_VALUE_42 ," 42"},
                { WEEK_VALUE_43 ," 43"},
                { WEEK_VALUE_44 ," 44"},
                { WEEK_VALUE_45 ," 45"},
                { WEEK_VALUE_46 ," 46"},
                { WEEK_VALUE_47 ," 47"},
                { WEEK_VALUE_48 ," 48"},
                { WEEK_VALUE_49 ," 49"},
                { WEEK_VALUE_50 ," 50"},
                { WEEK_VALUE_51 ," 51"},
                { WEEK_VALUE_52 ," 52"},
                { WEEK_VALUE_53 ," 53"},
                    };


                public const int WEEK_ALL = 9999;
                public const int WEEK_1 = 1;
                public const int WEEK_2 = 2;
                public const int WEEK_3 = 3;
                public const int WEEK_4 = 4;
                public const int WEEK_5 = 5;
                public const int WEEK_6 = 6;
                public const int WEEK_7 = 7;
                public const int WEEK_8 = 8;
                public const int WEEK_9 = 9;
                public const int WEEK_10 = 10;
                public const int WEEK_11 = 11;
                public const int WEEK_12 = 12;
                public const int WEEK_13 = 13;
                public const int WEEK_14 = 14;
                public const int WEEK_15 = 15;
                public const int WEEK_16 = 16;
                public const int WEEK_17 = 17;
                public const int WEEK_18 = 18;
                public const int WEEK_19 = 19;
                public const int WEEK_20 = 20;
                public const int WEEK_21 = 21;
                public const int WEEK_22 = 22;
                public const int WEEK_23 = 23;
                public const int WEEK_24 = 24;
                public const int WEEK_25 = 25;
                public const int WEEK_26 = 26;
                public const int WEEK_27 = 27;
                public const int WEEK_28 = 28;
                public const int WEEK_29 = 29;
                public const int WEEK_30 = 30;
                public const int WEEK_31 = 31;
                public const int WEEK_32 = 32;
                public const int WEEK_33 = 33;
                public const int WEEK_34 = 34;
                public const int WEEK_35 = 35;
                public const int WEEK_36 = 36;
                public const int WEEK_37 = 37;
                public const int WEEK_38 = 38;
                public const int WEEK_39 = 39;
                public const int WEEK_40 = 40;
                public const int WEEK_41 = 41;
                public const int WEEK_42 = 42;
                public const int WEEK_43 = 43;
                public const int WEEK_44 = 44;
                public const int WEEK_45 = 45;
                public const int WEEK_46 = 46;
                public const int WEEK_47 = 47;
                public const int WEEK_48 = 48;
                public const int WEEK_49 = 49;
                public const int WEEK_50 = 50;
                public const int WEEK_51 = 51;
                public const int WEEK_52 = 52;
                public const int WEEK_53 = 53;
                public static Dictionary<int, string> WEEK = new Dictionary<int, string>
                    {
                { WEEK_ALL ,"Todos"},
                { WEEK_1 ,"  1"},
                { WEEK_2 ," 2"},
                { WEEK_3 ," 3"},
                { WEEK_4 ," 4"},
                { WEEK_5 ," 5"},
                { WEEK_6 ," 6"},
                { WEEK_7 ," 7"},
                { WEEK_8 ," 8"},
                { WEEK_9 ," 9"},
                { WEEK_10 ," 10"},
                { WEEK_11 ," 11"},
                { WEEK_12 ," 12"},
                { WEEK_13 ," 13"},
                { WEEK_14 ," 14"},
                { WEEK_15 ," 15"},
                { WEEK_16 ," 16"},
                { WEEK_17 ," 17"},
                { WEEK_18 ," 18"},
                { WEEK_19 ," 19"},
                { WEEK_20 ," 20"},
                { WEEK_21 ," 21"},
                { WEEK_22 ," 22"},
                { WEEK_23 ," 23"},
                { WEEK_24 ," 24"},
                { WEEK_25 ," 25"},
                { WEEK_26 ," 26"},
                { WEEK_27 ," 27"},
                { WEEK_28 ," 28"},
                { WEEK_29 ," 29"},
                { WEEK_30 ," 30"},
                { WEEK_31 ," 31"},
                { WEEK_32 ," 32"},
                { WEEK_33 ," 33"},
                { WEEK_34 ," 34"},
                { WEEK_35 ," 35"},
                { WEEK_36 ," 36"},
                { WEEK_37 ," 37"},
                { WEEK_38 ," 38"},
                { WEEK_39 ," 39"},
                { WEEK_40 ," 40"},
                { WEEK_41 ," 41"},
                { WEEK_42 ," 42"},
                { WEEK_43 ," 43"},
                { WEEK_44 ," 44"},
                { WEEK_45 ," 45"},
                { WEEK_46 ," 46"},
                { WEEK_47 ," 47"},
                { WEEK_48 ," 48"},
                { WEEK_49 ," 49"},
                { WEEK_50 ," 50"},
                { WEEK_51 ," 51"},
                { WEEK_52 ," 52"},
                { WEEK_53 ," 53"},
                    };

                public const int YEAR_VALUE_ALL = 9999;
                public const int YEAR_VALUE_2021 = 2021;
                public const int YEAR_VALUE_2020 = 2020;
                public const int YEAR_VALUE_2019 = 2019;
                public const int YEAR_VALUE_2018 = 2018;

                public static Dictionary<int, string> YEAR_VALUE = new Dictionary<int, string>
                    {
                        {YEAR_VALUE_ALL, "Seleccione un Año" },
                        {YEAR_VALUE_2021, "2021" },
                        {YEAR_VALUE_2020, "2020" },
                        {YEAR_VALUE_2019, "2019" },
                        {YEAR_VALUE_2018, "2018" },


                    };

                public const int YEAR_VALUE_NULLABLE_2021 = 2021;
                public const int YEAR_VALUE_NULLABLE_2020 = 2020;
                public const int YEAR_VALUE_NULLABLE_2019 = 2019;
                public const int YEAR_VALUE_NULLABLE_2018 = 2018;

                public static Dictionary<int, string> YEAR_VALUE_NULLABLE = new Dictionary<int, string>
                    {

                        {YEAR_VALUE_NULLABLE_2021, "2021" },
                        {YEAR_VALUE_NULLABLE_2020, "2020" },
                        {YEAR_VALUE_NULLABLE_2019, "2019" },
                        {YEAR_VALUE_NULLABLE_2018, "2018" },


                    };

                public const int YEAR_ALL = 9999;
                public const int YEAR_2022 = 2022;
                public const int YEAR_2021 = 2021;
                public const int YEAR_2020 = 2020;
                public const int YEAR_2019 = 2019;
                public const int YEAR_2018 = 2018;

                public static Dictionary<int, string> YEAR = new Dictionary<int, string>
                    {
                        {YEAR_ALL, "Todos" },
                        {YEAR_2022, "2022" },
                        {YEAR_2021, "2021" },
                        {YEAR_2020, "2020" },
                        {YEAR_2019, "2019" },
                        {YEAR_2018, "2018" },
                    };


                public const int MONTH_VALUE_NULLABLE_JANUARY = 1;
                public const int MONTH_VALUE_NULLABLE_FEBRUARY = 2;
                public const int MONTH_VALUE_NULLABLE_MARCH = 3;
                public const int MONTH_VALUE_NULLABLE_APRIL = 4;
                public const int MONTH_VALUE_NULLABLE_MAY = 5;
                public const int MONTH_VALUE_NULLABLE_JUNE = 6;
                public const int MONTH_VALUE_NULLABLE_JULY = 7;
                public const int MONTH_VALUE_NULLABLE_AUGUST = 8;
                public const int MONTH_VALUE_NULLABLE_SEPTEMBER = 9;
                public const int MONTH_VALUE_NULLABLE_OCTOBER = 10;
                public const int MONTH_VALUE_NULLABLE_NOVEMBER = 11;
                public const int MONTH_VALUE_NULLABLE_DECEMBER = 12;

                public static Dictionary<int, string> MONTH_VALUE_NULLABLE = new Dictionary<int, string>
                    {

                        {MONTH_VALUE_NULLABLE_JANUARY, "Enero" },
                        {MONTH_VALUE_NULLABLE_FEBRUARY, "Febrero" },
                        {MONTH_VALUE_NULLABLE_MARCH, "Marzo" },
                        {MONTH_VALUE_NULLABLE_APRIL, "Abril" },
                        {MONTH_VALUE_NULLABLE_MAY, "Mayo" },
                        {MONTH_VALUE_NULLABLE_JUNE, "Junio" },
                        {MONTH_VALUE_NULLABLE_JULY, "Julio" },
                        {MONTH_VALUE_NULLABLE_AUGUST, "Agosto" },
                        {MONTH_VALUE_NULLABLE_SEPTEMBER, "Setiembre" },
                        {MONTH_VALUE_NULLABLE_OCTOBER, "Octubre" },
                        {MONTH_VALUE_NULLABLE_NOVEMBER, "Noviembre" },
                        {MONTH_VALUE_NULLABLE_DECEMBER, "Diciembre" },

                    };



                public const int MONTH_VALUE_ALL = 9999;
                public const int MONTH_VALUE_JANUARY = 1;
                public const int MONTH_VALUE_FEBRUARY = 2;
                public const int MONTH_VALUE_MARCH = 3;
                public const int MONTH_VALUE_APRIL = 4;
                public const int MONTH_VALUE_MAY = 5;
                public const int MONTH_VALUE_JUNE = 6;
                public const int MONTH_VALUE_JULY = 7;
                public const int MONTH_VALUE_AUGUST = 8;
                public const int MONTH_VALUE_SEPTEMBER = 9;
                public const int MONTH_VALUE_OCTOBER = 10;
                public const int MONTH_VALUE_NOVEMBER = 11;
                public const int MONTH_VALUE_DECEMBER = 12;

                public static Dictionary<int, string> MONTH_VALUE = new Dictionary<int, string>
                    {
                        {MONTH_VALUE_ALL, "Seleccione un Mes" },
                        {MONTH_VALUE_JANUARY, "Enero" },
                        {MONTH_VALUE_FEBRUARY, "Febrero" },
                        {MONTH_VALUE_MARCH, "Marzo" },
                        {MONTH_VALUE_APRIL, "Abril" },
                        {MONTH_VALUE_MAY, "Mayo" },
                        {MONTH_VALUE_JUNE, "Junio" },
                        {MONTH_VALUE_JULY, "Julio" },
                        {MONTH_VALUE_AUGUST, "Agosto" },
                        {MONTH_VALUE_SEPTEMBER, "Setiembre" },
                        {MONTH_VALUE_OCTOBER, "Octubre" },
                        {MONTH_VALUE_NOVEMBER, "Noviembre" },
                        {MONTH_VALUE_DECEMBER, "Diciembre" },

                    };

                public const int MONTH_ALL = 9999;
                public const int MONTH_JANUARY = 1;
                public const int MONTH_FEBRUARY = 2;
                public const int MONTH_MARCH = 3;
                public const int MONTH_APRIL = 4;
                public const int MONTH_MAY = 5;
                public const int MONTH_JUNE = 6;
                public const int MONTH_JULY = 7;
                public const int MONTH_AUGUST = 8;
                public const int MONTH_SEPTEMBER = 9;
                public const int MONTH_OCTOBER = 10;
                public const int MONTH_NOVEMBER = 11;
                public const int MONTH_DECEMBER = 12;

                public static Dictionary<int, string> MONTH = new Dictionary<int, string>
                    {
                        {MONTH_ALL, "Todos" },
                        {MONTH_JANUARY, "Enero" },
                        {MONTH_FEBRUARY, "Febrero" },
                        {MONTH_MARCH, "Marzo" },
                        {MONTH_APRIL, "Abril" },
                        {MONTH_MAY, "Mayo" },
                        {MONTH_JUNE, "Junio" },
                        {MONTH_JULY, "Julio" },
                        {MONTH_AUGUST, "Agosto" },
                        {MONTH_SEPTEMBER, "Setiembre" },
                        {MONTH_OCTOBER, "Octubre" },
                        {MONTH_NOVEMBER, "Noviembre" },
                        {MONTH_DECEMBER, "Diciembre" },

                    };

                public const int IS_FROM_IVC = 1;
                public const int IS_FROM_OTHER = 2;

                public static Dictionary<int, string> IS_FROM = new Dictionary<int, string>
                    {
                        {IS_FROM_IVC, "IVC" },
                        {IS_FROM_OTHER, "Tercero" },
                    };

                public const int HIRING_TYPE_ZERO = 0;
                public const int HIRING_TYPE_EMPLOYEE = 1;
                public const int HIRING_TYPE_WORKER = 2;
                public const int HIRING_TYPE_NONE = 3;
                public const int HIRING_TYPE_ALL = 9999;

                public static Dictionary<int, string> HIRING_TYPE = new Dictionary<int, string>
                    {
                    {HIRING_TYPE_ZERO,"" },
                        { HIRING_TYPE_ALL,"Todos"},
                        {HIRING_TYPE_EMPLOYEE, "Empleado" },
                        {HIRING_TYPE_WORKER, "Obrero" },
                        { HIRING_TYPE_NONE,"Otro" },
                    };
                public const int MACHINERY_TYPE_SOFT = 1;
                public const int MACHINERY_TYPE_TYPE = 2;

                public static Dictionary<int, string> MACHINERY_TYPE = new Dictionary<int, string>
                {
                    { MACHINERY_TYPE_SOFT, "Equipo Menor" },
                    { MACHINERY_TYPE_TYPE, "Maquinaria" }
                };

                public const int MACHINERY_STATUS_STAND_BY = 1;
                public const int MACHINERY_STATUS_IN_USE = 2;
                public const int MACHINERY_STATUS_INOPERATIVE = 3;
                public const int MACHINERY_STATUS_RETIRED = 4;
                public static Dictionary<int, string> MACHINERY_STATUS = new Dictionary<int, string>
                {
                    { MACHINERY_STATUS_STAND_BY, "Stand By" },
                    { MACHINERY_STATUS_IN_USE, "En Uso" },
                    { MACHINERY_STATUS_INOPERATIVE, "Inoperativo" },
                    { MACHINERY_STATUS_RETIRED, "Retirado" },
                };

                public const int INSURANCE_ZERO = 0;
                public const int INSURANCE_RIMAC = 1;
                public const int INSURANCE_MAPFRE = 2;
                public const int INSURANCE_LAPOSITIVA = 3;
                public const int INSURANCE_NULL = 4;
                public const int INSURANCE_PACIFICO = 5;
                public static Dictionary<int, string> MACHINERY_INSURANCE = new Dictionary<int, string>
                {
                    { INSURANCE_ZERO, "SIN FOLDING" },
                    { INSURANCE_RIMAC, "Rimac" },
                    { INSURANCE_MAPFRE, "Mapfre" },
                    { INSURANCE_LAPOSITIVA, "La Positiva" },
                    { INSURANCE_NULL,""},
                    { INSURANCE_PACIFICO,"Pacifico"},
                };



                public const int MACHINERY_SERVICE_CONDITION_S_OPERATOR = 1;
                public const int MACHINERY_SERVICE_CONDITION_ALL_COST = 2;
                public const int MACHINERY_SERVICE_CONDITION_C_OPERATOR = 3;
                public const int MACHINERY_SERVICE_CONDITION_NULL = 4;
                public static Dictionary<int, string> MACHINERY_SERVICE_CONDITION = new Dictionary<int, string>
                {
                    { MACHINERY_SERVICE_CONDITION_S_OPERATOR, "Seca-S/operador" },
                    { MACHINERY_SERVICE_CONDITION_ALL_COST, "A todo costo" },
                    { MACHINERY_SERVICE_CONDITION_C_OPERATOR, "Seca-C/operador" },
                    { MACHINERY_SERVICE_CONDITION_NULL, "" }
                };
            }
        }

        public static class Biddings
        {

            public const int TYPE_SOLO = 1;
            public const int TYPE_CONSORTIUM = 2;
            public static Dictionary<int, string> TYPE = new Dictionary<int, string>
            {
                 { TYPE_SOLO, "Individual"},
                { TYPE_CONSORTIUM, "Consorcio"},
            };


            public const int CURRENCY_PEN = 1;
            public const int CURRENCY_DOLLAR = 2;
            public const int CURRENCY_EURO = 3;

            public static Dictionary<int, string> CURRENCY = new Dictionary<int, string>
            {

                { CURRENCY_PEN, "Soles"},
                { CURRENCY_DOLLAR, "Dolares"},
                { CURRENCY_EURO, "Euros" }

            };

            public const int PARTICIPATION_ALL = 1;
            public const int PARTICIPATION_YES = 2;
            public const int PARTICIPATION_NO = 3;

            public static Dictionary<int, string> PARTICIPATION = new Dictionary<int, string>
            {
                { PARTICIPATION_ALL, "Todos"},
                { PARTICIPATION_YES, "de IVC"},
                { PARTICIPATION_NO, "sin IVC"},

            };

            public const int PERIOD_NULL = 0;
            public const int PERIOD_5 = 5;
            public const int PERIOD_10 = 10;
            public const int PERIOD_15 = 15;
            public const int PERIOD_20 = 20;

            public static Dictionary<int, string> PERIOD = new Dictionary<int, string>
            {
                { PERIOD_NULL, "Todos"},
                { PERIOD_5, "5 años"},
                { PERIOD_10, "10 años"},
                { PERIOD_15, "15 años"},
                { PERIOD_20, "20 años"}
            };


        }

        public static class RacsReports
        {
            public const int IN_PROCESS = 0;
            public const int LIFTED = 1;

            public static Dictionary<int, string> STATUS = new Dictionary<int, string>
            {
                { IN_PROCESS, "En Proceso"},
                { LIFTED, "Levantada"}
            };
        }

        public static class Training
        {
            public static class ResultStatusColor
            {
                public const int GRAY = 0;
                public const int BLUE = 1;
                public const int LIGHT_BLUE = 2;
                public const int GREEN = 3;
                public const int YELLOW = 4;
                public const int RED = 5;
                public const int BLACK = 6;
                public const int PURPLE = 7;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { GRAY, "Gris" },
                    { BLUE, "Azul" },
                    { LIGHT_BLUE, "Celeste" },
                    { GREEN, "Verde" },
                    { YELLOW, "Amarillo" },
                    { RED, "Rojo" },
                    { BLACK, "Negro" },
                    { PURPLE, "Púrpura" },
                };

                public static Dictionary<int, string> CLASSES = new Dictionary<int, string>
                {
                    { GRAY, "default" },
                    { BLUE, "primary" },
                    { LIGHT_BLUE, "info" },
                    { GREEN, "success" },
                    { YELLOW, "warning" },
                    { RED, "danger" },
                    { BLACK, "black" },
                    { PURPLE, "brand" },
                };
            }
        }

        public static class Quality
        {
            public static class EquimentCertificate
            {
                public static class CalibrationVerification
                {
                    public const int METHOD_TRANSFER_CALIBRATION = 0;
                    public const int METHOD_DIRECT_INVERSE_READING = 1;
                    public const int METHOD_TRACEABILITY_OF_PATTERNS = 2;
                    public const int METHOD_VERIFICATION = 3;

                    public static Dictionary<int, string> METHODS = new Dictionary<int, string>
                    {
                        {METHOD_TRANSFER_CALIBRATION, "Calibración por Transferencia" },
                        {METHOD_DIRECT_INVERSE_READING, "Lectura Directa - Inversa" },
                        {METHOD_TRACEABILITY_OF_PATTERNS, "Trazabilidad de los Patrones" },
                        {METHOD_VERIFICATION, "Verificación" }
                    };

                    public const int FRECUENCY_ANNUAL = 0;
                    public const int FRECUENCY_QUATERLY = 1;
                    public const int FRECUENCY_SEMIANNUAL = 2;

                    public static Dictionary<int, string> FRECUENCIES = new Dictionary<int, string>
                    {
                        {FRECUENCY_ANNUAL, "Anual" },
                        {FRECUENCY_QUATERLY, "Cuatrimestral" },
                        {FRECUENCY_SEMIANNUAL, "Semestral" }
                    };
                }

                public const int CONTROL_STATUS_EXPIRE = 0;
                public const int CONTROL_STATUS_OK = 1;
                public const int CONTROL_STATUS_RENEW = 2;

                public static Dictionary<int, string> CONTROL_STATUS = new Dictionary<int, string>
                {
                    {CONTROL_STATUS_EXPIRE, "Vencido" },
                    {CONTROL_STATUS_OK, "Ok" },
                    {CONTROL_STATUS_RENEW, "Tramitar Renovación" }
                };

                public const int OPERATIONAL_STATUS_OUT_OF_ORDER = 0;
                public const int OPERATIONAL_STATUS_WORK = 1;

                public static Dictionary<int, string> OPERATIONAL_STATUS = new Dictionary<int, string>
                {
                    {OPERATIONAL_STATUS_OUT_OF_ORDER, "Inoperativo" },
                    {OPERATIONAL_STATUS_WORK, "Operativo" }
                };

                public const int SITUATIONAL_STATUS_STAND_BY = 0;
                public const int SITUATIONAL_STATUS_IN_USE = 1;
                public const int SITUATIONAL_STATUS_REMOVED = 2;

                public static Dictionary<int, string> SITUATIONAL_STATUS = new Dictionary<int, string>
                {
                    {SITUATIONAL_STATUS_STAND_BY, "Stand by" },
                    {SITUATIONAL_STATUS_IN_USE, "En Uso" },
                    {SITUATIONAL_STATUS_REMOVED, "Retirado de Obra" }
                };
                //--
                public const int INSPECTION_TYPE_CALIBRATION = 1;
                public const int INSPECTION_TYPE_VERIFICATION = 2;
                public const int INSPECTION_TYPE_NONE = 0;
                public static Dictionary<int, string> INSPECTION_TYPE = new Dictionary<int, string>
                {
                    {INSPECTION_TYPE_CALIBRATION, "Calibración" },
                    {INSPECTION_TYPE_VERIFICATION, "Verificación" },
                    {INSPECTION_TYPE_NONE, "" }
                };
                //--
                public const int CALIBRATION_METHOD_DIRECT_COMPARISON = 1;
                public const int CALIBRATION_METHOD_TRANSFER = 2;
                public const int CALIBRATION_METHOD_SUBSTITUTION = 3;
                public const int CALIBRATION_METHOD_BALANCE = 4;
                public const int CALIBRATION_METHOD_SIMULATION = 5;
                public const int CALIBRATION_METHOD_REPRODUCTION = 6;
                public const int CALIBRATION_METHOD_DOTS = 7;
                public const int CALIBRATION_METHOD_NONE = 0;
                public static Dictionary<int, string> CALIBRATION_METHOD = new Dictionary<int, string>
                {
                    {CALIBRATION_METHOD_DIRECT_COMPARISON,"Calibración por comparación directa" },
                    {CALIBRATION_METHOD_TRANSFER,"Calibración por transferencia"},
                    {CALIBRATION_METHOD_SUBSTITUTION ,"Calibración por sustitución"},
                    {CALIBRATION_METHOD_BALANCE ,"Calibración por equilibrio"},
                    {CALIBRATION_METHOD_SIMULATION ,"Calibración por simulación"},
                    {CALIBRATION_METHOD_REPRODUCTION ,"Calibración por reproducción"},
                    { CALIBRATION_METHOD_DOTS , "Calibración por puntos fijos"},
                    { CALIBRATION_METHOD_NONE , ""}
                };

                //--
                public const int CALIBRATION_FRECUENCY_MONTHLY = 1;
                public const int CALIBRATION_FRECUENCY_TRIMONTH = 2;
                public const int CALIBRATION_FRECUENCY_QUARTMONTH = 3;
                public const int CALIBRATION_FRECUENCY_SIXMONTH = 4;
                public const int CALIBRATION_FRECUENCY_YEAR = 5;
                public const int CALIBRATION_FRECUENCY_NONE = 0;
                public static Dictionary<int, string> CALIBRATION_FRECUENCY = new Dictionary<int, string>
                {
                    {CALIBRATION_FRECUENCY_MONTHLY,"Mensual"},
                    {CALIBRATION_FRECUENCY_TRIMONTH,"Trimestral"},
                    {CALIBRATION_FRECUENCY_QUARTMONTH,"Cuatrimestral"},
                    {CALIBRATION_FRECUENCY_SIXMONTH,"Semestral"},
                    {CALIBRATION_FRECUENCY_YEAR,"Anual"},
                    {CALIBRATION_FRECUENCY_NONE,""}
                };
                //--
                public const int VALIDITY_ALL = 9999;
                public const int VALIDITY_OK = 31;
                public const int VALIDITY_WARNING = 16;
                public const int VALIDITY_DANGER = 1;
                public const int VALIDITY_OUT = -1;


                public static Dictionary<int, string> VALIDITY = new Dictionary<int, string>
                {
                    {VALIDITY_ALL,"Todos"},
                    {VALIDITY_OK, "Ok" },
                    {VALIDITY_WARNING, "Advertencia" },
                    {VALIDITY_DANGER, "Urgente" },
                    {VALIDITY_OUT, "Vencido" }
                };

                public const int HAS_AVOID_ALL = 2;
                public const int HAS_AVOID_TRUE = 1;
                public const int HAS_AVOID_FALSE = 0;

                public static Dictionary<int, string> HAS_AVOID = new Dictionary<int, string>
                {
                    {HAS_AVOID_ALL,"Todos"},
                    {HAS_AVOID_TRUE, "Ok" },
                    {HAS_AVOID_FALSE, "Revisar" },
                };

                public const int OPERATIONAL_FILTER_ALL = 2;
                public const int OPERATIONAL_FILTER_OUT_OF_ORDER = 0;
                public const int OPERATIONAL_FILTER_WORK = 1;


                public static Dictionary<int, string> OPERATIONAL_FILTER = new Dictionary<int, string>
                {
                    {OPERATIONAL_FILTER_ALL, "Todos" },
                    {OPERATIONAL_FILTER_OUT_OF_ORDER, "Inoperativo" },
                    {OPERATIONAL_FILTER_WORK, "Operativo" }

                };

                public const int SITUATIONAL_FILTER_ALL = 3;
                public const int SITUATIONAL_FILTER_STAND_BY = 0;
                public const int SITUATIONAL_FILTER_IN_USE = 1;
                public const int SITUATIONAL_FILTER_REMOVED = 2;

                public static Dictionary<int, string> SITUATIONAL_FILTER = new Dictionary<int, string>
                {
                    {SITUATIONAL_FILTER_ALL, "Todos" },
                    {SITUATIONAL_FILTER_STAND_BY, "Stand by" },
                    {SITUATIONAL_STATUS_IN_USE, "En Uso" },
                    {SITUATIONAL_FILTER_REMOVED, "Retirado de Obra" }
                };
            }
        }

        public static class Warehouse
        {
            public const int OUTPUT_VOUCHER = 0;
            public const int INPUT_VOUCHER = 1;

            public static Dictionary<int, string> VOUCHER_TYPES = new Dictionary<int, string>
                {
                    {OUTPUT_VOUCHER, "Vale de Salida" },
                    {INPUT_VOUCHER, "Vale de Ingreso" }
                };

            public static class UserTypes
            {
                public const int ThecnicalOfficeControl = 1;
                public const int OrderRequests = 2;
                public const int StoreKeepers = 3;
            }

            public static class FieldRequest
            {
                public static class Status
                {
                    public const int PRE_ISSUED = 1;
                    public const int ISSUED = 2;
                    public const int READYTOVALIDATE = 3;
                    public const int VALIDATED = 4;
                    public const int READYTOATTEND = 5;
                    public const int ATTENDED = 6;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PRE_ISSUED, "PRE-EMITIDO" },
                        { ISSUED, "EMITIDO" },
                        { READYTOVALIDATE, "LISTO PARA VALIDAR" },
                        { VALIDATED, "VALIDADO" },
                        { READYTOATTEND, "LISTO PARA ATENDER" },
                        { ATTENDED, "ATENDIDO" }
                    };
                }
            }

            public static class SupplyEntry
            {
                public static class Status
                {
                    public const int INPROCESS = 1;
                    public const int CONFIRMED = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { INPROCESS, "EN PROCESO" },
                        { CONFIRMED, "INGRESADO" }
                    };
                }
            }

            public static class ReEntryForReturn
            {
                public static class Status
                {
                    public const int INPROCESS = 1;
                    public const int CONFIRMED = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { INPROCESS, "EN PROCESO" },
                        { CONFIRMED, "INGRESADO" }
                    };
                }
            }
        }

        public static class Productions
        {
            public static class RDPs
            {
                public const int GROUP_CONCRETE = 0;
                public const int GROUP_ARQUITECTURE = 1;

                public static Dictionary<int, string> GROUPS = new Dictionary<int, string>
                {
                    {GROUP_CONCRETE, "Obras de Concreto" },
                    {GROUP_ARQUITECTURE, "Arquitectura" }
                };
            }
        }
    }
}
