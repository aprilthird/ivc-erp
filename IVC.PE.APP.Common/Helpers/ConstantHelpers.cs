using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Helpers
{
    public static class ConstantHelpers
    {
        public static class Menus
        {
            public static class Groups
            {
                public const int GENERAL = 0;
                public const int PRODUCTION = 1;
                public const int LOGISTIC = 2;
                public const int WAREHOUSE = 3;
                public const int TECHNICAL_OFFICE = 4;
                public const int TECHNICAL_AREA = 5;
                public const int EQUIPMENTS = 6;
                public const int QUALITY = 7;
                public const int SECURITY = 8;
                public const int ENVIRONMENT = 9;
                public const int HRWORKER = 10;
                public const int SIG = 11;

                public static Dictionary<int, string> DESC = new Dictionary<int, string>
                {
                    {GENERAL, "General" },
                    {PRODUCTION, "Producción" },
                    {LOGISTIC, "Logística" },
                    {WAREHOUSE, "Almacenes-Logística" },
                    {TECHNICAL_OFFICE, "Oficina Técnica" },
                    {TECHNICAL_AREA, "Área Técnica" },
                    {QUALITY, "Calidad" },
                    {SECURITY, "Seguridad" },
                    {ENVIRONMENT, "Medio Ambiente" },
                    {HRWORKER, "RRHH Obreros" },
                    {EQUIPMENTS, "Equipos" },
                    {SIG, "SIG" }
                };

                public static Dictionary<int, string> MENU_HEADERS = new Dictionary<int, string>
                {
                    {GENERAL, $"{GENERAL}. {DESC[GENERAL]}" },
                    {PRODUCTION, $"{PRODUCTION}. {DESC[PRODUCTION]}" },
                    {LOGISTIC, $"{LOGISTIC}. {DESC[LOGISTIC]}" },
                    {WAREHOUSE, $"{WAREHOUSE}. {DESC[WAREHOUSE]}" },
                    {TECHNICAL_OFFICE, $"{TECHNICAL_OFFICE}. {DESC[TECHNICAL_OFFICE]}" },
                    {TECHNICAL_AREA, $"{TECHNICAL_AREA}. {DESC[TECHNICAL_AREA]}" },
                    {QUALITY, $"{QUALITY}. {DESC[QUALITY]}" },
                    {SECURITY, $"{SECURITY}. {DESC[SECURITY]}" },
                    {ENVIRONMENT, $"{ENVIRONMENT}. {DESC[ENVIRONMENT]}" },
                    {HRWORKER, $"{HRWORKER}. {DESC[HRWORKER]}"},
                    {EQUIPMENTS, $"{EQUIPMENTS}. {DESC[EQUIPMENTS]}" },
                    {SIG, $"{SIG}. {DESC[SIG]}" },

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

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PRE_ISSUED, "PRE-EMITIDO" },
                        { ISSUED, "EMITIDO" },
                        { APPROVED, "APROBADO" },
                        { CANCELED, "ANULADO" },
                        { ORDER_C, "O/C GENERADA" },
                        { ORDER_S, "O/S GENERADA" },
                        { OBSERVED, "OBSERVADO" },
                        { APPROVED_PARTIALLY, "APROBADO PARCIALMENTE" }
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
                    public const int PROCESSED = 3;
                    public const int PROCESSED_PARTIALLY = 4;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { PRE_ISSUED, "PRE-EMITIDO" },
                        { ISSUED, "EMITIDO" },
                        { PROCESSED, "PROCESADO" },
                        { PROCESSED_PARTIALLY, "PROCESADO PARCIALMENTE" }
                    };
                }
            }
        }


        public static class ResponseMessages
        {
            public const string SUCCESS = "Éxito";
            public const string FAIL = "Error";

            public const string SUCCESS_ADDED = "Se agregó satisfactoriamente.";
            public const string SUCCESS_EDITED = "Se editó satisfactoriamente.";
            public const string SUCCESS_REMOVED = "Se eliminó satisfactoriamente.";

            public const string OK = "Entendido";
        }

        public static class HrWorker
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
            }
        }
    }
}
