using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Security
{
    [NotMapped]
    public class UspRacsH
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
        [Display(Name = "01. Falta Orden y Limpieza en áreas de trabajo y almacén.",
            Prompt = "01. Falta Orden y Limpieza en áreas de trabajo y almacén.")]
        public bool SCQ01 { get; set; }
        [Display(Name = "02. Falta señalización (cintas, carteles) en las áreas o delimitación deficiente",
            Prompt = "02. Falta señalización (cintas, carteles) en las áreas o delimitación deficiente")]
        public bool SCQ02 { get; set; }
        [Display(Name = "03. EPPs NO recomendados al área y al trabajo, no hay stock.",
            Prompt = "03. EPPs NO recomendados al área y al trabajo, no hay stock.")]
        public bool SCQ03 { get; set; }
        [Display(Name = "04. Documentación MASS desactualizados, sin archivar, no impresos, sin firmar y no están publicados.",
            Prompt = "04. Documentación MASS desactualizados, sin archivar, no impresos, sin firmar y no están publicados.")]
        public bool SCQ04 { get; set; }
        [Display(Name = "05. Derrame de Hidrocarburo y/o concreto.",
            Prompt = "05. Derrame de Hidrocarburo y/o concreto.")]
        public bool SCQ05 { get; set; }
        [Display(Name = "06. Escaleras (sin inspección, mal almacenadas, mal posicionada)",
            Prompt = "06. Escaleras (sin inspección, mal almacenadas, mal posicionada)")]
        public bool SCQ06 { get; set; }
        [Display(Name = "07. Instalaciones eléctricas mal instaladas o inadecuadas.",
            Prompt = "07. Instalaciones eléctricas mal instaladas o inadecuadas.")]
        public bool SCQ07 { get; set; }
        [Display(Name = "08. Maderas con clavos, presencia de elementos punzantes.",
            Prompt = "08. Maderas con clavos, presencia de elementos punzantes.")]
        public bool SCQ08 { get; set; }
        [Display(Name = "09. Mal Manejo y disposición de RRSS.",
            Prompt = "09. Mal Manejo y disposición de RRSS.")]
        public bool SCQ09 { get; set; }
        [Display(Name = "10. Accesos inadecuados (sin baranda, berma en mal estado, presencia de obstáculos)",
            Prompt = "10. Accesos inadecuados (sin baranda, berma en mal estado, presencia de obstáculos)")]
        public bool SCQ10 { get; set; }
        [Display(Name = "11. No se evidencia SCTR vigente e impreso y publicado en obra.",
            Prompt = "11. No se evidencia SCTR vigente e impreso y publicado en obra.")]
        public bool SCQ11 { get; set; }
        [Display(Name = "12. Falta agua para el personal.",
            Prompt = "12. Falta agua para el personal.")]
        public bool SCQ12 { get; set; }
        [Display(Name = "13. Ausencia del Residente.",
            Prompt = "13. Ausencia del Residente.")]
        public bool SCQ13 { get; set; }
        [Display(Name = "14. Ausencia del jefe y/o Supervisor SSI.",
            Prompt = "14. Ausencia del jefe y/o Supervisor SSI.")]
        public bool SCQ14 { get; set; }
        [Display(Name = "15. Equipos de emergencias en mal estado, sin inspección, materiales incompletos o ausencia o mal ubicados.",
            Prompt = "15. Equipos de emergencias en mal estado, sin inspección, materiales incompletos o ausencia o mal ubicados.")]
        public bool SCQ15 { get; set; }
        [Display(Name = "16. Mal manejo de producto químico (mal almacenamiento, mala disposición, falta hoja MSDS, sin rotulado)",
            Prompt = "16. Mal manejo de producto químico (mal almacenamiento, mala disposición, falta hoja MSDS, sin rotulado)")]
        public bool SCQ16 { get; set; }
        [Display(Name = "17. Andamios (sin tarjeta, sin barandas, rodapiés, sin inspección, plataforma incompleta, inestables)",
            Prompt = "17. Andamios (sin tarjeta, sin barandas, rodapiés, sin inspección, plataforma incompleta, inestables)")]
        public bool SCQ17 { get; set; }
        [Display(Name = "18. Falta o deficiente colocación de dispositivos de seguridad (guardas u otros)",
            Prompt = "18. Falta o deficiente colocación de dispositivos de seguridad (guardas u otros)")]
        public bool SCQ18 { get; set; }
        [Display(Name = "19. Diseño inadecuado del equipo o del trabajo.",
            Prompt = "19. Diseño inadecuado del equipo o del trabajo.")]
        public bool SCQ19 { get; set; }
        [Display(Name = "20. Falta o deficiente Iluminación en la zona de trabajo.",
            Prompt = "20. Falta o deficiente Iluminación en la zona de trabajo.")]
        public bool SCQ20 { get; set; }
        [Display(Name = "21. Materiales inflamables cerca de trabajos en caliente.",
            Prompt = "21. Materiales inflamables cerca de trabajos en caliente.")]
        public bool SCQ21 { get; set; }
        [Display(Name = "22. Mantenimiento inadecuado de equipos o herramientas.",
            Prompt = "22. Mantenimiento inadecuado de equipos o herramientas.")]
        public bool SCQ22 { get; set; }
        [Display(Name = "23. Deficiente estados de los sistemas de cierre (Cerraduras, pestillos, candados, otros)",
            Prompt = "23. Deficiente estados de los sistemas de cierre (Cerraduras, pestillos, candados, otros)")]
        public bool SCQ23 { get; set; }
        [Display(Name = "24. Escaleras, andamios, plataformas en mal estado.",
            Prompt = "24. Escaleras, andamios, plataformas en mal estado.")]
        public bool SCQ24 { get; set; }
        [Display(Name = "25. Condiciones climáticas adversas (tormenta eléctrica, granizada, etc.)",
            Prompt = "25. Condiciones climáticas adversas (tormenta eléctrica, granizada, etc.)")]
        public bool SCQ25 { get; set; }
        [Display(Name = "26. Presencia de agentes nocivos en la atmósfera.",
            Prompt = "26. Presencia de agentes nocivos en la atmósfera.")]
        public bool SCQ26 { get; set; }
        [Display(Name = "27. Material cerca al borde de la zanja.",
            Prompt = "27. Material cerca al borde de la zanja.")]
        public bool SCQ27 { get; set; }
        [Display(Name = "28.  Falta de coordinación o ausencia de vigías/ Excesiva velocidad de vehículos particulares/ Tolva de volquete sin asegurar/ Incumplimiento de Manejo Defensivo/ Trabajador dentro de radio de trabajo.",
            Prompt = "28.  Falta de coordinación o ausencia de vigías/ Excesiva velocidad de vehículos particulares/ Tolva de volquete sin asegurar/ Incumplimiento de Manejo Defensivo/ Trabajador dentro de radio de trabajo")]
        public bool SCQ28 { get; set; }
        [Display(Name = "29. Otras condiciones subestándares.",
            Prompt = "29. Otras condiciones subestándares.")]
        public bool SCQ29 { get; set; }

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
        [Display(Name = "01. Malas posturas, riesgo ergonómico.",
            Prompt = "01. Malas posturas, riesgo ergonómico.")]
        public bool SAQ01 { get; set; }
        [Display(Name = "02. Trabajador sobre plataformas inestables.",
            Prompt = "02. Trabajador sobre plataformas inestables.")]
        public bool SAQ02 { get; set; }
        [Display(Name = "03. EPP´s son utilizados inadecuadamente o no los usan.",
            Prompt = "03. EPP´s son utilizados inadecuadamente o no los usan.")]
        public bool SAQ03 { get; set; }
        [Display(Name = "04. Sin tarjetas de entrenamiento o desactualizadas o sin dictado de cursos.",
            Prompt = "04. Sin tarjetas de entrenamiento o desactualizadas o sin dictado de cursos.")]
        public bool SAQ04 { get; set; }
        [Display(Name = "05. No utilizar lentes de seguridad o en mal estado.",
            Prompt = "05. No utilizar lentes de seguridad o en mal estado.")]
        public bool SAQ05 { get; set; }
        [Display(Name = "06. Personal sin protección de manos o guantes en mal estado.",
            Prompt = "06. Personal sin protección de manos o guantes en mal estado.")]
        public bool SAQ06 { get; set; }
        [Display(Name = "07. No utilizar arnés, mala utilización de línea de anclaje y línea de vida, mal almacenamiento, falta de inspección.",
            Prompt = "07. No utilizar arnés, mala utilización de línea de anclaje y línea de vida, mal almacenamiento, falta de inspección.")]
        public bool SAQ07 { get; set; }
        [Display(Name = "08. No se generan permisos para trabajos de alto riesgo, firmas incompletas, no está en el área de trabajo.",
            Prompt = "08. No se generan permisos para trabajos de alto riesgo, firmas incompletas, no está en el área de trabajo.")]
        public bool SAQ08 { get; set; }
        [Display(Name = "09. No utilizar protector respiratorio según la actividad (polvos, gases, vapores, etc.)",
            Prompt = "09. No utilizar protector respiratorio según la actividad (polvos, gases, vapores, etc.)")]
        public bool SAQ09 { get; set; }
        [Display(Name = "10. Mala utilización de herramientas manuales (sin inspección, malas condiciones) o hechizas.",
            Prompt = "10. Mala utilización de herramientas manuales (sin inspección, malas condiciones) o hechizas.")]
        public bool SAQ10 { get; set; }
        [Display(Name = "11. No se generó el ATS o Evaluación deficiente o firmas incompletas.",
            Prompt = "11. No se generó el ATS o Evaluación deficiente o firmas incompletas.")]
        public bool SAQ11 { get; set; }
        [Display(Name = "12. No se generó la charla diaria o no firman el registro.",
            Prompt = "12. No se generó la charla diaria o no firman el registro.")]
        public bool SAQ12 { get; set; }
        [Display(Name = "13. No cumplir con los procedimientos de trabajo establecidos.",
            Prompt = "13. No cumplir con los procedimientos de trabajo establecidos.")]
        public bool SAQ13 { get; set; }
        [Display(Name = "14. Interferir o retirar dispositivos de Seguridad o de control Ambiental.",
            Prompt = "14. Interferir o retirar dispositivos de Seguridad o de control Ambiental.")]
        public bool SAQ14 { get; set; }
        [Display(Name = "15. Trabajar sobre equipos en movimiento o riesgosos.",
            Prompt = "15. Trabajar sobre equipos en movimiento o riesgosos.")]
        public bool SAQ15 { get; set; }
        [Display(Name = "16. Operar o conducir a velocidad inadecuada o distraído.",
            Prompt = "16. Operar o conducir a velocidad inadecuada o distraído.")]
        public bool SAQ16 { get; set; }
        [Display(Name = "17. Adoptar posiciones o posturas peligrosas.",
            Prompt = "17. Adoptar posiciones o posturas peligrosas.")]
        public bool SAQ17 { get; set; }
        [Display(Name = "18. Falta de atención en la tarea.",
            Prompt = "18. Falta de atención en la tarea.")]
        public bool SAQ18 { get; set; }
        [Display(Name = "19. Distraer, molestar, insultar, reñir, sorprender a otros colaboradores.",
            Prompt = "19. Distraer, molestar, insultar, reñir, sorprender a otros colaboradores.")]
        public bool SAQ19 { get; set; }

        [Display(Name = "20. No asegurar las herramientas o materiales en trabajos en altura.",
            Prompt = "20. No asegurar las herramientas o materiales en trabajos en altura.")]
        public bool SAQ20 { get; set; }
        [Display(Name = "21. No respetar el área de seguridad en las maniobras.",
            Prompt = "21. No respetar el área de seguridad en las maniobras.")]
        public bool SAQ21 { get; set; }
        [Display(Name = "22. Realizar trabajos sin bloquear energías al intervenir maquinas o equipos.",
            Prompt = "22. Realizar trabajos sin bloquear energías al intervenir maquinas o equipos.")]
        public bool SAQ22 { get; set; }
        [Display(Name = "23. Ingresar a áreas restringidas sin autorización.",
            Prompt = "23. Ingresar a áreas restringidas sin autorización.")]
        public bool SAQ23 { get; set; }
        [Display(Name = "24. No utilizar equipos de contención de derrames.",
            Prompt = "24. No utilizar equipos de contención de derrames.")]
        public bool SAQ24 { get; set; }
        [Display(Name = "25. No realizar la segregación adecuada de los residuos.",
            Prompt = "25. No realizar la segregación adecuada de los residuos.")]
        public bool SAQ25 { get; set; }
        [Display(Name = "26. Manipulación o traslado inadecuado de materiales peligrosos.",
            Prompt = "26. No asegurar las herramientas y materiales en altura")]
        public bool SAQ26 { get; set; }
        [Display(Name = "27. Dejar material, equipos, otros, abandonados/expuestos.",
            Prompt = "27. Dejar material, equipos, otros, abandonados/expuestos.")]
        public bool SAQ27 { get; set; }
        [Display(Name = "28. Dejar puertas y ventanas abiertas al término de sus labores.",
            Prompt = "28. Dejar puertas y ventanas abiertas al término de sus labores.")]
        public bool SAQ28 { get; set; }
        [Display(Name = "29. Retirar material, equipos, otros, sin autorización escrita.",
            Prompt = "29. Retirar material, equipos, otros, sin autorización escrita.")]
        public bool SAQ29 { get; set; }
        [Display(Name = "30. No realizar el Check list de Pre Uso.",
            Prompt = "30. No realizar el Check list de Pre Uso.")]
        public bool SAQ30 { get; set; }
        [Display(Name = "31. Fumar en zonas no autorizadas o cerca combustibles.",
            Prompt = "31. Fumar en zonas no autorizadas o cerca combustibles.")]
        public bool SAQ31 { get; set; }
        [Display(Name = "32. Levantar objetos de forma inadecuada.",
            Prompt = "32. Levantar objetos de forma inadecuada.")]
        public bool SAQ32 { get; set; }
        [Display(Name = "33. Realizar trabajos sin estar capacitado.",
            Prompt = "33. Realizar trabajos sin estar capacitado.")]
        public bool SAQ33 { get; set; }
        [Display(Name = "34. Trabajar bajo influencia de alcohol y/o drogas.",
            Prompt = "34. Trabajar bajo influencia de alcohol y/o drogas.")]
        public bool SAQ34 { get; set; }
        [Display(Name = "35. Realizar trabajos de alto riesgo (altura, caliente, etc.) sin autorización.",
            Prompt = "35. Realizar trabajos de alto riesgo (altura, caliente, etc.) sin autorización.")]
        public bool SAQ35 { get; set; }
        [Display(Name = "36. Otros actos subestándares.",
            Prompt = "36. Otros actos subestándares.")]
        public bool SAQ36 { get; set; }

        [Display(Name = "Especificar actos...",
            Prompt = "Especificar actos...")]
        public string SpecifyActs { get; set; }

        [Display(Name = "Acciones correctivas aplicadas...",
            Prompt = "Acciones correctivas aplicadass...")]
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
