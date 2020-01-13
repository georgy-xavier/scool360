<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Codebehind="StudentDetails.aspx.cs" Inherits="StdentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">	
	<script type="text/javascript">
		function preventBack() { window.history.forward(); }
		setTimeout("preventBack()", 0);
		window.onunload = function () { null };
		jQuery(document).ready(function ($) {
		   
			studentDetails.getMainDt();
			studentDetails.getSubMenu();
			//studentDetails.getTopDt();
		   
			$("._studExcelExprtBtn").click(function () {
				//studentDetails_aspx.exportCurStudentDtlsExcel();
				studentDetails.getExcelExportDt();
			});
			$("._studPdfExprtBtn").click(function () {

				studentDetails.getPdfExportDt();
			});
		});
	 </script>
	</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></ajaxToolkit:ToolkitScriptManager>
	
	<div class="container-fluid cudtomContFluid">   
			<div class="col-lg-10 col-md-10 col-xs-12">
				<div class="row">
					<div class="card0">
						<div class="cardHd">Student Details</div>
						 <%-- student top div --%>
						<div class="row stntStripBody" >
							<div class="_stdntTopStrip"></div>
						</div>
					</div>
				</div>
				<br>
				<div class="row">
					<div class="card0">
						 <ul class="nav nav-tabs">
							<li class="active"><a data-toggle="tab" href="#home"><i class="fa fa-home"></i> General</a></li>
							<li><a data-toggle="tab" href="#menu1"><i class="fa fa-address-card-o"></i> Communication</a></li>
							<li><a data-toggle="tab" href="#menu2"><i class="fa fa-group"></i> Family</a></li>
							<li><a data-toggle="tab" href="#menu3"><i class="fa fa-book"></i> Admission</a></li>
							<li><a data-toggle="tab" href="#menu4"><i class="fa fa-exclamation-circle"></i> Others</a></li>
							<li><a data-toggle="tab" href="#menu5"><i class="fa fa-exclamation-triangle"></i> Incidence</a></li>
							 <li>&nbsp;&nbsp;</li>
							 <li style="font-weight: 400;color: #607D8B;line-height: 50px;border-left: 1px solid #d8d8d8;padding-left: 20px;"><i class="fa fa-cloud-download"></i> Export Data :</li>
							 <li><a style="padding:5px;"><img src="Pics/Excel-icon.png" class="_studExcelExprtBtn" style="cursor:pointer;width: 40px;margin-left: 20px;" data-placement="top" data-toggle="tooltip" title="Export student details in excel"  ></a></li>
							 <li><a style="padding:5px;"><img src="Pics/Adobe_Reader_PDF.png"" class="_studPdfExprtBtn" style="cursor:pointer;width: 40px;margin-left: 20px;" data-placement="top" data-toggle="tooltip" title="Export student details in PDF"  ></a></li> 
						 </ul>
						  <div class="tab-content">
							<div id="home" class="tab-pane fade in active">
								<%--<h5>General Details</h5>--%>
								<hr>
								 <div class="_stdntGenDt"></div>
								<div class="row listItem"><div class="col-md-6">Name </div><div class="_stdntNm col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Gender  </div><div class="_stdntSex col-md-6 stdDetilsTxt "></div></div>
								<div class="row listItem"><div class="col-md-6">D.O.B</div><div class="_stdntDOB col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Age</div><div class="_stdntAge col-md-6 stdDetilsTxt"></div></div>
								 <div class="row listItem"><div class="col-md-6">Father/Guardian  </div><div class="_stdntFthrNm col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Standard  </div><div class="_stdntStd col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Class</div><div class="_stdntCls col-md-6 stdDetilsTxt"></div></div>
								
								<div class="row listItem"><div class="col-md-6">Religion  </div><div class="_stdntRlgn col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Caste</div><div class="_stdntCaste col-md-6 stdDetilsTxt"></div></div>
								<%--<div class="row listItem"><div class="col-md-6">Caste Category</div><div class="_stdntCasteCat col-md-6 stdDetilsTxt"></div></div>--%>
								<div class="row listItem"><div class="col-md-6">Blood Group</div><div class="_stdntBldGrp col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Social ID(Aadhar Number)</div><div class="_stdntSocialId col-md-6 stdDetilsTxt"></div></div>
								
								<br>
								<div class="well">
									<div class="_clsTimeLine"></div>
								</div>
							  
							</div>
							<div id="menu3" class="tab-pane fade">
								<hr>
								<div class="row listItem"><div class="col-md-6">Admission No  </div><div class="_stdntAdmNo col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Student Id</div><div class="_stdntRefId col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Current Standard  </div><div class="_stdntStd col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Current Class  </div><div class="_stdntCls col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Current Roll number  </div><div class="_stdntRollNo col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Current Batch  </div><div class="stdntBtch col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Joining Batch  </div><div class="_stdntFrstBtch col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Joining Standard</div><div class="_stdntFrstStd col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Joining Class</div><div class="_stdntFrstCls col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Date of Admission </div><div class="_stdntDOJ col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Mother Tongue</div><div class="_stdntMthrTongue col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">1st Language take</div><div class="_stdntFrstLng col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Student Category</div><div class="_stdntCatg col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Religion  </div><div class="_stdntRlgn col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Caste</div><div class="_stdntCaste col-md-6 stdDetilsTxt"></div></div>
								<%--<div class="row listItem"><div class="col-md-6">Caste Category</div><div class="_stdntCasteCat col-md-6 stdDetilsTxt"></div></div>--%>
								<div class="row listItem"><div class="col-md-6">Using School Bus </div><div class="_stdntUseBus col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Using Hostel  </div><div class="_stdntUseHstl col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Created By</div><div class="_stdntCreatedBy col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Created On</div><div class="_stdntCreatedAt col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Date of Leaving</div><div class="_stdntDOL col-md-6 stdDetilsTxt"></div></div>

							</div>
							<div id="menu1" class="tab-pane fade">
								<hr>
								<div class="row listItem"><div class="col-md-6">Residence Phone Number</div><div class="_stdntResPhNo col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Mobile Number</div><div class="_stdntMobNo col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Secondary Mobile Number</div><div class="_stdntAltPhNo col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Email </div><div class="_stdntEMail col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Address(Permanent)</div><div class="_stdntAdrsPer col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Address (Present)</div><div class="_stdntAdrsComm col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Location  </div><div class="_stdntLoc col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">State</div><div class="_stdntState col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Pin Code</div><div class="_stdntPIN col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Nationality  </div><div class="_stdntNation col-md-6 stdDetilsTxt"></div></div>
							</div>
							<div id="menu2" class="tab-pane fade">
								<hr>
								<div class="row listItem"><div class="col-md-6">Father's Name</div><div class="_stdntFthrNm col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Father's Educational Qualification</div><div class="_stdntFthrEduQ col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Father's Occupation</div><div class="_stdntFthrOcc col-md-6 stdDetilsTxt"></div></div> 
								<div class="row listItem"><div class="col-md-6">Mother's Name</div><div class="_stdntMthrNm col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Mother's Educational  Qualification  </div><div class="_stdntMthrEduQ col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Mother's Occupation </div><div class="_stdntMthrOcc col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Annual Income  </div><div class="_stdntFamIncom col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Number of Brothers</div><div class="_stdntBroCount col-md-6 stdDetilsTxt"></div></div>
								<div class="row listItem"><div class="col-md-6">Number of Sisters</div><div class="_stdntSisCount col-md-6 stdDetilsTxt"></div></div>
								 <div class="row listItem"><div class="col-md-12 text-center">
									 <div class="_stdntSiblDt stdDetilsTxt"></div>
								  </div></div>
							</div>
							<div id="menu4" class="tab-pane fade">
								<hr>
							  <div class="_extraField" style="min-height: 40vh;"><p>No any extra details available</p></div>
							  <%--<p>Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.</p>--%>
							</div>
							<div id="menu5" class="tab-pane fade">
								<hr>
							 <div class="_incdentDt" style="min-height: 40vh;">No Incidence faound</div>
							</div>
							 
						  </div>
					</div>
				</div>
				
			</div>
			<div class="col-lg-2 col-md-2">
				<div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
			</div>		
	</div>
	
	<%--<hr><hr>
	 <div id="StudentTopStrip" runat="server"> 
						  
							 <div id="winschoolStudentStrip" style="margin-left:20px;">
					   <table class="NewStudentStrip" width="100%"><tr>
					   <td class="left1"></td>
					   <td class="middle1" >
					   <table>
					   <tr>
					   <td>
						   <img alt="" src="images/img.png" width="82px" height="76px" />
					   </td>
					   <td>
					   </td>
					   <td>
					   <table width="500">
					   <tr>
					   <td class="attributeValue">Name</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   Arun Sunny</td>

					   <td class="attributeValue">Contact No</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   7411413619</td>
					   
					   <td></td>
					   </tr>
					  
					 <tr>
					   <td class="attributeValue">Class</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   BDS</td>
					   
					   <td class="attributeValue">Admission No</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   100</td>
					   
					   <td></td>
					   </tr>
					   <tr>
					   <td class="attributeValue">Class No</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   100</td>
					   
					   <td class="attributeValue">Age</td>
					   <td></td>
					   <td>:</td>
					   <td></td>
					   <td class="DBvalue">
						   22</td>
					   </tr>
					   
					   </table>
					   </td>
					   </tr>
					   
					   
						</table>
						</td>
						   
							   <td class="right1">
							   </td>
						   
						   </tr></table>
							
					</div>
					
	</div>			  
	
	 <div  class="card-1" style="background-color:white" >
		<table class="containerTable" style="width:100%;">
			<tr >
				<td class="no"> </td>
				<td class="n">Student Details</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >                                 
					   <asp:Panel ID="Panel1" runat="server">
				  

						  
						   <asp:Panel ID="Pnl_userdetailstabarea" runat="server">
						   <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"
						CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0" Font-Bold="True">
				<ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Promotion" >
				<HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/user4.png" /><b>GENERAL</b></HeaderTemplate>         
				

		   <ContentTemplate> 
				   
		<asp:Panel ID="Pnl_basicDetails" runat="server" >
				  
					
						<table class="tablelist">
							<tr>
								<td>
									&nbsp;</td>
								<td align="right" > 
									<asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
									Width="35px" Height="35px" onclick="Img_Excel_Click" 
										ToolTip="Export to Excel"  /></td>
							</tr>
							<tr><td colspan="2">  <div class="linestyle"></div></td></tr>
							<tr>
								<td class="leftside">
									Student Name:</td>
								<td>
									<asp:Label ID="Lbl_Name_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Sex:</td>
								<td>
									<asp:Label ID="Lbl_sex_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									D.O.B:</td>
								<td>
									<asp:Label ID="Lbl_dob_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Father/Guardian Name:</td>
								<td>
									<asp:Label ID="Lbl_father_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Standard:</span></td>
								<td>
									<asp:Label ID="Lbl_std_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Class:
									   </td>
								<td>
									<asp:Label ID="Lbl_class_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside" style="vertical-align:top">
									Address(Permanent):</td>
								<td>
									<asp:TextBox ID="Txt_Address" runat="server"  ReadOnly="True" ForeColor="Black"
									TextMode="MultiLine" Width="250px" BorderStyle="None" Font-Bold="True"
										Height="70px"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Joining Batch:</td>
								<td>
									<asp:Label ID="Lbl_joinbatch_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							 <tr>
								<td class="leftside">
									Joining Standard:</td>
								<td>
									<asp:Label ID="lbl_joinstandard" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Date of Admission:</td>
								<td>
									<asp:Label ID="Lbl_doa_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Religion:</td>
								<td>
									<asp:Label ID="Lbl_religion_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Caste:</td>
								<td>
									<asp:Label ID="Lbl_cast_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="leftside">
									Admission No:</td>
								<td>
									<asp:Label ID="lbl_AdmissioinNo_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
							
							  <tr>
								<td class="leftside">
									Student Id:</td>
								<td>
									<asp:Label ID="Lbl_StudentId" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
						   
							<tr>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					<div class="linestyle">                  
					</div>
					</asp:Panel>
				   
				   
