<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GradeExamReport.aspx.cs" Inherits="WinEr.GradeExamReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="container skin1">
		        <table cellpadding="0" cellspacing="0" class="containerTable">
			        <tr >
				        <td class="no"></td>
				        <td class="n" style="width: 954px">Grade Report</td>
				        <td class="ne"> </td>
			        </tr>
			        <tr>
				        <td class="o"> </td>
				        <td class="c" style="width: 954px" >
				        
				            <div>
				            
				                <table class="tablelist">
				                    <tr>
				                        <td class="leftside">Select Class</td>
				                        <td class="rightside"> 
				                            <asp:DropDownList ID="drpClass" runat="server" Width="200px" class="form-control" AutoPostBack="true" 
                                                onselectedindexchanged="drpClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
				                    </tr>
				                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





				                     <tr>
                                        <td class="leftside">
                                            Select Exam Type
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_ExamType" runat="server" Width="200px" class="form-control"
                                                AutoPostBack="True" onselectedindexchanged="Drp_ExamType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





				                      <tr>
				                        <td class="leftside">Select Exam</td>
				                        <td class="rightside"> 
				                            <asp:DropDownList ID="Drp_Exam" runat="server" Width="200px" class="form-control">
                                            </asp:DropDownList>
                                        </td>
				                    </tr> 
				                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





				                    
				                    <tr>
				                        <td class="leftside">Select Student</td>
				                        <td class="rightside"> 
				                            <asp:DropDownList ID="drp_Student" runat="server" Width="200px" class="form-control">
                                            </asp:DropDownList>
                                        </td>
				                    </tr> 
				                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





				                    
				                    <tr>
				                        <td colspan="2" align="center"><asp:Button ID="btnSelect" runat="server" class="btn btn-primary"
                                                Text="Select" onclick="btnSelect_Click" /></td>				                    
				                    </tr>
				                    <tr>
				                        <td colspan="2" align="center"><asp:Label ID="lblMessage" runat="server" Text="" class="control-label" ForeColor="Red"/></td>				                    
				                    </tr>
				                </table>
				                <hr /><br />
				               				             
                              
                             
                              
				            </div>
				        
				        </td>				        
				        <td class="e"> </td>
			        </tr>
			        <tr>
				        <td class="so"> </td>
				        <td class="s"></td>
				        <td class="se"> </td>
			        </tr>
		        </table>
	        </div>
        </ContentTemplate>
    
        
    </asp:UpdatePanel>
    <div class="clear"></div>                    
</div>
</asp:Content>


