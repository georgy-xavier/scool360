<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ScheduleTranportationFee.aspx.cs" Inherits="WinEr.ScheduleTranportationFee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_ScrechStud.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
     }
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

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

 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate>
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="30px" Width="30px" />  </td>
                <td class="n">Schedule Transportation Fee</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:200px">
                <table width="100%">
                <tr>
                <td align="center"><asp:Label ID="Lbl_msg" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                </table>
                <asp:Panel ID="Pnl_ScheduleFee" runat="server">   
                <center>
                <table width="500px">
              
                <tr>  
                <td align="right">Select Location:</td>
                <td align="left"><asp:DropDownList ID="Drp_Location" runat="server" Width="160px" class="form-control"
                        AutoPostBack="True" 
                        onselectedindexchanged="Drp_Location_SelectedIndexChanged"></asp:DropDownList></td>                
                <td>Select Period:</td>
                <td><asp:DropDownList ID="Drp_Period" runat="server" Width="160px" class="form-control"
                        AutoPostBack="True" onselectedindexchanged="Drp_Period_SelectedIndexChanged"></asp:DropDownList></td>
                        
                </tr>  
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
  
                <tr>  
                <td align="right">Due Date:</td>
                <td align="left"><asp:TextBox ID="Txt_DueDate" runat="server" Width="160px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                 CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_DueDate" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
                                 </td>                
                <td align="right">Last Date:</td>
                <td align="left"><asp:TextBox ID="Txt_LastDate" runat="server" Width="160px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                 CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_LastDate" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
                                 </td>
                </tr> 
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                    
                   <tr> 
                   <td align="right">Class</td>
                   <td align="right"><asp:DropDownList ID="Drp_Class" runat="server" Width="160px" class="form-control"
                           AutoPostBack="True" onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td>
                <td align="right">Batch:</td>
                 <td align="left">
                                  <asp:RadioButtonList ID="Rdo_Batch1" runat="server" AutoPostBack="true" 
                                      RepeatDirection="Horizontal" 
                                      onselectedindexchanged="Rdo_Batch1_SelectedIndexChanged">
                                      <asp:ListItem Selected="True" Text="Current" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1"></asp:ListItem>
                                  </asp:RadioButtonList>
                              </td></tr>   
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
           
                </table>
                </center>             
               <br />
                 <div class="linestyle"></div>
                <table width="100%"><tr><td align="center"><asp:Label ID="Lbl_Error" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
                
                 <asp:Panel ID="Pnl_DisplayDetails" runat="server">
                  <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		
		<div style=" overflow:auto; max-height: 400px;">
                        <asp:GridView ID="Grd_ScrechStud" runat="server" AutoGenerateColumns="False" 
                             Width="97%">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Destination Id" />
                                <asp:BoundField DataField="classid" HeaderText="classid" />
                                <asp:BoundField DataField="StudentId" HeaderText="StudentId" />
                                <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server" />
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField> 
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                <asp:BoundField DataField="Sex" HeaderText="Sex" />                                
                                <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_NewAmount" runat="server"  MaxLength="10" Text="" class="form-control"
                                            Width="75"></asp:TextBox>

                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewAmound_FilteredTextBoxExtender" 
                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_NewAmount">
                                        </ajaxToolkit:FilteredTextBoxExtender>

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
                        </asp:GridView>
                    </div>
		<br />
		<div style=" overflow:auto; max-height: 400px; text-align:center;">
		<asp:Button ID="Btn_Schedule" runat="server" Text="Schedule" class="btn btn-primary"
                onclick="Btn_Schedule_Click" />
		</div>
          </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	          
                 </asp:Panel>
                </asp:Panel>
               
                </div>
                
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
<WC:MSGBOX id="WC_MessageBox" runat="server" />   


<asp:Button runat="server" ID="Btn_Salereport" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_SALEREPORT"   runat="server"  PopupControlID="Pnl_SaleReport" CancelControlID="Btn_cancel" TargetControlID="Btn_Salereport"  />
     <asp:Panel ID="Pnl_SaleReport" runat="server" style="display:none"><%--style="display:none"--%>
 <div id="Div2" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Reminder</td>
            <td class="ne">&nbsp;</td>
       </tr>
       <tr >
            <td class="o"> </td>
            <td class="c" >
            <div id="Div_salereport" runat="server">
            
           <%--<table width="100%">
           <tr><td style="color:Red">sgdfhdjgh</td></tr>
            
            </table>--%>
            
            </div>
            <table width="100%"><tr><td align="center"> 
            <asp:Button ID="Btn_SalereportCancel" runat="server" Class="btn btn-success" Text="Ok" 
                    onclick="Btn_SalereportCancel_Click" />
                 <asp:Button ID="Btn_cancel" runat="server" Class="btn btn-danger" Text="cancel" />
            </td></tr></table>
           
            
                     </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel> 
  </ContentTemplate>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
