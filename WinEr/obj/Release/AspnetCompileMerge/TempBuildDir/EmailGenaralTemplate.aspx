<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailGenaralTemplate.aspx.cs" Inherits="WinEr.EmailGenaralTemplate" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
         
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
                <ProgressTemplate>
                
                
                        <div id="progressBackgroundFilter">

                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
               </asp:UpdateProgress>
         
    <div id="contents">
         

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate>
  
        <div class="container skin1"  >
         <table  cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no"><img alt="" src="Pics/mail1.png" height="35" width="35" /> </td>
                <td class="n">General Email Template</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                    <asp:Panel ID="Pnl_Initial" runat="server">
                <center>
                <table width="700px">
                <tr>
                <td align="right">Type</td>
                <td align="left"><asp:DropDownList ID="Drp_Type" runat="server" Width="180px" 
                        AutoPostBack="True" class="form-control"
                        onselectedindexchanged="Drp_Type_SelectedIndexChanged" ></asp:DropDownList><%--onselectedindexchanged="Drp_Type_SelectedIndexChanged"--%></td>
                         <td align="right">Replacements
                                 
                               </td>
                               <td >
                                   <asp:DropDownList ID="Drp_Replacements" runat="server" Width="180px"  class="form-control"
                                       AutoPostBack="true" onselectedindexchanged="Drp_Replacements_SelectedIndexChanged" 
                                      ><%-- onselectedindexchanged="Drp_Replacements_SelectedIndexChanged"--%>    </asp:DropDownList>
                                      </td>
                <td align="left"><asp:CheckBox  ID="Chk_Enable" runat="server" Text="Enable"/></td>
                 <td align="left">
                     <asp:Button  ID="Btn_Update" runat="server" Text="Update" 
                         Class="btn btn-info" onclick="Btn_Update_Click" /></td>
                </tr>
                  <tr>
                <td align="right"></td>
                <td align="left">
                <asp:Label ID="Label1" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                       </td>
                         <td align="right">
                         
                               </td>
                               <td  align="left">
                                   <asp:Label ID="Lbl_replacement" runat="server" Text="" class="control-label"  ToolTip="Copy Content" ></asp:Label>
                                      </td>
                <td align="left"></td>
                 <td align="left"></td>
                
              
                </tr>
                 <tr>
                 <td colspan="6" align="right""><asp:LinkButton ID="Lnl_NewTemplate" runat="server" 
                         Text="Add New Template" onclick="Lnl_NewTemplate_Click"></asp:LinkButton></td>
                  </tr>    
                <tr>
                
                <td colspan="6" align="center">   <asp:Label ID="Lbl_Msg" runat="server"  class="control-label" ForeColor="Red"></asp:Label>                               
                 
                
                </td>              
                </tr>
                </table>
                </center>
                </asp:Panel>
                
                  <br />
                 <asp:Panel ID="Panel_Email_Details" runat="server">                 
                 <center>
                 <div id="Div_Seperatos" runat="server">
                <%-- <table>
                 <tr>
                 <td>Student Name:($studen$)
                 </td>
                 <td>Parent Name:($Parent$)
                 </td>
                 <td>Staff Name:($Staff$)
                 </td>
                 </tr>
                 </table>--%>
                 </div>
                 </center>
		              <table>
		              
		               <tr id="RowSave" runat="server">
		              <td  align="right"><asp:Button ID="Btn_SaveTemplate" runat="server" Text="Save" 
                              Class="btn btn-info" onclick="Btn_SaveTemplate_Click" />
                              <asp:Button ID="Button1" runat="server" Text="Cancel" 
                              Class="btn btn-info" onclick="Button1_Click" />
                     </td></tr>
                     <tr>
                     <td align="center" >
                     <asp:Label ID="Lbl_TemplateSave" runat="server" class="control-label" ForeColor="Red"> </asp:Label>
                     </td>
                     </tr>
		              <tr>
		              
		              <td style="font-size:small;font-weight:bold;color:Black">
		              <div class="form-inline">
		              <b>Subject:</b>	
                     <asp:TextBox ID="Txt_EmailSubject" runat="server" Width="850px" class="form-control" ></asp:TextBox>
                     </div>
                     </td>
                     
                     </tr>
		              </table>
                         
                <div class="roundbox" >
		              <table width="100%">
		               <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		               <tr><td class="centerleft"></td><td class="centermiddle">
		
		               <h5>Email Body</h5>
                        <div class="linestyleNew"> </div>
                         <br />
                           <HTMLEditor:Editor ID="Editor_Body" runat="server" Height="300px" 
                                            Width="100%" />
                                            
                                            
                      </td><td class="centerright"></td></tr>
		              <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		              </table>
		
		
		            </div>
		    
		    </asp:Panel>   
                
                 </td>
                         <td class="e"></td>
                         </tr>
                    
                    <tr>
                        <td class="so">
                        </td>
                        <td class="s">
                        </td>
                        <td class="se">
                        </td>
             </tr>
                    
                    </table>
             
     </div>
     
      
      <div class="clear"></div>
     
     
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
        
     </ContentTemplate>      
     <%--<Triggers>
               <asp:PostBackTrigger ControlID="Img_Export"/>
      </Triggers>--%> 
                 
        </asp:UpdatePanel> 
</div>
</asp:Content>
