<%@ Page Language="C#" MasterPageFile="WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AllStudentView.aspx.cs" Inherits="WinEr.AllStudentView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	
	<link href="css%20files/dataTable.css" rel="stylesheet" />
	<script src="js%20files/dataTableExport.js"></script>
	<script src="js%20files/dataTables.js"></script>


	<style>
	 table.dataTable thead .sorting, table.dataTable thead .sorting_asc, table.dataTable thead .sorting_desc, table.dataTable thead .sorting_asc_disabled, table.dataTable thead .sorting_desc_disabled{
		 font-weight: 400 ;
	 }
	 table.dataTable>tbody>tr.child ul.dtr-details{
		 text-align:left;
	 }
	 table.dataTable>tbody>tr.child span.dtr-title{
			 min-width: 110px;
	font-weight: 500 !important;
	margin-right: 20px;
	 }
		td {
			-ms-word-wrap: break-word;
			word-wrap: break-word;
		}

		.buttons-collection {
			/*position: absolute !important;*/
		}
		.dt-buttons{

		}
		.dt-button-background{
			display:none !important;
		}
		.btn-group-vertical>.btn, .btn-group>.btn{
				padding: 0 30px 0 0;
		}
		.dt-button-collection{
			margin-top: 10px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<%--<ajaxToolkit:ToolkitScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true"/>--%>
	<%--<form id="form1" runat="server">--%>
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
	<div class="container-fluid card0">
		<ol class="breadcrumb">
			<%--<li><a href="#">Home</a></li>--%>
			<li class="active"><a href="#">All student Details</a></li>
		</ol>
		<div class="well text-center card0" style="margin:  0;padding: 10px;background-color: white;border-bottom: 0;">
			<input type="button" class="_btnShowLiveDtls btn btn-default" value="view All Live Student" />
			<input type="button" class="_btnShowHistDtls btn btn-default" value="History Student" />
			<input type="button" class="_btnShowTempDtls btn btn-default" value="Temporary / Registered Student" />
            <input type="button" class="_btnShowPromoDtls btn btn-default" value="PromotionList Student" />
			<span style="padding:0 20px 0 20px;"><a href="AllStudentView.aspx"><i class="fa fa-refresh"  style="color:green;"></i></a></span>
			<%--<a data-toggle="collapse" href="#allStdntColapse">View data filters</a>--%>

		</div>
		 <div class="panel-group">
			<div class="panel panel-default">
			  <div id="allStdntColapse" class="panel-collapse collapse">
				<div class="panel-body">
					<div class="_allStdntFilterDt"></div>
					
				</div>
				<%--<div class="panel-footer">Panel Footer</div>--%>
			  </div>
			</div>
		  </div>
		
		<%--<hr>
		<div class="row">
			<%--<input id="btnShowDtls" type="button" class="btn btn-default" name="view" />--%>
		<%--</div>

		<hr>--%>
		<div class="">
<%--			<table id="studentTable" class="table table-striped table-bordered cell-border hover dt-responsive dt-head-nowrap" style="width: 100%;"></table>--%>
			<table id="studentTable1" class="table table-striped card0" style="width:100%"></table>
			 <table id="studentTable2" class="table table-striped card0" style="width:100%"></table>
			 <table id="studentTable3" class="table table-striped card0" style="width:100%"></table>
            <table id="studentTable4" class="table table-striped card0" style="width:100%"></table>
		</div>
	</div>
	<%--</form>--%>
	<script>

		var initLimit = 100;
		var studentTable = null;
		var offset = 0;
		var obj = {};
		obj.limit = initLimit;
		obj.offset = offset;
		//allStudentDetails_aspx.makeFilter();
		$(document).ready(function () {

			$("._btnShowLiveDtls").on("click", function (e) {
				sessionStorage.setItem("$ALL$STD$TYPE$",1);//All Live
				obj.limit = 100;
				obj.offset = 0;
				obj.studentType = "LIVE";//LIVE//HISTORY//TEMPORARY
				$.ajax({
					type: "POST", url: "AppWebServices/StudentDataService.asmx/getAllStudentDt",
					contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
					data: JSON.stringify(obj),
					success: function (data) {
						if (data.d)
							createTable.OnLiveStudent(data.d, "LIVE");
					 //   $("#studentTable1").show();
					   // $("#studentTable3,#studentTable3_wrapper,#studentTable2,#studentTable2_wrapper").hide();
					}
				});
			});
			$("._btnShowHistDtls").on("click", function (e) {
				sessionStorage.setItem("$ALL$STD$TYPE$", 2);//all history
				obj.limit = 100;
				obj.offset = 0;
				obj.studentType = "HISTORY";//LIVE//HISTORY//TEMPORARY
				$.ajax({
					type: "POST", url: "AppWebServices/StudentDataService.asmx/getAllStudentDt",
					contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
					data: JSON.stringify(obj),
					success: function (data) {
						if (data.d)
							createTable.OnHistoryStudent(data.d, "HISTORY");
					   // $("#studentTable2").show();
					  //  $("#studentTable3,#studentTable3_wrapper,#studentTable1,#studentTable1_wrapper").hide();
					}
				});
			});
			$("._btnShowTempDtls").on("click", function (e) {
				sessionStorage.setItem("$ALL$STD$TYPE$",5);
				obj.limit = 100;
				obj.offset = 0;
				obj.studentType = "TEMPORARY";//LIVE//HISTORY//TEMPORARY
				$.ajax({
					type: "POST", url: "AppWebServices/StudentDataService.asmx/getAllStudentDt",
					contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
					data: JSON.stringify(obj),
					success: function (data) {
						if (data.d)
							createTable.OnTempStudent(data.d, "TEMPORARY");
					  //  $("#studentTable3").show();
					  //  $("#studentTable2,#studentTable2_wrapper,#studentTable1,#studentTable1_wrapper").hide();
					}
				});
			});
			$("._btnShowPromoDtls").on("click", function (e) {
			    sessionStorage.setItem("$ALL$STD$TYPE$", 5);
			    obj.limit = 100;
			    obj.offset = 0;
			    obj.studentType = "PROMOTION";//LIVE//HISTORY//TEMPORARY
			    $.ajax({
			        type: "POST", url: "AppWebServices/StudentDataService.asmx/getAllStudentDt",
			        contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
			        data: JSON.stringify(obj),
			        success: function (data) {
			            if (data.d)
			                createTable.OnPromotionStudent(data.d, "PROMOTION");
			            //  $("#studentTable3").show();
			            //  $("#studentTable2,#studentTable2_wrapper,#studentTable1,#studentTable1_wrapper").hide();
			        }
			    });
			});
			
		});
		function stdntDtlsAction(stdAction, stdId, stdntStatus) {
			PageMethods.StudentDtlsview(stdId, stdAction, stdntStatus, OnSuccess3, OnLoadDataError);
			function OnSuccess3(response, userContext, methodName) {
				if (response) {
					var actnUrl = response[0],
						actnMsg = response[1],
						stdntID = response[2],
						stdntType = response[3];
					if (stdntID) {
						document.cookie = "$CUR$STDNT$ID$=" + stdntID;
						document.cookie = "$CUR$STDNT$TYPE$=" + stdntType;
						if (actnUrl) {
							location.href = actnUrl;
							return false;
						} else {
							return actnMsg;
						}
					}
					else {
						alert("Something went wrong !");
					}
				}
			}
		}
		function addNextData(limits, offsets,type) {
			// $('#SubPreLoader').show();
			obj.limit = limits;
			obj.offset = offsets;
			$.ajax({
				type: "POST", url: "AppWebServices/StudentDataService.asmx/getAllStudentDt",
				contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
				data: JSON.stringify(obj),
				success: function (data) {
					if (data.d) {
						// OnGetAllDataSuccess(data.d);
						var dt = JSON.parse(data.d[1]);
						//if (type=="LIVE")
						studentTable.rows.add(dt).draw(false);

					}
				}
			});
		}
		function dtTableExtn() {
			studentTable.on('select deselect', function () {
				var selectedRows = table.rows({ selected: true }).count();
				studentTable.button(0).enable(selectedRows === 1);
				studentTable.button(1).enable(selectedRows > 0);
			});
		}
		var createTable = {
			OnLiveStudent: function (response, studentType) {
				var columns = [],data = JSON.parse(response[1]);
				columnNames = Object.keys(data[0]);
				for (var i in columnNames)
					columns.push({ data: columnNames[i], title: columnNames[i] });
				$('#studentTable1_wrapper').show();
				$('#studentTable2_wrapper,#studentTable3_wrapper,#studentTable4_wrapper').hide();
				loaddtTableSettings();
				studentTable = $('#studentTable1').DataTable({ data: data, columns: columns });
				dtTableExtn();
				if (response[0] > initLimit)
					addNextData(response[0], initLimit, studentType);
			},
			OnHistoryStudent: function (response, studentType) {
				var columns = [],data = JSON.parse(response[1]);
				columnNames = Object.keys(data[0]);
				for (var i in columnNames)
					columns.push({ data: columnNames[i], title: columnNames[i] });
				$('#studentTable2_wrapper').show();
				$('#studentTable1_wrapper,#studentTable3_wrapper,#studentTable3_wrapper').hide();
				loaddtTableSettings();
				studentTable = $('#studentTable2').DataTable({ data: data, columns: columns });
				dtTableExtn();
				if (response[0] > initLimit)
					addNextData(response[0], initLimit, studentType);
			},
			OnTempStudent: function (response, studentType) {
				var columns = [],data = JSON.parse(response[1]);
				columnNames = Object.keys(data[0]);
				for (var i in columnNames)
					columns.push({ data: columnNames[i], title: columnNames[i] });
				$('#studentTable3_wrapper').show();
				$('#studentTable1_wrapper,#studentTable2_wrapper,#studentTable4_wrapper').hide();
				loaddtTableSettings();
				studentTable = $('#studentTable3').DataTable({ data: data, columns: columns });		        
				dtTableExtn();
				if (response[0] > initLimit)
					addNextData(response[0], initLimit, studentType);
			},
			OnPromotionStudent: function (response, studentType) {
			    var columns = [], data = JSON.parse(response[1]);
			    columnNames = Object.keys(data[0]);
			    for (var i in columnNames)
			        columns.push({ data: columnNames[i], title: columnNames[i] });
			    $('#studentTable4_wrapper').show();
			    $('#studentTable1_wrapper,#studentTable2_wrapper,#studentTable3_wrapper').hide();
			    loaddtTableSettings();
			    studentTable = $('#studentTable4').DataTable({ data: data, columns: columns });
			    dtTableExtn();
			    //if (response[0] > initLimit)
			    //    addNextData(response[0], initLimit, studentType);
			},
		}


		
		function loaddtTableSettings() {
		    var instLocal = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$"));
		    if (instLocal)
		        instiDt = instLocal[1];
		    else
		        instiDt = "";
		    //var instiDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$")),
			 dateTime = new Date().toLocaleString(),
			year = (new Date().getFullYear());
			$.extend(true, $.fn.dataTable.defaults, {
				bFilter: true,
				responsive: true,
				destroy: true,
				paging: true,
				pageLength:100,
				dom: 'Bftip',
				select: true,
				//initComplete: function () {
				//	this.api().columns([7, 6, 9, 10, 3, 11]).every(function () {
				//		var column = this;
				//		var select = $('<select><option value="">' + column.title() + '</option></select>')
				//			.appendTo($(column.header()).empty())
				//			.on('change', function () {
				//				var val = $.fn.dataTable.util.escapeRegex(
				//					$(this).val()
				//				);
				//				column.search(val ? '^' + val + '$' : '', true, false).draw();
				//			});
				//		select.append('<select><option value=" " selected="selected"></select>');
				//		column.data().unique().each(function (d, j) {
				//			var val = $.fn.dataTable.util.escapeRegex(d);
				//			if (column.search() === "^" + val + "$") {
				//				select.append(
				//				  '<option value="' + d + '" selected="selected">' + d + "</option>"
				//				);
				//			} else {
				//				select.append('<option value="' + d + '">' + d + "</option>");
				//			}
				//		});
				//	})
				//},
				columnDefs: [//DEFAULT CLOUMN form is text
					{
						targets: [0],
						visible: false,
						searchable: false,
						sortable:false
					}
				],
				buttons: [
							{
								extend: 'selected', // Bind to Selected row
								text: '<i class="fa fa-eye"  style="color:blue;"></i>  View Student',
								name: 'edit',      // do not change name,
								enabled: false,
								action: function () {
									//get selected rows studentId
									var stdId = studentTable.rows({ selected: true }).data()[0].Id;
									
									var stdStatus = sessionStorage.getItem("$ALL$STD$TYPE$");
									if (stdStatus == 5)
										location.href = "ViewRegisteredStudents.aspx";
									else
										stdntDtlsAction("view", stdId, stdStatus);
									
								}
							},
							{
								extend: 'collection',
								text: '<i class="fa fa-cloud-download" style="color:#3F51B5;"></i>  Export Student Data',
								////buttons: ['copy', 'excel', 'csv', 'pdf', 'print', 'colvis']
								buttons: [
									{
										extend: 'excelHtml5',
										text: '<i class="fa fa-file-excel-o" style="color:green;"></i>  Export all column in Excel',
										titleAttr: 'Excel',
										messageTop: instiDt,
										footer: true
									},
									 {
										 extend: 'csvHtml5',
										 text: '<i class="fa fa-file-text-o"></i>  Export in column in csv ',
										 titleAttr: 'CSV',
										 footer: true

									 },
									
									 {
										 extend: 'colvis',
										 text: '<i class="fa fa-eye"></i>  Select column visibility',
										 titleAttr: 'select column'
									 },
									{
										extend: 'csvHtml5',
										text: '<i class="fa fa-file-text-o"></i>  Export visible column in csv ',
										titleAttr: 'CSV',
										exportOptions: {
											columns: ':visible'
										}
									},
									 {
										 extend: 'excelHtml5',
										 text: '<i class="fa fa-file-excel-o" style="color:green;"></i>  Export visible column in Excel',
										 titleAttr: 'Excel',
										 messageTop: instiDt,
										 footer: true,
										 exportOptions: {
											 columns: ':visible'
										 }
									 },
									{
										extend: 'pdfHtml5',
										text: '<i class="fa fa-file-pdf-o" style="color:red;"></i>  Export visible column in pdf',
										messageTop: instiDt,
										footer: true,
										titleAttr: 'PDF'
										//exportOptions: {
										//    columns: ':visible'
										//}
									},
									{
										extend: 'copyHtml5',
										text: '<i class="fa fa-sticky-note-o" style="color:#03A9F4;"></i>  Copy visible column',
										titleAttr: 'Copy',
										exportOptions: {
											columns: ':visible'
										}
									},
									{
										extend: 'print',
										text: '<i class="fa fa-file-excel-o" style="color:green;"></i>  Print visible column',
										exportOptions: {
											columns: ':visible'
										},
										messageTop: function () {
											return instiDt;
										},
										messageBottom: null
									}
								]
							}
				]
			});
		}
	</script>
</asp:Content>
