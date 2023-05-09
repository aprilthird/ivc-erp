namespace IVC.PE.ENTITIES.UspModels.Security
{
    using IVC.PE.CORE.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class UspRacs
    {
        public Guid? Id { get; set; }

        [Display(Name = "Código",
            Prompt = "Código")]
        public string Code { get; set; }

        public Guid ProjectId { get; set; }
        [Display(Name = "Proyecto",
            Prompt = "Proyecto")]
        public string ProjectAbbreviation { get; set; }
        public Uri ProjectLogoUrl { get; set; }

        public DateTime ReportDate { get; set; }
        [Display(Name = "Fecha de Reporte",
            Prompt = "Fecha de Reporte")]
        public string ReportDateStr => ReportDate.ToDateString();

        public string ApplicationUserId { get; set; }
        [Display(Name = "Reporta",
            Prompt = "Reporta")]
        public string ApplicationUserName { get; set; }

        public string LiftResponsibleId { get; set; }
        [Display(Name = "Responsable de Levantar Observación",
            Prompt = "Responsable de Levantar Observación")]
        public string LiftResponsibleName { get; set; }

        public Guid? WorkFrontId { get; set; }
        [Display(Name = "Frente de Trabajo",
            Prompt = "Frente de Trabajo")]
        public string WorkFrontCode { get; set; }

        public Guid? SewerGroupId { get; set; }
        [Display(Name = "Cuadrilla",
            Prompt = "Cuadrilla")]
        public string SewerGroupCode { get; set; }

        public Uri SignatureUrl { get; set; }

        //Substandar Condition
        [Display(Name = "¿Identifica Condición Subestándar?",
            Prompt = "¿Identifica Condición Subestándar?")]
        public bool IdentifiesSC { get; set; }
        [Display(Name = "Descripción de la Condición Subestándar",
            Prompt = "Descripción de la Condición Subestándar")]
        public string DescriptionIdentifiesSC { get; set; }
        [Display(Name = "01. Falta o inadecuadas barreras, guardas, bermas, barricadas, etc.",
            Prompt = "01. Falta o inadecuadas barreras, guardas, bermas, barricadas, etc.")]
        public bool SCQ01 { get; set; }
        [Display(Name = "02. Equipo de protección incorrecto o Inadecuado",
            Prompt = "02. Equipo de protección incorrecto o Inadecuado")]
        public bool SCQ02 { get; set; }
        [Display(Name = "03. Herramientas, Equipo o Materiales defectuosos.",
            Prompt = "03. Herramientas, Equipo o Materiales defectuosos.")]
        public bool SCQ03 { get; set; }
        [Display(Name = "04. Congestión o Acción Restringida.",
            Prompt = "04. Congestión o Acción Restringida.")]
        public bool SCQ04 { get; set; }
        [Display(Name = "05. Sistema de Aseguramiento, Advertencia Inadecuado o inexistente.",
            Prompt = "05. Sistema de Aseguramiento, Advertencia Inadecuado o inexistente.")]
        public bool SCQ05 { get; set; }
        [Display(Name = "06. Área con material inflamable almacenados inadecuadamente/ cerca de trabajos en caliente",
            Prompt = "06. Área con material inflamable almacenados inadecuadamente/ cerca de trabajos en caliente")]
        public bool SCQ06 { get; set; }
        [Display(Name = "07. Orden y Limpieza deficientes/ Desorden",
            Prompt = "07. Orden y Limpieza deficientes/ Desorden")]
        public bool SCQ07 { get; set; }
        [Display(Name = "08. Área con exceso de ruido",
            Prompt = "08. Área con exceso de ruido")]
        public bool SCQ08 { get; set; }
        [Display(Name = "09. Radiación cerca a los trabajadores",
            Prompt = "09. Radiación cerca a los trabajadores")]
        public bool SCQ09 { get; set; }
        [Display(Name = "10. Áreas con temperaturas extremas y/o altas",
            Prompt = "10. Áreas con temperaturas extremas y/o altas")]
        public bool SCQ10 { get; set; }
        [Display(Name = "11. Áreas con iluminación Deficiente o Excesiva",
            Prompt = "11. Áreas con iluminación Deficiente o Excesiva")]
        public bool SCQ11 { get; set; }
        [Display(Name = "12. Área con ventilación Inadecuada o deficiente/ agentes nocivos a la atmosfera",
            Prompt = "12. Área con ventilación Inadecuada o deficiente/ agentes nocivos a la atmosfera")]
        public bool SCQ12 { get; set; }
        [Display(Name = "13. Condiciones Ambientales Peligrosas/Mal manejo de productos químicos /Derrames",
            Prompt = "13. Condiciones Ambientales Peligrosas/Mal manejo de productos químicos /Derrames")]
        public bool SCQ13 { get; set; }
        [Display(Name = "14. Accesos inadecuados (Caminos, pisos, superficies inadecuadas.)",
            Prompt = "14. Accesos inadecuados (Caminos, pisos, superficies inadecuadas.)")]
        public bool SCQ14 { get; set; }
        [Display(Name = "15. Escaleras portátiles o rampas subestándares",
            Prompt = "15. Escaleras portátiles o rampas subestándares")]
        public bool SCQ15 { get; set; }
        [Display(Name = "16. Andamios y plataformas subestándares, sin inspección, sin tarjetas, inestables)",
            Prompt = "16. Andamios y plataformas subestándares, sin inspección, sin tarjetas, inestables)")]
        public bool SCQ16 { get; set; }
        [Display(Name = "17. Instalaciones eléctricas en mal estado, sin protección, deficiencia del sistema de cierre.",
            Prompt = "17. Instalaciones eléctricas en mal estado, sin protección, deficiencia del sistema de cierre.")]
        public bool SCQ17 { get; set; }
        [Display(Name = "18. Vehículos y maquinaria rodante inadecuados/ sin inspección",
            Prompt = "18. Vehículos y maquinaria rodante inadecuados/ sin inspección")]
        public bool SCQ18 { get; set; }
        [Display(Name = "19. Equipos/equipos de emergencia/herramientas inadecuadas/ sin inspección",
            Prompt = "19. Equipos/equipos de emergencia/herramientas inadecuadas/ sin inspección")]
        public bool SCQ19 { get; set; }
        [Display(Name = "20. Falta de señalización/señalización inadecuada",
            Prompt = "20. Falta de señalización/señalización inadecuada")]
        public bool SCQ20 { get; set; }
        [Display(Name = "21. Documentación MASS inexistente/inadecuada/ inviable",
            Prompt = "21. Documentación MASS inexistente/inadecuada/ inviable")]
        public bool SCQ21 { get; set; }
        [Display(Name = "22. Peligros ergonómicos",
            Prompt = "22. Peligros ergonómicos")]
        public bool SCQ22 { get; set; }
        [Display(Name = "23. Baños sucios",
            Prompt = "23. Baños sucios")]
        public bool SCQ23 { get; set; }
        [Display(Name = "24. Obstáculos en el área de trabajo",
            Prompt = "24. Obstáculos en el área de trabajo")]
        public bool SCQ24 { get; set; }
        [Display(Name = "25. Falta de elementos, componentes, equipamientos",
            Prompt = "25. Falta de elementos, componentes, equipamientos")]
        public bool SCQ25 { get; set; }
        [Display(Name = "26. Maderas con clavos, presencia de elementos punzocortantes",
            Prompt = "26. Maderas con clavos, presencia de elementos punzocortantes")]
        public bool SCQ26 { get; set; }
        [Display(Name = "27. Mal manejo y disposición de residuos sólidos",
            Prompt = "27. Mal manejo y disposición de residuos sólidos")]
        public bool SCQ27 { get; set; }
        [Display(Name = "28. Otras condiciones subestándares",
            Prompt = "28. Otras condiciones subestándares")]
        public bool SCQ28 { get; set; }
        [Display(Name = "Especificar condición...",
            Prompt = "Especificar condición...")]
        public string SpecifyConditions { get; set; }

        //Substandar Act
        [Display(Name = "¿Identifica acto Subestandar?",
            Prompt = "¿Identifica acto Subestandar?")]
        public bool IdentifiesSA { get; set; }
        [Display(Name = "Descripción del acto Subestandar",
            Prompt = "Descripción del acto Subestandar")]
        public string DescriptionIdentifiesSA { get; set; }
        [Display(Name = "01. Operar equipo sin autorización/sin tarjeta de entrenamiento, retirar sin autorización",
            Prompt = "01. Operar equipo sin autorización/sin tarjeta de entrenamiento, retirar sin autorización")]
        public bool SAQ01 { get; set; }
        [Display(Name = "02. No advertir / No comunicar una condición u acto subestandar",
            Prompt = "02. No advertir / No comunicar una condición u acto subestandar")]
        public bool SAQ02 { get; set; }
        [Display(Name = "03. No asegurar, interferir, retirar dispositivos de seguridad o de control ambiental",
            Prompt = "03. No asegurar, interferir, retirar dispositivos de seguridad o de control ambiental")]
        public bool SAQ03 { get; set; }
        [Display(Name = "04. Operar a velocidad indebida",
            Prompt = "04. Operar a velocidad indebida")]
        public bool SAQ04 { get; set; }
        [Display(Name = "05. Desactivar dispositivos de seguridad/no usar cinturón de seguridad",
            Prompt = "05. Desactivar dispositivos de seguridad/no usar cinturón de seguridad")]
        public bool SAQ05 { get; set; }
        [Display(Name = "06. Usar equipos defectuosos",
            Prompt = "06. Usar equipos defectuosos")]
        public bool SAQ06 { get; set; }
        [Display(Name = "07. No usar los EPP correctamente, especificar EPP.",
            Prompt = "07. No usar los EPP correctamente, especificar EPP.")]
        public bool SAQ07 { get; set; }
        [Display(Name = "08. Cargar de forma incorrecta (peso, longitud, volumen)/Riesgos ergonómicos",
            Prompt = "08. Cargar de forma incorrecta (peso, longitud, volumen)/Riesgos ergonómicos")]
        public bool SAQ08 { get; set; }
        [Display(Name = "09. Posicionarse de forma incorrecta dentro de una condición subestandar (plataformas inestables)",
            Prompt = "09. Posicionarse de forma incorrecta dentro de una condición subestandar (plataformas inestables)")]
        public bool SAQ09 { get; set; }
        [Display(Name = "10. Levantar carga de forma incorrecta",
            Prompt = "10. Levantar carga de forma incorrecta")]
        public bool SAQ10 { get; set; }
        [Display(Name = "11. Trabajar en posición indebida",
            Prompt = "11. Trabajar en posición indebida")]
        public bool SAQ11 { get; set; }
        [Display(Name = "12. Dar mantenimiento a equipo en operación, sin bloqueo de energía.",
            Prompt = "12. Dar mantenimiento a equipo en operación, sin bloqueo de energía.")]
        public bool SAQ12 { get; set; }
        [Display(Name = "13. Jugar con compañeros, distraer, insultar, reñir, molestar, sorprender",
            Prompt = "13. Jugar con compañeros, distraer, insultar, reñir, molestar, sorprender")]
        public bool SAQ13 { get; set; }
        [Display(Name = "14. Trabajar bajo la influencia de alcohol y/o drogas",
            Prompt = "14. Trabajar bajo la influencia de alcohol y/o drogas")]
        public bool SAQ14 { get; set; }
        [Display(Name = "15. Usar de forma inapropiada equipos y/o herramientas",
            Prompt = "15. Usar de forma inapropiada equipos y/o herramientas")]
        public bool SAQ15 { get; set; }
        [Display(Name = "16. Dejar materiales abandonados o expuestos",
            Prompt = "16. Dejar materiales abandonados o expuestos")]
        public bool SAQ16 { get; set; }
        [Display(Name = "17. Relizar de forma inadecuada la inspección pre-operativa",
            Prompt = "17. Relizar de forma inadecuada la inspección pre-operativa")]
        public bool SAQ17 { get; set; }
        [Display(Name = "18. Realizar de forma incorrecta la evaluación de riesgo",
            Prompt = "18. Realizar de forma incorrecta la evaluación de riesgo")]
        public bool SAQ18 { get; set; }
        [Display(Name = "19. Realizar de forma Incorrecta la evaluación del riesgo",
            Prompt = "19. Realizar de forma Incorrecta la evaluación del riesgo")]
        public bool SAQ19 { get; set; }

        [Display(Name = "20. Material cerca del borde de zanja",
            Prompt = "20. Material cerca del borde de zanja")]
        public bool SAQ20 { get; set; }
        [Display(Name = "21. Ausencia de vigía",
            Prompt = "21. Ausencia de vigía")]
        public bool SAQ21 { get; set; }
        [Display(Name = "22. No utilizar arnés/ inadecuado uso de línea de anclaje y línea de vida, falta de inspección",
            Prompt = "22. No utilizar arnés/ inadecuado uso de línea de anclaje y línea de vida, falta de inspección")]
        public bool SAQ22 { get; set; }
        [Display(Name = "23. No generar los permisos para trabajo de alto riesgo, IPERC, firmas incompletas; no vigentes, no estar en el área de trabajo, sin autorización",
            Prompt = "23. No generar los permisos para trabajo de alto riesgo, IPERC, firmas incompletas; no vigentes, no estar en el área de trabajo, sin autorización")]
        public bool SAQ23 { get; set; }
        [Display(Name = "24. No se realiza firma del registro de charla diaria",
            Prompt = "24. No se realiza firma del registro de charla diaria")]
        public bool SAQ24 { get; set; }
        [Display(Name = "25. No cumplir procedimiento de herramienta manuales y de poder",
            Prompt = "25. No cumplir procedimiento de herramienta manuales y de poder")]
        public bool SAQ25 { get; set; }
        [Display(Name = "26. No asegurar las herramientas y materiales en altura",
            Prompt = "26. No asegurar las herramientas y materiales en altura")]
        public bool SAQ26 { get; set; }
        [Display(Name = "27. No respetar el área de seguridad en las maniobras",
            Prompt = "27. No respetar el área de seguridad en las maniobras")]
        public bool SAQ27 { get; set; }
        [Display(Name = "28. Ingresar a áreas restringidas sin autorización",
            Prompt = "28. Ingresar a áreas restringidas sin autorización")]
        public bool SAQ28 { get; set; }
        [Display(Name = "29. No utiliza equipo de contención de derrames",
            Prompt = "29. No utiliza equipo de contención de derrames")]
        public bool SAQ29 { get; set; }
        [Display(Name = "30. No realizar la segregación adecuada de los residuos sólidos",
            Prompt = "30. No realizar la segregación adecuada de los residuos sólidos")]
        public bool SAQ30 { get; set; }
        [Display(Name = "31. Manipulación o traslado inadecuado de material peligroso",
            Prompt = "31. Manipulación o traslado inadecuado de material peligroso")]
        public bool SAQ31 { get; set; }
        [Display(Name = "32. Otros actos subestándar",
            Prompt = "32. Otros actos subestándar")]
        public bool SAQ32 { get; set; }
        [Display(Name = "Especificar actos...",
            Prompt = "Especificar actos...")]
        public string SpecifyActs { get; set; }

        //Immediate Correction
        [Display(Name = "Dar a conocer con anticipación el estados y las condiciones del sitio de trabajo",
            Prompt = "Dar a conocer con anticipación el estados y las condiciones del sitio de trabajo")]
        public bool ICQ01 { get; set; }
        [Display(Name = "Realizar los reportes correspondientes para seguir los conductos regulares de los ajustes",
            Prompt = "Realizar los reportes correspondientes para seguir los conductos regulares de los ajustes")]
        public bool ICQ02 { get; set; }
        [Display(Name = "Organizar los elementos de trabajo en el sitio correspondiente",
            Prompt = "Organizar los elementos de trabajo en el sitio correspondiente")]
        public bool ICQ03 { get; set; }
        [Display(Name = "Solicitar el cambio de EPP o de herramientas que estén en mal estado",
            Prompt = "Solicitar el cambio de EPP o de herramientas que estén en mal estado")]
        public bool ICQ04 { get; set; }
        [Display(Name = "¿Otras alternativas?",
            Prompt = "¿Otras alternativas?")]
        public bool ICQ05 { get; set; }
        [Display(Name = "Otras alternativas...",
            Prompt = "Otras alternativas...")]
        public string SpecifyAnotherAlternative { get; set; }
        [Display(Name = "Acciones correctivas aplicadas...",
            Prompt = "Acciones correctivas aplicadas...")]
        public string SpecifyAppliedCorrections { get; set; }
        public Uri ObservationImageUrl { get; set; }

        //RACS Lifting
        [Display(Name = "Levantamiento de observaciones",
            Prompt = "Levantamiento de observaciones")]
        public string LiftingObservations { get; set; }
        public Uri LiftingImageUrl { get; set; }
        [Display(Name = "Estado",
            Prompt = "Estado")]
        public int Status { get; set; }

        public string VersionCode { get; set; }
    }
}
