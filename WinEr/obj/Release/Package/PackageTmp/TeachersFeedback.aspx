<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="TeachersFeedback.aspx.cs" Inherits="WinEr.TeachersFeedback" %>
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
				        <td class="n" style="width: 954px">Feedback</td>
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
				                            <asp:DropDownList ID="drpClass" class="form-control" runat="server" Width="203px" AutoPostBack="true" 
                                                onselectedindexchanged="drpClass_SelectedIndexChanged">
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
				                            <asp:DropDownList ID="drp_Student" class="form-control" runat="server" Width="203px">
                                            </asp:DropDownList>
                                        </td>
				                    </tr> 
				                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				                     <tr>
				                        <td class="leftside">Select Period</td>
				                        <td class="rightside"> 
				                            <asp:DropDownList ID="drp_period" class="form-control" runat="server" Width="203px">
                                            </asp:DropDownList>
                                        </td>
				                    </tr> 
				                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				                    <tr>
				                        <td colspan="2" align="center"><asp:Button ID="btnSelect" class="btn btn-primary" runat="server" 
                                                Text="Select" onclick="btnSelect_Click"    /></td>				                    
				                    </tr>
				                    
				                    <tr>
				                        <td colspan="2" align="center"><asp:Label ID="lblMessage" runat="server" Text=""  ForeColor="Red"/></td>				                    
				                    </tr>
				                </table>
				                <hr /><br />
				               <div  runat="server" id="userfeedback" style="padding:10px;border:#404040 2px solid"  visible="false">
				               </div><br />
				                <asp:PlaceHolder runat="server" ID="plchfeedbackarea" />
				                   
                        
                              <br />
                              
                              <table width="100%"> 
                                <tr>
                                    <td align="center" ><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                                            onclick="btnSubmit_Click"  class="btn btn-info"  />  
                                        <asp:HiddenField ID="hdn_MasterId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_NeedApproval" runat="server" Value="0" />
                                            </td>
                                </tr> 
                              </table>
                              
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
