using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.UspModels.Security;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.ViewModels.RacsReportViewModels
{
    public class RacsReportViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Código",
            Prompt = "Código")]
        public string Code { get; set; }
        [Display(Name = "Proyecto",
            Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        [Display(Name = "Fecha de Reporte",
            Prompt = "Fecha de Reporte")]
        public string ReportDate { get; set; }
        [Display(Name = "Reporta",
            Prompt = "Reporta")]
        public string ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }
        [Display(Name = "Cuadrilla",
            Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }
        public Uri SignatureUrl { get; set; }

        //Substandar Condition
        [Display(Name = "¿Identifica Condición Subestándar?", 
            Prompt = "¿Identifica Condición Subestándar?")]
        public bool IdentifiesSC { get; set; }
        [Display(Name = "Descripción de la Condición Subestándar", 
            Prompt = "Descripción de la Condición Subestándar")]
        public string DescriptionIdentifiesSC { get; set; }
        [Display(Name = "01. Falta o Inadecuada barreras, guardas, barricadas, etc.", 
            Prompt = "01. Falta o Inadecuada barreras, guardas, barricadas, etc.")]
        public bool SCQ01 { get; set; }
        [Display(Name = "02. Equipo de protección Incorrecto o Inadecuado", 
            Prompt = "02. Equipo de protección Incorrecto o Inadecuado")]
        public bool SCQ02 { get; set; }
        [Display(Name = "03. Herramientas, equipos o materiales defectuosos.", 
            Prompt = "03. Herramientas, equipos o materiales defectuosos.")]
        public bool SCQ03 { get; set; }
        [Display(Name = "04. Congestión o Acción Restringida.", 
            Prompt = "04. Congestión o Acción Restringida.")]
        public bool SCQ04 { get; set; }
        [Display(Name = "05. Sistema de Aseguramiento, Advertencia Inadecuado o Inexistente.", 
            Prompt = "05. Sistema de Aseguramiento, Advertencia Inadecuado o Inexistente.")]
        public bool SCQ05 { get; set; }
        [Display(Name = "06. Peligros de incendio y explosión", 
            Prompt = "06. Peligros de incendio y explosión")]
        public bool SCQ06 { get; set; }
        [Display(Name = "07. Orden y limpieza deficientes/desorden", 
            Prompt = "07. Orden y limpieza deficientes/desorden")]
        public bool SCQ07 { get; set; }
        [Display(Name = "08. Exposición al ruido", 
            Prompt = "08. Exposición al ruido")]
        public bool SCQ08 { get; set; }
        [Display(Name = "09. Exposición a la radiación", 
            Prompt = "09. Exposición a la radiación")]
        public bool SCQ09 { get; set; }
        [Display(Name = "10. Temperaturas extremas", 
            Prompt = "10. Temperaturas extremas")]
        public bool SCQ10 { get; set; }
        [Display(Name = "11. Iluminación deficiente o excesiva", 
            Prompt = "11. Iluminación deficiente o excesiva")]
        public bool SCQ11 { get; set; }
        [Display(Name = "12. Ventilación inadecuada", 
            Prompt = "12. Ventilación inadecuada")]
        public bool SCQ12 { get; set; }
        [Display(Name = "13. Condiciones ambientales peligrosas", 
            Prompt = "13. Condiciones ambientales peligrosas")]
        public bool SCQ13 { get; set; }
        [Display(Name = "14. Accesos inadecuados (caminos, pisos, superficies inadecuadas)", 
            Prompt = "14. Accesos inadecuados (caminos, pisos, superficies inadecuadas)")]
        public bool SCQ14 { get; set; }
        [Display(Name = "15. Escaleras portátiles o rampas subestanadres", 
            Prompt = "15. Escaleras portátiles o rampas subestanadres")]
        public bool SCQ15 { get; set; }
        [Display(Name = "16. Andamios y plataformas subestandares",
            Prompt = "16. Andamios y plataformas subestandares")]
        public bool SCQ16 { get; set; }
        [Display(Name = "17. Instalaciones eléctricas en mal estado, sin protección",
            Prompt = "17. Instalaciones eléctricas en mal estado, sin protección")]
        public bool SCQ17 { get; set; }
        [Display(Name = "18. Vehículos y maquinaria rodante subestandares",
            Prompt = "18. Vehículos y maquinaria rodante subestandares")]
        public bool SCQ18 { get; set; }
        [Display(Name = "19. Equipos/Herramientas subestandares o inadecuados/sin inspección",
            Prompt = "19. Equipos/Herramientas subestandares o inadecuados/sin inspección")]
        public bool SCQ19 { get; set; }
        [Display(Name = "20. Falta de señalización/señalización inadecuada",
            Prompt = "20. Falta de señalización/señalización inadecuada")]
        public bool SCQ20 { get; set; }
        [Display(Name = "21. Documentación inexistente/inadecuada/inviable",
            Prompt = "21. Documentación inexistente/inadecuada/inviable")]
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
        [Display(Name = "26. Otras condiciones subestandar",
            Prompt = "26. Otras condiciones subestandar")]
        public bool SCQ26 { get; set; }
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
        [Display(Name = "01. Operar equipo sin autorización",
            Prompt = "01. Operar equipo sin autorización")]
        public bool SAQ01 { get; set; }
        [Display(Name = "02. No advertir/No comunicar una condición u acto subestandar",
            Prompt = "02. No advertir/No comunicar una condición u acto subestandar")]
        public bool SAQ02 { get; set; }
        [Display(Name = "03. No asegurar",
            Prompt = "03. No asegurar")]
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
        [Display(Name = "07. No usar los EPP correctamente",
            Prompt = "07. No usar los EPP correctamente")]
        public bool SAQ07 { get; set; }
        [Display(Name = "08. Cargar de forma incorrecta (peso, longitud, volumen)",
            Prompt = "08. Cargar de forma incorrecta (peso, longitud, volumen)")]
        public bool SAQ08 { get; set; }
        [Display(Name = "09. Posicionarse de forma incorrecta dentro de una condición subestandar",
            Prompt = "09. Posicionarse de forma incorrecta dentro de una condición subestandar")]
        public bool SAQ09 { get; set; }
        [Display(Name = "10. Levantar carga de forma incorrecta",
            Prompt = "10. Levantar carga de forma incorrecta")]
        public bool SAQ10 { get; set; }
        [Display(Name = "11. Trabajar en posición indebida",
            Prompt = "11. Trabajar en posición indebida")]
        public bool SAQ11 { get; set; }
        [Display(Name = "12. Dar mantenimiento a equipo en operación",
            Prompt = "12. Dar mantenimiento a equipo en operación")]
        public bool SAQ12 { get; set; }
        [Display(Name = "13. Jugar con compañeros",
            Prompt = "13. Jugar con compañeros")]
        public bool SAQ13 { get; set; }
        [Display(Name = "14. Trabajar bajo la influencia de alcohol y/o drogas",
            Prompt = "14. Trabajar bajo la influencia de alcohol y/o drogas")]
        public bool SAQ14 { get; set; }
        [Display(Name = "15. Usar de forma inapropiada equipos y/o herramientas",
            Prompt = "15. Usar de forma inapropiada equipos y/o herramientas")]
        public bool SAQ15 { get; set; }
        [Display(Name = "16. No cumplir procedimientos/estandares establecidos",
            Prompt = "16. No cumplir procedimientos/estandares establecidos")]
        public bool SAQ16 { get; set; }
        [Display(Name = "17. Relizar de forma inadecuada la inspección pre-operativa",
            Prompt = "17. Relizar de forma inadecuada la inspección pre-operativa")]
        public bool SAQ17 { get; set; }
        [Display(Name = "18. Realizar de forma incorrecta la evaluación de riesgo",
            Prompt = "18. Realizar de forma incorrecta la evaluación de riesgo")]
        public bool SAQ18 { get; set; }
        [Display(Name = "19. Otros actos subestandar",
            Prompt = "19. Otros actos subestandar")]
        public bool SAQ19 { get; set; }
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
        public string SpecifyAppliedCorrections { get; set; }
        [Display(Name = "Acciones correctivas aplicadas...",
            Prompt = "Acciones correctivas aplicadas...")]
        public string SpecifyAnotherAlternative { get; set; }
        public Uri ObservationImageUrl { get; set; }

        //RACS Lifting
        [Display(Name = "Levantamiento de observaciones",
            Prompt = "Levantamiento de observaciones")]
        public string LiftingObservations { get; set; }
        public Uri LiftingImageUrl { get; set; }
        [Display(Name = "Estado",
            Prompt = "Estado")]
        public int Status { get; set; }

    }

    public class RacsPdfViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Código",
            Prompt = "Código")]
        public string Code { get; set; }
                
        public Guid ProjectId { get; set; }
        [Display(Name = "Proyecto",
            Prompt = "Proyecto")]
        public string ProjectAbbreviation { get; set; }

        [Display(Name = "Fecha de Reporte",
            Prompt = "Fecha de Reporte")]
        public string ReportDate { get; set; }

        public string ApplicationUserId { get; set; }
        [Display(Name = "Reporta",
            Prompt = "Reporta")]
        public string ApplicationUserName { get; set; }

        public Guid SewerGroupId { get; set; }
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
        [Display(Name = "01. Falta o Inadecuada barreras, guardas, barricadas, etc.",
            Prompt = "01. Falta o Inadecuada barreras, guardas, barricadas, etc.")]
        public bool SCQ01 { get; set; }
        [Display(Name = "02. Equipo de protección Incorrecto o Inadecuado",
            Prompt = "02. Equipo de protección Incorrecto o Inadecuado")]
        public bool SCQ02 { get; set; }
        [Display(Name = "03. Herramientas, equipos o materiales defectuosos.",
            Prompt = "03. Herramientas, equipos o materiales defectuosos.")]
        public bool SCQ03 { get; set; }
        [Display(Name = "04. Congestión o Acción Restringida.",
            Prompt = "04. Congestión o Acción Restringida.")]
        public bool SCQ04 { get; set; }
        [Display(Name = "05. Sistema de Aseguramiento, Advertencia Inadecuado o Inexistente.",
            Prompt = "05. Sistema de Aseguramiento, Advertencia Inadecuado o Inexistente.")]
        public bool SCQ05 { get; set; }
        [Display(Name = "06. Peligros de incendio y explosión",
            Prompt = "06. Peligros de incendio y explosión")]
        public bool SCQ06 { get; set; }
        [Display(Name = "07. Orden y limpieza deficientes/desorden",
            Prompt = "07. Orden y limpieza deficientes/desorden")]
        public bool SCQ07 { get; set; }
        [Display(Name = "08. Exposición al ruido",
            Prompt = "08. Exposición al ruido")]
        public bool SCQ08 { get; set; }
        [Display(Name = "09. Exposición a la radiación",
            Prompt = "09. Exposición a la radiación")]
        public bool SCQ09 { get; set; }
        [Display(Name = "10. Temperaturas extremas",
            Prompt = "10. Temperaturas extremas")]
        public bool SCQ10 { get; set; }
        [Display(Name = "11. Iluminación deficiente o excesiva",
            Prompt = "11. Iluminación deficiente o excesiva")]
        public bool SCQ11 { get; set; }
        [Display(Name = "12. Ventilación inadecuada",
            Prompt = "12. Ventilación inadecuada")]
        public bool SCQ12 { get; set; }
        [Display(Name = "13. Condiciones ambientales peligrosas",
            Prompt = "13. Condiciones ambientales peligrosas")]
        public bool SCQ13 { get; set; }
        [Display(Name = "14. Accesos inadecuados (caminos, pisos, superficies inadecuadas)",
            Prompt = "14. Accesos inadecuados (caminos, pisos, superficies inadecuadas)")]
        public bool SCQ14 { get; set; }
        [Display(Name = "15. Escaleras portátiles o rampas subestanadres",
            Prompt = "15. Escaleras portátiles o rampas subestanadres")]
        public bool SCQ15 { get; set; }
        [Display(Name = "16. Andamios y plataformas subestandares",
            Prompt = "16. Andamios y plataformas subestandares")]
        public bool SCQ16 { get; set; }
        [Display(Name = "17. Instalaciones eléctricas en mal estado, sin protección",
            Prompt = "17. Instalaciones eléctricas en mal estado, sin protección")]
        public bool SCQ17 { get; set; }
        [Display(Name = "18. Vehículos y maquinaria rodante subestandares",
            Prompt = "18. Vehículos y maquinaria rodante subestandares")]
        public bool SCQ18 { get; set; }
        [Display(Name = "19. Equipos/Herramientas subestandares o inadecuados/sin inspección",
            Prompt = "19. Equipos/Herramientas subestandares o inadecuados/sin inspección")]
        public bool SCQ19 { get; set; }
        [Display(Name = "20. Falta de señalización/señalización inadecuada",
            Prompt = "20. Falta de señalización/señalización inadecuada")]
        public bool SCQ20 { get; set; }
        [Display(Name = "21. Documentación inexistente/inadecuada/inviable",
            Prompt = "21. Documentación inexistente/inadecuada/inviable")]
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
        [Display(Name = "26. Otras condiciones subestandar",
            Prompt = "26. Otras condiciones subestandar")]
        public bool SCQ26 { get; set; }
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
        [Display(Name = "01. Operar equipo sin autorización",
            Prompt = "01. Operar equipo sin autorización")]
        public bool SAQ01 { get; set; }
        [Display(Name = "02. No advertir/No comunicar una condición u acto subestandar",
            Prompt = "02. No advertir/No comunicar una condición u acto subestandar")]
        public bool SAQ02 { get; set; }
        [Display(Name = "03. No asegurar",
            Prompt = "03. No asegurar")]
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
        [Display(Name = "07. No usar los EPP correctamente",
            Prompt = "07. No usar los EPP correctamente")]
        public bool SAQ07 { get; set; }
        [Display(Name = "08. Cargar de forma incorrecta (peso, longitud, volumen)",
            Prompt = "08. Cargar de forma incorrecta (peso, longitud, volumen)")]
        public bool SAQ08 { get; set; }
        [Display(Name = "09. Posicionarse de forma incorrecta dentro de una condición subestandar",
            Prompt = "09. Posicionarse de forma incorrecta dentro de una condición subestandar")]
        public bool SAQ09 { get; set; }
        [Display(Name = "10. Levantar carga de forma incorrecta",
            Prompt = "10. Levantar carga de forma incorrecta")]
        public bool SAQ10 { get; set; }
        [Display(Name = "11. Trabajar en posición indebida",
            Prompt = "11. Trabajar en posición indebida")]
        public bool SAQ11 { get; set; }
        [Display(Name = "12. Dar mantenimiento a equipo en operación",
            Prompt = "12. Dar mantenimiento a equipo en operación")]
        public bool SAQ12 { get; set; }
        [Display(Name = "13. Jugar con compañeros",
            Prompt = "13. Jugar con compañeros")]
        public bool SAQ13 { get; set; }
        [Display(Name = "14. Trabajar bajo la influencia de alcohol y/o drogas",
            Prompt = "14. Trabajar bajo la influencia de alcohol y/o drogas")]
        public bool SAQ14 { get; set; }
        [Display(Name = "15. Usar de forma inapropiada equipos y/o herramientas",
            Prompt = "15. Usar de forma inapropiada equipos y/o herramientas")]
        public bool SAQ15 { get; set; }
        [Display(Name = "16. No cumplir procedimientos/estandares establecidos",
            Prompt = "16. No cumplir procedimientos/estandares establecidos")]
        public bool SAQ16 { get; set; }
        [Display(Name = "17. Relizar de forma inadecuada la inspección pre-operativa",
            Prompt = "17. Relizar de forma inadecuada la inspección pre-operativa")]
        public bool SAQ17 { get; set; }
        [Display(Name = "18. Realizar de forma incorrecta la evaluación de riesgo",
            Prompt = "18. Realizar de forma incorrecta la evaluación de riesgo")]
        public bool SAQ18 { get; set; }
        [Display(Name = "19. Otros actos subestandar",
            Prompt = "19. Otros actos subestandar")]
        public bool SAQ19 { get; set; }
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
        public string SpecifyAppliedCorrections { get; set; }
        [Display(Name = "Acciones correctivas aplicadas...",
            Prompt = "Acciones correctivas aplicadas...")]
        public string SpecifyAnotherAlternative { get; set; }
        public Uri ObservationImageUrl { get; set; }

        //RACS Lifting
        [Display(Name = "Levantamiento de observaciones",
            Prompt = "Levantamiento de observaciones")]
        public string LiftingObservations { get; set; }
        public Uri LiftingImageUrl { get; set; }
        [Display(Name = "Estado",
            Prompt = "Estado")]
        public int Status { get; set; }

    }

    public class UspRacsViewModel
    {
        public List<UspRacsSewegroup> UspRacsSewegroups { get; set; }
        public List<UspRacsSCQ> UspRacsSCQs { get; set; }
        public List<UspRacsSAQ> UspRacsSAQs { get; set; }
    }
}
