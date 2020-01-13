<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailConfiguration.aspx.cs" Inherits="WinEr.EmailConfiguration" %>
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

                <td class="no"><img alt="" src="Pics/mail1.png" height="35" width="35" />  </td>
                <td class="n">Email Config</td>
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
                <td align="left"><asp:DropDownList ID="Drp_Type" runat="server" class="form-control" Width="180px" 
                        AutoPostBack="True" onselectedindexchanged="Drp_Type_SelectedIndexChanged"></asp:DropDownList></td>
                         <td align="right">Replacements
                                 
                               </td>
                               <td >
                                   <asp:DropDownList ID="Drp_Replacements" runat="server" class="form-control" Width="180px"  
                                       AutoPostBack="true" onselectedindexchanged="Drp_Replacements_SelectedIndexChanged"
                                      ><%-- onselectedindexchanged="Drp_Replacements_SelectedIndexChanged"--%>    </asp:DropDownList>
                                      </td>
                <td align="left"><asp:CheckBox  ID="Chk_Enable" runat="server" Text="Enable"/></td>
                 <td align="left"><asp:Button  ID="Btn_Update" runat="server" Text="Update" 
                         Class="btn btn-info" onclick="Btn_Update_Click"/></td>
                </tr>
                  <tr>
                <td align="right"></td>
                <td align="left">
                <asp:Label ID="Label1" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                       </td>
                         <td align="right">
                         
                               </td>
                               <td  align="left">
                                   <asp:Label ID="Lbl_replacement" runat="server" class="control-label" Text=""  ToolTip="Copy Content" ></asp:Label>
                                      </td>
                <td align="left" width="100px"> <asp:CheckBox ID="Chk_ScheduleTime" runat="server" Text="Schedule Enable" AutoPostBack="true"
                                   oncheckedchanged="Chk_ScheduleTime_CheckedChanged" /></td>
                 <td width="200px">
                 <div class="form-inline">
                  <asp:DropDownList ID="Drp_Schedulehour" class="form-control" runat="server">
                                <asp:ListItem Text="HH" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                               </asp:DropDownList>   
                               <asp:DropDownList ID="Drp_ScheduleMinute" class="form-control" runat="server">
                                <asp:ListItem Text="MM" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                               </asp:DropDownList>
                               </div>
                                 </td>
                
              
                </tr>
                <tr>
                <td align="right"></td>
                <td align="left">
                <asp:Label ID="Lbl_Msg" runat="server" class="contol-label" ForeColor="Red"></asp:Label>
            
                       </td>
                         <td align="right">
                                 
                               </td>
                               <td >
                                  
                                      </td>
                <td align="left"></td>
                 <td align="left"></td>
                
              
                </tr>
                </table>
                </center>
                </asp:Panel>
                <br />
                 <asp:Panel ID="Panel_Email_Details" runat="server">
		              <table width="100%">
		              <tr>
		              <td style="font-size:small;font-weight:bold;color:Black"><b>Subject:</b>	
                     <asp:TextBox ID="Txt_EmailSubject" runat="server" Width="850px"  class="form-control" ></asp:TextBox>
                     </td></tr>
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
    
        </asp:UpdatePanel> 
        
        </div>



</asp:Content>
