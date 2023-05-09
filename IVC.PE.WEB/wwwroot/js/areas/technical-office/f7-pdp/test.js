var resultados = null;
var value1s = [];
var value2s = [];
var value3s = [];
var value4s = [];
var sources = [];

var sewergroup = null;

var cant = null;

$(function () {
	/*
	$("#sheduler_form").find("[name='select_sheduler_sewergroups']").change(function () {
		let formElements = $("#sheduler_form");
		sewergroup.append(formElements.find("[name='select_sheduler_sewergroups']").val());
		formElements.find("#SewerGroupSheduler").val(sewergroup);
		console.log($("#sheduler_form").find("#SewerGroupSheduler").val());
	});
	*/

	$("#Lanzar").on("click", function () {

		let formElements = $("#sheduler_form");
		console.log(formElements.find("[name='select_sheduler_sewergroups']").val());
		sewergroup = formElements.find("[name='select_sheduler_sewergroups']").val();
		console.log(sewergroup);
		$(formElements).find("[name='SewerGroupShedulers']").append($(formElements).find("[name='select_sheduler_sewergroups']").val());
		console.log($("#sheduler_form").find("[name='SewerGroupShedulers']").val());
		console.log($("#sheduler_form").find("[name='StartDate']").val());
		console.log($("#sheduler_form").find("[name='EndDate']").val());

		let data = new FormData($(formElements).get(0));
		console.log(data);
		$.ajax({
			url: _app.parseUrl("/oficina-tecnica/f7-pdp/listar-scheduler"),
			method: "GET",
			data: data,
			contentType: false,
			processData: false
		}).done(function (result) {
			///-----Se retorna un matriz -> cada f7 tiene su folding con los datos de longitudes
			sources = [];
			result.forEach(function (f7) {
				f7.folding.forEach(function (folding) {
					var fecha = new Date(folding.calendarDate);
					//-----Los values son los puntos que estarán dentro del calendario
					if (folding.excavatedLength != "0")
						value1s.push({
							//----Estructura para subir las fechas en este planificador -> mes/día/año
							from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							label: folding.excavatedLength,
							desc: "Something",
							customClass: "ganttRed",
							dataObj: folding.excavatedLength
						});
					if (folding.installedLength != "0")
						value2s.push({
							from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							label: folding.installedLength,
							desc: "Something",
							customClass: "ganttGreen",
							dataObj: folding.installedLength
						});
					if (folding.refilledLength != "0")
						value3s.push({
							from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							label: folding.refilledLength,
							desc: "Something",
							customClass: "ganttBlue",
							dataObj: folding.refilledLength
						});
					if (folding.granularBaseLength != "0")
						value4s.push({
							from: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							to: fecha.getMonth() + 1 + "/" + fecha.getDate() + "/" + fecha.getFullYear(),
							label: folding.granularBaseLength,
							desc: "Something",
							customClass: "ganttOrange",
							dataObj: folding.granularBaseLength
						});
				});
				///-----Los source son la columna base que conforma el calendario de la parte izquierda
				sources.push(
					{
						name: f7.sewerManifold.name,
						desc: "# Capas: " + f7.theoreticalLayer,
						values: []
					},
					{
						name: "L. Exca. : " + f7.sewerManifold.lengthOfDigging,
						desc: "Excavada",
						values: value1s
					},
					{
						name: "L. Instal. : " + f7.sewerManifold.lengthOfPipeInstalled,
						desc: "Instalada",
						values: value2s
					},
					{
						name: "H. Zanja: " + f7.sewerManifold.ditchHeight,
						desc: "Rellenada",
						values: value3s
					},
					{
						name: "H. Relleno: " + f7.filling,
						desc: "Base Granular",
						values: value4s
					},
					{
						name: "",
						desc: "",
						values: []
					}
				);
				///----------al final de agregar, reseteamos los valores
				value1s = [];
				value2s = [];
				value3s = [];
				value4s = [];
			});
			console.log(sources);
			//Número de tramos * Número de filas por tramo
			cant = 6 * 6;
			console.log(cant);
			$("#gant").gantt({
				source: sources,
				months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Augosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
				dow: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
				navigate: "scroll",
				maxScale: "days",
				minScale: "days",
				itemsPerPage: cant,
				onItemClick: function (data) {
					alert(data);
					console.log($(".dataPanel").width());
				},
				onAddClick: function (dt, rowId) {
					alert("Empty space clicked - add an item!");
				},
				onRender: function () {
					console.log("aki");
					$(".bar").width("26px");
					$(".spacer").text("TRAMOS");
					$(".fn-label").css("fontSize", 13);
				}
			});
		});
	});

	//prettyPrint();
});