</ContentTemplate>  
				

</ajaxToolkit:TabPanel>
				
				<ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Promotion"  >
				<HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>OTHERS</b></HeaderTemplate>                 
		   <ContentTemplate>

			   <asp:Panel ID="Pnl_otherdetails" runat="server">
			   <br/> 
				<div class="newsubheading">
					Personal details
					</div>
				 <div class="linestyle">                  
					</div>
					<table class="tablelist">
						<tr>
							<td class="leftside">
								&nbsp;</td>
							<td class="rightside">
								&nbsp;</td>
						</tr>
						 <tr>
							<td class="leftside">
								Using School Bus:</td>
							<td class="rightside">
								<asp:Label ID="lbl_CollegeBus" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						 <tr>
							<td class="leftside">
								Using Hostel:</td>
							<td class="rightside">
								<asp:Label ID="lbl_Hostel" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						
						<tr>
							<td class="leftside">
								Blood Group:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_blodgroup_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						 <tr>
								<td class="leftside">
									Aadhar Number:</td>
								<td>
									<asp:Label ID="Lbl_Aadharno" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
								</td>
							</tr>
						<tr>
							<td class="leftside">
								Nationality:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_nat_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Mother Tongue:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_mot_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Mother&#39;s Name:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_mothernane_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Father&#39;s Educational Qualification:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_fatherqlif_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Mother&#39;s Educational &nbsp;Qualification:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_motherqlfi_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Father&#39;s Occupation:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_fatherocc_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Mother&#39;s Occupation:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_motherocc_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Annual Income:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_annualincom_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside" style="vertical-align:top">
								Address (Present):</td>
							<td class="rightside">
								<asp:TextBox ID="Txt_addresspresent_ot" runat="server" BorderStyle="None" 
									Font-Bold="True" ForeColor="Black" Height="70px" TextMode="MultiLine" ReadOnly="True"
									Width="254px"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Location:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_location_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								State:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_state_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Pin Code:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_pin_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Residence Phone Number:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_resdphn_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Mobile Number:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_mob_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Secondary Mobile Number:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_mob_second_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Email :</td>
							<td class="rightside">
								<asp:Label ID="Lbl_email_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Number of Brothers:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_nofobrot_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Number of Sisters:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_noofsist_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								1st Language Wishes to take:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_firstlng_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								Student Category:</td>
							<td class="rightside">
								<asp:Label ID="Lbl_studcat_ot" runat="server" Font-Bold="True" 
									ForeColor="Black"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="leftside">
								&nbsp;</td>
							<td class="rightside">
								&nbsp;</td>
						</tr>
					</table>
			   <asp:Panel ID="Pnl_SiblingsDetails" runat="server">
			   <center>
					   <table  width="200px">                       
						   <tr>
						   <td align="left">
						   <asp:Label ID="Lbl_Sib" runat="server"></asp:Label>
						   </td>
						   </tr>
							<tr>
							<td  align="center">
							<asp:Panel ID="Pnl_SibDisplay" runat="server">                            
							   <asp:GridView ID="GrdSiblings" runat="server" AutoGenerateColumns="False"
						   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4"  OnRowCommand="GrdSiblings_RowCommand" 
						   ForeColor="Black" GridLines="Vertical" Width="500px" >
						   
						   <Columns>
								<asp:BoundField DataField="Id" HeaderText="Id" /> 
								<asp:BoundField DataField="StudentName" HeaderText="Name" /> 
								<asp:BoundField DataField="GardianName" HeaderText="Guardian Name" />
								<asp:ButtonField CommandName="View" 
									Text="&lt;img src='Pics/Details.png' width='30px' border=0 title='View' &gt;">
												<ItemStyle Width="30px" />
											</asp:ButtonField>
							</Columns>

						  
					   </asp:GridView>
					   <br />
					   </asp:Panel>
							</td>
							</tr>
						  
					   </table>
					   </center>
					   </asp:Panel>
				<div class="linestyle">                  
					</div>
					<asp:Panel ID="Pnl_custumarea" runat="server">
					
			   <div class="newsubheading">
					Extra details
					</div>
				
				<asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
				 <div class="linestyle">  </div> 
								
					</asp:Panel>   
			   
			   </asp:Panel>

</ContentTemplate>     

	 </ajaxToolkit:TabPanel>
				
				<ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Promotion"  >
				<HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/comments.png" /><b>INCIDENCE</b></HeaderTemplate>                 
					<ContentTemplate>
					 <div id= "TopTab" runat ="server">
							 
					  </div>
					</ContentTemplate>
				 </ajaxToolkit:TabPanel>
				</ajaxToolkit:tabcontainer>
						   
						   
			   </asp:Panel>
					
						  
		
					</asp:Panel>
						
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%>
	<%--	<asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
								  runat="server" CancelControlID="Btn_magok" 
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
						  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
						 <div class="container skin5" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> </td>
			<td class="n"><span style="color:White">alert!</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			   
				<asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
						</div>
			</td>
			<td class="e"> </td>
		</tr>
		<tr>
			<td class="so"> </td>
			<td class="s"> </td>
			<td class="se"> </td>
		</tr>
	</table>
	<br /><br />										   				   
</div>
</asp:Panel> 
	   
		
	 
	 --%>
	 
				
 

<%--<div class="col-lg-2 col-md-2 col-xs-12">


<div class="label">Student Info</div>

</div>
<div class="clear"></div>--%>
<div id="javascriptId" runat="server" hidden></div>
</asp:Content>

