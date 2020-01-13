<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ViewPaySlip.aspx.cs" Inherits="WinEr.ViewPaySlip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function PrintDiv() {

            var divContentsTable = document.getElementById("payslipdiv").innerHTML;
            var printWindow = window.open('', '', '', 'resizable=no');
            printWindow.document.write('<html><head></style><title>Fee Bill</title> <script type="text/javascript">function FinalPrint() { document.getElementById("printButton").style.visibility="hidden"; window.print(); this.window.close();} </scrip' + "t" + '><link href="css_bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" /><link href="~/Styles/Site.css" rel="stylesheet" type="text/css" /> <link rel="stylesheet" type="text/css" href="css files/whitetheme.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/campusstyle.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/winroundbox.css" title="style"  media="screen"/><link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/><link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen" />');
            printWindow.document.write('</head><body style="border-style: solid; border-width: 1px;"><div style="margin:20px;"><br />');
            printWindow.document.write('<p style="text-decoration: underline;text-align: left;font-size: 15px;"><strong>Payslip</strong></p>' + divContentsTable);//student table content
            printWindow.document.write('<br /><br /><button style="height: 40px; width:120px; margin:auto;display:block" id="printButton" onclick="FinalPrint()">Print This Report</button></input></div></body></html>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />


<div id="contents">
<div id="right">

<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>
</div>

<div id="left">

     <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
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
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
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

<div id="div_NotHaveRight" runat="server">
       <div id="Div1">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table width="100%">
                       <tr>
                       <td align="center">
                       <asp:Label ID="Lbl_notHavrgt" runat="server" ForeColor="Red" Text="You do not have the right to see this payslip"></asp:Label> </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                       
</div>

<div id="Div_havrgt" runat="server">

<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" >
			<tr >
				<td class="no"></td>
				<td class="n">View PaySlip</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
					<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
                    <asp:Panel ID="Pnl_ViewPayslip" runat="server">
                    <center>
                    <table width="400px" class="tablelist">
                    <tr>
                    <td class="leftside ">year</td>
                    <td class="rightside"><asp:DropDownList ID="Drp_Year" runat="server" Width="160px" class="form-control"
                            AutoPostBack="True" onselectedindexchanged="Drp_Year_SelectedIndexChanged"></asp:DropDownList></td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    
                    <tr>
                    <td class="leftside ">Month</td>
                    <td class="rightside"><asp:DropDownList ID="Drp_Month" runat="server" Width="160px" class="form-control"></asp:DropDownList></td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td></td>
                    <td><asp:Button ID="Btn_View" runat="server" Text="VIEW"  
                            class="btn btn-primary" onclick="Btn_View_Click"/></td></tr>
                            <tr><td colspan="2" align="center"><asp:Label ID="Lbl_payslipErr" runat="server" ForeColor="Red"></asp:Label></td></tr>
                    </table>
                    </center>
                    </asp:Panel>
                         <asp:Button ID="btn_pdf" runat="server" Enabled="true" Class="btn btn-primary"    
                Text="Export to PDF" OnClientClick="PrintDiv()" ></asp:Button>
                    <br />
                    <asp:Panel ID="Pnl_View" runat="server">
                        <div id="payslipdiv">
                            <div id="Div_Payslip" runat="server"></div>
                            </div>

                    </asp:Panel>
                    
                     </ContentTemplate>
                     
                    </asp:UpdatePanel>
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div> 
	</div>
</div>
<div class="clear"></div>
</div>
</asp:Content>
